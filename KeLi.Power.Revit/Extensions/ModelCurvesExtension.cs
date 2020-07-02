using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     ModelCurve list extension.
    /// </summary>
    public static class ModelCurvesExtension
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
    }
}
