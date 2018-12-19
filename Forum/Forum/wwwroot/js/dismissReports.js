var elements = document.getElementsByClassName("dismissButton");

var Ids = $(".entityId");
console.log(Ids);

for (var i = 0; i < elements.length; i++) {
    var test = Ids[i];  
    console.log(test.value);
    elements[i].onclick = function () {
        console.log("CLICKED");
        $.ajax(
            {
                type: "GET",
                url: "/Report/DismissPostReport",
                success: function (test) {
                    $('#tableDiv').html(test);
                    $.getScript("../js/reportModal.js");
                    $.getScript("../js/dismissReports.js");
                },
                data: {
                    "id": test.value
                }
            });
    };
}