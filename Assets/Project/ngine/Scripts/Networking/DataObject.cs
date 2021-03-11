using System.Collections.Generic;
using ProjectOne;

namespace Cngine
{
    public class DataObject<T>
    {
        public T Data { get; protected set; }


        protected DataObject(T model, bool dontWarnAboutMissingData = false)
        {
            Data = model;
            if (model == null && dontWarnAboutMissingData == false)
            {
                Log.Error("Can't create data object with null model");
            }
        }

        public void Refresh(T newData)
        {
            Data = newData;
            OnDataRefreshed();
        }

        protected virtual void OnDataRefreshed()
        {
        }

        public delegate T1 DataObjectCreator<T1>(T data) where T1 : DataObject<T>;
        protected static List<T1> GetList<T1>(List<T> data, DataObjectCreator<T1> creator) where T1 : DataObject<T>
        {
            var list = new List<T1>();
            if (data == null)
            {
                return list;
            }

            for (int i = 0, c = data.Count; i < c; ++i)
            {
                var mod = data[i];
                var created = creator(mod);
                list.Add(created);
            }

            return list;
        }
    }
}