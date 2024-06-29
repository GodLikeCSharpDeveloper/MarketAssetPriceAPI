using MarketAssetPriceAPI.Data.Models.Exchanges;
using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentController : ControllerBase
    {
        private readonly FintachartsService _fintachartsService;

        public InstrumentController(FintachartsService fintachartsService)
        {
            _fintachartsService = fintachartsService;
        }

        [HttpGet("list-instruments")]
        public async Task<IActionResult> GetAllInstruments([FromQuery] InstrumentQueryParameters parameters)
        {
            var data = await _fintachartsService.GetInstrumentsAsync(parameters);
            return Ok(data);
        }

        [HttpGet("list-providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var data = await _fintachartsService.GetProvidersAsync();
            return Ok(data);
        }

        [HttpGet("list-exchanges")]
        public async Task<IActionResult> GetAllExchanges([FromQuery] ExchangesQueryParameters parameters)
        {
            var data = await _fintachartsService.GetExchangesAsync(parameters);
            return Ok(data);
        }
    }
}
