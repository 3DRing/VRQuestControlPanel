using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRQuestControlPanel
{
    class CamerasManager
    {
        private static CamerasManager manager;

        public static CamerasManager GetInstance()
        {
            if(manager != null)
            {
                return manager;
            }else
            {
                manager = new CamerasManager();
                return manager;
            }
        }

        public static void Initialize()
        {
            GetInstance();
        }

        private FilterInfoCollection videoDevicesList;
        private List<CameraWrapper> cameras;

        private CamerasManager()
        {
            cameras = new List<CameraWrapper>();

            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevicesList)
            {
                cameras.Add(new CameraWrapper(videoDevice));
            }
        }

        public List<CameraWrapper> getCameras()
        {
            return cameras;
        }
    }
}
