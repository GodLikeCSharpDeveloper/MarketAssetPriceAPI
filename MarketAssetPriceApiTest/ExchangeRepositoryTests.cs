using MarketAssetPriceAPI.Data.Controllers;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    internal class ExchangeRepositoryTests : IDisposable
    {
        DbContextOptions<MarketDbContext> _dbContextOptions;
        private MarketDbContext dbContext;
        ExchangeRepository repository;
        [SetUp]
        public void Setup()
        {

            _dbContextOptions = new DbContextOptionsBuilder<MarketDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            dbContext = new MarketDbContext(_dbContextOptions);
            repository = new ExchangeRepository(dbContext);
        }
        [Test]
        public async Task AddExchangeTest_ReturnsSameObjectAsParameterWithAssignedId()
        {
            // Arrange
            var parameters = new ExchangeEntity { ExchangeName = "testName"};
            var expectedData = new ExchangeEntity { ExchangeName = "testName" };

            // Act
            var result = await repository.AddExchangeAsync(parameters);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.AtLeast(1));
            Assert.That(result.ExchangeName, Is.EqualTo(expectedData.ExchangeName));
        }
        [Test]
        public async Task GetByName_ShouldReturnEntityWithSameName()
        {
            // Arrange
            var parameters = "testName";
            var expectedData = new ExchangeEntity { ExchangeName = "testName" };

            // Act
            var result = await repository.GetEchangeAsync(parameters);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.AtLeast(1));
            Assert.That(result.ExchangeName, Is.EqualTo(expectedData.ExchangeName));
        }
        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
