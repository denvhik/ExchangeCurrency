using ExchangeCurrency.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Services
{
    public interface ICurrency
    {
        Task<decimal> ConvertCurrency(string SourceCurrency, decimal Amount, string TargetCurrency);
    }
}
