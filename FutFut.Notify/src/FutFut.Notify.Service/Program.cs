using System.Reflection;
using FutFut.Common.EfCore;
using FutFut.Common.Identity;
using FutFut.Common.MassTransit;
using FutFut.Notify.Service.Data;
using FutFut.Notify.Service.Data.Entities;
using FutFut.Notify.Service.Firebase;
using FutFut.Notify.Service.Mapping;
using FutFut.Notify.Service.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services
    .AddEfCoreDbContext<AppDbContext>()
    .AddEFCoreRepository<NotificationEntity, AppDbContext>()
    .AddEFCoreRepository<DeviceEntity, AppDbContext>()
    .AddMassTransitWithRabbitMQ()
    .AddFirebaseMessaging(builder.Configuration)
    .AddJwtBearerAuthentication();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(opt =>
    {
        opt.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();