using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public class ConnConfig
    {
        public static ConnConfig Config = null;

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// MQ服务器地址
        /// </summary>
        public Uri AMQPConnectionUrl { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        static ConnConfig()
        {
            Config = new ConnConfig();
        }
    }
}
