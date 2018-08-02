<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testupload.aspx.cs" Inherits="testupload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="Scripts/jquery-1.12.4.js"></script>
    <link rel="stylesheet" href="css/file-upload_fb.css" type="text/css"/>
    <script>
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUploadDoc.ClientID %>").click();
            }
        }
        // 顯示讀取遮罩
        function ShowProgressBar() {
            displayProgress();
            displayMaskFrame();
        }

        // 隱藏讀取遮罩
        function HideProgressBar() {
            var progress = $('#divProgress');
            var maskFrame = $("#divMaskFrame");
            progress.hide();
            maskFrame.hide();
        }
        // 顯示讀取畫面
        function displayProgress() {
            var w = $(document).width();
            var h = $(window).height();
            var progress = $('#divProgress');
            progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
            progress.show();
        }
        // 顯示遮罩畫面
        function displayMaskFrame() {
            var w = $(window).width();
            var h = $(document).height();
            var maskFrame = $("#divMaskFrame");
            maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
            maskFrame.show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <label class="file-upload" style=" font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;">
            <span>写真</span>
            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);"  AllowMultiple="True"/>
             </label>
         <br />
                                <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
    </div>
        <asp:Panel ID="Panel1" runat="server"></asp:Panel>
        <asp:HiddenField ID="image_HiddenField" runat="server" />
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
    </form>
    <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
</body>
</html>
