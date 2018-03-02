namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class DataResource<T>
    {
        public DataResource(T data)
        {
            this.Data = data;
        }

        public T Data { get; }
    }
}