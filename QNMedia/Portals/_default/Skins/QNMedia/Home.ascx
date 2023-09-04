<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<!-- Favicon -->
<script src='/Resources/Shared/scripts/jquery/jquery.js' ></script>

<link href="img/favicon.ico" rel="icon">
<!-- Google Web Fonts -->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700;800&family=Rubik:wght@400;500;600;700&display=swap" rel="stylesheet">

<!-- Icon Font Stylesheet -->
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css" rel="stylesheet">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css" rel="stylesheet">

<!-- Libraries Stylesheet -->
<link href="../../Portals/_default/Skins/QNMedia/lib/owlcarousel/assets/owl.carousel.min.css" rel="stylesheet">
<link href="../../Portals/_default/Skins/QNMedia/lib/animate/animate.min.css" rel="stylesheet">

<!-- Customized Bootstrap Stylesheet -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">

<!-- Template Stylesheet -->
<link href="../../Portals/_default/Skins/QNMedia/css/style.css" rel="stylesheet">
<!-- Spinner Start -->
<div id="spinner" class="show bg-white position-fixed translate-middle w-100 vh-100 top-50 start-50 d-flex align-items-center justify-content-center">
    <div class="spinner"></div>
</div>
<!-- Spinner End -->


<!-- Topbar Start -->
<div class="container-fluid bg-dark px-5 d-none d-lg-block">
    <div class="row gx-0">
        <div class="col-lg-8 text-center text-lg-start mb-2 mb-lg-0">
            <div class="d-inline-flex align-items-center" style="height: 45px;">
                <small class="me-3 text-light"><i class="fa fa-map-marker-alt me-2"></i>74 Hùng Vương - TP Tam Kỳ</small>
                <small class="me-3 text-light"><i class="fa fa-phone-alt me-2"></i><a href="tel:+84964567545" class="text-white text-decoration-none">0964 567 545</a></small>
                <small class="text-light"><i class="fa fa-envelope-open me-2"></i><a href="mailto:qnammedia@gmail.com" class="text-white text-decoration-none">qnammedia@gmail.com</a></small>
            </div>
        </div>
        <div class="col-lg-4 text-center text-lg-end">
            <div class="d-inline-flex align-items-center" style="height: 45px;">
                <a class="btn btn-sm btn-outline-light btn-sm-square rounded-circle me-2" href=""><i class="fab fa-twitter fw-normal"></i></a>
                <a class="btn btn-sm btn-outline-light btn-sm-square rounded-circle me-2" href=""><i class="fab fa-facebook-f fw-normal"></i></a>
                <a class="btn btn-sm btn-outline-light btn-sm-square rounded-circle me-2" href=""><i class="fab fa-linkedin-in fw-normal"></i></a>
                <a class="btn btn-sm btn-outline-light btn-sm-square rounded-circle me-2" href=""><i class="fab fa-instagram fw-normal"></i></a>
                <a class="btn btn-sm btn-outline-light btn-sm-square rounded-circle" href=""><i class="fab fa-youtube fw-normal"></i></a>
            </div>
        </div>
    </div>
</div>
<!-- Topbar End -->


<!-- Navbar Start -->
<div class="container-fluid position-relative p-0">
    <nav class="navbar navbar-expand-lg navbar-dark px-5 py-3 py-lg-0">
        <a href="/" class="navbar-brand p-0">
            <h1 class="m-0"><i class="fa fa-home me-3"></i>QN Media</h1>
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarCollapse">
            <span class="fa fa-bars"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarCollapse">
            <div class="navbar-nav ms-auto py-0">
                <a href="/" class="nav-item nav-link active">Trang chủ</a>
                <a href="/dichvu" class="nav-item nav-link ">Dịch vụ</a>
                <a href="about.html" class="nav-item nav-link">Liên hệ</a>
                <a href="contact.html" class="nav-item nav-link">Thông tin</a>
            </div>
            <button type="button" class="btn text-primary ms-3" data-bs-toggle="modal" data-bs-target="#searchModal"><i class="fa fa-search"></i></button>
            <a href="/" class="btn btn-primary py-2 px-4 ms-3">Đăng ký làm việc</a>
        </div>
    </nav>

    <div class="container-fluid bg-primary py-5 bg-header">
        <%--  <div class="row py-5">
          <div class="col-12 pt-lg-5 mt-lg-5 text-center">
            </div>
        </div>--%>
    </div>
</div>
<!-- Navbar End -->


<!-- Full Screen Search Start -->
<div class="modal fade" id="searchModal" tabindex="-1">
    <div class="modal-dialog modal-fullscreen">
        <div class="modal-content" style="background: rgba(9, 30, 62, .7);">
            <div class="modal-header border-0">
                <button type="button" class="btn bg-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body d-flex align-items-center justify-content-center">
                <div class="input-group" style="max-width: 600px;">
                    <input type="text" class="form-control bg-transparent border-primary text-white p-3" placeholder="Nhập nội dung tìm kiếm">
                    <button class="btn btn-primary px-4"><i class="bi bi-search"></i></button>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- Full Screen Search End -->
<div runat="server" id="ContentPane" />


<!-- Service Start -->

<!-- Service End -->




<!-- Footer Start -->
<div class="container-fluid bg-dark text-light mt-5 wow fadeInUp" data-wow-delay="0.1s">
    <div class="container">
        <div class="row gx-5">
            <div class="col-lg-4 col-md-6 footer-about">
                <div class="d-flex flex-column align-items-center justify-content-center text-center h-100 bg-primary p-4">
                    <a href="index.html" class="navbar-brand">
                        <h1 class="m-0 text-white"><i class="fa fa-user-tie me-2"></i>QN Media</h1>
                    </a>
                    <p class="mt-3 mb-4">Đơn vị tiên phong trong quá trình số hóa cho tổ chức, cá nhân tại Quảng Nam</p>
                    <div class="input-group">
                        <input type="text" class="form-control border-white p-3" placeholder="Email của bạn">
                        <button class="btn btn-dark">Nhận bảng tin</button>
                    </div>
                </div>
            </div>
            <div class="col-lg-8 col-md-6">
                <div class="row gx-5">
                    <div class="col-lg-6 col-md-12 pt-5 mb-5">
                        <div class="section-title section-title-sm position-relative pb-3 mb-4">
                            <h3 class="text-light mb-0">Thông tin liên hệ</h3>
                        </div>
                        <div class="d-flex mb-2">
                            <i class="bi bi-geo-alt text-primary me-2"></i>
                            <p class="mb-0">74 Hùng Vương - TP Tam Kỳ- Quảng Nam</p>
                        </div>
                        <div class="d-flex mb-2">
                            <i class="bi bi-envelope-open text-primary me-2"></i>
                            <p class="mb-0"><a href="mailto:qnammedia@gmail.com" class="text-white text-decoration-none">qnammedia@gmail.com</a></p>
                        </div>
                        <div class="d-flex mb-2">
                            <i class="bi bi-telephone text-primary me-2"></i>
                            <p class="mb-0"><a href="tel:+84964567545" class="text-white text-decoration-none">0964 567 545</a></p>
                        </div>
                        <div class="d-flex mt-4">
                            <a class="btn btn-primary btn-square me-2" href="#"><i class="fab fa-twitter fw-normal"></i></a>
                            <a class="btn btn-primary btn-square me-2" href="#"><i class="fab fa-facebook-f fw-normal"></i></a>
                            <a class="btn btn-primary btn-square me-2" href="#"><i class="fab fa-linkedin-in fw-normal"></i></a>
                            <a class="btn btn-primary btn-square" href="#"><i class="fab fa-instagram fw-normal"></i></a>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-12 pt-0 pt-lg-5 mb-5">
                        <div class="section-title section-title-sm position-relative pb-3 mb-4">
                            <h3 class="text-light mb-0">Danh mục menu</h3>
                        </div>
                        <div class="link-animated d-flex flex-column justify-content-start">
                            <a class="text-light mb-2" href="#"><i class="bi bi-arrow-right text-primary me-2"></i>Trang chủ</a>
                            <a class="text-light mb-2" href="#"><i class="bi bi-arrow-right text-primary me-2"></i>Dịch vụ</a>
                            <a class="text-light mb-2" href="#"><i class="bi bi-arrow-right text-primary me-2"></i>Liên hệ</a>
                            <a class="text-light mb-2" href="#"><i class="bi bi-arrow-right text-primary me-2"></i>Thông tin</a>
                        </div>
                        <div id="fb-root"></div>
                        <script>(function (d, s, id) {
                                var js, fjs = d.getElementsByTagName(s)[0];
                                if (d.getElementById(id)) return;
                                js = d.createElement(s); js.id = id;
                                js.src = "https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.0";
                                fjs.parentNode.insertBefore(js, fjs);
                            }(document, 'script', 'facebook-jssdk'));</script>

                        <!-- Your share button code -->
                        <div class="fb-share-button"
                            data-href="https://quangnammedia.vn"
                            data-layout="button_count">
                        </div>
                        <script src="https://sp.zalo.me/plugins/sdk.js"></script>
                        <div class="zalo-share-button" data-href="" data-oaid="666275801224680709" data-layout="1" data-color="blue" data-customize="false"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container-fluid text-white" style="background: #061429;">
    <div class="container text-center">
        <div class="row justify-content-end">
            <div class="col-lg-8 col-md-6">
                <div class="d-flex align-items-center justify-content-center" style="height: 75px;">
                    <p class="mb-0  fw-bold">
                        &copy; <a class="text-white text-decoration-none" href="#">QUẢNG NAM MEDIA LTD</a>
                    . All Rights Reserved. 
                   
                </div>
            </div>
        </div>
    </div>
</div>
<!-- Footer End -->

<!-- Back to Top -->
<a href="#" class="btn btn-lg btn-primary btn-lg-square rounded back-to-top"><i class="bi bi-arrow-up"></i></a>


<!-- JavaScript Libraries -->

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous"></script>
<script src="../../Portals/_default/Skins/QNMedia/lib/wow/wow.min.js"></script>
<script src="../../Portals/_default/Skins/QNMedia/lib/easing/easing.min.js"></script>
<script src="../../Portals/_default/Skins/QNMedia/lib/waypoints/waypoints.min.js"></script>
<script src="../../Portals/_default/Skins/QNMedia/lib/counterup/counterup.min.js"></script>
<script src="../../Portals/_default/Skins/QNMedia/lib/owlcarousel/owl.carousel.min.js"></script>
<script src="../../Portals/_default/Skins/QNMedia/js/main.js"></script>

    <style>
        .dnnModuleDialog {
            top:200px !important;
        }
    </style>
<script type="text/javascript" id="scriptcounter">
    if (sessionStorage.getItem("new_access") == null) {
        sessionStorage.setItem("new_access", true);
        $.getJSON('https://api.ipify.org?format=jsonp&callback=?', function (data) {
            console.log(data.ip);
        });
    }
    scriptcounter.remove();
</script>
