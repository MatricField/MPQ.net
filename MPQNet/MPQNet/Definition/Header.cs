using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.Definition
{
    internal abstract record class Header
    {
        protected long _BaseAddress;

        public virtual required long BaseAddress { get => _BaseAddress; init => _BaseAddress = value; }

        protected Header()
        {

        }

        [SetsRequiredMembers]
        protected Header(long baseAddress)
        {
            _BaseAddress = baseAddress;
        }
    }
}
