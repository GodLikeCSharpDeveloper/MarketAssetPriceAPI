using MarketAssetPriceAPI.Models.Bars;
using MarketAssetPriceAPI.Models.Instruments;
using MarketAssetPriceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Controllers
{
    public class BarsController(BarsService barsService) : ControllerBase
    {
        private readonly BarsService barsService = barsService;
        [HttpGet("list-bars")]
        public async Task<IActionResult> GetBarsCountBack([FromQuery] BarsCountBackQueryParameters parameters)
        {
            var data = await barsService.GetBarsCountBackAsync(parameters);
            return Ok(data);
        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetBarsDateRange([FromQuery] BarsDateRangeQueryParameters parameters)
        {
            var data = await barsService.GetBarsCountBackAsync(parameters);
            return Ok(data);
        }
    }
}
