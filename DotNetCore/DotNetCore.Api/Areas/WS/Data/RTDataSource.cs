using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.Infrastruct.Extensions;

namespace DotNetCore.Api.Areas.WS.Data
{
    public class RTDataSource:IDisposable
    {
        public event EventHandler<TagRawData> OnMessage;
        public event EventHandler<EventArgs> OnException;

        private Timer _timer;
        private Random _rd = new Random();
        private List<string> _lstTag = new List<string>();

        public void Start()
        {
            this._timer = new Timer(this.UpdateData, null, 1000, 2000);
        }

        private List<TagRawData> GenData()
        {
            var lstData = new List<TagRawData>();
            var timestamp = DateTime.Now.ToLong();
            int status = 0;
            foreach(var tag in _lstTag)
            {
                lstData.Add(new TagRawData
                {
                    TagName = tag,
                    Timestamp = timestamp,
                    Value = _rd.Next(10, 1000).ToString(),
                    Status = status
                });
            }
            return lstData;
        }

        private void UpdateData(object state)
        {
            if (this.OnMessage == null) return;
            var lstData = GenData();
            foreach(var data in lstData)
            {
                this.OnMessage(this, data);
            }
        }

        /// <summary>
        /// 订阅位号数据
        /// </summary>
        /// <param name="lstTag"></param>
        public void Subscribe(List<string> lstTag)
        {
            if (lstTag == null || lstTag.Count == 0) return;

            //var lstSubTag= lstTag.Except(this._lstTag);//取交集

            this._lstTag = this._lstTag.Union(lstTag).ToList();//取并集

        }

        /// <summary>
        /// 取消订阅位号数据
        /// </summary>
        /// <param name="lstTag"></param>
        public void UnSubscribe(List<string> lstTag)
        {
            this._lstTag.Except(lstTag);
        }

        public void Dispose()
        {
            this._timer.Dispose();
        }
    }

    public class TagRawData
    {
        public string TagName { get; set; }

        public long Timestamp { get; set; }

        public string Value { get; set; }

        public int Status { get; set; }
    }
}
