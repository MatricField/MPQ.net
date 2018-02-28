using System;
using System.Collections.Generic;
using System.Text;

namespace MPQNet.Compression.Huffman
{
    public sealed class TreeItem
    {
        /// <summary>
        /// Lower-weight tree item
        /// </summary>
        public TreeItem Next { get; set; }

        /// <summary>
        /// Higher-weight item
        /// </summary>
        public TreeItem Prev { get; set; }
        public TreeItem Parent { get; set; }
        public TreeItem ChildLo { get; set; }

        public void RemoveItem()
        {

        }
    }
}
