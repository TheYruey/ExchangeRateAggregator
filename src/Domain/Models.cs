// src/Domain/Models.cs
namespace Domain;

public record ExchangeRequest(string SourceCurrency, string TargetCurrency, decimal Amount);
public record Offer(string ProviderName, decimal ConvertedAmount);