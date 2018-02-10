using System;
using System.Text;

namespace MPQNet.HeaderDefinition
{
    /// <summary>
    /// Header signatures of MPQ headers
    /// </summary>
    public enum HeaderSignatures : uint
    {
        /// <summary>
        /// MPQ archive header ID ('MPQ\x1A')
        /// </summary>
        MPQ = 0x1A51504D,

        /// <summary>
        /// MPQ userdata entry ('MPQ\x1B')
        /// </summary>
        MPQ_UserData = 0x1B51504D,

        /// <summary>
        /// MPK archive header ID ('MPK\x1A')
        /// </summary>
        MPK = 0x1A4B504D,
    }
}
