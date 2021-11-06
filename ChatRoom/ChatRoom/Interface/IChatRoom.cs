using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Interface
{
    public interface IChatRoom
    {
        void AddClient(ClientListener client);
        void RemoveClient(ClientListener client);
        void Broadcast(string sendMsg);
    }
}
