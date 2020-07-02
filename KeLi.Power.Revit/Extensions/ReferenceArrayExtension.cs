using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     ReferenceArray entension.
    /// </summary>
    public static class ReferenceArrayExtension
    {
        /// <summary>
        ///     Converts the ReferenceArray to the Reference list.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static List<Reference> ToReferArray(this ReferenceArray references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new List<Reference>();

            foreach (Reference refer in references)
                results.Add(refer);

            return results;
        }
    }
}
