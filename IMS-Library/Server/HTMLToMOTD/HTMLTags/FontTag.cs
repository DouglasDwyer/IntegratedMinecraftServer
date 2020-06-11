using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IMS_Library.HTMLToMOTD
{
    internal class FontTag : HTMLStyledNodePart
    {
        public override void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers)
        {
            Regex regex = new Regex("color=\"([#0123456789abcdef]*)\"");
            Match match = regex.Match(nodeData);
            if(match.Success)
            {
                modifiers.Color = match.Groups[1].Value;
                nodeData = nodeData.Replace(match.Value, "");
            }
            ApplyStylingData(nodeData, modifiers);
        }

        public override void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers)
        {
            modifiers.Color = null;
        }
    }
}
