using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class ReviewModelProfile : Profile
    {
        public ReviewModelProfile()
        {
            CreateMap<ReviewDTO, ReviewModel>().ReverseMap();
        }
    }
}
