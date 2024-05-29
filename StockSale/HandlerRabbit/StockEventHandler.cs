using Stock.Repository.IRepository;
using StockSale.RabbitMQ.Bus.BusRabbit;
using StockSale.RabbitMQ.Bus.EventsQueue;

namespace Stock.HandlerRabbit
{
    public class StockEventHandler : IEventHandler<UpdateStockEvent>
    {
        private readonly IProductsRepository _productsRepository;

        public StockEventHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task Handle(UpdateStockEvent @event)
        {
            // Get the ProdictId
            var product = await _productsRepository.GetProductById(@event.ProductId);

            if (product != null)
            {
                // Update the stock
                product.Stock = @event.Stock;

                // Save the changes in the DB
                await _productsRepository.SaveChanges();
            }
        }
    }
}
