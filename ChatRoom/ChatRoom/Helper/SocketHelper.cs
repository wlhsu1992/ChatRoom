using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatRoom
{

    public class SocketHelper
    {
        const string webSocketGUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        public static byte[] GenerateHandShakingResponse(string s)
        {
            // 1. 取得Sec-WebSocket-Key標頭值
            string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
            // 2. 連結swk與RFC6455 GUID指定值
            string swka = swk + webSocketGUID;
            // 3. 將swka 進行SHA1後進行base64轉換
            byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
            string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

            // 伺服器確認握手HTTP回應
            return Encoding.ASCII.GetBytes(
                "HTTP/1.1 101 Switching Protocols\r\n" +
                "Connection: Upgrade\r\n" +
                "Upgrade: websocket\r\n" +
                "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");
        }
        public static string DecodeWebSocketMsg(byte[] bytes)
        {
            bool fin = (bytes[0] & 0b10000000) != 0; // 是否已從客戶端發送訊息
            bool mask = (bytes[1] & 0b10000000) != 0; // 是否遮蔽有效載荷資料

            int opcode = bytes[0] & 0b00001111; // 描述接收到的訊息類型。0x1表示是text message
            int msglen = bytes[1] & 0b01111111; // 0~125 表示訊息長度；126表示以下2bytes為長度;127表示以下8bytes為長度
            int offset = 2; //Masking-key 所在偏移位元組

            if (msglen == 126) // 表示訊息長度介於 126 ~ 65536(2^16) 之間
            {
                byte[] extendedPaylaodLength = new byte[] { bytes[3], bytes[2] };
                msglen = BitConverter.ToUInt16(extendedPaylaodLength, 0);
                offset = 4;
            }
            else if (msglen == 127) // 訊息長度介於 2^16 ~ 2^64之間
            {
                // msglen = BitConverter.ToUInt64(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2], bytes[9], bytes[8], bytes[7], bytes[6] }, 0);
                // offset = 10;
            }

            string text = string.Empty;

            if (msglen == 0) text = string.Empty;
            else if (mask)
            { // 取得Masking-Key對Payload進行解碼
                byte[] decoded = new byte[msglen];
                byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                offset += 4;

                for (int i = 0; i < msglen; ++i)
                    decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]);
                text = Encoding.UTF8.GetString(decoded);
            }
            else text = "mask bit not set";

            return text;
        }
        public static byte[] EncodeWebSocketMsg(string msg)
        {
            byte[] rawData = Encoding.UTF8.GetBytes(msg);
            int frameCount = 0;
            byte[] frame = new byte[10];

            frame[0] = (byte)129;        // 10000001

            if (rawData.Length <= 125)
            {
                frame[1] = (byte)rawData.Length;
                frameCount = 2;
            }
            else if (rawData.Length >= 126 && rawData.Length <= 65535)
            {
                frame[1] = (byte)126;    // 01111110  (MASK = 0)
                int len = rawData.Length;
                frame[2] = (byte)((len >> 8) & (byte)255);
                frame[3] = (byte)(len & (byte)255);
                frameCount = 4;
            }
            else
            {
                frame[1] = (byte)127;    // 01111111
                int len = rawData.Length;
                frame[2] = (byte)((len >> 56) & (byte)255);
                frame[3] = (byte)((len >> 48) & (byte)255);
                frame[4] = (byte)((len >> 40) & (byte)255);
                frame[5] = (byte)((len >> 32) & (byte)255);
                frame[6] = (byte)((len >> 24) & (byte)255);
                frame[7] = (byte)((len >> 16) & (byte)255);
                frame[8] = (byte)((len >> 8) & (byte)255);
                frame[9] = (byte)(len & (byte)255);
                frameCount = 10;
            }
            int bLength = frameCount + rawData.Length;
            byte[] reply = new byte[bLength];
            int bLim = 0;
            for (int i = 0; i < frameCount; i++)
            {
                reply[bLim] = frame[i];
                bLim++;
            }
            for (int i = 0; i < rawData.Length; i++)
            {
                reply[bLim] = rawData[i];
                bLim++;
            }
            return reply;
        }
    }
}
