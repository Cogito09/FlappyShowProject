#if USE_FIREBASE
using System.Collections.Generic;
using Cngine;
using Firebase.Database;

namespace Cngine
{
    public class DatabaseStreamer<T>
    {
        public Dictionary<string, T> DatabaseReflection = new Dictionary<string, T>();
        public DatabaseReference DatabaseReference;

        public DatabaseStreamer(DatabaseReference databaseReference)
        {
            DatabaseReference = databaseReference;
        }

        public void ConnectToStream()
        {
            DatabaseReference.ChildAdded += OnChildAdded;
            DatabaseReference.ChildChanged += OnChildChanged;
            DatabaseReference.ChildRemoved += OnChildRemoved;
        }

        public void DisconnectStream()
        {
            DatabaseReference.ChildAdded -= OnChildAdded;
            DatabaseReference.ChildChanged -= OnChildChanged;
            DatabaseReference.ChildRemoved -= OnChildRemoved;
        }

        protected virtual void OnChildRemoved(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Log.Error(e.DatabaseError.Message);
                return;
            }

            var key = e.Snapshot.Key;
            var value = e.Snapshot.Value;
            var json = e.Snapshot.GetRawJsonValue();
            Log.Info($"OnTileRemoved key : {key} , value { value}, json : {json}");
            DatabaseReflection.Remove(key);
        }

        protected virtual void OnChildChanged(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Log.Error(e.DatabaseError.Message);
                return;
            }

            var key = e.Snapshot.Key;
            var value = e.Snapshot.Value;
            var json = e.Snapshot.GetRawJsonValue();
            Log.Info($"OnTileChanged key : {key} , value { value}, json : {json}");
            var data = JsonSerializer.Deserialize<T>(json);

            DatabaseReflection.TryGetValue(key, out var val);
            if (val == null)
            {
                Log.Info($"tried to get value but its null, creating new");
            }

            DatabaseReflection[key] = data;
        }

        protected virtual void OnChildAdded(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Log.Error(e.DatabaseError.Message);
                return;
            }

            var key = e.Snapshot.Key;
            var json = e.Snapshot.GetRawJsonValue();
            var data = JsonSerializer.Deserialize<T>(json);
            DatabaseReflection[key] = data;
        }

        protected virtual void SendLocationTileData()
        {
            var tileIdsToLocationIds = new Dictionary<int, string>();
            tileIdsToLocationIds.Add(0, "qwergf12k35mtir0");
            tileIdsToLocationIds.Add(1, "qwergf12k35mtir1");
            tileIdsToLocationIds.Add(2, "qwergf12k35mtir2");
            tileIdsToLocationIds.Add(3, "qwergf12k35mtir3");
            var data = JsonSerializer.Serialize(tileIdsToLocationIds);
            Log.Info($" OnTileSended key : json : {data}");
            DatabaseReference.SetRawJsonValueAsync(data);
        }

        protected virtual void SendData(object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            Log.Info($" OnTileSended key : json : {jsonData}");
            DatabaseReference.SetRawJsonValueAsync(jsonData);
        }
    }
}
#endif