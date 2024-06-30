namespace WebAppFurnitureOrders.ViewModels
{
    public class AddNewProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int ProductGroupId { get; set; }
        public int ProviderId { get; set; }
        public int Count { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string ImageURL { get; set; }
        public IFormFile ImageFile { get; set; }
        public int WarehouseNumber { get; set; }

        public int WarehouseProductId { get; set; }
        public int WarehouseId { get; set; }
    }
}
