using Domain;

namespace Application;

// Clase que representa un servicio de intercambio de divisas
public class ExchangeService(IEnumerable<IExchangeRateProvider> providers)
{
    // Campo privado que almacena una colección de proveedores de tasas de cambio
    private readonly IEnumerable<IExchangeRateProvider> _providers = providers;

    // Método asíncrono que obtiene la mejor oferta de intercambio según una solicitud
    public async Task<Offer?> GetBestOfferAsync(ExchangeRequest request, CancellationToken cancellationToken)
    {
        // Se crean tareas para obtener ofertas de todos los proveedores
        var tasks = _providers.Select(p => p.GetOfferAsync(request, cancellationToken)).ToList();

        // Espera a que todas las tareas se completen y obtiene los resultados
        var results = await Task.WhenAll(tasks);

        // Filtra las ofertas no nulas, las ordena por el monto convertido en orden descendente
        // y selecciona la mejor oferta (la primera en el orden)
        var bestOffer = results
            .Where(offer => offer is not null)
            .OrderByDescending(offer => offer!.ConvertedAmount)
            .FirstOrDefault();

        // Devuelve la mejor oferta encontrada o null si no hay ofertas válidas
        return bestOffer;
    }
}