const AutocompleteDefaultOftion = {
  threshold: 2,
  maximumItems: 5,
  highlightTyped: true,
  highlightClass: 'text-primary',
  text: 'text',
  value: 'value',
  showValue: false,
    showValueBeforetext: false,
    allowcustomtext: false,
    allowcustomtextevent: false,
    showclearbutton: true,
   // btnclearmt:3,
};
HTMLElement.prototype.Autocomplete = function (options) {
    var field = this;
    window["options" + field.id] = Object.assign({}, AutocompleteDefaultOftion, options);
    //window[field.id + '_selected'] = { text: null, value: null };
    var options = window["options" + field.id];
    options.selected = { text: null, value: null };
    window["dropdown" + field.id] = null;

    field.parentNode.classList.add('dropdown');
    field.setAttribute('data-bs-toggle', 'dropdown');
    field.classList.add('dropdown-toggle');

    const dropdown = ce(`<div class="dropdown-menu w-100"></div>`);
    const right = field.className.includes("form-select") ? "-r20" : "";
    const clearbtn = ce(`<button type='button' class='btn btn-link btn-clear${right} text-dark d-none' ${options.btnclearmt ? "style='margin-top:" + options.btnclearmt+"px":""}' id='btnclear${field.id}'><i class='bi bi-x-lg'></i></button>`);
    if (options.dropdownClass)
        dropdown.classList.add(options.dropdownClass);
    if (options.showclearbutton) insertAfter(clearbtn, field);
    insertAfter(dropdown, field);
    window["dropdown" + field.id] = new bootstrap.Dropdown(field, options.dropdownOptions);
    field.addEventListener('click', (e) => {
        if (createItems(field) === 0) {
            e.stopPropagation();
            window["dropdown" + field.id].hide();
        }
    });

    field.addEventListener('input', () => {
        if (options.onInput) {
            options.onInput(field.value);
        }
        options.selected = { text: null, value: null };
        renderIfNeeded(field);
    });

    field.addEventListener('keydown', (e) => {
        if (e.keyCode === 27) {
            window["dropdown" + field.id].hide();
            return;
        }
        if (e.keyCode === 40) {
            window["dropdown" + field.id]._menu.children[0]?.focus();
            e.stopPropagation();
            return;
        }
    });
    if (!options.allowcustomtext) {
        field.addEventListener('change', () => {
            field.value = options.selected.text;
        });
    }
    if (options.allowcustomtextevent){
        field.addEventListener('change', () => {
            options.onSelectItem({
                value: '',
                text: field.value
            });
        });
    }
    if (options.showclearbutton) {
        field.addEventListener('change', () => {
            var btnclass = $("#btnclear" + field.id).attr("class");
            if (field.value) {
                $("#btnclear" + field.id).attr("class", btnclass.replace(" d-none", ""));
            } else {
                if (!btnclass.includes("d-none"))
                    $("#btnclear" + field.id).attr("class", btnclass + " d-none");
            }
        });
        $("#btnclear" + field.id).click(function () {
            options.selected = { text: null, value: null };
            field.value = '';
            options.onSelectItem({
                value: '',
                text: ''
            });
            $(this).attr("class", $(this).attr("class") + " d-none");
        });
    }
}
function renderIfNeeded(field) {
    if (createItems(field) > 0)
        window["dropdown" + field.id].show();
    else
        field.click();
}

function createItem(lookup, item, options, field) {
    let text;
    if (options.highlightTyped) {
        const idx = removeDiacritics(item.text)
            .toLowerCase()
            .indexOf(removeDiacritics(lookup).toLowerCase());
        const className = Array.isArray(options.highlightClass) ? options.highlightClass.join(' ')
            : (typeof options.highlightClass == 'string' ? options.highlightClass : '');
        text = item.text.substring(0, idx)
            + `<span class="${className}">${item.text.substring(idx, idx + lookup.length)}</span>`
            + item.text.substring(idx + lookup.length, item.text.length);
    } else {
        text = item.text;
    }

    if (options.showValue) {
        if (options.showValueBeforetext) {
            text = `${item.value} ${text}`;
        } else {
            text += ` ${item.value}`;
        }
    }
    return ce(`<button type="button" class="dropdown-item" data-text="${item.text}" data-value="${item.value}">${text}</button>`);
}
function createItems(field) {
    var options = window["options" + field.id];
    if (options.data.length == 0) {
        window["dropdown" + field.id].hide();
        return 0;
    }
    const lookup = field.value;
    if (lookup.length < options.threshold) {
        window["dropdown" + field.id].hide();
        return 0;
    }

    const items = field.nextSibling;
    items.innerHTML = '';

    const keys = Object.keys(options.data);

    let count = 0;
    for (let i = 0; i < keys.length; i++) {
        const key = keys[i];
        const entry = options.data[key];
        const item = {
            text: options.text ? entry[options.text] : key,
            value: options.value ? entry[options.value] : entry
        };

        if (removeDiacritics(item.text).toLowerCase().indexOf(removeDiacritics(lookup).toLowerCase()) >= 0 || item.text.trim()=='') {
            items.appendChild(this.createItem(lookup, item, options, field));
            if (options.maximumItems > 0 && ++count >= options.maximumItems)
                break;
        }
    }

    field.nextSibling.querySelectorAll('.dropdown-item').forEach((item) => {
        item.addEventListener('click', (e) => {
            let datatext = e.currentTarget.getAttribute('data-text');
            let dataValue = e.currentTarget.getAttribute('data-value');
            field.value = datatext;
            var curentselected = options.selected;
            if (dataValue) {
                // window[field.id + '_selected'] = { text: datatext, value: dataValue };
                options.selected = { text: datatext, value: dataValue };
            } else {
                options.selected = { text: null, value: null };
            }
            if (options.onSelectItem && curentselected != options.selected) {
                options.onSelectItem({
                    value: dataValue,
                    text: datatext
                });
            }
            window["dropdown" + field.id].hide();
            var btnclass = $("#btnclear" + field.id).attr("class");
            if (options.showclearbutton) {
                if (field.value) {
                    $("#btnclear" + field.id).attr("class", btnclass.replace(" d-none", ""));
                } else {
                    if (!btnclass.includes("d-none"))
                        $("#btnclear" + field.id).attr("class", btnclass + " d-none");
                }
            }
        });
    });
    return items.childNodes.length;
}
//function SetAutoCompleteValue(field, values) {
//    window[field.id + '_selected'] = { text: values.text, value: values.value };
//    field.value = values.text;
//    var options = window["options" + field.id];
//    if (options.showclearbutton) {
//        $("#btnclear" + field.id).attr("class", $("#btnclear" + field.id).attr("class").replace(" d-none", ""));
//    }
//    if (options.onSelectItem) {
//        options.onSelectItem({
//            value: values.value,
//            text: values.text
//        });
//    }
//}
HTMLElement.prototype.set_cpvalue = function (value,eventcall=true) {
    var field = this;
    var options = window["options" + field.id];
    var values = options.data.filter(x => x.value == value)[0];
    if (isEmpty(values)) {
        options.selected ={ text: null, value: null };
        field.value = "";
    } else {
        options.selected = { text: values.text, value: values.value };
        field.value = values.text;
    }
    if (options.showclearbutton) {
        $("#btnclear" + field.id).attr("class", $("#btnclear" + field.id).attr("class").replace(" d-none", ""));
    }
    if (options.onSelectItem && eventcall) {
        options.onSelectItem({
            value: options.selected.value,
            text: options.selected.text
        });
    }
}

function ce(html) {
  let div = document.createElement('div');
  div.innerHTML = html;
  return div.firstChild;
}

function insertAfter(elem, refElem) {
  return refElem.parentNode.insertBefore(elem, refElem.nextSibling);
}

function removeDiacritics(str) {
        return str
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '').replace(/[đĐ]/g, m => m === 'đ' ? 'd' : 'D');
}
function getfuction(eventcallback) {
    var functionName = eventcallback.name;
    var pr = "";
    if (eventcallback.pr.length > 0) {
        eventcallback.pr.forEach((item) => {
            if (pr == '')
                pr += item.toString();
            else
                pr += "," + item.toString();
        });
    }
    return functionName + "(" + pr + ")";
}
HTMLElement.prototype.get_cpselected = function () {
    var id = $(this).attr("id");
    var options = window["options" + id];
    return options.selected;
}
HTMLElement.prototype.get_cpvalue = function () {
    var id = $(this).attr("id");
    var options = window["options" + id];
    return options.selected.value;
}
HTMLElement.prototype.check_cpselected = function () {
    var id = $(this).attr("id");
    var options = window["options" + id];
    return options.selected.value != null;
    return false;
}