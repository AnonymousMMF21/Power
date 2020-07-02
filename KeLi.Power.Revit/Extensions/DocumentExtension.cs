using System;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Document extension.
    /// </summary>
    public static class DocumentExtension
    {
        /// <summary>
        ///     Creates a new model line element.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static ModelCurve NewModelCurve(this Document doc, Curve curve, SketchPlane plane)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (plane is null)
                throw new ArgumentNullException(nameof(plane));

            if (doc.IsFamilyDocument)
                return doc.FamilyCreate.NewModelCurve(curve, plane);

            return doc.Create.NewModelCurve(curve, plane);
        }
    }
}