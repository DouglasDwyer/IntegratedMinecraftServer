using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    internal class DivTag : HTMLStyledNodePart
    {
        public override void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers)
        {
            finalOutput += "\\n";
            ApplyStylingData(nodeData, modifiers);
        }

        public override void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers)
        {
            RemoveStylingData(modifiers);
        }
    }
}
