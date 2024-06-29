using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketController(WebSocketClientService webSocketClientService) : ControllerBase
    {
        private readonly WebSocketClientService webSocketClientService = webSocketClientService;
        [HttpGet("start")]
        public async Task<IActionResult> StartStreaming()
        {
            await webSocketClientService.StartAsync();
            await webSocketClientService.SendSubscriptionAsync();
            return Ok("Streaming started.");
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopStreaming()
        {
            await webSocketClientService.StopAsync();
            return Ok("Streaming stopped.");
        }
    }
}
