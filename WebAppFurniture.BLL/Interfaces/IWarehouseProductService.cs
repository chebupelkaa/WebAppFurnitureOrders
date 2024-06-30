using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IWarehouseProductService : IService<WarehouseProductDTO>
    {
        public Task<List<string>> GetImagesByWarehouseIdAsync(int warehouseId);

        public Task<IEnumerable<WarehouseProductDTO>> GetWarehouseProducts(int warehouseNumber);
        public Task<WarehouseProductDTO> GetWarehouseProduct(int productId, int warehouseNumber);

        public Task<WarehouseProductDTO> GetWarehouseProductByEntityAsync(WarehouseProductDTO warehouse);
    }
}
