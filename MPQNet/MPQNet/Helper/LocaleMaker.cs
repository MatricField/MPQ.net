using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Helper
{
    public static class LocaleMaker
    {
        public static int MAKELANGID(int primary, int sub) => (((ushort)sub) << 10) | ((ushort)primary);

        public static int PRIMARYLANGID(int lcid) => ((ushort)lcid) & 0x3ff;

        public static int SUBLANGID(int lcid) => ((ushort)lcid) >> 10;
    }
}
