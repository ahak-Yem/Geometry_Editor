using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MathLibrary;
using Point = MathLibrary.Point;
using System.Windows.Forms;

namespace GeometryLibrary
{
    /// <summary>
    /// Class for lines in 3D space.
    /// </summary>
    public class Line : Curve
    {
        public const string StartMessage = "Please select the start point of the line.";
        public const string EndMessage = "Please select the end point of the line.";

        /// <summary>
        /// The start point of the line.
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// The end point of the line.
        /// </summary>
        public Point EndPoint { get; set; }

        /// <summary>
        /// The length of the line.
        /// </summary>
        public override double Length => StartPoint.DistanceTo(EndPoint);

        /// <summary>
        /// The direction of the line as a normalized vector
        /// </summary>
        public Vector Direction => new Vector(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y, EndPoint.Z - StartPoint.Z).Normalize();

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="g">The graphics context to be used.</param>
        public override void Draw(Graphics g)
        {
            g.DrawLine(DrawPen, (float)StartPoint.X, (float)StartPoint.Y, (float)EndPoint.X, (float)EndPoint.Y);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        public Line(Point startPoint, Point endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Determines the equality between the current line and the passed object.
        /// </summary>
        /// <param name="obj">The passed object.</param>
        /// <returns>True for equality, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Line line)
            {
                return StartPoint.Equals(line.StartPoint) && EndPoint.Equals(line.EndPoint);
            }

            return false;
        }

        /// <summary>
        /// Calculates the hash code for the current line.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return StartPoint.GetHashCode() ^ EndPoint.GetHashCode();
        }

        /// <summary>
        /// The curve click handler used to create a line.
        /// </summary>
        public static ClickResult LineClickHandler(System.Drawing.Point clickPoint, MouseButtons buttons, int screenHeight,
            ref Curve currentElement, out string statusMessage)
        {
            ClickResult result = ClickResult.Canceled;
            statusMessage = string.Empty;

            Point worldPoint = TransformScreen2World(clickPoint, screenHeight);
            if (buttons != MouseButtons.Right)
            {
                if (currentElement == null || currentElement.GetType() != typeof(Line))
                {
                    currentElement = new Line(worldPoint, worldPoint);
                    result = ClickResult.Created;
                    statusMessage = EndMessage;
                }
                else
                {
                    (currentElement as Line).EndPoint = worldPoint;
                    result = ClickResult.Finished;
                    statusMessage = StartMessage;
                }
            }

            return result;
        }
    }
}
