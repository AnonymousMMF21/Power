using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Curve list extension.
    /// </summary>
    public static class CurvesExtension
    {
        /// <summary>
        ///     Draws ModelCurve list.
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<ElementId> Draw(this IEnumerable<Curve> curves, Document doc)
        {
            if (curves is null)
                return null;

            if (!curves.Any())
                return null;

            return curves.Select(f => f.Draw(doc)).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(this IEnumerable<T> curves) where T : Curve
        {
            var newCurves = curves.ToList();

            if (curves == null || newCurves.Count == 0)
                throw new NullReferenceException(nameof(newCurves));

            var results = new CurveLoop();
            var endPt = newCurves[0]?.GetEndPoint(1);

            results.Append(newCurves[0]);

            newCurves[0] = null;

            // If computing count equals curveLoop count, it should break.
            // Because, the curveLoop cannot find valid curve to append.
            var count = 0;

            while (count < newCurves.Count && results.Count() < newCurves.Count)
            {
                for (var i = 0; i < newCurves.Count; i++)
                {
                    if (newCurves[i] == null)
                        continue;

                    var p0 = newCurves[i].GetEndPoint(0);

                    var p1 = newCurves[i].GetEndPoint(1);

                    if (p0.IsAlmostEqualTo(endPt))
                    {
                        endPt = p1;

                        results.Append(newCurves[i]);

                        newCurves[i] = null;
                    }

                    // The curve should be reversed.
                    else if (p1.IsAlmostEqualTo(endPt))
                    {
                        endPt = p0;

                        results.Append(newCurves[i].CreateReversed());

                        newCurves[i] = null;
                    }
                }

                count++;
            }

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            return curves.ToCurveLoop();
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            return new List<CurveLoop> { curves.ToCurveLoop() };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            return new List<CurveLoop> { curves.ToCurveLoop() };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray> { ary };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray> { ary };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArrArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            var results = new CurveArrArray();

            results.Append(ary);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArrArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.ToList().Count == 0)
                throw new InvalidDataException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            var results = new CurveArrArray();

            results.Append(ary);

            return results;
        }
    }
}