using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MPQNet.UnitTests.StormLib
{
    internal static class Interop
    {
        private const string STORMLIB_PATH = @"lib\bin\storm.dll";

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free(IntPtr obj);

        [DllImport(STORMLIB_PATH, CallingConvention = CallingConvention.Winapi)]
        public static extern bool SFileOpenArchive(
            [MarshalAs(UnmanagedType.LPStr)]string filename,
            uint prioerity,
            uint openFlags,
            out IntPtr phMPQ);

        [DllImport(STORMLIB_PATH, CallingConvention = CallingConvention.Winapi)]
        public static extern bool SFileCloseArchive(IntPtr hMPQ);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OpenArchive(string filename, out IntPtr phMPQ)
        {
            return SFileOpenArchive(filename, 0, 0, out phMPQ);
        }
    }
}
