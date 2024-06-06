using Microsoft.EntityFrameworkCore;
using Sale.Data;
using Sale.Entities;
using Sale.Repository.IRepository;
using Stock.HandlerRabbit;



namespace Sale.Repository
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RabbitMQService _rabbitMqService;

        public SalesRepository(ApplicationDbContext dbContext, RabbitMQService rabbitMqService)
        {
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
        }
        public async Task<Sales> CreateSale(Sales sales)
        {
            if (sales == null)
            {
                throw new ArgumentNullException(nameof(sales));
            }

            var sale = 0;
            var stock = 1;


            if (stock <= 0 || sales.Amount > stock)
            {
                throw new Exception($"The amount {sales.Amount} is greater than the stock {stock}, the sell is not possible");
            }
            var finalStock = stock - sales.Amount;


            await _dbContext.Sales.AddAsync(sales);
            await _dbContext.SaveChangesAsync();

            //Publish the event to update the stock sales.ProductId, finalStock
            _rabbitMqService.PublishStock(sales.ProductId, finalStock);


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