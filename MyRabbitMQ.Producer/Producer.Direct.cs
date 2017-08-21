using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Producer
{
    /// <summary>
    /// 1.RabbitMQ 默认的 Exchange，无定义交换器;
    /// 2.消息传递时需要一个“RouteKey”，可以简单的理解为要发送到的队列名字。任何发送到Direct Exchange的消息都会被转发到RouteKey中指定的Queue。
    /// 3.如果vhost中不存在RouteKey中指定的队列名，则该消息会被抛弃。
    /// </summary>
    internal partial class Producer
    {
        public void Direct(string queue, string body)
        {
            string exchangeName = "";
            var factory = new ConnectionFactory()
            {
                Endpoint = new AmqpTcpEndpoint(new Uri(StaticiParam.MQ_Config.AMQPConnectionUrl)),
                VirtualHost = StaticiParam.MQ_Config.VirtualHost,
                UserName = StaticiParam.MQ_Config.UserName,
                Password = StaticiParam.MQ_Config.Password
            };

            using (var channel = factory.CreateConnection().CreateModel())
            {
                //定义queue
                channel.QueueDeclare(queue, true, false, false, null);

                IBasicProperties properties = channel.CreateBasicProperties();
                //设置数据持久化
                properties.DeliveryMode = 2;

                ///发送消息
                channel.BasicPublish(exchangeName, queue, properties, Encoding.UTF8.GetBytes(body));
            }
        }
    }
}
