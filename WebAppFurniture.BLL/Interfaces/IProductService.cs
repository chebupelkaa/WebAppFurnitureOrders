using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IProductService : IService<ProductDTO>
    {
        public Task<IEnumerable<ProductDTO>> GetAllFilteredAsync(string furnitureType); 
        public Task<IEnumerable<ProductDTO>> GetAllSearchedAsync(string searchedText);
        public Task<IEnumerable<ProductDTO>> GetAllCatalogAsync();
        public Task<ProductDTO> GetProductByPrametersAsync(string Name, string Description, string Type, double Cost, int ProductGroupId,int ProviderId);
        public Task<IEnumerable<ProductDTO>> GetWarehouseProducts(int warehouseNumber);

        public Task<IEnumerable<ProductStatsDTO>> GetTopProductsByOrderCount(DateTime StartDate, DateTime EndDate, int count);
        public Task<IEnumerable<ProductStatsDTO>> GetWorstProductsByOrderCount(DateTime StartDate, DateTime EndDate, int count);
        public Task<IEnumerable<ProductDTO>> GetProductsByIds(IEnumerable<int> productIds);

    }
}
