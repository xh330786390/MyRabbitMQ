using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMQ
{
    public class MqClient : IMqClient
    {
        #region 私有字段
        private static MqClient mqSendClient = null;
        private static MqClient mqReceiveClient = null;
        private static object objLock = new object();
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        private QueueingBasicConsumer _consumer;
        private IBasicProperties _properties;
        #endregion

        public bool Connected { get; set; }

        public MsgHandler MsgHandler = null;

        /// <summary>
        /// 获取单例实体
        /// </summary>
        /// <param name="msgModel"></param>
        /// <returns></returns>
        public static MqClient CreateMqClient(EnumMsgModel msgModel)
        {
            MqClient mqClient = null;
            if (msgModel == EnumMsgModel.Send)
            {
                if (mqSendClient == null)
                {
                    lock (objLock)
                    {
                        mqSendClient = new MqClient();
                        mqSendClient.Connection(ConnConfig.Config, EnumMsgModel.Send);
                    }
                }
                mqClient = mqSendClient;
            }
            else
            {
                if (mqReceiveClient == null)
                {
                    lock (objLock)
                    {
                        mqReceiveClient = new MqClient();
                        mqReceiveClient.Connection(ConnConfig.Config, EnumMsgModel.Receive);
                    }
                }
                mqClient = mqReceiveClient;
            }
            return mqClient;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="connConfig"></param>
        /// <returns></returns>
        public bool Connection(ConnConfig config, EnumMsgModel msgModel)
        {
            bool connect = false;
            try
            {
                _factory = new ConnectionFactory();
                _factory.VirtualHost = config.VirtualHost;
                _factory.Endpoint = new AmqpTcpEndpoint(config.AMQPConnectionUrl);

                _factory.UserName = config.UserName;
                _factory.Password = config.Password;

                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();


                _channel.QueueDeclare(config.QueueName, true, false, false, null);

                if (msgModel == EnumMsgModel.Send)
                {
                    _properties = _channel.CreateBasicProperties();
                    _properties.DeliveryMode = 2;
                }
                else
                {
                    _consumer = new QueueingBasicConsumer(_channel);
                    _channel.BasicConsume(config.QueueName, false, _consumer);
                    _channel.BasicQos(0, 1, false);
                }

                connect = true;
            }
            catch { connect = false; }
            Connected = connect;
            return connect;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="msg"></param>
        public void Send(string msg, string queue = null)
        {
            if (string.IsNullOrEmpty(queue))
            {
                queue = ConnConfig.Config.QueueName;
            }

            while (_connection == null || !_connection.IsOpen)
            {
                Connection(ConnConfig.Config, EnumMsgModel.Send);
                if (!Connected)
                {
                    Thread.Sleep(500);
                }
            }
            _channel.BasicPublish("", queue, _properties, Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue"></param>
        public void Receive(MsgHandler msgHandler, string queue = null)
        {
            if (string.IsNullOrEmpty(queue))
            {
                queue = ConnConfig.Config.QueueName;
            }

            while (_connection == null || !_connection.IsOpen)
            {
                Connection(ConnConfig.Config, EnumMsgModel.Receive);
                if (!Connected)
                {
                    Thread.Sleep(500);
                }
            }

            while (_connection != null && _connection.IsOpen)
            {
                var data = (BasicDeliverEventArgs)_consumer.Queue.Dequeue();
                var msg = Encoding.UTF8.GetString(data.Body);
                if (msgHandler != null)
                {
                    msgHandler(msg);
                }
                _channel.BasicAck(data.DeliveryTag, false);
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (_channel != null && _channel.IsOpen)
                _channel.Close();
            if (_connection != null && _connection.IsOpen)
                _connection.Close();
            _channel = null;
            _connection = null;
            Connected = false;
        }
    }
}
