<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_analysis.aspx.cs" Inherits="mamaro_analysis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title> analysis page</title>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="https://canvasjs.com/assets/script/jquery.canvasjs.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet">
            <style>
        .canvasjs-chart-container{margin-bottom:3rem!important;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="chartContainer" style="height: 300px; width: 100%;"></div>
        <div id="chartContainer1" style="height: 300px; width: 100%;"></div>
        <div id="chartContainer2" style="height: 300px; width: 100%;"></div>
        <asp:Panel ID="detailPanel" runat="server"></asp:Panel>
                <asp:Panel ID="javascriptPanel" runat="server"></asp:Panel>
    </div>
    </form>
</body>
</html>
