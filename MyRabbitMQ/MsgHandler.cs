using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMQ
{
    /// <summary>
    /// 消息回调
    /// </summary>
    /// <param name="msg"></param>
    public delegate void MsgHandler(string msg);
}
