using System;
using System.Collections.Generic;
using System.Text;

//Helpful code taken from https://stackoverflow.com/questions/44174614/similar-method-to-messagebox-in-net-core
namespace WindowManager
{
    public enum MsgBoxResult
        : int
    {
        Abort = 3,
        Cancel = 2,
        Ignore = 5,
        No = 7,
        Ok = 1,
        Retry = 4,
        Yes = 6
    }

    //[System.Flags]
    //public enum MsgBoxStyle
    //    : int
    //{
    //    AbortRetryIgnore = 2,
    //    ApplicationModal = 0,
    //    Critical = 0x10,
    //    DefaultButton1 = 0,
    //    DefaultButton2 = 0x100,
    //    DefaultButton3 = 0x200,
    //    Exclamation = 0x30,
    //    Information = 0x40,
    //    MsgBoxHelp = 0x4000,
    //    MsgBoxRight = 0x80000,
    //    MsgBoxRtlReading = 0x100000,
    //    MsgBoxSetForeground = 0x10000,
    //    OkCancel = 1,
    //    OkOnly = 0,
    //    Question = 0x20,
    //    RetryCancel = 5,
    //    SystemModal = 0x1000,
    //    YesNo = 4,
    //    YesNoCancel = 3
    //}


    [System.Flags]
    public enum MsgBoxStyle
    {
        /// <summary>
        /// OK button only (default). This member is equivalent to the Visual Basic constant <see langword="vbOKOnly" />.</summary>
        OkOnly = 0,
        /// <summary>
        /// OK and Cancel buttons. This member is equivalent to the Visual Basic constant <see langword="vbOKCancel" />.</summary>
        OkCancel = 1,
        /// <summary>
        /// Abort, Retry, and Ignore buttons. This member is equivalent to the Visual Basic constant <see langword="vbAbortRetryIgnore" />.</summary>
        AbortRetryIgnore = 2,
        /// <summary>
        /// Yes, No, and Cancel buttons. This member is equivalent to the Visual Basic constant <see langword="vbYesNoCancel" />.</summary>
        YesNoCancel = AbortRetryIgnore | OkCancel, // 0x00000003
        /// <summary>
        /// Yes and No buttons. This member is equivalent to the Visual Basic constant <see langword="vbYesNo" />.</summary>
        YesNo = 4,
        /// <summary>
        /// Retry and Cancel buttons. This member is equivalent to the Visual Basic constant <see langword="vbRetryCancel" />.</summary>
        RetryCancel = YesNo | OkCancel, // 0x00000005
        /// <summary>Critical message. This member is equivalent to the Visual Basic constant <see langword="vbCritical" />.</summary>
        Critical = 16, // 0x00000010
        /// <summary>Warning query. This member is equivalent to the Visual Basic constant <see langword="vbQuestion" />.</summary>
        Question = 32, // 0x00000020
        /// <summary>Warning message. This member is equivalent to the Visual Basic constant <see langword="vbExclamation" />.</summary>
        Exclamation = Question | Critical, // 0x00000030
        /// <summary>Information message. This member is equivalent to the Visual Basic constant <see langword="vbInformation" />.</summary>
        Information = 64, // 0x00000040
        /// <summary>First button is default. This member is equivalent to the Visual Basic constant <see langword="vbDefaultButton1" />.</summary>
        DefaultButton1 = 0,
        /// <summary>Second button is default. This member is equivalent to the Visual Basic constant <see langword="vbDefaultButton2" />.</summary>
        DefaultButton2 = 256, // 0x00000100
        /// <summary>Third button is default. This member is equivalent to the Visual Basic constant <see langword="vbDefaultButton3" />.</summary>
        DefaultButton3 = 512, // 0x00000200
        /// <summary>Application modal message box. This member is equivalent to the Visual Basic constant <see langword="vbApplicationModal" />.</summary>
        ApplicationModal = 0,
        /// <summary>System modal message box. This member is equivalent to the Visual Basic constant <see langword="vbSystemModal" />.</summary>
        SystemModal = 4096, // 0x00001000
        /// <summary>Help text. This member is equivalent to the Visual Basic constant <see langword="vbMsgBoxHelp" />.</summary>
        MsgBoxHelp = 16384, // 0x00004000
        /// <summary>Right-aligned text. This member is equivalent to the Visual Basic constant <see langword="vbMsgBoxRight" />.</summary>
        MsgBoxRight = 524288, // 0x00080000
        /// <summary>Right-to-left reading text (Hebrew and Arabic systems). This member is equivalent to the Visual Basic constant <see langword="vbMsgBoxRtlReading" />.</summary>
        MsgBoxRtlReading = 1048576, // 0x00100000
        /// <summary>Foreground message box window. This member is equivalent to the Visual Basic constant <see langword="vbMsgBoxSetForeground" />.</summary>
        MsgBoxSetForeground = 65536, // 0x00010000
    }


    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-messagebox
    internal class UnsafeNativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern MsgBoxResult MessageBox(System.IntPtr hWnd, string text, string caption, MsgBoxStyle options);
    }


    public class Interaction
    {


        private static string GetTitleFromAssembly(System.Reflection.Assembly CallingAssembly)
        {
            try
            {
                return CallingAssembly.GetName().Name;
            }
            catch (System.Security.SecurityException)
            {
                string fullName = CallingAssembly.FullName;
                int index = fullName.IndexOf(',');
                if (index >= 0)
                {
                    return fullName.Substring(0, index);
                }
                return "";
            }
        }


        public static MsgBoxResult MsgBox(string text, string caption, MsgBoxStyle options)
        {
            if (string.IsNullOrEmpty(caption))
                caption = GetTitleFromAssembly(System.Reflection.Assembly.GetCallingAssembly());

            if (System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                return UnsafeNativeMethods.MessageBox(System.IntPtr.Zero, text, caption, options);

            text = text.Replace("\"", @"\""");
            caption = caption.Replace("\"", @"\""");

            using (System.Diagnostics.Process p = System.Diagnostics.Process.Start("notify-send", "\"" + caption + "\" \"" + text + "\""))
            {
                p.WaitForExit();
            }

            return MsgBoxResult.Ok;
        }


        public static MsgBoxResult MsgBox(string text, string caption)
        {
            return MsgBox(text, caption, MsgBoxStyle.OkOnly);
        }


        public static MsgBoxResult MsgBox(string text)
        {
            return MsgBox(text, null);
        }


        public static MsgBoxResult MsgBox(object objText, object objCaption)
        {
            string text = System.Convert.ToString(objText, System.Globalization.CultureInfo.InvariantCulture);
            string caption = System.Convert.ToString(objCaption, System.Globalization.CultureInfo.InvariantCulture);

            return MsgBox(text, caption);
        }


        public static MsgBoxResult MsgBox(object objText)
        {
            return MsgBox(objText, null);
        }


    } // End Class Interaction


} // End Namespace WindowManager 
