using ExchangeCurrency.Model;
using FluentValidation;

namespace ExchangeCurrency.Validator
{

    public class CurrencyConversionValidator : AbstractValidator<CurrencyModel>
    {
       
        public CurrencyConversionValidator()
        {
            RuleFor(request => request.SourceCurrency).NotEmpty().Length(3).WithMessage("Source currency code must be 3 characters long.");
            RuleFor(request => request.SourceCurrency).NotEmpty().Length(3).Must(BeAValidCurrency).WithMessage(" Source Currency must be only UAH,USD,EUR");

            RuleFor(request => request.TargetCurrency).NotEmpty().Length(3).WithMessage("Target currency code must be 3 characters long.");
            RuleFor(request => request.TargetCurrency).NotEmpty().Length(3).Must(BeAValidCurrency).WithMessage(" Target Currency must be only UAH,USD,EUR");

            RuleFor(request => request.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
          

        }
        private bool BeAValidCurrency(string currency)
        {
            var validCurrencies = new[] { "USD", "UAH", "EUR" };
            return validCurrencies.Contains(currency);
        }
    }
    
}
