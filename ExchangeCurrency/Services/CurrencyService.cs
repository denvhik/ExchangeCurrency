using Microsoft.EntityFrameworkCore;
using ExchangeCurrency.Model;
using ExchangeCurrency.Services;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using ExchangeCurrency.Validator;
using FluentValidation.Results;
using FluentValidation;

namespace ExchangeCurrency.Services
{
    public class CurrencyService : ICurrency
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private readonly CurrencyModel _currencyModel;
        public CurrencyService(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
        }
        public async Task<decimal> ConvertCurrency(string sourceCurrency, decimal amount, string targetCurrency)
        {
           
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
            var validator = new CurrencyConversionValidator();
            ValidationResult validationResult = validator.Validate(_currencyModel);

            if (!validationResult.IsValid)
            {
                string errorMessages = string.Join("\n", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new ValidationException(errorMessages);
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
