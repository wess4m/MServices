using AutoMapper;
using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, Models.CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDto, Models.CartDetails>().ReverseMap();
            }, new LoggerFactory());
        }
    }
}
