var socket = new WebSocket('wss://localhost:44305/Message/UpdateChat');

setInterval(function () {
    var lastDates = document.getElementsByClassName("sent-on");

    var lastDateSentOn = lastDates[lastDates.length - 1];

    var onlyDate = lastDateSentOn.textContent.substring(8, lastDateSentOn.length);

    socket.send(onlyDate);
},
    1000);

socket.onmessage = (message) => {
    console.log(message.data);

    var targetDiv = $("#chat-box");

    var messagesArr = JSON.parse(message.data);
    console.log(messagesArr);
    if (messagesArr.length > 0) {
        for (var index = 0; index < messagesArr.length; index++) {

            console.log(messagesArr[index].LoggedInUser);

            if (messagesArr[index].LoggedInUser == messagesArr[index].AuthorName) {

                targetDiv.append('<div class="bg-burly text-forum p-10 m-10px half-width float-left border-5">'
                    + '<p class="bold m-0">' + messagesArr[index].AuthorName + ' said:</p>'
                    + '<p class="p-10">' + messagesArr[index].Description + '</p>'
                    + '<p class="m-0 font-14 text-end sent-on"><b>Sent on</b> ' + messagesArr[index].CreatedOn + '</p>'
                    + '</div>');
            }
            else {

                targetDiv.append('<div class="bg-burly text-forum p-10 m-10px half-width float-right border-5">'
                    + '<p class="bold m-0">' + messagesArr[index].AuthorName + ' said:</p>'
                    + '<p class="p-10">' + messagesArr[index].Description + '</p>'
                    + '<p class="m-0 font-14 text-end sent-on"><b>Sent on</b> ' + messagesArr[index].CreatedOn + '</p>'
                    + '</div>');
            }
        }
    }
    var element = document.getElementById("chat-box");

    scrollToBottom(element);
};

function scrollToBottom(el) {
    el.scrollTop = el.scrollHeight;
    el.scrollTop;
}

$('#searchButton')
    .click(function () {
        $.ajax(
            {
                type: "POST",
                url: "/Account/ChatWithSomebody",
                dataType: "text",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    recieverName: document.getElementById("myInput").value
                }),
                success: function (test) {
                    $('#messages-panel').html(test);
                    $.getScript("../js/site.js");
                    $.getScript("../js/sendMessages.js");
                    $.getScript("../js/searchForUser.js");

                    var element = document.getElementById("chat-box");
                    scrollToBottom(element);

                }
            });
    });