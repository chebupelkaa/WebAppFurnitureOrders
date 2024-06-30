using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вы не написали отзыв.")]
        public string Comment { get; set; }
        [Range(1, 5, ErrorMessage = "Вы не поставили оценку.")]
        public int Rating { get; set; }
        public DateTime Date { get; set; }
   
        public int ClientId { get; set; }
        [BindNever]
        public ClientModel Client { get; set; }
    }
}
