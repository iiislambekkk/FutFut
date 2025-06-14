using FutFut.Common.AWS3;
using FutFut.Common.EfCore;
using FutFut.Common.Identity;
using FutFut.Common.Logging;
using FutFut.Common.MassTransit;
using FutFut.Profile.Service.Data;
using FutFut.Profile.Service.Entities;
using FutFut.Profile.Service.HostedServices;
using FutFut.Profile.Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEfCoreDbContext<AppDbContext>()
    .AddEFCoreRepository<ProfileEntity, AppDbContext>()
    .AddEFCoreRepository<SystemWorks, AppDbContext>()
    .AddEFCoreRepository<PlayedHistoryEntity, AppDbContext>()
    .AddEFCoreRepository<AboutPhotoEntity, AppDbContext>()
    .AddEFCoreRepository<FriendShipEntity, AppDbContext>()
    .AddMassTransitWithRabbitMQ()
    .AddAWS3Storage()
    .AddJwtBearerAuthentication();

builder.Services.AddHostedService<ObjectStorageCleanupHostedService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddOpenApi();
builder.AddLoggingWithSeqAndLogstash();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(opt =>
    {
        opt.DocumentPath = "/openapi/v1.json";
    });
}

app.UseCors(opt =>
{
    opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();