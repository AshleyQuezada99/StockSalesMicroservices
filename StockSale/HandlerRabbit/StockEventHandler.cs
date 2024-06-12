using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Stock.Repository.IRepository;


namespace Stock.HandlerRabbit
{
    public class StockEventHandler : INotificationHandler<StockUpdateEvent>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger<StockUpdateEvent> _logger;


        public StockEventHandler(IProductsRepository productsRepository, ILogger<StockUpdateEvent> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        public async Task Handle(StockUpdateEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling StockUpdateEvent for ProductId: {notification.ProductId}, NewStock: {notification.FinalStock}");
            try
            {
                // Get the product by its ID
                var product = await _productsRepository.GetProductById(notification.ProductId);

                if (product != null)
                {
                    // Update the stock
                    product.Stock = notification.FinalStock;

                    // Save the changes in the database
                    await _productsRepository.SaveChanges();
                    _logger.LogInformation($"Stock updated for ProductId: {notification.ProductId}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update process
                Console.WriteLine($"An error occurred while handling the stock update event: {ex.Message}");
                _logger.LogWarning($"Product with Id {notification.ProductId} not found.");
            }
           
        }
    }
}
