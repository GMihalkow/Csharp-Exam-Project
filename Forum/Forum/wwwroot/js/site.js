$('.emote')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + e.target.innerText);
    });

$('.text')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + $(this).val());
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