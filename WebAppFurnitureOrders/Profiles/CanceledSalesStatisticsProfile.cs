using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class CanceledSalesStatisticsProfile:Profile
    {
        public CanceledSalesStatisticsProfile()
        {
            CreateMap<CanceledSalesStatisticsDTO, CanceledSalesStatistics>().ReverseMap();
        }
    }
}
