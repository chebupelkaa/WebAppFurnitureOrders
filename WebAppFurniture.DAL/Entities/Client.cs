using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAppFurniture.DAL.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IdentityUser? User { get; set; }
        public List<Review> Reviews { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
    }
}
 