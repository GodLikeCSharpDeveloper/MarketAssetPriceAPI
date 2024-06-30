using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    internal class InstrumentProviderRepositoryTests : IDisposable
    {
        DbContextOptions<MarketDbContext> _dbContextOptions;
        private MarketDbContext dbContext;
        InstrumentProviderRepository repository;
        [SetUp]
        public void Setup()
        {

            _dbContextOptions = new DbContextOptionsBuilder<MarketDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            dbContext = new MarketDbContext(_dbContextOptions);
            repository = new InstrumentProviderRepository(dbContext);
        }
        [Test]
        public async Task AddNewInstrumentProvider_AssignIdToMethodParameter()
        {
            // Arrange
            var parameters = new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 };
            var expectedData = new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 };

            // Act
            await repository.AddNewInstrumentProvider(parameters);
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parameters, Is.Not.Null);
                Assert.That(parameters.Id, Is.AtLeast(1));
                Assert.That(parameters.InstrumentId, Is.EqualTo(expectedData.InstrumentId));
                Assert.That(parameters.ProviderId, Is.EqualTo(expectedData.ProviderId));
            });
        }
        [Test]
        public async Task AddNewInstrumentProviders_AssignIdToMethodParameter()
        {
            // Arrange
            var parameters = new List<InstrumentProviderRelationEntity> { new() { InstrumentId = 1, ProviderId = 1 }, new() { InstrumentId = 2, ProviderId = 2 } };
            var expectedData = new List<InstrumentProviderRelationEntity> { new() { InstrumentId = 1, ProviderId = 1 }, new() { InstrumentId = 2, ProviderId = 2 } };

            // Act
            await repository.AddNewInstrumentProviders(parameters);
            // Assert
            parameters.ForEach(p => Assert.Multiple(() =>
            {
                Assert.That(p, Is.Not.Null);
                Assert.That(p.Id, Is.AtLeast(1));
                Assert.That(expectedData.Any(e=>e.InstrumentId == p.InstrumentId), Is.True);
                Assert.That(expectedData.Any(e => e.ProviderId == p.ProviderId), Is.True);
            }));
       
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
