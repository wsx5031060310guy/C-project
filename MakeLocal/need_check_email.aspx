<%@ Page Language="C#" AutoEventWireup="true" CodeFile="need_check_email.aspx.cs" Inherits="need_check_email" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <link rel="stylesheet" type="text/css" href="css/style.css">


    <style type="text/css">
        #header{
background-color:#ea9494;
height:80px;
position:absolute;
width:100%;
top:0;
text-align:center;
line-height:70px;
}
   .rin{
        text-align: left;
        min-width:150px;
 line-height: 100%;
    }
    .rin2 {
    display:none}
        div {
        text-align:center;
        }


        @media (max-width: 681px) {
            .rin2 {
                display: block;
                position: relative;

                top: 10px;
            }

            .rin {
                display: none;
            }
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
                                  <td class="rin"><asp:Image id="Label_logo" style="width:55px;height:auto;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>


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

                <asp:Label ID="Label1" runat="server" Visible="True"></asp:Label>
                &nbsp;<asp:Label ID="Label2" runat="server" Text=" 秒後にメインページが開きます。"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>
    </form>
</body>
</html>
