using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public interface IWebSocketClientControllerService
    {
        public Task Start();
        public Task SendSubscription();
        public Task Stop();
    }
}
