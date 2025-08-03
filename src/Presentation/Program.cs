// src/Presentation/Program.cs
using Microsoft.Extensions.DependencyInjection;
using Domain;
using Application;
using Infrastructure;

// 1. Configurar la inyección de dependencias
var services = new ServiceCollection();

// Configura un HttpClient para cada proveedor con su URL base
const string baseUrl = "http://host.docker.internal:5231/";
services.AddHttpClient<IExchangeRateProvider, Api1Provider>(client => client.BaseAddress = new Uri(baseUrl));
services.AddHttpClient<IExchangeRateProvider, Api2Provider>(client => client.BaseAddress = new Uri(baseUrl));
services.AddHttpClient<IExchangeRateProvider, Api3Provider>(client => client.BaseAddress = new Uri(baseUrl));

// Registra el servicio de aplicación
services.AddScoped<ExchangeService>();

var serviceProvider = services.BuildServiceProvider();

// 2. Ejecutar la lógica de la aplicación
var exchangeService = serviceProvider.GetRequiredService<ExchangeService>();
var request = new ExchangeRequest("USD", "DOP", 100m);

Console.WriteLine($"Buscando la mejor tasa para convertir {request.Amount} {request.SourceCurrency} a {request.TargetCurrency}...");

var bestOffer = await exchangeService.GetBestOfferAsync(request, CancellationToken.None);

// 3. Mostrar el resultado
if (bestOffer != null)
{
    Console.WriteLine("\n--- Mejor Oferta Encontrada ---");
    Console.WriteLine($"Proveedor: {bestOffer.ProviderName}");
    Console.WriteLine($"Monto Convertido: {bestOffer.ConvertedAmount:F2} {request.TargetCurrency}");
}
else
{
    Console.WriteLine("No se pudieron obtener ofertas de ningún proveedor.");
}