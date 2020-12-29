using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SaveUtf8Srt.Handlers
{
    public class CheckContentHandler : DirRecursiveHandler
    {
        private readonly IList<(string path, string oriFileName)> OriFilePaths = new List<(string path, string oriFileName)>();

        protected override void BeforeRecusiveSearch(string path)
        {
            OriFilePaths.Clear();
        }

        protected override void ProcessFiles(string path, IEnumerable<string> files)
        {
            if (!files.Any(x => x.EndsWith(".srt")) || !files.Any(x => x.EndsWith(".original.srt")))
                return;

            var oriSrtFile = files.First(x => x.EndsWith(".original.srt"));
            var oriSrt = GetContents(oriSrtFile);
            var srtFile = files.First(x => !x.Equals(oriSrtFile) && x.EndsWith(".srt"));
            var srt = GetContents(srtFile);

            if (oriSrt.Equals(srt, StringComparison.InvariantCultureIgnoreCase))
            {
                var name = Path.GetFileNameWithoutExtension(oriSrtFile);
                Console.WriteLine($"{name.Replace(".original", string.Empty)} is the same");
                OriFilePaths.Add((path, name));
            }
        }

        private string GetContents(string filePath)
        {
            var encoding = StrHelper.GetEncoding(filePath);
            var s = string.Empty;
            using (var sr = new StreamReader(filePath, encoding, true))
            {
                s = sr.ReadToEnd();
            }
            return s;
        }

        protected override void AfterRecusiveSearch(string path)
        {
            if (OriFilePaths.Count == 0)
                return;

            Console.WriteLine("Delete original file?");
            var command = Console.ReadLine();
            if (!string.Equals(command, "Y", StringComparison.InvariantCultureIgnoreCase))
                return;

            foreach (var (oripath, oriFileName) in OriFilePaths)
            {
                var filePath = Path.Combine(oripath, oriFileName + ".srt");
                File.Delete(filePath);
            }
            Console.WriteLine("done");
        }
    }
}
