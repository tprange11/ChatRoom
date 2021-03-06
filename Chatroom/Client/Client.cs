﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        string username;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        Task Send()
        {
            return Task.Run(() =>
            {
                string messageString = UI.GetInput();
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                stream.Write(message, 0, message.Count());
            });
            }
        Task Recieve()
        {
            return Task.Run(() =>
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage).Replace("\0", string.Empty));
            });
        }

        public void SetUsername()
        {
            UI.DisplayMessage("Please enter your username");
            username = UI.GetInput();
            
        }
        public string GetUsername()
        {
            return username;
        }
        public void DisplayIntroMessage()
        {
            UI.DisplayMessage("Welcome to the chat, " + username + "!");
        }
        public void SendUsername()
        {
            byte[] name = Encoding.ASCII.GetBytes(username);
            stream.Write(name, 0, name.Count());
        }
        public void Run()
        {
            while (true)
            {
                Parallel.Invoke(

                    async () =>
                    {
                        await Send();
                    },

                    async () =>
                    {
                        await Recieve();
                    }
                );
            }
        }
    }
}
