using System;
using System.Collections.Generic;

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
    }
}