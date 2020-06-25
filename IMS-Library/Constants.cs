using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// This class contains constant data that is necessary for program operation, like the paths to various IMS files.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// This is the name of the IMS Windows service.
        /// </summary>
        public const string ServiceName = "IMS";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, that contains data necessary for IMS's operation.
        /// </summary>
        public const string DataLocation = "/Data";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, which contains IMS logfiles.
        /// </summary>
        public const string LogLocation = DataLocation + "/Log";
        /// <summary>
        /// This is the path, relative to the IMS root directory, of the IMS settings file.
        /// </summary>
        public const string ConfigurationFilePath = DataLocation + "/imssettings.xml";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, which contains Minecraft server data.
        /// </summary>
        public const string ServerFolderLocation = DataLocation + "/Server";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, which contains Minecraft world data.
        /// </summary>
        public const string WorldFolderLocation = DataLocation + "/World";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, which contains executable assemblies that IMS uses (like the JRE).
        /// </summary>
        public const string BinariesFolderLocation = DataLocation + "/Binaries";
        /// <summary>
        /// This is the folder, relative to the IMS root directory, which contains Minecraft server executables.
        /// </summary>
        public const string JavaBinariesFolderLocation = BinariesFolderLocation + "/Servers";
        /// <summary>
        /// This is the path, relative to the IMS root directory, of the bundled Java runtime environment.
        /// </summary>
        public const string JavaExecutableLocation = BinariesFolderLocation + "/Java/bin/java.exe";
        /// <summary>
        /// This the path, relative to the IMS root directory, of the folder which contains all of the data necessary for a Bedrock server to run.
        /// </summary>
        public const string BedrockBinariesFolderLocation = BinariesFolderLocation + "/BedrockTemplates";
        /// <summary>
        /// This is the path, relative to the IMS root directory, of the folder used to store IMS plugins.
        /// </summary>
        public const string PluginFolderLocation = DataLocation + "/Plugin";
        /// <summary>
        /// This is the path of the current IMS root directory.
        /// </summary>
        public static readonly string ExecutionPath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();

        /// <summary>
        /// This is how often the <see cref="PortForwarder"/> will check to see whether IMS is still connected to a UPnP router.
        /// </summary>
        public const int CheckToEnsureNATConnectedInterval = 60000; //ms
        //public const int ComponentRestartClearTime = 10; //s

        /// <summary>
        /// This is a dictionary which can be used to convert hex color codes (in the format "#rrggbb") to Minecraft formatting letter codes.
        /// </summary>
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
