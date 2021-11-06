using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom
{
    public class Program
    {
        public static ChatRoom chatRoom = new ChatRoom();

        // 1. 建立Webocket監聽
        // 2. 當有使用者連入後，就建立新的執行緒負責接收客戶端socket msg
        // 3. 當聊天室中有一個人發出訊息時，就將訊息廣播給其他人
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11300);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();
                Console.WriteLine("Connected");
                ClientListener client = new ClientListener(handler, chatRoom);
                chatRoom.AddClient(client);
                Thread t = new Thread(new ThreadStart(client.ReceiveData));
                t.Start();
            }
        }
    }
}
