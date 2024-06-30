using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebAppFurnitureOrders.Models
{
    public class ClientModelEdit
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IdentityUser? User { get; set; }
        public List<ReviewModel> Reviews { get; set; } = new();
        public List<OrderModel> Orders { get; set; } = new();
        public List<NotificationModel> Notifications { get; set; } = new();

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
