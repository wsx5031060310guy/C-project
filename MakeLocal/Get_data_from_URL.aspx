<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Get_data_from_URL.aspx.cs" Inherits="Get_data_from_URL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                </asp:Timer>
                <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
         <br/>
        <asp:TextBox ID="TextBox1" runat="server" Width="376px"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <br />
            <br />
        <asp:ListBox ID="ListBox5" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>
        <asp:ListBox ID="ListBox1" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>
        <asp:ListBox ID="ListBox2" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>
        <asp:ListBox ID="ListBox3" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>
        <asp:ListBox ID="ListBox4" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>
        <asp:ListBox ID="ListBox6" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <br/>

    </div>
    </form>
</body>
</html>
