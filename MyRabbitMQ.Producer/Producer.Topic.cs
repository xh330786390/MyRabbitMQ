using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Producer
{
    /// <summary>
    /// 任何发送到Topic Exchange的消息都会被转发到所有关心RouteKey中指定话题的Queue上
    ///1.这种模式需要RouteKey，要提前绑定Exchange与Queue。
    ///2.如果Exchange没有发现能够与RouteKey匹配的Queue，则会抛弃此消息。
    ///3.在进行绑定时，要提供一个该队列关心的主题，如“#.log.#”表示该队列关心所有涉及log的消息(一个RouteKey为”MQ.log.error”的消息会被转发到该队列)。
    ///4.“*”可以匹配一个标识符。“#”可以匹配0个或多个标识符。如“log.*”能与“log.warn”匹配，无法与“log.warn.timeout”匹配；但是“log.#”能与上述两者匹配。
    /// </summary>
    internal partial class Producer
    {
        public void Topic(string exchangeName, List<string> queues, List<string> keys, string body)
        {
            var factory = new ConnectionFactory()
            {
                //连接可以是：Endpoint，或HostName
                //Endpoint = new AmqpTcpEndpoint(new Uri(StaticiParam.MQ_Config.AMQPConnectionUrl)),
                HostName = "192.168.200.184",
                VirtualHost = StaticiParam.MQ_Config.VirtualHost,
                UserName = StaticiParam.MQ_Config.UserName,
                Password = StaticiParam.MQ_Config.Password
            };

            using (var channel = factory.CreateConnection().CreateModel())
            {
                //定义交换器，类型为：topic
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);

                queues.ForEach(queue =>
                {
                    //定义queue
                    channel.QueueDeclare(queue, true, false, false, null);

                    //将消息队列绑定到Exchange  
                    keys.ForEach(key =>
                    {
                        channel.QueueBind(queue, exchangeName, key);
                    });
                });
                ///发送消息
                keys.ForEach(key =>
                {
                    channel.BasicPublish(exchangeName, key, null, Encoding.UTF8.GetBytes(key + ":" + body));
                });
            }
        }
    }
}

