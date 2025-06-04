using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FutFut.Identity.Service.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder
    .Configuration.GetConnectionString("ApplicationDbContextConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(c =>
    {
        c.DocumentPath = "/openapi/v1.json";
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

