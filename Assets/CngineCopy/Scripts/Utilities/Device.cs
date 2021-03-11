using UnityEngine;

namespace Cngine
{
    public class Device
    {
        public bool IsEditor()
        {
            return Application.isEditor;
        }

        public bool IsDevelopment()
        {
            return Debug.isDebugBuild;
        }

        public string GetDeviceUniqueIdentifier()
        {
            string hash = "";
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    hash = "unity_editor_";
                    break;
            }

            return hash + SystemInfo.deviceUniqueIdentifier;
        }
    }
}