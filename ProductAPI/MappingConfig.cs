using AutoMapper;
using ProductAPI.Models.Dto;

namespace ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Models.Product>();
                config.CreateMap<Models.Product, ProductDto>();
            }, new LoggerFactory());
        }
    }
}
