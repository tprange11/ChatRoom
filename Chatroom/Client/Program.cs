using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {            
            Client client = new Client("192.168.0.137", 9999);
            client.SetUsername();
            UI.SetTitleBar("Chatroom - " + client.GetUsername());
            while (true)
            {
            client.Send();
            client.Recieve();
            }
            Console.ReadLine();
        }
    }
}
