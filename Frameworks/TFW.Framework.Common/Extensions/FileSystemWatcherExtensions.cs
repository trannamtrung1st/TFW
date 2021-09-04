using System.IO;

namespace TFW.Framework.Common.Extensions
{
    public static class FileSystemWatcherExtensions
    {
        public static FileSystemWatcher Start(this FileSystemWatcher watcher)
        {
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        public static FileSystemWatcher Stop(this FileSystemWatcher watcher)
        {
            watcher.EnableRaisingEvents = false;
            return watcher;
        }

        public static FileSystemWatcher NotifyFilter(this FileSystemWatcher watcher, NotifyFilters notifyFilters)
        {
            watcher.NotifyFilter = notifyFilters;
            return watcher;
        }

        public static FileSystemWatcher WatchOnly(this FileSystemWatcher watcher, string dir, string filter)
        {
            watcher.Path = dir;
            return watcher.Filter(filter);
        }

        public static FileSystemWatcher Watch(this FileSystemWatcher watcher, string dir, params string[] filters)
        {
            watcher.Path = dir;
            return watcher.AddFilters(filters);
        }

        public static FileSystemWatcher IncludeSubdirs(this FileSystemWatcher watcher, bool val)
        {
            watcher.IncludeSubdirectories = val;
            return watcher;
        }

        public static FileSystemWatcher Filter(this FileSystemWatcher watcher, string filter)
        {
            watcher.Filter = filter;
            return watcher;
        }

        public static FileSystemWatcher AddFilters(this FileSystemWatcher watcher, params string[] filters)
        {
            foreach (var filter in filters)
                watcher.Filters.Add(filter);
            return watcher;
        }

        public static FileSystemWatcher RemoveFilters(this FileSystemWatcher watcher, params string[] filters)
        {
            foreach (var filter in filters)
                watcher.Filters.Remove(filter);
            return watcher;
        }

        public static FileSystemWatcher ClearFilters(this FileSystemWatcher watcher, params string[] filters)
        {
            watcher.Filters.Clear();
            return watcher;
        }
    }
}
