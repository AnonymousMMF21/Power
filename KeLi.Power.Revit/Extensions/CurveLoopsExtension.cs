using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveLoop list extension.
    /// </summary>
    public static class CurveLoopsExtension
    {
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
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => s.ToCurveArray()).ToList();
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
