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

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free(IntPtr obj);
    }
}
