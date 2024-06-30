using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [DisplayName("Picture")]
        public string? Picture { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public IdentityUser? User { get; set; }
        public List<ReviewModel> Reviews { get; set; } = new();
        public List<OrderModel> Orders { get; set; } = new();
        public List<NotificationModel> Notifications { get; set; } = new();

        //[Required]
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Подтвердить пароль")]
        public string ConfirmPassword { get; set; }
    }
}

