using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Line extension.
    /// </summary>
    public static class LineExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static List<XYZ> GetEndPoints(this Line line, XYZ vector = null)
        {
            var p0 = line.GetEndPoint(0);
            var p1 = line.GetEndPoint(1);

            if (vector != null)
            {
                p0 += vector;
                p1 += vector;
            }

            return new List<XYZ> { p0, p1 };
        }
    }
}