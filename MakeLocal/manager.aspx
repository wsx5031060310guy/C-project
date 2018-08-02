<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manager.aspx.cs" Inherits="manager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server"></asp:View>
            <asp:View ID="View2" runat="server"></asp:View>
            <asp:View ID="View3" runat="server"></asp:View>
            <asp:View ID="View4" runat="server"></asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
