using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper
{
    public abstract class ARadioPlayer
    {
        protected IMediaControl mediaControl = null;

        public IBaseFilter CreateFilter(Guid category, string friendlyname)
        {
            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(category))
            {
                Console.WriteLine(device.Name);
                if (device.Name.CompareTo(friendlyname) == 0)
                {
                    //This can lead to a bluescreen! We need a serious fix here!
                    string s = device.DevicePath;
                    int i = device.Mon.IsRunning(null, null, null);
                       
                    device.Mon.BindToObject(null, null, ref iid, out source);
                    break;
                }
            }

            return (IBaseFilter)source;
        }
    }
}
