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
    public class ProviderService:IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        public ProviderService(IMapper mapper, IProviderRepository providerRepository)
        {
            _mapper = mapper;
            _providerRepository = providerRepository;
        }

        public async Task<ProviderDTO> CreateAsync(ProviderDTO entity)
        {
            await _providerRepository.CreateAsync(_mapper.Map<Provider>(entity));
            return entity;
        }

        public async Task<ProviderDTO> DeleteAsync(int id)
        {
            var existingEntity = await _providerRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _providerRepository.DeleteAsync(_mapper.Map<Provider>(existingEntity));
            return _mapper.Map<ProviderDTO>(existingEntity);
        }

        public async Task<IEnumerable<ProviderDTO>> GetAllAsync()
        {
            var products = await _providerRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProviderDTO>>(products);
            return mapped;
        }

        public async Task<ProviderDTO> GetByIdAsync(int id)
        {
            var reservation = _mapper.Map<ProviderDTO>(await _providerRepository.GetByIdAsync(id));
            if (reservation == null)
            {
                throw new ArgumentException("User not found");
            }
            var entity = _mapper.Map<ProviderDTO>(reservation);
            return entity;
        }

        public Task<ProviderDTO> UpdateAsync(ProviderDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
