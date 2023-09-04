<%@ Control Language="C#" AutoEventWireup="true" ClassName="QNMedia.Components.Home" Inherits="DotNetNuke.Entities.Modules.PortalModuleBase" %>
<%@ Register Src="~/DesktopModules/Controls/Home/Dashboard.ascx" TagPrefix="nv" TagName="Dashboard" %>
<%@ Register Src="~/DesktopModules/Controls/CMS/QT_DanhMuc.ascx" TagPrefix="qn" TagName="QTDM" %>

<section class="section dashboard content-home" id="selectionhome" style="display: none;">
    <nv:Dashboard runat="server" ID="Dashboard" />
</section>

<section class="section card" id="sectioncontent">
    <div class="card-body mt-3">
        <div class="components d-none" data="danhmuc">
            <qn:QTDM ID="qnqtdm" runat="server" />
        </div>
        <div class="components d-none" data="phanquyen">
               Phân quyền
        </div>
        <div class="components d-none" data="tintuc">
                Tin tức
        </div>
         <div class="components d-none" data="chitiet">
                chi tiết tin tức
        </div>
    </div>
</section>
<script>
    const userid = <%=UserId%>;
    const portalid = <%=PortalId%>;
    const displayname = '<%=UserInfo.DisplayName%>';
    const issupper = '<%=UserInfo.IsSuperUser%>' == 'True';
    const isadmin =  '<%=UserInfo.IsAdmin%>'=='True';
    const host = location.protocol + '//' + location.host + "/";
</script>