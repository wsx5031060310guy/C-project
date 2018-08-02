<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_2_1.aspx.cs" Inherits="registered_2_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_2_1.css">


    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript">


        $(function () {
            $("#accordion").accordion({
                collapsible: true, active: false, icons: false
            });
        });

        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUploadDoc.ClientID %>").click();
            }
        }

        function UploadFile1(fileUpload1) {
            if (fileUpload1.value != '') {
                document.getElementById("<%=btnUploadDoc1.ClientID %>").click();
            }
        }
        function PopulateDays() {

            var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
            var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
            var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
            var y = ddlYear.options[ddlYear.selectedIndex].value;
            var m = ddlMonth.options[ddlMonth.selectedIndex].value != 0;
            if (ddlMonth.options[ddlMonth.selectedIndex].value != 0 && ddlYear.options[ddlYear.selectedIndex].value != 0) {
                var dayCount = 32 - new Date(ddlYear.options[ddlYear.selectedIndex].value, ddlMonth.options[ddlMonth.selectedIndex].value - 1, 32).getDate();
                ddlDay.options.length = 0;
                for (var i = 1; i <= dayCount; i++) {
                    AddOption(ddlDay, i, i);
                }
            }
        }

        function AddOption(ddl, text, value) {
            var opt = document.createElement("OPTION");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }

        function Validate(sender, args) {
            var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
                var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
                var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
                args.IsValid = (ddlDay.selectedIndex != 0 && ddlMonth.selectedIndex != 0 && ddlYear.selectedIndex != 0)
            }

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
                    <td align="center" class="style1" width="50%">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/2/2.PNG" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="3" align="center">
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="書類提出"></asp:Label>
                                    &nbsp;<asp:Label ID="Label19" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%;"></td>
                                <td>
                                    <div id="accordion">
                                        <h3>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 15%">
                                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/registered_1_1/check1.png" Visible="False" />
                                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/images/registered_1_1/checked1.png" /></td>
                                                    <td align="center" style="width: 70%">ご本人の身分証明書<asp:Label ID="image_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:HyperLink ID='hy' runat="server" NavigateUrl="javascript:void(0);"
                                                            Target="_blank" Font-Size="Small" Font-Underline="False">詳しく見る</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </h3>
                                        <div>
                                            <p>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/registered_1_1/1.PNG" /></td>
                                                        <td align="center" style="width: 20%">
                                                            <label class="file-upload">
                                                                <span style="width: 8em;">画像を登録</span>
                                                                <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);" />
                                                            </label>
                                                            <br />
                                                            <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                                                            <asp:Image ID="type0_Image" runat="server" Height="100px" Width="150px" />

                                                            <br />

                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </p>
                                        </div>
                                        <h3>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 15%">
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/images/registered_1_1/check1.png" Visible="False" />
                                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/registered_1_1/checked1.png" /></td>
                                                    <td align="center" style="width: 70%">颜写真1枚<asp:Label ID="image_Label0" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:HyperLink ID='hy0' runat="server" NavigateUrl="javascript:void(0);"
                                                            Target="_blank" Font-Size="Small" Font-Underline="False">詳しく見る</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </h3>
                                        <div>
                                            <p>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <asp:Image ID="Image3" runat="server" Height="78px" ImageUrl="~/images/registered_1_1/2.PNG" Width="261px" /></td>
                                                        <td align="center" style="width: 20%">
                                                            <label class="file-upload">
                                                                <span style="width: 8em;">画像を登録</span>
                                                                <asp:FileUpload ID="fuDocument1" runat="server" onchange="UploadFile1(this);" />
                                                            </label>
                                                            <br />
                                                            <asp:Button ID="btnUploadDoc1" Text="Upload" runat="server" OnClick="UploadDocument1" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                                            <asp:Image ID="type1_Image" runat="server" Height="100px" Width="100px" />

                                                            <br />

                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </p>
                                        </div>
                                    </div>
                                    &nbsp;</td>
                                <td style="width: 10%;"></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="クレジットカードのご登録"></asp:Label>
                                    &nbsp;
                                <asp:Label ID="Label11" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                    Text="※必須"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="position: relative; display: block">
                                    <div style="position: absolute; background-color: #000; z-index: 999998; opacity: 0.8; width: 100%; height: 100%; text-align: center;">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <span style="color: white;">クローズドβ 版では直接ご当人同士でお支払いください。</span>
                                    </div>
                                    <div style="background-color: #CCCCCC">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 5%; height: 5%">&nbsp;</td>
                                                <td style="height: 5%">&nbsp;</td>
                                                <td style="width: 5%; height: 5%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>

                                                    <table style="width: 100%;">

                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label16" runat="server" Text="カード番号"></asp:Label>
                                                                <br />
                                                                <br />
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="bank_number_TextBox" runat="server" Width="100%" Wrap="False" placeholder="カード番号をハイフン無しで記入"
                                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                                <br />
                                                                <asp:Label ID="bank_number_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label15" runat="server" Text="有効期限"></asp:Label>
                                                                <br />
                                                                <br />
                                                            </td>
                                                            <td style="width: 30%" valign="top">
                                                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox" onchange="PopulateDays()"
                                                                    Height="20px">
                                                                </asp:DropDownList>
                                                                &nbsp;/&nbsp;<asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox" onchange="PopulateDays()"
                                                                    Height="20px">
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:DropDownList ID="ddlDay" runat="server" CssClass="textbox" Height="20px" Visible="False">
                                                                </asp:DropDownList>
                                                                <br />
                                                            </td>
                                                            <td style="width: 50%" valign="top">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="width: 40%" valign="top">
                                                                            <asp:Label ID="Label18" runat="server" Text="確認コード"></asp:Label>
                                                                            <br />
                                                                        </td>
                                                                        <td style="width: 60%" valign="top">
                                                                            <asp:TextBox ID="bank_second_number_TextBox" runat="server" Width="100%" Wrap="False"
                                                                                CssClass="textbox" Height="20px"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="bank_second_number_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label17" runat="server" Text="名義人"></asp:Label>
                                                                <br />
                                                                <br />
                                                            </td>
                                                            <td colspan="2" align="center">
                                                                <asp:TextBox ID="bank_person_TextBox" runat="server" Width="100%" Wrap="False" placeholder="カードの名義人を入力"
                                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                                <br />
                                                                <asp:Label ID="Label21" runat="server" Font-Size="Small" ForeColor="#666666" Text="※力一ドに記載してあるとリ入力してください"></asp:Label>
                                                                <br />
                                                                <asp:Label ID="bank_person_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5%; height: 5%">&nbsp;</td>
                                                <td style="height: 5%">&nbsp;</td>
                                                <td style="width: 5%; height: 5%">&nbsp;</td>
                                            </tr>
                                        </table>

                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <br />
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                    <asp:Button ID="Button2" runat="server" Text="保存して次へ" CssClass="file-upload"
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
