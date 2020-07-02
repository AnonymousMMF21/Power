using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Properties;
using KeLi.Power.Revit.Widgets;

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

        /// <summary>
        ///     Exports drawing list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="drawingPath"></param>
        /// <param name="setupName"></param>
        public static void ExportDrawingList(this Document doc, string drawingPath, string setupName = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var viewSheets = doc.GetInstanceList<ViewSheet>();

            var drawings = viewSheets.Where(w => w.ViewType == ViewType.DrawingSheet);

            var viewIds = drawings.Select(s => s.Id).ToList();

            if (viewIds.Count == 0)
                return;

            var setupNames = BaseExportOptions.GetPredefinedSetupNames(doc);

            if (string.IsNullOrWhiteSpace(setupName))
                setupName = setupNames.FirstOrDefault();

            var dwgOpts = DWGExportOptions.GetPredefinedOptions(doc, setupName);

            dwgOpts.MergedViews = true;

            var docName = Path.GetFileNameWithoutExtension(doc.PathName);

            doc.Export(drawingPath, string.Empty, viewIds, dwgOpts);

            var filePaths = Directory.GetFiles(drawingPath, "*.*");

            foreach (var filePath in filePaths)
            {
                var newFilePath = filePath.Replace($"{docName}-{Resources.Draw} - ", string.Empty);

                if (File.Exists(filePath))
                    File.Move(filePath, newFilePath);
            }
        }

        /// <summary>
        ///     To repeat call command.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="func"></param>
        /// <param name="flag"></param>
        /// <param name="ignoreEsc"></param>
        /// <returns></returns>
        public static bool RepeatCommand(this Document doc, Func<bool> func, bool flag = true, bool ignoreEsc = true)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                if (flag && func.Invoke())
                    return RepeatCommand(doc, func);
            }
            catch (OperationCanceledException)
            {
                if (ignoreEsc && func.Invoke())
                    return RepeatCommand(doc, func);
            }

            return false;
        }

        /// <summary>
        ///     Deletes element list by safely.
        /// </summary>
        /// <param name="elms"></param>
        public static void SafelyDeleteList(params Element[] elms)
        {
            if (elms is null)
                throw new ArgumentNullException(nameof(elms));

            elms.SafelyDeleteList();
        }

        /// <summary>
        ///     Deletes element list by safely.
        /// </summary>
        /// <param name="elms"></param>
        public static void SafelyDeleteList(this IEnumerable<Element> elms)
        {
            if (elms is null)
                throw new ArgumentNullException(nameof(elms));

            elms.ToList().ForEach(SafelyDelete);
        }

        /// <summary>
        ///     Deletes element list by safely.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ids"></param>
        public static void SafelyDeleteList(this Document doc, params ElementId[] ids)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            ids.ToList().ForEach(doc.SafelyDelete);
        }

        /// <summary>
        ///     Deletes element list by safely.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ids"></param>
        public static void SafelyDeleteList(this Document doc, IEnumerable<ElementId> ids)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            ids.ToList().ForEach(doc.SafelyDelete);
        }

        /// <summary>
        ///     Deletes element by safely.
        /// </summary>
        /// <param name="elm"></param>
        public static void SafelyDelete(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                return;

            if (elm.Id.IntegerValue == -1)
                return;

            elm.Document.Delete(elm.Id);
        }

        /// <summary>
        ///     Deletes element.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id"></param>
        public static void SafelyDelete(this Document doc, ElementId id)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (id is null)
                throw new ArgumentNullException(nameof(id));

            if (id.IntegerValue == -1)
                return;

            var elm = doc.GetElement(id);

            if (elm == null || !elm.IsValidObject)
                return;

            doc.Delete(id);
        }
    }
}