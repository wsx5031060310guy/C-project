<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manager_page.aspx.cs" Inherits="manager_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title> Manager</title>

	<!-- Google Fonts -->
	<link href='//fonts.googleapis.com/css?family=Roboto+Slab:400,100,300,700|Lato:400,100,300,700,900' rel='stylesheet' type='text/css'>

	<link rel="stylesheet" href="css/animate_forlogin.css">
	<!-- Custom Stylesheet -->
	<link rel="stylesheet" href="css/style_forlogin.css">

	<script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <style>
        body {

       font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;
   }
        </style>
</head>
<body>
    <div class="container">
		<div class="top">
			<h1 id="title" class="hidden"><span id="logo">MakeLocal <span>mamaro</span></span></h1>
		</div>
		<div class="login-box animated fadeInUp">
			<div class="box-header">
				<h2>バックグラウンド管理</h2>
			</div>
			<label for="username">メールアドレス</label>
			<br/>
			<input type="text" id="username">
			<br/>
			<label for="password">パスワード</label>
			<br/>
			<input type="password" id="password">
			<br/>
            <input id="loginbutton" type="button" value="ログイン"/>
			<br/>
            <span id="result_text" style="font-size: large; font-weight: bold; color: #FF0000"></span>
            <br/>
		</div>
	</div>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
    <script>

        $(document).ready(function () {
            $('#logo').addClass('animated fadeInDown');
            $("input:text:visible:first").focus();

            $("#loginbutton").click(function () {
                $('#result_text').text("");
                if ($('#username').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() != "" && $('#password').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() != "") {
                    $.ajax({
                        type: "POST",
                        url: "manager_page.aspx/check_login",
                        data: "{param1: '" + $('#username').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() + "' , param2 :'" + $('#password').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            $('#result_text').text(result.d);
                            alert(result.d);
                            var str = "<%= Session["manager"]%>".toString();

                                    if (str != "" || str != null) {
                                        window.location.href = "check_post.aspx";
                                    }
                                },
                                error: function (result) {
                                    //console.log(result.Message);
                                    $('#result_text').text(result.d);
                                }
                            });
                        } else {
                            $('#result_text').text("未入力");
                        }


            });



        });
        $('#username').focus(function () {
            $('label[for="username"]').addClass('selected');
        });
        $('#username').blur(function () {
            $('label[for="username"]').removeClass('selected');
        });
        $('#password').focus(function () {
            $('label[for="password"]').addClass('selected');
        });
        $('#password').blur(function () {
            $('label[for="password"]').removeClass('selected');
        });
</script>
</html>
