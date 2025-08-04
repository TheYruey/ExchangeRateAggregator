using Application.Services;
using Domain.Interfaces;
using Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

// --- Configurar Servicios (Inyección de Dependencias) ---

// 1. Añadir servicios de API y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configurar los proveedores de tasas de cambio
const string baseUrl = "http://host.docker.internal:5231/";
builder.Services.AddHttpClient<IExchangeRateProvider, Api1Provider>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<IExchangeRateProvider, Api2Provider>(client => client.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<IExchangeRateProvider, Api3Provider>(client => client.BaseAddress = new Uri(baseUrl));

// 3. Registrar el servicio de aplicación
builder.Services.AddScoped<ExchangeService>();


var app = builder.Build();

// --- Configurar el Pipeline de HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Habilita la UI interactiva de Swagger
}

// app.UseHttpsRedirection(); // <-- Comenta o elimina esta línea

app.MapControllers(); // Mapea los endpoints de los controllers

app.Run();
