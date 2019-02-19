using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class SubscribeRequestContent
    {
        public SubscribeRequestContent()
        {
            this.body = new List<SubscribeDevice>();
        }

        public string subType { get; set; }

        public List<SubscribeDevice> body { get; set; }
    }

    public class SubscribeDevice
    {
        public SubscribeDevice()
        {
            this.props = new List<DeviceProperty>();
        }

        public string deviceName { get; set; }

        public List<DeviceProperty> props { get; set; }
    }

    public class DeviceProperty
    {
        public string name { get; set; }
        public int period { get; set; }
    }

}
