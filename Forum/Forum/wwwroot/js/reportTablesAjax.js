$('#postReportsBtn')
    .click(function () {
        $.ajax(
            {
                type: "GET",
                url: "/Report/GetAllReports",
                success: function (test) {
                    $('#tableDiv').html(test);
                }
            });
        $("#postReportsBtn").removeClass("text-white");
        $("#postReportsBtn").removeClass("menu-bg-forum");

        $("#postReportsBtn").addClass("active");
    });