<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activation_check_week.aspx.cs" Inherits="Activation_check_week" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <style type="text/css">
         #header{
background-color:#ea9494;
height:80px;
position:fixed;
width:100%;
top:0;
text-align:center;
line-height:70px;
}
           div {
        text-align:center;
        }
        h1 {
        margin-top:100px;
        }
        body
        {
            font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;background-color:#E9EBEE;
        margin-left:0 !important;
            }
    </style>
</head>
<body>
       <form id="form1" runat="server">
                        　<div id="header">
                    <table style="width:100%;height:100%">
                        <tr>
                            <td align="left" width="15%">

                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                    <td class="rin"><asp:Image ID="Label_logo" alt="" src="images/logo1.png" runat="server" style="width:60px; height:auto"></asp:Image></td>
                                    </tr>
                                </table>
                            </td>
                            <td width="55%">
                                &nbsp;</td>
                            <td width="30%">
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                        <td>

                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div>
    <div>
    <h1><asp:Literal ID="ltMessage" runat="server" /></h1>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                        <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick">
        </asp:Timer>
                <asp:Label ID="Label3" runat="server" Text=" "></asp:Label>&nbsp;
                <asp:Label ID="Label1" runat="server" Visible="True"></asp:Label>
                &nbsp;<asp:Label ID="Label2" runat="server" Text=" 秒後にメインページが開きます。"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>
    </form>
</body>
</html>
