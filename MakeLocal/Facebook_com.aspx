<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Facebook_com.aspx.cs" Inherits="Facebook_com" enableEventValidation="false" %>

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
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" Visible="False" />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" Visible="False" />
        <asp:TextBox ID="TextBox2" runat="server" Height="82px" TextMode="MultiLine" Width="395px"></asp:TextBox>
        <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
    
        <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
        <asp:ListBox ID="ListBox3" runat="server"></asp:ListBox>
        <asp:ListBox ID="ListBox4" runat="server"></asp:ListBox>
        <asp:ListBox ID="ListBox5" runat="server"></asp:ListBox>
        <asp:ListBox ID="ListBox6" runat="server"></asp:ListBox>
        <asp:ListBox ID="ListBox7" runat="server"></asp:ListBox>
        <asp:Panel ID="Panel2" runat="server">
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
        </div>
        <p>

                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>

        </p>
    </form>
</body>
</html>
