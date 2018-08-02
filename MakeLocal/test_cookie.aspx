<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_cookie.aspx.cs" Inherits="test_cookie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="set" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="get" OnClick="Button2_Click" />
    </div>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Button" />
    </form>
</body>
</html>
