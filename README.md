# ChatRoom
* 使用WebSocket協定，實作多人匿名聊天專案。
* 伺服器端使用 C# 回應協定升級、編碼、解碼 WebSockt 二進制訊息。
* 建立多執行緒處理多客戶訊息監聽，通過訂閱-發布機制將聊天室提交訊息廣播給所有聊天室使用者。
* 前端使用 WebSocket API 對Socket Servr 發出連線請求。

# 啟動專案
+. 啟動web socket server執行檔  ChatRoom/bin/Debug/ChatRoom.exe
+. 瀏覽器執行 html file  ChatRoom/View/index.cshtml
+. 開始聊天
gif DEMO https://imgur.com/a/8QdQkCg

# 參考資料
* [MDN-Write Web Socket Server](https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/Writing_WebSocket_servers)
* [MDN-Writing WebSocket Client Applications](https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/Writing_WebSocket_client_applications)
* [MSND-Socket Server Listening](https://docs.microsoft.com/zh-tw/dotnet/framework/network-programming/listening-with-sockets)
* [MSDN-Using threads and threading](https://docs.microsoft.com/zh-tw/dotnet/standard/threading/using-threads-and-threading)


