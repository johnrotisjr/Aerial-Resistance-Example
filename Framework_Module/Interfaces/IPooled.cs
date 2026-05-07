namespace Framework_Module.Interfaces
{
    public interface IPooled<T> : IClearable
    {
        public void Set(T data);
    }
}