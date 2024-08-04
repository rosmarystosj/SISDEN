using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SISDEN.Models;
using Microsoft.EntityFrameworkCore;
using SISDEN.Services;
using Microsoft.AspNetCore.Identity;
using SISDEN.DTOS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using SISDEN.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IServicioEmail, EmailService>();
builder.Services.AddScoped<IRegistrarDenuncia, RegistroDenunciaService>();
builder.Services.AddScoped<ISesion, ObtenerSesionIdService>();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<SisdemContext>()
//    .AddDefaultTokenProviders();

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

// Configurar SignalR
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSession();
app.MapControllers();

// Configurar el endpoint del Hub
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();
