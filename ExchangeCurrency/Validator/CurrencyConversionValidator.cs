using ExchangeCurrency.Model;
using FluentValidation;

namespace ExchangeCurrency.Validator
{

    public class CurrencyConversionValidator : AbstractValidator<CurrencyModel>
    {
       
        public CurrencyConversionValidator()
        {
            RuleFor(request => request.SourceCurrency).NotEmpty().Length(3).WithMessage("Source currency code must be 3 characters long.");
            RuleFor(request => request.TargetCurrency).NotEmpty().Length(3).WithMessage("Target currency code must be 3 characters long.");
            RuleFor(request => request.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
          

        }
        
    }
    
}
