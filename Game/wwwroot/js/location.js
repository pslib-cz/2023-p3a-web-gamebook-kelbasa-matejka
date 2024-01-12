document.addEventListener("DOMContentLoaded", function () {
    let packBtn= document.getElementById("packBtn");
    let pack = document.getElementById("pack");
    packBtn.addEventListener("click", function () {
        if (pack.classList.contains("pack--open")) {
            pack.classList.remove("pack--open");
        }
        else {
            pack.classList.add("pack--open");
        }
    });
});