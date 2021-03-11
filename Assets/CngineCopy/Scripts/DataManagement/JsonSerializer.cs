using System;
using Newtonsoft.Json;

namespace Cngine
{
    public class JsonSerializer : Newtonsoft.Json.JsonSerializer
    {
        public static T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            return default(T);
        }


        public static string Serialize(object objectToSerialize, bool intended = false)
        {
            return JsonConvert.SerializeObject(objectToSerialize, intended ? Formatting.Indented : Formatting.None);
        }

        public static T Deserialize<T>(T data)
        {
            throw new NotImplementedException();
        }
    }
}