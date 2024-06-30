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
    public class ParameterService : IParameterService
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly IMapper _mapper;

        public ParameterService(IMapper mapper, IParameterRepository parameterRepository)
        {
            _mapper = mapper;
            _parameterRepository = parameterRepository;
        }

        public async Task<ParameterDTO> CreateAsync(ParameterDTO entity)
        {
            var mappedEntity = _mapper.Map<Parameter>(entity);
            await _parameterRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<ParameterDTO> DeleteAsync(int id)
        {
            var existingEntity = await _parameterRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($" not found.");
            }
            await _parameterRepository.DeleteAsync(_mapper.Map<Parameter>(existingEntity));
            return _mapper.Map<ParameterDTO>(existingEntity);
        }

        public async Task<IEnumerable<ParameterDTO>> GetAllAsync()
        {
            var mapped = _mapper.Map<IEnumerable<ParameterDTO>>(await _parameterRepository.GetAllAsync());
            return mapped;
        }

        public async Task<ParameterDTO> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<ParameterDTO>(await _parameterRepository.GetByIdAsync(id));
            if (entity == null)
            {
                throw new ArgumentException("not found");
            }
            var entityfound = _mapper.Map<ParameterDTO>(entity);
            return entityfound;
        }

        public async Task<ParameterDTO> GetParameterByNameAndValueAsync(string name, string value)
        {
            var allParameters = await _parameterRepository.GetAllAsync();
            var parameter = allParameters.FirstOrDefault(p => p.Name == name && p.Value == value);
            var mappedParameter = _mapper.Map<ParameterDTO>(parameter);
            return mappedParameter;
        }

        public Task<ParameterDTO> UpdateAsync(ParameterDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
