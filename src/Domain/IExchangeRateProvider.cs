namespace Domain;

public interface IExchangeRateProvider
{
    // El método debe devolver un objeto Offer o null si falla
    Task<Offer?> GetOfferAsync(ExchangeRequest request, CancellationToken cancellationToken);
}