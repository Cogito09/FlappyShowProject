using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cngine;
using UnityEngine;
using UnityEngine.Networking;

namespace Cngine
{
    public class NetworkManager
    {
        public void SendRequest(Request request)
        {
            GameMasterBase.BaseInstance.StartCoroutine(Send(request));
        }

        private IEnumerator Send(Request req)
        {
            UnityWebRequest request = CreateRequest(req);
            yield return request.SendWebRequest();
            Log.Info($"URL({request.url}) SERVER RESPONDED WITH: {request.downloadHandler.text}");
        }

        private UnityWebRequest CreateRequest(Request req)
        {
            var route = req.GetRoute();
            var parameters = req.GetParams();
            var listParams = req.GetParamsList();
            if (req.GetMethod() == Request.Method.Get)
            {
                ModifyRoute(parameters, listParams, ref route);
            }

            var request = req.GetMethod() == Request.Method.Get ?
                UnityWebRequest.Get(route) :
                UnityWebRequest.Post(route, GetFormWithParams(parameters, listParams));

            request.downloadHandler = new DownloadHandlerBuffer();

            var headers = req.GetHeaders();
            foreach (var header in headers)
            {
                if (header.Value == null)
                {
                    Log.Error($"{header.Key} is null. Cannot send Request which null value");
                    continue;
                }

                request.SetRequestHeader(header.Key, header.Value);
            }

            Log.Info($"Route {route}\r\n:");
            return request;
        }

        private WWWForm GetFormWithParams(Dictionary<string, string> parameters, Dictionary<string, List<object>> listOfParameters)
        {
            var form = new WWWForm();

            if (parameters != null)
            {
                foreach (var kV in parameters)
                {
                    form.AddField(kV.Key, kV.Value);
                }
            }

            if (listOfParameters != null)
            {
                foreach (var kV in listOfParameters)
                {
                    for (int i = 0, c = kV.Value.Count; i < c; ++i)
                    {
                        form.AddField(kV.Key, kV.Value[i].ToString());
                    }
                }
            }

            return form;
        }

        private void ModifyRoute(Dictionary<string, string> parameters, Dictionary<string, List<object>> listParams, ref string route)
        {
            if (parameters == null && listParams == null)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.Append(route);
            if (route.EndsWith("/") == false)
            {
                builder.Append("/");
            }

            var index = 0;
            if (parameters != null)
            {
                foreach (var kV in parameters)
                {
                    string parFormat = index == 0 ? "?{0}={1}" : "&{0}={1}";
                    builder.AppendFormat(parFormat, kV.Key, kV.Value);
                    index++;
                }
            }

            if (listParams != null)
            {
                foreach (var kV in listParams)
                {
                    for (int i = 0, c = kV.Value.Count; i < c; ++i)
                    {
                        string parFormat = index == 0 ? "?{0}={1}" : "&{0}={1}";
                        builder.AppendFormat(parFormat, kV.Key, kV.Value[i]);
                        index++;
                    }
                }
            }

            route = builder.ToString();
        }
    }
}