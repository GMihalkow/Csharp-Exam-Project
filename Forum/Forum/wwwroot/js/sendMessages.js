function scrollToBottom(el) {
    console.log(el);
    el.scrollTop = el.scrollHeight;
    el.scrollTop;
}

$("#sendButton")
    .click(function () {
        $.ajax(
            {
                type: "POST",
                url: "/Message/Send",
                dataType: "text",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    RecieverId: $("#RecieverId").val(),
                    Description: $("#Description").val()
                }),
                success: function (test) {
                    $('#messages-panel').html(test);

                    var element = document.getElementById('chat-box');
                    scrollToBottom(element);

                    $.getScript("../js/site.js");
                    $.getScript("../js/sendMessages.js");
                }
            });

        $.ajax(
            {
                type: "GET",
                url: "/Account/RecentConversations",
                success: function (test) {
                    $('#chat-conversations-div').html(test);
                }
            });
    });
