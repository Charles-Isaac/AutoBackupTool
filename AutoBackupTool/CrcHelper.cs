using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool
{
    public static class CrcHelper
    {
        public static uint Compute(Stream str)
        {
            uint CRC = 0;
            byte[] b = new byte[128];
            int byteRead = 0;
            while (0 < (byteRead = str.Read(b, 0, 128)))
            {
                CRC = Crc32C.Crc32CAlgorithm.Append(CRC, b, 0, byteRead);
            }
            return CRC;
        }

        public static uint Compute(byte[] b)
        {
            return Crc32C.Crc32CAlgorithm.Compute(b);
        }
    }
}
