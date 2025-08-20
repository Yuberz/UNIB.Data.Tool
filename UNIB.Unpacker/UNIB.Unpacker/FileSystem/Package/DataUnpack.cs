using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace UNIB.Unpacker
{
    class DataUnpack
    {
        private static List<DataFolder> m_FoldersTable = new List<DataFolder>();
        private static List<DataEntry> m_EntryTable = new List<DataEntry>();

        // Helper function to sanitize paths from the archive
        private static String SanitizePath(String pathFromArchive)
        {
            // Replace any Windows backslashes with the current system's directory separator
            return pathFromArchive.Replace('\\', Path.DirectorySeparatorChar);
        }

        public static void iDoIt(String m_IndexFile, String m_DstFolder)
        {
            using (FileStream TIndexStream = File.OpenRead(m_IndexFile))
            {
                var m_Header = new DataHeader();

                m_Header.dwTotalFolders = TIndexStream.ReadInt32();
                m_Header.dwTotalFiles = TIndexStream.ReadInt32();
                m_Header.dwArchiveSize = TIndexStream.ReadUInt32();
                m_Header.m_ArchiveName = Encoding.ASCII.GetString(TIndexStream.ReadBytes(52)).TrimEnd('\0');

                m_FoldersTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFolders; i++)
                {
                    var m_Folder = new DataFolder();

                    m_Folder.dwFilesInFolder = TIndexStream.ReadInt32();
                    m_Folder.dwUnknown1 = TIndexStream.ReadInt32();
                    m_Folder.dwUnknown2 = TIndexStream.ReadInt32();
                    m_Folder.m_FolderName = Encoding.ASCII.GetString(TIndexStream.ReadBytes(116)).TrimEnd('\0');
                    // FIX: Sanitize the folder name read from the archive
                    m_Folder.m_FolderName = SanitizePath(m_Folder.m_FolderName);

                    m_FoldersTable.Add(m_Folder);
                }

                TIndexStream.Seek(4, SeekOrigin.Current);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new DataEntry();

                    m_Entry.dwDecompressedSize = TIndexStream.ReadInt32();
                    m_Entry.dwCompressedSize = TIndexStream.ReadInt32();
                    m_Entry.dwOffset = TIndexStream.ReadUInt32();
                    m_Entry.m_FileName = Encoding.ASCII.GetString(TIndexStream.ReadBytes(64)).TrimEnd('\0');
                    // FIX: Sanitize the file name read from the archive
                    m_Entry.m_FileName = SanitizePath(m_Entry.m_FileName);

                    m_EntryTable.Add(m_Entry);

                    if (TIndexStream.Length == TIndexStream.Position)
                    {
                        break;
                    }

                    TIndexStream.Seek(4, SeekOrigin.Current);
                }

                // Use Path.Combine to find the .dat file
                String m_DataFile = Path.Combine(Path.GetDirectoryName(m_IndexFile), m_Header.m_ArchiveName);
                using (FileStream TDataStream = File.OpenRead(m_DataFile))
                {
                    Int32 j = 0;
                    foreach (var m_Folder in m_FoldersTable)
                    {
                        for (Int32 i = 0; i < m_Folder.dwFilesInFolder; i++, j++)
                        {
                            // Use Path.Combine to create the full output path
                            String m_FullPath = Path.Combine(m_DstFolder, m_Folder.m_FolderName, m_EntryTable[j].m_FileName);

                            // Also fix the info message to use the correct slash for display
                            String m_DisplayPath = Path.Combine(m_Folder.m_FolderName, m_EntryTable[j].m_FileName);
                            Utils.iSetInfo("[UNPACKING]: " + m_DisplayPath);

                            Utils.iCreateDirectory(Path.GetDirectoryName(m_FullPath)); // Ensure the parent directory exists

                            TDataStream.Seek(m_EntryTable[j].dwOffset, SeekOrigin.Begin);

                            var lpBuffer = TDataStream.ReadBytes(m_EntryTable[j].dwCompressedSize);

                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                    }

                    TDataStream.Dispose();
                }

                TIndexStream.Dispose();
            }
        }
    }
}
