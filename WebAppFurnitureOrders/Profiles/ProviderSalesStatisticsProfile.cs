using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ProviderSalesStatisticsProfile:Profile
    {
        public ProviderSalesStatisticsProfile()
        {
            CreateMap<ProviderSalesStatisticsDTO, ProviderSalesStatistics>().ReverseMap();
        }
    }
}
