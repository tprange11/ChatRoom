using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static ServerClient client;
        TcpListener server;
        private bool ConnectionOpen = true;


        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            Users clientList = new Users();
            server.Start();
        }
        public void Run()
        {
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    AcceptClient();
                    while (ConnectionOpen)
                    {
                        string message = client.Recieve();
                        Respond(message);
                    }
                });
            }
           
        }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new ServerClient(stream, clientSocket);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
