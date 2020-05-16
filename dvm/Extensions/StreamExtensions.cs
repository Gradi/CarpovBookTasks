using System;
using System.IO;

namespace dvm.Extensions
{
    public static class StreamExtensions
    {
        private const int GzipMagicNumber = 0x00001F8B;

        public static bool IsGzippedStream(this Stream stream)
        {
            int b1 = stream.ReadByte();
            int b2 = stream.ReadByte();

            return b1 != -1 && b2 != -1 &&
                   ((b1 << 8) | b2) == GzipMagicNumber;
        }

        public static byte[] ReadAllBytes(this Stream stream)
        {
            var bytes = new byte[8192];
            int totalBytes = 0;
            int pendingBytes = 0;

            while((pendingBytes = stream.Read(bytes.AsSpan(totalBytes))) != 0)
            {
                totalBytes += pendingBytes;
                if (totalBytes == bytes.Length)
                {
                    Array.Resize(ref bytes, bytes.Length * 2);
                }
            }
            Array.Resize(ref bytes, totalBytes);
            return bytes;
        }
    }
}
