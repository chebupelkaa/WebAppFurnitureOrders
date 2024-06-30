using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.ViewModels
{
    public class WarehouseManagementViewModel
    {
        public List<WarehouseModel> Warehouses { get; set; } = new();
        public List<ProductModel> Products { get; set; } = new();
        public List<ProviderModel> Providers { get; set; } = new();
        public List<ProductGroupModel> ProductGroups { get; set; } = new();
        public int SelectedWarehouseId { get; set; }
        public List<WarehouseProductModel> WarehouseProducts { get; set; } = new();

        public AddNewProductViewModel AddProductToWarehouseViewModel { get; set; }
    }
}
