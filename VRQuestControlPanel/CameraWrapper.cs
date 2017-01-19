using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRQuestControlPanel
{
    
    class CameraWrapper
    {
        private FilterInfo sourceInfo;
        private IVideoSource videoSource;
        private List<IBitmapScreen> screens;

        public bool IsRunning{
            get
            {
                return videoSource == null ? false : true;
            }
        }

        public CameraWrapper(FilterInfo sourceInfo)
        {
            this.sourceInfo = sourceInfo;            

            this.screens = new List<IBitmapScreen>();
        }

        private void InitializeSource()
        {
            this.videoSource = new VideoCaptureDevice(sourceInfo.MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(ShowingFrame);
        }

        private void ShowingFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            foreach(IBitmapScreen sc in screens)
            {
                sc.DrawBitmap(bitmap);
            }
        }

        public void AddScreen(IBitmapScreen screen)
        {
            if(screen == null)
            {
                throw new NullReferenceException("IBitmapScreen is null");
            }
            this.screens.Add(screen);
        }

        public String GetName()
        {
            return this.sourceInfo.Name;
        }

        public void Start()
        {
            if (videoSource == null)
            {
                InitializeSource();
            }

            if (videoSource != null)
            {
                if (!videoSource.IsRunning)
                {
                    videoSource.Start();
                }
            }
        }

        public void Stop()
        {
            if(videoSource != null)
            {
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                }
            }
        }
    }
}
