namespace Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ExchangeService(IEnumerable<IExchangeRateProvider> providers)
{
    private readonly IEnumerable<IExchangeRateProvider> _providers = providers;

    public async Task<Offer?> GetBestOfferAsync(ExchangeRequest request, CancellationToken cancellationToken)
    {
        var tasks = _providers.Select(p => p.GetOfferAsync(request, cancellationToken)).ToList();
        
        var results = await Task.WhenAll(tasks);

        var bestOffer = results
            .Where(offer => offer is not null)
            .OrderByDescending(offer => offer!.ConvertedAmount) // <-- Corrección de Warning
            .FirstOrDefault();
            
        return bestOffer;
    }
}