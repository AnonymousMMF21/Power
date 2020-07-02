using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Extensions;

namespace KeLi.Power.Revit.Widgets
{
    /// <summary>
    ///     Mesh utility.
    /// </summary>
    public static class MeshUtil
    {
        /// <summary>
        ///     Gets the element's point set base on distance.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<XYZ> GetPointList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            var pts = elm.GetEdgeList().SelectMany(f => f.Tessellate()).ToList();
            var results = pts.OrderBy(o => o.X).ThenBy(o => o.Y).ThenBy(o => o.Z).ToList();

            for (var i = 0; i < results.Count; i++)
            {
                for (var j = i + 1; j < results.Count; j++)
                {
                    if (results[i] is null || results[j] is null)
                        continue;

                    if (results[j].Round(2).ToString() == results[i].Round(2).ToString())
                        results[j] = null;
                }
            }

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        ///     Gets the element's mess-triange list dictionary.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Mesh, List<MeshTriangle>> GetMesh2TrianglesDict(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new Dictionary<Mesh, List<MeshTriangle>>();

            var meshes = elm.GetMeshList();

            meshes.ForEach(f => results.Add(f, f.GetMeshTriangleList()));

            return results;
        }

        /// <summary>
        ///     Gets the element's triange list.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static List<MeshTriangle> GetMeshTriangleList(this Mesh mesh)
        {
            if (mesh is null)
                throw new ArgumentNullException(nameof(mesh));

            var results = new List<MeshTriangle>();

            for (var i = 0; i < mesh.NumTriangles; i++)
                results.Add(mesh.get_Triangle(i));

            return results;
        }

        /// <summary>
        ///     Gets the element's mesh list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Mesh> GetMeshList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new List<Mesh>();

            elm.GetFaceList().ForEach(f => results.Add(f.Triangulate()));

            return results;
        }

        /// <summary>
        ///     Gets the element's face set for the specified direction.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="eps"></param>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static List<Face> GetFaceList(this Element elm, int eps = 6, params XYZ[] dirs)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var faces = elm.GetSolidList().SelectMany(s => s.Faces.Cast<Face>());
            var results = new List<Face>();

            foreach (var face in faces)
            {
                var box = face.GetBoundingBox();
                var min = box.Min;
                var noraml = face.ComputeNormal(min);

                if (dirs.Length == 0 || dirs.Any(f => f.AngleTo(noraml) < Math.Pow(1, -eps)))
                    results.Add(face);
            }

            return results;
        }

        /// <summary>
        ///     Gets the element's face set for the specified direction list.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="dirs"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static List<Face> GetFaceList(this Element elm, IEnumerable<XYZ> dirs, int eps = 6)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var faces = elm.GetSolidList().SelectMany(s => s.Faces.Cast<Face>());
            var results = new List<Face>();

            foreach (var face in faces)
            {
                var box = face.GetBoundingBox();
                var min = box.Min;
                var noraml = face.ComputeNormal(min);

                if (!dirs.Any() || dirs.Any(f => f.AngleTo(noraml) < Math.Pow(1, -eps)))
                    results.Add(face);
            }

            return results;
        }

        /// <summary>
        ///     Gets the element's edge list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Edge> GetEdgeList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            return elm.GetSolidList().SelectMany(f => f.Edges.Cast<Edge>()).ToList();
        }

        /// <summary>
        ///     Gets the element's valid solid list.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static List<Solid> GetSolidList(this Element elm, ViewDetailLevel lvl = default)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                throw new InvalidDataException(nameof(elm));

            var opt = new Options { ComputeReferences = true, DetailLevel = lvl };

            var ge = elm.get_Geometry(opt);

            return ge is null ? new List<Solid>() : ge.GetSolidList();
        }

        /// <summary>
        ///     Gets the element's valid solid list.
        /// </summary>
        /// <param name="ge"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static List<Solid> GetSolidList(this GeometryElement ge, int eps = 9)
        {
            if (ge is null)
                throw new ArgumentNullException(nameof(ge));

            var results = new List<Solid>();

            foreach (var obj in ge)
            {
                if (obj is Solid solid1 && solid1.Volume < Math.Pow(eps, -eps))
                    continue;

                if (obj is Solid solid)
                    results.Add(solid);

                else if (obj is GeometryInstance gi)
                {
                    var subGe = gi.GetInstanceGeometry();

                    if (subGe != null)
                        results = results.Union(subGe.GetSolidList(eps)).ToList();

                    if (gi.GetSymbolGeometry() != null)
                        results = results.Union(subGe.GetSolidList(eps)).ToList();
                }

                else if (obj is GeometryElement subGe)
                    results = results.Union(subGe.GetSolidList(eps)).ToList();
            }

            return results;
        }

    }
}
