using System;

namespace Cngine
{
    public static class Log
    {
        private static bool Enabled = true;

        public static void Info(string message)
        {
            if (Enabled == false)
            {
                return;
            }

            UnityEngine.Debug.Log(message);
        }

        public static void Error(string message, bool forceSend = false)
        {
            if (Enabled == false)
            {
                return;
            }
            UnityEngine.Debug.LogError(message);
        }

        public static void Exception(string message, bool forceSend = true)
        {
            if (Enabled == false)
            {
                return;
            }
            UnityEngine.Debug.LogException(new Exception(message));
        }

        public static void Warning(string message)
        {
            if (Enabled == false)
            {
                return;
            }
            UnityEngine.Debug.LogWarning(message);
        }

        public static void Debug(string message)
        {
            if (Enabled == false)
            {
                return;
            }
            UnityEngine.Debug.Log("DEBUG: " + message);
        }
    }
}