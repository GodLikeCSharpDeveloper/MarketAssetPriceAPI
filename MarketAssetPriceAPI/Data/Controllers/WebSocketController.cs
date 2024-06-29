using MarketAssetPriceAPI.Data.Services;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;

namespace MarketAssetPriceAPI.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketController(IWebSocketClientControllerService webSocketClientService) : ControllerBase
    {
        private readonly IWebSocketClientControllerService webSocketClientService = webSocketClientService;
        [HttpGet("start")]
        public async Task<IActionResult> StartStreaming()
        {
            await webSocketClientService.Start();
            await webSocketClientService.SendSubscription();
            return Ok("Streaming started.");
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopStreaming()
        {
            await webSocketClientService.Stop();
            return Ok("Streaming stopped.");
        }
    }
}
