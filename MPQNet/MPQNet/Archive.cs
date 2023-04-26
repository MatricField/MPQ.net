using MPQNet.Definition;
using MPQNet.Helper;
using MPQNet.IO;
using MPQNet.IO.FileIO;
using MPQNet.IO.LowLevelIO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MPQNet
{
    //public static class Archive
    //{
    //    public static bool TryLoadArchiveFromFile(string path, out IArchive result)
    //    {
    //        var io = new MemoryMappedFileIO(path);
    //        using (var stream = io.GetFullArchiveStream())
    //        {
    //            var infoSize = Marshal.SizeOf<ArchiveInfo>();
    //            UserDataInfo userDataInfo = default;
    //            bool notStop = true;
    //            while (notStop)
    //            {
    //                var info = stream.MarshalObjectFromStream<ArchiveInfo>();
    //                stream.Seek(-infoSize, SeekOrigin.Current);
    //                switch (info.ID)
    //                {
    //                    case Signatures.MPQ_UserData:
    //                        var userDataHeaderOffset = stream.Position;
    //                        var userDataHeader = stream.MarshalObjectFromStream<UserDataHeader>();
    //                        userDataInfo.UserDataOffset = userDataHeaderOffset;
    //                        userDataInfo.UserDataSize = userDataHeader.UserDataSize;
    //                        stream.Seek(userDataHeaderOffset + userDataHeader.HeaderOffset, SeekOrigin.Begin);
    //                        break;
    //                    case Signatures.MPQ:
    //                        io.BaseOffset = stream.Position;
    //                        if(userDataInfo.UserDataSize > 0)
    //                        {
    //                            userDataInfo.UserDataOffset -= stream.Position;
    //                        }
    //                        switch (info.FormatVersion)
    //                        {
    //                            case FormatVersions.V4:
    //                                if(TryReadArchiveV4(io, userDataInfo, out result))
    //                                {
    //                                    return true;
    //                                }
    //                                else
    //                                {
    //                                    goto case FormatVersions.V3;
    //                                }
    //                            case FormatVersions.V3:
    //                                if (TryReadArchiveV3(io, userDataInfo, out result))
    //                                {
    //                                    return true;
    //                                }
    //                                else
    //                                {
    //                                    goto case FormatVersions.V2;
    //                                }
    //                            case FormatVersions.V2:
    //                              if (TryReadArchiveV2(io, userDataInfo, out result))
    //                                {
    //                                    return true;
    //                                }
    //                                else
    //                                {
    //                                    goto case FormatVersions.V1;
    //                                }
    //                            case FormatVersions.V1:
    //                              if (TryReadArchiveV1(io, userDataInfo, out result))
    //                                {
    //                                    return true;
    //                                }
    //                                else
    //                                {
    //                                    goto default;
    //                                }
    //                            default:
    //                                throw new NotSupportedException();
    //                        }
    //                    default:
    //                        var pos = stream.Position;
    //                        if (pos == stream.Seek(0x200, SeekOrigin.Current))
    //                        {
    //                            notStop = false;
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //        result = default;
    //        return false;
    //    }

    //    private static bool TryReadArchiveV1(ILowLevelIOHandler IOHandler, in UserDataInfo userData, out IArchive archive)
    //    {
    //        using (var headerStream = IOHandler.GetStream(0, Marshal.SizeOf<ArchiveHeader1>()))
    //        {
    //            var header = headerStream.MarshalObjectFromStream<ArchiveHeader1>();
    //            if (header.HashTableOffset != 0 && header.BlockTableOffset != 0)
    //            {
    //                var hashTable = new BlockHashTable(
    //                    IOHandler,
    //                    header.HashTableOffset,
    //                    Convert.ToInt32(header.HashTableEntriesCount),
    //                    header.BlockTableOffset,
    //                    Convert.ToInt32(header.BlockTableEntriesCount));
    //                var fileIO = new FileIOHandler(IOHandler, header.SectorSize);
    //                archive = new MPQArchive(fileIO, hashTable, userData);
    //                return true;
    //            }
    //            else
    //            {
    //                archive = null;
    //                return false;
    //            }
    //        }
    //    }
    //    private static bool TryReadArchiveV2(ILowLevelIOHandler IOHandler, in UserDataInfo userData, out IArchive archive)
    //    {
    //        //TODO: read archive v2
    //        archive = default;
    //        return false;
    //    }
    //    private static bool TryReadArchiveV3(ILowLevelIOHandler IOHandler, in UserDataInfo userData, out IArchive archive)
    //    {
    //        //TODO: read archive v3
    //        archive = default;
    //        return false;
    //    }
    //    private static bool TryReadArchiveV4(ILowLevelIOHandler IOHandler, in UserDataInfo userData, out IArchive archive)
    //    {
    //        //TODO: read archive v4
    //        archive = default;
    //        return false;
    //    }
    //}
}
