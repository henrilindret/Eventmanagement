// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function setform(id) {
    if (id == 1) {
        document.getElementById("form1").style.display = "block";
        document.getElementById("form2").style.display = "none";
    }
    else {
        document.getElementById("form1").style.display = "none";
        document.getElementById("form2").style.display = "block";
    }
}