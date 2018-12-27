using System;
using System.Diagnostics;

namespace EksiClient
{
    /// <summary>
    /// Exception helper.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Write the specified sender and ex.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="ex">Ex.</param>
        public static void Write(object sender, Exception ex)
        {
            Debug.WriteLine(sender + "\n" + ex.Message);
        }
    }
}
