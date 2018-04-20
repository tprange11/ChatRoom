using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logger
{
    class Filelogger : ILogger 
    {
        private string cdate = DateTime.Now.ToString(@"yyyyMMdd_HHmmss: ");
        public string path = @"C:\temp\ChatRoom.log";
        public void Log(string data)
        {
            Object fileLock = new Object();
            lock (fileLock)
            {
                if (!File.Exists(path))
                {
                    try
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(cdate + data);
                        }
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        System.IO.Directory.CreateDirectory(@"C:\temp");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(cdate + data);
                    }
                }
            }

        }
        
    }
}
