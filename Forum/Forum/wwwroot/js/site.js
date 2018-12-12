function typeEmoticonInTextarea(el, newText) {
    var start = el.prop("selectionStart");
    var end = el.prop("selectionEnd");
    var text = el.val();
    var before = text.substring(0, start);
    var after = text.substring(end, text.length);
    el.val(before + newText + after);
    el[0].selectionStart = el[0].selectionEnd = start + newText.length;
    el.focus();
}

$(".emote")
    .off('click')
    .on("click", function (e) {
        typeEmoticonInTextarea($("#Description"), e.target.innerText);
        return false;
    });

function typeInTextarea(el, newText) {
    var start = el.prop("selectionStart");
    var end = el.prop("selectionEnd");
    var text = el.val();
    var before = text.substring(0, start);
    var after = text.substring(end, text.length);
    el.val(before + newText + after);
    el[0].selectionStart = el[0].selectionEnd = start + newText.length;
    el.focus();
}

$(".text")
    .off('click')
    .on("click", function () {
        typeInTextarea($("#Description"), $(this).val());
        return false;
    });

function myFunction() {
    if (document.getElementById("myDropdown").classList.contains("admin-dropdown-content")) {
        document.getElementById("myDropdown").classList.remove("admin-dropdown-content");
        document.getElementById("myDropdown").classList.toggle("show");
    }
    else {
        document.getElementById("myDropdown").classList.remove("show");
        document.getElementById("myDropdown").classList.toggle("admin-dropdown-content");
    }
}

window.onclick = function (event) {
    if (!event.target.classList.contains("fa-bars")) {
        document.getElementById("myDropdown").classList.remove("show");
        document.getElementById("myDropdown").classList.add("admin-dropdown-content");
    }
};

// Get the modal
var modal = document.getElementById('myModal');
// Get the button that opens the modal
var replyBtn = document.getElementById("replyBtn");


// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal 
replyBtn.onclick = function () {
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}