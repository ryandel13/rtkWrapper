using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper
{
    public class DABPlayer : IRadioPlayer
    {
        public int GetQuality()
        {
            throw new NotImplementedException();
        }

        public int GetFrequency()
        {
            throw new NotImplementedException();
        }

        public void OpenDevice()
        {
            throw new NotImplementedException();
        }

        public void CloseDevice()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SetFrequency(int freq)
        {
            throw new NotImplementedException();
        }

        public void SetBandwidth(int bandwidth)
        {
            throw new NotImplementedException();
        }

        public void InitPlayer()
        {
            throw new NotImplementedException();
        }


        public bool IsRunning()
        {
            throw new NotImplementedException();
        }


        public void SetQualityChangedCallBackHandler(RadioCallbackHandler.QualityCallBackHandler nCallback)
        {
            throw new NotImplementedException();
        }
    }
}
