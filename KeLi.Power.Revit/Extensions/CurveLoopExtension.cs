using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveLoop extension.
    /// </summary>
    public static class CurveLoopExtension
    {
        /// <summary>
        ///     Converts the CurveLoop to the Curve list.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new List<Curve>();

            foreach (var curve in curveLoop)
                results.Add(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop to the CurveArray.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new CurveArray();

            foreach (var curve in curveLoop)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(ToCurveArray).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => s.ToCurveArray()).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArrArray.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            var results = new CurveArrArray();

            foreach (var curveLoop in curveLoops)
                results.Append(curveLoop.ToCurveArray());

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArrArray.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(params CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            var results = new CurveArrArray();

            foreach (var curveLoop in curveLoops)
                results.Append(curveLoop.ToCurveArray());

            return results;
        }
    }
}