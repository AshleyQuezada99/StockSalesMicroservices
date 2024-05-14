using Sale.Entities;

namespace Sale.Repository.IRepository
{
    public interface ISalesRepository
    {
        Task<IEnumerable<Sales>> GetSales();
        Task<IEnumerable<Sales>> GetSalesByProduct(int ProductId);
        Task<Sales> GetSaleById(int id);
        Task<Sales> CreateSale(Sales sales);
        Task<Sales> UpdateSale(Sales sales);
        Task<bool> DeleteSale(int id);
        Task<bool> SaveChanges();

    }
}
