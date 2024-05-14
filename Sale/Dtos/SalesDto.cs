using Stock.Entities;

namespace Sale.Dtos
{
    public class SalesDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime DateSale { get; set; }
        public Products Product { get; set; }
    }
}
