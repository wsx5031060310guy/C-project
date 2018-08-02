<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_map.aspx.cs" Inherits="mamaro_map" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>mamaro map</title>
    <script src="js/moment.js"></script>
    <style>
       #map {
        height: 600px;
        width: 100%;
       }
    </style>
</head>
<body>

    <div id="map"></div>

    <form id="form1" runat="server">
    <div>
       <asp:Panel ID="main_Panel" runat="server" HorizontalAlign="Center">
        </asp:Panel>
    </div>
    </form>
     <div id="mamaroonline" style="text-align: center;"></div>
</body>
</html>
