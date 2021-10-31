//查找表格的<th>元素，讓它們可單擊
function makeSortable(table) {
    var headers = table.getElementsByTagName("th"); /*th所有行*/
    for (var i = 0; i < headers.length; i++) {
        (function (n) {
            var flag = false;
            headers[n].onclick = function () {
                // sortrows(table,n);
                var tbody = table.tBodies[0];//第一個<tbody>
                var rows = tbody.getElementsByTagName("tr");//tbody中的所有行
                rows = Array.prototype.slice.call(rows, 0);//真實數組中的快照
                //slice(tbody中的所有行~0)獲取元素
                //基於第n個<td>元素的值對行排序
                rows.sort(function (row1, row2) {
                    var cell1 = row1.getElementsByTagName("td")[n];//獲得第n個單元格
                    var cell2 = row2.getElementsByTagName("td")[n];
                    var val1 = cell1.textContent || cell1.innerText;//獲得文本內容
                    var val2 = cell2.textContent || cell2.innerText;

                    if (val1 < val2) {

                        return -1;

                    } else if (val1 > val2) {

                        return 1;
                    } else {
                        return 0;


                    }
                });
                if (flag) {
                    rows.reverse();
                }
                //在tbody中按它們的順序把行添加到最后
                //這將自動把它們從當前位置移走，故沒必要預先刪除它們
                //如果<tbody>還包含了除了<tr>的任何其他元素，這些節點將會懸浮到頂部位置
                for (var i = 0; i < rows.length; i++) {
                    tbody.appendChild(rows[i]);

                }

                flag = !flag;
            }
        }(i));
    }
}

window.onload = function () {
    var table = document.getElementsByTagName("table")[0];
    makeSortable(table);
}