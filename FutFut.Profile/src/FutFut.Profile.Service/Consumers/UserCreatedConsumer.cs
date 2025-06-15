using FutFut.Common;
using FutFut.Identity.Contracts;
using FutFut.Profile.Service.Entities;
using MassTransit;

namespace FutFut.Profile.Service.Consumers;

public class UserCreatedConsumer(IRepository<ProfileEntity> profileRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var existedUser = await profileRepository.GetAsync(p => p.Id == context.Message.Id);

        if (existedUser is not null)
        {
            return;
        }
        
        var profile = new ProfileEntity() {Id = context.Message.Id, DisplayName = context.Message.Email, Email = context.Message.Email};
        
        await profileRepository.CreateAsync(profile);
    }
}