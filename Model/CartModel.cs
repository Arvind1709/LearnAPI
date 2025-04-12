using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Model
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        // Navigation Property - A Cart has many CartItems
        public ICollection<CartItemModel> CartItems { get; set; }
    }

}

