using AutoMapper;
using FutFut.Common;
using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Data.Entities;
using FutFut.Notify.Service.Firebase;
using MassTransit;

namespace FutFut.Notify.Service.Consumers;

public class SendNotificationConsumer(
    FirebaseService firebaseService,
    IMapper mapper,
    IRepository<DeviceEntity> deviceRepo,
    IRepository<NotificationEntity> notificationRepo
) : IConsumer<SendNotification>
{
    public async Task Consume(ConsumeContext<SendNotification> context)
    {
        Console.ForegroundColor = ConsoleColor.Magenta; 
        Console.WriteLine("Consuming started");
        Console.ResetColor();
        
        var notificationEntity = new NotificationEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow,
            UserId = context.Message.UserId,
            Title = context.Message.Title,
            Content = context.Message.Content,
            Payload = context.Message.Payload,
            Type = context.Message.Type
        };
        
        await notificationRepo.CreateAsync(notificationEntity);

        var devicesTokens = (await deviceRepo.GetAllAsync(d => d.UserId == notificationEntity.UserId)).Select(d => d.Token).ToList();
        
        await firebaseService.SendAsync(
            mapper.Map<NotificationDto>(notificationEntity),
            devicesTokens
        );
    }
}