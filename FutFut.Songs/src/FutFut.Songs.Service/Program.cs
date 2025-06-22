using FutFut.Common.AWS3;
using FutFut.Common.EfCore;
using FutFut.Common.Logging;
using FutFut.Common.MassTransit;
using FutFut.Songs.Service.Data;
using FutFut.Songs.Service.Data.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.AddLoggingWithSeqAndLogstash();
builder.Services.AddEfCoreDbContext<AppDbContext>()
    .AddEFCoreRepository<SongEntity, AppDbContext>()
    .AddMassTransitWithRabbitMQ()
    .AddAWS3Storage();

builder.Services.AddControllers();

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
