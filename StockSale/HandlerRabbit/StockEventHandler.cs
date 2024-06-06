using Microsoft.AspNetCore.Http.HttpResults;
using Stock.Repository.IRepository;


namespace Stock.HandlerRabbit
{
    public class StockEventHandler
    {
        private readonly IProductsRepository _productsRepository;

        public StockEventHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async void HandleStockUpdateEvent(object sender, StockUpdateEvent e)
        {
            try
            {
                // Get the product by its ID
                var product = await _productsRepository.GetProductById(e.ProductId);

                if (product != null)
                {
                    // Update the stock
                    product.Stock = e.FinalStock;

                    // Save the changes in the database
                    await _productsRepository.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update process
                Console.WriteLine($"An error occurred while handling the stock update event: {ex.Message}");
            }
        }

    }
}
