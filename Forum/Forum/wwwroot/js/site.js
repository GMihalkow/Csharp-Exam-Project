$('.emote')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + e.target.innerText);
    });