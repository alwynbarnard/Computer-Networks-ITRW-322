using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    public class MessageCompression
    {
        Firebase firebaseDB;
        MainScreen mainScreen;

        public MessageCompression(MainScreen ms)
        {
            mainScreen = ms;
            firebaseDB = new Firebase(ms);
        }

        public static string Compress(string filePath, string uid)
        {
            FileInfo fi = new FileInfo(filePath);

            string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1),
                   extension = filePath.Substring(filePath.LastIndexOf('.') + 1);

            fileName = fileName.Remove(fileName.LastIndexOf('.'));
            fileName = fileName.Replace(" ", "").Replace("(", "").Replace(")", "");

            using (FileStream originalStream = fi.OpenRead())
            {
                if ((File.GetAttributes(fi.FullName) &
                    FileAttributes.Hidden) != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    string compressedFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{uid}\Compressed\{fileName}({extension}).gz";
                    FileInfo compressedFileInfo = new FileInfo(compressedFilePath);

                    if (!compressedFileInfo.Exists)
                    {
                        using (FileStream compressedFileStream = File.Create($@"{AppDomain.CurrentDomain.BaseDirectory}\{uid}\Compressed\{fileName}({extension}).gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                                   CompressionMode.Compress))
                            {
                                originalStream.CopyTo(compressionStream);
                            }
                        }
                    }

                    return compressedFilePath;
                }
                else
                    return null;
            }
        }

        public static string Decompress(string fileToDecompress, string uid)
        {
            string fileAndExtension = GetFileName(fileToDecompress);
            
            using (Stream originalFileStream = new FileStream(fileToDecompress, FileMode.Open, FileAccess.Read))
            {
                string decompressedFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{uid}\Decompressed\{fileAndExtension}";
                FileInfo decompressedFileInfo = new FileInfo(decompressedFilePath);

                if (!decompressedFileInfo.Exists)
                {
                    using (FileStream decompressedFileStream = File.Create($@"{AppDomain.CurrentDomain.BaseDirectory}\{uid}\Decompressed\{fileAndExtension}"))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }

                    return decompressedFileInfo.FullName;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string GetFileName(string path)
        {
            string fileName = path.Substring(path.LastIndexOf("%2F") + 3);
            fileName = fileName.Remove(fileName.LastIndexOf('('));
            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

            string extension = path.Substring(path.LastIndexOf('(') + 1, path.LastIndexOf(')') - path.LastIndexOf('(') - 1);

            return fileName + "." + extension;
        }
    }
}
