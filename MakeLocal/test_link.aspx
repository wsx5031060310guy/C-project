<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_link.aspx.cs" Inherits="test_link" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="ja">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server" Width="507px"></asp:TextBox>
        <br />
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
        <br/><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br/>
        <asp:Panel ID="Panel1" runat="server"></asp:Panel>
    </div>
    </form>
</body>
</html>
