var x = window.matchMedia("(max-width: 1000px)")
x.addListener(function myFunction(x) {
  if (x.matches) { // If media query matches
    var nav = document.getElementsByClassName("main-nav")[0];
    nav.classList.remove("two-thirds-width"); 
  }
  else {
    var nav = document.getElementsByClassName("main-nav")[0];
    nav.classList.add("two-thirds-width");
  }
});