using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class ParameterProductService:IParameterProductService
    {
        private readonly IParameterProductRepository _parameterProductRepository;
        private readonly IMapper _mapper;

        public ParameterProductService(IMapper mapper, IParameterProductRepository parameterProductRepository)
        {
            _mapper = mapper;
            _parameterProductRepository = parameterProductRepository;
        }

        public async Task<ParameterProductDTO> CreateAsync(ParameterProductDTO entity)
        {
            var mappedEntity = _mapper.Map<ParameterProduct>(entity);
            await _parameterProductRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<ParameterProductDTO> DeleteAsync(int id)
        {
            var existingEntity = await _parameterProductRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _parameterProductRepository.DeleteAsync(_mapper.Map<ParameterProduct>(existingEntity));
            return _mapper.Map<ParameterProductDTO>(existingEntity);
        }

        public async Task<IEnumerable<ParameterProductDTO>> GetAllAsync()
        {
            var entities = await _parameterProductRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ParameterProductDTO>>(entities);
            return mapped;
        }

        public async Task<ParameterProductDTO> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<ParameterProductDTO>(await _parameterProductRepository.GetByIdAsync(id));
            if (entity == null)
            {
                throw new ArgumentException("not found");
            }
            var mapped = _mapper.Map<ParameterProductDTO>(entity);
            return mapped;
        }

        public async Task <IEnumerable<ParameterProductDTO>> GetByProductIdAsync(int productId)
        {
            var entities = await _parameterProductRepository.GetAllAsync();
            var parameterProducts = entities.Where(p => p.ProductId == productId );
            var mapped = _mapper.Map<IEnumerable<ParameterProductDTO>>(entities);
            return mapped;
        }

        public async Task<ParameterProductDTO> GetParameterProductByParametersAsync(int productId, int parameterId)
        {
            var all = await _parameterProductRepository.GetAllAsync();
            var parameter = all.FirstOrDefault(p => p.ProductId == productId && p.ParameterId == parameterId);
            var mapped = _mapper.Map<ParameterProductDTO>(parameter);
            return mapped;
        }

        public Task<ParameterProductDTO> UpdateAsync(ParameterProductDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
