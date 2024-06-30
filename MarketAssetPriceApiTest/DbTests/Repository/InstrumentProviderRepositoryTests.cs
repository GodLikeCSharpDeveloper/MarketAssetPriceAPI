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
        private DbContextOptions<MarketDbContext> _dbContextOptions;
        private MarketDbContext dbContext;
        private InstrumentProviderRepository repository;

        [SetUp]
        public void Setup()
        {
            var dbName = $"TestDatabase_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<MarketDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
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
            await repository.AddNewInstrumentProviders(new List<InstrumentProviderRelationEntity> { parameters });

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
            var parameters = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 },
                new InstrumentProviderRelationEntity { InstrumentId = 2, ProviderId = 2 }
            };

            var expectedData = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 },
                new InstrumentProviderRelationEntity { InstrumentId = 2, ProviderId = 2 }
            };

            // Act
            await repository.AddNewInstrumentProviders(parameters);

            // Assert
            parameters.ForEach(p => Assert.Multiple(() =>
            {
                Assert.That(p, Is.Not.Null);
                Assert.That(p.Id, Is.AtLeast(1));
                Assert.That(expectedData.Any(e => e.InstrumentId == p.InstrumentId), Is.True);
                Assert.That(expectedData.Any(e => e.ProviderId == p.ProviderId), Is.True);
            }));
        }

        [Test]
        public async Task UpdateInstrumentProviders_UpdateExistingRelations()
        {
            // Arrange
            var initialData = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 },
                new InstrumentProviderRelationEntity { InstrumentId = 2, ProviderId = 2 }
            };

            await repository.AddNewInstrumentProviders(initialData);

            initialData[0].InstrumentId = 1;
            initialData[0].ProviderId = 3;
            initialData[1].InstrumentId = 1;
            initialData[1].ProviderId = 4;

            // Act
            await repository.UpdateInstrumentProviders(initialData);

            // Assert
            var allData = await dbContext.InstrumentProviderRelations.ToListAsync();
            Assert.Multiple(() =>
            {
                Assert.That(allData, Has.Count.EqualTo(2));
                Assert.That(allData.Any(e => e.ProviderId == 3), Is.True);
                Assert.That(allData.Any(e => e.ProviderId == 4), Is.True);
            });
        }

        [Test]
        public async Task RemoveInstrumentProviders_RemoveSpecifiedRelations()
        {
            // Arrange
            var initialData = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 },
                new InstrumentProviderRelationEntity { InstrumentId = 2, ProviderId = 2 }
            };

            await repository.AddNewInstrumentProviders(initialData);

            // Act
            await repository.RemoveInstrumentProviders(initialData);

            // Assert
            var allData = await dbContext.InstrumentProviderRelations.ToListAsync();
            Assert.That(allData, Is.Empty);
        }
        [Test]
        public async Task GetInstrumentProviderByInstrumentIds_ReturnsCorrectRelations()
        {
            // Arrange
            var instrumentProviders = new List<InstrumentProviderRelationEntity>
            {
                new InstrumentProviderRelationEntity { InstrumentId = 1, ProviderId = 1 },
                new InstrumentProviderRelationEntity { InstrumentId = 2, ProviderId = 2 },
                new InstrumentProviderRelationEntity { InstrumentId = 3, ProviderId = 3 }
            };
            await dbContext.InstrumentProviderRelations.AddRangeAsync(instrumentProviders);
            await dbContext.SaveChangesAsync();

            var instrumentIds = new List<int> { 1, 3 };

            // Act
            var result = await repository.GetInstrumentProviderByInstrumentIds(instrumentIds);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.Any(ip => ip.InstrumentId == 1 && ip.ProviderId == 1), Is.True);
                Assert.That(result.Any(ip => ip.InstrumentId == 3 && ip.ProviderId == 3), Is.True);
                Assert.That(result.All(ip => instrumentIds.Contains(ip.InstrumentId)), Is.True);
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
