#if USE_FIREBASE
using System;
using System.Collections.Generic;
using Cngine;
using Firebase.Database;

namespace Cngine
{
    
    public abstract class DatabaseDataStreamer<T>
    {
        public DatabaseReference DatabaseReference;

        public DatabaseDataStreamer(DatabaseReference databaseReference)
        {
            DatabaseReference = databaseReference;
            
            if (DatabaseReference == null)
            {
                Log.Error("Database Reference not setup, assign DatabaseReference on diriveing object constructor");
            }
        }

        public void ConnectToStream()
        {
            DatabaseReference.ChildAdded += OnDatabaseChildAdded;
            DatabaseReference.ChildChanged += OnDatabaseChildChanged;
            DatabaseReference.ChildRemoved += OnDatabaseChildRemoved;
        }

        public void DisconnectStream()
        {
            DatabaseReference.ChildAdded -= OnDatabaseChildAdded;
            DatabaseReference.ChildChanged -= OnDatabaseChildChanged;
            DatabaseReference.ChildRemoved -= OnDatabaseChildRemoved;
        }

        protected abstract void OnChildAdded(string key, T data);
        protected abstract void OnChildRemoved(string key, T data);
        protected abstract void OnChildChanged(string key, T data);

        protected virtual void OnDataFetched(Dictionary<string, T> data) {}

        protected void DispathToMainThread(Action action)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(action);
        }

        public void FetchData()
        {
            DatabaseReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    return;
                }

                if (task.IsCanceled)
                {
                    return;
                }

                var snapshot = task.Result;
                var key = snapshot.Key;
                var value = snapshot.Value;
                var json = snapshot.GetRawJsonValue();
                Log.Info($"Data Fetched, key : {key} , value { value}, json : {json}");
                var data = JsonSerializer.Deserialize<Dictionary<string, T>>(json);
                OnDataFetched(data);
            });
        }

        protected virtual void OnDatabaseChildChanged(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Log.Error(e.DatabaseError.Message);
                return;
            }

            var key = e.Snapshot.Key;
            var value = e.Snapshot.Value;
            var json = e.Snapshot.GetRawJsonValue();
            Log.Info($" OnTileAdded key : {key} , value { value}, json : {json}");

            var data = JsonSerializer.Deserialize<T>(json);
            OnChildChanged(key, data);
        }

        private void OnDatabaseChildRemoved(object sender, ChildChangedEventArgs e) 
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

            var data = JsonSerializer.Deserialize<T>(json);
            OnChildRemoved(key,data);
        }


        protected virtual void OnDatabaseChildAdded(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Log.Error(e.DatabaseError.Message);
                return;
            }

            var key = e.Snapshot.Key;
            var value = e.Snapshot.Value;
            var json = e.Snapshot.GetRawJsonValue();
            Log.Info($" OnTileAdded key : {key} , value { value}, json : {json}");

            var data = JsonSerializer.Deserialize<T>(json);
            OnChildAdded(key, data);
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