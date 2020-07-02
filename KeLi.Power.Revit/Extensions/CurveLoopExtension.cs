using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     CurveLoop extension.
    /// </summary>
    public static class CurveLoopExtension
    {
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
    }
}