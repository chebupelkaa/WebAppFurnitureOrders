using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ParameterProfile:Profile
    {
        public ParameterProfile()
        {
            CreateMap<ParameterDTO, ParameterModel>().ReverseMap();
        }
    }
}
