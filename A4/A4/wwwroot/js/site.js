// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var cells = document.querySelectorAll('.grid-cell');
    cells.forEach(function (cell) {
        cell.addEventListener('click', function () {
            this.classList.toggle('clicked');
        });
    });
});

function selectCell(row, col) {
    document.getElementById('cell-row').value = row;
    document.getElementById('cell-col').value = col;
    document.getElementById('cell-selection-form').submit();
}