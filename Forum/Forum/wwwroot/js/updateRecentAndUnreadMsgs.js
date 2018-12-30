setInterval(function () {
    $.ajax(
        {
            type: "GET",
            url: "/Account/RecentConversations",
            success: function (test) {
                if ($('#chat-conversations-div').html !== test.html) {
                    $('#chat-conversations-div').html(test);
                }
            }
        });
},
    3000);