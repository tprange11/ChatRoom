﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    interface IUserClient
    {
        void Send(Message message);
        Message Recieve();
        bool CheckIfConnected();
        void CloseStream();
    }
}
