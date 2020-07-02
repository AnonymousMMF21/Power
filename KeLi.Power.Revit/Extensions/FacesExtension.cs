using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     Face list extension.
    /// </summary>
    public static class FacesExtension
    {
        /// <summary>
        ///     Converts the Face list to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray<T>(this IEnumerable<T> faces) where T : Face
        {
            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }

        /// <summary>
        ///     Converts the Face list to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray<T>(params T[] faces) where T : Face
        {
            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }
    }
}
