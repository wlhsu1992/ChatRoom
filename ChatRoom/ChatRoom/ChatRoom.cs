using ChatRoom.Interface;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ChatRoom
{
    public class ChatRoom : IChatRoom
    {
        private List<ClientListener> clients;

        public ChatRoom()
        {
            clients = new List<ClientListener>();
        }

        public void AddClient(ClientListener client)
        {
            clients.Add(client);
        }

        public void RemoveClient(ClientListener client)
        {
            clients.Remove(client);
            client.Handler.Shutdown(SocketShutdown.Both);
            client.Handler.Close();
        }

        public void Broadcast(string sendMsg)
        {
            byte[] msg = SocketHelper.EncodeWebSocketMsg(sendMsg);
            foreach(var c in clients) {
                c.Handler.Send(msg);
            }
        }
    }
}
