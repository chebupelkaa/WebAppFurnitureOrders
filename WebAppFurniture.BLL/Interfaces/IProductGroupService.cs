using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IProductGroupService : IService<ProductGroupDTO>
    {
        public Task<ProductGroupDTO> FindByNameAsync(string name);
    }
}
