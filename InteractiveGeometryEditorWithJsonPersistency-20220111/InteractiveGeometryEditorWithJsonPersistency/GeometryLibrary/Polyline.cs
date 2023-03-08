using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathLibrary;
using Point = MathLibrary.Point;

namespace GeometryLibrary
{
    /// <summary>
    /// Class for polylines in 3D space.
    /// </summary>
    public class Polyline : Curve, ISurface
    {
        public const string StartMessage = "Please select the start point of the polyline.";
        public const string NextMessage = "Please select the next point of the polyline with the left mouse button or click right to cancel.";
        public const string EndMessage = "Please select the next point of the polyline with the left mouse button or click right to end.";
        private readonly List<Point> _points = new List<Point>();

        /// <summary>
        /// The length of the polyline.
        /// </summary>
        public override double Length
        {
            get
            {
                double length = 0.0;
                foreach (var lineSegment in LineSegments)
                {
                    length += lineSegment.Length;
                }

                return length;
            }
        }

        /// <summary>
        /// Is the polyline closed, i.e. are the first and the last point the same?
        /// </summary>
        public bool IsClosed
        {
            get
            {
                if (!IsValid)
                    return false;

                Point firstPoint = _points.First();
                Point lastPoint = _points.Last();
                return (firstPoint.X == lastPoint.X && firstPoint.Y == lastPoint.Y && firstPoint.Z == lastPoint.Z);
            }
        }

        /// <summary>
        /// Is the polyline planar, i.e. lay all the points in the same plane?
        /// </summary>
        public bool IsPlanar
        {
            get
            {
                if (_points.Count < 3)
                    return false;

                // Here we have to test, whether all points lay in the same plane.
                // For this purpose we calculate the cross products for all subsequent line segments.
                // If the cross product of the first line segment with all other line segments are collinear,
                // then the line segments are coplanar.
                bool isFirst = true;
                Vector firstLineSegment = LineSegments.First();
                List<Vector> normals = new List<Vector>();
                foreach (var lineSegment in LineSegments)
                {
                    // Skip the first one
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }

                    Vector normal = firstLineSegment.CrossProduct(lineSegment);
                    normals.Add(normal);
                }

                Vector firstNormal = normals.First();
                foreach (var normal in normals)
                {
                    // Skip the first one
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }

                    if (!firstNormal.AreCollinear(normal))
                        return false;
                }

                return true;
            }
        }

        private IEnumerable<Vector> LineSegments
        {
            get
            {
                for (int index = 0; index < _points.Count - 1; index++)
                {
                    Point firstPoint = _points[index];
                    Point secondPoint = _points[index + 1];
                    yield return new Vector(secondPoint.X - firstPoint.X, secondPoint.Y - firstPoint.Y,
                        secondPoint.Z - firstPoint.Z);
                }
            }
        }

        /// <summary>
        /// Is the polyline valid, i.e. does it contain at least 2 points.
        /// </summary>
        public bool IsValid => _points.Count >= 2;

        /// <summary>
        /// The area of the polyline.
        /// </summary>
        public double Area
        {
            get
            {
                if (!IsClosed || !IsPlanar)
                    return 0.0;

                // Area calculation according to http://geomalgorithms.com/a01-_area.html 
                Vector resultVector = new Vector();
                int n = _points.Count;
                for (int index = 0; index < _points.Count - 2; index++)
                {
                    Vector firstPoint = _points[index].AsVector();
                    Vector secondPoint = _points[index + 1].AsVector();

                    resultVector = resultVector.Add(firstPoint.CrossProduct(secondPoint));
                }

                // Normal
                Vector firstLineSegment = LineSegments.First();
                Vector lastLineSegment = LineSegments.Last();
                Vector normal = firstLineSegment.CrossProduct(lastLineSegment).Normalize();

                // We return only positive values
                return Math.Abs(0.5 * normal.DotProduct(resultVector));                    
            }
        }

        /// <summary>
        /// The points of the polyline as <see cref="IReadOnlyList&lt;Point&gt;"/>.
        /// </summary>
        public IReadOnlyList<Point> Points => _points.AsReadOnly();

        /// <summary>
        /// Adds the passed point to the polyline.
        /// </summary>
        /// <param name="newPoint">The point to add.</param>
        public void AddPoint(Point newPoint)
        {
            _points.Add(newPoint);
        }

        /// <summary>
        /// Inserts the passed point at the given index to the polyline.
        /// </summary>
        /// <param name="index">The index where the point will be added.</param>
        /// <param name="newPoint">The point to add.</param>
        public void InsertPoint(int index, Point newPoint)
        {
            _points.Insert(index, newPoint);
        }

        /// <summary>
        /// Removes the point at the given index from the polyline.
        /// </summary>
        /// <param name="index">The index where the point will be removed.</param>
        public void RemovePoint(int index)
        {
            _points.RemoveAt(index);
        }

        /// <summary>
        /// Draws a polyline.
        /// </summary>
        /// <param name="g">The graphics context to be used.</param>
        public override void Draw(Graphics g)
        {
            var points = _points.Select(p => new PointF((float)p.X, (float)p.Y));
            g.DrawLines(DrawPen, points.ToArray());
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="points">The points defining the polyline.</param>
        public Polyline(Point[] points)
        {
            _points.AddRange(points);
        }

        /// <summary>
        /// Determines the equality between the current polyline and the passed object.
        /// </summary>
        /// <param name="obj">The passed object.</param>
        /// <returns>True for equality, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Polyline polyline)
            {
                if (_points.Count != polyline._points.Count)
                    return false;

                for (int i = 0; i < _points.Count; i++)
                {
                    if (!_points[i].Equals(polyline._points[i]))
                        return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the hash code for the current polyline.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (var point in _points)
            {
                hashCode ^= point.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// The curve click handler used to create a polyline.
        /// </summary>
        public static ClickResult PolylineClickHandler(System.Drawing.Point clickPoint, MouseButtons buttons, int screenHeight,
            ref Curve currentElement, out string statusMessage)
        {
            ClickResult result = ClickResult.Canceled;
            statusMessage = string.Empty;

            Point worldPoint = TransformScreen2World(clickPoint, screenHeight);
            if (currentElement == null || currentElement.GetType() != typeof(Polyline))
            {
                if (buttons == MouseButtons.Left)
                {
                    currentElement = new Polyline(new Point[] { worldPoint });
                    result = ClickResult.Created;
                    statusMessage = NextMessage;
                }
                else if (buttons == MouseButtons.Right)
                {
                    result = ClickResult.Canceled;
                    statusMessage = StartMessage;
                }
            }
            else
            {
                Polyline p = currentElement as Polyline;

                if (buttons == MouseButtons.Left)
                {
                    p.AddPoint(worldPoint);
                    result = ClickResult.PointHandled;
                    statusMessage = EndMessage;

                }
                else if (buttons == MouseButtons.Right && p.Points.Count < 2)
                {
                    result = ClickResult.Canceled;
                    statusMessage = StartMessage;
                }
                else if (buttons == MouseButtons.Right)
                {
                    result = ClickResult.Finished;
                    statusMessage = StartMessage;
                }
            }

            return result;
        }
    }
}
