using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ
{
    public interface IMqClient
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        bool Connected { get; set; }
        /// <summary>
        /// 连接服务
        /// </summary>
        /// <param name="config">连接信息</param>
        /// <param name="msgModel">Send:发送模式，Receive:接收模式</param>
        /// <returns></returns>
        bool Connection(ConnConfig config, EnumMsgModel msgModel);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queue">指定队列名称</param>
        /// <param name="msg">消息内容</param>
        void Send(string msg, string queue = null);
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="msgHandler"></param>
        void Receive(MsgHandler msgHandler, string queue = null);
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
    }
}
