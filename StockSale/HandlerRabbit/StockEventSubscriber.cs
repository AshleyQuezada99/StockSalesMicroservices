namespace Stock.HandlerRabbit
{
    public class StockEventSubscriber
    {
        private readonly RabbitMQService _rabbitMQService;
        private readonly StockEventHandler _stockEventHandler;

        // Constructor to initialize the StockEventSubscriber
        public StockEventSubscriber(RabbitMQService rabbitMQService, StockEventHandler stockEventHandler)
        {
            _rabbitMQService = rabbitMQService;
            _stockEventHandler = stockEventHandler;

            // Subscribe to the OnMessageReceived event of RabbitMQService
            _rabbitMQService.OnMessageReceived += _stockEventHandler.HandleStockUpdateEvent;
        }

        // Method to dispose the subscriber and unsubscribe from events
        public void Dispose()
        {
            // Check if the event handler is not null before unsubscribing
            if (_rabbitMQService != null && _stockEventHandler != null)
            {
                // Unsubscribe from the OnMessageReceived event of RabbitMQService
                _rabbitMQService.OnMessageReceived -= _stockEventHandler.HandleStockUpdateEvent;
            }
        }

    }
}
