namespace Domain.Interfaces; 
using Domain.Entities;

public interface IExchangeRateProvider
{
    Task<Offer?> GetOfferAsync(ExchangeRequest request, CancellationToken cancellationToken);
}
