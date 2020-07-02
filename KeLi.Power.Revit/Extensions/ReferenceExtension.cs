using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Reference extension.
    /// </summary>
    public static class ReferenceExtension
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