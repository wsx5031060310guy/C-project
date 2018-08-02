<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered_1_2.aspx.cs" Inherits="registered_1_2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
      <link rel="stylesheet" href="css/file-upload.css" type="text/css"/>
      <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered_1_2.css">



      <script src="Scripts/jquery-1.12.4.js"></script>
  <script src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript" >

        $(function () {
            $("#accordion").accordion({
                collapsible: true, active: false
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

        function UploadFile2(fileUpload2) {
            if (fileUpload2.value != '') {
                document.getElementById("<%=btnUploadDoc2.ClientID %>").click();
            }
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
                    <table style="width:100%;height:100%">
                        <tr>
                            <td align="left" width="15%">

                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                    <td class="rin"><asp:Image id="Label_logo" style="width:60px;height:auto;cursor:pointer;margin-left:3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
<!--hehe-->
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

        <table style="width:100%;">
            <tr>
                <td class="space">&nbsp;</td>
                <td align="center" class="style1" width="50%">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/step/1/3.PNG" />
                </td>
                <td class="space">&nbsp;</td>
            </tr>
            <tr>
                <td class="space">&nbsp;</td>
                <td>
                    <table style="width:100%;">
                        <tr>
                            <td colspan="3" align="center">
                                <br />
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                    Text="書類提出"></asp:Label>
                                    <br />
                                    <br />
                                    </td>
                        </tr>
                        <tr>
                            <td style="width:10%;">
                            </td>
                            <td>
<div id="accordion">
  <h3>
<table style="width:100%;">
          <tr>
              <td style="width: 15%"> <asp:Image ID="Image4" runat="server" ImageUrl="~/images/registered_1_1/check1.png" Visible="False" />
      <asp:Image ID="Image5" runat="server" ImageUrl="~/images/registered_1_1/checked1.png" /></td>
              <td align="center" style="width: 70%">ご本人の身分証明書&nbsp; <asp:Label ID="Label12" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                    Text="※必須"></asp:Label>
                                    <asp:Label ID="image_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    </td>
              <td style="width: 15%">
                                <asp:HyperLink id='hy' runat="server" NavigateUrl="javascript:void(0);"
                                    Target="_blank" Font-Size="Small" Font-Underline="False">詳しく見る</asp:HyperLink>
                                                </td>
          </tr>
      </table>
    </h3>
  <div>
    <p>
        <table style="width:100%;">
            <tr>
                <td style="width: 80%"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/registered_1_1/1.PNG" /></td>
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
     <table style="width:100%;">
          <tr>
              <td style="width: 15%"> <asp:Image ID="Image6" runat="server" ImageUrl="~/images/registered_1_1/check1.png" Visible="False" />
      <asp:Image ID="Image7" runat="server" ImageUrl="~/images/registered_1_1/checked1.png" /></td>
              <td align="center" style="width: 70%">顔写真1枚&nbsp;
                                <asp:Label ID="Label13" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                    Text="※必須"></asp:Label>
                                    <asp:Label ID="image_Label0" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    </td>
              <td style="width: 15%">
                                <asp:HyperLink id='hy0' runat="server" NavigateUrl="javascript:void(0);"
                                    Target="_blank" Font-Size="Small" Font-Underline="False">詳しく見る</asp:HyperLink>
                                                </td>
          </tr>
      </table>
    </h3>
  <div>
    <p>
        <table style="width:100%;">
            <tr>
                <td style="width: 80%"><asp:Image ID="Image3" runat="server" Height="78px" ImageUrl="~/images/registered_1_1/2.PNG" Width="261px" /></td>
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
    <h3>
     <table style="width:100%;">
          <tr>
              <td style="width: 15%"> <asp:Image ID="Image10" runat="server" ImageUrl="~/images/registered_1_1/check1.png" Visible="False" />
      <asp:Image ID="Image11" runat="server" ImageUrl="~/images/registered_1_1/checked1.png" /></td>
              <td align="center" style="width: 70%">資格者証&nbsp;
                                <asp:Label ID="Label2" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                    Text="※必須"></asp:Label>
                                    <asp:Label ID="image_Label1" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    </td>
              <td style="width: 15%">
                                <asp:HyperLink id='hy1' runat="server" NavigateUrl="javascript:void(0);"
                                    Target="_blank" Font-Size="Small" Font-Underline="False">詳しく見る</asp:HyperLink>
                                                </td>
          </tr>
      </table>
    </h3>
  <div>
    <p>
        <table style="width:100%;">
            <tr>
                <td style="width: 80%"><asp:Image ID="Image12" runat="server" Height="78px" ImageUrl="~/images/registered_1_1/1.PNG" Width="261px" /></td>
                <td align="center" style="width: 20%">
                    <label class="file-upload">
            <span style="width: 8em;">画像を登録</span>
            <asp:FileUpload ID="fuDocument2" runat="server" onchange="UploadFile2(this);" />
             </label>
                             <br />
                                <asp:Button ID="btnUploadDoc2" Text="Upload" runat="server" OnClick="UploadDocument2" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"></asp:SqlDataSource>
                                <asp:Image ID="type2_Image" runat="server" Height="100px" Width="100px" />

                                 <br />

                                <br />
                </td>
            </tr>
        </table>
      </p>
  </div>
</div>
                                &nbsp;</td>
                            <td style="width:10%;">
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td align="center">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                    Text="報酬のお支払い先"></asp:Label>
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
                            <td style="position:relative;display:block">
                                <div style="position:absolute;background-color: #000;z-index: 999998; opacity: 0.8; width: 100%; height: 100%;text-align: center;">
                                   <br/><br/><br/><br/>
                                     <span style="color:white;">クローズドβ 版では直接ご当人同士でお支払いください。</span>
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

                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width: 20%" valign="top">
                                                <asp:Label ID="Label14" runat="server" Text="種類"></asp:Label>
                                                <br />
                                            </td>
                                            <td style="width: 40%" valign="top">
                                                <asp:RadioButtonList ID="bank_type_RadioButtonList" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem>銀行</asp:ListItem>
                                                    <asp:ListItem>ゆうちょ</asp:ListItem>
                                                </asp:RadioButtonList>
                                    <asp:Label ID="bank_type_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="width: 40%" valign="top">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width: 10%">(</td>
                                                        <td>
                                                <asp:RadioButtonList ID="bank_type_detail_RadioButtonList" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem>普通</asp:ListItem>
                                                    <asp:ListItem>当座</asp:ListItem>
                                                </asp:RadioButtonList>
                                                        </td>
                                                        <td style="width: 10%">)</td>
                                                    </tr>
                                                </table>
                                    <asp:Label ID="bank_type_detail_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="Label15" runat="server" Text="金融機関名"></asp:Label>
                                                <br />
                                            </td>
                                            <td valign="top">
                                <asp:TextBox ID="bank_name_TextBox" runat="server" Width="90%" Wrap="False" placeholder="例) はてな銀行"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                    <asp:Label ID="bank_name_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td valign="top">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width: 35%">
                                                <asp:Label ID="Label18" runat="server" Text="支店名"></asp:Label>
                                                        </td>
                                                        <td style="width: 65%">
                                <asp:TextBox ID="bank_name_detail_TextBox" runat="server" Width="100%" Wrap="False" placeholder="例) 新宿支店"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                    <asp:Label ID="bank_name_detail_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="Label16" runat="server" Text="口座番号"></asp:Label>
                                                <br />
                                            </td>
                                            <td colspan="2" valign="top">
                                <asp:TextBox ID="bank_number_TextBox" runat="server" Width="100%" Wrap="False" placeholder="番号をハイフン無しで記入"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                    <asp:Label ID="bank_number_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="Label17" runat="server" Text="名義人"></asp:Label>
                                                <br />
                                            </td>
                                            <td colspan="2" valign="top">
                                <asp:TextBox ID="bank_person_TextBox" runat="server" Width="100%" Wrap="False" placeholder="例) タナカ トーリム             ※全角カタカナ"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
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
        <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
    </form>
</body>
</html>
