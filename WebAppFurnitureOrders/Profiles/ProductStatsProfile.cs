using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ProductStatsProfile:Profile
    {
        public ProductStatsProfile()
        {
            CreateMap<ProductStatsDTO, ProductStatsModel>().ReverseMap();
        }
    }
}
