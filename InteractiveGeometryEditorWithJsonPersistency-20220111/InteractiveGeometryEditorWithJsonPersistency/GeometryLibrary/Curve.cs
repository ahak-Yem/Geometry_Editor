using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace GeometryLibrary
{
    [Serializable]
    /// <summary>
    /// Common base class for different types of curves.
    /// </summary>
    public abstract class Curve
    {
        /// <summary>
        /// The length of the curve.
        /// </summary>
        public abstract double Length { get; }

        [JsonIgnore]
        /// <summary>
        /// The pen used to draw the different types of curves. 
        /// </summary>
        public Pen DrawPen { get; set; } = new Pen(Color.Black);

        /// <summary>
        /// Draws a curve.
        /// </summary>
        /// <param name="g">The graphics context to be used.</param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Transforms the screen coordinates from the passed <see cref="Point"/>  
        /// to world coordinates for the returned <see cref="MathLibrary.Point"/>.
        /// </summary>
        /// <param name="screenPoint">The <see cref="Point"/> in screen coordinates.</param>
        /// <param name="screenHeight">The height of the screen (control).</param>
        /// <returns>A <see cref="MathLibrary.Point"/> in world coordinates.</returns>
        public static MathLibrary.Point TransformScreen2World(System.Drawing.Point screenPoint, int screenHeight)
        {
            return new MathLibrary.Point(screenPoint.X, -(screenPoint.Y - screenHeight));
        }
    }
}
