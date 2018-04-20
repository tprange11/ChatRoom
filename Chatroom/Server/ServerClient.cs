using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerClient : IUserClient
    {
        NetworkStream stream;
        TcpClient client;
        Logger.ILogger logger;
        public int UserId;
        public string username;
        public ServerClient(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            logger = new Logger.Filelogger();
            UserId = stream.GetHashCode();
        }
        public void Send(Message message)
        {
            Object sendLock = new Object();
            lock (sendLock)
            {
                try
                {
                    byte[] messageBody = Encoding.ASCII.GetBytes(message.Body);
                    stream.Write(messageBody, 0, messageBody.Count());
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: '{0}'", e);
                }
            }
        }
        public Message Recieve()
        {
            Object recieveLock = new Object();
            lock (recieveLock)
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                string recievedMessageString = username + ": " + Encoding.ASCII.GetString(recievedMessage).Replace("\0", string.Empty);
                Message message = new Message(this, recievedMessageString);
                //logger.Log(recievedMessageString);
                return message;
            }
        }
        public string RecieveUsername()
        {
            byte[] RecievedUsername = new byte[256];
            stream.Read(RecievedUsername, 0, RecievedUsername.Length);
            username = Encoding.ASCII.GetString(RecievedUsername).Replace("\0", string.Empty);
            Console.WriteLine(username + " entered the room");
            logger.Log(username + " entered the room");
            return username;
        }
        public void CloseStream()
        {

        }
        public bool CheckIfConnected()
        {
            return true;
        }

    }
}
