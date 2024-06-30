using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Profiles
{
    public class NotificationProfile:Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationDTO, Notification>().ReverseMap();
        }
    }
}
