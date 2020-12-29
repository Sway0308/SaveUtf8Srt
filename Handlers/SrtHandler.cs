using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SaveUtf8Srt.Handlers
{
    public class SrtHandler : DirRecursiveHandler
    {
        protected override void ProcessFiles(string path, IEnumerable<string> files)
        {
            var srts = files.Where(x => x.EndsWith(".srt"));
            if (!srts.Any())
                return;

            foreach (var srt in srts)
            {
                var encoding = StrHelper.GetEncoding(srt);
                if (encoding == Encoding.UTF8 || encoding == Encoding.Unicode)
                    continue;

                var fileName = Path.GetFileName(srt);
                var newFileName = fileName.Replace(".srt", ".original.srt");
                var newPath = Path.Combine(path, newFileName);

                var s = string.Empty;
                using (var sr = new StreamReader(srt, Encoding.Default, true))
                {
                    s = sr.ReadToEnd();
                }

                File.Move(srt, newPath);
                File.WriteAllText(srt, s, Encoding.UTF8);
            }
        }
    }
}
