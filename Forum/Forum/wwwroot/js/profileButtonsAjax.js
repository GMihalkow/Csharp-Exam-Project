$("#myProfileButton")
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Account/MyProfile",
                success: function (test) {
                    $('#profile-target-div').html(test);
                    //$.getScript("../js/reportModal.js");
                    //$.getScript("../js/dismissPostReports.js");
                }
            });

        $("#myProfileButton").removeClass("text-white");
        $("#myProfileButton").removeClass("menu-bg-forum");

        $("#myProfileButton").addClass("active");

    });