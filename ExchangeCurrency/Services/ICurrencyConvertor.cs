using ExchangeCurrency.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Services
{
    public interface ICurrencyConvertor
    {
        Task<decimal> ConvertCurrency(string sourceCurrency, decimal amount, string targetCurrency);
    }
}
