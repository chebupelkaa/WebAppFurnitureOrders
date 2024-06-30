using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class CategorySalesStatisticsProfile:Profile
    {
        public CategorySalesStatisticsProfile()
        {
            CreateMap<CategorySalesStatisticsDTO, CategorySalesStatistics>().ReverseMap();
        }
    }
}
