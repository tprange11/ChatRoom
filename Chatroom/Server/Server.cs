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
        TcpListener server;
        Dictionary<int, IUserClient> users;
        Queue<Message> messages;
        Logger.ILogger logger;
        private bool ConnectionOpen = true;
        public static ServerClient client;

        private List<TcpClient> socketList = new List<TcpClient>();
        


        public Server()
        {
            users = new Dictionary<int, IUserClient>();
            messages = new Queue<Message>();
            logger = new Logger.Filelogger();
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            Users clientList = new Users();
            server.Start();
        }
        public void Run()
        {
            logger.Log("ChatRoom Server Started");
            while (true)
            {
                Parallel.Invoke(
                    //This thread is always listening for new clients (users)
                    async () =>
                    {
                        await AcceptClient();
                    },
                    //This thread is always listening for new messages
                    async () =>
                    {
                        await GetAllMessages();
                    },
                    //This thread is always sending new messages
                    async () =>
                    {
                        await SendAllMessages();
                    },
                    //This thread is always checking for new connections and checking for disconnections
                    async () =>
                    {
                        await CheckIfConnected();
                    }
                );
            }            
        }
        Task AcceptClient()
        {
            return Task.Run(() =>
            { 
                Object clientListLock = new Object();
                lock (clientListLock)
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = server.AcceptTcpClient();
                    socketList.Add(clientSocket);
                    Console.WriteLine("Connected");
                    NetworkStream stream = clientSocket.GetStream();
                    client = new ServerClient(stream, clientSocket);
                    users.Add(client.UserId, client);
                    client.RecieveUsername();
                }
            });
        }
        Task CheckIfConnected()
        {
            return Task.Run(() =>
            {
                Object userListLock = new Object();
                lock (userListLock)
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        ServerClient currentUser = (ServerClient)users.ElementAt(i).Value;
                        if (!currentUser.CheckIfConnected())
                        {
                            int userKey = users.ElementAt(i).Key;
                            users.Remove(userKey);
                        }
                    }
                }
            });
        }
        Task GetAllMessages()
        {
            return Task.Run(() =>
            {
                Object messageLock = new Object();
                lock (messageLock)
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        Parallel.Invoke(
                            async () =>
                            {
                                await RecieveMessage(users.ElementAt(i).Value);
                            }
                        );
                    }
                }
            });
        }
        Task RecieveMessage(IUserClient user)
        {
            return Task.Run(() =>
            {
                Object receiveLock = new Object();
                lock (receiveLock)
                {
                    Message message = user.Recieve();
                    logger.Log(message.Body);
                    messages.Enqueue(message);
                }
            });
        }
        Task SendAllMessages()
        {
            return Task.Run(() =>
            {
                Object messageLock = new Object();
                lock (messageLock)
                {
                    if (messages.Count > 0)
                    {
                        for (int i = 0; i < users.Count; i++)
                        {
                            for (int j = 0; j < messages.Count; j++)
                            {
                                users.ElementAt(i).Value.Send(messages.ElementAt(j));
                            }
                        }
                        messages.Clear();
                    }
                }
            });
        }
    }
}
