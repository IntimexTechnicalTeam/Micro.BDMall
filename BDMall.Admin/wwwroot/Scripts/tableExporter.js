/* 
 * jQuery-tableExport - v1.0 - 2016-05-25
 * https://github.com/Archakov06/jQuery-tableExport
 * Released under the MIT License
*/

(function ($) {

    $.fn.tableExport = function (options) {

        var defaults = $.extend({
            filename: 'table',
            format: 'csv',
            cols: '',
            head_delimiter: ',',
            separator: ",",
            column_delimiter: ',',
            onbefore: function (t) { },
            onafter: function (t) { }
        }, options);

        var options = $.extend(defaults, options);
        var $this = $(this);
        var cols = options.cols ? options.cols.split(',') : [];
        var result = '';
        var data_type = { 'csv': 'text/csv', 'txt': 'text/plain', 'xls': 'application/vnd.ms-excel', 'json': 'application/json', };

        if (typeof options.onbefore != "function" || typeof options.onafter != "function" || !options.format || !options.head_delimiter || !options.column_delimiter || !options.filename) { console.error('One of the parameters is incorrect.'); return false; }

        function getHeaders() {
            var th = $this.find('thead th');
            var arr = [];

            th.each(function (i, e) {

                if (cols.length)
                    cols.forEach(function (c) {
                        if (c == i + 1)
                            arr.push(e.innerText);
                    });
                else
                    arr.push(e.innerText);

            });

            return arr;
        }

        function getItems() {
            var tr = $this.find('tbody tr');
            var arr = [];

            tr.each(function (i, e) {
                var s = [];

                if (cols.length) {
                    cols.forEach(function (c) {
                        var innerText = $(e).find('td:nth-child(' + c + ')').text();
                        innerText = innerText.ReplaceAll('"', '""');
                        if (innerText.indexOf(',') > 0) {
                            innerText = '"' + innerText + '"';
                        }
                        s.push(innerText);
                    });
                    arr.push(s);
                }
                else {
                    var td = $(e).find('td');
                    td.each(function (i, t) {
                        var innerText = t.innerText;
                        innerText = innerText.ReplaceAll('"', '""');
                        if (innerText.indexOf(',') > 0) {
                            innerText = '"' + innerText + '"';
                        }
                        s.push(innerText);
                    });
                    arr.push(s);
                }

            });

            return arr;
        }
        function checkBrowser() {

        }
        function download(data, filename, format) {
            ua = navigator.userAgent;
            ua = ua.toLocaleLowerCase();
            var now = new Date();
            var time_arr = [
                'DD:' + now.getDate(),
                'MM:' + now.getDate(),
                'YY:' + now.getFullYear(),
                'hh:' + now.getHours(),
                'mm:' + now.getMinutes(),
                'ss:' + now.getSeconds()
            ];

            for (var i = 0; i < time_arr.length; i++) {
                var key = time_arr[i].split(':')[0];
                var val = time_arr[i].split(':')[1];
                filename = filename.replace('%' + key + '%', val);
            }
            if (ua.match(/msie/) != null || ua.match(/trident/) != null) {
                // has module unable identify ie11 and Edge
                if (ua.match(/msie ([\d.]+)/) != null) {
                    if (ua.match(/msie ([\d.]+)/)[1] < 10) {
                        var oWin = window.top.open("about:blank", "_blank");
                        oWin.document.write('sep=,\r\n' + data);
                        oWin.document.close();
                        oWin.document.execCommand('SaveAs', true, filename + '.' + format);
                        oWin.close();
                    }
                    else {
                        var BOM = "\uFEFF";
                        var csvData = new Blob([BOM + data], { type: data_type[format] });
                        navigator.msSaveBlob(csvData, filename + '.' + format);
                    }

                } else if (ua.match(/rv:([\d.]+)/) != null) {
                    var BOM = "\uFEFF";
                    var csvData = new Blob([BOM + data], { type: data_type[format] });
                    navigator.msSaveBlob(csvData, filename + '.' + format);
                }

            } else if (ua.match(/chrome/) != null || ua.match(/firefox/) != null) {
                var a = document.createElement("a");
                document.body.appendChild(a);
                a.style.display = 'none';
                a.href = URL.createObjectURL(new Blob(["\uFEFF" + data], { type: data_type[format] + ";charset=utf-8;" }));
                a.download = filename + '.' + format;
                a.click();
                a.remove();
            }
        }
        options.onafter($this);
        switch (options.format) {

            case "csv":
                var headers = getHeaders();
                var items = getItems();

                //result += headers.join(options.head_delimiter) + "\n";

                items.forEach(function (item, i) {
                    result += item.join(options.column_delimiter) + "\r\n";
                });

                break;

            case "txt":
                var headers = getHeaders();
                var items = getItems();

                result += headers.join(options.head_delimiter) + "\n";
                items.forEach(function (item, i) {
                    var temp = item.join(options.column_delimiter).replace(/<br>/g, '\r\n');
                    result += temp.replace(/&#8194/g, ' ');
                });

                break;

            case "xls":
                var headers = getHeaders();
                var items = getItems();
                template = '<table><thead>%thead%</thead><tbody>%tbody%</tbody></table>';

                var res = '';
                headers.forEach(function (item, i) {
                    res += '<th>' + item + '</th>';
                }); template = template.replace('%thead%', res);

                res = '';
                items.forEach(function (item, i) {
                    res += '<tr>';
                    item.forEach(function (td, i) {
                        res += '<td>' + td + '</td>';
                    });
                    res += '</tr>';
                }); template = template.replace('%tbody%', res);

                result = template;
                break;

            case "sql":
                var headers = getHeaders();
                var items = getItems();

                items.forEach(function (item, i) {
                    result += "INSERT INTO table (" + headers.join(",") + ") VALUES ('" + item.join("','") + "');";
                });

                break;

        }

        download(result, options.filename, options.format);

        options.onbefore($this);


    }

}(jQuery));