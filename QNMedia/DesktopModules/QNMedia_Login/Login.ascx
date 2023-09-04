<%@ Control Language="C#" Inherits="QTI.SAMLSSO.Login" CodeFile="Login.ascx.cs" AutoEventWireup="true" %>


<div class="container-fluid py-5 pt-0 wow fadeInUp" data-wow-delay="0.1s" style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp;max-width:500px;" >
   <div class="container py-5">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogindnn" CssClass="center">
                  <div class="row g-3 " >
                    <div class="col-12">
                      <label for='<%=txtUserName.ClientID %>' class="form-label fw-bold fs-6">Tài khoản</label>
                       <asp:TextBox ID="txtUserName" ToolTip="Tên đăng nhập" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqrusername" ControlToValidate="txtUserName" ValidationGroup="login" runat="server" Display="Dynamic" ErrorMessage="<br/>Vui lòng nhập tên đăng nhập!" ForeColor="Red" CssClass="d-none"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12">
                      <label for='<%=txtPassWord.ClientID %>' class="form-label  fw-bold fs-6">Mật khẩu</label>
                        <asp:TextBox ID="txtPassWord" runat="server" ToolTip="Mật khẩu" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqrpass" ControlToValidate="txtPassWord" ValidationGroup="login" runat="server" Display="Dynamic" ErrorMessage="<br/>Vui lòng nhập mật khẩu!" ForeColor="Red" CssClass="d-none"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12">
                      <div class="form-check">
                          <asp:CheckBox ID="cbSavelogin" runat="server" CssClass="form-check-input"  />
                          <label class="form-check-label fw-bold" for='<%=cbSavelogin.ClientID %>'>Ghi nhớ đăng nhập</label>
                      </div>
                    </div>
                    <div class="col-12">
                         <asp:Label ID="lblMS" runat="server" Text="Tài khoản hoặc mật khẩu không đúng!<br/>"  ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Button ID="btnLogindnn" TabIndex="-1" runat="server" Text="ĐĂNG NHẬP" CssClass="btn btn-primary w-100" OnClick="btnLogindnn_Click" ValidationGroup="login" CausesValidation="true"  />
                    </div>
                  </div>
            </asp:Panel>
       </div>
</div>
<style type="text/css">
</style>
<script type="text/javascript">
    $(document).ready(function () {
        var us = document.getElementById('<%=txtUserName.ClientID %>');
        var pw = document.getElementById('<%=txtPassWord.ClientID %>');
        var cblogin = document.getElementById('<%=cbSavelogin.ClientID %>');
        var buttonlogin = document.getElementById('<%=btnLogindnn.ClientID %>');
        us.setAttribute("placeholder", "Nhập tài khoản");
        pw.setAttribute("placeholder", "Nhập mật khẩu");
        if (checkCookie("qnmedialogin_user")) {
            us.value = Base64.decode(getCookie("qnmedialogin_user"));
            cblogin.checked = true;
        }
        if (checkCookie("qnmedialogin_pwd")) {
            pw.value = Base64.decode(getCookie("qnmedialogin_pwd"));
        }
        buttonlogin.addEventListener("click", function () {
            if (cblogin.checked) {
                setLoginCookie();
            } else {
                clearLoginCookie();
            }
            us.set_req(!us.check_null() ? "Vui lòng nhập tên đăng nhập" : "");
            if (!us.check_null()) {
                return;
            } 
            pw.set_req(!pw.check_null() ? "Vui lòng nhập mật khẩu" : "");
            if (!pw.check_null()) {
                return;
            }
        });
    });
    function setLoginCookie() {
        var us = document.getElementById('<%=txtUserName.ClientID %>');
        var pw = document.getElementById('<%=txtPassWord.ClientID %>');
        var date = new Date();
        setCookie("qnmedialogin_user", Base64.encode(us.value), date.addDays(5));
        setCookie("qnmedialogin_pwd", Base64.encode(pw.value), date.addDays(5));
    }
    function clearLoginCookie() {
        setCookie("qnmedialogin_user", '', new Date());
        setCookie("qnmedialogin_pwd", '', new Date());
    }
</script>
