$("#chatButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/Chat",
                success: function (test) {
                    $('#messages-panel').html(test);
                    $.getScript("../js/site.js");
                    $.getScript("../js/globalChat.js");
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

        $("#chatButton").removeClass("text-white");
        $("#chatButton").removeClass("menu-bg-forum");

        $("#chatButton").addClass("active");

        if ($("#unreadMessagesButton").hasClass("active")) {
            $("#unreadMessagesButton").removeClass("active");

            $("#unreadMessagesButton").addClass("menu-bg-forum");
            $("#unreadMessagesButton").addClass("text-white");
        }

    });