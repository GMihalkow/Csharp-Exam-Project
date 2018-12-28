﻿function scrollToBottom(el) {
    console.log(el);
    el.scrollTop = el.scrollHeight;
    el.scrollTop;
}

$("#chatButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/Chat",
                success: function (test) {
                    $('#messages-panel').html(test);
                    $.getScript("../js/site.js");
                    $.getScript("../js/sendMessages.js");

                    var element = document.getElementById("chat-box");
                    scrollToBottom(element);
                }
            });

        $.ajax(
            {
                type: "GET",
                url: "/Account/RecentConversations",
                success: function (test) {
                    $('#chat-conversations-div').html(test);
                    $.getScript("../js/searchForUser.js");
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