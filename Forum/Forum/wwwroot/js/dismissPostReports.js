﻿var elements = document.getElementsByClassName("dismissButton");

var Ids = $(".entityId");

for (var i = 0; i < elements.length; i++) {
    var test = Ids[i];

    elements[i].onclick = function () {
        $.ajax(
            {
                type: "GET",
                url: "/Report/DismissPostReport",
                success: function (test) {
                    $('#tableDiv').html(test);
                    $.getScript("../js/reportModal.js");
                    $.getScript("../js/dismissPostReports.js");
                },
                data: {
                    "id": test.value
                }
            });
    };
}