using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class WarehouseProfile:Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseDTO, WarehouseModel>().ReverseMap();
        }
    }
}
