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
     |  |              Creation Time: 05/13/2020 06:02:02 PM |  |  |     |         |      |
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
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Power.Revit.Properties;
using KeLi.Power.Revit.Widgets;

namespace KeLi.Power.Revit.Builders
{
    /// <summary>
    ///     Wall's belt builder.
    /// </summary>
    public static class WallBeltBuilder
    {
        /// <summary>
        ///     Creates wall's belt.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="sweeps"></param>
        public static void CreateWallSweep(this Wall wall, List<BeltInfo> sweeps)
        {
            if (wall is null)
                return;

            if (sweeps == null || sweeps.Count == 0)
                return;

            var doc = wall.Document;
            var symbols = doc.GetTypeList<FamilySymbol>();
            var materials = doc.GetInstanceList<Material>();
            var datas = new List<BeltData>();

            var defIndex = sweeps.FindIndex(f => f.IsAbsolute);
            var defProfile = symbols.FirstOrDefault(f => f.Name == sweeps[defIndex].ProfileName);
            var defMaterial = materials.FirstOrDefault(f => f.Name == sweeps[defIndex].MaterialName);
            var defOffset = sweeps[defIndex].Distance;

            if (defMaterial != null && defProfile != null)
                datas.Add(new BeltData(defIndex, defProfile.Id, defMaterial.Id, defOffset));

            // ↑
            for (var i = defIndex - 1; i >= 0; i--)
            {
                var belowProfile = symbols.FirstOrDefault(f => f.Name == sweeps[i + 1].ProfileName);

                if (belowProfile != null)
                {
                    var itemProfile = symbols.FirstOrDefault(f => f.Name == sweeps[i].ProfileName);

                    if (itemProfile == null)
                        continue;

                    var belowHeight = belowProfile.LookupParameter(Resources.WallBelt_Height).AsDouble();
                    var belowOffset = sweeps[i + 1].Distance;
                    var belowReverse = sweeps[i + 1].Flip;

                    var itemHeight = itemProfile.LookupParameter(Resources.WallBelt_Height).AsDouble();
                    var itemMaterialId = materials.FirstOrDefault(f => f.Name == sweeps[i].MaterialName)?.Id;
                    var itemDistance = sweeps[i].Distance;
                    var itemReverse = sweeps[i].Flip;
                    var itemIsAbs = sweeps[i].IsAbsolute;
                    double itemOffset;

                    if (itemIsAbs)
                        itemOffset = itemDistance;

                    else if (!itemReverse && !belowReverse)
                        itemOffset = belowOffset + belowHeight + itemDistance;

                    else if (!itemReverse)
                        itemOffset = belowOffset + itemDistance;

                    else if (!belowReverse)
                        itemOffset = belowOffset + belowHeight + itemDistance + itemHeight;

                    else
                        itemOffset = belowOffset + itemDistance + itemHeight;

                    sweeps[i].Distance = itemOffset;
                    datas.Add(new BeltData(i, itemProfile.Id, itemMaterialId, itemOffset) { Flip = itemReverse, IsAbsolute = itemIsAbs });
                }
            }

            // ↓
            for (var i = defIndex + 1; i < sweeps.Count; i++)
            {
                var aboveProfile = symbols.FirstOrDefault(f => f.Name == sweeps[i - 1].ProfileName);

                if (aboveProfile == null)
                    continue;

                var itemProfile = symbols.FirstOrDefault(f => f.Name == sweeps[i].ProfileName);

                if (itemProfile != null)
                {
                    var aboveHeight = aboveProfile.LookupParameter(Resources.WallBelt_Height).AsDouble();
                    var aboveReverse = sweeps[i - 1].Flip;
                    var aboveOffset = sweeps[i - 1].Distance;

                    var itemHeight = itemProfile.LookupParameter(Resources.WallBelt_Height).AsDouble();
                    var itemMaterialId = materials.FirstOrDefault(f => f.Name == sweeps[i].MaterialName)?.Id;
                    var itemDistance = sweeps[i].Distance;
                    var itemReverse = sweeps[i].Flip;
                    var itemIsAbs = sweeps[i].IsAbsolute;
                    double itemOffset;

                    if (itemIsAbs)
                        itemOffset = itemDistance;

                    else if (!itemReverse && !aboveReverse)
                        itemOffset = aboveOffset - itemDistance - itemHeight;

                    else if (!itemReverse)
                        itemOffset = aboveOffset - aboveHeight - itemDistance - itemHeight;

                    else if (!aboveReverse)
                        itemOffset = aboveOffset - itemDistance;

                    else
                        itemOffset = aboveOffset - aboveHeight - itemDistance;

                    sweeps[i].Distance = itemOffset;
                    datas.Add(new BeltData(i, itemProfile.Id, itemMaterialId, itemOffset) { Flip = itemReverse, IsAbsolute = itemIsAbs });
                }
            }

            if (wall.WallType.Duplicate(Guid.NewGuid().ToString()) is WallType wallType)
            {
                var cs = wallType.GetCompoundStructure();
                var sweepInfos = cs.GetWallSweepsInfo(WallSweepType.Sweep);

                sweepInfos.ToList().ForEach(f => cs.RemoveWallSweep(WallSweepType.Sweep, f.Id));
                wallType.SetCompoundStructure(cs);
            }

            doc.CreateWallBeltList(wall.WallType.Id, datas);
        }

        /// <summary>
        ///     Creates wall's belt list order by id.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wallTypeId"></param>
        /// <param name="belts"></param>
        public static void CreateWallBeltList(this Document doc, ElementId wallTypeId, IEnumerable<BeltData> belts)
        {
            belts = belts.OrderBy(o => o.Index);
            belts.ToList().ForEach(f => doc.CreateWallBelt(wallTypeId, f));
        }

        /// <summary>
        ///     Creates wall's belt.
        /// </summary>
        /// <param name="wallTypeId"></param>
        /// <param name="sweep"></param>
        /// <param name="doc"></param>
        public static void CreateWallBelt(this Document doc, ElementId wallTypeId, BeltData sweep)
        {
            if (doc is null)
                return;

            if (wallTypeId is null)
                return;

            if (sweep is null)
                return;

            if (doc.GetElement(wallTypeId) is WallType wallType)
            {
                var cs = wallType.GetCompoundStructure();

                var sweepInfo = new WallSweepInfo(true, WallSweepType.Sweep) { Id = sweep.Index + 1 };

                if (sweep.MaterialId != null)
                    sweepInfo.MaterialId = sweep.MaterialId;

                if (sweep.ProfileId != null)
                    sweepInfo.ProfileId = sweep.ProfileId;

                sweepInfo.Distance = sweep.Offset;
                sweepInfo.IsProfileFlipped = sweep.Flip;

                cs.AddWallSweep(sweepInfo);
                wallType.SetCompoundStructure(cs);
            }
        }
    }

    /// <summary>
    ///     Wall's belt info.
    /// </summary>
    public class BeltInfo
    {
        /// <summary>
        ///     Wall's belt info.
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="materialName"></param>
        /// <param name="distance"></param>
        public BeltInfo(string profileName, string materialName, double distance)
        {
            ProfileName = profileName;
            MaterialName = materialName;
            Distance = distance;
        }

        /// <summary>
        ///     ProfileName
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        ///     MaterialName
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        ///     Distance
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        ///     Flip
        /// </summary>
        public bool Flip { get; set; }

        /// <summary>
        ///     IsAbsolute
        /// </summary>
        public bool IsAbsolute { get; set; }
    }

    /// <summary>
    ///     Wall's belt data.
    /// </summary>
    public class BeltData
    {
        /// <summary>
        ///     Wall sweep data.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="profileId"></param>
        /// <param name="materialId"></param>
        /// <param name="offset"></param>
        public BeltData(int index, ElementId profileId, ElementId materialId, double offset)
        {
            Index = index;
            ProfileId = profileId;
            MaterialId = materialId;
            Offset = offset;
        }

        /// <summary>
        ///     Index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     ProfileId
        /// </summary>
        public ElementId ProfileId { get; set; }

        /// <summary>
        ///     MaterialId
        /// </summary>
        public ElementId MaterialId { get; set; }

        /// <summary>
        ///     Offset
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        ///     Flip
        /// </summary>
        public bool Flip { get; set; }

        /// <summary>
        ///     IsAbsolute
        /// </summary>
        public bool IsAbsolute { get; set; }
    }
}