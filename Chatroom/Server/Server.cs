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
        private List<TcpClient> socketList = new List<TcpClient>();


        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.137"), 9999);
            Users clientList = new Users();
            server.Start();
        }
        public void Run()
        {
            int seats = 10;
            Task[] tasks = new Task[seats];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    AcceptClient();
                    while (ConnectionOpen)
                    {
                        client.RecieveUsername();
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
            socketList.Add(clientSocket);
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new ServerClient(stream, clientSocket);
        }
        private void Respond(string body)
        {
            for (int i = 0; i > socketList.Count; i++)
            {
                client.Send(socketList[i],body);
            }
        }
    }
}
