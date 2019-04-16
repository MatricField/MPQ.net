namespace MPQNet.IO
{
    public interface IMPQHashTable
    {
        IMPQFileInfo this[string key] { get; }

        bool TryGetValue(string key, out IMPQFileInfo value);
    }
}