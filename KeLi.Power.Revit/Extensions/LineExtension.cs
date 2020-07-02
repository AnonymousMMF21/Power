using System;
using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Line extension.
    /// </summary>
    public static class LineExtension
    {
        /// <summary>
        ///     Returns a new Offseted line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Line Offset(this Line line, XYZ vector = null)
        {
            var p0 = line.GetEndPoint(0);
            var p1 = line.GetEndPoint(1);

            if (vector != null)
            {
                p0 += vector;
                p1 += vector;
            }

            return Line.CreateBound(p0, p1);
        }

        /// <summary>
        ///     Returns the round point with custom precision.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static XYZ Round(this XYZ point, int eps = 4)
        {
            if (point is null)
                throw new ArgumentNullException(nameof(point));

            var roundX = Math.Round(point.X, eps);
            var roundY = Math.Round(point.Y, eps);
            var roundZ = Math.Round(point.Z, eps);

            return new XYZ(roundX, roundY, roundZ);
        }
    }
}