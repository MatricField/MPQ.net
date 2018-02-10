using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.HeaderDefinition
{
    /// <summary>
    /// Supported versions of MPQ Format
    /// </summary>
    public enum FormatVersions : ushort
    {
        /// <summary>
        /// Used up to The Burning Crusade
        /// </summary>
        V1 = 0,

        /// <summary>
        /// The Burning Crusade and newer
        /// </summary>
        V2 = 1,

        /// <summary>
        /// WoW - Cataclysm beta or newer
        /// </summary>
        V3 = 2,

        /// <summary>
        /// WoW - Cataclysm beta or newer
        /// </summary>
        V4 = 3,
    }
}
