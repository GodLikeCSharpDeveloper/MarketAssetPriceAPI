using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    internal class ProviderRepositoryTests : IDisposable
    {
        DbContextOptions<MarketDbContext> _dbContextOptions;
        private MarketDbContext dbContext;
        ProviderRepository repository;
        [SetUp]
        public void Setup()
        {

            _dbContextOptions = new DbContextOptionsBuilder<MarketDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            dbContext = new MarketDbContext(_dbContextOptions);
            repository = new ProviderRepository(dbContext);
        }
        [Test]
        public async Task AddNewProvider_ReturnsSameObjectAsParameterWithAssignedId()
        {
            // Arrange
            var parameters = new ProviderEntity
            {
                DefaultOrderSize = 10,
                Exchange = "testExch",
                ProviderName = "TestName",
                Symbol = "TestSymbol",
            };
            var expectedData = new ProviderEntity
            {
                DefaultOrderSize = 10,
                Exchange = "testExch",
                ProviderName = "TestName",
                Symbol = "TestSymbol",
            };

            // Act
            var result = await repository.AddNewProvider(parameters);

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.AtLeast(1));
                Assert.That(result.DefaultOrderSize, Is.EqualTo(expectedData.DefaultOrderSize));
                Assert.That(result.Exchange, Is.EqualTo(expectedData.Exchange));
                Assert.That(result.ProviderName, Is.EqualTo(expectedData.ProviderName));
                Assert.That(result.Symbol, Is.EqualTo(expectedData.Symbol));
            });
        }
        [Test]
        public async Task AddNewProviders_ReturnSameListObjectsWithAssignedIds()
        {
            // Arrange
            var parameters = new List<ProviderEntity> {
                new()
                {
                    DefaultOrderSize = 10,
                    Exchange = "testExch",
                    ProviderName = "TestName",
                    Symbol = "TestSymbol",
                },
                new()
                {
                    DefaultOrderSize = 101,
                    Exchange = "testExch2",
                    ProviderName = "TestName2",
                    Symbol = "TestSymbol2",
                },
            };
            var expectedData = parameters.ToList();

            // Act
            var result = await repository.AddNewProviders(parameters);

            result.ForEach(res =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(res, Is.Not.Null);
                    Assert.That(res.Id, Is.AtLeast(1));
                    Assert.That(expectedData.Any(e => e.DefaultOrderSize == res.DefaultOrderSize), Is.True);
                    Assert.That(expectedData.Any(e => e.Exchange == res.Exchange), Is.True);
                    Assert.That(expectedData.Any(e => e.ProviderName == res.ProviderName), Is.True);
                    Assert.That(expectedData.Any(e => e.Symbol == res.Symbol), Is.True);
                });
            });
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
