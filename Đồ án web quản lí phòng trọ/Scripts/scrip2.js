document.addEventListener("DOMContentLoaded", function () {
    var spansDaThue = document.querySelectorAll('span[data-status="Đã thuê"]');
    var spansBaoTri = document.querySelectorAll('span[data-status="Bảo trì"]');
    var spansTrong = document.querySelectorAll('span[data-status="Trống"]');

    spansDaThue.forEach(function (span) {
        span.style.backgroundColor = "red";
    });
    spansBaoTri.forEach(function (span) {
        span.style.backgroundColor = "yellow";
    });
    spansTrong.forEach(function (span) {
        span.style.backgroundColor = "#55C2FF";
    });
});