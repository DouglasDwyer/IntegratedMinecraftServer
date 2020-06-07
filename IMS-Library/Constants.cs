using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public static class Constants
    {
        public const string ServiceName = "IMS";
        public const string WebsiteLocation = "/Web";
        public const string DataLocation = "/Data";
        public const string LogLocation = DataLocation + "/Log";
        public const string ConfigurationFilePath = DataLocation + "/imssettings.xml";
        public const string ServerFolderLocation = DataLocation + "/Server";
        public const string WorldFolderLocation = DataLocation + "/World";
        public const string BinariesFolderLocation = DataLocation + "/Binaries";
        public const string ServerBinariesFolderLocation = BinariesFolderLocation + "/Servers";
        public const string JavaExecutableLocation = BinariesFolderLocation + "/Java/bin/java.exe";
        public const string GraalVMExecutableLocation = BinariesFolderLocation + "/GraalVM/java.exe";
        public static readonly string ExecutionPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();

        public const int CheckToEnsureNATConnectedInterval = 60000; //ms
        public const int ComponentRestartClearTime = 10; //s
        public const int InternalServerCommunicationPort = 43891;

        public static readonly Dictionary<string, string> MinecraftColorsHexAndFormattingCodes = new Dictionary<string, string>();

        static Constants()
        {
            MinecraftColorsHexAndFormattingCodes["#000000"] = "0";
            MinecraftColorsHexAndFormattingCodes["#555555"] = "8";
            MinecraftColorsHexAndFormattingCodes["#aaaaaa"] = "7";
            MinecraftColorsHexAndFormattingCodes["#ffffff"] = "f";
            MinecraftColorsHexAndFormattingCodes["#0000aa"] = "1";
            MinecraftColorsHexAndFormattingCodes["#5555ff"] = "9";
            MinecraftColorsHexAndFormattingCodes["#00aaaa"] = "3";
            MinecraftColorsHexAndFormattingCodes["#55ffff"] = "b";
            MinecraftColorsHexAndFormattingCodes["#00aa00"] = "2";
            MinecraftColorsHexAndFormattingCodes["#55ff55"] = "a";
            MinecraftColorsHexAndFormattingCodes["#ffaa00"] = "6";
            MinecraftColorsHexAndFormattingCodes["#ffff55"] = "e";
            MinecraftColorsHexAndFormattingCodes["#aa0000"] = "4";
            MinecraftColorsHexAndFormattingCodes["#ff5555"] = "c";
            MinecraftColorsHexAndFormattingCodes["#aa00aa"] = "5";
            MinecraftColorsHexAndFormattingCodes["#ff55ff"] = "d";
        }
    }
}
