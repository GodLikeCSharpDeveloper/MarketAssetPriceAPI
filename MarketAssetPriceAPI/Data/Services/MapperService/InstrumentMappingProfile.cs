using AutoMapper;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketAssetPriceAPI.Data.Services.MapperService
{
    public class InstrumentMappingProfile : Profile
    {
        public InstrumentMappingProfile()
        {
            CreateMap<Instrument, InstrumentEntity>()
            .ForMember(dest => dest.ApiProviderId, opt => opt.MapFrom(src => src.ApiProviderId))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dest => dest.Kind, opt => opt.MapFrom(src => src.Kind))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.TickSize, opt => opt.MapFrom(src => (double?)src.TickSize))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.BaseCurrency, opt => opt.MapFrom(src => src.BaseCurrency))
            .ForMember(dest => dest.Providers, opt => opt.MapFrom(src => src.Mappings != null ? src.Mappings.Select(m => new ProviderEntity
            {
                DefaultOrderSize = m.Value.DefaultOrderSize,
                Exchange = m.Value.Exchange,
                Symbol = m.Value.Symbol,
                ProviderName = m.Key
            }).ToList() : new List<ProviderEntity>()));
        }
    }
}
