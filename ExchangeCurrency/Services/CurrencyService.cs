using Microsoft.EntityFrameworkCore;
using ExchangeCurrency.Model;
using ExchangeCurrency.Services;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ExchangeCurrency.Validator;
using FluentValidation.Results;
using FluentValidation;

namespace ExchangeCurrency.Services
{
    public class CurrencyService : ICurrency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<CurrencyModel> _validator;
        private readonly CurrencyModel _currencyModel;
        public CurrencyService(IHttpClientFactory httpClientFactory, IValidator<CurrencyModel> validator)
        {
            _httpClientFactory = httpClientFactory;
            _validator = validator;
        }
        
        public CurrencyService(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
        }
        public async Task<decimal> ConvertCurrency(string sourceCurrency, decimal amount, string targetCurrency)
        {
            var validationResult = _validator.Validate(new CurrencyModel
            {
                SourceCurrency = sourceCurrency,
                Amount = amount,
                TargetCurrency = targetCurrency
            });

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://api.fastforex.io/fetch-all?api_key=fd1ff3baec-976e3f9845-rzsk3b");

            var jsonObject = JsonDocument.Parse(response).RootElement;
            var results = jsonObject.GetProperty("results");

            var exchangeRates = new Dictionary<string, decimal>();

            

            foreach (var rate in results.EnumerateObject())
            {
                var currency = rate.Name;
                var rateValue = rate.Value.GetDecimal();
                exchangeRates[currency] = rateValue;
            }         
            var validCurrencies = new List<string> { "UAH", "USD", "EUR" };
            var sourceCurrencyUpper = sourceCurrency.ToUpper();
            var targetCurrencyUpper = targetCurrency.ToUpper();

            if (!validCurrencies.Contains(sourceCurrencyUpper) || !validCurrencies.Contains(targetCurrencyUpper))
            {
                throw new ArgumentException("Invalid currency codes. Only UAH, USD, and EUR are allowed.");
            }

            if (exchangeRates.ContainsKey(sourceCurrencyUpper) && exchangeRates.ContainsKey(targetCurrencyUpper))
            {
                decimal sourceCurrencyRate = exchangeRates[sourceCurrencyUpper];
                decimal targetCurrencyRate = exchangeRates[targetCurrencyUpper];

                decimal convertedAmount = (amount / sourceCurrencyRate) * targetCurrencyRate;
                return convertedAmount;
            }

            throw new Exception("Invalid currency codes.");

        }
    }
}      
