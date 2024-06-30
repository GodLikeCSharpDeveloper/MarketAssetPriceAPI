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
    public class ExchangeServiceTests
    {
        private Mock<IExchangeRepository> _mockExchangeRepository;
        private ExchangeService _exchangeService;

        [SetUp]
        public void Setup()
        {
            _mockExchangeRepository = new Mock<IExchangeRepository>();
            _exchangeService = new ExchangeService(_mockExchangeRepository.Object);
        }

        [Test]
        public async Task GetOrAddExchangeEntity_ExistingEntity_ReturnsEntity()
        {
            // Arrange
            var exchangeEntity = new ExchangeEntity { ExchangeName = "TestExchange" };
            _mockExchangeRepository.Setup(repo => repo.GetEchangeAsync(exchangeEntity.ExchangeName))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _exchangeService.GetOrAddExchangeEntity(exchangeEntity);

            // Assert
            Assert.That(exchangeEntity, Is.EqualTo(result));
            _mockExchangeRepository.Verify(repo => repo.AddExchangeAsync(It.IsAny<ExchangeEntity>()), Times.Never);
        }

        [Test]
        public async Task GetOrAddExchangeEntity_NewEntity_AddsEntity()
        {
            // Arrange
            var exchangeEntity = new ExchangeEntity { ExchangeName = "NewExchange" };
            _mockExchangeRepository.Setup(repo => repo.GetEchangeAsync(exchangeEntity.ExchangeName))
                .ReturnsAsync((ExchangeEntity)null);
            _mockExchangeRepository.Setup(repo => repo.AddExchangeAsync(exchangeEntity))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _exchangeService.GetOrAddExchangeEntity(exchangeEntity);

            // Assert
            Assert.That(exchangeEntity, Is.EqualTo(result));
            _mockExchangeRepository.Verify(repo => repo.AddExchangeAsync(exchangeEntity), Times.Once);
        }

        [Test]
        public async Task GetExchangeEntityByName_ExistingEntity_ReturnsEntity()
        {
            // Arrange
            var exchangeName = "ExistingExchange";
            var exchangeEntity = new ExchangeEntity { ExchangeName = exchangeName };
            _mockExchangeRepository.Setup(repo => repo.GetEchangeAsync(exchangeName))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _exchangeService.GetExchangeEntityByName(exchangeName);

            // Assert
            Assert.That(exchangeEntity, Is.EqualTo(result));
        }

        [Test]
        public async Task AddExchangeAsync_AddsEntity_ReturnsEntity()
        {
            // Arrange
            var exchangeEntity = new ExchangeEntity { ExchangeName = "NewExchange" };
            _mockExchangeRepository.Setup(repo => repo.AddExchangeAsync(exchangeEntity))
                .ReturnsAsync(exchangeEntity);

            // Act
            var result = await _exchangeService.AddExchangeAsync(exchangeEntity);

            // Assert
            Assert.That(exchangeEntity, Is.EqualTo(result));
        }
    }
}
