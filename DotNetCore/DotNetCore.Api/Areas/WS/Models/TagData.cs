using DotNetCore.ViewModel.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Areas.WS.Models
{
    public class TagData
    {
        public string ObjectName { get; set; }

        public string PropertyName { get; set; }

        public long TimeStamp { get; set; }

        public TagDataStatus Status { get; set; }

        public string Value { get; set; }

        public string TagName
        {
            get
            {
                return $"{this.ObjectName}-{this.PropertyName}";
            }
        }

        public static string GetTagName(string objName,string proName)
        {
            return $"{objName}-{proName}";
        }
    }
}
