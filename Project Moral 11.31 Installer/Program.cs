using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
class Program
{
        static void Main(string[] args)
    {;
        var httpClient = new WebClient();

        List<string> versions = JsonConvert.DeserializeObject<List<string>>(httpClient.DownloadString(Globals.SeasonBuildVersion + "/versions.json"));
        Console.Clear();

        Console.Title = "Moral Build Installer";
        Console.Write("Do you want to install version ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("11.31");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("?\nPlease type ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("'Yes' ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("or ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("'No'\n");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(">> ");

        var Version = Console.ReadLine();
        var FN11 = 0;

        switch (Version.ToLower())
        {
            case "yes":
                FN11 = 10;
                break;
            case "no":
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Closing...");
                Console.Out.Flush();
                Thread.Sleep(2000);
                return;
            default:
                Main(args);
                return;
        }

        var targetVersion = versions[FN11].Split("-")[1];
        var manifestUrl = $"{Globals.SeasonBuildVersion}/{targetVersion}/{targetVersion}.manifest";
        var manifest = JsonConvert.DeserializeObject<FileManifest.ManifestFile>(httpClient.DownloadString(manifestUrl));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Please enter a game folder location: ");
        Console.ForegroundColor = ConsoleColor.White;
        var targetPath = Console.ReadLine();
        Console.WriteLine();

        Installer.Download(manifest, targetVersion, targetPath).GetAwaiter().GetResult();
    }
}

internal class Globals
{
    public const string SeasonBuildVersion = "https://manifest.fnbuilds.services";
    public const int CHUNK_SIZE = 536870912 / 8;
}

internal class FileManifest
{
    public class ChunkedFile
    {
        public List<int> ChunksIds = new();
        public string File = string.Empty;
        public long FileSize = 0;
    }

    public class ManifestFile
    {
        public string Name = string.Empty;
        public List<ChunkedFile> Chunks = new();
        public long Size = 0;
    }
    internal class ConvertStorageSize
    {
        public static string FormatBytesWithSuffix(long bytes)
        {
            string[] Suffix = { "Bi", "KiB", "MiB", "GiB", "TiB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }
    }
}
