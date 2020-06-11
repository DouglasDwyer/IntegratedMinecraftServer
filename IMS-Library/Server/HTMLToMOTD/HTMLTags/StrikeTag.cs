using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    internal class StrikeTag : HTMLStyledNodePart
    {
        public override void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers)
        {
            modifiers.Strikethrough = true;
            ApplyStylingData(nodeData, modifiers);
        }

        public override void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers)
        {
            modifiers.Strikethrough = false;
        }
    }
}
