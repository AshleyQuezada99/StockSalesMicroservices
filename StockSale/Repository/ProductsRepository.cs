using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Entities;
using Stock.Repository.IRepository;

namespace Stock.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Products> CreateProduct(Products product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            await _dbContext.Products.AddAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var deleteId = await _dbContext.Products.FindAsync(id);

            if (deleteId == null)
            {
                throw new Exception("The provided id was not found");
            }

            _dbContext.Products.Remove(deleteId);
            return true;
        }

        public async Task<Products> GetProductById(int id)
        {
            var result = await _dbContext.Products.FindAsync(id);
            if (result == null)
            {
                throw new Exception($"Product {id} not found");
            }

            return result;

        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
           return (_dbContext.SaveChanges() >= 0);
        }

        public async Task<Products> UpdateProduct(Products product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var updateId = await _dbContext.Products.FindAsync(product.Id);

            if (updateId == null)
            {
                throw new Exception("The provided id did not match with any record to update");
            }

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}
