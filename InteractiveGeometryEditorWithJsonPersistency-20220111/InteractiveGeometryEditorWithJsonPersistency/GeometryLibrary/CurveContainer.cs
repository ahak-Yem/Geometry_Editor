using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GeometryLibrary
{
    public class CurveContainer<T> : List<T> where T : Curve
    {
        public void DrawElements(Graphics g)
        {
            foreach (T element in this)
            {
                element.Draw(g);
            }
        }
    }
}
