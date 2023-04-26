using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RawUserData
    {
        // Maximum size of the user data
        uint cbUserDataSize;

        // Offset of the MPQ header, relative to the begin of this header
        uint dwHeaderOffs;

        // Appears to be size of user data header (Starcraft II maps)
        uint cbUserDataHeader;
    }
}
