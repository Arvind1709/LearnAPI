namespace LearnAPI.ModelDTO
{
    public class ProformaItemDto
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice => Quantity * PricePerUnit;
    }
}
