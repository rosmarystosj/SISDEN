using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using SISDENFRONT.Data;
using SISDENFRONT;
using Blazored.LocalStorage;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<RegistroEntidadService>();
builder.Services.AddSingleton<RegistroDenuncianteService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<LoginModelService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<Comentarios>();
builder.Services.AddScoped<DenunciasService>();
builder.Services.AddScoped<ArticulosService>();
builder.Services.AddHttpContextAccessor();  
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<EntidadService>();
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://sisdem-agf5defyachrh7g9.eastus-01.azurewebsites.net/")
    });


builder.Services.AddAuthorizationCore();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
