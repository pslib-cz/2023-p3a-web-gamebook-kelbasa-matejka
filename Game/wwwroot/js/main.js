document.addEventListener("DOMContentLoaded", function () {
    const modalBtns = document.querySelectorAll(".modal-open");
    const modal = document.getElementById("modal");

    modalBtns.forEach(function (btn) {
        btn.addEventListener("click", function () {
            modal.classList.toggle("modal--open");
        });
    });
});