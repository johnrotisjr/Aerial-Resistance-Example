namespace Framework_Module.Interfaces
{
    public interface IPooled<T>
    {
        public void Set(T data);
        public void Clear();
    }
}