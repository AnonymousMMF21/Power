using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     SpatialElement extension.
    /// </summary>
    public static class SpatialElementExtension
    {
        /// <summary>
        ///     Returns intersection element list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<T> IntersectElementList<T>(this SpatialElement room, Document doc) where T : Element
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return room.IntersectElementList(doc).Where(w => w is T).Cast<T>().ToList();
        }

        /// <summary>
        ///     Returns intersection element list.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static List<Element> IntersectElementList(this SpatialElement room, Document doc, SpatialElementBoundaryLocation loc = default)
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var opt = new SpatialElementBoundaryOptions { SpatialElementBoundaryLocation = loc };
            var calc = new SpatialElementGeometryCalculator(doc, opt);
            var solid = calc.CalculateSpatialElementGeometry(room).GetGeometry();
            var instFilter = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            var itstFilter = new ElementIntersectsSolidFilter(solid);

            return instFilter.WherePasses(itstFilter).ToList();
        }
    }
}
