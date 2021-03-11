using UnityEngine;

namespace Cngine
{
    public static class DataStore
    {
        public static T Load<T>(string tag, T defaultValue = default(T))
        {
            var uncodedJson = LoadData(tag);
            if (string.IsNullOrEmpty(uncodedJson))
            {
                return defaultValue;
            }
            return JsonSerializer.Deserialize<T>(uncodedJson);
        }

        public static void Save<T>(T toSave, string tag)
        {
            var decodedJson = toSave;
            if (toSave == null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(decodedJson);
            SaveData(tag, json);
        }
        
        private static void SaveData(string key, string data)
        {
            if (PlayerPrefs.HasKey(key) == false)
            {
                Log.Debug( "Data have not been saved under " + key + " key. Given data : \n\n"+data);
            }
            
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        private static string LoadData(string key)
        {
            string uncodedData = null;
            if (PlayerPrefs.HasKey(key))
            {
                uncodedData = PlayerPrefs.GetString(key);
            }
            return uncodedData;
        }
    }
    
    
}