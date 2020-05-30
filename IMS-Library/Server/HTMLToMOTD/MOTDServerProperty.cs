using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace IMS_Library.HTMLToMOTD
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MOTDServerProperty : ServerProperty
    {
        protected static readonly Dictionary<string, Type> HTMLFormattingTags = new Dictionary<string, Type>();

        protected Stack<HTMLNodePart> NodeParts = new Stack<HTMLNodePart>();

        static MOTDServerProperty()
        {
            HTMLFormattingTags["b"] = typeof(BTag);
            HTMLFormattingTags["div"] = typeof(DivTag);
            HTMLFormattingTags["font"] = typeof(FontTag);
            HTMLFormattingTags["i"] = typeof(ITag);
            HTMLFormattingTags["u"] = typeof(UTag);
            HTMLFormattingTags["strike"] = typeof(StrikeTag);
            HTMLFormattingTags["sup"] = typeof(SupTag);
        }

        public MOTDServerProperty(string propertyName) : base(propertyName)
        {
        }

        public override string GetData(ServerConfiguration configuration, FieldInfo field)
        {
            string[] data = Regex.Split((string)field.GetValue(configuration), @"(?=<)|(?<=>)");
            string finalString = PropertyName + "=" + HTMLActiveModifiers.FormattingResetCharacter;
            HTMLActiveModifiers textModifiers = new HTMLActiveModifiers();
            foreach(string subelement in data)
            {
                if(subelement.StartsWith("<"))
                {
                    try
                    {
                        if(subelement.StartsWith("</"))
                        {
                            NodeParts.Pop().RemoveModifiers(ref finalString, textModifiers);
                            finalString += textModifiers.GetFormattingCodes();
                        }
                        else
                        {
                            string[] subelementData = Regex.Split(subelement, @"^<([a-zA-z]*)(?: ([^>]*))?>$");
                            string nodeData = subelementData.Length == 4 ? subelementData[2] : "";
                            HTMLNodePart part = (Activator.CreateInstance(HTMLFormattingTags[subelementData[1]]) as HTMLNodePart);
                            part.ApplyModifiers(ref finalString, nodeData, textModifiers);
                            NodeParts.Push(part);
                            finalString += textModifiers.GetFormattingCodes();
                        }
                    }
                    catch(Exception e)
                    {
                        Logger.WriteWarning("Couldn't parse MOTD data!\n" + e);
                        finalString += WebUtility.HtmlDecode(subelement.Replace("&nbsp;", " "));
                    }
                }
                else
                {
                    finalString += WebUtility.HtmlDecode(subelement.Replace("&nbsp;", " "));
                }
            }
            return finalString;
        }

        private bool CheckRegexMatch(string input, string pattern, out Match match)
        {
            Regex regex = new Regex(pattern);
            match = regex.Match(input);
            return match.Success;
        }
    }
}
