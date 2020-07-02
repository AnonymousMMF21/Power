using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     XYZ Extension.
    /// </summary>
    public static class XyzExtension
    {
        /// <summary>
        ///     Gets the result of whether the point is in the plane direction polygon.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool InPlanePolygon(this XYZ pt, IEnumerable<Line> polygon)
        {
            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            if (polygon is null)
                throw new ArgumentNullException(nameof(polygon));

            var x = pt.X;
            var y = pt.Y;
            var xs = new List<double>();
            var ys = new List<double>();

            foreach (var line in polygon)
            {
                xs.Add(line.GetEndPoint(0).X);
                ys.Add(line.GetEndPoint(0).Y);
            }

            var minX = xs.Min();
            var maxX = xs.Max();
            var minY = ys.Min();
            var maxY = ys.Max();
            var lines = polygon.ToList();

            if (lines.Count == 0 || x < minX || x > maxX || y < minY || y > maxY)
                return false;

            var result = false;

            for (int i = 0, j = lines.Count - 1; i < lines.Count; j = i++)
            {
                var dx = xs[j] - xs[i];
                var dy = ys[j] - ys[i];

                if (ys[i] > y != ys[j] > y && x < dx * (y - ys[i]) / dy + xs[i])
                    result = !result;
            }

            return result;
        }

        /// <summary>
        ///     Resets point's x axis value.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static XYZ ResetX(this XYZ pt, double x = 0d)
        {
            if (pt is null)
                throw new NullReferenceException(nameof(pt));

            return new XYZ(x, pt.Y, pt.Z);
        }

        /// <summary>
        ///     Resets point's Y axis value.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static XYZ ResetY(this XYZ pt, double y = 0d)
        {
            if (pt is null)
                throw new NullReferenceException(nameof(pt));

            return new XYZ(pt.X, y, pt.Z);
        }

        /// <summary>
        ///     Resets point's Z axis value.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static XYZ ResetZ(this XYZ pt, double z = 0d)
        {
            if (pt is null)
                throw new NullReferenceException(nameof(pt));

            return new XYZ(pt.X, pt.Y, z);
        }

        /// <summary>
        ///     Finds the nearest element from the point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type FindNearestElement<T>(this XYZ pt)
        {
            var dd = new List<string>();

            return null;
        }
    }
}