using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ.Consumer
{
    public class StaticiParam
    {
        public static ConnConfig MQ_Config = null;
        static StaticiParam()
        {
            MQ_Config = new ConnConfig();
            MQ_Config.AMQPConnectionUrl = ConfigurationManager.AppSettings["AMQPConnectionUrl"];
            MQ_Config.VirtualHost = ConfigurationManager.AppSettings["VirtualHost"];
            MQ_Config.UserName = ConfigurationManager.AppSettings["UserName"];
            MQ_Config.Password = ConfigurationManager.AppSettings["Password"];
            MQ_Config.ExchangeName = ConfigurationManager.AppSettings["ExchangeName"];
            MQ_Config.QueueName = ConfigurationManager.AppSettings["QueueName"];
        }
    }
}

