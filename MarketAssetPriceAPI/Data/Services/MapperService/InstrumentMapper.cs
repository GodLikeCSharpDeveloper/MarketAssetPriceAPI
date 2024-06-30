using AutoMapper;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Services.MapperService
{
    public class InstrumentMapper
    {
        public static InstrumentEntity MapInstrumentEntity(Instrument instrument)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<InstrumentMappingProfile>());
            var mapper = config.CreateMapper();
            var mappedInstrument = mapper.Map<InstrumentEntity>(instrument);
            return mappedInstrument;
        }
        public static ProviderEntity MapProviderEntity(KeyValuePair<string, Mapping> provider)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProviderMappingProfile>());
            var mapper = config.CreateMapper();
            var mappedProvider = mapper.Map<ProviderEntity>(provider);
            return mappedProvider;
        }
    }
}
