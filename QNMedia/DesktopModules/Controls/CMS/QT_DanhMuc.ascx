<%@ Control Language="C#" AutoEventWireup="true" ClassName="NV.Admin.QT_DanhMuc" %>

<link href="/Portals/_default/Skins/AdminSkin/assets/bstable/jquery.treegrid.css" rel="stylesheet">
<link href="/Portals/_default/Skins/AdminSkin/assets/bstable/bootstrap-table.min.css" rel="stylesheet">
<script src="/Portals/_default/Skins/AdminSkin/assets/bstable/jquery.treegrid.min.js"></script>
<script src="/Portals/_default/Skins/AdminSkin/assets/bstable/bootstrap-table.min.js"></script>
<script src="/Portals/_default/Skins/AdminSkin/assets/bstable/bootstrap-table-treegrid.min.js"></script>
Tài liệu tham khảo tại
<a href="https://examples.bootstrap-table.com/#methods/refresh.html">bootstrap table</a>
<table id="qldm_tbldanhmuc" data-toggle="table" data-search="true" >
</table>
<script>
    var $tableqtdm = $('#qldm_tbldanhmuc');
    var api = '/DesktopModules/Services/API/CMSAPI/Sys_Categorys_Get?type=tintuc';
    var data = GetAPI(api).result;
    var sys_mdupdate_selectdm;
    QTDM_Init();
    function QTDM_Init() {
        QTDM_Binding(data);
    }
    function QTDM_Binding(data) {
        $tableqtdm.bootstrapTable({
            data: data,
            idField: 'ID',
            showColumns: true,
            columns: [
                {
                    field: 'ck',
                    checkbox: true
                },
                {
                    field: 'Title',
                    sortable: true,
                    title: 'Tên danh mục'
                },
                {
                    field: 'IsPublic',
                    title: 'Sử dụng',
                    sortable: true,
                    align: 'center',
                    formatter: 'qtdm_statusFormatter'
                },
                {
                    title: 'Thao tác',
                    formatter: 'qtdm_actionFormatter',
                    align: 'center',
                }
            ],
            treeShowField: 'Title',
            parentIdField: 'ParentID',
            onPostBody: function () {
                var columns = $tableqtdm.bootstrapTable('getOptions').columns;
                if (columns && columns[0][1].visible) {
                    $tableqtdm.treegrid({
                        treeColumn: 1,
                        onChange: function () {
                            $tableqtdm.bootstrapTable('resetView')
                        }
                    })
                }
            }
        });
        table_eventbinding();
        function table_eventbinding() {
            $("#qldm_tbldanhmuc .btn-delete").on("click", function () {
                var id = $(this).attr("data-id");
                $.confirm({
                    title: 'Xác nhận xóa',
                    content: 'Bạn có chắn chắn muốn xóa mục này?',
                    buttons: {
                        cancel: {
                            text: 'Hủy',
                            btnClass: 'btn-red',
                            action: function () {
                            }
                        },
                        confirm: {
                            text: 'Xác nhận',
                            btnClass: 'btn-blue',
                            action: function () {
                                data = data.filter(x => x.ID != id);
                                $tableqtdm.bootstrapTable('load', data);
                                table_eventbinding();
                            }
                        },
                    }
                });
            });
            $("#qldm_tbldanhmuc .btn-edit").on("click", function () {
                var data_options = data.to_treeoption("ParentID", "Title", "ID");
                sys_mdupdate_selectroot.innerHTML = "";
                sys_mdupdate_selectdm = $('#sys_mdupdate_selectroot').comboTree({
                    source: data_options,
                    isMultiple: true,
                    //selectableLastNode: true,
                    withSelectAll: true,
                    cascadeSelect: true,
                });
                //sys_mdupdate_selectdm.onChange(function () {
                //    console.log(this.getSelectedIds());
                //});
                //sys_mdupdate_selectdm.getSelectedNames();
                //sys_mdupdate_selectdm.getSelectedIds();
                //sys_mdupdate_selectdm.selected: ['0']

                //sys_mdupdate_selectdm.setSource(source);
                // clear selection
                //sys_mdupdate_selectdm.clearSelection();
                // select all items.
                //sys_mdupdate_selectdm.selectAll();
                // set selection
                //sys_mdupdate_selectdm.setSelection(selectionIdList):
                //onChange(callBackFunction)

            });

        }
    }
    function qtdm_sttFormatter(value, row, index) {
        return `<input class="form-check-input" type="checkbox" value="" ${value ? "checked" : ""} data-id="${row["ID"]}" data-pid="${row["ParentID"]}">`;
    }
    function qtdm_statusFormatter(value, row, index) {
        return `<input class="form-check-input" type="checkbox" value="" ${value ? "checked" : ""} data-id="${row["ID"]}" data-pid="${row["ParentID"]}">`;
    }
    function qtdm_actionFormatter(value, row, index) {
        var action = "";
        if (row["CountChild"] == 0)
            action += `<button type='button' class='btn btn-outline-danger btn-sm btn-delete me-1' data-id="${row["ID"]}" ><i class="bi bi-trash" ></i></button>`;
        action += `<button type='button' class='btn btn-outline-secondary btn-sm btn-edit  me-1' data-bs-toggle="modal" data-bs-target="#sys_mdupdate_Categorys" data-id="${row["ID"]}"><i class="bi bi-pencil-fill"></i></button>`;
        if (row["Level"] <= 3)
            action += `<button type='button' class='btn btn-outline-primary btn-sm btn-add' data-bs-toggle="modal" data-bs-target="#sys_mdupdate_Categorys" data-id="${row["ID"]}"><i class="bi bi-plus"></i></button>`;
        return action;
    }
  
</script>
<style>
    .treegrid-expander {
        top: 3px;
        margin-right: 3px;
    }
</style>
<!-- Modal -->
<div class="modal fade" id="sys_mdupdate_Categorys" tabindex="-1" aria-labelledby="sys_mdupdate_CategorysLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h1 class="modal-title fs-5" id="sys_mdupdate_CategorysLabel">CẬP NHẬT DANH MỤC</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="row g-1">
                    <div class="col-md-6">Danh mục cha</div>
                    <div class="col-md-6 ">
                        <input type="text" id="sys_mdupdate_selectroot" class="form-select form-select-sm" placeholder="Chọn cấp cha" autocomplete="off"/>
                    </div>
                    <div class="col-md-6 ">Tên danh mục</div>
                    <div class="col-md-6 ">
                        <input type="text" class="form-control form-control-sm" id="qtdm_txttendm" value="" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary">Save changes</button>
            </div>
        </div>
    </div>
</div>
