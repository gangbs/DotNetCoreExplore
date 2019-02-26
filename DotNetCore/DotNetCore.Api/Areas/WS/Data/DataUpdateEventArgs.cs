using DotNetCore.Api.Areas.WS.Models;
using DotNetCore.ViewModel.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Areas.WS.Data
{
    public class DataUpdateEventArgs:EventArgs
    {
        //public DataUpdateEventArgs(string tagName, TagData tagData)
        //{
        //    TagName = tagName;
        //    TagData = tagData;
        //}

        public string TagName { get; set; }

        public TagData TagData { get; set; }
    }


    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            this.Error = exception;
        }
        public Exception Error { get; set; }
    }
}
