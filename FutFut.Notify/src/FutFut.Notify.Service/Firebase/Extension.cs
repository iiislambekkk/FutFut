
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace FutFut.Notify.Service.Firebase;

public static class Extension
{
    public static IServiceCollection AddFirebaseMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var googleSettings = configuration.GetSection(nameof(FirebaseSettings)).Get<FirebaseSettings>()!;
        
        var firebaseApp = FirebaseApp.Create(
            new AppOptions()
            {
                Credential = GoogleCredential.FromFile(googleSettings.CredentialsPath)
            });
        
        services.AddSingleton(FirebaseMessaging.GetMessaging(firebaseApp));
        services.AddScoped<FirebaseService>();

        return services;
    }
}