using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary
{
    /// <summary>
    /// The event args used for the <see cref="StatusManager.StatusMessageChanged"/> event.
    /// </summary>
    public class StatusMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">The status message.</param>
        public StatusMessageEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The status message.
        /// </summary>
        public string Message { get; }
    }
}
