using System.Collections.Generic;
using ProjectOne;

namespace Cngine
{
    public abstract class Request
    {
        public enum Method
        {
            Get = 1,
            Post = 2,
        }

        public const string HEADER_USER_ID = "userId";

#if UNITY_ANDROID && !UNITY_EDITOR
        public const string HEADER_SECRET = "secret";
#endif

        public virtual Method GetMethod()
        {
            return Method.Get;
        }

        public virtual Dictionary<string, string> GetHeaders()
        {
            var headers = new Dictionary<string, string>
            {
                {HEADER_USER_ID, GameMasterBase.BaseInstance.GetUserId()},
    
#if UNITY_ANDROID && !UNITY_EDITOR
                {HEADER_SECRET, "ABC" /*AndroidDeviceUtilities.MainSignature*/}, 
#endif
            };

            return headers;
        }

        public virtual Dictionary<string, string> GetParams()
        {
            return null;
        }

        public virtual Dictionary<string, List<object>> GetParamsList()
        {
            return null;
        }

        protected abstract string Route();

        public string GetRoute()
        {
            var route = Route();
            if (route[0] != '/')
            {
                route = $"/{route}";
            }

            if (route[route.Length - 1] == '/')
            {
                route = route.Remove(route.Length - 1, 1);
            }

            return $"https://us-central1-projectone-9d015.cloudfunctions.net/api{route}";
        }

        public void Send()
        {
            GameMasterBase.NetworkManager.SendRequest(this);
        }
    }
}