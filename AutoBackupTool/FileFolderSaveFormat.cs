using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crc32C;
using ProtoBuf;

namespace AutoBackupTool
{
    [ProtoContract]
    public class FileSaveFormat
    {
        public FileSaveFormat()
        {

        }
        public FileSaveFormat(string FileSavedPath, string FileFullPath)
        {
            FilePath = FileSavedPath;
            EncryptedZippedData = System.IO.File.ReadAllBytes(FileFullPath);
            //FileCRC = Crc32C.Crc32CAlgorithm.Compute(EncryptedZippedData);
            // TODO: zip and encrypt file
        }
        [ProtoMember(1)]
        public string FilePath { get; set; }
        //[ProtoMember(2)]
        //public uint FileCRC { get; set; }
        [ProtoMember(2)]
        public byte[] EncryptedZippedData { get; set; }
    }
}
