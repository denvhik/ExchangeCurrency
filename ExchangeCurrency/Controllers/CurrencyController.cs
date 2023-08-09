using ExchangeCurrency.Model;
using ExchangeCurrency.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<decimal>> ConvertCurrency([FromBody] CurrencyModel currencyModel)
        {
            try
            {
                var convertedAmount = await _iCurrency.ConvertCurrency(currencyModel.SourceCurrency, currencyModel.Amount, currencyModel.TargetCurrency);
                return Ok(convertedAmount);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting currency: {ex.Message}");
            }
        }
    }
}