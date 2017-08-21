﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRabbitMQ;
using RabbitMQ.Client.Events;

namespace MyRabbitMQ.Consumer
{
    /// <summary>
    /// 消费方
    /// </summary>
    internal partial class Consumer
    {
        /// <summary>
        /// topic消费
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queue"></param>
        /// <param name="keys"></param>
        public void TopicConsumer(string exchangeName, string queue, List<string> keys)
        {
            var factory = new ConnectionFactory()
            {
                Endpoint = new AmqpTcpEndpoint(new Uri(StaticiParam.MQ_Config.AMQPConnectionUrl)),
                VirtualHost = StaticiParam.MQ_Config.VirtualHost,
                UserName = StaticiParam.MQ_Config.UserName,
                Password = StaticiParam.MQ_Config.Password
            };

            using (var channel = factory.CreateConnection().CreateModel())
            {
                //定义交换器，类型为：topic
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);

                //定义queue
                channel.QueueDeclare(queue, true, false, false, null);

                //将消息队列绑定到Exchange  
                keys.ForEach(key =>
                {
                    channel.QueueBind(queue, exchangeName, key);
                });

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, args) =>
                {
                    var body = args.Body;
                    var message = Encoding.UTF8.GetString(body);

                    //消息应答，消费者反馈给生产者，确保已经收到，然后生产者从队列中移除
                    channel.BasicAck(args.DeliveryTag, false);
                    Console.WriteLine(" [x] {0}", message);
                };

                channel.BasicConsume(queue: queue, noAck: false, consumer: consumer);

                Console.Read();
            }
        }
    }
}
