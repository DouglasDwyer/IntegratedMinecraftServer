using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    public abstract class HTMLNodePart
    {
        public abstract void ApplyModifiers(ref string finalOutput, string nodeData, HTMLActiveModifiers modifiers);
        public abstract void RemoveModifiers(ref string finalOutput, HTMLActiveModifiers modifiers);
    }
}
