using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMQ.Consumer
{
    /// <summary>
    /// 消费方
    /// </summary>
    internal partial class Consumer
    {
        /// <summary>
        /// direct 消费
        /// </summary>
        /// <param name="queue"></param>
        public void DirectConsumer(string queue)
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
                //定义queue
                channel.QueueDeclare(queue, true, false, false, null);

                //@1：方法一
                //{
                //    var consumer = new QueueingBasicConsumer(channel);
                //    string BasicConsume = channel.BasicConsume(queue, false, consumer);

                //    while (true)
                //    {
                //        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                //        var body = ea.Body;
                //        var message = Encoding.UTF8.GetString(body);
                //        Console.WriteLine("Received {0}", message);

                //        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                //    }
                //}

                //@2：方法二
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var body = args.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Received {0}", message);

                        //消息应答，消费者反馈给生产者，确保已经收到，然后生产者从队列中移除
                        channel.BasicAck(args.DeliveryTag, false);
                    };

                    channel.BasicConsume(queue: queue, noAck: false, consumer: consumer);

                    //阻塞：以便消费数据
                    Console.ReadKey();
                }
            }
        }
    }
}
