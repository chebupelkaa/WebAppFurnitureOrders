using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class WarehouseProductProfile:Profile
    {
        public WarehouseProductProfile()
        {
            CreateMap<WarehouseProductDTO, WarehouseProductModel>().ReverseMap();
        }
    }
}
