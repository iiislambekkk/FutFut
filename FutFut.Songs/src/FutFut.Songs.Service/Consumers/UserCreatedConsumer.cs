using FutFut.Common;
using FutFut.Identity.Contracts;
using FutFut.Songs.Service.Data.Entities;
using MassTransit;

namespace FutFut.Songs.Service.Consumers;

public class UserCreatedConsumer(IRepository<ArtistEntity> artistRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var existedUser = await artistRepository.GetAsync(p => p.Id == context.Message.Id);

        if (existedUser is not null)
        {
            return;
        }
        
        var profile = new ArtistEntity() {Id = context.Message.Id, Name = context.Message.Email};
        
        await artistRepository.CreateAsync(profile);
    }
}