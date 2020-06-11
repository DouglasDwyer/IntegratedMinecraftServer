using System;
using System.Collections.Generic;
using System.Text;

namespace IMS_Library.HTMLToMOTD
{
    internal class HTMLActiveModifiers
    {
        public bool Bold, Italic, Underline, Strikethrough, Obfuscated;
        public string Color;

        public const string FormattingCharacter = "\\u00A7";
        public const string FormattingResetCharacter = FormattingCharacter + "r";

        public string GetFormattingCodes()
        {
            string finalString = FormattingResetCharacter;
            if (!string.IsNullOrEmpty(Color))
            {
                finalString += FormattingCharacter + Constants.MinecraftColorsHexAndFormattingCodes[Color];
            }
            if (Bold)
            {
                finalString += FormattingCharacter + "l";
            }
            if (Italic)
            {
                finalString += FormattingCharacter + "o";
            }
            if (Underline)
            {
                finalString += FormattingCharacter + "n";
            }
            if (Strikethrough)
            {
                finalString += FormattingCharacter + "m";
            }
            if (Obfuscated)
            {
                finalString += FormattingCharacter + "k";
            }
            return finalString;
        }
    }
}
