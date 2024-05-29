using StockSale.RabbitMQ.Bus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSale.RabbitMQ.Bus.EventsQueue
{
    public class UpdateStockEvent : Event
    {
        public int ProductId { get; set; }
        public int Stock {  get; set; }
        public UpdateStockEvent(int productId, int stock)
        {
            ProductId = productId;
            Stock = stock;
        }
    }
}
