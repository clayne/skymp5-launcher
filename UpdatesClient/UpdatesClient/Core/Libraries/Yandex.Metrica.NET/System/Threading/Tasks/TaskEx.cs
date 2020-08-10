namespace System.Threading.Tasks
{
    internal static class TaskEx
    {
        public static async Task Run(Action action)
        {
            await Task.Run(action);
        }

        public static async Task Delay(TimeSpan delay)
        {
            await Task.Delay(delay);
        }

        public static async Task Delay(TimeSpan delay, CancellationToken token)
        {
            await Task.Delay(delay, token);
        }

        public static async Task<TResult> FromResult<TResult>(TResult result)
        {
            return await Task.FromResult<TResult>(result);
        }
    }
}
