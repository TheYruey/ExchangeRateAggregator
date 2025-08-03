// MockApi/Program.cs
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simulación de API1 (JSON) 
app.MapPost("/api1/rate", (Api1Request req) => {
    if (req.From == "USD" && req.To == "DOP") {
        return Results.Ok(new { rate = 58.5m });
    }
    return Results.BadRequest(new { error = "Currency not supported" });
});

// Simulación de API2 (XML) 
app.MapPost("/api2/exchange", async (HttpRequest request) => {
    using var reader = new StreamReader(request.Body);
    var xmlBody = await reader.ReadToEndAsync();
    var doc = XDocument.Parse(xmlBody);
    var amount = decimal.Parse(doc.Root.Element("Amount").Value);
    var resultXml = new XDocument(new XElement("XML", new XElement("Result", amount * 58.7m)));
    return Results.Content(resultXml.ToString(), "application/xml");
});

// Simulación de API3 (JSON) 
app.MapPost("/api3/convert", (Api3Request req) => {
    if (req.Exchange.SourceCurrency == "USD") {
        var total = req.Exchange.Quantity * 58.6m;
        return Results.Ok(new {
            StatusCode = 200,
            Message = "Success",
            Data = new { Total = total }
        });
    }
    return Results.BadRequest(new { StatusCode = 400, Message = "Error" });
});

app.Run();

// Clases de ayuda para el binding
public record Api1Request(string From, string To, decimal Value);
public record Api3Request(ExchangeData Exchange);
public record ExchangeData(string SourceCurrency, string TargetCurrency, decimal Quantity);