using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Services;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentController(IInstrumentControllerService instrumentControllerService) : ControllerBase
    {
        private readonly IInstrumentControllerService instrumentControllerService = instrumentControllerService;

        [HttpGet("list-instruments")]
        public async Task<IActionResult> GetAllInstruments([FromQuery] InstrumentQueryParameters parameters)
        {
            var data = await instrumentControllerService.GetInstruments(parameters);
            if(data==null)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpGet("list-providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var data = await instrumentControllerService.GetProviders();
            if (data == null)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpGet("list-exchanges")]
        public async Task<IActionResult> GetAllExchanges([FromQuery] ExchangesQueryParameters parameters)
        {
            var data = await instrumentControllerService.GetExchanges(parameters);
            if (data == null)
                return BadRequest(data);
            return Ok(data);
        }
    }
}
