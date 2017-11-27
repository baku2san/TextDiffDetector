using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDiffDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            var watcher = new FileWatcher();

            Console.ReadKey();
        }
    }
}
