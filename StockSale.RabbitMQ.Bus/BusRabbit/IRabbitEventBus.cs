using StockSale.RabbitMQ.Bus.Commands;
using StockSale.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSale.RabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task SendCommand<T>(T command) where T: Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>() where T : Event
                                where TH : IEventHandler<T>;
    }
}
