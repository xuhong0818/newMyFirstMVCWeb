function clickScore() {
    // body... 
    if (!document.getElementById("scores")) return false;
    var scores = document.getElementById("scores");
    scores.onmouseover = function () {
        this.style.backgroundColor = "#2aa8c6";
        this.style.cursor = "pointer";
    };
    scores.onmouseout = function () {
        this.style.backgroundColor = "#99cccc";
    };
    var ifg = true;
    scores.onmouseup = function () {
        var tables = document.getElementsByTagName("table");
        var rows = tables[0].getElementsByTagName("tr");
        var score = [];
        for (var i = 1; i < rows.length; i++) {
            score[i] = rows[i].getElementsByTagName("td");
        }
        for (i = 2; i < score.length; i++) {
            var temp = score[i][2].innerHTML;
            var temps = score[i];
            for (var j = i - 1; j >= 1; j--) {
                var now = score[j][2].innerHTML;
                if (temp < now) {
                    score[j + 1] = score[j];
                } else {
                    break;
                }
            }
            score[j + 1] = temps;
        }
        var text = [];
        for (i = 1; i < rows.length; i++) {
            text[i] = [];
        }
        for (i = 1; i < rows.length; i++) {
            for (j = 0; j < score[i].length; j++) {
                text[i][j] = score[i][j].innerHTML;
            }
        }
        if (ifg) {
            for (i = 1; i < rows.length; i++) {
                for (j = 0; j < score[i].length; j++) {
                    rows[i].cells[j].innerHTML = text[i][j];

                }
            }

        } else {
            for (i = 1; i < rows.length; i++) {
                for (j = 0; j < score[i].length; j++) {
                    rows[i].cells[j].innerHTML = text[rows.length - i][j];
                }
            }
        }
        ifg = !ifg;
    };
}
addLoadEvent(clickScore);