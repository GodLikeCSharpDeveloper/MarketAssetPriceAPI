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
    internal class InstrumentRepositoryTests : IDisposable
    {
        DbContextOptions<MarketDbContext> _dbContextOptions;
        private MarketDbContext dbContext;
        InstrumentRepository repository;
        [SetUp]
        public void Setup()
        {
            var dbName = $"TestDatabase_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<MarketDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            dbContext = new MarketDbContext(_dbContextOptions);
            repository = new InstrumentRepository(dbContext);
        }
        [Test]
        public async Task AddNewInstrument_ReturnsSameObjectAsParameterWithAssignedId()
        {
            // Arrange
            var parameters = new InstrumentEntity
            {
                TickSize = 1,
                ApiProviderId = "TestId",
                BaseCurrency = "USD",
                Currency = "USD",
                Description = "testDescription",
                Kind = "testKind",
            };
            var expectedData = new InstrumentEntity
            {
                TickSize = 1,
                ApiProviderId = "TestId",
                BaseCurrency = "USD",
                Currency = "USD",
                Description = "testDescription",
                Kind = "testKind"
            };

            // Act
            var result = await repository.AddNewInstrument(parameters);

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.AtLeast(1));
                Assert.That(result.ApiProviderId, Is.EqualTo(expectedData.ApiProviderId));
                Assert.That(result.TickSize, Is.EqualTo(expectedData.TickSize));
                Assert.That(result.BaseCurrency, Is.EqualTo(expectedData.BaseCurrency));
                Assert.That(result.Currency, Is.EqualTo(expectedData.Currency));
                Assert.That(result.Description, Is.EqualTo(expectedData.Description));
                Assert.That(result.Kind, Is.EqualTo(expectedData.Kind));
            });
        }
        [Test]
        public async Task AddNewInstruments_ReturnsSameObjectAsParameterWithAssignedId()
        {
            // Arrange
            var parameters = new List<InstrumentEntity> {
                new()
                {
                    TickSize = 1,
                    ApiProviderId = "TestId",
                    BaseCurrency = "USD",
                    Currency = "USD",
                    Description = "testDescription",
                    Kind = "testKind"
                },
                new()
                {
                    TickSize = 2,
                    ApiProviderId = "TestId2",
                    BaseCurrency = "USD2",
                    Currency = "USD2",
                    Description = "testDescription2",
                    Kind = "testKind2"
                }
            };
            var expectedData = parameters.ToList();

            // Act
            var result = await repository.AddNewInstruments(parameters);

            result.ForEach(res =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(res, Is.Not.Null);
                    Assert.That(res.Id, Is.AtLeast(1));
                    Assert.That(expectedData.Any(e=>e.ApiProviderId==res.ApiProviderId), Is.True);
                    Assert.That(expectedData.Any(e => e.TickSize == res.TickSize), Is.True);
                    Assert.That(expectedData.Any(e => e.BaseCurrency == res.BaseCurrency), Is.True);
                    Assert.That(expectedData.Any(e => e.Currency == res.Currency), Is.True);
                    Assert.That(expectedData.Any(e => e.Description == res.Description), Is.True);
                    Assert.That(expectedData.Any(e => e.Kind == res.Kind), Is.True);
                });
            });
        }
        [Test]
        public async Task GetInstrumentsByApiProviderIds_ReturnsInstrumentsList_WithParametersContainsProviderIdProperty()
        {
            // Arrange
            var parameters = new List<string> { "TestId", "TestId2" };
            var expectedData = parameters.ToList();
            var seedData = new List<InstrumentEntity> {
                new()
                {
                    TickSize = 1,
                    ApiProviderId = "TestId",
                    BaseCurrency = "USD",
                    Currency = "USD",
                    Description = "testDescription",
                    Kind = "testKind"
                },
                new()
                {
                    TickSize = 2,
                    ApiProviderId = "TestId2",
                    BaseCurrency = "USD2",
                    Currency = "USD2",
                    Description = "testDescription2",
                    Kind = "testKind2"
                }
            };


            // Act
            await repository.AddNewInstruments(seedData);
            var result = await repository.GetInstrumentsByApiProviderIds(parameters);
            result.ForEach(res =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(res, Is.Not.Null);
                    Assert.That(res.Id, Is.AtLeast(1));
                    Assert.That(expectedData.Any(e => e == res.ApiProviderId), Is.True);
                });
            });
        }
        [Test]
        public async Task UpdateInstrument_UpdatesExistingInstrument()
        {
            // Arrange
            var instrument = new InstrumentEntity { ApiProviderId = "1", Symbol = "SYM1" };
            await dbContext.Instruments.AddAsync(instrument);
            await dbContext.SaveChangesAsync();

            instrument.Symbol = "NEW_SYM1";

            // Act
            var result = await repository.UpdateInstrument(instrument);

            // Assert
            var updatedInstrument = await dbContext.Instruments.FindAsync(result.Id);
            Assert.Multiple(() =>
            {
                Assert.That(updatedInstrument, Is.Not.Null);
                Assert.That(updatedInstrument.Symbol, Is.EqualTo("NEW_SYM1"));
            });
        }

        [Test]
        public async Task UpdateInstruments_UpdatesExistingInstruments()
        {
            // Arrange
            var instruments = new List<InstrumentEntity>
            {
                new InstrumentEntity { ApiProviderId = "1", Symbol = "SYM1" },
                new InstrumentEntity { ApiProviderId = "2", Symbol = "SYM2" }
            };
            await dbContext.Instruments.AddRangeAsync(instruments);
            await dbContext.SaveChangesAsync();

            instruments[0].Symbol = "NEW_SYM1";
            instruments[1].Symbol = "NEW_SYM2";

            // Act
            var result = await repository.UpdateInstruments(instruments);

            // Assert
            var updatedInstruments = await dbContext.Instruments.ToListAsync();
            Assert.Multiple(() =>
            {
                Assert.That(updatedInstruments.Count, Is.EqualTo(2));
                Assert.That(updatedInstruments.Any(i => i.ApiProviderId == "1" && i.Symbol == "NEW_SYM1"), Is.True);
                Assert.That(updatedInstruments.Any(i => i.ApiProviderId == "2" && i.Symbol == "NEW_SYM2"), Is.True);
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
