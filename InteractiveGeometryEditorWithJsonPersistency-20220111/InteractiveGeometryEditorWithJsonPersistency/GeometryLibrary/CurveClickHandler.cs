using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeometryLibrary
{
    public enum ClickResult
    {
        Created,
        PointHandled,
        Finished,
        Canceled
    }

    /// <summary>
    /// The delegate type used for the curve click handling.
    /// </summary>
    /// <param name="clickPoint">The click point on the screen area.</param>
    /// <param name="buttons">The pressed buttons.</param>
    /// <param name="screenHeight">The height of the given control.</param>
    /// <param name="currentElement">The current <see cref="Curve"/> element.</param>
    /// <param name="statusMessage">The status message.</param>
    /// <returns>The click result.</returns>
    public delegate ClickResult CurveClickHandler(System.Drawing.Point clickPoint, MouseButtons buttons, int screenHeight, 
        ref Curve currentElement, out string statusMessage);
}
