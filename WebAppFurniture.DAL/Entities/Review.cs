using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
