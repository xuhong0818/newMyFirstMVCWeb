var tableId = document.getElementById('table1');
for (var i = 1; i < tableId.rows.length; i) {
    alert(tableId.rows[i].cells[1].innerHTML);
}
