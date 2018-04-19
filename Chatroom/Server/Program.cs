using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Filelogger logger = new Logger.Filelogger();
            logger.Log("ChatRoom Server Started");
            new Server().Run();
            Console.ReadLine();
            logger.Log("ChatRoom Server Stopped");
        }
    }
}
