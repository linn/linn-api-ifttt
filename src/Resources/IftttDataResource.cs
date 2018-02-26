namespace Linn.Api.Ifttt.Resources
{
    public class IftttDataResource<T>
    {
        public IftttDataResource(T data)
        {
            this.Data = data;
        }

        public T Data { get; }
    }
}