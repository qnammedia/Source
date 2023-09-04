<%@ Control Language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/DesktopModules/QNMedia_Login/Login.ascx" TagPrefix="nv" TagName="Login" %>

<link href="https://fonts.gstatic.com" rel="preconnect">
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;700&display=swap" rel="stylesheet">
 <%--<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">--%>

<!-- Vendor CSS Files -->
<link href="..<%=SkinPath%>assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/quill/quill.snow.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/quill/quill.bubble.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/remixicon/remixicon.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/vendor/simple-datatables/style.css" rel="stylesheet">
<link href="..<%=SkinPath%>assets/css/style.css" rel="stylesheet">
<link href="..<%=SkinPath%>index.css" rel="stylesheet">
<script src="..<%=SkinPath%>/js/systems.js"></script>

   <div class="container">
      <section class="section register min-vh-100 d-flex flex-column align-items-center justify-content-center py-4">
        <div class="container">
          <div class="row justify-content-center">
            <div class="col-lg-4 col-md-6 d-flex flex-column align-items-center justify-content-center">
              <div class="d-flex justify-content-center py-4">
                <div  class="logo d-flex align-items-center w-auto">
                  <img src="..<%=SkinPath%>assets/img/logo.png" alt="">
                  <span class="d-lg-block">QNMEDIA OFFICE</span>
                </div>
              </div><!-- End Logo -->
              <div class="card mb-3">
                <div class="card-body">
                    <h3 class="pt-3 text-center pb-0">Đăng nhập hệ thống</h3>
                    <hr />
                   <nv:Login ID="Login" runat="server" />
                </div>
              </div>
              <div class="credits">
                Designed by <a href="https://quangnammedia.vn" target="_blank">MrNguyenVuIt</a>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  <div id="ContentPane" runat="server" class="d-none"></div>

<script src="..<%=SkinPath%>assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
<script src="..<%=SkinPath%>assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
