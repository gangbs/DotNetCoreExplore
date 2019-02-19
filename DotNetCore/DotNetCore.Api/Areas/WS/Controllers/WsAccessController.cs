using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Infrastruct.Extensions;
using DotNetCore.ViewModel.WebSocket;

namespace DotNetCore.Api.Areas.WS.Controllers
{
    [Route("api/ws/access")]
    [ApiController]
    public class WsAccessController : ControllerBase
    {

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = HttpContext.WebSockets.AcceptWebSocketAsync().Result;

                await WsHandler(socket);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }


        public async Task WsHandler(WebSocket socket)
        {
            //认证消息验证
            if (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024 * 4];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var reqAuth = GetReceiveObj<WsRequest<AuthRequestContent>>(buffer);
                bool isAuth = true;
                if (isAuth)
                {//验证成功

                    string sessionId = HttpContext.Session.Id;
                    var resAuth = new WsResponse<AuthResponseContent> { resID = reqAuth.reqID, resType = RequestType.request.ToString(), content = new AuthResponseContent { auth = Convert.ToInt32(true).ToString(), sessionId = sessionId } };
                    var sendBuffer = GetSendBuffer<WsResponse<AuthResponseContent>>(resAuth);
                    await socket.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    //session相关操作，如缓存一些连接信息
                }
                else
                {//验证失败
                    var resAuth = new WsResponse<object> { resID = reqAuth.reqID, resType = RequestType.request.ToString(), content = new { auth = Convert.ToInt32(false).ToString() } };
                    var sendBuffer = GetSendBuffer<WsResponse<object>>(resAuth);
                    await socket.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "认证失败，断开socket通道", CancellationToken.None);
                }
            }

            //等待接收订阅消息
            if (socket.State == WebSocketState.Open)
            {//1.接收心跳包，2.接收订阅消息，3.响应订阅请求
                var buffer = new byte[1024 * 4];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var req = GetReceiveObj<WsRequest<object>>(buffer);
                if (req.reqType == RequestType.heartbreak.ToString())
                {//接收订阅消息
                    await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var reqSub = GetReceiveObj<WsRequest<SubscribeRequestContent>>(buffer);
                    //去订阅位号数据

                    //响应订阅请求
                    bool subFlag = true;
                    var resSub = new WsResponse<object> { resID = reqSub.reqID, resType = ResponseType.subscribe.ToString(), content = new { result = Convert.ToInt32(subFlag).ToString() } };
                    var sendBuffer = GetSendBuffer<WsResponse<object>>(resSub);
                    await socket.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {//继续接收心跳包

                }

            }

            //推送实时数据()
            while (socket.State == WebSocketState.Open)
            {//1.推送实时数据，2.

            }
        }
        
        public T GetReceiveObj<T>(byte[] buffer)
        {
            try
            {
                string str = buffer.Byte2String();
                return str.ToObject<T>();
            }
            catch(Exception exp)
            {
                return default(T);
            }
        }

        public ArraySegment<byte> GetSendBuffer<T>(T obj)
        {
            string str = obj.ToJson();
            var buffer = str.String2Byte();
            return buffer;
        }
    }
}