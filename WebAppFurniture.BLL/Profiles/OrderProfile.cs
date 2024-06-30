using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }
}
