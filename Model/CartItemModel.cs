using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearnAPI.Model
{
    public class CartItemModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Cart")]
        public int CartId { get; set; }

        // Navigation Property - A CartItem belongs to one Cart
        [ForeignKey("CartId")]
        [JsonIgnore]
        public CartModel Cart { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }


        // Navigation Property
        public BookModel Book { get; set; }
    }

}
