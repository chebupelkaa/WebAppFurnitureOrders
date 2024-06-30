using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ClientModelProfile:Profile
    {
        public ClientModelProfile()
        {
            CreateMap<ClientDTO, ClientModel>().ReverseMap();

        }
    }
}
