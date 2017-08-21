using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lt = new List<string>() { "Q1", "Q2", "Q3" };

            Consumer consumer = new Consumer();
            consumer.DirectConsumer("Direct1");

            //consumer.FanoutConsumer(StaticiParam.MQ_Config.ExchangeName, lt);
            Console.Read();
        }
    }
}
