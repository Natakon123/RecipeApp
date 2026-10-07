// Dynamic ingredient rows for the Create / Edit recipe forms
(function () {
    var table = document.getElementById('ingredients');
    var tbody = table.querySelector('tbody');
    var template = document.getElementById('ingredient-template');
    var form = document.getElementById('recipeForm');
    var prefix = table.getAttribute('data-prefix');

    // Re-number rows 0..n-1 so ASP.NET model binding sees a contiguous list
    function reindex() {
        Array.prototype.forEach.call(tbody.querySelectorAll('tr'), function (row, i) {
            Array.prototype.forEach.call(row.querySelectorAll('input'), function (input) {
                var field = input.name.split('.').pop();
                input.name = prefix + '.Ingredients[' + i + '].' + field;
                input.id = (prefix + '_Ingredients_' + i + '__' + field).replace(/\./g, '_');
            });
        });
    }

    function addRow() {
        // Template input names look like "<prefix>.__i__.Name"; reindex() rebuilds the final names
        var holder = document.createElement('tbody');
        holder.innerHTML = template.innerHTML.replace(/\.__i__\./g, '.');
        var row = holder.querySelector('tr');
        tbody.appendChild(row);
        reindex();
        row.querySelector('input').focus();
    }

    document.getElementById('addIngredient').addEventListener('click', addRow);

    tbody.addEventListener('click', function (e) {
        if (e.target.classList.contains('remove')) {
            e.preventDefault();
            e.target.closest('tr').remove();
            reindex();
        }
    });

    form.addEventListener('submit', function () {
        // Drop rows the user left completely empty, then renumber
        Array.prototype.forEach.call(tbody.querySelectorAll('tr'), function (row) {
            var empty = Array.prototype.every.call(row.querySelectorAll('input'), function (i) {
                return i.value.trim() === '';
            });
            if (empty) row.remove();
        });
        reindex();
    });

    if (tbody.querySelectorAll('tr').length === 0) addRow();
})();
