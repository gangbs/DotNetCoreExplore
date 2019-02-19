using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class WsRequest<T>
    {
        public string reqID { get; set; }
        public string reqType { get; set; }

        public T content { get; set; }
    }
}
