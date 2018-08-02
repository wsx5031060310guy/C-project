<%@ Page Language="C#" AutoEventWireup="true" CodeFile="twitter_gov_1.aspx.cs" Inherits="twitter_gov_1" enableEventValidation="false" %>

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
        <asp:Label ID="Label1" runat="server" Text="URL:"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer2" runat="server" Interval="10000" OnTick="Timer2_Tick">
                </asp:Timer>

                <asp:Label ID="Label5" runat="server" Text="Last update time:"></asp:Label>
                <br />

                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>

        <br/>
        <asp:TextBox ID="TextBox1" runat="server" Width="376px"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" Visible="False" />
    
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="page html:"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox2" runat="server" Height="82px" TextMode="MultiLine" Width="395px"></asp:TextBox>
        <asp:ListBox ID="ListBox1" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
            <asp:ListBox ID="ListBox2" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
            <asp:ListBox ID="ListBox3" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
         <asp:ListBox ID="ListBox4" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
    <asp:ListBox ID="ListBox5" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
    <asp:ListBox ID="ListBox6" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
        <asp:ListBox ID="ListBox7" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
    <asp:ListBox ID="ListBox8" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>
         <asp:ListBox ID="ListBox11" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>

                    <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                <asp:ListBox ID="ListBox9" runat="server" Height="152px" style="margin-top: 0px" Width="100%"></asp:ListBox>

                    </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
