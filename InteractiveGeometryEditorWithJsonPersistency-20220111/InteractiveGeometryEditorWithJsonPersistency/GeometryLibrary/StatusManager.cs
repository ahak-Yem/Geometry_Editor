using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary
{
    /// <summary>
    /// Singleton class for showing status messages in the application.
    /// </summary>
    public class StatusManager
    {
        private static StatusManager _statusManager = null;

        /// <summary>
        /// Private constructor for singleton pattern.
        /// </summary>
        private StatusManager()
        { }

        /// <summary>
        /// Get the one and only instance.
        /// </summary>
        public static StatusManager Instance
        {
            get
            {
                if (_statusManager == null)
                {
                    _statusManager = new StatusManager();
                }
                return _statusManager;
            }
        }

        /// <summary>
        /// The event is raised when the status message changes. 
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessageChanged; 

        /// <summary>
        /// Set a new status message.
        /// </summary>
        /// <param name="statusMessage">The status message.</param>
        public void SetStatus(string statusMessage)
        {
            if (statusMessage == null)
                return;

            if (StatusMessageChanged != null)
                StatusMessageChanged(this, new StatusMessageEventArgs(statusMessage));
        }
    }
}
