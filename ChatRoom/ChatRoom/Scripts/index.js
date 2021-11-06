var button = document.querySelector("button");
var nickname = document.getElementById("nickname");
button.addEventListener("click", onClickButton);

function onClickButton() {
    var url = "chat.html?nickname=" + nickname.value;
    document.location.href = url;
}
