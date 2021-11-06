
var urlParams = new URLSearchParams(window.location.search);
var nickname = urlParams.get('nickname');

var websocket = new WebSocket(`ws://127.0.0.1:11300/?nickname=${nickname}`);
var sendMsgBlock = document.getElementById("send-msg");
var clsoeBtn = document.getElementById("close");

sendMsgBlock.addEventListener("keyup", onSendMsg)

window.onbeforeunload = function () {
    websocket.close();
}

websocket.onopen = function (e) {
    console.log(e.readyState);
};

websocket.onclose = function (e) {
    console.log(e.readyState);
};

websocket.onmessage = function (e) {
    writeToScreen(e.data);
};

websocket.onerror = function (e) {
    console.log(`ERROR:${e.data}`);
};

function doSend(message) {
    console.log(message);
    websocket.send(message);
}

function writeToScreen(message) {
    var html = "<div class='message_row message'>" + "<div class= 'message-content'>" +
        "<div class='message-text'>" + message + "</div></div></div>";
    var chatRoomEle = document.getElementById("chatRoom");
    chatRoomEle.insertAdjacentHTML("beforeend", html);
}

function onSendMsg(e) {
    if (e.keyCode === 13) {
        event.preventDefault();
        var msg = sendMsgBlock.value;
        if (msg) websocket.send(msg);
        sendMsgBlock.value = "";
        sendMsgBlock.focus();
    }
}

function onCheckStatus() {
    console.log(websocket.readyState);
}
