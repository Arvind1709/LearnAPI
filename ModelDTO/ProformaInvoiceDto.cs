namespace LearnAPI.ModelDTO
{
    public class ProformaInvoiceDto
    {
        public int OrderId { get; set; }
        public string BuyerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<ProformaItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
