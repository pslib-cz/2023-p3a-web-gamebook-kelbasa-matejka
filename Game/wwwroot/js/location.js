document.addEventListener("DOMContentLoaded", function () {
    let packBtn = document.getElementById("packBtn");
    let pack = document.getElementById("pack");

    packBtn.addEventListener("click", function () {
        pack.classList.toggle("pack--open");
    });
});