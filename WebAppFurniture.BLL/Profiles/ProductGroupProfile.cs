using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;


namespace WebAppFurniture.BLL.Profiles
{
    public class ProductGroupProfile : Profile
    {
        public ProductGroupProfile()
        {
            CreateMap<ProductGroupDTO, ProductGroup>().ReverseMap();
        }
    }
}
