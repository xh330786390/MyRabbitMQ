using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Producer
{
    /// <summary>
    /// 任何发送到Fanout Exchange的消息都会被转发到与该Exchange绑定(Binding)的所有Queue上。
    /// 1.这种模式需要提前将Exchange与Queue进行绑定，一个Exchange可以绑定多个Queue，一个Queue可以同多个Exchange进行绑定
    /// 2.这种模式不需要RouteKey
    /// 3.如果接受到消息的Exchange没有与任何Queue绑定，则消息会被抛弃。
    /// </summary>
    internal partial class Producer
    {
        public void Fanout(string exchangeName, List<string> queues, string body)
        {
            string routingKey = "";
            StaticiParam.MQ_Config.ExchangeName = "Fanout";

            var factory = new ConnectionFactory()
            {
                Endpoint = new AmqpTcpEndpoint(new Uri(StaticiParam.MQ_Config.AMQPConnectionUrl)),
                VirtualHost = StaticiParam.MQ_Config.VirtualHost,
                UserName = StaticiParam.MQ_Config.UserName,
                Password = StaticiParam.MQ_Config.Password
            };

            using (var channel = factory.CreateConnection().CreateModel())
            {
                //定义交换器，类型为：fanout
                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);

                queues.ForEach(queue =>
                {
                    //定义queue
                    channel.QueueDeclare(queue, true, false, false, null);

                    //将消息队列绑定到Exchange  
                    channel.QueueBind(queue, exchangeName, routingKey);
                });

                ///发送消息
                channel.BasicPublish(exchangeName, routingKey, null, Encoding.UTF8.GetBytes(body));
            }
        }
    }
}
