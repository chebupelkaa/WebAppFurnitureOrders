using WebAppFurniture.BLL.DTO;

namespace WebAppFurnitureOrders.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }
    }
}
