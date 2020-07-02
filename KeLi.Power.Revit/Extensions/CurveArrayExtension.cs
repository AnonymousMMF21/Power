using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveArray extension.
    /// </summary>
    public static class CurveArrayExtension
    {
        /// <summary>
        ///     Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            return curveArray.ToCurveList().ToCurveLoop();
        }

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
        ///     Converts the CurveArray to the Curve list.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            var results = new List<Curve>();

            foreach (Curve curve in curveArray)
                results.Add(curve);

            return results;
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