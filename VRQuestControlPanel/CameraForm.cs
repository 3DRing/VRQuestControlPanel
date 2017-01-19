using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace VRQuestControlPanel
{
    public partial class CameraForm : Form, IBitmapScreen
    {

        private CamerasManager manager;
        private CameraWrapper camera;

        private Dictionary<int, int> camsMap;

        public CameraForm()
        {
            InitializeComponent();
            camsMap = new Dictionary<int, int>();

            manager = CamerasManager.GetInstance();

            InitializeCamerasList();
            this.Closing += FormClosing;
        }

        private void InitializeCamerasList()
        {
            List<CameraWrapper> cameras = manager.getCameras();
            int i = 0;
            foreach(CameraWrapper c in cameras)
            {
                if (!c.IsRunning)
                {
                    cbSourceList.Items.Add(c.GetName());
                    camsMap.Add(cbSourceList.Items.Count - 1, i);
                }
                i++;
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
            if (camera != null)
            {
                camera.Stop();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int camNumber = -1;
            camsMap.TryGetValue(cbSourceList.SelectedIndex, out camNumber);
            if (camNumber != -1) {
                if (cbSourceList.Text.Equals(""))
                {
                    return;
                }
                camera = manager.getCameras()[camNumber];
                camera.AddScreen(this);
                camera.Start();
            }else
            {
                throw new Exception("Impossible");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (camera != null)
            {
                camera.Stop();
            }
        }

        public void DrawBitmap(Bitmap bitmap)
        {
            pbCameraView.Image = bitmap;
        }
    }
}
