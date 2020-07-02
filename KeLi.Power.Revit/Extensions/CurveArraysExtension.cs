using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveArray list extension.
    /// </summary>
    public static class CurveArraysExtension
    {        
        /// <summary>
             ///     Converts the CurveArray list to the CurveLoop list.
             /// </summary>
             /// <param name="curveArrays"></param>
             /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveArrArray.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveArrArray.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results;
        }
    }
}
