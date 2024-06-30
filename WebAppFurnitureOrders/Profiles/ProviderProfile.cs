using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ProviderProfile:Profile
    {
        public ProviderProfile()
        {
            CreateMap<ProviderDTO, ProviderModel>().ReverseMap();
        }
    }
}
