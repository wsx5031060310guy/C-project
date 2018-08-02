<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_home_friend.aspx.cs" Inherits="user_home_friend" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link rel="stylesheet" href="css/jquery.fileupload.css">
    <!-- semantic core CSS file -->
    <link rel="stylesheet" type="text/css" href="css/semantic.css">
    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/file-upload_fb.css" type="text/css" />
    <link href="css/MonthPicker.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/tipped.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="css/jssocials.css" />
    <link rel="stylesheet" type="text/css" href="css/jssocials-theme-flat.css" />
    <!-- Magnific Popup core CSS file -->
    <link rel="stylesheet" href="css/magnific-popup.css">

    <link rel="stylesheet" href="css/style.css" />
    <link rel="stylesheet" href="css/user_home_friend.css">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja"></script>
    <script src="Scripts/jquery-1.12.4.js"></script>

    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>



    <script src="js/semantic.js"></script>


    <!-- Magnific Popup core JS file -->
    <script src="js/jquery.magnific-popup.js"></script>

    <script src="Scripts/jquery-ui.js"></script>



    <script src="js/MonthPicker_jp.js"></script>
    <script src="Scripts/datepicker-ja.js"></script>

    <%--    <link rel="stylesheet" href="css/darktooltip.css">
        <script src="Scripts/jquery.darktooltip.js"></script>--%>



    <script src="Scripts/tipped.js"></script>
    <script src="Scripts/freewall.js"></script>
    <script src="js/jquery.nicescroll.js"></script>


    <script src="//connect.facebook.net/ja_JP/sdk.js"></script>
    <script>

        window.fbAsyncInit = function () {
            FB.init({
                appId: '',
                xfbml: true,
                version: 'v2.8'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/ja_JP/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));

        function postToWallUsingFBUi(to_link, to_picture, to_message) {
            //console.log(to_picture);
            FB.ui(
{
    method: 'feed',
    name: '地域のいい情報をGETしました！',
    link: to_link,
    picture: to_picture,
    caption: '',
    description: to_message,
    message: ''
}
);
        }
    </script>
    <script>
        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });
            $("body").niceScroll();

            //search friend
            var element = document.getElementById('txtSearch_f');
            if (typeof (element) != 'undefined' && element != null) {
                // exists.
                $("#<%=txtSearch_f.ClientID %>").autocomplete({
                     source: function (request, response) {
                         var param1 = "<%= Session["friend_id"]%>".toString();
                         $.ajax({
                             url: "test_search.aspx/Getsearch_friend",
                             data: "{ 'prefix': '" + request.term + "','who':'" + param1 + "'}",
                             dataType: "json",
                             type: "POST",
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(';')[0],
                                         val: item.split(';')[1],
                                         icon: item.split(';')[2]
                                     }
                                 }))
                             },
                             error: function (response) {
                                 alert(response.responseText);
                             },
                             failure: function (response) {
                                 alert(response.responseText);
                             }
                         });
                     },
                     select: function (e, i) {
                         window.location.href = "user_home_friend.aspx?=" + i.item.val;
                     },
                     minLength: 1
                 });

                     $("#<%=txtSearch_f.ClientID %>").data("ui-autocomplete")._renderItem = function (ul, item) {

                     var $li = $('<li>'),
                         $img = $('<img>');

                     $img.attr({
                         src: item.icon,
                         alt: item.label,
                         width: "50px",
                         height: "50px"
                     });
                     $li.attr('data-value', item.label);
                     $li.append('<div>');
                     $li.find('div').append($img).append(item.label);

                     return $li.appendTo(ul);
                 };
             }

         });

         function dlgLogin() {
             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox");
             whitebg.style.display = "none";
             dlg.style.display = "none";
             var dlg1 = document.getElementById("dlgbox1");
             dlg1.style.display = "none";
             var dlg2 = document.getElementById("dlgbox_login");
             dlg2.style.display = "none";
             var dlg3 = document.getElementById("second_login_d");
             dlg3.style.display = "none";
         }

         function showDialog() {
             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox");
             whitebg.style.display = "block";
             dlg.style.display = "block";

             var winWidth = window.innerWidth;
             var winHeight = window.innerHeight;

             dlg.style.left = 0 + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
         function showDialog_m() {
             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox1");
             whitebg.style.display = "block";
             dlg.style.display = "block";

             var winWidth = window.innerWidth;
             var winHeight = window.innerHeight;

             dlg.style.left = 0 + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
         function showDialog_login() {
             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox_login");
             whitebg.style.display = "block";
             dlg.style.display = "block";

             var winWidth = window.innerWidth;
             var winHeight = window.innerHeight;

             dlg.style.left = 0 + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
        //new state message notice
         $(window).load(function () {
             var param1 = "<%= Session["id"]%>".toString();
             if (param1 != "") {
                 $.ajax({
                     type: "POST",
                     url: "user_home_friend.aspx/count_list",
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
                 url: "user_home_friend.aspx/new_state_list",
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
                        url: "user_home_friend.aspx/new_state_notice_list_scroll",
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
                 url: "user_home_friend.aspx/chat_notice_list",
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
                url: "user_home_friend.aspx/friend_notice_list",
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
                url: "user_home_friend.aspx/search_friend_notice_list",
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
                url: "user_home_friend.aspx/friend_notice_check",
                data: "{param1: '" + checkfriend_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    $("#" + click_id).removeClass("file-upload1");
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
                url: "user_home_friend.aspx/friend_notice_check_del",
                data: "{param1: '" + checkfriend_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    $("#" + click_id).removeClass("file-upload1");
                    $("#" + click_id).addClass("nocheckfriend");
                    $("#" + click_id).prop('disabled', true);

                    $("#friendcheck_" + checkfriend_str).removeClass("file-upload1");
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
                url: "user_home_friend.aspx/friend_notice_donotfind",
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
                url: "user_home_friend.aspx/friend_notice_addfind",
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
                url: "user_home_friend.aspx/toget_friend_list",
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
                         url: "user_home_friend.aspx/search_friend_notice_list_scroll",
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

         function blike(click_id) {
             var param1 = "<%= Session["id"]%>".toString();
             var like_str = "";
             var cut = click_id.indexOf('t');
             like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var yn = 1;
             $.ajax({
                 type: "POST",
                 url: "user_home_friend.aspx/like_or_not",
                 data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: true,
                 cache: false,
                 success: function (result) {
                     //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                     console.log(result.d);
                     //window.location.href = "Date_Calendar_success.aspx";
                 },
                 error: function (result) {
                     //console.log(result.Message);
                     //alert(result.d);
                 }
             });
         }
         function like(click_id) {
             var param1 = "<%= Session["id"]%>".toString();
             var like_str = "";
             var cut = click_id.indexOf('t');
             like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var yn = 0;
             $.ajax({
                 type: "POST",
                 url: "user_home_friend.aspx/like_or_not",
                 data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: true,
                 cache: false,
                 success: function (result) {
                     //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                     console.log(result.d);
                     //window.location.href = "Date_Calendar_success.aspx";
                 },
                 error: function (result) {
                     //console.log(result.Message);
                     //alert(result.d);
                 }
             });
         }

         function like_who_answer(click_id) {
             var param1 = "<%= Session["id"]%>".toString();
             var like_str = "";
             var cut = click_id.indexOf('_');
             like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var yn = 0;
             $.ajax({
                 type: "POST",
                 url: "user_home_friend.aspx/like_who_ans",
                 data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: true,
                 cache: false,
                 success: function (result) {
                     //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                     //console.log(result.d);
                     $('#' + click_id).css("color", "#4183C4");
                     $('#' + click_id).attr("onclick", "blike_who_answer(this.id)");
                     var cc = parseInt($('#likecount' + like_str).text());
                     cc -= 1;
                     $('#likecount' + like_str).text('');
                     $('#likecount' + like_str).text(cc);
                 },
                 error: function (result) {
                     //console.log(result.Message);
                     //alert(result.d);
                 }
             });
         }

         function blike_who_answer(click_id) {
             var param1 = "<%= Session["id"]%>".toString();
            var like_str = "";
            var cut = click_id.indexOf('_');
            like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            var yn = 1;
            $.ajax({
                type: "POST",
                url: "user_home_friend.aspx/like_who_ans",
                data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);

                    $('#' + click_id).css("color", "#D84C4B");
                    $('#' + click_id).attr("onclick", "like_who_answer(this.id)");
                    var cc = parseInt($('#likecount' + like_str).text());
                    cc += 1;
                    $('#likecount' + like_str).text('');
                    $('#likecount' + like_str).text(cc);
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }




        function slike_who_answer(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var like_str = "";
            var cut = click_id.indexOf('_');

            like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            var cut1 = like_str.indexOf('_');
            like_str = like_str.substr(0, cut1);
            var yn = 0;
            $.ajax({
                type: "POST",
                url: "user_home_friend.aspx/like_who_ans",
                data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);

                    $('#' + click_id).css("color", "#4183C4");
                    $('#' + click_id).attr("onclick", "blike_who_answer(this.id)");
                    var cc = parseInt($('#likecount' + like_str).text());
                    cc -= 1;
                    $('#likecount' + like_str).text('');
                    $('#likecount' + like_str).text(cc);
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }

        function sblike_who_answer(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var like_str = "";
            var cut = click_id.indexOf('_');
            like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            var cut1 = like_str.indexOf('_');
            like_str = like_str.substr(0, cut1);
            var yn = 1;
            $.ajax({
                type: "POST",
                url: "user_home_friend.aspx/like_who_ans",
                data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);

                    $('#' + click_id).css("color", "#D84C4B");
                    $('#' + click_id).attr("onclick", "like_who_answer(this.id)");
                    var cc = parseInt($('#likecount' + like_str).text());
                    cc += 1;
                    $('#likecount' + like_str).text('');
                    $('#likecount' + like_str).text(cc);
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }


        function who_answer(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var like_str = "";
            var cut = click_id.indexOf('_');
            like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "user_home_friend.aspx/who_ans",
                data: "{param1: '" + param1 + "' , param2 :'" + like_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    $('#whowanttoanswer_' + like_str).empty();
                    $('#whowanttoanswer_' + like_str).append(result.d);
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }

        function small_sendmessage(e, click_id) {
            if (e.which == 13 || e.keyCode == 13) {
                e.preventDefault();

                var param1 = "<%= Session["id"]%>".toString();

                var send_str = "";
                var send_img = "";
                var cut = click_id.indexOf('l');
                send_str = click_id.substr(cut + 1, click_id.length - cut - 1);
                cut = send_str.indexOf('_');
                send_img = send_str.substr(cut + 1, send_str.length - cut - 1);
                send_str = send_str.substr(0, cut);
                //var imgpath = $('#make-image' + send_img).prop('src');
                //action
                webaction(send_str, 3);
                var imgpath = $('#make-image_' + send_img).attr("src");

                var mess = $('#' + click_id).val().replace("'", "").replace('"', "").replace("`", "").trim();
                if (mess != "") {

                    $.ajax({
                        type: "POST",
                        url: "user_home_friend.aspx/small_sendtopost",
                        data: "{param1: '" + param1 + "' , param2 :'" + send_str + "',param3 :'" + mess + "',param4 :'" + imgpath + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            console.log(result.d);
                            //window.location.href = "Date_Calendar_success.aspx";
                            //$('#make-image' + send_img).attr("src") = "";
                            //$('#' + click_id).val() = "";


                            window.location.href = "user_home_friend.aspx";
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }
            }
        }



        function sendmessage(e, click_id) {
            if (e.which == 13 || e.keyCode == 13) {
                e.preventDefault();

                var param1 = "<%= Session["id"]%>".toString();

                var send_str = "";
                var send_img = "";
                var cut = click_id.indexOf('y');
                send_str = click_id.substr(cut + 1, click_id.length - cut - 1);
                cut = send_str.indexOf('_');
                send_img = send_str.substr(cut + 1, send_str.length - cut - 1);
                send_str = send_str.substr(0, cut);
                //var imgpath = $('#make-image' + send_img).prop('src');
                //action
                webaction(send_str, 3);
                var imgpath = $('#make-image' + send_img).attr("src");

                var mess = $('#' + click_id).val().replace("'", "").replace('"', "").replace("`", "").trim();
                if (mess != "") {

                    $.ajax({
                        type: "POST",
                        url: "user_home_friend.aspx/sendtopost",
                        data: "{param1: '" + param1 + "' , param2 :'" + send_str + "',param3 :'" + mess + "',param4 :'" + imgpath + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            console.log(result.d);
                            //window.location.href = "Date_Calendar_success.aspx";
                            //$('#make-image' + send_img).attr("src") = "";
                            //$('#' + click_id).val() = "";

                            window.location.href = "user_home_friend.aspx";
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }
            }
        }



        $(function () {

            $("#salutation").selectmenu({
                width: 150
            });
            $("#dialog").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 100
                },
                hide: {
                    effect: "explode",
                    duration: 100
                }
            });

            $('.image-link').magnificPopup({
                type: 'image',
                mainClass: 'mfp-with-zoom', // this class is for CSS animation below

                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it

                    duration: 300, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function

                    // The "opener" function should return the element from which popup will be zoomed in
                    // and to which popup will be scaled down
                    // By defailt it looks for an image tag:
                    opener: function (openerElement) {
                        // openerElement is the element on which popup was initialized, in this case its <a> tag
                        // you don't need to add "opener" option if this code matches your needs, it's defailt one.
                        return openerElement.is('img') ? openerElement : openerElement.find('img');
                    }
                }

            });

            $('.zoom-gallery').each(function () { // the containers for all your galleries
                $(this).magnificPopup({
                    delegate: 'a',
                    type: 'image',
                    closeOnContentClick: false,
                    closeBtnInside: false,
                    mainClass: 'mfp-with-zoom mfp-img-mobile',
                    image: {
                        verticalFit: true,
                        titleSrc: function (item) {
                            return item.el.attr('title') + ' &middot; <a class="image-source-link" href="' + item.el.attr('data-source') + '" target="_blank">image source</a>';
                        }
                    },
                    gallery: {
                        enabled: true
                    },
                    zoom: {
                        enabled: true,
                        duration: 300, // don't foget to change the duration also in CSS
                        opener: function (element) {
                            return element.find('img');
                        }
                    }
                });
            });




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

        function chat_forfriend() {
            var param1 = "<%= Session["id"]%>".toString();
              var param2 = "<%= Session["friend_id"]%>".toString();
              $.ajax({
                  type: "POST",
                  url: "user_home_friend.aspx/chatchat_friend",
                  data: "{param1: '" + param1 + "',param2: '" + param2 + "' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: true,
                  cache: false,
                  success: function (result) {
                      //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                      //console.log(result.d);
                      window.location.href = result.d;
                  },
                  error: function (result) {
                      //console.log(result.Message);
                      //alert(result.d);
                  }
              });
          }
          function check_like_list(clickid) {
              var like_str = "";
              var cut = clickid.indexOf('_');
              like_str = clickid.substr(cut + 1, clickid.length - cut - 1);
              $.ajax({
                  type: "POST",
                  url: "user_home_friend.aspx/like_list",
                  data: "{param1: '" + like_str + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: true,
                  cache: false,
                  success: function (result) {
                      //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                      //console.log(result.d);
                      $('#dlg-body_like_list').empty();
                      $('#dlg-body_like_list').append(result.d);
                      //console.log(result.d);

                      var whitebg = document.getElementById("white-background");
                      var dlg = document.getElementById("dlgbox_like_list");
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

          function dlgLogin_lik_close() {

              var whitebg = document.getElementById("white-background");
              var dlg = document.getElementById("dlgbox_like_list");
              whitebg.style.display = "none";
              dlg.style.display = "none";
          }
          //
          function report_mess(click_id) {
              var up_str = "";
              var cut = click_id.indexOf('_');
              up_str = click_id.substr(cut + 1, click_id.length - cut - 1);

              var whitebg = document.getElementById("white-background");
              var dlg = document.getElementById("dlgbox_report_" + up_str);
              whitebg.style.display = "block";
              dlg.style.display = "block";

              var winWidth = window.innerWidth;
              var winHeight = window.innerHeight;

              dlg.style.left = 0 + "px";
              dlg.style.top = winHeight / 10 + "px";
          }
          function dlgrecanel(click_id) {
              var up_str = "";
              var cut = click_id.indexOf('_');
              up_str = click_id.substr(cut + 1, click_id.length - cut - 1);

              var whitebg = document.getElementById("white-background");
              var dlg = document.getElementById("dlgbox_report_" + up_str);
              whitebg.style.display = "none";
              dlg.style.display = "none";
          }
          function dlgreport(click_id) {
              var str1 = "<%= Session["id"]%>".toString();
             var up_str = "";
             var cut = click_id.indexOf('_');
             up_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var check_select = true;
             $('#reportla_' + up_str).text("");
             if (document.querySelector('input[name="report_' + up_str + '"]:checked') == null) {
                 check_select = false;
                 $('#reportla_' + up_str).text("未入力");
             }
             if (check_select) {

                 $.ajax({
                     type: "POST",
                     url: "user_home_friend.aspx/report_bad",
                     data: "{param1: '" + str1 + "',param2: '" + up_str + "',param3:'" + document.querySelector('input[name="report_' + up_str + '"]:checked').value + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: true,
                     cache: false,
                     success: function (result) {
                         //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                         alert(result.d);
                         //console.log(result.d);

                         var whitebg = document.getElementById("white-background");
                         var dlg = document.getElementById("dlgbox_report_" + up_str);
                         whitebg.style.display = "none";
                         dlg.style.display = "none";
                     },
                     error: function (result) {
                         //console.log(result.Message);
                         //alert(result.d);
                     }
                 });
             }
         }
         //
        $(function () {
            var didScroll = true;
             var count = 10;
             $(window).scroll(function () {
                 if ($(window).scrollTop() + $(window).height() == $(document).height()) {
                     if (didScroll) {
                         didScroll = false;
                         //alert("bottom!");
                         count += 10;
                         display_PostProgress();
                         $('#javaplace_formap').empty();
                         $.ajax({
                             type: "POST",
                             url: "user_home_friend.aspx/search_new_post",
                             data: "{param1: '" + count + "'}",
                             contentType: "application/json; charset=utf-8",
                             dataType: "json",
                             async: true,
                             cache: false,
                             success: function (result) {
                                 //Successfully gone to the server and returned with the string result of the server side function do what you want with the result

                                 //console.log(count);
                                 //console.log(result.d);
                                 //window.location.href = "main.aspx";
                                 didScroll = true;
                                 $('#homepage_user').append(result.d);
                                 Hide_PostProgressBar();
                                 //var whitebg = document.getElementById("white-background");
                                 //var dlg = document.getElementById("dlgbox_report_" + up_str);
                                 //whitebg.style.display = "none";
                                 //dlg.style.display = "none";
                             },
                             error: function (result) {
                                 //console.log(result.Message);
                                 //alert(result.d);
                                 Hide_PostProgressBar();
                             }
                         });

                     }
                 }
             });
         });
         $(function () {
             $(document).tooltip();
         });

         function webaction(smid, type) {
             if (smid != "" && type != "") {
                 $.ajax({
                     type: 'POST',
                     url: 'user_home_friend.aspx/user_action',
                     data: "{param1: '" + smid + "' , param2 :'" + type + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: true,
                     cache: false,
                     success: function (result) {
                         //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                         //console.log(result.d);
                     },
                     error: function (result) {
                         //console.log(result.Message);
                         //alert(result.d);
                     }
                 });
             }
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
			.size320 {
				width: 280px;
				height: 280px;
			}
			.size44 {
				width: 100%;
				height: 100%;
			}
			.size22 {
				width: 50%;
				height: 50%;
			}
			.size14 {
				width: 25%;
				height: 100%;
			}
			.size41 {
				width: 100%;
				height: 25%;
			}
			.size24 {
				width: 50%;
				height: 100%;
			}
			.size42 {
				width: 100%;
				height: 50%;
			}
		</style>
</head>
<body>
    <script src="js/jquery.lazy.js"></script>
    <form id="form1" runat="server">
        <script src="Scripts/jssocials.js"></script>
        <div id="white-background">
        </div>
         <div id="dlgbox_tofriend_list" class="dlg">
            <div id="dlg-header_tofriend_list" class="dlgh">共通の友達</div>
            <div id="dlg-body_tofriend_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_tofriend_list" class="dlgf">
                <input id="Button5" type="button" value="閉じる" onclick="dlgLogin_tofri_close()" class="file-upload1"/>
            </div>
        </div>
        <div id="dlgbox_like_list" class="dlg">
            <div id="dlg-header_like_list" class="dlgh">いいね</div>
            <div id="dlg-body_like_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_like_list" class="dlgf">
                <input id="Button4" type="button" value="閉じる" onclick="dlgLogin_lik_close()" class="file-upload1" />
            </div>
        </div>

        <div id="dlgbox_firend" class="dlg">
            <div id="dlg-header_friend" class="dlgh">友達承認</div>
            <div id="dlg-body_friend" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_friend" class="dlgf">
                <input id="Button2" type="button" value="閉じる" onclick="dlgLogin_fri_close()" class="file-upload1" />
            </div>
        </div>

        <div id="dlgbox" class="dlg">
            <div id="dlg-header">メッセ一ジ</div>
            <div id="dlg-body" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer">
                <input id="Button3" type="button" value="閉じる" onclick="dlgLogin_chat_notice_close()" class="file-upload1" />
            </div>
        </div>



        <div id="dlgbox1" class="dlg">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer1">
                <input id="Button1" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload1" />
            </div>
        </div>

        <div id="sitebody">
            <div id="header">
                <table style="width: 100%; height: 100%">
                    <tr>
                        <td class="header-left">

                            <table style="width: 100%;">
                                <tr>
                                    <td width="5%">&nbsp;</td>
                                    <td class="icon_left"><span onclick="ShowSidebarLeft()"><i class="fa fa-bars" style="font-size: 35px; color: white; cursor: pointer;"></i></span></td>
                                    <td class="rin">
                                        <asp:Image ID="Label_logo" Style="height: auto;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                    <!--hehe-->
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                        <td class="header-right">
                            <table style="width: 100%;">
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
                                                <div id="chat_count" style="border-radius:6px 6px 6px 0px;background-color:red;min-width:20px;min-height:20px;position:absolute;top:-10px;right: 0px;">&nbsp;<span style="color:white;">1</span>&nbsp;</div>
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
                                    <td class="icon_right"><i class="fa fa-bell-o" style="font-size: 30px; color: white; cursor: pointer;" onclick="ShowSidebarRight()"></i></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="sidebar_left">
            </div>
            <div id="div_sidebar_right">
                <div id="sidebar_right">
                    <table style="width: 100%; margin-top: 6em;">
                        <tbody>
                            <tr>
                                <td>
                                    <button type="button" onclick="gotomy()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">マイページ</button>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">友達申請</button>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">メッセ一ジ</button>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <button type="button" onclick="new_state_notice()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">お知らせ</button>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; margin-top: 15px; background-color: transparent; color: #000000; cursor: pointer;">ヘルプ</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>


                </div>
                <!--End sidebar_right-->
            </div>
            <!--End div_sidebar_right-->
            <div id="content">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Panel ID="head_homepage_user" runat="server"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF">
                            <asp:Panel ID="homepage_user" runat="server"></asp:Panel>
                            <br/>
                            <div id="post_progress" style="text-align:center; display: none; width:100%;height:100px;" >
    <asp:Image ID="post_progress_img" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
                            <asp:Panel ID="homepage_user3" runat="server" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="border-width: thin; border-style: solid; background-color: #DDDDDD; width: 100%; height: 50px;">
                                            <table style="width: 100%; height: 100%;">
                                                <tr>
                                                    <td style="width: 5%" align="right">&nbsp;</td>
                                                    <td style="width: 10%" align="left">

                                                        <asp:Button ID="Button13" runat="server" Text="すべての友達" BorderStyle="None" ForeColor="Black" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="Button13_Click" />

                                                    </td>
                                                    <td style="width: 15%" align="right">

                                                        <asp:Button ID="Button14" runat="server" Text="共通の友達" BorderStyle="None" ForeColor="#999999" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="Button14_Click" />

                                                    </td>
                                                    <td style="width: 70%" align="right">

                                                        <asp:TextBox ID="txtSearch_f" runat="server" CssClass="textbox" Height="20px" placeholder="ニックネームから検索" Width="50%" Wrap="False"></asp:TextBox>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class='space' height="5%">&nbsp;</td>
                                                    <td width="90%" height="5%">&nbsp;</td>
                                                    <td class='space' height="5%">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class='space'>&nbsp;</td>
                                                    <td>
                                                        <asp:Panel ID="friend_Panel" runat="server">
                                                        </asp:Panel>
                                                        <asp:Panel ID="together_friend_Panel" runat="server">
                                                        </asp:Panel>
                                                    </td>
                                                    <td class='space'>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class='space' height="5%">&nbsp;</td>
                                                    <td width="90%" height="5%">&nbsp;</td>
                                                    <td class='space' height="5%">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>


            </div>
        </div>
        <div>
            <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
                <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
                <br />
                <font color="#95989A" size="2px">読み込み中</font>
            </div>
            <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
            </div>
            <asp:Panel ID="javaplace_formap" runat="server"></asp:Panel>
            <asp:Panel ID="javaplace" runat="server">
            </asp:Panel>

        </div>
        <script type="text/javascript">
            function Hide_PostProgressBar() {
                var progress = $('#post_progress');
                progress.hide();
            }
            // 顯示讀取畫面
            function display_PostProgress() {
                var progress = $('#post_progress');
                progress.show();
            }
            $('#content').click(function () {
                var div = document.getElementById('sidebar_left');
                var menu = document.getElementById('menu_right');
                if (div.style.display == 'block' || menu.style.display == 'block') {
                    div.setAttribute("style", "display:none");
                    menu.setAttribute("style", "display:none");
                }
            });
            function ShowSidebarLeft() {
                var div = document.getElementById('sidebar_left');
                if (div.style.display != 'none') {
                    div.setAttribute("style", "display:none");

                }
                else {
                    div.setAttribute("style", "display:block; z-index:9999");
                }
            }

            function ShowSidebarRight() {
                var div = document.getElementById('div_sidebar_right');
                if (div.style.display == 'block') {
                    div.setAttribute("style", "display:none");

                }
                else {
                    div.setAttribute("style", "display:block; z-index:999999");
                }
            }
        </script>
    </form>
</body>
</html>
