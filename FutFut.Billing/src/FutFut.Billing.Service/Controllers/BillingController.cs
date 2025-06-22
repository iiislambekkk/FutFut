using System.Security.Claims;
using FutFut.Billing.Service.Data.Entities;
using FutFut.Common;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Grpc;
using Stripe;
using Stripe.Checkout;

namespace FutFut.Billing.Service.Controllers;

[ApiController]
[Route("billing")]
public class BillingController(IRepository<UserEntity> userRepository) : ControllerBase
{
    [Authorize]
    [HttpGet("{priceId}")]
    public async Task<ActionResult<string>> GetStripeId(string priceId)
    {
        Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId);
        
        var userEntity = await userRepository.GetAsync(userId);

        if (userEntity == null)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new User.UserClient(channel);

            var user = await client.GetUserAsync(new GetUserRequest() { UserId = userId.ToString() });
            if (user == null) return NotFound("User not found");

            userEntity = new UserEntity() { Id = Guid.Parse(user.UserId), Email = user.Email };
            await userRepository.CreateAsync(userEntity);
        }

        if (string.IsNullOrEmpty(userEntity.StripeCustomerId))
        {
            
            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = userEntity.Email,
            });
        
            userEntity.StripeCustomerId = customer.Id;
            await userRepository.UpdateAsync(userEntity);
        }
        
        var subscriptionService = new SubscriptionService();
        
        var existingActive = await subscriptionService.ListAsync(new SubscriptionListOptions
        {
            Customer = userEntity.StripeCustomerId,
            Status = SubscriptionStatuses.Active,
            Limit = 1
        });

        if (existingActive.Any())
        {
            return Ok();
        }
        
        var subscriptions = await subscriptionService.ListAsync(new SubscriptionListOptions
        {
            Customer = userEntity.StripeCustomerId,
            Status = SubscriptionStatuses.Incomplete,
            Limit = 1
        });

        var existing = subscriptions.FirstOrDefault();
        if (existing != null)
        {
            var invoiceService = new InvoiceService();
            var invoice = await invoiceService.GetAsync(existing.LatestInvoiceId, new InvoiceGetOptions
            {
                Expand = new List<string> { "payment_intent" }
            });
            
            return Ok(invoice.LatestRevision.ConfirmationSecret);
        }
        
        var subscription = await subscriptionService.CreateAsync(new SubscriptionCreateOptions
        {
            Customer = userEntity.StripeCustomerId,
            Items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions()
                {
                    Price = priceId
                },
            },
            PaymentBehavior = "default_incomplete",
            Expand = new List<string> { "latest_invoice.confirmation_secret" }
        });
        
        Console.WriteLine($"Subscription created: {subscription.LatestInvoice.ConfirmationSecret}");
        
        var clientSecret = subscription.LatestInvoice.ConfirmationSecret;
        
        return Ok(clientSecret);
    }
    
    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        const string endpointSecret = "whsec_4c8da1b7793150d419244db2805a8ba07f8f38d734d5f1f8ef7ab54fb70e15a4";
        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            stripeEvent = EventUtility.ConstructEvent(json,
                signatureHeader, endpointSecret);

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                Console.WriteLine(paymentIntent.Customer);
                Console.ResetColor();
            }
            else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
            {
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                // Then define and call a method to handle the successful attachment of a PaymentMethod.
                // handlePaymentMethodAttached(paymentMethod);
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }
}