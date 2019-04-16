using MPQNet.Definition;

namespace MPQNet.IO
{
    public interface IMPQHashTable
    {
        BlockEntry this[string key] { get; }

        bool TryGetValue(string key, out BlockEntry value);
    }
}