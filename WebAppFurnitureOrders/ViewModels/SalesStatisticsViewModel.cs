using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.ViewModels
{
    public class SalesStatisticsViewModel
    {
       
        public List<ProviderSalesStatistics> ProviderSalesStatistics { get; set; }
        public List<CategorySalesStatistics> CategorySalesStatistics { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalCost {  get; set; }

        public IEnumerable<string> ProviderNames { get; set; }
        public IEnumerable<decimal> ProviderSales { get; set; }
        public IEnumerable<string> CategoryNames { get; set; }
        public IEnumerable<decimal> CategorySales { get; set; }



        public List<ProductModel> TopProducts { get; set; }
        public List<ProductModel> WorstProducts { get; set; }


        public List<CanceledSalesStatistics> CanceledSalesStatistics { get; set; }
    }
}
