using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<OrderDTO, OrderModel>().ReverseMap();
        }
    }
}
