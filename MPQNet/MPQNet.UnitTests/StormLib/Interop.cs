using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.UnitTests.StormLib
{
    internal static class Interop
    {
        private static class Win32
        {
            [DllImport(@"Lib\StormLibWin32.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileOpenArchive(
                [MarshalAs(UnmanagedType.LPStr)]string filename,
                uint prioerity,
                uint openFlags,
                out IntPtr phMPQ);

            [DllImport(@"Lib\StormLibWin32.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileCloseArchive(IntPtr hMPQ);
        }

        private static class Win64
        {
            [DllImport(@"Lib\StormLibWin64.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileOpenArchive(
                [MarshalAs(UnmanagedType.LPStr)]string filename,
                uint prioerity,
                uint openFlags,
                out IntPtr phMPQ);

            [DllImport(@"Lib\StormLibWin64.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileCloseArchive(IntPtr hMPQ);
        }

        public static bool CloseArchive(IntPtr hMPQ)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        return Win32.SFileCloseArchive(hMPQ);
                    case Architecture.X64:
                        return Win64.SFileCloseArchive(hMPQ);
                }
            }
            throw new NotSupportedException();
        }

        public static bool OpenArchive(string filename, out IntPtr phMPQ)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        return Win32.SFileOpenArchive(filename, 0, 0, out phMPQ);
                    case Architecture.X64:
                        return Win64.SFileOpenArchive(filename, 0, 0, out phMPQ);
                }
            }
            throw new NotSupportedException();
        }
    }
}
