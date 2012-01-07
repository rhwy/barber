using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace ArtOfNet.RazorTemplateCommand.Cmd
    {
    public static class ShellUtil
        {
        public static IEnumerable<FileInfo> FindFile(string root, string pattern)
            {
            return FindFile(root, pattern, null);
            }
        public static IEnumerable<FileInfo> FindFile(string root, string pattern, Action<Exception> onError)
            {
            if (!Directory.Exists(root))
                {
                throw new ArgumentNullException("root");
                }

            string[] files = null;
            try
                {
                files = Directory.GetFiles(root, pattern, SearchOption.TopDirectoryOnly);
                }
            catch (Exception ex)
                {
                if (onError != null)
                    Console.WriteLine(ex.Message);
                }
            if (files != null)
                {
                IEnumerator enumerator = files.GetEnumerator();
                while (enumerator.MoveNext())
                    {
                    yield return new FileInfo((string)enumerator.Current);
                    }
                }

            string[] dirs = null;
            try
                {
                dirs = Directory.GetDirectories(root);
                }
            catch (Exception ex)
                {
                if (onError != null)
                    Console.WriteLine(ex.Message);
                }
            if (dirs != null)
                {
                foreach (string dir in dirs)
                    {
                    IEnumerator enumerator = FindFile(dir, pattern, onError).GetEnumerator();
                    while (enumerator.MoveNext())
                        {
                        yield return (FileInfo)enumerator.Current;
                        }
                    }
                }
            }

        public static void ForEachFile(this IEnumerable<FileInfo> list, Action<FileInfo> action)
            {
            foreach (FileInfo item in list)
                {
                action(item);
                }
            }

        public static IEnumerable<FileInfo> JoinFile(this IEnumerable<FileInfo> list, params IEnumerable<FileInfo>[] anotherList)
            {
            foreach (FileInfo item in list)
                {
                yield return item;
                }

            foreach (IEnumerable<FileInfo> alist in anotherList)
                {
                foreach (FileInfo item in alist)
                    {
                    yield return item;
                    }
                }
            }

        public static FileInfo FirstOrDefault(this IEnumerable<FileInfo> list, string defaultPath)
            {
            FileInfo result = null;
            if (list != null)
                {
                result = list.FirstOrDefault();
                if (result == null && File.Exists(defaultPath))
                    {
                    result = new FileInfo(defaultPath);
                    }
                }
            return result;
            }
        }
    }
