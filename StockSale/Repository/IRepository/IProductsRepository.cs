using Stock.Entities;

namespace Stock.Repository.IRepository
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> GetProducts();
        Task<Products> GetProductById(int id);
        Task<Products> CreateProduct(Products product);
        Task<Products> UpdateProduct(Products product);
        Task<bool> DeleteProduct(int id);
        Task<bool> SaveChanges();

    }
}
