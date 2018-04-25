using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using RestSharp;

namespace CloudBuildDownloader
{
    public enum BuildMode
    {
        Debug,
        Release
    }

    public enum Platform
    {
        Windows
    }

    public class Options
    {
        [Option(Required = true)]
        public BuildMode build_mode { get; set; }
        [Option(Required = true)]
        public Platform platform { get; set; }
        [Option(Required = true)]
        public string build_name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed((options) => run_program(options))
                .WithNotParsed((errors) =>
                {
                });
        }

        const string UnityAccessTokenFile = "../../../unity_access_token.txt";

        private static void run_program(Options options)
        {
            string out_dir = create_out_dir(options);
            var token = File.ReadAllText(UnityAccessTokenFile);
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("The token file does not contain any token.");
            var c = new UnityClient(token);

            Console.WriteLine("Gets latest successful builds...");
            List<Build> builds = get_latest_win64_debug_builds(c, options.build_name);
            if (builds == null || builds.Count == 0)
                throw new Exception(string.Format("Either there have not been any successful builds or the --build_name {0} does not exist", options.build_name));
            var latest_build = get_latest(builds);

            Console.WriteLine("Got latest build with build number " + latest_build.build);
            for (; ; )
            {
                Thread.Sleep(1000 * 10); //10 seconds
                Console.WriteLine();
                Console.WriteLine("Checks for a new build...");
                builds = get_latest_win64_debug_builds(c, options.build_name);
                var new_latest_build = get_latest(builds);
                if (new_latest_build.build > latest_build.build)
                {
                    process_new_build(out_dir, new_latest_build);
                    break;
                }
                else
                {
                    Console.WriteLine("Found no new build.");
                }
            }
        }

        private static void process_new_build(string out_dir, Build new_latest_build)
        {
            Console.WriteLine("New build found!");
            var download_info = new_latest_build?.links?.download_primary;
            if (download_info == null)
                throw new Exception("Could not get download info from the latest build. The gathered info are: \n" + JsonConvert.SerializeObject(new_latest_build, Formatting.Indented));

            Method method;
            if (!Enum.TryParse(download_info.method, true, out method))
                throw new Exception("Could not parse the method type of the download link of the latest successful build.");
            var file_type = download_info.meta?.type;
            if (file_type != "ZIP")
                throw new Exception("The latest build file format is expected to be ZIP but the resolved type are: " + file_type);

            Console.WriteLine("Downloading completed build...");
            var bytes = download_from_url(download_info.href, method);
            Console.WriteLine("Unzips the new build...");
            unzip_into(out_dir, bytes);
            Console.WriteLine("The new build is now unzipped at: \n" + out_dir);
        }

        private static string create_out_dir(Options options)
        {
            string bin_dir;
            switch(options.build_mode)
            {
                case BuildMode.Debug:
                    bin_dir = "debug_bin";
                    break;
                case BuildMode.Release:
                    bin_dir = "release_bin";
                    break;
                default:
                    throw new Exception("Unhandled enum value " + options.build_mode);
            }

            string platform_dir;
            switch(options.platform)
            {
                case Platform.Windows:
                    platform_dir = "windows";
                    break;
                default:
                    throw new Exception("Unhandled enum value " + options.platform);
            }

            return Path.Combine("../../../..", bin_dir, platform_dir);
        }

        private static void unzip_into(string path, byte[] bytes)
        {
            if (Directory.Exists(path))
            {
                var old = Path.ChangeExtension(path, ".old");
                Directory.Move(path, old);
                Directory.Delete(old, true);
            }

            using (var mem_stream = new MemoryStream(bytes))
            using (var zip = new ZipArchive(mem_stream, ZipArchiveMode.Read))
            {
                foreach(var e in zip.Entries)
                {
                    using (var es = e.Open())
                    {
                        var out_path = Path.Combine(path, e.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(out_path));
                        using (var out_fs = new FileStream(out_path, FileMode.CreateNew))
                        {
                            es.CopyTo(out_fs);
                        }
                    }
                }
            }
        }

        private static byte[] download_from_url(string url, Method method = Method.GET)
        {
            return new RestClient(url).DownloadData(new RestRequest(method), true);
        }

        private static List<Build> get_latest_win64_debug_builds(UnityClient c, string build_name)
        {
            return c.GetLatestSuccessfulBuildOf("adapt-alliance-gamestudio", "pl-war", build_name);
        }

        private static Build get_latest(List<Build> builds)
        {
            Build latest = null;
            foreach (var b in builds)
                if (latest == null || latest.build < b.build)
                    latest = b;
            return latest;
        }
    }
}
