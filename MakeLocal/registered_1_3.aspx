<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_1_3.aspx.cs" Inherits="registered_1_3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui.css">

    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_1_3.css">

    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>

    <script type="text/javascript">


        // 顯示讀取遮罩
        function ShowProgressBar() {
            displayProgress();
            displayMaskFrame();
        }

        // 隱藏讀取遮罩
        function HideProgressBar() {
            var progress = $('#divProgress');
            var maskFrame = $("#divMaskFrame");
            progress.hide();
            maskFrame.hide();
        }
        // 顯示讀取畫面
        function displayProgress() {
            var w = $(document).width();
            var h = $(window).height();
            var progress = $('#divProgress');
            progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
            progress.show();
        }
        // 顯示遮罩畫面
        function displayMaskFrame() {
            var w = $(window).width();
            var h = $(document).height();
            var maskFrame = $("#divMaskFrame");
            maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
            maskFrame.show();
        }

    </script>
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

            <table style="width: 100%;">
                <tr>
                    <td class="space">&nbsp;</td>
                    <td style="width: 50%">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/1/4.PNG" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 15%">&nbsp;</td>
                                <td style="width: 70%" align="center">
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ありがとうございました。"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="承認まで少々お時間を頂戴します。"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Black"
                                        Text="ご登録のメールアドレスに承認のご連絡を致します。"></asp:Label>
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Black"
                                        Text="サポートの前に説明動画をお済ませください"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="Label12" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                                <td style="width: 15%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <iframe width="400" height="300" src="" frameborder="0" allowfullscreen></iframe>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <br />
                                    <asp:Button ID="Button2" runat="server" Text="視聽しました" CssClass="file-upload"
                                        Width="50%" OnClientClick="ShowProgressBar();" OnClick="Button2_Click" />
                                    <br />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>&nbsp;</td>
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
