<%@ Control Language="C#" ClassName="NV.UI.Loadding"  %>
<div id="spinnermodal"  class="spinnermodal d-none" >
 <div class="loadding p-5">
    <div class="spinner-border text-primary mb-3" role="status">
      <span class="visually-hidden">Đang tải dữ liệu...</span>
    </div>
      <label class="w-100 fw-bold">Đang tải dữ liệu...</label>
    </div>
</div>
<style type="text/css">
    .spinnermodal{
        margin:auto;
        position: fixed;
        z-index: 1060;
        left: 0;
        top: 0%;
        width: 100%; /* Full width */
        height: 100%; /* Full height */
        overflow: auto; /* Enable scroll if needed */
        background-color:transparent;
    }
    .spinnermodal .loadding{
        position:fixed;
        left:45%;
        top:40%;
        text-align: center;
        border-radius:5px;
        background-color: #fff; /* Fallback color */
        opacity:0.6;
    }
</style>