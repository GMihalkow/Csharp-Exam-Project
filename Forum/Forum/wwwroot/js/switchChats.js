var recentConversationButtons = document.getElementsByClassName("recentConversationButton");
for (var index = 0; index < recentConversationButtons.length; index++) {

    recentConversationButtons[index].onclick = function TEST(e, index) {

        var btnElement = e.target;
        var allBtns = document.getElementsByClassName("recentConversationButton");

        console.log(btnElement.classList);

        btnElement.classList.remove("text-white");
        btnElement.classList.remove("menu-bg-forum");

        btnElement.classList.remove("text-forum");
        btnElement.classList.remove("bg-white");

        btnElement.classList.add("active");

        for (var i = 0; i < allBtns.length; i++) {
            if (allBtns[i] !== btnElement) {
                if (allBtns[i].classList.contains("active")) {
                    allBtns[i].classList.remove("active");

                    allBtns[i].classList.add("menu-bg-forum");
                    allBtns[i].classList.add("text-white");
                }
            }
        }
        
        $.ajax(
            {
                type: "POST",
                url: "/Account/ChatWithSomebody",
                dataType: "text",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    recieverName: e.target.value
                }),
                success: function (test) {
                    $('#messages-panel').html(test);
                    $.getScript("../js/site.js");

                    var counter = 0;

                    var scripts = document.getElementsByName("script");
                    for (var i = 0; i < scripts.length; i++) {
                        if (scripts[i].src == '../js/sendMessages.js') {
                            counter++;
                        }
                    }

                    if (counter == 0) {
                        $.getScript("../js/sendMessages.js");
                    }

                    var element = document.getElementById("chat-box");
                    scrollToBottom(element);
                }
            });
    };
}