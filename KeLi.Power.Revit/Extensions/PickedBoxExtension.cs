using System;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

using static System.Math;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     PickedBox extension.
    /// </summary>
    public static class PickedBoxExtension
    {
        /// <summary>
        ///     Converts to bounding box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ ToBoundingBoxXYZ(this PickedBox box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var minPt = box.Min;
            var maxPt = box.Max;

            var minX = Min(minPt.X, maxPt.X);
            var minY = Min(minPt.Y, maxPt.Y);
            var minZ = Min(minPt.Z, maxPt.Z);

            var maxX = Max(minPt.X, maxPt.X);
            var maxY = Max(minPt.Y, maxPt.Y);
            var maxZ = Max(minPt.Z, maxPt.Z);

            return new BoundingBoxXYZ { Min = new XYZ(minX, minY, minZ), Max = new XYZ(maxX, maxY, maxZ) };
        }
    }
}