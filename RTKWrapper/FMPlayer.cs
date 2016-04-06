using DirectShowLib;
using RTKWrapper.internals;
using RTKWrapper.utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RTKWrapper
{
    public class FMPlayer : ARadioPlayer, IRadioPlayer 
    {
        

        private static String USBDEVICENAME = "RTL2832UUSB";

        private Boolean isInitialized = false;
        private Boolean isRunning = false;
        private int bytes = 0;
        byte pnType = 0;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private FMWorker fmworker = new FMWorker();
        private IGraphBuilder graphBuilder;
        private IMediaControl mediaControl;
        private int hr = 0;

        private IBaseFilter theDevice;
        private IBaseFilter theRenderer;

        private int quality = 0;

        private int frequency = 87500;

        
        

        public int GetQuality()
        {
            return this.quality;
        }

        public int GetFrequency()
        {
            return this.frequency;
        }

        public void SetFrequency(int freq)
        {
            this.frequency = freq;
            hr = RTKFM.RTFM_SetFrequency(frequency);
        }

        public void SetBandwidth(int bandwidth)
        {
            throw new InvalidOperationException();
        }

        public void OpenDevice()
        {
            throw new NotImplementedException();
        }

        public void CloseDevice()
        {
            throw new NotImplementedException();
        }

        public unsafe void Start()
        {


            hr = RTKFM.RTFM_SetRDSCallBackInternal();
            Console.WriteLine("SETCALLBACK" + hr);
            hr = RTKFM.RTFM_StartRDS();
            Console.WriteLine("STARTRDS" + hr);
            if (!DeviceHelper.DeviceForServiceExistent(USBDEVICENAME))
            {
                throw new DeviceNotConnectedException(USBDEVICENAME);
            }
            if (!isInitialized)
            {
                //Startup
               
                
                

                isInitialized = true;
                graphBuilder = null;
                //IBaseFilter theDevice = null;
                //IBaseFilter theRenderer = null;

                
                //Create the Graph
                graphBuilder = (IGraphBuilder)new FilterGraph();

                //Create the media control for controlling the graph
                mediaControl = (IMediaControl)graphBuilder;
                
                // Get the source filter and add to the graph
                theDevice = CreateFilter(FilterCategory.LegacyAmFilterCategory, (string)"RTKFMSourceFilter");
                hr = graphBuilder.AddFilter(theDevice, "source filter");
                Console.WriteLine("ADDFILTER " + hr);
                // Output pin from the source filter
                IPin SourceOut = DsFindPin.ByDirection(theDevice, PinDirection.Output, 0);

                // Open Audio Renderer
                theRenderer = CreateFilter(FilterCategory.AudioRendererCategory, (string)"Default DirectSound Device");
                hr = graphBuilder.AddFilter(theRenderer, "audio render");
                Console.WriteLine("ADDFILTER " + hr);
                // Input pin for the Renderer
                IPin RenderIn = DsFindPin.ByDirection(theRenderer, PinDirection.Input, 0);

                // Connect up the filters
                hr = graphBuilder.Connect(SourceOut, RenderIn);
                Console.WriteLine("CONNECT " + hr);
                // Not sure if necessary
                
                Console.WriteLine("AudioSample " + hr);
                // Tune in kHz
               
                

                hr = RTKFM.RTFM_SetFrequency(frequency);
                Console.WriteLine("FREQ " + hr);
                int pnLow = 0, pnHigh = 0;
                Console.WriteLine("TunerRange " + hr);
                
                //Implizites OpenDevice() & Start()
                mediaControl.Run();
                StartFMWorkerThread();

                

                int pnSamplePerSec = 48000;
                int pnBitPerSample = 1780;

                isRunning = true;
               // mediaControl.Stop();
                //hr = RTKFM.RTFM_CloseDevice();
                //hr = RTKFM.RTFM_OpenDevice();
                hr = RTKFM.RTFM_SetAudioSampleRate(48000);
                hr = RTKFM.RTFM_SetDeemphasisTC('0');
                hr = RTKFM.RTFM_StartRDS();
               // hr = RTKFM.RTFM_Start();
            }
            else if(!isRunning)
            {
                mediaControl.Run();
                isRunning = true;
            }
        }

        public unsafe void StartRDS()
        {
            hr = RTKFM.RTFM_StartRDS();
            Console.WriteLine(hr);
        }

        #region WorkerDefinition
        private void StartFMWorkerThread()
        {

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += FMWorker_DoWork;
            backgroundWorker.ProgressChanged += FMWorker_ProgressChanged;
            backgroundWorker.RunWorkerAsync(fmworker);

        }

        private void FMWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.quality = e.ProgressPercentage;
            //Console.WriteLine("RDSERR: " + RTKFM.getErrStatus());
        }

        private void FMWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            fmworker = (FMWorker)e.Argument;
            fmworker.ObserveSignalStrength(worker, e);
        }

        #endregion

        public void Stop()
        {
            if (mediaControl != null)
            {
                mediaControl.Stop();
                isRunning = false;
            }
        }

        public bool IsRunning()
        {
            return this.isRunning;
        }


        public string ReadRDS()
        {
            return fmworker.getRDSStream();
        }


        public void SetQualityChangedCallBackHandler(RadioCallbackHandler.QualityCallBackHandler nCallback)
        {
            fmworker.setCallBack(nCallback);
        }
    }
}
