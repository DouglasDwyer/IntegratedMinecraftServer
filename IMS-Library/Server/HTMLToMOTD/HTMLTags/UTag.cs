using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    internal class UTag : HTMLStyledNodePart
    {
        public override void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers)
        {
            modifiers.Underline = true;
            ApplyStylingData(nodeData, modifiers);
        }

        public override void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers)
        {
            modifiers.Underline = false;
        }
    }
}
