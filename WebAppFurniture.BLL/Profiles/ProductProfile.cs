using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;


namespace WebAppFurniture.BLL.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();

        }
    }
}
