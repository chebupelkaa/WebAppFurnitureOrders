using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
