namespace Linn.Api.Ifttt.Service
{
    using System;
    using System.Threading.Tasks;

    public class LazyAsync<T> where T : class
    {
        private readonly Func<Task<T>> factory;

        private Task<T> task;

        public LazyAsync(Func<Task<T>> factory)
        {
            this.factory = factory;
        }

        public async Task<T> GetValue()
        {
            lock (this.factory)
            {
                if (this.task == null)
                {
                    this.task = this.factory();
                }
            }

            return await this.task;
        }
    }
}
