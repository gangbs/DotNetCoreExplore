using DotNetCore.Api.Areas.WS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Areas.WS.Data
{
    public class TagSubObj
    {
        public string ObectName { get; set; }

        public string PropertyName { get; set; }

        public string TagName
        {
            get
            {
                return TagData.GetTagName(this.ObectName,this.PropertyName);
            }
        }
    }

    public class TagSubObjCompare : IEqualityComparer<TagSubObj>
    {
        public bool Equals(TagSubObj x, TagSubObj y)
        {
            if(x.ObectName==y.ObectName)
            {
                if(x.PropertyName==y.PropertyName)
                {
                    return true;
                }
            }
            return false;
        }
        
        public int GetHashCode(TagSubObj obj)
        {
            return obj.ObectName.GetHashCode()+obj.PropertyName.GetHashCode();
        }
    }


    public interface IRTDataProxy<TData>:IDisposable
    {
        event EventHandler<DataUpdateEventArgs> OnMessage;

        event EventHandler<EventArgs> OnException;

        void InitClient();

        DataUpdateEventArgs TranTagData(TData rawData);

        void Subscribe(List<TagSubObj> lstTag);

        void UnSubscribe(List<TagSubObj> lstTag);
    }

    public class RTDataProxy : IRTDataProxy<TagRawData>
    {
        public event EventHandler<DataUpdateEventArgs> OnMessage;
        public event EventHandler<EventArgs> OnException;

        private RTDataSource _dataSource = new RTDataSource();

        public void InitClient()
        {
            this._dataSource.OnMessage += this.MessageHandler;
            this._dataSource.OnException += this.ExceptionHandler;
            this._dataSource.Start();
        }

        public void InitClient(List<TagSubObj> lstTag)
        {
            this.InitClient();
            this.Subscribe(lstTag);
        }

        /// <summary>
        /// 订阅位号数据
        /// </summary>
        /// <param name="lstTag"></param>
        public void Subscribe(List<TagSubObj> lstTag)
        {
            //var lstTagName = lstTag.Select(x => x.TagName).ToList();
            this._dataSource.Subscribe(lstTag);
        }       

        /// <summary>
        /// 取消订阅位号数据
        /// </summary>
        /// <param name="lstTag"></param>
        public void UnSubscribe(List<TagSubObj> lstTag)
        {
            //var lstTagName = lstTag.Select(x => x.TagName).ToList();
            this._dataSource.UnSubscribe(lstTag);
        }

        /// <summary>
        /// 将订阅上来的数据转换为模块所需的格式
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public DataUpdateEventArgs TranTagData(TagRawData rawData)
        {
            var data = new DataUpdateEventArgs();
            data.TagName = TagData.GetTagName(rawData.ObjectName, rawData.PropertyName);
            data.TagData = new TagData { ObjectName=rawData.ObjectName, PropertyName=rawData.PropertyName, Status=rawData.Status, TimeStamp=rawData.Timestamp, Value=rawData.Value };
            return data;
        }

        /// <summary>
        /// 接收订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rawData"></param>
        private void MessageHandler(object sender, TagRawData rawData)
        {
            if (this.OnMessage != null)
            {
                var tagData = TranTagData(rawData);
                OnMessage(this, tagData);
            }
        }

        /// <summary>
        /// 接收异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ExceptionHandler(object sender, EventArgs args)
        {
            if (this.OnException != null)
            {
                OnException(this, args);
            }
        }

        public void Dispose()
        {
            this._dataSource.OnMessage -= this.MessageHandler;
            this._dataSource.OnException -= this.ExceptionHandler;
            this._dataSource.Dispose();
        }
    }

}
