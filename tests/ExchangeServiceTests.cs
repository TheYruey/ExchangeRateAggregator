using Xunit;
using Moq;
using Domain;
using Application;

namespace Tests;

public class ExchangeServiceTests
{
    [Fact]
    public async Task GetBestOfferAsync_ShouldReturnHighestOffer_WhenMultipleProvidersSucceed()
    {
        // Arrange (Preparar)
        var request = new ExchangeRequest("USD", "DOP", 100);

        var mockProvider1 = new Mock<IExchangeRateProvider>();
        mockProvider1
            .Setup(p => p.GetOfferAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Offer("P1", 5850m));

        var mockProvider2 = new Mock<IExchangeRateProvider>();
        mockProvider2
            .Setup(p => p.GetOfferAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Offer("P2", 5870m)); // La mejor oferta

        var mockProvider3 = new Mock<IExchangeRateProvider>(); // Proveedor que falla
        mockProvider3
            .Setup(p => p.GetOfferAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Offer?)null);

        var providers = new[] { mockProvider1.Object, mockProvider2.Object, mockProvider3.Object };
        var service = new ExchangeService(providers);

        // Act (Actuar)
        var result = await service.GetBestOfferAsync(request, CancellationToken.None);

        // Assert (Verificar)
        Assert.NotNull(result);
        Assert.Equal("P2", result.ProviderName);
        Assert.Equal(5870m, result.ConvertedAmount);
    }
}