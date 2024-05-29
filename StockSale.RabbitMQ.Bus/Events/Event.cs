﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSale.RabbitMQ.Bus.Events
{
    public abstract class Event
    {
        public DateTime Timestamp { get; protected set; }
        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}