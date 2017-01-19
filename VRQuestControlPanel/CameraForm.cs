using System;
using System.ComponentModel;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Drawing;

namespace VRQuestControlPanel
{
    public partial class CameraForm : Form
    {

        private FilterInfoCollection videoDevicesList;
        private IVideoSource videoSource;

        public CameraForm()
        {
            InitializeComponent();
            InitializeCamerasList();
            this.Closing += FormClosing;
        }

        private void InitializeCamerasList()
        {
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevicesList)
            {
                cbSourceList.Items.Add(videoDevice.Name);
            }
            if (cbSourceList.Items.Count > 0)
            {
                cbSourceList.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormClosing(object sender, CancelEventArgs e)
        {
            // signal to stop
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(videoDevicesList[cbSourceList.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            videoSource.SignalToStop();
            if (videoSource != null && videoSource.IsRunning && pbCameraView.Image != null)
            {
                pbCameraView.Image.Dispose();
            }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pbCameraView.Image = bitmap;
        }
    }
}
