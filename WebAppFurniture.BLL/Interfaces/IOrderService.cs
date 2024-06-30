using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IOrderService : IService<OrderDTO>
    {
        public Task<IEnumerable<OrderDTO>> GetOrdersByClientIdAsync(int id);

        public Task<IEnumerable<ProviderSalesStatisticsDTO>> GetProviderSalesStatisticsAsync(DateTime StartDate, DateTime EndDate);

        public Task<IEnumerable<CategorySalesStatisticsDTO>> GetCategorySalesStatisticsAsync(DateTime StartDate, DateTime EndDate);

        public Task<IEnumerable<CanceledSalesStatisticsDTO>> GetCanceledSalesStatisticsAsync(DateTime startDate, DateTime endDate);

        public  Task<double> GetTotalCostStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}
