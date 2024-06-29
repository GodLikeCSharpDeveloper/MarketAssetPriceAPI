using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    public class BarsController(BarsControllerService barsService) : ControllerBase
    {
        private readonly BarsControllerService barsService = barsService;
        [HttpGet("list-bars")]
        public async Task<IActionResult> GetBarsCountBack([FromQuery] BarsCountBackQueryParameters parameters)
        {
            var data = await barsService.GetBarsDataAsync(parameters);
            return Ok(data);
        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetBarsDateRange([FromQuery] BarsDateRangeQueryParameters parameters)
        {
            var data = await barsService.GetBarsDataAsync(parameters);
            return Ok(data);
        }
        [HttpGet("time-back")]
        public async Task<IActionResult> GetBarsDateRange([FromQuery] BarsTimeBackQueryParameters parameters)
        {
            var data = await barsService.GetBarsDataAsync(parameters);
            return Ok(data);
        }
    }
}
