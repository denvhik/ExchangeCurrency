using NUnit.Framework;
using FluentValidation;
using ExchangeCurrency.Validator;
using ExchangeCurrency.Model;

namespace ExchangeCurrency.Test
{
    public class CurrencyConversionTests
    {
        private CurrencyConversionValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CurrencyConversionValidator();
        }

        [Test]
        public void Should_Pass_Validation_When_Parameters_Are_Valid()
        {
         
            var request = new CurrencyModel
            {
                SourceCurrency = "USD",
                Amount = 100,
                TargetCurrency = "EUR"
            };

        
            var result = _validator.Validate(request);

         
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Should_Fail_Validation_When_SourceCurrency_Is_Invalid()
        {
           
            var request = new CurrencyModel
            {
                SourceCurrency = "USDD", // Invalid currency code
                Amount = 100,
                TargetCurrency = "EUR"
            };

            
            var result = _validator.Validate(request);

            
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Source Currency' must be 3 characters long.", result.Errors[0].ErrorMessage);
        }
    }
}
