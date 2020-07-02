using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;

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
        ///     Sets the material's texture.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public static void SetTexture(this Material material, double angle, double[] offset, double[] size)
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
                xOffsetProp.Value = UnitUtils.Convert(offset[0], DUT_MILLIMETERS, xOffsetProp.DisplayUnitType);

            if (asset[TextureRealWorldOffsetY] is AssetPropertyDistance yOffsetProp)
                yOffsetProp.Value = UnitUtils.Convert(offset[1], DUT_MILLIMETERS, yOffsetProp.DisplayUnitType);

            if (asset[TextureRealWorldScaleX] is AssetPropertyDistance xSizeProp)
                xSizeProp.Value = UnitUtils.Convert(size[0], DUT_MILLIMETERS, xSizeProp.DisplayUnitType);

            if (asset[TextureRealWorldScaleY] is AssetPropertyDistance ySizeProp)
                ySizeProp.Value = UnitUtils.Convert(size[1], DUT_MILLIMETERS, ySizeProp.DisplayUnitType);

            editScope.Commit(true);
        }
    }
}