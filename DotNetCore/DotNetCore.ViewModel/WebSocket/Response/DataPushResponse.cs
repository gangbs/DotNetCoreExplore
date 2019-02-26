using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class DataPushResponse
    {
        public string resType { get; set; }

        public DataPushContent content { get; set; }
    }


    public class DataPushContent
    {
        public string subType { get; set; }

        public Dictionary<string,IEnumerable<TagDataPush>> body { get; set; }
    }

    public class TagDataPush
    {
        public string value { get; set; }
        public long timestamp { get; set; }

        public TagDataStatus status { get; set; }

    }

    public enum TagDataStatus
    {
        normal=0,
        noValue=1
    }
}
