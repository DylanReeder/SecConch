using System;
using System.IO;

namespace SecConch
{
    /// <summary>
    /// delete files that were created after a certain date
    /// 
    /// </summary>
    class MemoryManager
    {
        private String[] Directories { get; set; }//location of the folder to delete
        private String TopFolder;
        private String TodaysDate { get; set; }
        private int fileTTL { get; set; }//file TTL in days. 14 days would be 2 weeks
        private String delDate { get; set; }

        //T is the top level directory
        //F is the number of days a directory remains before deletion.
        public MemoryManager(String T, int F)
        {
            Directories= Directory.GetDirectories(T);//bin/recordings
            TopFolder = T;
            fileTTL = F;
        }

        //filename should be of the format MM_dd_yyyy_hh_mm_xx_Recording.avi
        //the files should all have the same MM_dd_yyyy under with different hh_xx
        //The folder structure is Bin> Recordings > DateFolder
        //where each unique date gets its own folder named in the format MM_dd_yyyy
        public String getDateFromName(String folderName)
        {
            String folderDate = folderName.Substring(0,10);
            return folderDate;
        }

        private void setDelDate()
        {
            TimeSpan subtractor = new TimeSpan(fileTTL,0,0,0);
            delDate = DateTime.Now.Subtract(subtractor).ToString("MM_dd_yyyy");
        }

        public void delOldFolders()
        {
            Directories = Directory.GetDirectories(TopFolder);//refresh in case a new directory has appeared.
            setDelDate();
            foreach (String element in Directories)
            {
                if (element == TopFolder + delDate)
                {
                    Directory.Delete(element,true);
                }
            }
        }
    }
}
