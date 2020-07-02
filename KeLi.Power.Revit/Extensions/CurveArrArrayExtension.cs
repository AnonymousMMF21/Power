using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveArrArray extension.
    /// </summary>
    public static class CurveArrArrayExtension
    {
        /// <summary>
        ///     Resets the family symbol profile's location to zero point.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static CurveArrArray ResetCurveArrArray(this CurveArrArray profile, double eps = 1e-2)
        {
            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            var results = new CurveArrArray();
            var pts = profile.ToCurveList().Select(s => s.GetEndPoint(0));

            pts = pts.OrderBy(o => o.Z).ThenBy(o => o.Y).ThenBy(o => o.X);

            var location = pts.FirstOrDefault();

            foreach (CurveArray lines in profile)
            {
                var tmpLines = new CurveArray();

                foreach (var line in lines.Cast<Line>())
                {
                    if (line.Length < eps)
                        throw new InvalidDataException(line.ToString());

                    var pt1 = line.GetEndPoint(0) - location;
                    var pt2 = line.GetEndPoint(1) - location;
                    var newLine = Line.CreateBound(pt1, pt2);

                    tmpLines.Append(newLine);
                }

                results.Append(tmpLines);
            }

            return results;
        }

        /// <summary>
        ///     Converts the CurveArrArray to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveLoop>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves.ToCurveLoop());

            return results;
        }

        /// <summary>
        ///     Converts the CurveArrArray to the Curve list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<Curve>();

            foreach (CurveArray curves in curveArrArray)
                results.AddRange(curves.ToCurveList());

            return results;
        }

        /// <summary>
        ///     Converts the CurveArrArray to the CurveArray list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveArray>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves);

            return results;
        }
    }
}