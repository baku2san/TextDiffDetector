using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace TextDiffDetector
{

    public static class FileSystemWatcherExtensions // http://neue.cc/2011/07/06_332.html
    {
        public static IObservable<FileSystemEventArgs> CreatedAsObservable(this FileSystemWatcher watcher)
        {
            return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
                h => (sender, e) => h(e), h => watcher.Created += h, h => watcher.Created -= h);
        }

        public static IObservable<FileSystemEventArgs> DeletedAsObservable(this FileSystemWatcher watcher)
        {
            return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
                h => (sender, e) => h(e), h => watcher.Deleted += h, h => watcher.Deleted -= h);
        }

        public static IObservable<RenamedEventArgs> RenamedAsObservable(this FileSystemWatcher watcher)
        {
            return Observable.FromEvent<RenamedEventHandler, RenamedEventArgs>(
                h => (sender, e) => h(e), h => watcher.Renamed += h, h => watcher.Renamed -= h);
        }

        public static IObservable<FileSystemEventArgs> ChangedAsObservable(this FileSystemWatcher watcher)
        {
            return Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
                h => (sender, e) => h(e), h => watcher.Changed += h, h => watcher.Changed -= h);
        }
    }
}
