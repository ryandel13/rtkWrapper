using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper.internals
{
    public class RTKFM
    {


        //Setter for current Frequency
        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_SetFrequency", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_SetFrequency(int nFrequencyKHz);

        //Setter for desired AudioSampleRate
        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_SetAudioSampleRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_SetAudioSampleRate(uint nSampleFreqHz);

        //Getter for current Signal Quality in percentage
        [DllImport("RTKFM", EntryPoint = "RTFM_GetSignalQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_GetSignalQuality(ref int pSignalQuality);

        //Starts RDS functionality
        [DllImport("RTKFM", EntryPoint = "RTFM_StartRDS", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_StartRDS();

        //Starts RDS functionality
        [DllImport("RTKFM", EntryPoint = "RTFM_OpenDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_OpenDevice();

        //Starts RDS functionality
        [DllImport("RTKFM", EntryPoint = "RTFM_CloseDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_CloseDevice();

        //Starts RDS functionality
        [DllImport("RTKFM", EntryPoint = "RTFM_Start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_Start();

        //Stops RDS functionality
        [DllImport("RTKFM", EntryPoint = "RTFM_StopRDS", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_StopRDS();

        //GetsRDS Bytecode
        [DllImport("RTKFM", EntryPoint = "RTFM_GetRDSSync", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_GetRDSSync(ref int pRDSByte);

        //GetsRDS Bytecode
        [DllImport("RTKFM", EntryPoint = "RTFM_GetRDSQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_GetRDSQuality(ref int pRDSQuality);

        // Unused currently
        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_GetTunerRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_GetTunerRange(ref int pnLowRange, ref int pnUpperRange);

        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_GetPCMInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_GetPCMInfo(ref byte pnType, ref int pnSamplePerSec, ref int pnBitPerSample);

        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_SetDeemphasisTC", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTFM_SetDeemphasisTC(char nDeemphasis);
        
        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_SetPCMCallBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTFM_SetPCMCallBack(RTFM_NOTIFY_PCM_DATA_FUNCTION pfnPCMCallBack);

        [DllImport("RTKFM.DLL", EntryPoint = "RTFM_SetRDSCallBack", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTFM_SetRDSCallBack(RTFM_NOTIFY_RDS_DATA_FUNCTION pRDSCallBack);

        public delegate void RTFM_NOTIFY_RDS_DATA_FUNCTION(ref ushort pBuffer, char nSize, uint ErrStatus);

        public unsafe delegate void RTFM_NOTIFY_PCM_DATA_FUNCTION(short *pBuffer, uint nSize, char nMode);

        private static RTFM_NOTIFY_RDS_DATA_FUNCTION callBack;
        private static ushort pBuffer = 0;
        private static char nSize = '0';
        private static uint errStatus = 0;

        internal static int ErrStatus = 75;
        internal static int nMode;

        public static int getErrStatus()
        {
            return RTKFM.ErrStatus + RTKFM.nMode;
        }

       
        public static unsafe void PCMHandler(short *pBuffer, uint nSize, char nMode)
        {
            Console.WriteLine("OKOKOK");
            RTKFM.nMode = Int32.Parse(nMode.ToString());
        }

        public static void Handler(ref ushort pBuffer, char nSize, uint ErrStatus)
        {
            Console.WriteLine("RDS");
            RTKFM.ErrStatus = (int)ErrStatus;
        }

        public unsafe static int RTFM_SetPCMCallBackInternal()
        {
            int x = RTFM_SetPCMCallBack(PCMHandler);
            return x;
        }

        public static int RTFM_SetRDSCallBackInternal()
        {
            RTFM_NOTIFY_RDS_DATA_FUNCTION handler = new RTFM_NOTIFY_RDS_DATA_FUNCTION(Handler);


            int x = RTFM_SetRDSCallBack(handler);
            return x;
        }
    }
}
