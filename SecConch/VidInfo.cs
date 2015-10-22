using System;
using System.Diagnostics;


namespace SecConch
{
    public class VidInfo
    {
        public string CameraIP { get; set; }
        public string OutputFolder { get; set; }
        private string FinalOutputFolder;
        public string OutputFile { get; set; }
        public int Duration { get; set; }// duration in seconds of the video recording 
        public static string ffmpegLocation = "ffmpeg\\bin\\ffmpeg.exe";
        private string CurDateTime;
        private string CurDate;
        private string LogFile;

        public VidInfo(string cameraIP, string outputFolder, int duration )
        {
            CameraIP = cameraIP;
            OutputFolder = outputFolder;
            Duration = duration;
        }

        public void RecordStream(int profile)
        {
            CurDateTime = DateTime.Now.ToString("MM_dd_yyyy_h_mm_tt_");
            CurDate = DateTime.Now.ToString("MM_dd_yyyy");
            FinalOutputFolder = OutputFolder + CurDate + "\\";
            System.IO.Directory.CreateDirectory(FinalOutputFolder);
            System.IO.Directory.CreateDirectory(FinalOutputFolder + "\\log");
            OutputFile = FinalOutputFolder + CurDateTime + "Recording.avi";
            LogFile = FinalOutputFolder + "\\log\\" + CurDateTime + "_log.txt";
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.FileName = "CMD.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/C " + ffmpegLocation + " -loglevel error -i rtsp://" + CameraIP + ":554/video.pro" + profile.ToString()
                              + " -b:v 128k -vcodec copy -r 30 -t " + Duration.ToString() + " -y " + OutputFile + " > " + LogFile + " 2>&1";
            p.Start();

            //Monitor thread stuff
            //FailMonitor monitor = new FailMonitor(p, OutputFile);
            //System.Threading.Thread newThread;
            //newThread = new System.Threading.Thread(monitor.MonitorProcess);
            //newThread.Start();

#if DEBUG
            //System.Console.WriteLine("C:\\Users\\Daniel\\Desktop\\NezzerJobRelated\\ffmpeg\\bin\\ffmpeg - i rtsp://172.16.1.183:554/video.pro2 -b:v 128k -vcodec copy -r 30 -t 120 -y \"C:\\Users\\Daniel\\Desktop\\NezzerJobRelated\\TestRecord\\Recording.avi\"");
            //C:\\Users\\Daniel\\Desktop\\NezzerJobRelated\\ffmpeg\\bin\\ffmpeg -i rtsp://172.16.1.183:554/video.pro2 -b:v 128k -vcodec copy -r 30 -t 120 -y "C:\\Users\\Daniel\\Desktop\\NezzerJobRelated\\TestRecord\\Recording.avi"
            // ffmpegLocation + " -i rtsp://" + CameraIP + ":554/video.pro2 -b:v 128k -vcodec copy -r 30 -t" + Duration.ToString() + " -y " + OutputFile
#endif
        }
    }
}
