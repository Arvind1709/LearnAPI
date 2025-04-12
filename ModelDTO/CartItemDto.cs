namespace LearnAPI.ModelDTO
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        // Add book details if required

        // Extra Book Details
        public string BookTitle { get; set; }
        public decimal Price { get; set; }
        public string BookCover { get; set; }
    }
}
