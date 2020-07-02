using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace KeLi.Power.Revit.Extensions
{
    /// <summary>
    ///     GenericForm extension.
    /// </summary>
    public static class GenericFormExtension
    {
        /// <summary>
        ///     Associates material.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="parmName"></param>
        /// <param name="isInstance"></param>
        public static void AssociateMaterial(this GenericForm elm, string parmName, bool isInstance = true)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (string.IsNullOrWhiteSpace(parmName))
                throw new ArgumentNullException(nameof(parmName));

            var mgr = elm.Document.FamilyManager;
            var familyParm = mgr.AddParameter(parmName, BuiltInParameterGroup.PG_MATERIALS, ParameterType.Material, isInstance);
            var parm = elm.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM);

            if (parm != null)
                mgr.AssociateElementParameterToFamilyParameter(parm, familyParm);
        }
    }
}
