using FutFut.Billing.Service.Data;
using FutFut.Billing.Service.Data.Entities;
using FutFut.Common.EfCore;
using FutFut.Common.Identity;
using FutFut.Common.Logging;
using FutFut.Common.MassTransit;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEfCoreDbContext<AppDbContext>()
    .AddEFCoreRepository<UserEntity, AppDbContext>()
    .AddMassTransitWithRabbitMQ()
    .AddJwtBearerAuthentication();

StripeConfiguration.ApiKey =
    "sk_test_51Rac5bCmqDLurcRBVOuNn4FnaoJBZpNopeXwMHBs2bERfwPZYI008BhZDucde224WlCmCCP7WxhYj9qOa7BqVPqe00wO0V9R3C";

builder.AddLoggingWithSeqAndLogstash();

builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(c =>
    {
        c.DocumentPath = "/openapi/v1.json";
    });
    
    app.UseCors(b =>
    {
        b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
