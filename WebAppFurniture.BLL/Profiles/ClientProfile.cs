using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Profiles
{
    public class ClientProfile:Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Client>().ReverseMap();
        }
    }
}
