using System;
using System.Threading;
using System.Threading.Tasks;

namespace EksiClient
{
	/// <summary>
	/// Async helper.
	/// </summary>
    public static class AsyncHelper
    {
        static readonly TaskFactory _myTaskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

		/// <summary>
		/// Runs the sync.
		/// </summary>
		/// <returns>The sync.</returns>
		/// <param name="func">Func.</param>
		/// <typeparam name="TResult">The 1st type parameter.</typeparam>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

		/// <summary>
		/// Runs the sync.
		/// </summary>
		/// <param name="func">Func.</param>
        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
