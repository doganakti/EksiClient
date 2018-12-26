using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EksiClient
{
	/// <summary>
	/// Shaa.
	/// </summary>
	public static class Shaa
	{
        static Random random = new Random();

        /// <summary>
        /// Write the specified title and payload.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="title">Title.</param>
        /// <param name="payload">Payload.</param>
        public static void Write(string title, string payload)
		{
            Debug.WriteLine($"\n");
            Debug.WriteLine($"{title.ToUpperInvariant()}");
            Debug.WriteLine(payload);
		}

		/// <summary>
		/// Check the specified action and label.
		/// </summary>
		/// <returns>The check.</returns>
		/// <param name="action">Action.</param>
		/// <param name="label">Label.</param>
		public static double CheckTime(Action action, string label)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			action();
			stopwatch.Stop();
            Write(label, stopwatch.Elapsed.TotalSeconds.ToString() + " seconds");
			return stopwatch.Elapsed.TotalSeconds;
		}

        /// <summary>
        /// Gets the random number.
        /// </summary>
        /// <returns>The random number.</returns>
        /// <param name="floor">Floor.</param>
        public static int GetRandomNumber(int floor)
        {
            int r = random.Next(floor);
            return r;
        }
	}
}
