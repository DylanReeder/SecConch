using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SecConch
{
    /// <summary>
    /// PARTIALLY DEPRECATED IN FAVOR OF FILEMONITOR
    /// </summary>


    /* FFMPEG sometimes will hang without reporting any error. AFAIK, the only
    * way to determine if the hang has occured programatically, is to check if
    * if the associated file is still growing or has stopped growing. 
    * The idea behind the failmonitor is to constantly check if the file is still
    * growing. If not, we kill the ffmpeg process and start a fresh one. 
    * this error only tends to occur during times of network turbulence, so it shouldn't
    * happen at all, but we can't allow any down time if possible, so we are implementing
    * this program in order to prevent that. Likely as not, we will just be eating system 
    * resources. 
    *
    */
    class FailMonitor
    {
        Process p; //the ffmpeg process we are going to monitor.
        String FileLocation; // the file p is writing to

        public FailMonitor(Process ffmpegProcess, String fileLocation)
        {
            p = ffmpegProcess;
            FileLocation = fileLocation;
        }

        public void MonitorProcess()
        {
            StartCheck();
            ContinuousCheck();
            System.Console.WriteLine("The process has exited!");
        }
        //THIS FUNCTION DOES NOT WORK CORRECTLY. IT ERRORS.
        // prevents the file from hanging at the start of the recording
        // restarts the recording if it did hang.
        // This process should never actually be needed as I have been unable to
        // replicate this issue for a while.
        //TODO restart the process with the correct timing
        private void StartCheck()
        {
            bool firstPass = true;
            while (firstPass == true)
            {
                firstPass = false;
                System.Threading.Thread.Sleep(15000);
                if (!File.Exists(FileLocation))
                {
                    p.Kill();
                    p.CloseMainWindow();
                    //TODO: This should send a signal back to the controller instead of 
                    // handling this by itself. 
                    p.WaitForExit();
                    System.Threading.Thread.Sleep(4000);
                    p.Start();
                    firstPass = true;
                    //debug stuff for StartCheck()
#if DEBUG
                    Console.WriteLine("The file doesn't exist.");
#endif
                }
#if DEBUG
                else
                {
                    Console.WriteLine("The file does exist.");
                }
#endif
            }
        }

        // If the recording hangs in the middle of a stream there is no way to detect it directly(that i've found)
        // instead, this monitors the file its reading to. If the file has stopped growing
        // it is assumed that the process has hung up on something and needs to restart.
        // This issue has only happened once, so like StartCheck(), ContinuousCheck()
        // shouldnt need to ever actually restart the process.
        private void ContinuousCheck()
        {
            while (!p.HasExited)
            {
                System.Threading.Thread.Sleep(30000);
                System.Console.WriteLine("The thread is running: " + FileLocation);
                //create system file watcher
                // every (5,15,30 seconds?) check to see if the file has still changed size.
                //if yes, do nothing, 
                // if no, wait 15 seconds and check again,
                // if still no, restart the process.
            }
        }
        //some function that alerts the controller that the stream has failed
        //triggering it to restart. 
    }

    public class FailMonitorThread
    {

    }
}
