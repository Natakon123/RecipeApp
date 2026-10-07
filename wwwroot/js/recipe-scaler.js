// Dynamic serving / portion scaler for the recipe detail page
(function () {
    var scaler = document.getElementById('scaler');
    if (!scaler) return;

    var base = parseFloat(scaler.getAttribute('data-base-servings')) || 1;
    var input = document.getElementById('servings');
    var qtys = document.querySelectorAll('.qty');
    var nutrition = document.getElementById('nutrition');

    function fmt(n, digits) {
        // Round to at most `digits` decimals and drop trailing zeros
        return String(parseFloat(n.toFixed(digits)));
    }

    function current() {
        var v = parseInt(input.value, 10);
        if (isNaN(v) || v < 1) v = 1;
        if (v > 100) v = 100;
        return v;
    }

    function update() {
        var servings = current();
        var factor = servings / base;

        Array.prototype.forEach.call(qtys, function (el) {
            var baseQty = parseFloat(el.getAttribute('data-base'));
            el.textContent = isNaN(baseQty) ? '' : fmt(baseQty * factor, 2);
        });

        if (nutrition) {
            var label = nutrition.querySelector('.servings-label');
            if (label) label.textContent = servings;
            nutrition.querySelector('.total-cal').textContent = fmt(parseFloat(nutrition.dataset.cal) * servings, 0);
            nutrition.querySelector('.total-pro').textContent = fmt(parseFloat(nutrition.dataset.pro) * servings, 1);
            nutrition.querySelector('.total-carb').textContent = fmt(parseFloat(nutrition.dataset.carb) * servings, 1);
            nutrition.querySelector('.total-fat').textContent = fmt(parseFloat(nutrition.dataset.fat) * servings, 1);
        }
    }

    input.addEventListener('input', update);
    input.addEventListener('change', function () { input.value = current(); update(); });
    document.getElementById('servingsMinus').addEventListener('click', function () {
        input.value = Math.max(1, current() - 1); update();
    });
    document.getElementById('servingsPlus').addEventListener('click', function () {
        input.value = Math.min(100, current() + 1); update();
    });
    document.getElementById('servingsReset').addEventListener('click', function () {
        input.value = base; update();
    });
})();
