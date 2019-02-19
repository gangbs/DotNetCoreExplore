using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class WsResponse<T>
    {
        public string resID { get; set; }
        public string resType { get; set; }

        public T content { get; set; }
    }
}
