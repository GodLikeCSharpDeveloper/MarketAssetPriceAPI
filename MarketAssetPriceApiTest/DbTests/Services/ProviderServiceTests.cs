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
    public class ProviderServiceTests
    {
        private Mock<IProviderRepository> _mockProviderRepository;
        private Mock<IExchangeService> _mockExchangeService;
        private ProviderService _ProviderService;

        [SetUp]
        public void Setup()
        {
            _mockProviderRepository = new Mock<IProviderRepository>();
            _mockExchangeService = new Mock<IExchangeService>();
            _ProviderService = new ProviderService(_mockProviderRepository.Object, _mockExchangeService.Object);
        }

        [Test]
        public async Task AddExchangeAsync_AddsEntity_ReturnsEntity()
        {
            // Arrange
            var exchangeEntity = new ProviderEntity { ProviderName = "NewProvider" };
            _mockProviderRepository.Setup(repo => repo.AddNewProvider(exchangeEntity))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _ProviderService.AddNewProvider(exchangeEntity);

            // Assert
            Assert.That(exchangeEntity.ProviderName, Is.EqualTo(result.ProviderName));
        }
        [Test]
        public async Task AddExchangeListAsync_AddsEntity_ReturnsEntity()
        {
            // Arrange
            var exchangeEntity = new List<ProviderEntity> { new() { ProviderName = "NewProvider" }, new() { ProviderName = "SecondProvider"} };
            _mockProviderRepository.Setup(repo => repo.AddNewProviders(exchangeEntity))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _ProviderService.AddNewProviders(exchangeEntity);

            // Assert
            exchangeEntity.ForEach(e => 
            { 
                Assert.That(result.Any(d => d.ProviderName == e.ProviderName), Is.True);
            });
        }
    }
}
