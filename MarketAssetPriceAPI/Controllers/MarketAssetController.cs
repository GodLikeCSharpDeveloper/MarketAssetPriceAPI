using MarketAssetPriceAPI.Models;
using MarketAssetPriceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketAssetController : ControllerBase
    {
        private readonly FintachartsService _fintachartsService;

        public MarketAssetController(FintachartsService fintachartsService)
        {
            _fintachartsService = fintachartsService;
        }

        [HttpGet("price-info")]
        public async Task<IActionResult> GetPriceInfo([FromQuery] string asset)
        {
            var data = await _fintachartsService.GetExchangeDataAsync();
            return Ok(data);
        }
        [HttpGet("list-instruments")]
        public async Task<IActionResult> GetAllInstruments([FromQuery] string provider, [FromQuery] string kind)
        {
            var data = await _fintachartsService.GetInstrumentsAsync(provider, kind);
            return Ok(data);
        }
    }
}
