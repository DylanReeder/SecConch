using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Nito;//Deques

namespace SecConch
{

    /// <summary>
    /// File Monitor is intended to be effecient way of observing and
    /// coordinating several files to make sure that the process
    /// creating them is running smoothly.
    /// </summary>
    class FileMonitor
    {
        FileSystemWatcher watcher;
        int NumberCameras;
        Deque<String> FileList;

        public FileMonitor(int numCameras)
        {
            watcher = new FileSystemWatcher();
            NumberCameras = numCameras;
            FileList = new Deque<string>(NumberCameras);
        }
        ///field: list of files to watch
        ///field: file system watcher object
        ///field: 

        ///method: watch this file // adds to list of files to watch
        ///method: stop watching this file
        ///method: 
    }
}
