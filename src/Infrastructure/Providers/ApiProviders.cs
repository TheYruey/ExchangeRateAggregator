namespace Infrastructure.Providers; // <-- Actualizado
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;
using Domain.Entities;
using Domain.Interfaces;

public abstract class BaseProvider(HttpClient httpClient)
{
    protected readonly HttpClient HttpClient = httpClient;
}

public class Api1Provider(HttpClient client) : BaseProvider(client), IExchangeRateProvider {
    public async Task<Offer?> GetOfferAsync(ExchangeRequest request, CancellationToken ct)
    {
        var payload = new { from = request.SourceCurrency, to = request.TargetCurrency, value = request.Amount };
        try
        {
            var response = await HttpClient.PostAsJsonAsync("api1/rate", payload, ct);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
            var rate = data.GetProperty("rate").GetDecimal();
            return new Offer("API1_JSON", request.Amount * rate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en API1: {ex.Message}");
            return null;
        }
    }
}
public class Api2Provider(HttpClient client) : BaseProvider(client), IExchangeRateProvider {
    public async Task<Offer?> GetOfferAsync(ExchangeRequest request, CancellationToken ct)
    {
        var xmlPayload = $"<XML><From>{request.SourceCurrency}</From><To>{request.TargetCurrency}</To><Amount>{request.Amount}</Amount></XML>";
        var content = new StringContent(xmlPayload, System.Text.Encoding.UTF8, "application/xml");
        try
        {
            var response = await HttpClient.PostAsync("api2/exchange", content, ct);
            response.EnsureSuccessStatusCode();
            var xmlResponse = await response.Content.ReadAsStringAsync(ct);
            var doc = XDocument.Parse(xmlResponse);
            var resultElement = doc.Root?.Element("Result");
            if (resultElement == null) return null; // <-- Corrección de Warning
            var result = decimal.Parse(resultElement.Value);
            return new Offer("API2_XML", result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en API2: {ex.Message}");
            return null;
        }
    }
}
public class Api3Provider(HttpClient client) : BaseProvider(client), IExchangeRateProvider {
    public async Task<Offer?> GetOfferAsync(ExchangeRequest request, CancellationToken ct)
    {
        var payload = new { exchange = new { sourceCurrency = request.SourceCurrency, targetCurrency = request.TargetCurrency, quantity = request.Amount }};
        try
        {
            var response = await HttpClient.PostAsJsonAsync("api3/convert", payload, ct);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
            if (data.GetProperty("statusCode").GetInt32() == 200)
            {
                var total = data.GetProperty("data").GetProperty("total").GetDecimal();
                return new Offer("API3_JSON_Nested", total);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en API3: {ex.Message}");
            return null;
        }
    }
}