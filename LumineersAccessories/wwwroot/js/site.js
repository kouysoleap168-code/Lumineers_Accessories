// Lumineers Accessories — site.js

document.addEventListener("DOMContentLoaded", function () {
    // Auto-hide toast notifications after 3 seconds
    var toasts = document.querySelectorAll(".lumineers-toast");
    toasts.forEach(function (toast) {
        setTimeout(function () {
            toast.classList.remove("show");
            toast.style.transition = "opacity .4s ease";
            toast.style.opacity = "0";
            setTimeout(function () { toast.remove(); }, 400);
        }, 3000);
    });

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(function (anchor) {
        anchor.addEventListener("click", function (e) {
            var target = document.querySelector(this.getAttribute("href"));
            if (target) {
                e.preventDefault();
                target.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
    });

    // Quantity input: clamp to stock level on product details page
    var qtyInput = document.querySelector(".quantity-input");
    if (qtyInput) {
        var max = parseInt(qtyInput.getAttribute("max"), 10);
        if (!isNaN(max)) {
            qtyInput.addEventListener("change", function () {
                var v = parseInt(this.value, 10);
                if (isNaN(v) || v < 1) this.value = 1;
                if (v > max) this.value = max;
            });
        }
    }

    // Wishlist heart toggle animation
    document.querySelectorAll(".product-actions form button .bi-heart").forEach(function (heart) {
        var form = heart.closest("form");
        if (form) {
            form.addEventListener("submit", function () {
                heart.classList.remove("bi-heart");
                heart.classList.add("bi-heart-fill");
                heart.style.color = "#C97A7A";
            });
        }
    });
});
