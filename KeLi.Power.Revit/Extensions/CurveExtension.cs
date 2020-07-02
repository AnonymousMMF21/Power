using System;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Builders;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Curve Extension.
    /// </summary>
    public static class CurveExtension
    {
        /// <summary>
        ///     Draws a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static ElementId Draw(this Curve curve, Document doc, double eps = 1e-6)
        {
            if (doc is null)
                return null;

            if (curve is null)
                return null;

            XYZ normal = null;
            XYZ endPt = null;

            if (curve is Arc arc)
            {
                normal = arc.Normal;
                endPt = arc.Center;
            }

            else if (curve is Ellipse ellipse)
            {
                normal = ellipse.Normal;
                endPt = ellipse.Center;
            }

            else if (curve is Line line)
            {
                var refAsix = XYZ.BasisZ;

                if (Math.Abs(line.Direction.AngleTo(XYZ.BasisZ)) < eps)
                    refAsix = XYZ.BasisX;

                else if (Math.Abs(line.Direction.AngleTo(-XYZ.BasisZ)) < eps)
                    refAsix = XYZ.BasisX;

                normal = line.Direction.CrossProduct(refAsix).Normalize();
                endPt = line.Origin;
            }

            if (normal == null)
                throw new NullReferenceException(nameof(normal));

            var plane = normal.CreatePlane(endPt);
            var sketchPlane = SketchPlane.Create(doc, plane);

            return doc.NewModelCurve(curve, sketchPlane).Id;
        }
    }
}