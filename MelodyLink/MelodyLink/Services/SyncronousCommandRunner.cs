namespace MelodyLink.Services
{
    public class SyncronousCommandRunner
    {
        public T Run<T>(Func<Task<T>> task)
        {
            return task().GetAwaiter().GetResult();
        }
    }
}