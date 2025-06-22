using FutFut.Common;
using FutFut.Identity.Contracts;
using FutFut.Songs.Service.Data.Entities;
using MassTransit;

namespace FutFut.Songs.Service.Consumers;

public class UserDeletedConsumer(IRepository<ArtistEntity> artistRepository) : IConsumer<UserDeleted>
{
    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var existedProfile = await artistRepository.GetAsync(p => p.Id == context.Message.Id);

        if (existedProfile is null)
        {
            return;
        }
        
        await artistRepository.DeleteAsync(existedProfile.Id);
    }
}