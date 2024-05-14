using Stock.Entities;

namespace Sale.Entities
{
    public class Sales
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime DateSale { get; set; }
        public Products Product { get; set; }
    }
}
