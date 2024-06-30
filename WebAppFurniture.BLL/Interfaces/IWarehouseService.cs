using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IWarehouseService : IService<WarehouseDTO>
    {
        public Task<IEnumerable<WarehouseDTO>> GetWarehouses(int warehouseNumber);

        public Task<WarehouseDTO> GetWarehouseByEntityAsync(WarehouseDTO warehouse);
    }
}
