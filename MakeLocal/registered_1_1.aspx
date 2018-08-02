<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_1_1.aspx.cs" Inherits="registered_1_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link rel="stylesheet" href="css/registered_1_1.css">
     <link rel="stylesheet" href="css/style.css">
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
  
  <%--  <style type="text/css">
         body {
  font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;
   background-color:#E9EBEE;
    margin-left:0 !important;
}
         #header{
background-color:#ea9494;
height:80px;
text-align:center;
position: fixed;
    width: 100%;
    top:0;
line-height:70px;
}
         .file-upload {
    height:auto !important;
    width:auto !important;
}
        #Image1 {
                margin-top: 20px;
        }
         .rin{
        text-align: left;
        min-width:150px;
        line-height:100%;
    }

    .rin2 {
    display:none}
        #Panel1 {
            display: none;
        }


        .textbox {
-webkit-border-radius: 5px;
-moz-border-radius: 5px;
border-radius: 5px;
  }
          input:focus
  {
      border: 2px solid #AA88FF;
    background-color: #ff7575;
    }
                  img {
    width: 100%;
    height: auto;
}
        .space{
    width: 25%;
}
        .file-upload-custom {
        width:11em !important;

        }
        .file-upload-custom span {
        position: static !important;

        }
        @media (max-width: 680px){
.space{
    display: none;
}

.rin2{
        display:block;
    position: relative;
    text-align: center;

    }

#div_sidebar_right{
    width: 100px;
    height: 100%;
    position: relative;
    float: right;
    z-index: 9997;
    display: none;
    }
#sidebar_right{
width: 100%;
background-color:#e7e7e7;
z-index: 9998;
text-align:left;
position: fixed;
top: 1em;
display: block;
}



}/*--------------------------end media*/
    </style> --%>
<link rel="stylesheet" href="css/file-upload.css" type="text/css"/>
<link rel="stylesheet" href="css/jquery-ui.css">
      <script src="Scripts/jquery-1.12.4.js"></script>
  <script src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript" >

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
                <div id="header">
                    <table style="width:100%;height:100%">
                        <tr>
                            <td align="left" width="15%">

                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                   <td class="rin"><asp:Image id="Label_logo" style="width:60px;height:auto;cursor:pointer;margin-left:3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>

<!--hehe-->

                                    </tr>
                                </table>
                            </td>
                            <td width="55%">
                                &nbsp;</td>
                            <td width="30%">
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div>
    <div>

        <table style="width:100%;">
            <tr>
                <td class="space">
                </td>
                <td align="center" class="style1" width="50%">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/1/2.PNG" />
                </td>
                <td class="space">
                </td>
            </tr>
            <tr>
                <td class="space">
                    &nbsp;</td>
                <td>
                    <table style="width:100%;">
                        <tr>
                            <td align="center">
                                <br />
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                    Text="預かりの様子がわかる写真のアップ口ード"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td align="center" /*style="border-style: solid; border-width: thin"*/>


                                <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                                 <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto">
                                 </asp:Panel>



                                        <label class="file-upload file-upload-custom">
            <span style="width:8em;">画像を登録</span>
            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);"  AllowMultiple="True"/>
             </label>
                            <br />

                            <br />

                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                    Text="自己紹介文の作成"></asp:Label>
                                    <br />
                                    <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="ひと言コメント"></asp:Label>
                                <br />
                                <asp:TextBox ID="title_TextBox" runat="server" Width="100%" Wrap="False" placeholder="例)3人の子育て経験!乳幼児も見れます"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br /><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="自己総会文"></asp:Label>
                                <br />
                                <asp:TextBox ID="myself_content_TextBox" runat="server" Width="100%" placeholder="例) はじめまして、この度自分の子どもが大学生になったので、時間 に余裕ができました。子どもが好きなので子どもに関わる仕事をした いと思って今回参加させて頂きあmした。子育て経験豊富で、乳幼児 の預りも可能です。ぜひご連絡ください。"
                                    CssClass="textbox" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                    <br /><br />
                            </td>
                        </tr>
                    </table>
                    <table style="width:100%;">
                        <tr>
                            <td style="width:10%;">
                            </td>
                            <td>

                                &nbsp;</td>
                            <td style="width:10%;">
                            </td>
                        </tr>
                        <tr>
                             <td>&nbsp;</td>
                            <td align="center">
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Button ID="Button2" runat="server" Text="保存して次へ" CssClass="file-upload"
                                    Width="50%" OnClientClick="ShowProgressBar();" OnClick="Button2_Click" />
                                <br />
                             </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td class="space">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="space">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td class="space">
                    &nbsp;</td>
            </tr>
        </table>

    </div>
        <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
    </form>
</body>
</html>
