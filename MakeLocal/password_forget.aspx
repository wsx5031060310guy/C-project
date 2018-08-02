<%@ Page Language="C#" AutoEventWireup="true" CodeFile="password_forget.aspx.cs" Inherits="password_forget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>



    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/bootstrap.css">
    <link rel="stylesheet" href="css/style.css" />
    <link rel="stylesheet" href="css/password_forget.css" />


    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="http://code.jquery.com/jquery-latest.js"></script>
     <script src="Scripts/jquery-ui.js"></script>


    <link rel="stylesheet" href="css/file-upload.css" type="text/css">
</head>
<body>
    <form id="form1" runat="server">
        <div id="header">
            <table style="width: 100%; height: 100%">
                <tr>
                    <td align="left" width="15%">

                        <table style="width: 100%;">
                            <tr>
                                <td width="5%">&nbsp;</td>

                                <td class="rin">
                                    <asp:Image ID="Label_logo" Style="width: 60px; height: auto; cursor: pointer; margin-left: 3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                <!--hehe-->


                            </tr>
                        </table>
                    </td>
                    <td width="55%">&nbsp;</td>
                    <td width="30%">
                        <table style="width: 100%;">
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>

            <table style="width: 100%; margin-top: 20px">
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">&nbsp;</td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="パスワードをお忘れの場合"></asp:Label>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label4" runat="server" Text="メールアドレス"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label6" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="60%">
                                    <asp:TextBox ID="loginname_TextBox" runat="server" Width="100%" Wrap="False" placeholder=" メールアドレスを入力"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" class="style2" align="center">
                                    <br />
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                    <asp:Button ID="Button1" runat="server" CssClass="file-upload" Height="40px"
                                        Text="次へ" Width="100%" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <%--<td  colspan="2" class="style2" align="center"><table style="width:100%">
                                <tbody>
                                    <tr>
                            <td style="width:49%">
                                <asp:Button ID="Button4" runat="server" CssClass="file-upload" Width="100%" Height="40px"
                                    Text="子育てサポーターになる" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                            </td>
                                        <td style="width:2%"></td>
                                <td style="width:49%">
                                <asp:Button ID="Button5" runat="server" CssClass="file-upload" Width="100%" Height="40px"
                                    Text="こどもを預ける" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                            </td>
                        </tr>
                                </tbody>
                                </table></td>--%>
                            </tr>


                            <tr>
                                <td colspan="2" width="100%" align="center">&nbsp;</td>
                                <br />

                            </tr>
                        </table>
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">&nbsp;</td>
                    <td class="space">&nbsp;</td>
                </tr>
            </table>

        </div>
        <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
            <br />
            <font color="#95989A" size="2px">読み込み中</font>
        </div>
        <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
        </div>
    </form>
</body>
</html>
