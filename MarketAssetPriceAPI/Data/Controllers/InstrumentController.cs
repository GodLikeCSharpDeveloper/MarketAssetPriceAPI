using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentController : ControllerBase
    {
        private readonly InstrumentControllerService instrumentControllerService;

        public InstrumentController(InstrumentControllerService instrumentControllerService)
        {
            this.instrumentControllerService = instrumentControllerService;
        }

        [HttpGet("list-instruments")]
        public async Task<IActionResult> GetAllInstruments([FromQuery] InstrumentQueryParameters parameters)
        {
            var data = await instrumentControllerService.GetInstrumentsAsync(parameters);
            return Ok(data);
        }

        [HttpGet("list-providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var data = await instrumentControllerService.GetProvidersAsync();
            return Ok(data);
        }

        [HttpGet("list-exchanges")]
        public async Task<IActionResult> GetAllExchanges([FromQuery] ExchangesQueryParameters parameters)
        {
            var data = await instrumentControllerService.GetExchangesAsync(parameters);
            return Ok(data);
        }
    }
}
