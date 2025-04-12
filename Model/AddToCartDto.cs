namespace LearnAPI.Model
{
    public class AddToCartDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
