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
            List<string> lt_fanout = new List<string>() { "Fanout1", "Fanou2", "Fanout3" };
            List<string> lt_topic = new List<string>() { "topic.1", "topic.2", "topic.3", "topic.1.a" };
            List<string> lt_key = new List<string>() { "topic.#" };

            Consumer consumer = new Consumer();
            consumer.TopicConsumer("Topic", "topic.2", lt_key);

            //consumer.DirectConsumer("Direct1");

            //consumer.FanoutConsumer(StaticiParam.MQ_Config.ExchangeName, lt);
            Console.Read();
        }
    }
}
