<%@ Control Language="C#" ClassName="NV.UI.Toast" AutoEventWireup="true" %>
<div class="position-fixed bottom-0 end-0 p-3" style="z-index: 1100">
    <div id="liveToast" class="toast" role="alert" aria-live="polite" aria-atomic="true" data-bs-delay="3000">
        <div class="toast-header">
            <strong class="me-auto" id="mstitle"></strong>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
        <div class="toast-body text-justify" id="mscontent">
        </div>
    </div>
</div>
<script>
    function ShowMessage(message, title = 'Thông báo') {
        var toastLive = document.getElementById('liveToast')
        let bsAlert = new bootstrap.Toast(toastLive);
        mstitle.innerHTML = title;
        mscontent.innerHTML = message;
        bsAlert.show();
    }
    UploadDefaultOftion = { multiple: false, showgrid: true, maxlength: 1024, MaDV: "Systems", Type: 0, SID: 0, files:[] };
    HTMLElement.prototype.filesInit = function (options, accept = "application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint,text/plain, application/pdf, image/*,zip,application/zip,application/x-zip,application/x-zip-compressed") {
        var control = $(this);
        control.html('');
        var id = control.attr("id");
        window["uploadoptions_" + id] = Object.assign({}, UploadDefaultOftion, options);
        var options = window["uploadoptions_" + id];
        window['lsttempfile_' + id] = options.files;
        var input = `<input type="file" class="form-control" ${options.multiple ? 'multiple="multiple"' : ''}  id="${id}_fileinput"  accept="${accept}" />`;
        control.append(input);
        if (options.multiple) {
            control.append(`<ol class="list-group list-group-flush" id='tblfiles_${id}'></ol>`);
            if (options.files.length > 0) {
                bindingExistsFile(controlid, options);
            }
        }
        $("#" + id + "_fileinput").change( function (e) {
            if (!e.target.files || !window.FileReader) {
                if (!options.multiple) {
                    options.files = [];
                }
                return;
            }
            var files = e.target.files;
            if (options.onChangeFile) {
                options.onChangeFile({
                });
            }
            var totalsize = 0;
            var totalfile = 0;
            for (var i = 0; i < files.length; i++){
                totalsize += Math.round(files[i].size / 1024);
                totalfile++;
            }
            if (options.files.length > 0) {
                options.files.forEach((item) => {
                    totalsize += Math.round(item.FileSize / 1024);
                    totalfile++;
                });
            }
            console.log(totalsize);
            if (totalsize > options.maxlength) {
                document.getElementById(id + "_fileinput").value = "";
                $.alert(`Tổng dung lượng ${totalfile} tệp ${totalsize} KB vượt tối đa cho phép ${options.maxlength} KB, vui lòng chọn lại tệp tin khác!`);
                return;
            }
            for (var i = 0; i < files.length; i++) {
                var f = files[i];
                upload(id, f, options);
               // formData.append('myFile', f);
            }
            if (options.onFileChanged) {
                options.onFileChanged({
                    files: options.files,
                    err: "",
                });
            }
            if (options.multiple) {
                document.getElementById(id + "_fileinput").value = "";
            }
        });
    }
    const toBase64 = file => new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
    function bindingExistsFile(controlid,  options) {
        for (var file in options.files) {
            file.UID = generateQuickGuid();
            appendFile(controlid, file.UID, file.ID);
            appendFileAttr(controlid, file.UID, file, options);
        }
        SortFileByOrder(`tblfiles_${controlid}`);
    }
    function appendFileAttr(controlid, elementId, data,options) {
        const fileRoot = document.getElementById(elementId);
        const order = fileRoot.querySelector(".file__order");
        const title = fileRoot.querySelector(".file__title");
        const size = fileRoot.querySelector(".file__size");
        const buttondelete = fileRoot.querySelector(".file__delete");
        title.setAttribute("href", data.FileURL);
        size.textContent = ` ${Math.round(data.FileSize / 1024)} KB`;
        order.setAttribute("value", data.ThuTu);
        order.setAttribute("data-id", data.ID);
        order.setAttribute("data-uid", elementId);
        buttondelete.setAttribute("data-id", data.ID);
        buttondelete.setAttribute("data-uid", elementId);
        fileRoot.setAttribute("data-index", data.ThuTu);
        buttondelete.addEventListener("click", function (e) {
            var elementId = $(this).attr("data-uid");
            var id = $(this).attr("data-id");
            var data = options.files.filter(x => x.UID == elementId)[0];
            if (id != 0) {
                
            } else {
                var api = host + `desktopmodules/qnmedia/api/files/DeleteTempFile`;
                PostAPI(api, data);
                //Xóa file tạm
            }
            if (options.onDeleted) {
                options.onDeleted({
                    data: data
                });
            }
            options.files = options.files.filter(x => x.UID != elementId);
            $("#" + elementId).remove();
               
        });
        order.addEventListener("change", function (e) {
            var index = options.files.findIndex(x => x.UID == elementId);
            options.files[index].ThuTu = e.target.value;
            fileRoot.setAttribute("data-index", e.target.value);
            SortFileByOrder(`tblfiles_${controlid}`);
            if (options.onSorted) {
                options.onSorted({
                    data: data
                });
            }
        });
    }
    function appendFile(controlid,fileId,dataid=0) {
        // Khởi tạo cấu trúc giao diện file
        const newFile = `
            <div class='input-group'>
            <button class='file__delete btn btn-danger btn-sm' data-id='${dataid}' type='button' ><i class="bi bi-trash me-1"></i></button>
            <input class='file__order form-control  form-control-sm mwpx-50 text-end' data-id='${dataid}'  type='number' min='0' max='100'>
            <div class='form-control'>
                <label class="file__size mt-1 fw-bold me-1 badge text-bg-success"></label>
                <a class="file__title mt-1 me-1" target='_blank'></a>
                <label class="file__percent mt-1 font-italic me-1"></label>
                <label class="file__progress mt-1 fw-bold font-italic"></label>
            </div>
        </div>`;
        // Bọc giao diện bởi 1 li với id = {fileId}
        const li = document.createElement("li");
        li.setAttribute("id", fileId);
        li.setAttribute("data-id", dataid);
        li.classList.add("list-group-item");
        li.innerHTML = newFile;
        // Bước cuối cùng, đẩy giao diện vào wrapper của nó  
        document.querySelector(`#tblfiles_${controlid}`).append(li);
    }
    function fileProgressing(elementId, fileInfo, progressing) {
        const fileRoot = document.getElementById(elementId);
        const progressFile = fileRoot.querySelector(".file__progress");
        const percent = fileRoot.querySelector(".file__percent");
        const title = fileRoot.querySelector(".file__title");
        // Cập nhật các thông tin về title, percent và progress của tệp
        progressFile.style.width = progressing + "%";
        percent.textContent = "(Đã tải lên "+ progressing + "%)";
        title.textContent = fileInfo.name ?? "khong-ro";
    }
    function upload(controlid,newFile,options) {
        // 1. Khởi tạo XHR để call API
        const xhr = new XMLHttpRequest();
        // 2. Thêm dữ liệu
        const data = new FormData();
        data.append("file", newFile);
        // 3. Tạo id cho file và thêm nó vào giao diện
        const elementId = generateQuickGuid();
        // 4. method upload tệp
        xhr.open("POST", host + `desktopmodules/qnmedia/api/files/UpdateFileTemp`, true);
        if (options.multiple) {
            appendFile(controlid,elementId);
        // 5. Lắng nghe sự kiện progress lấy tỉ lệ %
            xhr.upload.addEventListener("progress", function (event) {
                const percent = Math.round((event.loaded / event.total) * 100);
                fileProgressing(elementId, newFile, percent);
            });
        }
        xhr.onreadystatechange = () => {
            if (xhr.readyState === 4) {
                var response = JSON.parse(xhr.response);
                if (response.status) {
                    if (response.result.length > 0) {
                        var data = response.result[0];
                        data.SID = options.SID;
                        data.MaDV = options.MaDV;
                        data.ID = 0;
                        data.Type = options.Type;
                        data.UID = elementId;
                        if (options.multiple) {
                            data.ThuTu = (options.files.length > 0 ? (options.files.sort(
                                function (a, b) {
                                    return parseFloat(b['ThuTu']) - parseFloat(a['ThuTu']);
                                }
                            )[0]['ThuTu']) : 0) + 1;
                            appendFileAttr(controlid, elementId, data, options);
                            SortFileByOrder(`tblfiles_${controlid}`);
                            options.files.push(data);
                        } else {
                            data.ThuTu = 1;
                            options.files = [];
                            options.files.push(data);
                        }
                    }
                }
            }
        }
        // 6. Gửi yêu cầu
        xhr.send(data);
    }
    function SortFileByOrder(control, keyname ='data-index') {
        const fileRoot = document.getElementById(control);
        var subjects = fileRoot.querySelectorAll(`[${keyname}]`);
        var subjectsArray = Array.from(subjects);
        let sorted = subjectsArray.sort((a, b) => a.dataset.index - b.dataset.index);
        sorted.forEach(e => {
            document.getElementById(control).appendChild(e);
        });
    }
    function generateQuickGuid() {
        return Math.random().toString(36).substring(2, 15) +
            Math.random().toString(36).substring(2, 15);
    }
    HTMLElement.prototype.get_files = function () {
        var id = $(this).attr("id");
        if (window["uploadoptions_" + id]) return window["uploadoptions_" + id].files;
        return [];
    }
    HTMLElement.prototype.set_files = function (files) {
        var id = this.getAttribute("id");
        var options = window["uploadoptions_" + id];
        for (var file in files) {
            file.UID = generateQuickGuid();
            appendFile(controlid, file.UID, file.ID);
            appendFileAttr(controlid, file.UID, file, options);
        }
        SortFileByOrder(`tblfiles_${controlid}`);
    }
</script>
