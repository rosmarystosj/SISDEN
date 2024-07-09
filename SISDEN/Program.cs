using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SISDEN.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions.Common;
using SISDEN.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IServicioEmail,  EmailService>();
builder.Services.AddScoped<IRegistrarDenuncia, RegistroDenunciaService>();
builder.Services.AddScoped<ISesion, ObtenerSesionIdService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SisdemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SisdemContext"));

});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
