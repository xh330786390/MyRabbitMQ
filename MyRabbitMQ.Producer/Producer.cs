using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Producer
{
    /// <summary>
    /// 生产方
    /// RabbitMQ提供了四种Exchange模式：fanout,direct,topic,header
    /// header模式在实际使用中较少，
    /// 本实例只对前三种模式进行比较。
    /// </summary>
    internal partial class Producer
    {
        public static void Send()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            //创建连接对象，基于 Socket
            using (var connection = factory.CreateConnection())
            {
                //创建新的渠道、会话
                using (var channel = connection.CreateModel())
                {
                    //声明队列
                    channel.QueueDeclare(queue: "hello",    //队列名
                        durable: false,     //持久性
                        exclusive: false,   //排他性
                        autoDelete: false,  //自动删除
                        arguments: null);

                    const string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",  //交换机名
                        routingKey: "hello",    //路由键
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
