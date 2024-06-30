using AutoMapper;
using System.Collections.Generic;
using System.Globalization;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWarehouseProductRepository _warehouseProductRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IProductRepository productRepository,IOrderRepository orderRepository,
            IProductGroupRepository productGroupRepository, IWarehouseProductRepository warehouseProductRepository, IWarehouseRepository warehouseRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
            _warehouseRepository = warehouseRepository;
            _warehouseProductRepository = warehouseProductRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ProductDTO> CreateAsync(ProductDTO entity)
        {
            var mappedEntity = _mapper.Map<Product>(entity);
            await _productRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public Task<ProductDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return mapped;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllCatalogAsync()
        {
            var products = _mapper.Map<IEnumerable<ProductDTO>>(await _productRepository.GetAllAsync());
            foreach (var p in products)
            {
                var group = _mapper.Map<ProductGroupDTO>(await _productGroupRepository.GetByIdAsync(p.ProductGroupId));
                p.ProductGroup = group;
            }

            var filteredProducts = products.Where(p => p.Type != "под заказ");
            var mapped = _mapper.Map<IEnumerable<ProductDTO>>(filteredProducts);
            return mapped;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllFilteredAsync(string furnitureType)
        {
            var products = _mapper.Map <IEnumerable<ProductDTO>> (await _productRepository.GetAllAsync());

            foreach (var p in products)
            {
                var group = _mapper.Map<ProductGroupDTO>(await _productGroupRepository.GetByIdAsync(p.ProductGroupId));
                p.ProductGroup = group;
            }
            IEnumerable<ProductDTO> filteredProducts;
            if (furnitureType == "Другое")
            {
                filteredProducts = products.Where(p => p.ProductGroup.Name != "Шкаф" && p.ProductGroup.Name != "Комод" && 
                p.ProductGroup.Name != "Стул" && p.ProductGroup.Name != "Стол" && p.ProductGroup.Name != "Тумба" && p.ProductGroup.Name != "Диван" && p.Type != "Под заказ");
            }
            else filteredProducts = products.Where(p => p.ProductGroup.Name == furnitureType && p.Type !="под заказ");

            var mapped = _mapper.Map<IEnumerable<ProductDTO>>(filteredProducts);
            return mapped;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllSearchedAsync(string searchedText)
        {
            var products = _mapper.Map<IEnumerable<ProductDTO>>(await _productRepository.GetAllAsync());
            foreach (var p in products)
            {
                var group = _mapper.Map<ProductGroupDTO>(await _productGroupRepository.GetByIdAsync(p.ProductGroupId));
                p.ProductGroup = group;
            }
            var searchKeywords = searchedText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var lowerCaseProducts = products.Select(p =>
            {
                p.Name = p.Name.ToLower();
                p.ProductGroup.Name = p.ProductGroup.Name.ToLower();
                return p;
            });

            var searchedProducts = lowerCaseProducts.Where(p =>
            {
                foreach (var keyword in searchKeywords)
                {
                    if (!p.Name.Contains(keyword) && !p.ProductGroup.Name.Contains(keyword))
                    {
                        return false;
                    }
                }
                return true;
            });

            var res = searchedProducts.Where(p => p.Type != "под заказ");
            var mapped = _mapper.Map<IEnumerable<ProductDTO>>(res);

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            foreach (var p in mapped)
            {
                p.Name = textInfo.ToTitleCase(p.Name);
            }

            return mapped;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = _mapper.Map<ProductDTO>(await _productRepository.GetByIdAsync(id));
            if (product == null)
            {
                throw new ArgumentException("not found");
            }
            var entity = _mapper.Map<ProductDTO>(product);
            return entity;
        }

        public async Task<ProductDTO> GetProductByPrametersAsync(string Name, string Description, string Type, double Cost, int ProductGroupId, int ProviderId)
        {
            var allProduct= await _productRepository.GetAllAsync();
            var product = allProduct.FirstOrDefault(p => p.Name == Name && p.Description == Description && p.Type == Type && p.Cost==Cost && p.ProviderId==ProviderId && p.ProductGroupId== ProductGroupId);
            var mapped= _mapper.Map<ProductDTO>(product);
            return mapped;
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO entity)
        {
            var existingEntity = await _productRepository.GetByIdAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"{entity.Id} not found.");
            }
            existingEntity.Name = entity.Name;
            existingEntity.Cost = entity.Cost;
            existingEntity.Description = entity.Description;
            existingEntity.ProductGroupId = entity.ProductGroupId;
            existingEntity.ProviderId = entity.ProviderId;

            await _productRepository.UpdateAsync(existingEntity);
            return _mapper.Map<ProductDTO>(existingEntity);
        }



        //топ-n по количетсву заказов
        public async Task<IEnumerable<ProductStatsDTO>> GetTopProductsByOrderCount(DateTime StartDate, DateTime EndDate, int count)
        {
            var orders = await _orderRepository.GetAllAsync();
            foreach (var order in orders) order.Product=await _productRepository.GetByIdAsync(order.ProductId);
            
            var filteredOrders = orders.Where(o => o.Date >= StartDate && o.Date <= EndDate && o.Product.Type.ToLower() != "под заказ" );
            var groupedProducts = filteredOrders
                    .GroupBy(o => o.ProductId)
                    .Select(g => new ProductStatsDTO
                    {
                        Product = _mapper.Map<ProductDTO>(g.First().Product),
                        OrderCount = g.Count(),
                        TotalOrderAmount = g.Sum(o => o.TotalCost) 
                    })
                    .OrderByDescending(g => g.OrderCount)
                    .Take(count);

            return groupedProducts;
        }
        public async Task<IEnumerable<ProductStatsDTO>> GetWorstProductsByOrderCount(DateTime StartDate, DateTime EndDate, int count)
        {
            var orders = await _orderRepository.GetAllAsync();
            foreach (var order in orders) order.Product = await _productRepository.GetByIdAsync(order.ProductId);

            var products = await _productRepository.GetAllAsync();
            var filteredProducts = products.Where(p => !orders.Any(o => o.ProductId == p.Id) && p.Type.ToLower() != "под заказ");

            var filteredOrders = orders.Where(o => o.Date >= StartDate && o.Date <= EndDate && o.Product.Type.ToLower() != "под заказ" );

            var groupedProducts = filteredOrders
                    .GroupBy(o => o.ProductId)
                    .Select(g => new ProductStatsDTO
                    {
                        Product = _mapper.Map<ProductDTO>(g.First().Product),
                        OrderCount = g.Count(),
                        TotalOrderAmount = g.Sum(o => o.TotalCost) 
                    })
                    .Concat(filteredProducts.Select(p => new ProductStatsDTO
                    {
                        Product = _mapper.Map<ProductDTO>(p),
                        OrderCount = 0,
                        TotalOrderAmount = 0
                    }))
                    .OrderBy(g => g.OrderCount)
                    .Take(count);
            //OrderCount = g.Sum(o => o.Status.ToLower() == "отменен" ? 0 : 1),
            //            TotalOrderAmount = g.Where(o => o.Status.ToLower() != "отменен").Sum(o => o.TotalCost)
            return groupedProducts;
        }
        public async Task<IEnumerable<ProductDTO>> GetProductsByIds(IEnumerable<int> productIds)
        {
            var products = new List<ProductDTO>();
            foreach (var productId in productIds)
            {
                var product = await GetByIdAsync(productId);
                if (product != null) products.Add(product);
            }
            var productDTOs = products;
            return productDTOs;
        }


        public async Task<IEnumerable<ProductDTO>> GetWarehouseProducts(int warehouseNumber)
        {
            var warehouses = _mapper.Map<IEnumerable<WarehouseDTO>>(await _warehouseRepository.GetAllAsync());
            var products = _mapper.Map<IEnumerable<ProductDTO>>(await _productRepository.GetAllAsync());
            var warehouseProducts = _mapper.Map<IEnumerable<WarehouseProductDTO>>(await _warehouseProductRepository.GetAllAsync());

            var warehouseProductsForNumber = warehouseProducts.Where(wp => wp.WarehouseNumber == warehouseNumber);
            var productIds = warehouseProductsForNumber.Select(wp => wp.ProductId).Distinct();
            var productsForWarehouse = products.Where(p => productIds.Contains(p.Id)).ToList();

            return productsForWarehouse;
        }
    }
}
