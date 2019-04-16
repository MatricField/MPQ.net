﻿using MPQNet.Definition;
using MPQNet.Helper;
using MPQNet.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MPQNet
{
    public static class Archive
    {
        public static bool TryLoadArchiveFromFile(string path, out IArchive result)
        {
            var io = new MemoryMappedFileIO(path);
            using (var stream = io.GetFullArchiveStream())
            {
                var infoSize = Marshal.SizeOf<ArchiveInfo>();
                UserDataHeader userDataHeader = default;
                bool notStop = true;
                while (notStop)
                {
                    var info = stream.MarshalObjectFromStream<ArchiveInfo>();
                    stream.Seek(-infoSize, SeekOrigin.Current);
                    switch (info.ID)
                    {
                        case Signatures.MPQ_UserData:
                            var userDataHeaderOffset = stream.Position;
                            userDataHeader = stream.MarshalObjectFromStream<UserDataHeader>();
                            stream.Seek(userDataHeaderOffset + userDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case Signatures.MPQ:
                            io.BaseOffset = stream.Position;
                            switch (info.FormatVersion)
                            {
                                case FormatVersions.V4:
                                    if(TryReadArchiveV4(io, userDataHeader, out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        goto case FormatVersions.V3;
                                    }
                                case FormatVersions.V3:
                                    if (TryReadArchiveV3(io, userDataHeader, out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        goto case FormatVersions.V2;
                                    }
                                case FormatVersions.V2:
                                  if (TryReadArchiveV2(io, userDataHeader, out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        goto case FormatVersions.V1;
                                    }
                                case FormatVersions.V1:
                                  if (TryReadArchiveV1(io, userDataHeader, out result))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        goto default;
                                    }
                                default:
                                    throw new NotSupportedException();
                            }
                        default:
                            var pos = stream.Position;
                            if (pos == stream.Seek(0x200, SeekOrigin.Current))
                            {
                                notStop = false;
                            }
                            break;
                    }
                }
            }
            result = default;
            return false;
        }

        private static bool TryReadArchiveV1(ILowLevelIOHandler IOHandler, UserDataHeader userData, out IArchive archive)
        {
            using (var headerStream = IOHandler.GetStream(0, Marshal.SizeOf<ArchiveHeader1>()))
            {
                var header = headerStream.MarshalObjectFromStream<ArchiveHeader1>();
                if (header.HashTableOffset != 0 && header.BlockTableOffset != 0)
                {
                    var hashTable = new BlockHashTable(
                        IOHandler,
                        header.HashTableOffset,
                        Convert.ToInt32(header.HashTableEntriesCount),
                        header.BlockTableOffset,
                        Convert.ToInt32(header.BlockTableEntriesCount));
                    archive = new BlockTableArchive(IOHandler, hashTable, userData);
                    return true;
                }
                else
                {
                    archive = null;
                    return false;
                }
            }
        }
        private static bool TryReadArchiveV2(ILowLevelIOHandler IOHandler, UserDataHeader userData, out IArchive archive)
        {
            //TODO: read archive v2
            archive = default;
            return false;
        }
        private static bool TryReadArchiveV3(ILowLevelIOHandler IOHandler, UserDataHeader userData, out IArchive archive)
        {
            //TODO: read archive v3
            archive = default;
            return false;
        }
        private static bool TryReadArchiveV4(ILowLevelIOHandler IOHandler, UserDataHeader userData, out IArchive archive)
        {
            //TODO: read archive v4
            archive = default;
            return false;
        }
    }
}
