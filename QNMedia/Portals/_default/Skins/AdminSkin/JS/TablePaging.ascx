<%@ Control Language="C#" ClassName="QNMedia.JS.TablePaging"  %>
<script>    
    const TablePagingDefaultOftion = {
        page: { size: 10, curent: 0 },
        rowsnumber: { display: true, name: "STT", class: "STT"},
        service: { api: "" },
        pageoption: { size: "size", page: "page",visible: 5 },
        cols: []
    };
    HTMLElement.prototype.TablePaging = function (op) {
        window["tableoptions" + this.id] = Object.assign({}, TablePagingDefaultOftion, op);
        var options = window["tableoptions" + this.id];
        var lstcolumn = options.cols;
        var columndisplay = "";
        // create header 
        if (options.rowsnumber.display) {
            columndisplay += "<th style='width:30px;text-align:center;vertical-align:middle;border-left:0;'  classname='stt'>" + options.rowsnumber.name + "</th>";
        }
        for (var i = 0; i < lstcolumn.length; i++) {
            var HeaderStyle = lstcolumn[i].HeaderStyle ? lstcolumn[i].HeaderStyle : "";
            columndisplay += "<th style='" + HeaderStyle + ";'>" + lstcolumn[i].Name + "</th>";
        }
        // create data content
        var sizename = `{${options.pageoption.size}}`;
        var apidata = options.service.api.replace(sizename, options.page.size);
        var pagename = `{${options.pageoption.page}}`;
        apidata = apidata.replace(pagename, options.page.curent);
        var datascrource = [];
        var data = GetAPI(apidata);
        var columdata = '';
        if (data.status == "OK") {
            datascrource = data.result;
            columdata = TablePaging_genrow(datascrource, lstcolumn, options);
        }
        //create pagging
        var table = "<table class='align-middle mb-0 table table-striped table-borderless table-hover' id='tbl" + this.id + "'><thead class='gridtitleformat'><tr>";
        table += columndisplay + "</tr>";
        table += "</thead><tbody id='gridbody_" + this.id+"'>";
        table += columdata;
        table += "</tbody></table>";
        var pagination = "<div class='pagination-container d-flex justify-content-end'  id='footer" + this.id + "'>";
        var option = "<hr id='hr" + this.id + "'/><div class='row' id='footer" + this.id + "'><div class='col-md-2 col-4 mb-2'><select class='form-select form-select-sm' name='state' id='select" + this.id + "'>"
        var next = [5, 10, 20, 50, 100];
        if (!next.includes(options.page.size)) {
            next.push(options.page.size);
            next.sort((a, b) => a - b);
        }
        next.forEach((value) => {
                option += "<option value='" + value + "'" + (value == options.page.size ? "selected" : "") + ">" + value + " dòng/ trang</option>";
        });
        option += "</select></div><div class='col-md-10 col-8' id='search" + this.id + "'>";
        pagination += "<div class='curentpage'></div><div class='pagination'></div></div>";
        option += pagination + "</div></div>";
        table += option;
        var element = document.getElementById(this.id);
        element.innerHTML = table;
        getpaging(data, options);
        $("#select" + this.id).on("change", function () {
            const pagesize = event.target.value;
            options.page.size = pagesize;
            options.page.curent = 0;
            var apidata = options.service.api.replace(sizename, options.page.size);
            var pagename = `{${options.pageoption.page}}`;
            apidata = apidata.replace(pagename, 0);
            var datascrource = [];
            var data = GetAPI(apidata);
            if (data.status == "OK") {
                datascrource = data.result;
                columdata = TablePaging_genrow(datascrource, lstcolumn, options);
                $("#gridbody_" + element.id).html(columdata);
                getpaging(data, options);
            }
        });
        function getpaging(data, options) {
            // $("#footer" + element.id + ' .curentpage').html("Tổng số  " + data.result.length + "/" + data.length);
            $("#footer" + element.id + ' .pagination').bootpag({
                total: data.totalpage,
                maxVisible: options.pageoption.visible,
                leaps: false,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on('page', function (event, num) {
                
            });
            $("#footer" + element.id + ' .pagination li').on("click",function (){
                var num = $(this).attr("data-lp");
                if (options.page.curent != num - 1) {
                    options.page.curent = num - 1;
                    var apidata = options.service.api.replace(sizename, options.page.size);
                    var pagename = `{${options.pageoption.page}}`;
                    apidata = apidata.replace(pagename, num - 1);
                    var datascrource = [];
                    var data = GetAPI(apidata);
                    if (data.status == "OK") {
                        datascrource = data.result;
                        columdata = TablePaging_genrow(datascrource, lstcolumn, options);
                        document.getElementById("gridbody_" + element.id).innerHTML = columdata;
                    }
                }
            });
        }
    }
    function TablePaging_genrow(datascrource, lstcolumn, options) {
        var columdata = '';
        for (var i = 0; i < datascrource.length; i++) {
            columdata += "<tr>";
            if (options.rowsnumber.display) {
                var stt = (options.page.curent * options.page.size) + i + 1;
                columdata += "<td style='text-align:center;vertical-align:middle;'  class='stt'>" + stt + "</td>";
            }
            for (var k = 0; k < lstcolumn.length; k++) {
                var ContentStyle = lstcolumn[k].ContentStyle ? lstcolumn[k].ContentStyle : "";
                var lstkey = [];
                var template = "";
                if (lstcolumn[k].Template) {
                    if (lstcolumn[k].Template.HTML) {
                        template = lstcolumn[k].Template.HTML;
                        var lstkey = lstcolumn[k].Template.Key;
                        if (lstkey.length > 0) {
                            for (var key = 0; key < lstkey.length; key++) {
                                template = template.toString().replaceAll("#" + lstkey[key] + "#", (datascrource[i][lstkey[key]]).toString().replaceAll("\n", "<br>").replaceAll("\'", "*_*").replaceAll("\"", "*__*"));
                            }
                        }
                    }
                } else {
                    template = (datascrource[i][lstcolumn[k].Key]).toString().replaceAll("\n", "<br>").replaceAll("\'", "*_*").replaceAll("\"", "*__*");
                }
                template = template.toString().replaceAll("#ColIndex#", stt);
                var chr = ("\\");
                template = template.toString().replaceAll("##", chr + "\"");
                if (lstcolumn[k].Code) {
                    var eq = new Function(template)();
                    eq = eq ? eq : '';
                    columdata += "<td style='" + ContentStyle + ";vertical-align:middle;'>" + eq.replaceAll("*_*", "\'").replaceAll("*__*", "\"") + "</td>";
                } else {
                    columdata += "<td style='" + ContentStyle + ";vertical-align:middle;'>" + template.replaceAll("*_*", "\'").replaceAll("*__*", "\"") + "</td>";
                }
            }
            columdata += "</tr>";
        }
        return columdata;
    }
</script>