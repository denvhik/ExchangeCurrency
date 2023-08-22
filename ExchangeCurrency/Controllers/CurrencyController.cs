using ExchangeCurrency.Model;
using ExchangeCurrency.Services;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
namespace ExchangeCurrency.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrency _iCurrency;

        public CurrencyConversionController(ICurrency iCurrency)
        {
            _iCurrency = iCurrency;
        }

        [HttpPost("Exchange")]
        public async Task<ActionResult<decimal>> ConvertCurrency([FromBody] CurrencyModel currencyModel)
        {
            try
            {
                
                var convertedAmount = await _iCurrency.ConvertCurrency(currencyModel.SourceCurrency, currencyModel.Amount, currencyModel.TargetCurrency);
                return Ok(convertedAmount);
            }
            catch (ValidationException validationEx)
            {
                var errorMessages = validationEx.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting currency: {ex.Message}");
            }
           
        }
    }
}