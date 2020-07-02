using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     XYZ Extension.
    /// </summary>
    public static class XyzExtension
    {
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