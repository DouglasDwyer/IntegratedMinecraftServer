using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    internal class ITag : HTMLStyledNodePart
    {
        public override void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers)
        {
            modifiers.Italic = true;
            ApplyStylingData(nodeData, modifiers);
        }

        public override void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers)
        {
            modifiers.Italic = false;
        }
    }
}
