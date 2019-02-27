using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.Api.Areas.WS.Data;
using DotNetCore.Api.Areas.WS.Models;
using DotNetCore.Infrastruct.Extensions;
using DotNetCore.ViewModel.WebSocket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Areas.WS.Controllers
{
    [Route("ws/access")]
    [ApiController]
    public class WsController : ControllerBase
    {
        private WebSocket _webSocket;
        private WsReceiver _wsReceiver;
        private DateTime _lastHearBeat;
        private Timer _hearBeatMonitor, _dataPushTimer;
        private Dictionary<string, TagData> _rTData = new Dictionary<string, TagData>();
        private RTDataProxy _rTDataProxy = new RTDataProxy();
        private bool _isAuth = false;

        public WsController()
        {
            
        }


        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                this._webSocket = HttpContext.WebSockets.AcceptWebSocketAsync().Result;
                var buffer = new byte[1024 * 4];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                this._wsReceiver = new WsReceiver(this._webSocket);
                this._rTDataProxy.InitClient();
                await WsHandler();
            }

            //取消订阅

            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        public async Task WsHandler()
        {
            this._rTDataProxy.OnMessage += this.OnDataUpdate;
            this._wsReceiver.OnAuth += this.AuthHandler;
            this._wsReceiver.OnClose += this.CloseHandler;
            this._wsReceiver.OnError += this.ErrorHandler;
            this._wsReceiver.OnHeartbeat += this.HearBeatHandler;
            this._wsReceiver.OnSubscribe += this.SubscribeHandler;
            this._wsReceiver.OnUnSubscribe += this.UnSubscribeHandler;

            await this._wsReceiver.Listen();


            this._rTDataProxy.OnMessage -= this.OnDataUpdate;
            this._wsReceiver.OnAuth -= this.AuthHandler;
            this._wsReceiver.OnClose -= this.CloseHandler;
            this._wsReceiver.OnError -= this.ErrorHandler;
            this._wsReceiver.OnHeartbeat -= this.HearBeatHandler;
            this._wsReceiver.OnSubscribe -= this.SubscribeHandler;
            this._wsReceiver.OnUnSubscribe -= this.UnSubscribeHandler;
            this._rTDataProxy.Dispose();
            this._hearBeatMonitor?.Dispose();
            this._dataPushTimer?.Dispose();
        }

        private void AuthHandler(object sender, WsRequest<AuthRequestContent> args)
        {
            if(CheckToken(args.content.token))
            {//认证通过               
                this._isAuth = true;
                if (CheckSession(args.content.sessionId))
                {//session存在
                    var res = new WsResponse<object> { resID=args.reqID, resType=ResponseType.subscribe.ToString(), content=new { result=1 } };
                    this.SendMsg<WsResponse<object>>(res);//订阅成功消息
                    //立即推送位号数据
                    PushData(null);
                }
                else
                {//session不存在
                    string sessionId = HttpContext.Session.Id;
                    var res = new WsResponse<AuthResponseContent> { resID = args.reqID, resType = ResponseType.request.ToString(), content = new AuthResponseContent { auth="1", sessionId=sessionId } };
                    this.SendMsg<WsResponse<AuthResponseContent>>(res);//返回session
                }
                //心跳检测，30秒检测一次
                this._hearBeatMonitor = new Timer(this.HearBeatCheck, null, 2000, 30000);
            }
            else
            {
                this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "authentication failed", CancellationToken.None);
            }
        }

        private void HearBeatHandler(object sender, EventArgs args)
        {
            if (!this._isAuth)
            {
                this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "authentication failed", CancellationToken.None);
                return;
            }
            this._lastHearBeat = DateTime.Now;
        }

        private void SubscribeHandler(object sender, WsRequest<SubscribeRequestContent> args)
        {
            if (!this._isAuth)
            {
                this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "authentication failed", CancellationToken.None);
                return;
            }

            //订阅数据
            this.Subscribe(args.content.body);

            //响应客户端订阅
            var res = new WsResponse<object> { resID=args.reqID, resType=RequestType.subscribe.ToString(), content=new { result="1" } };
            this.SendMsg<WsResponse<object>>(res);
            
            if(this._dataPushTimer==null)
            {
                this._dataPushTimer = new Timer(this.PushData, null, 0, 2000);
            }            
        }

        private void UnSubscribeHandler(object sender, WsRequest<SubscribeRequestContent> args)
        {
            if (!this._isAuth)
            {
                this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "authentication failed", CancellationToken.None);
                return;
            }

            //订阅数据
            this.UnSubscribe(args.content.body);
        }

        private void ErrorHandler(object sender, EventArgs args)
        {

        }

        private void CloseHandler(object sender, EventArgs args)
        {
            if(this._webSocket.CloseStatus!=null)
            {
                this._webSocket.CloseAsync( WebSocketCloseStatus.NormalClosure,"receive close command",CancellationToken.None);
            }
        }

        private void HearBeatCheck(object state)
        {
            if (this._lastHearBeat == default(DateTime)) return;

            DateTime dtNow = DateTime.Now;
            if (dtNow.Subtract(this._lastHearBeat).TotalSeconds > 60)
            {
                this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "heart stop", CancellationToken.None);
            }
        }

        private bool CheckToken(string token)
        {
            return true;
        }

        private bool CheckSession(string sessionId)
        {
            if(string.IsNullOrEmpty(sessionId))
            {
                return false;
            }
            else
            {
                return true;
            }           
        }        

        private async void SendMsg<T>(T msg)
        {
            string str = msg.ToJson();
            var buffer = str.String2Byte();
            if(this._webSocket.State== WebSocketState.Open)
            {
                await this._webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private void PushData(object state)
        {
            var resData = new DataPushResponse { resType = ResponseType.push.ToString(), content = new DataPushContent { subType = "device_prop", body = new Dictionary<string, IEnumerable<TagDataPush>>() } };           
            foreach(var data in this._rTData.Values)
            {
                resData.content.body.Add($"{data.ObjectName}:{data.PropertyName}", new List<TagDataPush> { new TagDataPush { value = data.Value, status = data.Status, timestamp = data.TimeStamp } });
            }
            this.SendMsg<DataPushResponse>(resData);
        }

        private void Subscribe(List<SubscribeDevice> lstSubObj)
        {
            long timestamp = DateTime.Now.ToLong();

            List<TagSubObj> lstNeedSub = new List<TagSubObj>();
            foreach (var obj in lstSubObj)
            {
                foreach (var p in obj.props)
                {
                    var data = new TagData { ObjectName = obj.deviceName, PropertyName = p.name, TimeStamp = timestamp, Status = TagDataStatus.noValue, Value = "###" };
                    if (this._rTData.TryAdd(data.TagName, data))
                    {
                        lstNeedSub.Add(new TagSubObj { ObectName=obj.deviceName, PropertyName=p.name });
                    }
                }
            }

            //拿lstNeedSub去实时数据库订阅
            this._rTDataProxy.Subscribe(lstNeedSub);
        }

        private void UnSubscribe(List<SubscribeDevice> lstSubObj)
        {
            List<TagSubObj> lstNeedUnSub = new List<TagSubObj>();
            foreach (var obj in lstSubObj)
            {
                foreach (var p in obj.props)
                {
                    var data = new TagData { ObjectName = obj.deviceName, PropertyName = p.name };
                    if (this._rTData.Remove(data.TagName))
                    {
                        lstNeedUnSub.Add(new TagSubObj { ObectName = obj.deviceName, PropertyName = p.name });
                    }
                }
            }

            //拿lstNeedUnSub去实时数据库取消位号订阅
            this._rTDataProxy.UnSubscribe(lstNeedUnSub);
        }

        private void OnDataUpdate(object sender, DataUpdateEventArgs args)
        {
            if(this._rTData.ContainsKey(args.TagName))
            {
                this._rTData[args.TagName] = args.TagData;
            }          
        }

    }        
}