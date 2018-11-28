$('.emote')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + e.target.innerText);
    });

//TODO: Finish input index implementing
$('.text')
    .off('click')
    .click(function (e) {
        var textarea = $('#Description');
        textarea.val(textarea.val() + $(this).val());
        
    });