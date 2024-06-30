using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ParameterProductProfile:Profile
    {
        public ParameterProductProfile()
        {
            CreateMap<ParameterProductDTO, ParameterProductModel>().ReverseMap();
        }
    }
}
