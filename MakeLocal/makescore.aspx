<%@ Page Language="C#" AutoEventWireup="true" CodeFile="makescore.aspx.cs" Inherits="makescore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
	<!-- Bootstrap Core CSS -->
	<link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
	<link rel="stylesheet" type="text/css" href="css/bootstrap-theme.min.css">
	<link rel="stylesheet" type="text/css" href="js/agency.min.js">

	<!-- Custom CSS -->
	<link rel="stylesheet" type="text/css" href="css/our/style_mypage_profile.css">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css"/>

     <style type="text/css">
        #header{
background-color:#ea9494;
height:80px;
text-align:center;
line-height:70px;
}
        body
        {
            font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif !important;
 background-color: #E9EBEE;
font-style: normal !important;
        }
    </style>
    <script>
        function report_create_success(click_id) {
            var report_str = "";
            var cut = click_id.indexOf('_');
            report_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "makescore.aspx/report_build_report",
                data: "{param1: '" + report_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    window.location.href = "user_home.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
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
    	<div class="container">
		<div class="row">
			<div class="col-md-7  col-md-offset-2">
				<div class="row text_style lead" >報告書</div><!--label-->
                <div class="row">
					<div class="col-md-1 text_style"></div>
                    <div class="col-md-1 text_style"></div>
                    <div class="col-sm-8 col-md-8">
                        <asp:Panel ID="user_photo" runat="server"></asp:Panel>
                    </div>
                    <div class="col-md-1 text_style"></div>
                    <div class="col-md-1 text_style"></div>
				</div><!---->
				<div class="row">
					<div class="col-sm-4 text_style"><b>預かり日時</b></div>
					<div class="col-sm-8 col-md-8">


                             <asp:Panel ID="sandboxcontainer" runat="server"></asp:Panel>

					</div>
				</div><!--end caring date-->
				<div class="row">
					<div class="col-md-4 text_style" ></div>
					<div class="col-md-8">

					</div>
				</div><!--row 3 end-->
				<div class="row">
					<div class="col-md-4 text_style">預かり内容</div><!---->
					<div class="col-sm-8 col-md-8">

                             <asp:Panel ID="report_content" runat="server"></asp:Panel>


					</div>
				</div><!---->
				<div class="row">
					<div class="col-md-4 text_style">預かり時の様子</div>
                    <div class="col-md-8">
                        <asp:Panel ID="report_list" runat="server"></asp:Panel>
                    </div>
				</div><!---->
				<div class="row" >
					<div class="col-md-6 col-md-offset-3" style="height: 50px;">
                        <asp:Panel ID="button_pan" runat="server"></asp:Panel>
					</div>
				</div><!---->
			</div>
		</div>
	</div><!--end main body-->
    </form>
</body>
</html>
