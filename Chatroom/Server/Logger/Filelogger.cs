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
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(cdate + data);                    
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
