using System.Text.Json;
using FirebaseAdmin.Messaging;

namespace FutFut.Notify.Service.Firebase;

public class FirebaseService
{
    private readonly FirebaseMessaging _firebaseMessaging;

    public FirebaseService(FirebaseMessaging firebaseMessaging)
    {
        _firebaseMessaging = firebaseMessaging;
    }

    public async Task SendAsync(NotificationDto notificationDto, List<string> tokens)
    {
        foreach (var token in tokens)
        {
             
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = notificationDto.Title,
                    Body = notificationDto.Content
                },
                Data = JsonSerializer.Deserialize<Dictionary<string, string>>(notificationDto.Payload)
            };

            var response = await _firebaseMessaging.SendAsync(message);
            Console.WriteLine($"Sent: {response}");
        }
    }
}