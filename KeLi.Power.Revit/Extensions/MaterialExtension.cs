using System;
using System.IO;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;

using KeLi.Power.Revit.Builders;

using static Autodesk.Revit.DB.DisplayUnitType;
using static Autodesk.Revit.DB.Visual.UnifiedBitmap;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Material extension.
    /// </summary>
    public static class MaterialExtension
    {
        /// <summary>
        ///     Sets the material's base texture.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="initAngle"></param>
        public static void SetTextureBase(this Material material, double angle, double[] offset, double[] size, TextureAngle initAngle)
        {
            if (offset == null || offset.Length == 0)
                return;

            if (size == null || size.Length == 0)
                return;

            angle += (int)initAngle;
            angle %= 360;

            if(angle < 0)
                throw new InvalidDataException(nameof(angle));

            material.SetTextureBase(angle, offset, size);
        }

        /// <summary>
        ///     Sets the material's texture.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public static void SetTextureBase(this Material material, double angle, double[] offset, double[] size)
        {
            var doc = material.Document;

            if (offset == null || offset.Length == 0)
                return;

            if (size == null || size.Length == 0)
                return;

            using var editScope = new AppearanceAssetEditScope(doc);
            var asset = editScope.Start(material.AppearanceAssetId);

            if (asset[TextureWAngle] is AssetPropertyDouble angleProp)
                angleProp.Value = angle;

            if (asset[TextureRealWorldOffsetX] is AssetPropertyDistance xOffsetProp)
                xOffsetProp.Value = UnitUtils.Convert(offset[0], DUT_DECIMAL_FEET, xOffsetProp.DisplayUnitType);

            if (asset[TextureRealWorldOffsetY] is AssetPropertyDistance yOffsetProp)
                yOffsetProp.Value = UnitUtils.Convert(offset[1], DUT_DECIMAL_FEET, yOffsetProp.DisplayUnitType);

            if (asset[TextureRealWorldScaleX] is AssetPropertyDistance xSizeProp)
            {
                var minXSize = UnitUtils.Convert(0.01, xSizeProp.DisplayUnitType, DUT_MILLIMETERS);

                if (size[0] > minXSize)
                    xSizeProp.Value = UnitUtils.Convert(size[0], DUT_DECIMAL_FEET, xSizeProp.DisplayUnitType);
            }

            if (asset[TextureRealWorldScaleY] is AssetPropertyDistance ySizeProp)
            {
                var minYSize = UnitUtils.Convert(0.01, ySizeProp.DisplayUnitType, DUT_MILLIMETERS);

                if (size[1] > minYSize)
                    ySizeProp.Value = UnitUtils.Convert(size[1], DUT_DECIMAL_FEET, ySizeProp.DisplayUnitType);
            }

            editScope.Commit(true);
        }

        /// <summary>
        ///     Sets texture's path.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="texturePath"></param>
        public static void SetTexturePath(this Material material, string texturePath)
        {
            var doc = material.Document;
            using var editScope = new AppearanceAssetEditScope(doc);
            var editableAsset = editScope.Start(material.AppearanceAssetId);
            var bitmapAssist = editableAsset[UnifiedbitmapBitmap];

            if (bitmapAssist is AssetPropertyString path && path.IsValidValue(texturePath))
                path.Value = texturePath;

            editScope.Commit(true);
        }

        /// <summary>
        ///     Computes texture's initial angle.
        /// </summary>
        /// <param name="baseX"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static double ComputeTextureAngle(this XYZ baseX, double eps = 1e-6)
        {
            var baseRadian = baseX.AngleTo(XYZ.BasisX);

            // To compute min radian with x asix.
            if (baseRadian >= Math.PI / 2)
                baseRadian = Math.PI - baseRadian;

            var result = 0d;

            // 1st quadrant.
            if (baseX.X > eps && baseX.Y > eps)
                result = 90 - baseRadian * 180 / Math.PI;

            // 2nd quadrant.
            else if (baseX.X < -eps && baseX.Y > eps)
                result = 270 + baseRadian * 180 / Math.PI;

            // 3rd quadrant.
            else if (baseX.X < -eps && baseX.Y < -eps)
                result = 180 + baseRadian * 180 / Math.PI;

            // 4th quadrant.
            else if (baseX.X > eps && baseX.Y < -eps)
                result = 90 + baseRadian * 180 / Math.PI;

            // →
            else if (baseX.X > eps && baseX.Y <= eps)
                result = 90;

            // ↓
            else if (baseX.X <= eps && baseX.Y < -eps)
                result = 180;

            // ←
            else if (baseX.X < -eps && baseX.Y <= eps)
                result = 270;

            // ↑ 
            else if (baseX.X > eps && baseX.Y <= eps)
                result = 0;

            return result;
        }
    }
}