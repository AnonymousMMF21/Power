using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

using static System.Math;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     BoundingBoxXYZ extension.
    /// </summary>
    public static class BoundingBoxXyzExtension
    {
        /// <summary>
        ///     Returns a new round box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ Round(this BoundingBoxXYZ box, int eps = 4)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return new BoundingBoxXYZ { Min = box.Min.Round(eps), Max = box.Max.Round(eps) };
        }

        /// <summary>
        ///     Returns the union box between the box1 and the box2.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ Union(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            var box1Min = box1.Min;
            var box1Max = box1.Max;

            var box2Min = box2.Min;
            var box2Max = box2.Max;

            var minX = Min(box1Min.X, box2Min.X);
            var minY = Min(box1Min.Y, box2Min.Y);
            var minZ = Min(box1Min.Z, box2Min.Z);

            var maxX = Max(box1Max.X, box2Max.X);
            var maxY = Max(box1Max.Y, box2Max.Y);
            var maxZ = Max(box1Max.Z, box2Max.Z);

            return new BoundingBoxXYZ { Min = new XYZ(minX, minY, minZ), Max = new XYZ(maxX, maxY, maxZ) };
        }

        /// <summary>
        ///     Returns the intersection box between the box1 and the box2.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ Intersect(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            var box1Min = box1.Min;
            var box1Max = box1.Max;

            var box2Min = box2.Min;
            var box2Max = box2.Max;

            var minX = Max(box1Min.X, box2Min.X);
            var minY = Max(box1Min.Y, box2Min.Y);
            var minZ = Max(box1Min.Z, box2Min.Z);

            var maxX = Min(box1Max.X, box2Max.X);
            var maxY = Min(box1Max.Y, box2Max.Y);
            var maxZ = Min(box1Max.Z, box2Max.Z);

            return new BoundingBoxXYZ { Min = new XYZ(minX, minY, minZ), Max = new XYZ(maxX, maxY, maxZ) };
        }

        /// <summary>
        ///     Gets the plane edge set and z axis value equals no zero.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<Line> GetPlaneEdgeList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var vectors = box.GetPlaneVectorList();

            var p1 = vectors[0];
            var p2 = vectors[1];
            var p3 = vectors[2];
            var p4 = vectors[3];

            var p12 = Line.CreateBound(p1, p2);
            var p23 = Line.CreateBound(p2, p3);
            var p34 = Line.CreateBound(p3, p4);
            var p41 = Line.CreateBound(p4, p1);

            return new List<Line> { p12, p23, p34, p41 };
        }

        /// <summary>
        ///     Gets the box's plane 4 vectors.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<XYZ> GetPlaneVectorList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var p1 = box.Min;
            var p2 = new XYZ(box.Max.X, box.Min.Y, p1.Z);
            var p3 = new XYZ(box.Max.X, box.Max.Y, p1.Z);
            var p4 = new XYZ(p1.X, box.Max.Y, p1.Z);

            return new List<XYZ> { p1, p2, p3, p4 };
        }

        /// <summary>
        ///     Gets the space edge list.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<Line> GetSpaceEdgeList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var vectors = box.GetSpaceVectorList();

            var p1 = vectors[0];
            var p2 = vectors[1];
            var p3 = vectors[2];
            var p4 = vectors[3];
            var p5 = vectors[4];
            var p6 = vectors[5];
            var p7 = vectors[6];
            var p8 = vectors[7];

            var p12 = Line.CreateBound(p1, p2);
            var p14 = Line.CreateBound(p1, p4);
            var p15 = Line.CreateBound(p1, p5);
            var p23 = Line.CreateBound(p2, p3);
            var p24 = Line.CreateBound(p2, p4);
            var p34 = Line.CreateBound(p3, p4);
            var p37 = Line.CreateBound(p3, p7);
            var p48 = Line.CreateBound(p4, p8);
            var p56 = Line.CreateBound(p5, p6);
            var p58 = Line.CreateBound(p5, p8);
            var p67 = Line.CreateBound(p6, p7);
            var p78 = Line.CreateBound(p7, p8);

            return new List<Line> { p12, p14, p15, p23, p24, p34, p37, p48, p56, p58, p67, p78 };
        }

        /// <summary>
        ///     Gets the box's space 8 vectors.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<XYZ> GetSpaceVectorList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var p1 = box.Min;
            var p2 = new XYZ(box.Max.X, box.Min.Y, p1.Z);
            var p3 = new XYZ(box.Max.X, box.Max.Y, p1.Z);
            var p4 = new XYZ(p1.X, box.Max.Y, p1.Z);
            var p5 = new XYZ(p1.X, p1.Y, box.Max.Z);
            var p6 = new XYZ(box.Max.X, p1.Y, box.Max.Z);
            var p7 = new XYZ(p1.X, box.Max.Y, box.Max.Z);
            var p8 = box.Max;

            return new List<XYZ> { p1, p2, p3, p4, p5, p6, p7, p8 };
        }

        /// <summary>
        ///     Gets the box's center point.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static XYZ GetBoxCenter(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return (box.Max + box.Min) / 2;
        }

        /// <summary>
        ///     Gets the box's length on y axis dreiction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxLength(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.X - box.Min.X;
        }

        /// <summary>
        ///     Gets the box's width on x axis direction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxWidth(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.Y - box.Min.Y;
        }

        /// <summary>
        ///     Gets the box's height on z axis direction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxHeight(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.Z - box.Min.Z;
        }
    }
}