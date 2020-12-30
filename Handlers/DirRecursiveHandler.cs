using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SaveUtf8Srt.Handlers
{
    public abstract class DirRecursiveHandler
    {
        public void RecusiveSearch(string path)
        {
            BeforeRecusiveSearch(path);
            DoRecusiveSearch(path);
            AfterRecusiveSearch(path);
        }

        protected virtual void BeforeRecusiveSearch(string path) { }

        protected virtual void DoRecusiveSearch(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path) || path.EndsWith("System Volume Information"))
                return;

            var (dirs, isAccessable) = GetEnumerateDirectories(path);
            if (!isAccessable)
                return;
            if (dirs.Any())
            {
                var innerDirs = Directory.EnumerateDirectories(path);
                ProcessDirs(innerDirs);
                foreach (var dir in innerDirs)
                {
                    DoRecusiveSearch(dir);
                }
            }

            var files = Directory.EnumerateFiles(path);
            ProcessFiles(path, files);
        }

        protected virtual void AfterRecusiveSearch(string path) { }

        private (IEnumerable<string> dirs, bool isAccessable) GetEnumerateDirectories(string path)
        {
            try
            {
                if (path is null)
                {
                    throw new System.ArgumentNullException(nameof(path));
                }

                return (Directory.EnumerateDirectories(path), true);
            }
            catch
            {
                return (Enumerable.Empty<string>(), false);
            }
        }

        protected virtual void ProcessDirs(IEnumerable<string> innerDirs)
        { }

        protected virtual void ProcessFiles(string path, IEnumerable<string> files)
        { }
    }
}
