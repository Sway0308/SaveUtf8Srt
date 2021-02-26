using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUtf8Srt.Handlers
{
    public class MoveBig5SrtHandler : DirRecursiveHandler
    {
        private string _BackupPath;

        public void MoveSrt(string srtPath, string backupPath)
        {
            _BackupPath = backupPath;
            Directory.CreateDirectory(_BackupPath);
            RecusiveSearch(srtPath);
        }

        protected override void ProcessFiles(string path, IEnumerable<string> files)
        {
            var srts = files.Where(x => x.EndsWith(".original.srt"));
            if (!srts.Any())
                return;

            foreach (var srt in srts)
            {
                var fileName = Path.GetFileName(srt);
                var newPath = Path.Combine(_BackupPath, fileName);

                File.Move(srt, newPath);
            }
        }
    }
}
