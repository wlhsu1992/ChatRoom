# ChatRoom
使用WebSocket協定，實作多人匿名聊天專案。
伺服器端使用 C# 回應協定升級、編碼、解碼 WebSockt 二進制訊息。
建立多執行緒處理多客戶訊息監聽，通過訂閱-發布機制將聊天室提交訊息廣播給所有聊天室使用者。
前端使用 WebSocket API 對Socket Servr 發出連線請求。

#專案DEMO

