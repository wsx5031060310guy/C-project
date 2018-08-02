<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_1_4.aspx.cs" Inherits="registered_1_4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui.css">

    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_1_4.css">

    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/datepicker-ja.js"></script>

    <script type="text/javascript">

        $(function () {
            $("#datepicker").datepicker($.datepicker.regional["ja"]);
            $("#datepicker0").datepicker($.datepicker.regional["ja"]);
            $("#datepicker1").datepicker($.datepicker.regional["ja"]);
        });

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
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/1/5.PNG" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 10%">&nbsp;</td>
                                <td align="center" style="width: 80%">
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ご本人様の確認が取れました。"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="運営との面接がありますので、ご都合をお教えください。 (目安30分)"></asp:Label>
                                    <br />
                                    <br />
                                    <br />
                                </td>
                                <td style="width: 10%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <br />
                                    <br />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 35%">&nbsp;</td>
                                            <td>
                                                <asp:CheckBox ID="use_rule_CheckBox" runat="server" />
                                                <asp:HyperLink ID="HyperLink1" runat="server" Font-Overline="False" Font-Underline="False" NavigateUrl="" Target="_blank">利用規約</asp:HyperLink>
                                                <asp:Label ID="Label58" runat="server" Text="に同意する"></asp:Label>
                                                <br />
                                                <asp:Label ID="use_rule_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />


                                                <br />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <br />
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
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
