using DotNetCore.ViewModel.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Areas.WS.Data
{
    public class DataUpdateEventArgs:EventArgs
    {
        public DataUpdateEventArgs(string tagName, TagDataPush tagData)
        {
            TagName = tagName;
            TagData = tagData;
        }
        public string TagName { get; private set; }
        public TagDataPush TagData { get; private set; }
    }


    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            this.Error = exception;
        }
        public Exception Error { get; private set; }
    }
}
