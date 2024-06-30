using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ProductGroupProfile:Profile
    {
        public ProductGroupProfile()
        {
            CreateMap<ProductGroupDTO, ProductGroupModel>().ReverseMap();
        }
    }
}
