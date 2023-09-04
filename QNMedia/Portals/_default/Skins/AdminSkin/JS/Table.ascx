<%@ Control Language="C#" ClassName="QNMedia.JS.Table"  %>
<script type="text/javascript">
    var datascources = [];
    var lstcolumn = [{ Name: "Tên", Key: "Ten", HeaderStyle: "text-align:center", ContentStyle: "text-align:center;", Onclick: { Event: "", Par: {} } }, { Name: "Tuổi", Key: "Age", HeaderStyle: "width:25%;text-align:center" }, { Name: "Lớp", Key: "Class", HeaderStyle: "width:25%" }];
    //GenTable("testtable3", {
    //    Data: datascources,
    //    ListColumn: lstcolumn,
    //    Header: { AllowCustom: true, Template: header, NameOrder: "TT" },
    //    Items: { AllowCustom: false, Template: body, ItemPage: 6 }
    //});

    HTMLElement.prototype.DataTable = function (tableinfo, lstcolumn) {
        var control = this;
        controlname = control.id;
        var allowcustomheader = tableinfo.Header.AllowCustom;
        var allowcustomdata = tableinfo.Items.AllowCustom;
        var rowseventkey = tableinfo.Items.EventKeys;
        var rowattribute = tableinfo.Items.Attr;
        var rowsid = tableinfo.Items.ID;
        var datascrource = tableinfo.Data;
        var columdata = "";
        var columdisplay = "";
        var itemspagecount = tableinfo.Items.ItemPage;
        var ordername = tableinfo.Header.NameOrder;
        var lstcolumn = tableinfo.ListColumn;
        var ChildKey = tableinfo.Items.ChildKey;
        var GroupFKey = '';
        var GroupFName = '';
        var ItemGroupName = '';
        var ItemGroupKey = '';
        if (tableinfo.Items.GroupTF) {
            GroupFKey = tableinfo.Items.GroupTF.Key;
            GroupFName = tableinfo.Items.GroupTF.Name;
            GroupFOrder = tableinfo.Items.GroupTF.OrderTrue;
            ItemGroupKey = tableinfo.Items.Groups.Key;
        }
        if (tableinfo.Items.Groups) {
            ItemGroupKey = tableinfo.Items.Groups.Key;
            ItemGroupName = tableinfo.Items.Groups.Name;
        }
        if (!allowcustomheader) {
            if (ordername) {
                columdisplay += "<th style='width:30px;text-align:center;vertical-align:middle;border-left:0;' classname='stt'>" + ordername + "</th>";
            }
            for (var i = 0; i < lstcolumn.length; i++) {
                var borderleft = '';
                if (!ordername) {
                    if (i == 0) {
                        borderleft = "border-left:0;";
                    }
                }
                var HeaderStyle = lstcolumn[i].HeaderStyle ? lstcolumn[i].HeaderStyle : "";
                columdisplay += "<th style='" + HeaderStyle + ";" + borderleft + "'>" + lstcolumn[i].Name + "</th>";
            }
        } else {
            columdisplay = tableinfo.Header.Template;
        }
        var colcount = lstcolumn.length;
        if (!allowcustomdata) {
            if (GroupFKey != '') {
                var lst1 = [];
                var lst2 = [];
                var groupfname1 = '';
                var groupfname2 = '';
                if (GroupFOrder) {
                    lst1 = datascrource.filter(x => x[GroupFKey]);
                    lst2 = datascrource.filter(x => !x[GroupFKey]);
                    groupfname1 = new Function(GroupFName.replace("#" + GroupFKey + "#", 'true'))();
                    groupfname2 = new Function(GroupFName.replace("#" + GroupFKey + "#", 'false'))();
                }
                else {
                    lst1 = datascrource.filter(x => !x[GroupFKey]);
                    lst2 = datascrource.filter(x => x[GroupFKey]);
                    groupfname1 = new Function(GroupFName.replace("#" + GroupFKey + "#", 'false'))();
                    groupfname2 = new Function(GroupFName.replace("#" + GroupFKey + "#", 'true'))();
                }
                var sttcol = 'A';
                if (lst1.length > 0) {
                    columdata += "<tr><td colspan='" + colcount + "'><b>" + sttcol + ". " + groupfname1.toUpperCase() + " (" + lst1.length + ")" + "</b></td></tr>";
                    sttcol = 'B';
                    if (ItemGroupKey != '') {
                        var grouplist = [...new Map(lst1.map(item => [item[ItemGroupKey], item])).values()];
                        grouplist.sort(function (a, b) { return (+a[ItemGroupKey]) - (+b[ItemGroupKey]); });
                        for (var i = 0; i < grouplist.length; i++) {
                            var newlst = lst1.filter(x => x[ItemGroupKey] == grouplist[i][ItemGroupKey]);
                            columdata += "<tr><td colspan='" + colcount + "'><b>" + Romanize(i + 1) + ". " + grouplist[i][ItemGroupName].toUpperCase() + " (" + newlst.length + ")" + "</b></td></tr>";
                            columdata += GenTableRows(newlst);
                        }
                    }
                    else {
                        columdata += GenTableRows(lst1);
                    }
                }
                if (lst2.length > 0) {
                    columdata += "<tr><td colspan='" + colcount + "'><b>" + sttcol + ". " + groupfname2.toUpperCase() + " (" + lst2.length + ")" + "</b></td></tr>";
                    if (ItemGroupKey != '') {
                        var grouplist = [...new Map(lst2.map(item => [item[ItemGroupKey], item])).values()];
                        grouplist.sort(function (a, b) { return (+a[ItemGroupKey]) - (+b[ItemGroupKey]); });
                        for (var i = 0; i < grouplist.length; i++) {
                            var newlst = lst2.filter(x => x[ItemGroupKey] == grouplist[i][ItemGroupKey]);
                            columdata += "<tr><td colspan='" + colcount + "'><b>" + Romanize(i + 1) + ". " + grouplist[i][ItemGroupName].toUpperCase() + " (" + newlst.length + ")" + "</b></td></tr>";
                            columdata += GenTableRows(newlst);
                        }
                    }
                    else {
                        columdata += GenTableRows(lst1);
                    }
                }
            }
            else {

                if (ItemGroupKey != '') {
                    var grouplist = [...new Map(datascrource.map(item => [item[ItemGroupKey], item])).values()];
                    grouplist.sort(function (a, b) { return (+a[ItemGroupKey]) - (+b[ItemGroupKey]); });
                    for (var i = 0; i < grouplist.length; i++) {
                        var newlst = datascrource.filter(x => x[ItemGroupKey] == grouplist[i][ItemGroupKey]);
                        columdata += "<tr><td colspan='" + colcount + "'><b>" + Romanize(i + 1) + ". " + grouplist[i][ItemGroupName].toUpperCase() + " (" + newlst.length + ")" + "</b></td></tr>";
                        columdata += GenTableRows(newlst);
                    }
                }
                else {
                    columdata += GenTableRows(datascrource);
                }
            }

        } else {
            columdata = tableinfo.ItemTemplate;
        }
        function GenTableRows(list) {
            var columdata = "";
            for (var i = 0; i < list.length; i++) {
                var lst = [];
                lst.push(list[i]);
                var tt = i + 1;
                columdata += GenRows(lst, tt);
                if (ChildKey) {
                    if (list[i][ChildKey]) {
                        var child = GenRowsChild(list[i][ChildKey], tt);
                        if (child) {
                            columdata += child;
                        }
                    }
                }
            }
            return columdata;
        }
        function Romanize(num) {
            if (isNaN(num))
                return NaN;
            var digits = String(+num).split(""),
                key = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM",
                    "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC",
                    "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
                roman = "",
                i = 3;
            while (i--)
                roman = (key[+digits.pop() + (i * 10)] || "") + roman;
            return Array(+digits.join("") + 1).join("M") + roman;
        }
        function GenRowsChild(lst, tt) {
            return GenRows(lst, tt, true, 4);
        }
        function GenRows(datascrource, tt = 0, child = false, level = 0) {
            var columdata = "";
            for (var i = 0; i < datascrource.length; i++) {
                var rowsevent = tableinfo.Items.EventName;
                var tr = "<tr ";
                if (rowsid != '') {
                    var id = datascrource[i][rowsid]
                    tr += " data-id='" + id + "' id='tr_" + controlname + "_" + id + "' ";
                }
                if (rowsevent) {
                    if (rowseventkey) {
                        for (var key = 0; key < rowseventkey.length; key++) {
                            rowsevent = rowsevent.replaceAll("#" + rowseventkey[key] + "#", datascrource[i][rowseventkey[key]]);
                        }
                    }
                    if (rowattribute)
                        tr += rowattribute;
                    tr += " style='cursor:pointer;' onclick='" + rowsevent + ";'>";
                } else {
                    if (rowattribute)
                        tr += rowattribute;
                    tr += ">";
                }
                columdata += tr;
                var stt = child ? (tt + '.' + (i + 1)) : (tt);
                if (ordername) {
                    var colstt = level != 0 ? ("<i class='ps-" + level + " childrow w-100'>" + stt + "</i>") : (stt);
                    columdata += "<td style='text-align:center;vertical-align:middle;'  class='stt'>" + colstt + "</td>";
                }
                for (var k = 0; k < lstcolumn.length; k++) {
                    var ContentStyle = lstcolumn[k].ContentStyle ? lstcolumn[k].ContentStyle : "";
                    if (!lstcolumn[k].AllowCustom) {
                        columdata += "<td style='" + ContentStyle + ";vertical-align:middle;'>" + datascrource[i][lstcolumn[k].Key] + "</td>";
                    } else {
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
                }
                columdata += "</tr>";
            }
            return columdata;
        }
        //var table = "<table class='align-middle mb-0 table table-borderless table-striped table-hover' id='tbl" + controlname + "'><thead class='gridtitleformat'><tr>";
        var table = "<table class='align-middle mb-0 table table-striped table-borderless table-hover' id='tbl" + controlname + "'><thead class='gridtitleformat'><tr>";
        table += columdisplay + "</tr>";
        table += "</thead><tbody>";
        table += columdata;
        table += "</tbody></table>";
        var pagination = "<div class='pagination-container d-flex justify-content-end'>";

        //pagination += option;
        //table = option + table;
        if (datascrource.length > itemspagecount) {
            var option = "<hr id='hr" + controlname + "'/><div class='row' id='footer" + controlname + "'><div class='col-md-2 col-4  mb-2'><select class='form-select form-select-sm' name='state' id='select" + controlname + "'>"
            //option+="<option value='5000'>Tất cả</option>";
            var countdata = datascrource.length + 1;
            for (var i = 1; i < 5; i++) {
                var next = i * itemspagecount;
                //if (countdata + itemspagecount> next ) {
                option += "<option value='" + next + "'>" + next + " dòng/ trang</option>";
                //}
            }
            option += "</select></div><div class='col-md-10 col-8 ' id='search" + controlname + "'>";
            pagination += "<div><nav><ul class='pagination' id='pg" + controlname
                + "'><li class='page-item' data-page='prev'><span>&laquo; <span class='sr-only'>(current)</span></span> </li>"
                + "<li class='page-item' data-page='next' id='prev" + controlname + "'> <span>&raquo; <span class='sr-only'>(current)</span></span></li></ul></nav></div></div>";
            option += pagination + "</div></div>";

            table += option;
        } else {
            table += "<div class=' d-none' id='footer" + controlname + "'></div>";
        }
        document.getElementById(controlname).innerHTML = table;
        getPagination(controlname, countdata, itemspagecount);
        var curentpg = window["curentpg" + controlname];
        if (curentpg) {
            $('#pg' + controlname + ' li[data-page="' + curentpg + '"]').click();
        }
    }
    function getPagination(controlname, totalRows = 0, itemspagecount = 6) {
        var lastPage = 1;
        var table = "#tbl" + controlname;
        $("#select" + controlname)
            .on('change', function (evt) {
                //$('.paginationprev').html('');						// reset pagination
                lastPage = 1;
                $('#pg' + controlname)
                    .find('li')
                    .slice(1, -1)
                    .remove();
                var trnum = 0; // reset tr counter
                var maxRows = parseInt($(this).val()); // get Max Rows from select option
                if (maxRows > totalRows) {
                    $('#pg' + controlname).hide();
                } else {
                    $('#pg' + controlname).show();
                }
                var totalRows = $(table + ' tbody tr').length;
                $(table + ' tr:gt(0)').each(function () {
                    // each TR in  table and not the header
                    trnum++; // Start Counter
                    if (trnum > maxRows) {
                        // if tr number gt maxRows
                        $(this).hide(); // fade it out
                    }
                    if (trnum <= maxRows) {
                        $(this).show();
                    } // else fade in Important in case if it ..
                }); //  was fade out to fade it in
                //	numbers of pages
                if (totalRows > maxRows) {
                    $('#pg' + controlname).show();
                    var totalpage = Math.ceil(totalRows / maxRows);
                    for (var i = 1; i <= totalpage;) {
                        // for each page append pagination li
                        $('#prev' + controlname)
                            .before(
                                '<li data-page="' +
                                i +
                                '">\
								  <span>' +
                                i++ +
                                '<span class="sr-only">(current)</span></span>\
								</li>'
                            )
                            .show();
                    } // end for i

                } else {
                    $('#pg' + controlname).hide();
                    window["curentpg" + controlname] = 1;
                }
                $('#pg' + controlname + '  [data-page="1"]').addClass('active'); // add active class to the first li
                $('#pg' + controlname + ' li').on('click', function (evt) {
                    // on click each page
                    evt.stopImmediatePropagation();
                    evt.preventDefault();
                    var pageNum = $(this).attr('data-page'); // get it's number

                    var maxRows = parseInt($("#select" + controlname).val()); // get Max Rows from select option

                    if (pageNum == 'prev') {
                        if (lastPage == 1) {
                            return;
                        }
                        pageNum = --lastPage;
                    }
                    if (pageNum == 'next') {
                        if (lastPage == $('#pg' + controlname + ' li').length - 2) {
                            return;
                        }
                        pageNum = ++lastPage;
                    }

                    lastPage = pageNum;
                    window["curentpg" + controlname] = pageNum;

                    var trIndex = 0; // reset tr counter
                    $('#pg' + controlname + ' li').removeClass('active'); // remove active class from all li
                    $('#pg' + controlname + '  [data-page="' + lastPage + '"]').addClass('active'); // add active class to the clicked
                    // $(this).addClass('active');					// add active class to the clicked
                    limitPagging(controlname, itemspagecount);
                    $(table + ' tr:gt(0)').each(function () {
                        // each tr in table not the header
                        trIndex++; // tr index counter
                        // if tr index gt maxRows*pageNum or lt maxRows*pageNum-maxRows fade if out
                        if (
                            trIndex > maxRows * pageNum ||
                            trIndex <= maxRows * pageNum - maxRows
                        ) {
                            $(this).hide();
                        } else {
                            $(this).show();
                        } //else fade in
                    }); // end of for each tr in table
                }); // end of on click pagination list
                limitPagging(controlname, itemspagecount);
            })
            .val(itemspagecount)
            .change();
    }

    function limitPagging(controlname, itemscount = 5) {
        if ($('#pg' + controlname + ' li').length > 7) {
            if ($('#pg' + controlname + ' li.active').attr('data-page') <= 3) {
                $('#pg' + controlname + ' li:gt(5)').hide();
                $('#pg' + controlname + ' li:lt(5)').show();
                $('#pg' + controlname + ' [data-page="next"]').show();
            }
            if ($('#pg' + controlname + ' li.active').attr('data-page') > 3) {
                $('#pg' + controlname + ' li:gt(0)').hide();
                $('#pg' + controlname + ' [data-page= "next"]').show();
                for (let i = parseInt($('#pg' + controlname + ' li.active').attr('data-page')) - 2;
                    i <= parseInt($('#pg' + controlname + ' li.active').attr('data-page')) + 2; i++) {
                    $('#pg' + controlname + ' [data-page="' + i + '"]').show();
                }
            }
        }
    }
</script>
<script type="text/javascript">
</script>
<style>
         

        </style>
<style>
    .pagination li:hover {
        cursor: pointer;
    }

    table tr:nth-child(even) {
        background-color: #fff;
    }

    .pagination > li {
        display: inline;
    }

        .pagination > li > a:focus, .pagination > li > a:hover, .pagination > li > span:focus, .pagination > li > span:hover {
            z-index: 2;
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }

    .pagination > .active > a, .pagination > .active > a:focus, .pagination > .active > a:hover, .pagination > .active > span, .pagination > .active > span:focus, .pagination > .active > span:hover {
        z-index: 3;
        color: #fff;
        cursor: default;
        background-color: #337ab7;
        border-color: #337ab7;
    }

    .pagination > li:first-child > a, .pagination > li:first-child > span {
        margin-left: 0;
        border-top-left-radius: 4px;
        border-bottom-left-radius: 4px;
    }

    .pagination > li > a, .pagination > li > span {
        position: relative;
        float: left;
        padding: 6px 12px;
        margin-left: -1px;
        line-height: 1.42857143;
        color: #337ab7;
        text-decoration: none;
        background-color: #fff;
        border: 1px solid #ddd;
    }

    .sr-only {
        position: absolute;
        width: 1px;
        height: 1px;
        padding: 0;
        margin: -1px;
        overflow: hidden;
        clip: rect(0,0,0,0);
        border: 0;
    }

    .pagination {
        margin: 0;
    }

    .gridtitleformat {
        vertical-align: middle !important;
        border-bottom: 1px solid rgba(26,54,126,.125);
        /*background: #198754;*/
        background: #fff;
        color: #495057;
    }

        .gridtitleformat th {
            /*border: 1px solid #7a6868;*/
            text-align: center;
            line-height: 2;
        }

    tfoot tr {
        display: inherit !important;
    }

    .table {
        width: 100%;
        margin-bottom: 1rem;
        background-color: transparent;
    }

    .table-responsive {
        overflow-x: hidden !important;
        overflow-y: auto;
        height: 100%;
    }

        .table-responsive thead th {
            position: sticky;
            top: 0;
            z-index: 1;
            /*background: #198754;*/
            background: #fff;
            /* border-left:1px solid #a4a8ad;*/
        }

        .table-responsive tbody th {
            position: sticky;
            left: 0;
        }
</style>