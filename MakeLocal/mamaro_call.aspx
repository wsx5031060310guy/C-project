<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_call.aspx.cs" Inherits="mamaro_call" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
            <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title> call</title>
           <script src="Scripts/jquery-1.12.4.js"></script>
            <link rel="stylesheet" href="css/jquery-ui.css">

    <style>
         body{
            margin:0px;
        }
         .center {
    margin: auto;
    margin-top: 60px;
    width: 60%;
    border: 3px solid #73AD21;
    padding: 10px;
    height:auto;
    cursor:pointer;
    text-align:center;
    font-size: xx-large;
}
        .phonecall{
            background-image:url('./images/call_img/hotline.gif');
            width:200px;
            height:200px;
            -moz-background-size:cover;
-webkit-background-size:cover;
-o-background-size:cover;
background-size:cover;
        }
    </style>
</head>
<body>





    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="mainPanel" runat="server"></asp:Panel>
    </div>
    </form>
</body>
</html>
