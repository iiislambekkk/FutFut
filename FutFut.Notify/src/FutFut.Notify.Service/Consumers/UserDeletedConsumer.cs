using FutFut.Common;
using FutFut.Identity.Contracts;
using FutFut.Notify.Service.Data.Entities;
using MassTransit;

namespace FutFut.Notify.Service.Consumers;

public class UserDeletedConsumer(IRepository<UserEntity> profileRepository) : IConsumer<UserDeleted>
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