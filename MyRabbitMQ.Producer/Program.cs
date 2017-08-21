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
            Producer producer = new Producer();
            producer.Direct("Direct1", "我是钢铁网");

            //producer.Fanout(StaticiParam.MQ_Config.ExchangeName, lt_fanout, "我是钢铁网");
            Console.Read();
        }
    }
}
