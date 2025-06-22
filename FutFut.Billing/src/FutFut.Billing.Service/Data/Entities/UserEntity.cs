using FutFut.Common;

namespace FutFut.Billing.Service.Data.Entities;

public class UserEntity : IEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = String.Empty;
    public string? StripeCustomerId { get; set; }
}
