using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.Profiles
{
    public class NotificationModelProfile:Profile
    {
        public NotificationModelProfile()
        {
            CreateMap<NotificationDTO, NotificationModel>().ReverseMap();
        }
    }
}
