<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default4.aspx.cs" Inherits="Default4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
             <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>

  <script src="Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui.css">

    <link rel="stylesheet" href="css/jquery.fileupload.css">
        <link rel="stylesheet" href="css/file-upload_fb.css" type="text/css"/>
<style type="text/css">
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
          #progressbar {
    background-color: black;
    background-repeat: repeat-x;
    border-radius: 13px;
    padding: 3px;
}

#progressbar > div {
    background-color: orange;
    width: 0% ;
    height: 20px;
    border-radius: 10px;
}
     </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
        <asp:Panel ID="Panel2" runat="server" Width="700px">
        </asp:Panel>
         <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#1B3563" size="2px">資料処理中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        <asp:Panel ID="javaplace" runat="server">
        </asp:Panel>
    </form>
    <table style="width:100%;">
        <tr>
            <td style="border-style: solid; border-width: thin">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                【依頼願い】
                <ul type="disc">
                 <li>単発/定期&nbsp;&nbsp;単発</li>
                 <li>日時&nbsp;&nbsp;9 月 1 日 18 : 00-20 : 00</li>
                 <li>依頼内容&nbsp;&nbsp;送迎 、 一時預け</li>
                 <li>お願いするお子様&nbsp;&nbsp;1 歳 ( 1 名 ) </li>
                 <li>現在のステータス&nbsp;&nbsp;確認待ち</li>
                 <li>確認用URL&nbsp;&nbsp;xxxxxx // xxxxxxx.com</li>
                </ul>
                <a href=" + http://www.w3schools.com/html/ + ">5555</a>
                【依頼願い】
                <ul type="disc">
                 <li>単発/定期&nbsp;&nbsp;定期</li>
                 <li>定期&nbsp;&nbsp;9 月 1 日 - 9 月 10 日</li>
                 <li>曜日&nbsp;&nbsp;金曜日 18 : 00-20 : 00</li>
                 <li>曜日&nbsp;&nbsp;月曜日 13 : 00-18 : 00</li>
                 <li>依頼内容&nbsp;&nbsp;送迎 、 一時預け</li>
                 <li>お願いするお子様&nbsp;&nbsp;1 歳 ( 1 名 ) </li>
                 <li>現在のステータス&nbsp;&nbsp;確認待ち</li>
                 <li>確認用URL&nbsp;&nbsp;xxxxxx // xxxxxxx.com</li>
                </ul>

            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</body>
</html>
