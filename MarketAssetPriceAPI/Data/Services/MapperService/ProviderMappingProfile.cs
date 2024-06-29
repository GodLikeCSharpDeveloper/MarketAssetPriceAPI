using AutoMapper;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Services.MapperService
{
    public class ProviderMappingProfile : Profile
    {
        public ProviderMappingProfile()
        {
            CreateMap<KeyValuePair<string, Mapping>, ProviderEntity>().ForMember(dest=> dest.ProviderName, opt=>opt.MapFrom(src=>src.Key)).
                ForMember(dest=>dest.Exchange, opt=>opt.MapFrom(src=>src.Value.Exchange)).
                ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Value.Symbol)).
                ForMember(dest => dest.DefaultOrderSize, opt => opt.MapFrom(src => src.Value.DefaultOrderSize));
        }
    }
}
