$("#changeUsernameButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/ChangeUsername",
                success: function (test) {
                    $('#account-panel').html(test);
                }
            });

        $("#changeUsernameButton").removeClass("text-white");
        $("#changeUsernameButton").removeClass("menu-bg-forum");

        $("#changeUsernameButton").addClass("active");

        //if ($("#settingsButton").hasClass("active")) {
        //    $("#settingsButton").removeClass("active");

        //    $("#settingsButton").addClass("menu-bg-forum");
        //    $("#settingsButton").addClass("text-white");
        //}

    });