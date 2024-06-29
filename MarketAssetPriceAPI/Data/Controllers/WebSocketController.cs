using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketController(WebSocketClientControllerService webSocketClientService) : ControllerBase
    {
        private readonly WebSocketClientControllerService webSocketClientService = webSocketClientService;
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
