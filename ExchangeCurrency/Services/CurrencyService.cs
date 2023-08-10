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
        public CurrencyService(IHttpClientFactory httpClientFactory, IValidator<CurrencyModel> validator, CurrencyModel currencyModel)
        {
            _httpClientFactory = httpClientFactory;
            _validator = validator;
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
            var response = await client.GetStringAsync("https://api.fastforex.io/fetch-all?api_key=3ddd8f5cf1-3defc33746-rz4ky3");

            var jsonObject = JsonDocument.Parse(response).RootElement;
            var results = jsonObject.GetProperty("results");

            var exchangeRates = new Dictionary<string, decimal>();

            

            foreach (var rate in results.EnumerateObject())
            {
                var currency = rate.Name;
                var rateValue = rate.Value.GetDecimal();
                exchangeRates[currency] = rateValue;
            }
           
            var sourceCurrencyUpper = sourceCurrency.ToUpper();
            var targetCurrencyUpper = targetCurrency.ToUpper();

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
