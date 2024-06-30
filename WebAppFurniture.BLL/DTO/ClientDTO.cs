using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IdentityUser? User { get; set; }

        public List<ReviewDTO> Reviews { get; set; } = new();
        public List<OrderDTO> Orders { get; set; } = new();
        public List<NotificationDTO> Notifications { get; set; } = new();
    }
}
