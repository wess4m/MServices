using AutoMapper;
using CouponAPI.Models.Dto;

namespace CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Models.Coupon>();
                config.CreateMap<Models.Coupon, CouponDto>();
            }, new LoggerFactory());
        }
    }
}
