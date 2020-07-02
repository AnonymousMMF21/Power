/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 01/15/2020 07:39:20 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Extensions;
using KeLi.Power.Revit.Filters;
using KeLi.Power.Revit.Widgets;

namespace KeLi.Power.Revit.Builders
{
    /// <summary>
    ///     Family loader.
    /// </summary>
    public static class FamilyLoader
    {
        /// <summary>
        ///     Creates a new extrusion symbol.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="boundary"></param>
        /// <param name="plane"></param>
        /// <param name="thick"></param>
        /// <returns></returns>
        public static Document CreateExtrusion(this Document doc, CurveArrArray boundary, Plane plane, double thick)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (boundary is null)
                throw new ArgumentNullException(nameof(boundary));

            if (plane is null)
                throw new ArgumentNullException(nameof(plane));

            var tplPath = doc.GetTemplateFilePath();

            if (!File.Exists(tplPath))
                throw new FileNotFoundException(tplPath);

            var fdoc = doc.Application.NewFamilyDocument(tplPath);

            fdoc.AutoTransaction(() =>
            {
                var skectchPlane = SketchPlane.Create(fdoc, plane);

                if (skectchPlane is null)
                    throw new NullReferenceException(nameof(skectchPlane));

                fdoc.FamilyCreate.NewExtrusion(true, boundary.ResetCurveArrArray(), skectchPlane, thick);
            });

            return fdoc;
        }

        /// <summary>
        ///     Creates a new sweep symbol with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="profile"></param>
        /// <param name="path"></param>
        /// <param name="loc"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Document CreateSweep(this Document doc, CurveArrArray profile, ReferenceArray path, ProfilePlaneLocation loc = default, int index = 0)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            if (profile.Size == 0 || profile.ToCurveList().Count == 0)
                throw new InvalidDataException(nameof(path));

            if (path is null)
                throw new ArgumentNullException(nameof(path));

            if (path.Size == 0)
                throw new InvalidDataException(nameof(path));

            var tplPath = doc.GetTemplateFilePath();

            if (!File.Exists(tplPath))
                throw new FileNotFoundException(tplPath);

            var fdoc = doc.Application.NewFamilyDocument(tplPath);

            fdoc.AutoTransaction(() =>
            {
                var dprofile = doc.Application.Create.NewCurveLoopsProfile(profile.ResetCurveArrArray());

                fdoc.FamilyCreate.NewSweep(true, path, dprofile, index, loc);
            });

            return fdoc;
        }

        /// <summary>
        ///     Loads family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="rfaPath"></param>
        /// <returns></returns>
        public static FamilySymbol NewLoadFamily(this Document doc, string rfaPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (string.IsNullOrWhiteSpace(rfaPath))
                throw new ArgumentNullException(nameof(rfaPath));

            if (!File.Exists(rfaPath))
                throw new FileNotFoundException(rfaPath);

            doc.LoadFamily(rfaPath, new OverrideLoadOption(), out var family);

            if (family == null)
            {
                var families = doc.GetInstanceList<Family>();

                family = families.FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(rfaPath));
            }

            if (family != null)
            {
                var symbolId = family.GetFamilySymbolIds().FirstOrDefault();

                var result = doc.GetElement(symbolId) as FamilySymbol;

                if (result != null && !result.IsActive)
                    result.Activate();

                return result;
            }

            throw new FileLoadException(nameof(rfaPath));
        }

        /// <summary>
        ///     Loads family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fdoc"></param>
        /// <returns></returns>
        public static FamilySymbol NewLoadFamily(this Document doc, Document fdoc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (fdoc is null)
                throw new ArgumentNullException(nameof(fdoc));

            var family = fdoc.LoadFamily(doc);
            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();
            var result = doc.GetElement(symbolId) as FamilySymbol;

            if (result != null && !result.IsActive)
                result.Activate();

            return result;
        }
    }

    /// <summary>
    ///     Override load option
    /// </summary>
    public class OverrideLoadOption : IFamilyLoadOptions
    {
        /// <summary>
        ///     When the family existed in another document.
        /// </summary>
        /// <param name="familyInUse"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool OnFamilyFound(bool familyInUse, out bool overwrite)
        {
            overwrite = true;

            return true;
        }

        /// <summary>
        ///     When the shared family existed in another document.
        /// </summary>
        /// <param name="sharedFamily"></param>
        /// <param name="familyInUse"></param>
        /// <param name="source"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwrite)
        {
            overwrite = true;

            source = FamilySource.Family;

            return true;
        }
    }
}