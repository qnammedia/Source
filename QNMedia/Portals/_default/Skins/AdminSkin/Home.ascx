<%@ Control Language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="nv" TagName="menu" Src="~/Portals/_default/Skins/AdminSkin/Controls/Menu.ascx" %>
<%@ Register TagPrefix="nv" TagName="loadding" Src="~/Portals/_default/Skins/AdminSkin/Controls/Loadding.ascx" %>
<%@ Register TagPrefix="nv" TagName="toast" Src="~/Portals/_default/Skins/AdminSkin/Controls/Toast.ascx" %>
<%@ Register TagPrefix="nv" TagName="tablejs" Src="~/Portals/_default/Skins/AdminSkin/JS/Table.ascx" %>
<%@ Register TagPrefix="nv" TagName="tableserverjs" Src="~/Portals/_default/Skins/AdminSkin/JS/TablePaging.ascx" %>
<%@ Register Src="~/DesktopModules/Controls/Home/Message.ascx" TagPrefix="nv" TagName="Message" %>
<%@ Register Src="~/DesktopModules/Controls/Home/Noti.ascx" TagPrefix="nv" TagName="Noti" %>
<%@ Register Src="~/DesktopModules/Controls/Home/AccountInfo.ascx" TagPrefix="nv" TagName="AccountInfo" %>
<%@ Register Src="~/DesktopModules/Controls/Home/Home.ascx" TagPrefix="nv" TagName="HomeAdmin" %>
<link href="<%=SkinPath%>assets/img/favicon.png" rel="icon">
<link href="<%=SkinPath%>assets/img/apple-touch-icon.png" rel="apple-touch-icon">
<nv:loadding ID="loadding" runat="server" />
<!-- Google Fonts -->
<link href="https://fonts.gstatic.com" rel="preconnect">
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap" rel="stylesheet">
<%--<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">--%>
<!-- Vendor CSS Files -->
<script src='/Resources/Shared/scripts/jquery/jquery.js' ></script>
<link href="<%=SkinPath%>assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/quill/quill.snow.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/quill/quill.bubble.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/remixicon/remixicon.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/vendor/simple-datatables/style.css" rel="stylesheet">
<link href="<%=SkinPath%>assets/css/style.css" rel="stylesheet">
<link href="<%=SkinPath%>css/comboTreePlugin_style.css" rel="stylesheet">
<link href="<%=SkinPath%>index.css" rel="stylesheet">
<link href="<%=SkinPath%>CSS/jquery-confirm.min.css" rel="stylesheet">
<script src="<%=SkinPath%>/js/systems.js"></script>

<header id="header" class="header fixed-top d-flex align-items-center ">
    <div class="d-flex align-items-center justify-content-between  ">
        <span class="logo d-flex align-items-center">
            <%--<img src="<%=SkinPath%>assets/img/logo.png" alt="">--%>
            <span class="d-none d-lg-block">CMS ADMIN SITES </span>
        </span>
        <i class="bi bi-list toggle-sidebar-btn"></i>
    </div>
    <!-- End Logo -->

    <div class="search-bar">
        <div class="search-form d-flex align-items-center">
            <input type="text" name="query" placeholder="Nhập từ khóa tìm kiếm" title="Tìm kiếm">
            <button type="button" title="Search"><i class="bi bi-search"></i></button>
        </div>
    </div>
    <!-- End Search Bar -->

    <nav class="header-nav ms-auto ">
        <ul class="d-flex align-items-center">

            <nv:Noti ID="Noti" runat="server" />
            <!-- End Notification Nav -->
            <nv:Message ID="Message" runat="server" />
            <!-- End Messages Nav -->
            <nv:AccountInfo ID="AccountInfo" runat="server" />
            <!-- End Profile Nav -->
        </ul>
    </nav>
    <!-- End Icons Navigation -->
</header>
<!-- End Header -->

<!-- ======= Sidebar ======= -->
<aside id="sidebar" class="sidebar">
    <nv:menu runat="server" ID="menu" />
</aside>
<!-- End Sidebar-->

<main id="main" class="main">
    <div class="pagetitle">
        <h1 id="rootnav"></h1>
        <%--     <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a href="index.html">Home</a></li>
          <li class="breadcrumb-item">Icons</li>
          <li class="breadcrumb-item active">Bootstrap</li>
        </ol>
      </nav>--%>
    </div>
    <div id="ContentPane" runat="server">
        <nv:HomeAdmin ID="homeadmin" runat="server" />
    </div>
</main>
<footer id="footer" class="footer">
    <div class="copyright">
        &copy; Copyright <strong><span>Quảng Nam Media -
            <script>document.write(new Date().getFullYear())</script>
        </span></strong>. All Rights Reserved
    </div>
    <div class="credits">
        Designed by <a href="https://quangnammedia.vn/">MrNguyenVuIt</a>
    </div>
</footer>
<!-- End Footer -->

<a href="#" class="back-to-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>
<nv:tablejs ID="tablejs" runat="server" />
<nv:tableserverjs ID="tableserverjs" runat="server" />
<!-- Vendor JS Files -->

<script src="<%=SkinPath%>assets/vendor/apexcharts/apexcharts.min.js"></script>
<script src="<%=SkinPath%>assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
<script src="<%=SkinPath%>assets/vendor/chart.js/chart.min.js"></script>
<script src="<%=SkinPath%>assets/vendor/echarts/echarts.min.js"></script>
<script src="<%=SkinPath%>assets/vendor/quill/quill.min.js"></script>
<script src="<%=SkinPath%>assets/vendor/simple-datatables/simple-datatables.js"></script>
<script src="<%=SkinPath%>assets/vendor/tinymce/tinymce.min.js"></script>
<%--<script src="<%=SkinPath%>assets/vendor/php-email-form/validate.js"></script>--%>
<!-- Template Main JS File -->
<script src="<%=SkinPath%>/js/notify.js"></script>
<script src="<%=SkinPath%>/js/comboTreePlugin.js"></script>
<script src="<%=SkinPath%>/js/autocomplete.js"></script>
<script src="<%=SkinPath%>/js/jquery.bootpag.min.js"></script>
<script src="<%=SkinPath%>assets/js/main.js"></script>
<script src="<%=SkinPath%>/js/jquery-confirm.min.js"></script>
<nv:toast ID="toast" runat="server" />
<style id="adminstyle">

</style>
<script>
    var xclick = false;
    $(document).ready(function () {
        if (issupper || isadmin) {
            adminstyle.innerHTML = `.sidebar{
                    left:90px !important;
                    }
.header{
left:90px;
}
`;
            $(".toggle-sidebar-btn").click(function () {
                if (!xclick) {
                    adminstyle.innerHTML = `.sidebar{
                    left:-300px !important;
                    }.header{
left:90px;
}`;
                    xclick = true;
                } else {
                    adminstyle.innerHTML = `.sidebar{
                    left:90px !important;
                    }.header{
left:90px;
}`;
                    xclick = false;
                }
            });
        }
    });
</script>

