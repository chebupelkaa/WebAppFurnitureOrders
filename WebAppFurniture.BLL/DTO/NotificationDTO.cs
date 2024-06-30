using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public ClientDTO Client { get; set; }
    }
}
