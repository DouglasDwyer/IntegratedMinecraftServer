using IMS_Library.HTMLToMOTD;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IMS_Library.HTMLToMOTD
{
    public abstract class HTMLStyledNodePart : HTMLNodePart
    {
        protected List<Action<HTMLActiveModifiers>> StylingDataRemovalActions = new List<Action<HTMLActiveModifiers>>();

        public void ApplyStylingData(string nodeData, HTMLActiveModifiers modifiers)
        {
            if(!string.IsNullOrEmpty(nodeData))
            {
                Match match = null;
                if(MatchRegex(nodeData, "style=\"(.*?)\"", out match))
                {
                    nodeData = match.Groups[1].Value;
                    match = null;
                    foreach(string subpart in nodeData.Split(";"))
                    {
                        if(MatchRegex(subpart, @"color: rgb\(([0-9]*), ([0-9]*), ([0-9]*)\)", out match))
                        {
                            string color = "#";
                            color += BitConverter.ToString(new byte[] { byte.Parse(match.Groups[1].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[3].Value) }).Replace("-", "").ToLower();
                            modifiers.Color = color;
                            StylingDataRemovalActions.Add(m => m.Color = null);
                        }
                    }
                }
            }
        }

        public void RemoveStylingData(HTMLActiveModifiers modifiers)
        {
            foreach(Action<HTMLActiveModifiers> action in StylingDataRemovalActions)
            {
                action(modifiers);
            }
        }

        protected bool MatchRegex(string text, string pattern, out Match match)
        {
            Regex regex = new Regex(pattern);
            match = regex.Match(text);
            return match.Success;
        }
    }
}
