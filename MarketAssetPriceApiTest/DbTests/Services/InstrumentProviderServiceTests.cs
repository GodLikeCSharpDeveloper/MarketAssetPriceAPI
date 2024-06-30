using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.DbService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class InstrumentProviderServiceTests
    {
        private Mock<IInstrumentProviderRepository> _mockInstrumentProviderRepository;
        private InstrumentProviderService _instrumentProviderService;

        [SetUp]
        public void Setup()
        {
            _mockInstrumentProviderRepository = new Mock<IInstrumentProviderRepository>();
            _instrumentProviderService = new InstrumentProviderService(_mockInstrumentProviderRepository.Object);
        }

        [Test]
        public async Task AddNewInstrumentProvider_ValidEntity_CallsRepository()
        {
            // Arrange
            var instrumentProvider = new InstrumentProviderRelationEntity { ProviderId = 1, InstrumentId = 1 };

            // Act
            await _instrumentProviderService.AddNewInstrumentProvider(instrumentProvider);

            // Assert
            _mockInstrumentProviderRepository.Verify(repo => repo.AddNewInstrumentProvider(instrumentProvider), Times.Once);
        }

        [Test]
        public async Task AddNewInstrumentProvider_InvalidEntity_DoesNotCallRepository()
        {
            // Arrange
            var invalidInstrumentProvider = new InstrumentProviderRelationEntity { ProviderId = 0, InstrumentId = 1 };

            // Act
            await _instrumentProviderService.AddNewInstrumentProvider(invalidInstrumentProvider);

            // Assert
            _mockInstrumentProviderRepository.Verify(repo => repo.AddNewInstrumentProvider(It.IsAny<InstrumentProviderRelationEntity>()), Times.Never);
        }

        [Test]
        public async Task AddNewInstrumentProviders_ValidEntities_CallsRepository()
        {
            // Arrange
            var instrumentProviders = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { ProviderId = 1, InstrumentId = 1 },
                new InstrumentProviderRelationEntity { ProviderId = 2, InstrumentId = 2 }
            };

            // Act
            await _instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);

            // Assert
            _mockInstrumentProviderRepository.Verify(repo => repo.AddNewInstrumentProviders(instrumentProviders), Times.Once);
        }

        [Test]
        public async Task AddNewInstrumentProviders_InvalidEntities_DoesNotCallRepository()
        {
            // Arrange
            var instrumentProviders = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { ProviderId = 0, InstrumentId = 1 },
                new InstrumentProviderRelationEntity { ProviderId = 2, InstrumentId = 0 }
            };

            // Act
            await _instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);

            // Assert
            _mockInstrumentProviderRepository.Verify(repo => repo.AddNewInstrumentProviders(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Never);
        }

        [Test]
        public async Task AddNewInstrumentProviders_NullList_DoesNotCallRepository()
        {
            // Act
            await _instrumentProviderService.AddNewInstrumentProviders(null);

            // Assert
            _mockInstrumentProviderRepository.Verify(repo => repo.AddNewInstrumentProviders(It.IsAny<List<InstrumentProviderRelationEntity>>()), Times.Never);
        }
    }
}
