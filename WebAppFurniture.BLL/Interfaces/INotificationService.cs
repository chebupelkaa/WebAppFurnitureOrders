using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface INotificationService : IService<NotificationDTO>
    {
        public Task<IEnumerable<NotificationDTO>> GetNotificationsByClientIdAsync(int id);

        public Task<IEnumerable<NotificationDTO>> MarkAllAsReadAsync(int clientid);
    }
}
