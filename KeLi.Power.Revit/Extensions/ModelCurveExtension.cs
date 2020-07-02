using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Filters;
using KeLi.Power.Revit.Properties;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     ModelCurve extension.
    /// </summary>
    public static class ModelCurveExtension
    {
        /// <summary>
        ///     Sets ModelCurve list's style.
        /// </summary>
        public static void SetModelCurveList(this IEnumerable<ModelCurve> modelCurves, string lineName = null, Color color = null)
        {
            if (modelCurves is null)
                return;

            if (!modelCurves.ToList().Any())
                return;

            modelCurves.ToList().ForEach(f => f.SetModelCurve(lineName, color));
        }

        /// <summary>
        ///     Sets ModelCurve's style.
        /// </summary>
        /// <param name="modelCurve"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        public static void SetModelCurve(this ModelCurve modelCurve, string lineName = null, Color color = null)
        {
            if (modelCurve is null)
                return;

            var doc = modelCurve.Document;
            var lineCtg = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            if (string.IsNullOrWhiteSpace(lineName))
                lineName = Resources.DebugLine;

            if (!lineCtg.SubCategories.Contains(lineName))
            {
                if (color == null)
                    color = new Color(255, 0, 0);

                var modelCtg = doc.Settings.Categories.NewSubcategory(lineCtg, lineName);

                modelCtg.LineColor = color;
            }

            var graphicsStyles = doc.GetInstanceList<GraphicsStyle>();
            var modelStyle = graphicsStyles.FirstOrDefault(f => f.GraphicsStyleCategory.Name == lineName);
            var parm = modelCurve.get_Parameter(BuiltInParameter.BUILDING_CURVE_GSTYLE);

            if (modelStyle != null)
                parm.Set(modelStyle.Id);
        }
    }
}