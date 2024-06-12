using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Stock.HandlerRabbit
{
    public class RabbitMQService
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private IConnection _connection;
        private IModel _channel;
        private readonly IMediator _mediator; // Add a reference to MediatR
        private readonly IServiceProvider _serviceProvider;
        public RabbitMQService(string hostname, string queueName, IServiceProvider serviceProvider)
        {
            _hostname = hostname;
            _queueName = queueName;
            _serviceProvider = serviceProvider;
            CreateConnection();
            StartListening(); // Start listening for messages when creating the connection
       
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(30),
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                AutomaticRecoveryEnabled = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void PublishStock(int productId, int finalStock)
        {
            var stockUpdateEvent = new StockUpdateEvent
            {
                ProductId = productId,
                FinalStock = finalStock
            };

            var message = JsonConvert.SerializeObject(stockUpdateEvent);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: _queueName,
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }

        // Private method to start listening for messages
        private async void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockUpdateEvent = JsonConvert.DeserializeObject<StockUpdateEvent>(message);

                // Create a new scope to resolve IMediator
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(stockUpdateEvent);
                }
            };

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: true,
                                  consumer: consumer);

            // Log to verify the event subscription
            Console.WriteLine(" [x] StartListening - OnMessageReceived subscribed");
        }


        // Method to release resources
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }

    public class StockUpdateEvent : INotification
    {
        public int ProductId { get; set; }
        public int FinalStock { get; set; }
    }
}
