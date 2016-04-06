using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RTKWrapper
{
    public interface IRadioPlayer
    {
       

        int GetQuality();
        int GetFrequency();

        void SetFrequency(int freq);
        void SetBandwidth(int bandwidth);

        void SetQualityChangedCallBackHandler(RadioCallbackHandler.QualityCallBackHandler nCallback);

        void OpenDevice();
        void CloseDevice();

        void Start();
        void Stop();

        Boolean IsRunning();
    }

    public class RadioCallbackHandler
    {
        public delegate void QualityCallBackHandler(int nQuality);
    }
}
