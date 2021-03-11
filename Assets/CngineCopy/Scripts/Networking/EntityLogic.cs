namespace ProjectOne
{
    public abstract class EntityLogic<T>
    {
        public T Data;

        protected abstract void OnObjectCreated(T Data);
        public abstract void OnObjectChanged(T Data);
        public abstract void OnObjectDestroyed();

        public EntityLogic(T data)
        {
            Data = data;
        }
    }
}