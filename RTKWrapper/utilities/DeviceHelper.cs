using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTKWrapperTest")]

namespace RTKWrapper.utilities
{
    internal class DeviceHelper
    {
        internal static void FindDevices()
        {
            var usbDevices = GetDevices();

            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}, DeviceStack: {3}",
                    usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description, usbDevice.Service);
            }
        }

        internal static Boolean DeviceForServiceExistent(String ServiceName)
        {
             var usbDevices = GetDevices();
             foreach (DeviceInfo usbDevice in usbDevices)
             {
                 if (usbDevice.Service != null)
                 {
                     if (usbDevice.Service.Equals(ServiceName, StringComparison.CurrentCultureIgnoreCase))
                     {
                         return true;
                     }
                 }
             }
             return false;
        }

        static List<DeviceInfo> GetDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new DeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description"),
                (string)device.GetPropertyValue("Service")
                ));
            }

            collection.Dispose();
            return devices;
        }
    }

    class DeviceInfo
    {
        public DeviceInfo(string deviceID, string pnpDeviceID, string description, string service)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.Service = service;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
        public string Service { get; private set; }
    }
}
