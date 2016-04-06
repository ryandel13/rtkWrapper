using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper.internals
{
    internal class RTDAB
    {
        /**
         * 
         * DAB FUNCTIONS
         * 
         **/

        [DllImport("RTKDAB", EntryPoint = "RTDAB_SetFreqAndBW", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTDAB_SetFreqAndBW(int nFrequency, int nBandwidth);

        //Getter for current Signal Quality in percentage
        [DllImport("RTKDAB", EntryPoint = "RTDAB_GetSignalQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTDAB_GetSignalQuality(ref int pSignalQuality);

    }
}
