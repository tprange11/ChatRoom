using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerClient
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        public string username;
        public ServerClient(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
        }
        public void Send(TcpClient Client,string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public string Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage).Replace("\0",string.Empty);
            Console.WriteLine(recievedMessageString);
            return recievedMessageString;
        }
        public string RecieveUsername()
        {
            byte[] RecievedUsername = new byte[256];
            stream.Read(RecievedUsername, 0, RecievedUsername.Length);
            username = Encoding.ASCII.GetString(RecievedUsername).Replace("\0", string.Empty);
            Console.WriteLine(username);
            return username;
        }

    }
}
