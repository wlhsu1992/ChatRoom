using ChatRoom.Interface;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ChatRoom.Helper;
using System.Web;

namespace ChatRoom
{

    public class ClientListener : IClientListener
    {
        private string userName;
        private Socket handler;
        private ChatRoom room;

        public ClientListener(Socket handler, ChatRoom room)
        {
            this.userName = string.Empty;
            this.handler = handler;
            this.room = room;
        }

        public Socket Handler { get { return handler; } }

        public void ReceiveData()
        {
            String data = string.Empty;

            try
            {
                while (true)
                {
                    var bytes = new byte[1024];
                    int bytesRec = Handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    if (Regex.IsMatch(data, "^GET", RegexOptions.IgnoreCase))
                    {
                        var querys = HTTPHelper.ParseUrlQueryString(data);
                        userName = HttpUtility.UrlDecode(querys["nickname"]);
                        byte[] response = SocketHelper.GenerateHandShakingResponse(data);
                        handler.Send(response);
                        room.Broadcast($"{userName} 加入到聊天室當中");
                    }
                    else
                    {
                        string msg = SocketHelper.DecodeWebSocketMsg(bytes);
                        if(msg!=string.Empty) room.Broadcast($"{userName}:{msg}");
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                room.Broadcast($"{userName} 離開聊天室");
            }
        }
    }
}
