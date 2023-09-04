<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountInfo.ascx.cs" Inherits="QNMedia.Controls.TrangChu.AccountInfo" %>
<li class="nav-item dropdown pe-3 ">
    <a class="nav-link nav-profile d-flex align-items-center pe-0" href="#" data-bs-toggle="dropdown">
        <img src="..<%=SkinPath%>assets/img/profile-img.jpg" alt="Profile" class="rounded-circle">
        <span class="d-none d-md-block dropdown-toggle ps-2" id="loginname"></span>
    </a>
    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile ">
        <li class="dropdown-header">
            <select class='form-select' id="og_user">
                <option value="value">Giám đốc bệnh viện</option>
            </select>
            <span id="lb_cvuser"></span>
        </li>
        <li >
            <hr class="dropdown-divider">
        </li>
        <li class="menuitem ms-1 p-2">
            <a class="dropdown-item d-flex align-items-center" root="Tài khoản" title="Thông tin tài khoản" href="/account/profile">
                <i class="bi bi-person"></i>
                <span>Thông tin tài khoản</span>
            </a>
        </li>
        <li>
            <hr class="dropdown-divider">
        </li>

        <li class="menuitem ms-1 p-2">
            <a class="dropdown-item d-flex align-items-center" root="Tài khoản" title="Cài đặt tài khoản" href="/account/settings">
                <i class="bi bi-gear"></i>
                <span>Cài đặt tài khoản</span>
            </a>
        </li>
        <li>
            <hr class="dropdown-divider">
        </li>

        <li  class="menuitem ms-1 p-2">
                  <a class="dropdown-item d-flex align-items-center" root="Tài khoản" title="Hỗ trợ" href="/account/faq">
                <i class="bi bi-question-circle"></i>
                <span>Hỗ trợ</span>
            </a>
        </li>
        <li>
            <hr class="dropdown-divider">
        </li>
        <li>
            <a class="dropdown-item d-flex align-items-center" href="/logoff.aspx?">
                <i class="bi bi-box-arrow-right"></i>
                <span>Đăng xuất</span>
            </a>
        </li>
    </ul>
</li>
<script>
    $(document).ready(function () {
        loginname.innerHTML = displayname;
        og_user.set_option(lstpolicy, "ID", "TenChucVu");
        og_user.addEventListener("change", function () {
            window.location.replace("/Default.aspx?p="+this.value);
        });
        if (!isEmpty(policy) || policy) {
            og_user.value = policy.ID;
            if (lstpolicy.length > 1) {
                og_user.style.display = "";
                lb_cvuser.style.display = "none";
            } else {
                og_user.style.display = "none";
                lb_cvuser.style.display = "";
                lb_cvuser.innerHTML = policy.TenChucVu;
            }
        } else {
            og_user.style.display = "none";
            if (issupper) lb_cvuser.innerHTML = "Supper Administrator";
            lb_cvuser.style.display = "";
        }
        MenuItemInit();
    });
</script>