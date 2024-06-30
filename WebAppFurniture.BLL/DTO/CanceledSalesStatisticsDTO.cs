using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.BLL.DTO
{
    public class CanceledSalesStatisticsDTO
    {
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public int CountCanceled { get; set; }
    }
}
