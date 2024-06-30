using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDTO, ProductModel>().ReverseMap();

            //CreateMap<List<ProductDTO>, List<ProductModel>>();
            AllowNullCollections = true;
            AddGlobalIgnore("Product");
        }
    }
}
