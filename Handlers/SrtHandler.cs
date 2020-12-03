using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUtf8Srt.Handlers
{
    public class SrtHandler
    {
        public void Execute(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path) || path.EndsWith("System Volume Information"))
                return;

            var (dirs, isAccessable) = GetEnumerateDirectories(path);
            if (!isAccessable)
                return;
            if (dirs.Any())
            {
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    Execute(dir);
                }
            }
            else
            {
                CheckSrt(path);
            }
        }

        private (IEnumerable<string> dirs, bool isAccessable) GetEnumerateDirectories(string path)
        {
            try
            { 
                return (Directory.EnumerateDirectories(path), true);
            }
            catch
            {
                return (Enumerable.Empty<string>(), false);
            }
        }

        private void CheckSrt(string path)
        {
            var srts = Directory.EnumerateFiles(path).Where(x => x.EndsWith(".srt"));
            if (!srts.Any())
                return;

            foreach (var srt in srts)
            {
                var encoding = GetEncoding(srt);
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

        private Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.Default;
        }
    }
}
