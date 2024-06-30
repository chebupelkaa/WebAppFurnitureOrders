using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IMapper _mapper;
        public ProductGroupService(IMapper mapper, IProductGroupRepository productGroupRepository)
        {
            _mapper = mapper;
            _productGroupRepository = productGroupRepository;
        }

        public async Task<ProductGroupDTO> CreateAsync(ProductGroupDTO entity)
        {
            await _productGroupRepository.CreateAsync(_mapper.Map<ProductGroup>(entity));
            return entity;
        }

        public async Task<ProductGroupDTO> DeleteAsync(int id)
        {
            var existingEntity = await _productGroupRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _productGroupRepository.DeleteAsync(_mapper.Map<ProductGroup>(existingEntity));
            return _mapper.Map<ProductGroupDTO>(existingEntity);
        }

        public async Task<IEnumerable<ProductGroupDTO>> GetAllAsync()
        {
            var products = await _productGroupRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProductGroupDTO>>(products);
            return mapped;
        }
        public async Task<ProductGroupDTO> FindByNameAsync(string name)
        {
            var products = await _productGroupRepository.GetAllAsync();
            var res = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            var mapped = _mapper.Map<ProductGroupDTO>(res);
            return mapped;
        }

        public async Task<ProductGroupDTO> GetByIdAsync(int id)
        {
            var reservation = _mapper.Map<ProductGroupDTO>(await _productGroupRepository.GetByIdAsync(id));
            if (reservation == null)
            {
                throw new ArgumentException("User not found");
            }
            var entity = _mapper.Map<ProductGroupDTO>(reservation);
            return entity;
        }

        public Task<ProductGroupDTO> UpdateAsync(ProductGroupDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
