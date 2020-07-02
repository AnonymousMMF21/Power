using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     UIDocument extension.
    /// </summary>
    public static class UIDocumentExtension
    {
        /// <summary>
        ///     To repeat call command.
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="func"></param>
        /// <param name="flag"></param>
        /// <param name="ignoreEsc"></param>
        /// <returns></returns>
        public static bool RepeatCommand(this UIDocument uidoc, Func<bool> func, bool flag = true, bool ignoreEsc = true)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                if (flag && func.Invoke())
                    return RepeatCommand(uidoc, func);
            }
            catch (OperationCanceledException)
            {
                if (ignoreEsc && func.Invoke())
                    return RepeatCommand(uidoc, func);
            }

            return false;
        }

    }
}
