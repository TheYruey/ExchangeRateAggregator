using System.Net;
using Moq;
using Moq.Protected;
using Domain.Entities;
using Infrastructure.Providers;

namespace tests.Infrastructure;

public class ProviderTests
{
    private readonly ExchangeRequest _request = new("USD", "DOP", 100);

    // --- Pruebas para Api1Provider ---
    [Fact]
    public async Task Api1Provider_ShouldReturnOffer_WhenApiSucceeds()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""rate"": 58.5}")
        };
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
        var provider = new Api1Provider(httpClient);

        // Act
        var result = await provider.GetOfferAsync(_request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5850, result.ConvertedAmount);
    }

    [Fact]
    public async Task Api1Provider_ShouldReturnNull_WhenApiFails()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("API is down"));

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
        var provider = new Api1Provider(httpClient);

        // Act
        var result = await provider.GetOfferAsync(_request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    // --- Pruebas para Api2Provider ---
    [Fact]
    public async Task Api2Provider_ShouldReturnOffer_WhenApiSucceeds()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"<XML><Result>5870</Result></XML>")
        };
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
        var provider = new Api2Provider(httpClient);

        // Act
        var result = await provider.GetOfferAsync(_request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5870, result.ConvertedAmount);
    }

    // --- Pruebas para Api3Provider ---
    [Fact]
    public async Task Api3Provider_ShouldReturnOffer_WhenApiSucceeds()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""statusCode"": 200, ""data"": {""total"": 5860}}")
        };
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
        var provider = new Api3Provider(httpClient);

        // Act
        var result = await provider.GetOfferAsync(_request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5860, result.ConvertedAmount);
    }
}
