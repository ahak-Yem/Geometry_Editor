using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MathLibrary;
using Point = MathLibrary.Point;

namespace GeometryLibrary
{
    /// <summary>
    /// Class for circles in 3D space.
    /// </summary>
    public class Circle : Curve, ISurface
    {
        public const string StartMessage = "Please select the center point of the circle.";
        public const string EndMessage = "Please select a point at the circle's circumference.";

        /// <summary>
        /// The center point of the circle.
        /// </summary>
        public Point CenterPoint { get; set; }

        /// <summary>
        /// The normal of the circle.
        /// </summary>
        public Vector Normal { get; set; }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// The length of the circle.
        /// </summary>
        public override double Length => 2.0 * Math.PI * Radius;

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="g">The graphics context to be used.</param>
        public override void Draw(Graphics g)
        {
            // Build a rectangle to describe the circle
            float x = (float)(CenterPoint.X - Radius);
            float y = (float)(CenterPoint.Y - Radius);
            float diameter = 2f * (float)Radius;
            RectangleF rectangle = new RectangleF(x, y, diameter, diameter);
            g.DrawEllipse(DrawPen, rectangle);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="centerPoint">The center point.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="radius">The radius.</param>
        public Circle(Point centerPoint, Vector normal, double radius)
        {
            this.CenterPoint = centerPoint;
            this.Normal = normal;
            this.Radius = radius;
        }

        /// <summary>
        /// The area of the circle.
        /// </summary>
        public double Area => Math.PI * Radius * Radius;

        /// <summary>
        /// Determines the equality between the current circle and the passed object.
        /// </summary>
        /// <param name="obj">The passed object.</param>
        /// <returns>True for equality, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Circle circle)
            {
                return CenterPoint.Equals(circle.CenterPoint) && Normal.Equals(circle.Normal) && Math.Abs(Radius - circle.Radius) <= PointVectorBase.Tolerance;
            }

            return false;
        }

        /// <summary>
        /// Calculates the hash code for the current circle.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return CenterPoint.GetHashCode() ^ Normal.GetHashCode() ^ Radius.GetHashCode();
        }

        /// <summary>
        /// The curve click handler used to create a circle.
        /// </summary>
        public static ClickResult CircleClickHandler(System.Drawing.Point clickPoint, MouseButtons buttons, int screenHeight,
            ref Curve currentElement, out string statusMessage)
        {
            ClickResult result = ClickResult.Canceled;
            statusMessage = string.Empty;

            Point worldPoint = TransformScreen2World(clickPoint, screenHeight);
            if (buttons != MouseButtons.Right)
            {
                if (currentElement == null || currentElement.GetType() != typeof(Circle))
                {
                    currentElement = new Circle(worldPoint, new Vector(), 0);
                    result = ClickResult.Created;
                    statusMessage = EndMessage;
                }
                else
                {
                    Circle c = currentElement as Circle;
                    c.Radius = c.CenterPoint.DistanceTo(worldPoint);
                    result = ClickResult.Finished;
                    statusMessage = StartMessage;
                }
            }

            return result;
        }
    }
}
