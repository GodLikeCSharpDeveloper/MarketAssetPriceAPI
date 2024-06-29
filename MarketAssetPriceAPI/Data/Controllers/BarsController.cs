using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    public class BarsController(IBarsControllerService barsService) : ControllerBase
    {
        private readonly IBarsControllerService barsService = barsService;
        [HttpGet("list-bars")]
        public async Task<IActionResult> GetBarsCountBack([FromQuery] BarsCountBackQueryParameters parameters)
        {
            var result = await barsService.GetBarsData(parameters);
            if (result == null || !result.IsSuccess)           
                return BadRequest(result);            
            return Ok(result);
        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetBarsDateRange([FromQuery] BarsDateRangeQueryParameters parameters)
        {
            var result = await barsService.GetBarsData(parameters);
            if (result == null || !result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("time-back")]
        public async Task<IActionResult> GetBarsDateRange([FromQuery] BarsTimeBackQueryParameters parameters)
        {
            var result = await barsService.GetBarsData(parameters);
            if (result == null || !result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
