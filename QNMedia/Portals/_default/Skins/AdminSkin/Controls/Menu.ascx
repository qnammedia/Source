<%@ Control Language="C#" AutoEventWireup="true" ClassName="NV.UI.Menu" %>

<ul class="sidebar-nav menuitem" id="sidebar-nav">
    <li class="nav-item">
        <a class="nav-link  menu-action" root="Trang chủ" href="/">
            <i class="bi bi-house-fill"></i>
            <span>Trang chủ</span>
        </a>
    </li>
</ul>
<script type="text/javascript">
    $(document).ready(function () {
        MenuItemInit();
        loadstate();

    });
    $(window).on('popstate', function (event) {
        loadstate();
    });
    function MenuItemInit() {
        CreateMenuItem();
        $(".menuitem a").on("click", function () {
            var root = $(this).attr("root");
            var title = $(this).attr("title");
            var href = $(this).attr("href");
            if (href != "#") {
                let newUrlIS = "";
                if (root) {
                    if (title) {
                        newUrlIS = window.location.origin +'/<%= TabController.CurrentPage.TabName %>'+ '?component=' + href ;
                    }
                    else {
                        newUrlIS = window.location.origin + '/<%= TabController.CurrentPage.TabName %>?';
                    }
                }
                if (window["pr"]) {
                    newUrlIS += "&" + window["pr"];
                }
                window.history.pushState({}, null, newUrlIS);
                actioncomponent($(this));
                window["pr"] = null;
            }
            return false;
        });
    }
    function CreateMenuItem() {
        var curentsite = '<%= TabController.CurrentPage.TabName %>';
        var api = "/DesktopModules/Services/API/CMSAPI/Policy_Get";
        var rs = GetAPI(api);
        if (rs.result) {
            var data = rs.result;
            var root = data.filter(x => x.Parent == null || x.Parent == "");
            root.forEach(function (item) {
                var curentmenu = document.getElementById("sidebar-nav").innerHTML;
                var rootmenu = `<li class="nav-item" id="mn${item.ID}">
                    <a class="nav-link collapsed" data-bs-target="#gr-${item.ID}" id="link-gr-${item.ID}" data-bs-toggle="collapse" href="#">
                        <i class="${item.IconClass}"></i><span>${item.Name}</span><i class="bi bi-chevron-down ms-auto"></i>
                    </a>
                    <ul id="gr-${item.ID}" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                    </ul></li>`;
                document.getElementById("sidebar-nav").innerHTML = curentmenu + rootmenu;
                var child = data.filter(x => x.Parent == item.ID);
                child.forEach(function (childitem) {
                    document.getElementById(`gr-${item.ID}`).addMenuItem(childitem.Href, item.Name, childitem.Name, childitem.Class);
                });
                child.forEach(function (childitem) {
                    if (childitem.InitFucntion) {
                        document.getElementById(`gr-${item.ID + childitem.Href}`).addEventListener("click", function (e) {
                            window[childitem.InitFucntion]();
                        });
                    }
                });
            });
        }
    }
    function loadstate() {
        var url_string = window.location.href;
        var url = new URL(url_string);
        var component = url.searchParams.get("component");
        var find = 0;
        $(".menuitem a").each(function (item) {
            if ($(this).attr("href") == component || $(this).attr("href") == "/") {
                actioncomponent($(this));
                var target = $(this).attr("target");
                if (target && $("#link-" + target).attr("class").includes("collapsed")) {
                    document.getElementById("link-" + target).click();
                }
                find++;
                return;
            }
        });
        if (find == 0) {
            window.history.pushState({}, null, window.location.origin);
            $(".menuitem a[href='/']").trigger("click");
        }
    }
    function actioncomponent(obj) {
        $(".menuitem a").each(function (item) {
            $(this).attr("class", "nav-link menu-action collapsed");
        });
        $(obj).attr("class", "nav-link menu-action");
        var root = $(obj).attr("root");
        var href = $(obj).attr("href");
        if (href != "#") {
            if (href == '/') {
                $("#selectionhome").removeAttr("style");
                sectioncontent.style.display = 'none';
            } else {
                selectionhome.style.display = 'none';
                $("#sectioncontent").removeAttr("style");
                $(".components").attr("class", "components d-none");
                var components = href.split('_');
                if (components.length > 0) {
                    var component = components[components.length - 1];
                    $(".components[data=" + component + "]").attr("class", "components");
                }
            }
            //switch (href) {
            //    case "quantri_danhmucphongban":
            //        QTHT_PhongBan_Binding();
            //        break;
            //    case "quantri_danhmucchucvu":
            //        QTHT_ChucVu_Binding();
            //        break;
            //    case "quantri_danhmucnguoidung":
            //        QTHT_NguoiDung_Binding();
            //        break;
            //    case "tacnghiepvanthu/cauhinhvanthu":
            //        TNVT_CauHinhInit();
            //        break;
            //    case "vanbanden_vaosovbden":
            //        TNVT_VaoSoDen_Init();
            //        break;
            //    case "vanbanden_lapinvanbanden":
            //        TNVT_LapSoVanBanDen_Init();
            //        break;
            //}
            var title = $(obj).attr("title");
            if (root) {
                var icon = '<i class="bi bi-backspace-reverse me-1"></i>';
                if (title) {
                    rootnav.innerHTML = icon + root + " / " + title;
                }
                else {
                    rootnav.innerText = root;
                }
            } else {
                rootnav.innerText = icon + "Trang chủ";
            }
        }
    }
</script>
