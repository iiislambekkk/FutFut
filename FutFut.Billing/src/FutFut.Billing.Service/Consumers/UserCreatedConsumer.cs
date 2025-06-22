using FutFut.Billing.Service.Data.Entities;
using FutFut.Common;
using FutFut.Identity.Contracts;
using MassTransit;

namespace FutFut.Billing.Service.Consumers;

public class UserCreatedConsumer(IRepository<UserEntity> profileRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var existedUser = await profileRepository.GetAsync(p => p.Id == context.Message.Id);

        if (existedUser is not null)
        {
            return;
        }
        
        var profile = new UserEntity() {Id = context.Message.Id, Email = context.Message.Email};
        
        await profileRepository.CreateAsync(profile);
    }
}