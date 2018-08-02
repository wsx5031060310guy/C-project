<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_history.aspx.cs" Inherits="test_history" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
    <style>
        body {
    margin:0;
    overflow:hidden;
}
        .mainBody {
    position: absolute;
    top: 0px;
    bottom: 100px;
    width:100%;
}
        .bottombody{
    position: absolute;
    bottom: 0px;
    width:100%;
    height:100px;
    background-color:rgba(210, 210, 210, 1);
}
        .back_button
        {
            position: absolute;
            background-image:url('images/mamaro/back_page 2.png');
            background-color:rgba(0, 0, 0, 0);
            background-size: 100% 100%;
            background-repeat: no-repeat;
            width:90px;
            height:90px;
            border-style:none;
            cursor: pointer;
            right:62%;
        }
        .next_button
        {
            position: absolute;
            background-image:url('images/mamaro/next_page 2.png');
            background-color:rgba(0, 0, 0, 0);
            background-size: 100% 100%;
            background-repeat: no-repeat;
            width:90px;
            height:90px;
            border-style:none;
            cursor: pointer;
            right:32%;
        }
    </style>
    <script language="javascript">
        function gonext() {
            history.forward();
        }
        function goback() {
            history.back();
        }
</script>
</head>
<body>
<%--    <div class="mainBody">
<iframe id="maincontent" src="https:///main_guest_light.aspx?=221-0851" width="100%" height="100%" frameborder="0" sandbox="allow-same-origin allow-scripts"></iframe>
        </div><br/>--%>

    <form id="form1" runat="server">
    <div class="mainBody">
        <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%"></asp:Panel>
    </div>
         <div class="bottombody">
        <input type=button class="back_button" onClick=goback()>
                <input type=button class="next_button" onClick=gonext()>
    </div>
    </form>
</body>
</html>
