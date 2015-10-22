using System;
using System.Collections.Generic;

namespace SecConch
{
    class Controller
    {
        static MemoryManager MM;
        static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {
            int numCameras = 0;
            string line;
            int duration = 120;//duration of each video in seconds<---------------------------change to change time. 3600 = 1 hr
            String OutputFolder = "Recordings\\";
            System.IO.Directory.CreateDirectory(OutputFolder);
            MM = new MemoryManager(OutputFolder, 14);
            List<VidInfo> streamList = new List<VidInfo>(10);
            System.IO.StreamReader file = new System.IO.StreamReader("Cameras.txt");
            Console.WriteLine("Exit this window to end the application. The current recordings will continue for their duration and close as normal.");
            while ((line = file.ReadLine()) != null)
            {
                numCameras++;
                streamList.Add(new VidInfo(line, OutputFolder, duration));
            }
            RecordLoop(streamList, duration);
        }

        private static void RecordLoop(List<VidInfo> l, int dur)
        {
            int profile = 0;
            while (true)
            {
                if (profile == 2) { profile = 3; } else { profile = 2; }//set profile to be 2 or 3
                foreach (VidInfo element in l)
                {
                    element.RecordStream(profile);
                }
                System.Threading.Thread.Sleep((dur - 20) * 1000);
                MM.delOldFolders();
            }
        }
    }
}
