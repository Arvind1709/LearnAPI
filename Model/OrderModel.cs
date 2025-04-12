using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Model
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled

        // Navigation Property - One Order has many OrderItems
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
