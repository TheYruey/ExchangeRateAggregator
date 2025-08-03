using Domain;

namespace Application;

public class ExchangeService(IEnumerable<IExchangeRateProvider> providers)
{
    // Usamos IEnumerable para que el sistema de DI pueda inyectar todos los proveedores registrados
    private readonly IEnumerable<IExchangeRateProvider> _providers = providers;

    public async Task<Offer?> GetBestOfferAsync(ExchangeRequest request, CancellationToken cancellationToken)
    {
        // Creamos una lista de tareas, una por cada proveedor
        var tasks = _providers.Select(p => p.GetOfferAsync(request, cancellationToken)).ToList();
        
        // Ejecutamos todas las tareas en paralelo y esperamos a que terminen [cite: 18]
        var results = await Task.WhenAll(tasks);

        // Filtramos las ofertas válidas y las ordenamos para encontrar la mejor [cite: 18]
        var bestOffer = results
            .Where(offer => offer is not null)
            .OrderByDescending(offer => offer.ConvertedAmount)
            .FirstOrDefault();
            
        return bestOffer;
    }
}