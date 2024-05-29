using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockSale.RabbitMQ.Bus.BusRabbit;
using StockSale.RabbitMQ.Bus.Commands;
using StockSale.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace StockSale.RabbitMQ.Bus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handler;
        private readonly List<Type> _eventTypes;

        public RabbitEventBus(IMediator mediator, Dictionary<string, List<Type>> handler, List<Type> eventTypes)
        {
            _mediator = mediator;
            _handler = handler;
            _eventTypes = eventTypes;
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "rabbit-ash-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerEventType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handler.ContainsKey(eventName))
            {
                _handler.Add(eventName, new List<Type>());
            }

            if (_handler[eventName].Any(x => x.GetType() == handlerEventType))
            {
                throw new ArgumentException($"El manejador {handlerEventType.Name} fue registrado anteriormente por {eventName}");
            }

            _handler[eventName].Add(handlerEventType);

            var factory = new ConnectionFactory()
            {
                HostName = "rabbit-ash-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();


            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var nameEvent = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                if (_handler.ContainsKey(nameEvent))
                {
                    var subscriptions = _handler[nameEvent];
                    foreach (var sb in subscriptions)
                    {
                        var manejador = Activator.CreateInstance(sb);
                        if (manejador == null) continue;

                        var typeEvent = _eventTypes.SingleOrDefault(x => x.Name == nameEvent);
                        var eventoDS = JsonConvert.DeserializeObject(message, typeEvent);

                        var concretoTipo = typeof(IEventHandler<>).MakeGenericType(typeEvent);

                        await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
