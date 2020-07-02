using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Reference list extension.
    /// </summary>
    public static class ReferencesExtension
    {
        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this IEnumerable<Reference> references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }

        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(params Reference[] references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }
    }
}
