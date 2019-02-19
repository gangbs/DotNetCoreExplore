using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class AuthResponseContent
    {
        public string auth { get; set; }

        public string sessionId { get; set; }
    }
}
