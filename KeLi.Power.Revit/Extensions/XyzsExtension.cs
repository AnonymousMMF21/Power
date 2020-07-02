using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Xyz list extension.
    /// </summary>
    public static class XyzsExtension
    {
        /// <summary>
        ///     Returns sorted XYZ list.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<XYZ> Order(this IEnumerable<XYZ> pts)
        {
            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            if (!pts.Any())
                throw new InvalidDataException(nameof(pts));

            return pts.OrderBy(o => o.X).ThenBy(o => o.Y).ThenBy(o => o.Z).ToList();
        }
    }
}
