using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper
{
    [Serializable()]
    public class DeviceNotConnectedException : Exception
    {
        public DeviceNotConnectedException() : base() { }
        public DeviceNotConnectedException(string message) : base(message) { }
        public DeviceNotConnectedException(string message, System.Exception inner) : base(message, inner) { }

        protected DeviceNotConnectedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
