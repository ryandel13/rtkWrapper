using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper.internals
{
    class FMWorker
    {
        private Boolean running;
        private int hr = 0;
        private String rdsStream;
        private int bytes;
        private int quality;
        private RadioCallbackHandler.QualityCallBackHandler callBack;

        public void ObserveSignalStrength(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        {
            running = true;
            while (running)
            {
                int q = this.checkQuality();
                if (q != quality)
                {
                    quality = q;
                    if (callBack != null)
                    {
                        callBack.Invoke(q);
                    }
                }
                this.getRDSSync();
                worker.ReportProgress(q);
            }

        }

        public void InterruptWorker()
        {
            running = !running;
        }

        private int checkQuality()
        {
            int q = 0;
            hr = RTKFM.RTFM_GetSignalQuality(ref q);
            //Console.WriteLine("Quality: Q = " + q);
            return q;
        }

        public String getRDSStream()
        {
            return this.rdsStream;
        }

        private void getRDSSync()
        {
            byte BYTE = 0;
            int x = 0;
            int sync = 0;
            int rdsq = 0;
            hr = RTKFM.RTFM_GetSignalQuality(ref x);
            hr = RTKFM.RTFM_GetRDSQuality(ref rdsq);
            hr = RTKFM.RTFM_GetRDSSync(ref sync);

            System.Threading.Thread.Sleep(100);
            Console.WriteLine("FM Q: " +x + " RDSLINE: Q = " + rdsq + "; SYNC = " + sync);
        }

        internal void setCallBack(RadioCallbackHandler.QualityCallBackHandler nCallback)
        {
            this.callBack = nCallback;
        }
    }
}
