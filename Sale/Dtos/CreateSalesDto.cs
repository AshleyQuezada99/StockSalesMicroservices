namespace Sale.Dtos
{
    public class CreateSalesDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime DateSale { get; set; }
    }
}
