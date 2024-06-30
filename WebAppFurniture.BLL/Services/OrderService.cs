using AutoMapper;

using WebAppFurniture.BLL.DTO;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        public OrderService(IMapper mapper, IOrderRepository prderRepository,
            IProductRepository productRepository, IProductGroupRepository productGroupRepository, IProviderRepository providerRepository)
        {
            _mapper = mapper;
            _orderRepository = prderRepository;
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
            _providerRepository = providerRepository;
        }
        public async Task<OrderDTO> CreateAsync(OrderDTO entity)
        {
            var mappedEntity = _mapper.Map<Order>(entity);
            await _orderRepository.CreateAsync(mappedEntity);
            return entity;
        }

        public async Task<OrderDTO> DeleteAsync(int id)
        {
            var existingEntity = await _orderRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"not found.");
            }
            await _orderRepository.DeleteAsync(_mapper.Map<Order>(existingEntity));
            return _mapper.Map<OrderDTO>(existingEntity);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var mapped = _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.GetAllAsync());
            return mapped;
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<OrderDTO>(await _orderRepository.GetByIdAsync(id));
            if (entity == null)
            {
                throw new ArgumentException("not found");
            }
            var mapped = _mapper.Map<OrderDTO>(entity);
            return mapped;
        }

        public async Task<IEnumerable<CategorySalesStatisticsDTO>> GetCategorySalesStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var mappedOrders = _mapper.Map<IEnumerable<Order>>(await _orderRepository.GetAllAsync());

            foreach (var order in mappedOrders)
            {
                order.Product = await _productRepository.GetByIdAsync(order.ProductId);
                order.Product.Provider = await _providerRepository.GetByIdAsync(order.Product.ProviderId);
                order.Product.ProductGroup = await _productGroupRepository.GetByIdAsync(order.Product.ProductGroupId);
            }

            //var categorySalesStatistics = mappedOrders
            //    .Where(o => o.Date >= startDate && o.Date <= endDate)
            //    .GroupBy(o => o.Product.ProductGroup.Name)
            //    .Select(g => new CategorySalesStatisticsDTO
            //    {
            //        CategoryName = g.Key,
            //        TotalSales = (decimal)g.Sum(o => o.TotalCost)
            //    })
            //    .OrderByDescending(c => c.TotalSales);

            var categorySalesStatistics = mappedOrders
                    .Where(o => o.Date >= startDate && o.Date <= endDate && o.Status != "Отменен" && o.Product.Type != "под заказ")
                    .GroupBy(o => o.Product.ProductGroup.Name)
                    .Select(g => new CategorySalesStatisticsDTO
                    {
                        CategoryName = g.Key,
                        TotalSales = g.Count()
                    })
                    .OrderByDescending(c => c.TotalSales);

            return categorySalesStatistics;
        }

        public async Task<IEnumerable<CanceledSalesStatisticsDTO>> GetCanceledSalesStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var orders = _mapper.Map<IEnumerable<Order>>(await _orderRepository.GetAllAsync());

            var canceledOrders = orders.Where(o => o.Status == "Отменен");
            foreach (var order in canceledOrders)
            {
                order.Product = await _productRepository.GetByIdAsync(order.ProductId);
                order.Product.Provider = await _providerRepository.GetByIdAsync(order.Product.ProviderId);
                order.Product.ProductGroup = await _productGroupRepository.GetByIdAsync(order.Product.ProductGroupId);
            }
            var canceledSalesStatistics = canceledOrders
                 .Where(o => o.Date >= startDate && o.Date <= endDate)
                 .GroupBy(o => new { o.Product.Name, ProductGroupName = o.Product.ProductGroup?.Name })
                .Select(g => new CanceledSalesStatisticsDTO
                {
                    ProductName = g.Key.Name,
                    ProductGroupName = g.Key.ProductGroupName,
                    CountCanceled = g.Count()
                });

            return canceledSalesStatistics;
        }
        public async Task<double> GetTotalCostStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var orders = _mapper.Map<IEnumerable<Order>>(await _orderRepository.GetAllAsync());
            foreach (var order in orders) order.Product = await _productRepository.GetByIdAsync(order.ProductId);
            double totalCost = orders
                .Where(order => order.Date >= startDate && order.Date <= endDate && order.Product.Type!="под заказ")
                .Sum(order => order.TotalCost);
            return totalCost;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientIdAsync(int id)
        {
            var allOrders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.GetAllAsync());
            return allOrders.Where(o => o.ClientId == id);
        }

        public async Task<IEnumerable<ProviderSalesStatisticsDTO>> GetProviderSalesStatisticsAsync(DateTime StartDate, DateTime EndDate)
        {
            var mappedOrders = _mapper.Map<IEnumerable<Order>>(await _orderRepository.GetAllAsync());

            foreach (var order in mappedOrders)
            {
                var product = await _productRepository.GetByIdAsync(order.ProductId);
                order.Product = product;
                var provider = await _providerRepository.GetByIdAsync(order.Product.ProviderId);
                order.Product.Provider = provider;
                var productgroup = await _productGroupRepository.GetByIdAsync(order.Product.ProductGroupId);
                order.Product.ProductGroup = productgroup;
            }
            var providerSalesStatistics = mappedOrders
                .Where(o => o.Date >= StartDate && o.Date <= EndDate)
                .GroupBy(o => o.Product.Provider.Name)
                .Select(g => new ProviderSalesStatisticsDTO
                {
                    ProviderName = g.Key,
                    TotalSales = (decimal)g.Sum(o => o.TotalCost)
                })
                .OrderByDescending(c => c.TotalSales);
            return providerSalesStatistics;
        }

        public async Task<OrderDTO> UpdateAsync(OrderDTO entity)
        {
            var existing = await _orderRepository.GetByIdAsync(entity.Id);
            if (existing == null)
            {
                throw new ArgumentException("not found");
            }
            existing.ClientId = entity.ClientId;
            existing.ProductId = entity.ProductId;
            existing.Date = entity.Date;
            existing.Status = entity.Status;
            existing.TotalCost=entity.TotalCost;
            existing.ProgressCount = entity.ProgressCount;
            try
            {
                await _orderRepository.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _mapper.Map<OrderDTO>(existing);
        }
    }
}
