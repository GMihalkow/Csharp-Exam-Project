$("#myProfileButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/MyProfile",
                success: function (test) {
                    $('#profile-target-div').html(test);
                }
            });

        $("#myProfileButton").removeClass("text-white");
        $("#myProfileButton").removeClass("menu-bg-forum");

        $("#myProfileButton").addClass("active");

        if ($("#settingsButton").hasClass("active")) {
            $("#settingsButton").removeClass("active");

            $("#settingsButton").addClass("menu-bg-forum");
            $("#settingsButton").addClass("text-white");
        }

    });

$("#settingsButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/Settings",
                success: function (test) {
                    $('#profile-target-div').html(test);
                    $.getScript("../js/accountPanelSettings.js");
                }
            });

        $("#settingsButton").removeClass("text-white");
        $("#settingsButton").removeClass("menu-bg-forum");

        $("#settingsButton").addClass("active");


        if ($("#myProfileButton").hasClass("active")) {
            $("#myProfileButton").removeClass("active");

            $("#myProfileButton").addClass("menu-bg-forum");
            $("#myProfileButton").addClass("text-white");
        }
    });