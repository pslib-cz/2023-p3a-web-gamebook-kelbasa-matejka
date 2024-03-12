document.addEventListener("DOMContentLoaded", function () {
    const modalAboutBtns = document.querySelectorAll(".modal-about-open");
    console.log(modalAboutBtns);
    const modalAbout = document.getElementById("modal-about");
    console.log(modalAbout);
    const modalLeadboardBtns = document.querySelectorAll(".modal-leadboard-open");
    console.log(modalLeadboardBtns)
    const modalLeadboard = document.getElementById("modal-leadboard");
    console.log(modalLeadboard);

    modalLeadboardBtns.forEach(function (btn) {
        btn.addEventListener("click", function () {
            modalLeadboard.classList.toggle("modal--open");
        });
    });

    modalAboutBtns.forEach(function (btn) {
        btn.addEventListener("click", function () {
            modalAbout.classList.toggle("modal--open");
        });
    });
});