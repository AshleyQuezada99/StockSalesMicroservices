using Microsoft.EntityFrameworkCore;
using Sale.Data;
using Sale.Entities;
using Sale.Repository.IRepository;
using Sale.Data;



namespace Sale.Repository
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SalesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Sales> CreateSale(Sales sales)
        {
            if (sales == null)
            {
                throw new ArgumentNullException(nameof(sales));
            }
            var stock = _dbContext.Sales
                    .Include(s => s.Product)
                    .Where(s => s.Product.Id == sales.ProductId)
                    .Select(s => s.Product.Stock)
                    .FirstOrDefault();

            if (stock <= 0)
            {
                throw new Exception("The stock of this product is 0, the sell is not possible");
            }

            var finalStock = stock - sales.Amount;

            await _dbContext.Sales.AddAsync(sales);

            //Update the stock
            /*var saleProduct = await _dbContext.Sales.FindAsync(sales.ProductId);
            var product = saleProduct.Product;
            if (product != null)
            {
                product.Stock = finalStock;
                await _dbContext.SaveChangesAsync();
            }
            */
            return sales;
        }

        public async Task<bool> DeleteSale(int id)
        {
            var deleteId = await _dbContext.Sales.FindAsync(id);

            if (deleteId == null)
            {
                throw new Exception("The id was not found");
            }

            _dbContext.Sales.Remove(deleteId);
            return true;
        }

        public async Task<Sales> GetSaleById(int id)
        {
            var result = await _dbContext.Sales.FindAsync(id);

            if (result == null)
            {
                throw new Exception("The provided id was not found");
            }

            return result;
        }

        public async Task<IEnumerable<Sales>> GetSales()
        {
            return await _dbContext.Sales.ToListAsync();
        }

        public async Task<IEnumerable<Sales>> GetSalesByProduct(int ProductId)
        {
            var result = await _dbContext.Sales.Where(s => s.ProductId == ProductId).ToListAsync();

            if (result == null)
            {
                throw new Exception("There is not any record with the ProductId provided");
            }

            return result;
        }

        public async Task<bool> SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }

        public async Task<Sales> UpdateSale(Sales sales)
        {
            if (sales == null)
            {
                throw new Exception(nameof(sales));
            }

            var updateId = await _dbContext.Sales.FindAsync(sales.Id);

            if (updateId == null)
            {
                throw new Exception("The provided id did not match with any record in the sales");
            }

            _dbContext.Sales.Update(updateId);

            return sales;

        }
    }
}