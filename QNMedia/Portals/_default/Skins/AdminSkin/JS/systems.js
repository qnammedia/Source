function ValidateInputValue(controlvalue, value, mscontrol = '') {
    var IsValidate = controlvalue != value;
    if (IsValidate) {
        if (document.getElementById(mscontrol)) {
            document.getElementById(mscontrol).className = "invalid-feedback d-none";
        }
        return { IsValidate: IsValidate, Value: controlvalue };
    } else {
        if (mscontrol != '') {
            document.getElementById(mscontrol).className = "invalid-feedback d-block";
        }
        return { IsValidate: IsValidate };
    }
}
function ValidateEmail(controlvalue, mscontrol) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var IsValidate = emailReg.test(controlvalue.trim());
    if (document.getElementById(mscontrol))
        document.getElementById(mscontrol).className = "invalid-feedback " + (IsValidate ? "" : "d-none");
    return { IsValidate: IsValidate, Value: controlvalue.trim() };
}
const APITIME = "/desktopModules/QNMedia/API/systems/GetCurrentDateTime";
var currentdatetime = new Date();
var setdatetime;
function GetCurentTime() {
    currentdatetime = new Date(GetAPI(APITIME));
    if (setdatetime) {
        clearInterval(setdatetime);
    }
    setdatetime = setInterval(SetCurrentTime, 1000);
    return currentdatetime;
}
function SetCurrentTime() {
    currentdatetime.setSeconds(currentdatetime.getSeconds() + 1);
}
function ValidateList(lst, key, value, mscontrol = '') {
    var IsValidate = { IsValidate: true, Value: lst };
    jQuery(lst).each(function (index) {
        if (lst[index][key] == value) {
            if (document.getElementById(mscontrol)) {
                document.getElementById(mscontrol).className = "invalid-feedback d-block";
            }
            IsValidate = { IsValidate: false };
            return;
        }
    });
    if (document.getElementById(mscontrol) && IsValidate.IsValidate) {
        document.getElementById(mscontrol).className = "invalid-feedback d-none";
    }
    return IsValidate;
}

function BindingSelect(dt, controlname, keyname, textname, textselectall = "", name = "") {
    var option = "";
    if (textselectall != "") {
        option += "<option selected value=''>" + textselectall + "</option>";
    }
    if (dt.length > 0) {

        for (var i = 0; i < dt.length; i++) {
            var value = dt[i][keyname];
            var text = dt[i][textname];
            var dataname = name != "" ? ("name='" + dt[i][name] + "'") : "";
            if (textselectall != "") {
                option += "<option  value='" + value + "' " + dataname + ">" + text + "</option>";
            } else {
                if (i == 0) {
                    option += "<option selected value='" + value + "' " + dataname + ">" + text + "</option>";
                } else {
                    option += "<option  value='" + value + "' " + dataname + ">" + text + "</option>";
                }
            }
        }
    }
    document.getElementById(controlname).innerHTML = option;
}

function GetAPI(api) {
    ShowLoadding();
    var obj = {};
    jQuery.ajax({
        type: "GET",
        url: api,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (dt) {
            obj = dt;
            HideLoadding();
        },
        error: function () {
            HideLoadding();
        }
    });
    return obj;
}

function PostAPI(api, data) {
    jQuery.ajax({
        type: "POST",
        url: api,
        contentType: "application/json; charset=utf-8",
        id: JSON.stringify(data),
        dataType: "json",
        async: false,
        success: function (dt) {
            return dt;
        },
        error: function () {
        }
    });
}
function PostAPIWait(api, data) {
    ShowLoadding();
    var rs = {
        status: true,
    };
    jQuery.ajax({
        type: "POST",
        url: api,
        contentType: "application/json; charset=utf-8",
        id: JSON.stringify(data),
        dataType: "json",
        async: false,
        success: function (dt) {
                rs.status = dt.status == 'OK';
                rs.data = dt.result;
            HideLoadding();
        },
        error: function (e) {
            rs.status = false;
            rs.message = e;
            HideLoadding();
        }
    });
    return rs;
}
function HideAllValidate() {
    $('div[class="invalid-feedback d-block"]').each(function (index, item) {
        $(item).attr('class', 'invalid-feedback d-none');
    });
}
function FormatStringdate(dt, longday = true) {
    var date = new Date(dt);
    if (longday)
        return moment(date, 'HH:mm DD/MM/YYYY').format("HH:mm MM/DD/YYYY");
    else
        return moment(date, 'DD/MM/YYYY').format("MM/DD/YYYY");
}
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}
function days(date_2, date_1) {
    let difference = date_1.getTime() - date_2.getTime();
    let TotalDays = Math.ceil(difference / (1000 * 3600 * 24));
    return TotalDays;
}
function CheckInputMinMax(obj) {
    var min = parseInt(obj.getAttribute("min"));
    var max = parseInt(obj.getAttribute("max"));
    var value = obj.value;
    if (!value) {
        obj.value = obj.defaultValue;
        return false;
    }
    else {
        if (parseInt(value) < min || parseInt(value) > max) {
            obj.value = obj.defaultValue;
            return false;
        }
    }
    return true;
}
HTMLElement.prototype.check_number = function () {
    var min = parseInt(this.getAttribute("min"));
    var max = parseInt(this.getAttribute("max"));
    var value = this.value;
    if (!value) {
        return false;
    }
    else {
        if (parseInt(value) < min || parseInt(value) > max) {
            return false;
        }
    }
    return true;
}
HTMLElement.prototype.check_date = function () {
    var min = new Date(this.getAttribute("min"));
    var max = new Date(this.getAttribute("max"));
    var value = this.value;
    if (!value) {
        return false;
    }
    else {
        var date = new Date(value);
        if (date < min || date > max) {
            return false;
        }
    }
    return true;
}
HTMLElement.prototype.compare_date = function (cpvalue, compare = 'N') {
    const valuecompare = new Date(cpvalue);
    const value = new Date(this.value);
    compare = compare.toUpperCase();
    switch (compare) {
        case 'N': return value < valuecompare;
        case 'L': return value > valuecompare;
        case 'B': return value > valuecompare;
        case 'NB': return value <= valuecompare;
        case 'LB': return value >= valuecompare;
        default: return value == valuecompare;
    }
}
HTMLInputElement.prototype.CheckInputMinMax = () => {
    var min = parseInt($(this).attr("min"));
    var max = parseInt($(this).attr("max"));
    var value = $(this).attr("value");
    var value = $(this).attr("value");
    var defaultvalue = $(this).prop('defaultValue');
    if (!value || parseInt(value) < min || parseInt(value) > max) {
        $(this).attr("value", defaultvalue);
        return false;
    }
    return true;
}
HTMLTextAreaElement.prototype.get_text = function(){
    return this.value.trim().split("\n").join("<br>");
}
HTMLTextAreaElement.prototype.set_text = function (content) {
    this.value = content.split("<br>").join("\n");
}
function isEmpty(obj) {
    return jQuery.isEmptyObject(obj);
}
HTMLInputElement.prototype.exportWord =function (filename='', title = 'XUẤT WORD') {
    var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
        "xmlns:w='urn:schemas-microsoft-com:office:word' " +
        "xmlns='http://www.w3.org/TR/REC-html40'>" +
        "<head><meta charset='utf-8'><title>" + title + "</title></head><body>";
    var footer = "</body></html>";
    if (filename == '') {
        filename = this.getAttribute("id")+"_word";
    }
    var sourceHTML = header + this.innerHTML + footer;
    var source = 'id:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
    var fileDownload = document.createElement("a");
    document.body.appendChild(fileDownload);
    fileDownload.href = source;
    fileDownload.download = filename + '.doc';
    fileDownload.click();
    document.body.removeChild(fileDownload);
}
function ToRoman(num) {
    if (typeof num !== 'number')
        return false;

    var digits = String(+num).split(""),
        key = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM",
            "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC",
            "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
        roman_num = "",
        i = 3;
    while (i--)
        roman_num = (key[+digits.pop() + (i * 10)] || "") + roman_num;
    return Array(+digits.join("") + 1).join("M") + roman_num;
}


function SetButtonScrollEvent(next, back, divcontainer) {
    var button = document.getElementById(next);
    button.onclick = function () {
        var container = document.getElementById(divcontainer);
        sideScroll(container, 'right', 15, 300, 25);
    };

    var back = document.getElementById(back);
    back.onclick = function () {
        var container = document.getElementById(divcontainer);
        sideScroll(container, 'left', 15, 300, 25);
    };
}

function sideScroll(element, direction, speed, distance, step) {
    scrollAmount = 0;
    var slideTimer = setInterval(function () {
        if (direction == 'left') {
            element.scrollLeft -= step;
        } else {
            element.scrollLeft += step;
        }
        scrollAmount += step;
        if (scrollAmount >= distance) {
            window.clearInterval(slideTimer);
        }
    }, speed);
}
function TextRowChange(e) {
    var datarows = e.attr("data-rows");
    if ((e.attr("class")).includes("text-0")) {
        e.attr("class", e.attr("class").replace("text-0", "text-" + datarows));
    } else {
        e.attr("class", e.attr("class").replace("text-" + datarows, "text-0"));
    }
}
function GetTimeOut(curentdate, fromtime, totalsecond) {
    var sec = totalsecond - (curentdate.getTime() - new Date(fromtime).getTime()) / 1000;
    return Math.floor(sec);
}
var taskcount = 0;
function ShowLoadding() {
    spinnermodal.className = 'spinnermodal';
    taskcount++;
}
function HideLoadding() {
    setTimeout(function () {
        if (taskcount <= 1) {
            spinnermodal.className = 'spinnermodal d-none';
        }
        taskcount--;
    }, 0);
}
function ListToCompleteData(lst, keyname, keyvalue) {
    var rs = [];
    if (lst.length > 0) {
        lst.forEach((item) => {
            rs.push({ text: item[keyname],value:item[keyvalue]});
        });
    }
    return rs;
}
Array.prototype.toCompleteData = function (keyname, keyvalue) {
    var rs = [];
    this.forEach((item) => {
        rs.push({ text: item[keyname], value: item[keyvalue] });
    });
    return rs;
}
Array.prototype.filterLike = function(lstdatakey, filter,load=false) {
    filter = filter.trim().toLowerCase();
    if (filter == '') return $(this);
    if (load)  ShowLoadding();
    var newlst = [];
    var lst =$(this);
    for (const child of  lst){
        var item = child;
        var match = 0;
        var str = "";
        lstdatakey.forEach((key) => {
            str += child[key].toLowerCase();
        });
        if (str.includes(filter)) {
            item["match"] = 100;
            newlst.push(item);
            continue;
        }
        str = removeDiacritics(str);
        filter = removeDiacritics(filter);
        if (str.includes(filter)) {
            item["match"] = 99;
            newlst.push(item);
            continue;
        }
        filter.split(' ').forEach((item) => {
            if (str.includes(item)) {
                match++;
            }
            if (match >= 5) {
                match = 80;
                return;
            }
        });
        if (match > 0) {
            item["match"] = match;
            newlst.push(item);
        }
    };
    if (load) HideLoadding();
    return newlst.sort((a, b) => b.match - a.match);
}
HTMLElement.prototype.set_req = function (message) {
    this.setCustomValidity(message);
    this.reportValidity();
}
HTMLElement.prototype.check_null = function (message) {
    return this.value.trim().replaceAll("\n","") != '';
}
HTMLElement.prototype.set_option = function (dt, keyname, textname, datagroup = "", textselectall = "") {
    var option = "";
    if (textselectall != "") {
        option += "<option selected value='' disabled>" + textselectall + "</option>";
    }
    if (dt.length > 0) {
        for (var i = 0; i < dt.length; i++) {
            var value = dt[i][keyname];
            var text = dt[i][textname];
            var dataname = datagroup != "" ? ("datagroup='" + dt[i][datagroup] + "'") : "";
            if (textselectall != "") {
                option += "<option  value='" + value + "' " + dataname + ">" + text + "</option>";
            } else {
                if (i == 0) {
                    option += "<option selected value='" + value + "' " + dataname + ">" + text + "</option>";
                } else {
                    option += "<option  value='" + value + "' " + dataname + ">" + text + "</option>";
                }
            }
        }
    }
    this.innerHTML = option;
}
function getParamValue(paramName) {
    var url = window.location.search.substring(1); //get rid of "?" in querystring
    var qArray = url.split('&'); //get key-value pairs
    for (var i = 0; i < qArray.length; i++) {
        var pArr = qArray[i].split('='); //split key and value
        if (pArr[0] == paramName)
            return pArr[1]; //return value
    }
}
HTMLElement.prototype.addMenuItem = function (link, grouptitle, title, liclass = "") {
    var id = this.getAttribute("id");
    this.innerHTML += `<li class="${liclass}">
            <a href="${link}" id="${id + link}" root="${grouptitle}" target="${id}" title="${title}" class="menu-action">
                <i class="bi bi-circle"></i><span>${title}</span>
            </a>
     </li>`;
   
}
HTMLElement.prototype.set_disabled = function (disabled=true) {
    if (disabled) {
        this.setAttribute("disabled", "disabled");
    } else {
        this.removeAttribute("disabled");
    }
}
Array.prototype.get_distinct = function (key) {
    var data = this;
    var rs = [];
    var keys = [];
    this.forEach((item) => {
        if (!keys.includes(item[key])) {
            rs.push(data.filter(x => x[key] == item[key])[0]);
            keys += item[key];
        }
    });
    return rs;
}
Array.prototype.update_value = function (key, datakey, updatekey,updatevalue) {
    var index = this.findIndex(x => x[key] == datakey);
    this[index][updatekey] = updatevalue;
}
Array.prototype.update_allvalue = function (key, datakey, updatekey, updatevalue) {
    this.forEach((item) + function () {
        if (item[key] == datakey) {
            item[updatekey] = updatevalue;
        }
    });
}
Array.prototype.update = function (key, datakey, updatevalue) {
    var index = this.findIndex(x => x[key] == datakey);
    this[index]= updatevalue;
}
function TempColSort(classname,idkey='ID',valuekey='ThuTu',min=0,max=100) {
    return `<input class="form-control form-control-sm text-end ${classname}" type="number" data-id="#${idkey}#" value="#${valuekey}#" min='${min}' max="${max}">`;
}
function TempColCheck(classname, idkey, valuekey) {
    return 'return `<input type="checkbox" class="form-checked-input ' + classname + '" data-id="#' + idkey + '#"  \${#' + valuekey + '#?"checked":""}>`;';
}

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie(cname) {
    let ck = getCookie(cname);
    return (ck != "");
}
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}
var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
HTMLElement.prototype.get_number = function() {
    var str = this.value;
    return str.replace(/\D/g,'');
}

HTMLElement.prototype.set_date = function (date) {
    try {
        var currentDate = date.toISOString().substring(0, 10);
        this.value = currentDate;
    } catch (e) {
        date = new Date(date);
        date.setUTCHours(24, 0, 0, 0);
        var currentDate = date.toISOString().substring(0, 10);
        this.value = currentDate;
    }
    
}
Array.prototype.to_treeoption = function (parentname, keyname, valuename, root = 0) {
    var treedata = [];
    var data = this;
    var rootdata = data.filter(x => x[parentname] == root);
    rootdata.forEach(function (item) {
        var treeitem = { title: item[keyname], id: item[valuename] };
        var filterchilddata = data.filter(x => x[parentname] == item[valuename]);
        if (filterchilddata.length > 0) {
            treeitem.subs = [];
            filterchilddata.forEach(function (item) {
                treeitem.subs.push(filterchild( item));
            });
        }
        treedata.push(treeitem);
    });
    function filterchild(child) {
        var treeitem = { title: child[keyname], id: child[valuename] };
        var filterchilddata = data.filter(x => x[parentname] == child[valuename]);
        if (filterchilddata.length > 0) {
            treeitem.subs = [];
            filterchilddata.forEach(function (item) {
                treeitem.subs.push(filterchild(item));
            });
        }
        return treeitem;
    }
    return treedata;
}