using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.DbService;
using MarketAssetPriceAPI.Data.Services.MapperService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class InstrumentServiceTests
    {
        private Mock<IInstrumentRepository> _mockInstrumentRepository;
        private Mock<IProviderService> _mockProviderService;
        private Mock<IInstrumentProviderService> _mockInstrumentProviderService;
        private Mock<IExchangeService> _mockExchangeService;
        private InstrumentService _instrumentService;

        [SetUp]
        public void Setup()
        {
            _mockInstrumentRepository = new Mock<IInstrumentRepository>();
            _mockProviderService = new Mock<IProviderService>();
            _mockInstrumentProviderService = new Mock<IInstrumentProviderService>();
            _mockExchangeService = new Mock<IExchangeService>();
            _instrumentService = new InstrumentService(_mockInstrumentRepository.Object,
                                                       _mockProviderService.Object,
                                                       _mockInstrumentProviderService.Object,
                                                       _mockExchangeService.Object);
        }

        [Test]
        public async Task AddNewInstrument_ValidInstrument_CallsRepositoryAndServices()
        {
            // Arrange
            var instrument = new Instrument
            {
                ApiProviderId = "Instrument1",
                Mappings = new Dictionary<string, Mapping> {
                    {
                        "test", new Mapping { Symbol = "symbol", DefaultOrderSize = 1, Exchange = "Exchange1" }
                    }
                }
            };
            var providerEntities = new List<ProviderEntity>
            {
                new ProviderEntity { Id = 1, ProviderName = "Provider1", ExchangeId = 1 }
            };


            _mockProviderService.Setup(s => s.AddNewProviders(It.IsAny<List<ProviderEntity>>()))
                                .ReturnsAsync(providerEntities);

            // Act
            await _instrumentService.AddNewInstrument(instrument);

            // Assert
            _mockInstrumentRepository.Verify(r => r.AddNewInstrument(It.Is<InstrumentEntity>(ie => ie.ApiProviderId == "Instrument1")), Times.Once);
            _mockProviderService.Verify(s => s.AddNewProviders(It.IsAny<List<ProviderEntity>>()), Times.Once);
            _mockInstrumentProviderService.Verify(s => s.AddNewInstrumentProviders(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Once);
        }

        [Test]
        public async Task AddNewInstruments_ValidInstruments_CallsRepositoryAndServices()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new()
                {
                    ApiProviderId = "Instrument1",
                    Mappings = new Dictionary<string, Mapping> 
                    {
                        {
                            "test", new Mapping { Symbol = "symbol", DefaultOrderSize = 1, Exchange = "Exchange1" }
                        }
                    }
                },
                new()
                {
                    ApiProviderId = "Instrument2",
                    Mappings = new Dictionary<string, Mapping>
                    {
                        {
                            "test2", new Mapping { Symbol = "symbol2", DefaultOrderSize = 2, Exchange = "Exchange2" }
                        }
                    }
                }
            };
            var instrumentEntities = new List<InstrumentEntity>
            {
                new()
                {
                    Id = 1,
                    ApiProviderId = "Instrument1",
                    Providers = new List<ProviderEntity>
                    {
                        new ProviderEntity { Id = 1, ProviderName = "Provider1", ExchangeId = 1 }
                    }
                },
                new()
                {
                    Id = 2,
                    ApiProviderId = "Instrument2",
                    Providers = new List<ProviderEntity>
                    {
                        new ProviderEntity { Id = 2, ProviderName = "Provider2", ExchangeId = 2 }
                    }
                },
            };

            _mockInstrumentRepository.Setup(r => r.AddNewInstruments(It.IsAny<List<InstrumentEntity>>()))
                                     .ReturnsAsync(instrumentEntities);

            _mockProviderService.Setup(s => s.AddNewProviders(It.IsAny<List<ProviderEntity>>()))
                                .ReturnsAsync(instrumentEntities.First().Providers);

            _mockExchangeService.Setup(e => e.GetOrAddExchangeEntity(It.IsAny<ExchangeEntity>()))
                                .ReturnsAsync(new ExchangeEntity { Id = 1, ExchangeName = "Exchange1" });

            // Act
            await _instrumentService.AddNewInstruments(instruments);

            // Assert
            _mockInstrumentRepository.Verify(r => r.AddNewInstruments(It.IsAny<List<InstrumentEntity>>()), Times.Once);
            _mockProviderService.Verify(s => s.AddNewProviders(It.IsAny<List<ProviderEntity>>()), Times.Exactly(instruments.Count));
            _mockInstrumentProviderService.Verify(s => s.AddNewInstrumentProviders(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Exactly(instruments.Count));
        }
        [Test]
        public async Task AddOrUpdateNewInstruments_AddsAndUpdatesInstruments()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new Instrument { ApiProviderId = "1", Symbol = "SYM1" },
                new Instrument { ApiProviderId = "2", Symbol = "SYM2" }
            };

            var instrumentEntities = new List<InstrumentEntity>
            {
                new InstrumentEntity { ApiProviderId = "1", Symbol = "SYM1" },
                new InstrumentEntity { ApiProviderId = "2", Symbol = "SYM2" }
            };

            var existingInstruments = new List<InstrumentEntity>
            {
                new InstrumentEntity { ApiProviderId = "1", Symbol = "SYM3" }
            };

            _mockInstrumentRepository.Setup(repo => repo.GetInstrumentsByApiProviderIds(It.IsAny<List<string>>()))
                .ReturnsAsync(existingInstruments);

            _mockInstrumentRepository.Setup(repo => repo.AddNewInstruments(It.IsAny<List<InstrumentEntity>>()))
                .ReturnsAsync((List<InstrumentEntity> entities) => entities);

            _mockInstrumentRepository.Setup(repo => repo.UpdateInstruments(It.IsAny<List<InstrumentEntity>>()))
                .ReturnsAsync((List<InstrumentEntity> entities) => entities);

            // Act
            await _instrumentService.AddOrUpdateNewInstruments(instruments);

            // Assert
            _mockInstrumentRepository.Verify(repo => repo.GetInstrumentsByApiProviderIds(It.IsAny<List<string>>()), Times.Once);
            _mockInstrumentRepository.Verify(repo => repo.AddNewInstruments(It.IsAny<List<InstrumentEntity>>()), Times.Once);
            _mockInstrumentRepository.Verify(repo => repo.UpdateInstruments(It.IsAny<List<InstrumentEntity>>()), Times.Once);
            _mockInstrumentProviderService.Verify(service => service.UpdateInstrumentProvidersRelations(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Once);
        }

        [Test]
        public async Task UpdateInstruments_UpdatesExistingInstruments()
        {
            // Arrange
            var existingInstruments = new List<InstrumentEntity>
            {
                new InstrumentEntity { ApiProviderId = "1", Symbol = "SYM1", Providers = new List<ProviderEntity> { new ProviderEntity { Id = 1 } } },
                new InstrumentEntity { ApiProviderId = "2", Symbol = "SYM2", Providers = new List<ProviderEntity> { new ProviderEntity { Id = 2 } } }
            };

            var instrumentsToUpdate = new List<InstrumentEntity>
            {
                new InstrumentEntity { ApiProviderId = "1", Symbol = "NEW_SYM1", Providers = new List<ProviderEntity> { new ProviderEntity { Id = 3 } } },
                new InstrumentEntity { ApiProviderId = "2", Symbol = "NEW_SYM2", Providers = new List<ProviderEntity> { new ProviderEntity { Id = 4 } } }
            };

            _mockInstrumentRepository.Setup(repo => repo.UpdateInstruments(It.IsAny<List<InstrumentEntity>>()))
                .ReturnsAsync((List<InstrumentEntity> entities) => entities);

            // Act
            await _instrumentService.UpdateInstruments(existingInstruments, instrumentsToUpdate);

            // Assert
            _mockInstrumentRepository.Verify(repo => repo.UpdateInstruments(It.IsAny<List<InstrumentEntity>>()), Times.Once);
            _mockInstrumentProviderService.Verify(service => service.UpdateInstrumentProvidersRelations(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Once);
        }
    }

}
