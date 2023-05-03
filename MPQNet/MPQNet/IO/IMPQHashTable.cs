using MPQNet.Definition;

namespace MPQNet.IO
{
    internal interface IMPQHashTable
    {
        RawBlockEntry this[string key] { get; }

        bool TryGetValue(string key, out RawBlockEntry value);
    }
}