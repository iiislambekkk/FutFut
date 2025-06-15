using FutFut.Common;
using FutFut.Identity.Contracts;
using FutFut.Profile.Service.Entities;
using MassTransit;

namespace FutFut.Profile.Service.Consumers;

public class UserDeletedConsumer(IRepository<ProfileEntity> profileRepository) : IConsumer<UserDeleted>
{
    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var existedProfile = await profileRepository.GetAsync(p => p.Id == context.Message.Id);

        if (existedProfile is null)
        {
            return;
        }
        
        await profileRepository.DeleteAsync(existedProfile.Id);
    }
}