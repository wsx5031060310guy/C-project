<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_1.aspx.cs" Inherits="registered_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_1.css">



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
                                    <asp:Image ID="Label_logo" Style="width: 60px; height: auto;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>

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
                    <td width="50%" align="center">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/1/1.PNG" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="サポートできる内容をお選びください"></asp:Label>
                                    &nbsp;<asp:Label ID="Label9" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※複数可"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td>
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatColumns="2"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem>送迎</asp:ListItem>
                                        <asp:ListItem>利用宅で預かる</asp:ListItem>
                                        <asp:ListItem>自宅で預かる</asp:ListItem>
                                        <asp:ListItem>乳児預かり</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td width="30%" align="left">
                                    <br />
                                    <asp:Label ID="Label10" runat="server" Text="同時受入れ人数"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:DropDownList ID="howmany_DropDownList" runat="server" CssClass="textbox"
                                        Height="20px">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label12" runat="server" Text="人"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td align="left">
                                    <br />
                                    <asp:Label ID="Label11" runat="server" Text="サポートできる年齢"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="age_range_start_year_DropDownList" runat="server" CssClass="textbox"
                                        Height="20px">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="Label68" runat="server" Text="歳"></asp:Label>

                                    <asp:DropDownList ID="age_range_start_month_DropDownList" runat="server" CssClass="textbox"
                                        Height="20px">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="Label13" runat="server" Text="ヶ月  ~"></asp:Label>

                                    <asp:DropDownList ID="age_range_end_year_DropDownList" runat="server" CssClass="textbox"
                                        Height="20px">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="Label69" runat="server" Text="歳"></asp:Label>

                                    <asp:DropDownList ID="age_range_end_month_DropDownList" runat="server" CssClass="textbox"
                                        Height="20px">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="Label70" runat="server" Text="ヶ月"></asp:Label>

                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ご経験と資格をお選びください"></asp:Label>
                                    &nbsp;<asp:Label ID="Label3" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※複数可"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td>
                                    <asp:CheckBoxList ID="CheckBoxList2" runat="server" RepeatColumns="2"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem>子育て経験</asp:ListItem>
                                        <asp:ListItem>障害児の預かり経験</asp:ListItem>
                                        <asp:ListItem>保育士資格</asp:ListItem>
                                        <asp:ListItem>幼稚園教諭資格</asp:ListItem>
                                        <asp:ListItem>小学校教員資格</asp:ListItem>
                                        <asp:ListItem>医師．看護士資格</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="3" align="center">
                                    <br />
                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="その他の特徴をお選びください"></asp:Label>
                                    &nbsp;<asp:Label ID="Label5" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※複数可"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="15%">&nbsp;</td>
                                <td width="30%" valign="top">
                                    <asp:CheckBoxList ID="CheckBoxList3" runat="server" RepeatColumns="1">
                                        <asp:ListItem>ペットあり</asp:ListItem>
                                        <asp:ListItem>自家用車送迎可</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="Label15" runat="server" Text="("></asp:Label>
                                            </td>
                                            <td width="70%">
                                                <asp:CheckBoxList ID="CheckBoxList4" runat="server" RepeatColumns="2">
                                                    <asp:ListItem>室內</asp:ListItem>
                                                    <asp:ListItem>室外</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td width="15%">
                                                <asp:Label ID="Label17" runat="server" Text=")"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Text="("></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text="チャイルドシートあり" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text=")"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ルールを決めてください"></asp:Label>
                                    &nbsp;<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" Text="ルール"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="baby_rule_TextBox" runat="server" CssClass="textbox" Height="200px" placeholder="例) - お着替えを2着持ってきてください。 - お迎えが遅れる場合は連絡をください。"
                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="特記事項"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="baby_notice_TextBox" runat="server" CssClass="textbox" Height="100px" placeholder="例) - 家には猫がいます。 -熱が出た場合には大事のないようにお迎えに来てく ださい"
                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="3" align="center">
                                    <br />
                                    <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="報酬金額を設定してください"></asp:Label>
                                    &nbsp;<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td width="60%">
                                    <br />
                                    <asp:TextBox ID="money_TextBox" runat="server" Height="100%" Width="90%"></asp:TextBox>
                                    <br />
                                </td>
                                <td width="20%" valign="middle">
                                    <br />
                                    <asp:Label ID="Label71" runat="server" Text="円 / 時間"></asp:Label>
                                    <br />
                                </td>

                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td colspan="2">
                                    <br />
                                    <asp:Label ID="money_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

                                    <br />
                                    <asp:Label ID="Label72" runat="server" Text="相場は1時間800円~1200円となっております" ForeColor="#999999"></asp:Label>
                                    <br />
                            </tr>
                        </table>
                        <br />
                        <hr />

                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="サポート可能日"></asp:Label>
                                    &nbsp;<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label21" runat="server" Text="サポートできる曜日に✔マークをつけて時間を選択してください"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <br />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox0" runat="server" Text="月" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList0" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label23" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList0" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label38" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList0" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label46" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList0" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label54" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox1" runat="server" Text="火" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList1" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label24" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList1" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label61" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList1" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label47" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList1" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label55" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox2" runat="server" Text="水" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList2" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label25" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList2" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label62" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList2" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label48" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList2" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label56" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox3" runat="server" Text="木" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList3" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label26" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList3" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label63" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList3" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label49" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList3" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label57" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox4" runat="server" Text="金" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList4" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label27" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList4" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label64" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList4" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label50" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList4" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label58" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="week_of_day_CheckBox5" runat="server" Text="土" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="start_hour_DropDownList5" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label28" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList5" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label65" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList5" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label51" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList5" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label59" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="auto-style1">
                                                <asp:CheckBox ID="week_of_day_CheckBox6" runat="server" Text="日" />
                                            </td>
                                            <td align="center" class="auto-style1">
                                                <asp:DropDownList ID="start_hour_DropDownList6" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label29" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="start_minute_DropDownList6" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label66" runat="server" Text="分  ~"></asp:Label>
                                                <asp:DropDownList ID="end_hour_DropDownList6" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>01</asp:ListItem>
                                                    <asp:ListItem>02</asp:ListItem>
                                                    <asp:ListItem>03</asp:ListItem>
                                                    <asp:ListItem>04</asp:ListItem>
                                                    <asp:ListItem>05</asp:ListItem>
                                                    <asp:ListItem>06</asp:ListItem>
                                                    <asp:ListItem>07</asp:ListItem>
                                                    <asp:ListItem>08</asp:ListItem>
                                                    <asp:ListItem>09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label52" runat="server" Text="時"></asp:Label>
                                                <asp:DropDownList ID="end_minute_DropDownList6" runat="server" CssClass="textbox"
                                                    Height="20px">
                                                    <asp:ListItem>00</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="Label60" runat="server" Text="分"></asp:Label>
                                            </td>
                                            <td class="auto-style1"></td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                    <asp:Button ID="Button2" runat="server" Text="保存して次へ" CssClass="file-upload"
                                        Width="50%" OnClick="Button2_Click" OnClientClick="ShowProgressBar();" />
                                </td>
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
