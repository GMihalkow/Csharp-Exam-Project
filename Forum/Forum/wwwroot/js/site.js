$('.emote')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + e.target.innerText);
    });

$("#first").click(function () {
    $(this).css("background-color", "red");
    $("#second").css("background-color", "transparent");
});
$("#second").click(function () {
    $(this).css("background-color", "red");
    $("#first").css("background-color", "transparent");
});