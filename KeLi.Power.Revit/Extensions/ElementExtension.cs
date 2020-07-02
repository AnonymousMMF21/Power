using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Builders;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Element extension.
    /// </summary>
    public static class ElementExtension
    {
        /// <summary>
        ///     Gets the element's location point.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static XYZ GetLocationPoint<T>(this T elm) where T : Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            if (elm.Id.IntegerValue == -1)
                throw new InvalidDataException(nameof(elm));

            return elm.Location is LocationPoint pt ? pt.Point : throw new InvalidCastException(elm.Name);
        }

        /// <summary>
        ///     Gets the element's location cuve.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static Curve GetLocationCurve<T>(this T elm) where T : Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            if (elm.Id.IntegerValue == -1)
                throw new InvalidDataException(nameof(elm));

            return !(elm.Location is LocationCurve curve) ? throw new InvalidCastException(elm.Name) : curve.Curve;
        }

        /// <summary>
        ///     Sets the element's color fill pattern.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="fillPattern"></param>
        /// <param name="color"></param>
        public static void SetColorFill(this Element elm, Element fillPattern, Color color)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            if (elm.Id.IntegerValue == -1)
                throw new InvalidDataException(nameof(elm));

            if (fillPattern is null)
                throw new ArgumentNullException(nameof(fillPattern));

            if (color is null)
                throw new ArgumentNullException(nameof(color));

            var doc = elm.Document;
            var graSetting = doc.ActiveView.GetElementOverrides(elm.Id);

            graSetting.SetProjectionFillPatternId(fillPattern.Id);
            graSetting.SetProjectionFillColor(color);
            doc.ActiveView.SetElementOverrides(elm.Id, graSetting);
        }

        /// <summary>
        ///     Gets the element's projection area.
        /// </summary>
        /// <param name="elm">A element</param>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">The input element is invalid.</exception>
        /// <returns>Returns projection area.</returns>
        public static double GetShadowArea(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            if (elm.Id.IntegerValue == -1)
                throw new InvalidDataException(nameof(elm));

            var areas = new List<double>();
            var geo = elm.get_Geometry(new Options());

            foreach (var instance in geo.Select(s => s as GeometryInstance))
            {
                if (instance is null)
                    continue;

                foreach (var item in instance.GetInstanceGeometry())
                {
                    var solid = item as Solid;

                    if (null == solid || solid.Faces.Size <= 0)
                        continue;

                    var plane = XYZ.BasisZ.CreatePlane(XYZ.Zero);

                    ExtrusionAnalyzer analyzer;

                    try
                    {
                        analyzer = ExtrusionAnalyzer.Create(solid, plane, XYZ.BasisZ);
                    }
                    catch
                    {
                        continue;
                    }

                    if (analyzer is null)
                        continue;

                    areas.Add(analyzer.GetExtrusionBase().Area);
                }
            }

            return areas.Max();
        }

        /// <summary>
        ///     Returns a new round box by specified element.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ RoundBox(this Element elm, int eps = 6)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            if (elm.Id.IntegerValue == -1)
                throw new InvalidDataException(nameof(elm));

            var box = elm.get_BoundingBox(elm.Document.ActiveView);

            return new BoundingBoxXYZ { Min = box.Min.Round(eps), Max = box.Max.Round(eps) };
        }
    }
}