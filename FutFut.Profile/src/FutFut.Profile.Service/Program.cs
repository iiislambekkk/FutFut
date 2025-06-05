using FutFut.Common.EfCore;
using FutFut.Profile.Service.Data;
using FutFut.Profile.Service.Entities;
using FutFut.Profile.Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEfCoreDbContext<AppDbContext>();
builder.Services.AddEFCoreRepository<ProfileEntity, AppDbContext>();
builder.Services.AddEFCoreRepository<PlayedHistory, AppDbContext>();
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