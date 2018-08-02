<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Chat.aspx.cs" Inherits="Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
     <meta name="viewport" content="width=device-width, initial-scale=1"><meta name="description"><meta name="author">
        <script src="Scripts/jquery-1.12.4.js"></script>
        <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/bootstrap-theme.css" rel="stylesheet" />
    <!-- Magnific Popup core CSS file -->
<link rel="stylesheet" href="css/magnific-popup.css">
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css"/>
    <link rel="stylesheet" href="css/jquery-ui.css">
     <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/chatroom.css">
        <link rel="stylesheet" href="css/jquery.fileupload.css">


    <!--Reference the SignalR library. -->
    <script src="Scripts/bootstrap.min.js"></script>





    <script>
        //$(function () {
        //    $('.image-link').magnificPopup({
        //        type: 'image',
        //        mainClass: 'mfp-with-zoom', // this class is for CSS animation below

        //        zoom: {
        //            enabled: true, // By default it's false, so don't forget to enable it

        //            duration: 300, // duration of the effect, in milliseconds
        //            easing: 'ease-in-out', // CSS transition easing function

        //            // The "opener" function should return the element from which popup will be zoomed in
        //            // and to which popup will be scaled down
        //            // By defailt it looks for an image tag:
        //            opener: function (openerElement) {
        //                // openerElement is the element on which popup was initialized, in this case its <a> tag
        //                // you don't need to add "opener" option if this code matches your needs, it's defailt one.
        //                return openerElement.is('img') ? openerElement : openerElement.find('img');
        //            }
        //        }

        //    });

        //    $('.zoom-gallery').each(function () { // the containers for all your galleries
        //        $(this).magnificPopup({
        //            delegate: 'a',
        //            type: 'image',
        //            closeOnContentClick: false,
        //            closeBtnInside: false,
        //            mainClass: 'mfp-with-zoom mfp-img-mobile',
        //            image: {
        //                verticalFit: true,
        //                titleSrc: function (item) {
        //                    return item.el.attr('title') + ' &middot; <a class="image-source-link" href="' + item.el.attr('data-source') + '" target="_blank">image source</a>';
        //                }
        //            },
        //            gallery: {
        //                enabled: true
        //            },
        //            zoom: {
        //                enabled: true,
        //                duration: 300, // don't foget to change the duration also in CSS
        //                opener: function (element) {
        //                    return element.find('img');
        //                }
        //            }
        //        });
        //    });


        //});

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

        $(window).load(function () {
            var param1 = "<%= Session["id"]%>".toString();
            if (param1 != "") {
                $.ajax({
                    type: "POST",
                    url: "Chat.aspx/count_list",
                    data: "{param1: '" + param1 + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (result) {
                        //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                        $('#new_state_count').empty();
                        $('#new_state_count').append('&nbsp;<span style="color:white;">' + result.d[0] + '</span>&nbsp;');
                        if (result.d[0] != "0") {
                            $('#new_state_count').show();
                        }
                        $('#chat_count').empty();
                        $('#chat_count').append('&nbsp;<span style="color:white;">' + result.d[1] + '</span>&nbsp;');
                        if (result.d[1] != "0") {
                            $('#chat_count').show();
                        }
                        $('#friend_count').empty();
                        $('#friend_count').append('&nbsp;<span style="color:white;">' + result.d[2] + '</span>&nbsp;');
                        if (result.d[2] != "0") {
                            $('#friend_count').show();
                        }
                        //console.log(result.d);





                        //window.location.href = "Date_Calendar_success.aspx";
                    },
                    error: function (result) {
                        //console.log(result.Message);
                        //alert(result.d);
                    }
                });
            }


        });
        function new_state_notice() {
            $('#new_state_count').hide();
            var param1 = "<%= Session["id"]%>".toString();
            $.ajax({
                type: "POST",
                url: "Chat.aspx/new_state_list",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#dlg-body1').empty();
                    $('#dlg-body1').append(result.d);
                    //console.log(result.d);

                    var whitebg = document.getElementById("white-background");
                    var dlg = document.getElementById("dlgbox1");
                    whitebg.style.display = "block";
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 10 + "px";



                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        ///scroll new state list
        $(function () {
            $('#dlg-body1').on('scroll', function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    var param1 = "<%= Session["id"]%>".toString();
                    $.ajax({
                        type: "POST",
                        url: "Chat.aspx/new_state_notice_list_scroll",
                        data: "{param1: '" + param1 + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result

                            $('#dlg-body1').append(result.d);

                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }
            })
        });

        function dlgLogin_new_state_notice_close() {

            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox1");
            whitebg.style.display = "none";
            dlg.style.display = "none";
        }
        function new_state_notice_click(click_id) {
            var checkclick_str = "";
            var cut = click_id.indexOf('_');
            checkclick_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            window.location.href = "main_status.aspx?_status=newstatus_" + checkclick_str;
        }
        function new_state_big_notice_click(click_id) {
            var checkclick_str = "";
            var cut = click_id.indexOf('_');
            checkclick_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            window.location.href = "main_status.aspx?_status_big=newstatusbig_" + checkclick_str;
        }

        //chat room notice
        function chat_notice() {
            $('#chat_count').hide();
            var param1 = "<%= Session["id"]%>".toString();
            $.ajax({
                type: "POST",
                url: "Chat.aspx/chat_notice_list",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#dlg-body').empty();
                    $('#dlg-body').append(result.d);
                    //console.log(result.d);

                    var whitebg = document.getElementById("white-background");
                    var dlg = document.getElementById("dlgbox");
                    whitebg.style.display = "block";
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 10 + "px";



                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function dlgLogin_chat_notice_close() {

            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox");
            whitebg.style.display = "none";
            dlg.style.display = "none";
        }
        function chat_notice_click(click_id) {
            var checkclick_str = "";
            var cut = click_id.indexOf('_');
            checkclick_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            window.location.href = "Chat.aspx?_chat=" + checkclick_str;
        }
        //friend notice list
        function friend_notice() {
            $('#friend_count').hide();
            var param1 = "<%= Session["id"]%>".toString();
            $.ajax({
                type: "POST",
                url: "Chat.aspx/friend_notice_list",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#dlg-body_friend').empty();

                    //友達リクエスト
                    $('#dlg-body_friend').append("<table width='100%'><tr><td width='30%'><span>友達リクエスト</span></td><td width='70%'></td><tr></table><hr/>");
                    $('#dlg-body_friend').append(result.d);

                    //var whitebg = document.getElementById("white-background");
                    //var dlg = document.getElementById("dlgbox_firend");
                    //whitebg.style.display = "block";
                    //dlg.style.display = "block";

                    //var winWidth = window.innerWidth;
                    //var winHeight = window.innerHeight;

                    //dlg.style.left = 0 + "px";
                    //dlg.style.top = winHeight / 10 + "px";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
            $.ajax({
                type: "POST",
                url: "Chat.aspx/search_friend_notice_list",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //$('#dlg-body_friend').empty();

                    //友達リクエスト
                    $('#dlg-body_friend').append("<table width='100%'><tr><td width='30%'><span>知り合いかも</span></td><td width='70%'></td><tr></table><hr/>");
                    $('#dlg-body_friend').append(result.d);
                    //console.log(result.d);

                    var whitebg = document.getElementById("white-background");
                    var dlg = document.getElementById("dlgbox_firend");
                    whitebg.style.display = "block";
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 10 + "px";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function dlgLogin_fri_close() {

            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox_firend");
            whitebg.style.display = "none";
            dlg.style.display = "none";
        }
        function dlgcheckfriend(click_id) {
            var checkfriend_str = "";
            var cut = click_id.indexOf('_');
            checkfriend_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "Chat.aspx/friend_notice_check",
                data: "{param1: '" + checkfriend_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    $("#" + click_id).removeClass("file-upload");
                    $("#" + click_id).addClass("nocheckfriend");
                    $("#" + click_id).prop('disabled', true);

                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function dlgcheckfriend_del(click_id) {
            var checkfriend_str = "";
            var cut = click_id.indexOf('_');
            checkfriend_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "Chat.aspx/friend_notice_check_del",
                data: "{param1: '" + checkfriend_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    $("#" + click_id).removeClass("file-upload");
                    $("#" + click_id).addClass("nocheckfriend");
                    $("#" + click_id).prop('disabled', true);

                    $("#friendcheck_" + checkfriend_str).removeClass("file-upload");
                    $("#friendcheck_" + checkfriend_str).addClass("nocheckfriend");
                    $("#friendcheck_" + checkfriend_str).prop('disabled', true);

                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function dlgcheckfriend_donotfind(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var checkfriend_str = "";
            var cut = click_id.indexOf('_');
            checkfriend_str = click_id.substr(cut + 1, click_id.length - cut - 1);

            var cut1 = checkfriend_str.indexOf('_');
            var checkfriend_str1 = checkfriend_str.substr(cut1 + 1, checkfriend_str.length - cut1 - 1);
            var checkfriend_str2 = checkfriend_str.substr(0, cut1);

            //console.log(checkfriend_str1);
            //console.log(checkfriend_str2);

            $.ajax({
                type: "POST",
                url: "Chat.aspx/friend_notice_donotfind",
                data: "{param1: '" + param1 + "',param2: '" + checkfriend_str1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    $("#friendpanel_" + checkfriend_str2).remove();
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function dlgcheckfriend_addfri(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var checkfriend_str = "";
            var cut = click_id.indexOf('_');
            checkfriend_str = click_id.substr(cut + 1, click_id.length - cut - 1);

            var cut1 = checkfriend_str.indexOf('_');
            var checkfriend_str1 = checkfriend_str.substr(cut1 + 1, checkfriend_str.length - cut1 - 1);
            var checkfriend_str2 = checkfriend_str.substr(0, cut1);

            //console.log(checkfriend_str1);
            //console.log(checkfriend_str2);

            $.ajax({
                type: "POST",
                url: "Chat.aspx/friend_notice_addfind",
                data: "{param1: '" + param1 + "',param2: '" + checkfriend_str1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    $("#friendpanel_" + checkfriend_str2).remove();
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });

        }
        function dlgLogin_tofri_close() {
            var dlg = document.getElementById("dlgbox_tofriend_list");
            dlg.style.display = "none";
            var dlg1 = document.getElementById("dlgbox_firend");
            dlg1.style.display = "block";
            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg1.style.left = 0 + "px";
            dlg1.style.top = winHeight / 10 + "px";
        }
        function check_tofriend_list(clickid) {
            var param1 = "<%= Session["id"]%>".toString();
            var toget_friend_str = "";
            var cut = clickid.indexOf('_');
            toget_friend_str = clickid.substr(cut + 1, clickid.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "Chat.aspx/toget_friend_list",
                data: "{param1: '" + param1 + "',param2: '" + toget_friend_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    $('#dlg-body_tofriend_list').empty();
                    $('#dlg-body_tofriend_list').append(result.d);
                    //console.log(result.d);

                    var dlg = document.getElementById("dlgbox_tofriend_list");
                    dlg.style.display = "block";

                    var winWidth = window.innerWidth;
                    var winHeight = window.innerHeight;

                    dlg.style.left = 0 + "px";
                    dlg.style.top = winHeight / 10 + "px";

                    var dlg1 = document.getElementById("dlgbox_firend");
                    dlg1.style.display = "none";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        ///
        $(function () {
            $('#dlg-body_friend').on('scroll', function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    var param1 = "<%= Session["id"]%>".toString();
                    $.ajax({
                        type: "POST",
                        url: "Chat.aspx/search_friend_notice_list_scroll",
                        data: "{param1: '" + param1 + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result

                            $('#dlg-body_friend').append(result.d);

                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }
            })
        });
        //friend end
        function gotomy() {
            window.location.href = "user_home.aspx";
        }

     </script>
     <style type="text/css">
        #new_state_count
        {
            display:none;
        }
        #chat_count{
            display:none;
        }
        #friend_count{
            display:none;
        }
         body
        {
            font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;background-color:#E9EBEE;
        }
        .container {
            background-color: #E9EBEE;
            padding: 20px;
        }
    </style>
</head>
<body>
      <div id="white-background">
        </div>
     <div id="dlgbox_tofriend_list" class="dlg">
            <div id="dlg-header_tofriend_list" class="dlgh">共通の友達</div>
            <div id="dlg-body_tofriend_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_tofriend_list" class="dlgf">
                <input id="Button4" type="button" value="閉じる" onclick="dlgLogin_tofri_close()" class="file-upload"/>
            </div>
        </div>
                <div id="dlgbox_firend" class="dlg">
            <div id="dlg-header_friend" class="dlgh">友達承認</div>
            <div id="dlg-body_friend" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_friend" class="dlgf">
                <input id="Button2" type="button" value="閉じる" onclick="dlgLogin_fri_close()" class="file-upload"/>
            </div>
        </div>
        <div id="dlgbox">
            <div id="dlg-header">メッセ一ジ</div>
            <div id="dlg-body" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer">
                <input id="Button3" type="button" value="閉じる" onclick="dlgLogin_chat_notice_close()" class="file-upload"/>
            </div>
        </div>


                 <div id="dlgbox1">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer1">
                <input id="Button13" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload"/>
            </div>
        </div>
    <div id="header">
                    <table style="width:100%;height:100%">
                        <tr>
                            <td align="left" width="15%">

                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                        <td class="icon_left"><span onclick="ShowSidebarLeft()"><i class="fa fa-bars" style="font-size:30px; color: white;cursor: pointer;"></i></span></td>
                                    <td class="rin"><asp:Image id="Label_logo" style="height:auto;cursor:pointer;margin-left:3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
<!--hehe-->

                                    </tr>
                                </table>
                            </td>
                            <td width="55%">
                                &nbsp;</td>
                            <td class="header-right">
                                <table style="width:100%; height: 50%">
                                    <tr>
                                        <td class="topnav_right">
                                            <button type="button" onclick="gotomy()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">マイページ</button>
                                        </td>
                                         <td class="topnav_right">
                                             <div style="position:relative;">
                                                <div id="friend_count" style="border-radius:6px 6px 6px 0px;background-color:red;min-width:20px;min-height:20px;position:absolute;top:-10px;right: 0px;">&nbsp;<span style="color:white;">1</span>&nbsp;</div>
                                            <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">友達申請</button>
                                                 </div>
                                        </td>
                                        <td class="topnav_right">
                                             <div style="position:relative;">
                                                <div id="chat_count" style="border-radius:6px; background-color:red;min-width:20px;min-height:20px;position:absolute;top:-10px;right: 0px;">&nbsp;<span style="color:white;">1</span>&nbsp;</div>
                                            <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">メッセ一ジ</button>
                                                 </div>
                                        </td>
                                        <td class="topnav_right">
                                             <div style="position:relative;">
                                                <div id="new_state_count" style="border-radius:6px; background-color:red;min-width:20px;min-height:20px;position:absolute;top:-10px;right: 0px;">&nbsp;<span style="color:white;">1</span>&nbsp;</div>
                                            <button type="button" onclick="new_state_notice()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">お知らせ</button>
                                                 </div>
                                        </td>
                                        <td class="topnav_right">

                                   <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">ヘルプ</button>

                                        </td>
                                         <td class="icon_right"><i class="fa fa-bell-o" style="font-size:30px; color: white;cursor: pointer;margin-right:10px;" onclick="ShowSidebarRight()"></i>
                                         </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div>
     <div id="div_sidebar_right">
            <div id="sidebar_right">
                <table style="width: 100%; margin-top: 4em;">
                    <tbody>
                        <tr><td >

                            <button type="button" onclick="gotomy()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">マイページ</button>
                        </td></tr>
                        <tr><td >

                            <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">友達申請</button>
                        </td></tr>
                        <tr><td>

                             <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">メッセ一ジ</button>
                        </td> </tr>
                        <tr><td>

                            <button type="button" onclick="new_state_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">お知らせ</button>
                        </td> </tr>
                        <tr><td>

                           <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; margin-top: 15px; background-color: transparent; color: #000000; cursor: pointer;">ヘルプ</button>
                        </td></tr>



                    </tbody>
                </table>
            </div><!--End sidebar_right-->
        </div><!--End div_sidebar_right-->


    <div class="container">
        <div id="discussion" style="height:340px;overflow: scroll;margin-top:70px;overflow-x: hidden;">
        </div>
        <textarea style="width:100%;border-radius:10px;" id="content" name="Text1" cols="40" rows="4"></textarea><br />
        <button style="width:100%;height:40px;font-size:20px;border-radius: 10px;background-color: pink;" id="btn">送信する</button>
    </div>

    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="main_content" runat="server"></asp:Panel>
    </div>
                                <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;position: absolute; top: 0px;">
</div>
        <asp:Panel ID="javaplace" runat="server">
                </asp:Panel>
    </form>
    <div class="selection_bubble_root" style="display: none;"></div>
    <script>

        $(document).ready(function () {
            if ($(window).width() < 680) {


                var h1 = $('#typing_box').height();
                var h2 = $('#friend_name').height();
                var h3 = $('#header').height();
                var h = $(window).height() - h1 - h2 - h3 - 22;
                $('#chat_Panel2').height(h);

            }
        });


        function ShowSidebarRight() {
            var div = document.getElementById('div_sidebar_right');
            if (div.style.display == 'block') {
                div.setAttribute("style", "display:none");

            }
            else {
                div.setAttribute("style", "display:block;");
            }
        }

        $(".small_talk_div_leave").on("click", function () {

            if ($(window).width() < 680) {
                $('#list').css({
                    display: 'none',
                });;
                $('#contact_list').css({
                    display: 'none',
                });;
                $('#friend_name').css({
                    display: 'block',
                    width: '100%',
                    height: '100%'
                });;
                $('#chat_window').css({
                    display: 'block',
                    height: '100%',
                    width: '100%'
                });;

            }


        });

</script>
</body>
</html>
