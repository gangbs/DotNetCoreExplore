using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
    public enum RequestType
    {
        /// <summary>
        /// 心跳包
        /// </summary>
        heartbreak=0,
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe=1,
        /// <summary>
        /// 取消订阅
        /// </summary>
        unsubscribe=2,
        /// <summary>
        /// 普通请求
        /// </summary>
        request=3
    }
}
