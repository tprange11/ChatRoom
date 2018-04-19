using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Users
    {
        // member variables
        public Dictionary<string, TcpClient> ChatUsers;

        // constructor
        public Users()
        {
            ChatUsers = new Dictionary<string, TcpClient>();
        }
        // member methods

        public void Add(string username, TcpClient Client)
        {
            ChatUsers.Add(username, Client);
            
        }

        public void Remove(string username)
        {
            ChatUsers.Remove(username);

        }
        public string ListUsers()
        {
            return "";
        }
        //public TcpClient GetUsersClient()
        //{
        //    ChatUsers.Where
        //}
    }
}
