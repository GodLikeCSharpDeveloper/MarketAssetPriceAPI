using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Services.MapperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest.Controller.Mapper
{
    [TestFixture]
    public class ProviderMappingTests
    {
        [Test]
        public void MapInstrumentEntity_MapsCorrectly()
        {
            // Arrange
            var instrument = new Instrument
            {
                ApiProviderId = "TestId",
                Description = "Instrument1",
                Kind = "Type1",
                BaseCurrency = "curr",
                Currency = "currency",
                Mappings = new Dictionary<string, Mapping> { { "name", new Mapping() { DefaultOrderSize = 1, Exchange = "change", Symbol = "symbol" } } },
                Symbol = "symbol",
                TickSize = 1
            };

            // Act
            var result = InstrumentMapper.MapInstrumentEntity(instrument);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(instrument.ApiProviderId, Is.EqualTo(result.ApiProviderId));
                Assert.That(instrument.Currency, Is.EqualTo(result.Currency));
                Assert.That(instrument.BaseCurrency, Is.EqualTo(result.BaseCurrency));
                Assert.That(instrument.Kind, Is.EqualTo(result.Kind));
                Assert.That(instrument.Description, Is.EqualTo(result.Description));
                Assert.That(instrument.Symbol, Is.EqualTo(result.Symbol));
                Assert.That(instrument.TickSize, Is.EqualTo(result.TickSize));
                Assert.That(instrument.Mappings.FirstOrDefault().Value.Exchange, Is.EqualTo(result.Providers.FirstOrDefault().Exchange.ExchangeName));
                Assert.That(instrument.Mappings.FirstOrDefault().Value.Symbol, Is.EqualTo(result.Providers.FirstOrDefault().Symbol));
                Assert.That(instrument.Mappings.FirstOrDefault().Value.DefaultOrderSize, Is.EqualTo(result.Providers.FirstOrDefault().DefaultOrderSize));
            });
        }
        [Test]
        public void MapProviderEntity_MapsCorrectly()
        {
            // Arrange
            var instrument = new KeyValuePair<string, Mapping>("name", new Mapping() { DefaultOrderSize = 1, Exchange = "change", Symbol = "symbol" });

            // Act
            var result = InstrumentMapper.MapProviderEntity(instrument);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(instrument.Key, Is.EqualTo(result.ProviderName));
                Assert.That(instrument.Value.Exchange, Is.EqualTo(result.Exchange.ExchangeName));
                Assert.That(instrument.Value.DefaultOrderSize, Is.EqualTo(result.DefaultOrderSize));
                Assert.That(instrument.Value.Symbol, Is.EqualTo(result.Symbol));
            });
        }
    }

}
