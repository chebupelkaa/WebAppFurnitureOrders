using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.ViewModels
{
    public class OrdersViewModel
    {
        public List<CustomOrderViewModel> CustomOrders { get; set; }
        public List<OrderViewModel> RegularOrders { get; set; }
    }
}
