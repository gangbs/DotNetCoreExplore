﻿using System;

namespace DotNetCore.ViewModel.WebSocket
{
    public class AuthRequestContent
    {
        public string token { get; set; }

        public string sessionId { get; set; }
    }
}
