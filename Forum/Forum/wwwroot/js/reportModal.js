//Getting report modal elements
var reportModals = document.getElementsByClassName('reportModal');

//Getting report modal buttons
var reportBtns = document.getElementsByClassName("reportBtn");

//Getting close buttons
var closeButtons = document.getElementsByClassName("close");
var noButtons = document.getElementsByClassName("closeBtn");

//Creating the onclick actions for all the reportBtns
function testFunction(index) {
    reportBtns[i].onclick = function () {
        reportModals[index].style.display = "block";
    };
}

for (var i = 0; i < reportBtns.length; i++) {
    testFunction(i);
}

//Creating the onclick actions for all the close buttons
for (var closeIndex = 0; closeIndex < closeButtons.length; closeIndex++) {
    closeButtons[closeIndex].onclick = function () {
        for (var index = 0; index < reportModals.length; index++) {
            reportModals[index].style.display = "none";
        }
    };
}

for (var noIndex = 0; noIndex < noButtons.length; noIndex++) {
    noButtons[noIndex].onclick = function () {
        for (var index = 0; index < reportModals.length; index++) {
            reportModals[index].style.display = "none";
        }
    };
}