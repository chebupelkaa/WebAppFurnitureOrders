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
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWarehouseProductRepository _warehouseProductRepository;
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        public WarehouseService(IMapper mapper, IWarehouseRepository warehouseRepository, IWarehouseProductRepository warehouseProductRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _warehouseProductRepository = warehouseProductRepository;
            _productRepository = productRepository;
        }

        public async Task<WarehouseDTO> CreateAsync(WarehouseDTO entity)
        {
            var mappedEntity = _mapper.Map<Warehouse>(entity);
            await _warehouseRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<WarehouseDTO> DeleteAsync(int id)
        {
            var existingEntity = await _warehouseRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"not found.");
            }
            await _warehouseRepository.DeleteAsync(_mapper.Map<Warehouse>(existingEntity));
            return _mapper.Map<WarehouseDTO>(existingEntity);
        }

        public async Task<IEnumerable<WarehouseDTO>> GetAllAsync()    
        {
            var mapped = _mapper.Map<IEnumerable<WarehouseDTO>>(await _warehouseRepository.GetAllAsync());
            return mapped;
        }

      
        public async Task<WarehouseDTO> GetByIdAsync(int id)
        {
            var warehouse = _mapper.Map<WarehouseDTO>(await _warehouseRepository.GetByIdAsync(id));
            if (warehouse == null)
            {
                throw new ArgumentException("not found");
            }
            var entity = _mapper.Map<WarehouseDTO>(warehouse);
            return entity;
        }

        public async Task<WarehouseDTO> UpdateAsync(WarehouseDTO entity)
        {
            //var existingEntity = await _warehouseRepository.GetByIdAsync(entity.Id);
            if (entity == null)
            {
                throw new ArgumentException($"{entity.Id} not found.");
            }
            else await _warehouseRepository.UpdateAsync(_mapper.Map<Warehouse>(entity));
            //existingEntity.Color = entity.Color;
            //existingEntity.Material = entity.Material;
            //existingEntity.Count = entity.Count;
            //existingEntity.ImageUrl = entity.ImageUrl;
            //existingEntity.WarehouseProducts = null;


            return _mapper.Map<WarehouseDTO>(entity);
        }


        public async Task<IEnumerable<WarehouseDTO>> GetWarehouses(int warehouseNumber)
        {
            var warehouses = _mapper.Map<IEnumerable<WarehouseDTO>>(await _warehouseRepository.GetAllAsync());
            var warehouseProducts = _mapper.Map<IEnumerable<WarehouseProductDTO>>(await _warehouseProductRepository.GetAllAsync());

            var warehouseIds = warehouseProducts
                .Where(wp => wp.WarehouseNumber == warehouseNumber)
                .Select(wp => wp.WarehouseId)
                .Distinct();

            var warehousesForNumber = warehouses.Where(w => warehouseIds.Contains(w.Id)).ToList();
            return warehousesForNumber;
        }
        public async Task<WarehouseDTO> GetWarehouseByEntityAsync(WarehouseDTO warehouse)
        {
            var all = await _warehouseRepository.GetAllAsync();
            var Warehouse = all.FirstOrDefault(p => p.Count == warehouse.Count && p.Color==warehouse.Color && p.Material==warehouse.Material && p.ImageUrl==warehouse.ImageUrl);
            var mapped = _mapper.Map<WarehouseDTO>(Warehouse);
            return mapped;
        }


    }
}
