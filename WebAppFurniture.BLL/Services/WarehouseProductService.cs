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
    public class WarehouseProductService:IWarehouseProductService
    {
        private readonly IWarehouseProductRepository _warehouseProductRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseProductService(IMapper mapper, IWarehouseProductRepository warehouseProductRepository, IWarehouseRepository warehouseRepository)
        {
            _mapper = mapper;
            _warehouseProductRepository = warehouseProductRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<WarehouseProductDTO> CreateAsync(WarehouseProductDTO entity)
        {
            var mappedEntity = _mapper.Map<WarehouseProduct>(entity);
            await _warehouseProductRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<WarehouseProductDTO> DeleteAsync(int id)
        {
            var existingEntity = await _warehouseProductRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"not found.");
            }
            await _warehouseProductRepository.DeleteAsync(_mapper.Map<WarehouseProduct>(existingEntity));
            return _mapper.Map<WarehouseProductDTO>(existingEntity);
        }

        public async Task<IEnumerable<WarehouseProductDTO>> GetAllAsync()
        {
            var mapped = _mapper.Map<IEnumerable<WarehouseProductDTO>>(await _warehouseProductRepository.GetAllAsync());
            return mapped;
        }

        public async Task<WarehouseProductDTO> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<WarehouseProductDTO>(await _warehouseProductRepository.GetByIdAsync(id));
            if (entity == null)
            {
                throw new ArgumentException("not found");
            }
            var mapped = _mapper.Map<WarehouseProductDTO>(entity);
            return mapped;
        }

        public async Task<List<string>> GetImagesByWarehouseIdAsync(int warehouseId)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            if (warehouse != null)
            {
                var imageURLs = new List<string>();
                imageURLs.Add(warehouse.ImageUrl);
                return imageURLs;
            }
            else
            {
                return new List<string>();
            }
        }

        public async Task<WarehouseProductDTO> UpdateAsync(WarehouseProductDTO entity)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{entity.Id} not found.");
            }
            var existingEntity = await _warehouseProductRepository.GetByIdAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"{entity.Id} not found.");
            }

            existingEntity.WarehouseNumber = entity.WarehouseNumber;
            existingEntity.WarehouseId = entity.WarehouseId;
            existingEntity.ProductId = entity.ProductId;

            _warehouseProductRepository.Detach(existingEntity);

            await _warehouseProductRepository.UpdateAsync(existingEntity);

            return _mapper.Map<WarehouseProductDTO>(existingEntity);
        }
        public async Task<IEnumerable<WarehouseProductDTO>> GetWarehouseProducts(int warehouseNumber)
        {
            var warehouseProducts = _mapper.Map<IEnumerable<WarehouseProductDTO>>(await _warehouseProductRepository.GetAllAsync());
            var result = warehouseProducts.Where(p => p.WarehouseNumber == warehouseNumber).ToList();
            return result;
        }
        public async Task<WarehouseProductDTO> GetWarehouseProduct(int productId ,int warehouseNumber)
        {
            var warehouseProducts = _mapper.Map<IEnumerable<WarehouseProductDTO>>(await _warehouseProductRepository.GetAllAsync());
            var result = warehouseProducts.FirstOrDefault(p => p.WarehouseNumber == warehouseNumber && p.ProductId == productId);
            return _mapper.Map<WarehouseProductDTO>(result);
        }
        public async Task<WarehouseProductDTO> GetWarehouseProductByEntityAsync(WarehouseProductDTO warehouse)
        {
            var all = await _warehouseProductRepository.GetAllAsync();
            var Warehouse = all.FirstOrDefault(p => p.WarehouseId==warehouse.WarehouseId && p.ProductId==warehouse.ProductId && p.WarehouseNumber==warehouse.WarehouseNumber);
            var mapped = _mapper.Map<WarehouseProductDTO>(Warehouse);
            return mapped;
        }
    }
}
