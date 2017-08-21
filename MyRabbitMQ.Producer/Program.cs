using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lt_fanout = new List<string>() { "Fanout1", "Fanou2", "Fanout3" };
            List<string> lt_topic = new List<string>() { "topic.1", "topic.2", "topic.3", "topic.1.a" };
            List<string> lt_key = new List<string>() { "topic.#" };
            Producer producer = new Producer();

            producer.Topic("Topic", lt_topic, lt_key, "我是钢铁网");

            //producer.Direct("Direct", "我是钢铁网");
            //producer.Fanout("Fanout", lt_fanout, "我是钢铁网");
            Console.Read();
        }
    }
}
