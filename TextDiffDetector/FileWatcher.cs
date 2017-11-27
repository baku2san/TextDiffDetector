using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Collections.Concurrent;

namespace TextDiffDetector
{
    class FileWatcher : IDisposable
    {
        //private BlockingCollection<IQueueingData> BlockingCollection;
        private FileSystemWatcher watcher = null;
        string WatchingPath;

        private long previousLength { get; set; }

        //public FileWatcher(BlockingCollection<IQueueingData> bc) : this()
        //{
        //    BlockingCollection = bc;
        //}
        public FileWatcher(string path = @"C:\Users\takan\OneDrive\ドキュメント\Visual Studio 2017\Projects\TextDiffDetector\test\")
        {
            previousLength = 0;
            WatchingPath = path;
            watcher = new FileSystemWatcher()
            {
                Path = WatchingPath,
                NotifyFilter =
                    (
                        //    System.IO.NotifyFilters.LastAccess
                        //    System.IO.NotifyFilters.FileName,
                        //    System.IO.NotifyFilters.DirectoryName,
                        NotifyFilters.CreationTime |
                        NotifyFilters.LastWrite),
                Filter = "",    // 全て
                // SynchronizingObject = this   // UIのスレッドにマーシャリングする時に必要 /コンソールアプリケーションでの使用では必要ない
                IncludeSubdirectories = false
            };
            //イベントハンドラの追加
            // Rxを使用することで、イベント集約
            watcher.ChangedAsObservable()
                .Throttle(TimeSpan.FromSeconds(1))      // 一秒内のイベントに集約→一秒内に複数回イベントがあっても一回のイベントに。（実際に複数あったら消えるが、あり得ないと判断）
                .Subscribe(e => DetectDifference(e));

            //監視を開始する
            watcher.EnableRaisingEvents = true;
            //Console.WriteLine("監視を開始しました。");
        }

        public void Dispose()
        {
            watcher.EnableRaisingEvents = false;    // とりあえず停止しておいてからにする
            ((IDisposable)watcher).Dispose();
        }

        private void DetectDifference(FileSystemEventArgs eventArgs)
        {
            var fileInfo = new FileInfo(eventArgs.FullPath);
            if (previousLength != fileInfo.Length) {
                using (var fileStream = fileInfo.OpenRead()) {
                    fileStream.Seek(previousLength, SeekOrigin.Begin);
                    using (var streamReader = new StreamReader(fileStream, Encoding.GetEncoding("Shift_JIS"), true))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            Console.WriteLine(streamReader.ReadLine());
                        }
                    }
                }
                previousLength = fileInfo.Length;
            }
            //BlockingCollection.Add(lineData);
        }

    }
}