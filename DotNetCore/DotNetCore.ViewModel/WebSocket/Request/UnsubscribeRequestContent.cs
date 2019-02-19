using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.ViewModel.WebSocket
{
   public class UnsubscribeRequestContent
    {
        public UnsubscribeRequestContent()
        {
            this.body = new List<UnsubscribeDevice>();
        }

        public string unsubType { get; set; }

        public List<UnsubscribeDevice> body { get; set; }
        
    }


    public class UnsubscribeDevice
    {
        public UnsubscribeDevice()
        {
            this.props = new List<DeviceProperty>();
        }

        public string deviceName { get; set; }

        public List<DeviceProperty> props { get; set; }
    }

    public class UnsubscribeDeviceProperty
    {
        public string name { get; set; }
    }

}
