using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.Infrastruct.Extensions;
using DotNetCore.ViewModel.WebSocket;

namespace DotNetCore.Api.Areas.WS.Data
{
    public class WsReceiver
    {
        readonly WebSocket _webSocket;
        public event EventHandler OnClose;
        public event EventHandler OnError;
        public event EventHandler<WsRequest<AuthRequestContent>> OnAuth;
        public event EventHandler OnHeartbeat;
        public event EventHandler<WsRequest<SubscribeRequestContent>> OnSubscribe;
        public event EventHandler<WsRequest<SubscribeRequestContent>> OnUnSubscribe;

        public WsReceiver(WebSocket webSocket)
        {
            this._webSocket = webSocket;
        }

        public async Task Listen()
        {
            try
            {
                while (this._webSocket.State == WebSocketState.Open)
                {
                    await ReceiveMsg();
                }
            }
            catch(Exception e)
            {
                if (this.OnError != null) this.OnError(this,null);
            }
        }

        private async Task ReceiveMsg()
        {
            var buffer = new byte[1024 * 4];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            //关闭socket请求
            if (result.CloseStatus != null)
            {
                if (this.OnClose != null) this.OnClose(this,null);
            }
            
            var obj = GetReceiveObj<WsRequest<object>>(buffer);

            if (obj.reqType == RequestType.request.ToString() && this.OnAuth != null)
            {//认证请求
                //var context = obj.content.ToObject<AuthRequestContent>();
                var context = (AuthRequestContent)obj.content;
                var req = new WsRequest<AuthRequestContent> { reqID = obj.reqID, reqType = obj.reqType, content = context };
                this.OnAuth(this, req);
            }
            else if (obj.reqType == RequestType.heartbreak.ToString() && this.OnHeartbeat != null)
            {//心跳包
                this.OnHeartbeat(this, null);
            }
            else if (obj.reqType == RequestType.subscribe.ToString() && this.OnSubscribe != null)
            {//订阅请求
                //var context = obj.content.ToObject<SubscribeRequestContent>();
                var context = (SubscribeRequestContent)obj.content;
                var req = new WsRequest<SubscribeRequestContent> { reqID = obj.reqID, reqType = obj.reqType, content = context };
                this.OnSubscribe(this, req);
            }
            else if (obj.reqType == RequestType.unsubscribe.ToString() && this.OnUnSubscribe != null)
            {//取消订阅请求
                //var context = obj.content.ToObject<SubscribeRequestContent>();
                var context = (SubscribeRequestContent)obj.content;
                var req = new WsRequest<SubscribeRequestContent> { reqID = obj.reqID, reqType = obj.reqType, content = context };
                this.OnUnSubscribe(this, req);
            }

        }

        private T GetReceiveObj<T>(byte[] buffer)
        {
            string str = buffer.Byte2String();
            return str.ToObject<T>();
        }


        private bool TryTranToObj<T>(string json,out T obj)
        {
            try
            {
                obj = json.ToObject<T>();
                return true;
            }
            catch(Exception e)
            {
                obj = default(T);
                return false;
            }
        }

    }
}
