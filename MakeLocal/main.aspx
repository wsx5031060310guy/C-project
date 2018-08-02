<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="main" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>


    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja&sensor=true"></script>

         <script src="Scripts/jquery-1.12.4.js"></script>


    	<!-- Bootstrap Core CSS -->
		<link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
		<link rel="stylesheet" type="text/css" href="css/bootstrap-theme.min.css">
		<link rel="stylesheet" type="text/css" href="js/agency.min.js">

    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/jquery.fileupload.css">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">

    <!-- semantic core CSS file -->
    <link rel="stylesheet" type="text/css" href="css/semantic.css">
<script src="js/semantic.js"></script>
    <!-- Magnific Popup core CSS file -->
<link rel="stylesheet" href="css/magnific-popup.css">
<!-- Magnific Popup core JS file -->
<script src="js/jquery.magnific-popup.js"></script>

  <script src="Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui.css">
        <link rel="stylesheet" href="css/file-upload_fb.css" type="text/css"/>
     <link rel="stylesheet" href="css/style.css"/>


    <link rel="stylesheet" href="css/tipped.css">
        <script src="Scripts/tipped.js"></script>
    <script src="Scripts/freewall.js"></script>
    <script src="js/jquery.nicescroll.js"></script>

    <link rel="stylesheet" type="text/css" href="css/jssocials.css" />
    <link rel="stylesheet" type="text/css" href="css/jssocials-theme-flat.css" />

    <link rel="stylesheet" type="text/css" href="css/main.css">
    <%--<link href="https://fonts.googleapis.com/earlyaccess/hannari.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/earlyaccess/notosansjapanese.css" rel="stylesheet" />--%>


    <script src="//connect.facebook.net/ja_JP/sdk.js"></script>
                                   <script>
                                       window.fbAsyncInit = function () {
                                           FB.init({
                                               //appId: '',
                                               appId: '',
                                               xfbml: true,
                                               version: 'v2.8',
                                               scope: 'id,name,first_name,last_name,gender,locale,link,cover,picture,email'
                                           });
                                           FB.Event.subscribe('auth.statusChange', OnLogin);
                                       };

                                       (function (d, s, id) {
                                           var js, fjs = d.getElementsByTagName(s)[0];
                                           if (d.getElementById(id)) { return; }
                                           js = d.createElement(s); js.id = id;
                                           js.src = "//connect.facebook.net/ja_JP/sdk.js";
                                           fjs.parentNode.insertBefore(js, fjs);
                                       }(document, 'script', 'facebook-jssdk'));

                                       //
                                       function OnLogin(response) {
                                           var str = "<%= Session["id"]%>".toString();

                                           if (str == "" || str == null) {
                                               if (response.authResponse) {
                                                   FB.api('/me?fields=id,name,first_name,last_name,gender,locale,link,cover,picture,email', LoadValues);
                                               }
                                           }
                                       }

                                       //This method will load the values to the labels
                                       function LoadValues(me) {
                                           console.log(me);
                                           if (me.name) {
                                               var name = "", id = "", email = "", pic = "", cov = "";
                                               var myname = me.name;
                                               if (myname) {
                                                   name = me.name;
                                               }
                                               myname = me.id;
                                               if (myname) {
                                                   id = me.id;
                                               }
                                               myname = me.email;
                                               if (myname) {
                                                   email = me.email;
                                               }
                                               //myname = me.gender;
                                               //if (myname) {
                                               ////sex
                                               //    document.getElementById('Gender').innerHTML = me.gender;
                                               //}
                                               myname = me.picture;
                                               if (myname) {
                                                   pic = me.picture.data.url;
                                               }
                                               myname = me.cover;
                                               if (myname) {
                                                   cov = me.cover.source;
                                               }
                                               $.ajax({
                                                   type: "POST",
                                                   url: "main.aspx/FB_res",
                                                   data: "{param1: '" + name + "',param2: '" + id + "',param3: '" + email + "',param4: '" + pic + "',param5: '" + cov + "'}",
                                                   contentType: "application/json; charset=utf-8",
                                                   dataType: "json",
                                                   async: true,
                                                   cache: false,
                                                   success: function (result) {
                                                       //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                                       //console.log(result.d);

                                                       if (result.d == "one") {
                                                           window.location.href = "main.aspx";
                                                       } else if (result.d == "two") {
                                                           window.location.href = "registered0.aspx";
                                                       } else if (result.d == "three") {
                                                           alert("メールをご確認ください。");
                                                       }
                                                   },
                                                   error: function (result) {
                                                       //console.log(result.Message);
                                                       //alert(result.d);
                                                   }
                                               });


                                           }
                                       }

                                       function postToWallUsingFBUi(to_link, to_picture, to_message) {
                                           //console.log(to_picture);
                                           FB.ui(
      {
          method: 'feed',
          name: '',
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
        var displayCloseFoo = function (position) {
            var lat = position.coords.latitude;
            var lon = position.coords.longitude;
        };

        var displayError = function (error) {
            var errors = {
                1: 'Permission denied',
                2: 'Position unavailable',
                3: 'Request timeout'
            };
            //alert("Error: " + errors[error.code]);
        };

        var runGeo = function () {
            if (navigator.geolocation) {
                var timeoutVal = 10 * 1000 * 1000;
                navigator.geolocation.getCurrentPosition(
                        displayCloseFoo,
                        displayError,
                        { enableHighAccuracy: true, timeout: timeoutVal, maximumAge: 0 }
                );
            }
            else {
                //alert("Geolocation is not supported by this browser");
            }
        };
        runGeo();
        ////$(document).ready(function () {
        ////    //ShowProgressBar();
        ////    //HideProgressBar();
        ////});
        ////$(window).load(function () {
        ////    //HideProgressBar();
        ////    //ShowProgressBar();
        ////});
        //$(document).ready(function () {
        //    console.log("document loaded");
        //});

        //$(window).load(function () {
        //    console.log("window loaded");
        //});

        $(function () {
            $('#login_password_text').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    $('#login_check_Button').click();
                    return false;
                }
            });



        });


        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });
            $("body").niceScroll();

            $("#<%=txtSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "test_search.aspx/Getsearch",
                        data: "{ 'prefix': '" + request.term + "'}",
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

            $("#<%=txtSearch.ClientID %>").data("ui-autocomplete")._renderItem = function (ul, item) {

                var $li = $('<li>'),
                    $img = $('<img>');


                $img.attr({
                    src: item.icon,
                    alt: item.label,
                    width: "40px",
                    height: "40px"
                });

                $li.attr('data-value', item.label);

                $li.append('<div>');
                $li.find('div').append($img).append(item.label);

                return $li.appendTo(ul);
            };

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
        //new state message notice
        $(window).load(function () {
            var param1 = "<%= Session["id"]%>".toString();
            if (param1 != "") {
                $.ajax({
                    type: "POST",
                    url: "main.aspx/count_list",
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
                url: "main.aspx/new_state_list",
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
        ///
        $(function () {
            $('#dlg-body1').on('scroll', function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    var param1 = "<%= Session["id"]%>".toString();
                    $.ajax({
                        type: "POST",
                        url: "main.aspx/new_state_notice_list_scroll",
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
                url: "main.aspx/chat_notice_list",
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
                url: "main.aspx/friend_notice_list",
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
                url: "main.aspx/search_friend_notice_list",
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
                url: "main.aspx/friend_notice_check",
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
                url: "main.aspx/friend_notice_check_del",
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
                url: "main.aspx/friend_notice_donotfind",
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
                url: "main.aspx/friend_notice_addfind",
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
                url: "main.aspx/toget_friend_list",
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
                         url: "main.aspx/search_friend_notice_list_scroll",
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
        function showDialog() {
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox");
            whitebg.style.display = "block";
            dlg.style.display = "block";

            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
            dlg.style.top = winHeight / 10 + "px";
        }
        function showDialog_m() {
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox1");
            whitebg.style.display = "block";
            dlg.style.display = "block";

            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
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

        function gotodate(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var sup_str = "";
            var cut = click_id.indexOf('_');
            sup_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "main.aspx/changetodate",
                data: "{param1: '" + param1 + "' , param2 :'" + sup_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    window.location.href = "Date_Calendar.aspx";
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }


        function blike(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
             var like_str = "";
             var cut = click_id.indexOf('t');
             like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var yn = 1;
             $.ajax({
                 type: "POST",
                 url: "main.aspx/like_or_not",
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
                 url: "main.aspx/like_or_not",
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
                url: "main.aspx/like_who_ans",
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
                url: "main.aspx/like_who_ans",
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
                url: "main.aspx/like_who_ans",
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
                url: "main.aspx/like_who_ans",
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
                url: "main.aspx/who_ans",
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
                        url: "main.aspx/small_sendtopost",
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


                            window.location.href = "main.aspx";
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
                        url: "main.aspx/sendtopost",
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

                            window.location.href = "main.aspx";
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

            var body = document.body, html = document.documentElement;

            var height = Math.max(body.scrollHeight, body.offsetHeight,
                                   html.clientHeight, html.scrollHeight, html.offsetHeight);

            //$("#sidebar_left").height(height);

            $("#salutation").selectmenu({
                width: 150
            });

            $("#dialog").dialog({
                autoOpen: false,
                show: {
                    effect: "fold",
                    duration: 100
                },
                hide: {
                    effect: "blind",
                    duration: 100
                },
                closeText :"",
                open: function () {
                    $("#addmsg").empty();
                    if (navigator.geolocation) {
                        navigator.geolocation.getCurrentPosition(function (position) {
                            var pos = {
                                lat: position.coords.latitude,
                                lng: position.coords.longitude
                            };
                            $.ajax({
                                url: "get_GPS.ashx",
                                type: "get",
                                data: { address: "0", lat: pos.lat, lng: pos.lng },
                                async: true,
                                dataType: "json",
                                success: function (r) {
                                    //console.log(r.address);
                                    document.getElementById("txt2").value = r.address;
                                    runScript();
                                    //postcode_HiddenField.value = r.postal_code;


                                }
                            });

                        }, function () {
                            //handleLocationError(true, infoWindow, map.getCenter());
                        });
                    } else {
                        // Browser doesn't support Geolocation
                        //handleLocationError(false, infoWindow, map.getCenter());
                    }
                }
            });
            $("#share_div").dialog({
                autoOpen: false,
                show: {
                    effect: "fold",
                    duration: 100
                },
                hide: {
                    effect: "blind",
                    duration: 100
                }
            });

            $("#opener").on("click", function () {
                $("#dialog").dialog("open");
            });


            $("#place_select").on("click", function () {
                $("#addmsg").empty();
                $("#addmsg").append(document.getElementById("txt2").value).show();
                var str = document.getElementById("txt2").value;
                if (str.trim() != "") {
                    runScript();
                    var placeHidden = document.getElementById('<%= place_va.ClientID %>');
                    if (placeHidden)//checking whether it is found on DOM, but not necessary
                    {
                        placeHidden.value = document.getElementById("txt2").value;
                    }

                }
                $("#dialog").dialog("close");
            });

            $('.ui.dropdown')
                .dropdown({
                    onChange: function (value, text, $selectedItem) {
                        // nothing built in occurs
                        var myHidden = document.getElementById('<%= type_div.ClientID %>');
                        var cut = text.trim().indexOf('>');
                        var sub = text.trim().substr(cut + 1, text.length - 1);
                        if (myHidden)//checking whether it is found on DOM, but not necessary
                        {
                            myHidden.value = sub.trim();
                        }

                    }
                })
            ;
            $('#select')
                .dropdown()
            ;


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

            $("#login_Button").on("click", function () {
                $("#first_login_d").hide();
                $("#second_login_d").show();
            });





        });

                $(document).ready(function () {
                    initMap();
                    var str1 = "<%= Session["id"]%>".toString();
                    if (str1 != "" || str1 != null) {
                        check_photo(str1);
                    }





                    $("#login_check_Button").click(function () {
                        if ($('#login_username_text').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() != "" && $('#login_password_text').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() != "") {
                            $.ajax({
                                type: "POST",
                                url: "main.aspx/check_login",
                                data: "{param1: '" + $('#login_username_text').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() + "' , param2 :'" + $('#login_password_text').val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '').trim() + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                cache: false,
                                success: function (result) {
                                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                    //console.log(result.d);
                                    $('#result_text').text(result.d);
                                    alert(result.d);
                                    var str = "<%= Session["id"]%>".toString();

                                    if (str != "" || str != null) {
                                        //check_photo(str);
                                        window.location.href = "main.aspx";
                                        dlgLogin();
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

                function check_photo(par) {
                    var str1 = "<%= Session["id"]%>".toString();
                    if (str1 != "" || str1 != null) {
                        $.ajax({
                            type: "POST",
                            url: "main.aspx/check_photo",
                            data: "{param1: '" + par + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                var str = result.d;

                                if (str != "" || str != null) {
                                    $('#userphoto_div').empty();
                                    $('#userphoto_div').append('<a href="' + str + '" data-source="' + str + '" title="" style="width:40;height:40;"><img src="' + str + '" width="40" height="40" /></a>');

                                }
                            },
                            error: function (result) {
                                //console.log(result.Message);
                            }
                        });
                    }
                }

                function initMap() {

                    //var mapOptions = {
                    //    center: new google.maps.LatLng(35.455846, 139.589405),
                    //    zoom: 180,
                    //    mapTypeId: google.maps.MapTypeId.ROADMAP
                    //};
                    //var mapElement = document.getElementById("show_map_area");

                    //map = new google.maps.Map(mapElement, mapOptions);
                }

                //mark map messages
                //放DataTable資料的全域變數
                var array = new Array();
                var markers = [];
                //抓出地址資料
                function QueryDataTable() {
                    array = new Array();
                    $.ajax(
                    {
                        url: 'DataTableSource.ashx',
                        type: 'post',
                        async: true,
                        data: { ranid: ranid },
                        dataType: 'json',
                        success: function (datas) {
                            $(datas).each(function (index, item) {
                                array.push(item); //一一加入陣列
                            });
                            repeatFunc(0); //一個一個項目標記在地圖上

                        }
                    });
                }





                //網頁上所有的DOM都載入後
                var len = 0;
                var chec = true;

                function runScript() {
                    var add = document.getElementById("txt2").value;
                    $.ajax({
                        url: 'find_city_area.ashx',
                        type: 'post',
                        data: { address: add },
                        async: true,
                        dataType: 'json',
                        success: function (r) {
                            var mess = r.Message;
                            postcode_HiddenField.value = mess;
                            //document.getElementById("txt1").value = mess;
                            //$('#div_showMap').show();
                            MarkerOne(r);

                        }
                    });
                }

                var infowindow = new google.maps.InfoWindow();
                var map, geocoder;

                function MarkerOne(item) {
                    $.ajax({
                        url: "MarkerOne.ashx",
                        type: "get",
                        data: { address: item.address, lat: item.lat, lng: item.lng },
                        async: true,
                        dataType: "json",
                        success: function (r) {
                            var location; //取得此資料的位置
                            //建立緯經度座標物件
                            var latlng;
                            if (item.lat == 0 && item.lng == 0) {
                                location = r.results[0].geometry.location;
                                latlng = new google.maps.LatLng(location.lat, location.lng);
                            } else {
                                latlng = new google.maps.LatLng(item.lat, item.lng);
                            }
                            var myOptions = {
                                zoom: 16,
                                center: latlng,
                                mapTypeId: google.maps.MapTypeId.ROADMAP
                            };
                            /*產生地圖*/
                            map = new google.maps.Map(document.getElementById("div_showMap"), myOptions);
                            var imageUrl = ""; //空字串就會使用預設圖示

                            //加一個Marker到map中
                            var marker = new google.maps.Marker({
                                position: latlng,
                                map: map,
                                draggable: true,
                                icon: imageUrl,
                                html: item.lat + "," + item.lng
                            });
                            geocodePosition(marker.getPosition());
                            google.maps.event.addListener(marker, 'dragend', function () {
                                geocodePosition(marker.getPosition());
                            });


                            //markers.push(marker);
                        },
                        error: function (result) {
                            alert(result.responseText);
                        }

                    });
                }
                function geocodePosition(pos) {
                    geocoder = new google.maps.Geocoder();
                    geocoder.geocode
                     ({
                         latLng: pos
                     },
                         function (results, status) {
                             if (status == google.maps.GeocoderStatus.OK) {
                                 document.getElementById("txt2").value = results[0].formatted_address;

                                 var placeHidden_lat = document.getElementById('<%= lat_HiddenField.ClientID %>');
                                 if (placeHidden_lat)//checking whether it is found on DOM, but not necessary
                                 {
                                     placeHidden_lat.value = pos.lat();
                                     //console.log(pos.lat());
                                 }
                                 var placeHidden_lng = document.getElementById('<%= lng_HiddenField.ClientID %>');
                                    if (placeHidden_lng)//checking whether it is found on DOM, but not necessary
                                    {
                                        placeHidden_lng.value = pos.lng();
                                        //console.log(pos.lng());
                                    }

                                 var arrAddress = results[0].address_components;
                                 var itemRoute = '';
                                 var itemLocality = '';
                                 var itemCountry = '';
                                 var itemPc = '';
                                 var itemSnumber = '';

                                 // iterate through address_component array
                                 $.each(arrAddress, function (i, address_component) {
                                     //console.log('address_component:' + i);

                                     if (address_component.types[0] == "route") {
                                         //console.log(i + ": route:" + address_component.long_name);
                                         itemRoute = address_component.long_name;
                                     }

                                     if (address_component.types[0] == "locality") {
                                         //console.log("town:" + address_component.long_name);
                                         itemLocality = address_component.long_name;
                                     }

                                     if (address_component.types[0] == "country") {
                                         //console.log("country:" + address_component.long_name);
                                         itemCountry = address_component.long_name;
                                     }

                                     if (address_component.types[0] == "postal_code") {
                                         //console.log("pc:" + address_component.long_name);
                                         itemPc = address_component.long_name;
                                         postcode_HiddenField.value = address_component.long_name;
                                     }

                                     if (address_component.types[0] == "street_number") {
                                         //console.log("street_number:" + address_component.long_name);
                                         itemSnumber = address_component.long_name;
                                     }
                                     //return false; // break the loop
                                 });
                             }
                             else {
                                 alert('Cannot determine address at this location.' + status);
                             }
                         }
                     );
                }
                function UploadFile(fileUpload) {
                    if (fileUpload.value != '') {
                        document.getElementById("<%=btnUploadDoc.ClientID %>").click();
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
         $(window).load(function () {
             $('#loading').hide();
         });
         function check_like_list(clickid) {
             var like_str = "";
             var cut = clickid.indexOf('_');
             like_str = clickid.substr(cut + 1, clickid.length - cut - 1);
             $.ajax({
                 type: "POST",
                 url: "main.aspx/like_list",
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
                     url: "main.aspx/report_bad",
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
                     //alert("bottom!");
                     if (didScroll) {
                         didScroll = false;
                         count += 10;
                         display_PostProgress();
                         $.ajax({
                             type: "POST",
                             url: "main.aspx/search_new_post",
                             data: "{param1: '" + count + "'}",
                             contentType: "application/json; charset=utf-8",
                             dataType: "json",
                             async: true,
                             cache: false,
                             success: function (result) {
                                 //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                 didScroll = true;
                                 if (result.d == "0") {
                                 } else {
                                     $('#javaplace_formap').empty();
                                     $('#Panel2').append(result.d);
                                 }
                                 //console.log(count);
                                 //console.log(result.d);
                                 //window.location.href = "main.aspx";
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
                     url: 'main.aspx/user_action',
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
    <script src="Scripts/jssocials.js"></script>

    <div id="loading" style="width: 100%;height: 100%;  left: 0;position: fixed;display: block;opacity: 0.7;background-color: #fff;z-index: 9999999990;text-align: center;" >
    <img id="loading-image" style="z-index: 9999999999;margin-top:45vh" src="images/loading.gif" alt="読み込み中" />
    <br />
    <font color="#95989A" style="z-index: 9999999999;" size="2px">読み込み中</font>
</div>
            <form id="form1" runat="server">
<div id="white-background">
        </div>
                 <div id="dlgbox_tofriend_list" class="dlg">
            <div id="dlg-header_tofriend_list" class="dlgh">共通の友達</div>
            <div id="dlg-body_tofriend_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_tofriend_list" class="dlgf">
                <input id="Button4" type="button" value="閉じる" onclick="dlgLogin_tofri_close()" class="file-upload1"/>
            </div>
        </div>

                <div id="dlgbox_like_list" class="dlg">
            <div id="dlg-header_like_list" class="dlgh">いいね</div>
            <div id="dlg-body_like_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_like_list" class="dlgf">
                <input id="Button1" type="button" value="閉じる" onclick="dlgLogin_lik_close()" class="file-upload1"/>
            </div>
        </div>

                <div id="dlgbox_firend" class="dlg">
            <div id="dlg-header_friend" class="dlgh">友達承認</div>
            <div id="dlg-body_friend" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_friend" class="dlgf">
                <input id="Button2" type="button" value="閉じる" onclick="dlgLogin_fri_close()" class="file-upload1"/>
            </div>
        </div>
        <div id="dlgbox">
            <div id="dlg-header">メッセ一ジ</div>
            <div id="dlg-body" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer">
                <input id="Button3" type="button" value="閉じる" onclick="dlgLogin_chat_notice_close()" class="file-upload1"/>
            </div>
        </div>


                 <div id="dlgbox1">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer1">
                <input id="Button13" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload1"/>
            </div>
        </div>


                <div id="dlgbox_login">
            <div id="dlg-header_login"></div>
            <div id="dlg-body_login" style="height: 350px; overflow: auto">
                <div id="first_login_d" style="width: 100%;height:100%">
                    <table style="width: 100%;height:100%" align="center">
                        <tr><td width="10%" height="20%"></td><td width="80%" height="20%"></td><td width="10%" height="20%"></td></tr>
                        <tr><td width="10%"></td><td width="80%">
                            <table style="width: 100%;height:100%;">
                    <tr>
                        <td style="width: 100%;height:30%;">
                            <span style="font-size: large">すべての機能を利用するには</span><br/>
                            <span style="font-size: large">ログイン / 新規登録が必要です。</span><br/><br/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%;">
                            <input id="login_Button" type="button" value="ログイン" style="width:70%;height:100%; font-size: large;" class="file-upload1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%;">
                            <asp:Button ID="register_Button" runat="server" Text="新規会員登録" onclick="register_Button_Click" OnClientClick="ShowProgressBar();" style="width:70%;height:100%;" CssClass="file-upload1" Font-Size="Large" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%;">
                            <div class="fb-login-button" autologoutlink="false" scope="email" data-size="xlarge" >  Facebookでログインする</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:10%;">
                            <a href="password_forget.aspx" target="_blank" style="text-decoration:none;">パスワードを忘れた方はこちら</a>
                        </td>
                    </tr>
                </table>
                                     </td><td width="10%"></td></tr>
                        <tr><td></td><td></td><td></td></tr>
                        </table>
                    </div>

            <div id="second_login_d" style="width: 100%;height:100%">
                <table style="width: 100%;height:100%;" align="center">
                        <tr><td width="10%" height="20%"></td><td width="80%" height="20%"></td><td width="10%" height="20%"></td></tr>
                        <tr><td width="10%"></td><td width="80%" height="70%">
                <table style="width: 100%;height:100%;">
                    <tr>
                        <td style="width: 100%;height:20%;">
                             <span style="font-size: large">ご登録のメールアドレスと</span><br/>
                            <span style="font-size: large">パスワードを入力してください</span><br/>
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 100%;height:20%;">
                            <span id="result_text" style="color: #FF0000; font-size: large;"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%;">
                            <input id="login_username_text" type="text" name='login_username_text' class='textbox' placeholder='メールアドレス' style='height:100%;width:80%; font-size: large;' />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%; font-size: large;">
                            <input id="login_password_text" type="password" name='login_password_text' class='textbox' placeholder='パスワード' style='height:100%;width:80%; font-size: large;' />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;height:20%;">
                            <input id="login_check_Button" type="button" value="ログイン" class="file-upload1" style="width:70%;height:100%; font-size: large;" />
                        </td>
                    </tr>
                </table>
                    </td><td width="10%"></td></tr>
                        <tr><td></td><td></td><td></td></tr>
                        </table>
            </div>
                </div>
            <div id="dlg-footer_login">
            </div>
        </div>


            <div id="sitebody">
　<div id="header">
                    <table style="width:100%;height:100%">
                        <tr>
                            <td class="header-left">

                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                        <td class="icon_left"><span onclick="ShowSidebarLeft()"><i class="fa fa-bars" style="font-size:3.5em; color: white;cursor: pointer;" ></i></span></td>

                                         <td class="rin"><asp:Image id="Label_logo" style="height:auto;cursor:pointer;margin-left:3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
<!--hehe-->

                                    </tr>
                                </table>
                            </td>
                            <td class="header-midle">
                                <asp:TextBox ID="txtSearch" runat="server" Width="70%" Height="30px" placeholder=" 友達を検索"></asp:TextBox>
                            </td>
                           <td class="header-right">
                                <table style="width:100%;">
                                    <tr>
                                        <td class="topnav_right">
                                            <button type="button" onclick="window.location.href='user_home.aspx';" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">マイページ</button>
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
                                         <td class="icon_map"><span onclick="ShowMap()"><i class="fa fa-location-arrow fa-undo " style="font-size:30px; color: white;cursor: pointer;"></i></span></td>
                                        <td class="icon_right"><span onclick="ShowRightMenu()"><i class="fa fa-bell-o" style="font-size:30px; color: white;cursor: pointer;"></i></span></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div>
<div class="div_sidebar_left">
　<div id="sidebar_left">
      <table style="width: 100%; margin-top: 3em;">
                        <tr>

                            <td>

                                <br /><br />
                               <span><img src="images/new.png" style="height: 25px;width: 25px;"></span><asp:Button ID="social_Button" runat="server" Text="二ュースフィード" BorderStyle="None" BackColor="transparent" OnClick="social_Button_Click" OnClientClick="ShowProgressBar();" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>

                            <td class="hidden">
                              <asp:Button ID="Button_for_kid" runat="server" Text="子育てサポート" BorderStyle="None" BackColor="transparent" Enabled="False" OnClientClick="ShowProgressBar();" OnClick="Button_for_kid_Click" />
                                <br /><br />
                            </td>
                            </tr>
                         <tr>

                            <td class="hidden">
                                                               <table style="width:100%;">
                                    <tr>

                                        <td><asp:Button ID="supporter_list" runat="server" Text="サポーター" BorderStyle="None" BackColor="transparent" OnClick="supporter_list_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><asp:Button ID="video_list_Button" runat="server" Text="講習動画" BorderStyle="None" BackColor="transparent" OnClick="video_list_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><asp:Button ID="supporter_manger" runat="server" Text="サポート管理" BorderStyle="None" BackColor="transparent" OnClick="supporter_manger_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                </table>
                                <br /><br />
                            </td>
                            </tr>
                        <tr>

                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button_for_social" style="font-size:20px; font-weight:bold" runat="server" Text="工リア情報" BorderStyle="None" BackColor="transparent" Enabled="False" />

                            </td>
                            </tr>


                        <tr>
                            <td>
                               <table style="width:100%;margin-top:20px">
                                    <tr>

                                        <td><span><img src="images/food.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type0_Button" runat="server" Text="お食事" BorderStyle="None" BackColor="transparent" OnClick="message_type0_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span><img src="images/supporter.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type1_Button" runat="server" Text="人気スポット" BorderStyle="None" BackColor="transparent" OnClick="message_type1_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span><img src="images/event.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type2_Button" runat="server" Text="イベント" BorderStyle="None" BackColor="transparent" OnClick="message_type2_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span><img src="images/hospital.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type3_Button" runat="server" Text="病院" BorderStyle="None" BackColor="transparent" OnClick="message_type3_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span><img src="images/park.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type4_Button" runat="server" Text="公園／レジャー" BorderStyle="None" BackColor="transparent" OnClick="message_type4_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span><img src="images/milk_room.png" style="height: 25px;width: 25px;"></span><asp:Button ID="message_type5_Button" runat="server" Text="授乳室" BorderStyle="None" BackColor="transparent" OnClick="message_type5_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            </tr>

                    </table>
     <div>
     </div>
　</div>
                </div>

                <div id="div_sidebar_right">
　<div id="sidebar_right">
      <div id="show_map_area" align="center" style="width: 100%; height: 100%">
      </div>

          <div id="menu_right">
        <table style="width: 100%; margin-top: 6em;">
                 <tr>
                        <td>
                          <button type="button" onclick="gotomy()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">マイページ</button>
                     </td>
                 </tr>

                 <tr>
                    <td>
                    <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">友達申請</button>
                 </td>
             </tr>

             <tr>
                <td>
                 <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">メッセ一ジ</button>
             </td>
         </tr>

         <tr>
            <td>
             <button type="button" onclick="new_state_notice()" style="border-style: none; background-color: #E7E7E7;margin-top: 15px; cursor: pointer;">お知らせ</button>
         </td>
     </tr>
          <tr>
            <td>
            <button type="button" onclick="window.location.href='Help.html';" style="border-style: none;margin-top: 15px;  background-color: transparent; color: #000000; cursor: pointer;">ヘルプ</button>
         </td>
     </tr>
         </table>
    </div><!--End menu_right-->

     </div>
　</div>

　<div id="content">
     <table style="width: 100%;height: 100%;margin-top: -20px">
         <tr>
             <td height="20%" valign="top">
                 <asp:Panel ID="post_message_panel" runat="server">


          <table style="border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;width: 100%; height: 100%;">
                     <tr>
                         <td>
                             <table style="width: 100%; height: 100%;">
                                 <tr>
                                     <td class="auto-style1"></td>
                                     <td align="right" width="5%" valign="bottom">
                                         <asp:Image ID="Image2" runat="server" ImageUrl="~/images/photo.png" Height="20px" Width="20px" />
                                         <br/>
                                     </td>
                                         <td align="left" valign="bottom">
                                     <label class="file-upload" style=" font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;">
            <span class="camera" style="cursor:pointer;">写真</span>
            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);" AllowMultiple="True"/>
             </label>
                             <br />
                                <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>

                                     </td>
                                     <td valign="bottom" width="10%">
                                         <br />

                                     </td>
                                 </tr>
                             </table>
                             <hr/>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             <table style="width: 100%;">
                                 <tr>
                                     <td class="auto-style1">&nbsp;</td>
                                     <td align="left" height="60px" width="60px">
                                         <asp:Panel ID="Panel3" runat="server"></asp:Panel>
                                         <div id="userphoto_div" class="zoom-gallery">

                                             </div>
                                     </td>
                                     <td valign="middle">
                                         <asp:TextBox ID="post_message_TextBox" runat="server" Width="100%" placeholder=" 地域の情報を書き込もう" Height="100%" TextMode="MultiLine" BorderStyle="None"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                     </td>
                                     <td colspan="2">
                                         <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" style="height:auto;max-height:200px;">
                                             <asp:HiddenField ID="image_HiddenField" runat="server" />
                                         </asp:Panel>
                                     </td>
                                 </tr>
                             </table>
                            <hr/>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             <table class="new_post_bottom_bar" style="width: 100%;">
                                 <tr>
                                     <td class="auto-style1">  <img alt="" src="images/tag.png" height="15px" width="15px" /></td>
                                     <td align="left" width="20%">


 <select name="post_type" id="select">
     <option value="">カテゴリー</option>
      <option value="0">お食事</option>
      <option value="2">イベント</option>
      <option value="3">病院</option>
      <option value="4">公園／レジャー</option>
       <option value="5">授乳室</option>
</select>


                                     </td>
                                     <td width="25%">
                                         <div id="addmsg"></div>
                                         <asp:HiddenField ID="place_va" runat="server" />
                                         <asp:HiddenField ID="lat_HiddenField" runat="server" />
                                         <asp:HiddenField ID="lng_HiddenField" runat="server" />
                                         <asp:HiddenField ID="postcode_HiddenField" runat="server" />
                                         <img alt="" src="images/pin.png" height="15px" width="15px" />
                                         <input id="opener" type="button" value="位置情報" style="border-style: none; background-color: #FFFFFF; color: #CCCCCC;" />

                                     </td>
                                     <td width="25%">
<span>
  <div class="ui inline dropdown" style="white-space:nowrap">
    <div class="text">
      <img class="ui avatar image" src="images/icon/public.png" height="15px" width="15px">
      一般公開
    </div>
    <i class="dropdown icon"></i>
    <div class="menu">
       <div class="item">
        <img class="ui avatar image" src="images/icon/public.png">
        一般公開
      </div>
      <div class="item">
        <img class="ui avatar image" src="images/icon/neighborhood.png">
        地域限定
      </div>
      <div class="item">
        <img class="ui avatar image" src="images/icon/friend.png">
        友達
      </div>
    </div>
  </div>
</span>

                                         <asp:HiddenField ID="type_div" runat="server" />
                                     </td>
                                     <td>
                             <asp:Button ID="post_message_Button" runat="server" BackColor="#000099" BorderStyle="None" ForeColor="White" Text="投稿" Width="95%" OnClick="post_message_Button_Click" Height="25px" />
                                     </td>
                                     <caption>
                                         <br/>
                                         <br/>
                                     </caption>
                                 </tr>
                             </table>
                         </td>
                     </tr>
                 </table>
                 </asp:Panel>
                  <asp:Panel ID="Panel_for_support_list" runat="server" Height="100px"></asp:Panel>

             </td>
         </tr>
         <tr>
             <td>
                 <asp:ScriptManager ID="ScriptManager1" runat="server">
                 </asp:ScriptManager>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server" Width="100%">
            </asp:Panel>
                             <asp:Panel ID="Panel_for_supplist" runat="server"></asp:Panel>
    </ContentTemplate>
                 </asp:UpdatePanel>

                 <br />
               <div id="post_progress" style="text-align:center; display: none; width:100%;height:100px;" >
    <asp:Image ID="post_progress_img" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
             </td>
         </tr>
     </table>
　</div>
</div>




   <%-- <div>

        <div style="width: 100%; height: 50px"></div>
        <div style="width: 20%;" class="main">1</div>
        <div style="width: 60%;" class="main1">2</div>
        <div style="width: 20%;" class="main2">3</div>

        <table style="width: 100%; height: 30%;">
            <tr>
                <td style="width: 15%;">&nbsp;</td>
                <td style="width: 60%;">&nbsp;</td>
                <td style="width: 25%;">&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 10%">
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button2" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button4" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button5" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                               &nbsp;&nbsp;<asp:Button ID="Button6" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button7" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button8" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button9" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button10" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                           <td>
                                &nbsp;&nbsp;<asp:Button ID="Button11" runat="server" Text="Button" BorderStyle="None" BackColor="Silver" />
                               <br /><br />
                            </td>
                        </tr>
                    </table>

                </td>
                <td>

                </td>
                <td>
                    <div align="center">
<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d208050.53811007156!2d139.60380047594595!3d35.43524026117918!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x60185becbbb66509%3A0x69683f660285400!2z5pel5pys56We5aWI5bed57ij5qmr5r-x5biC!5e0!3m2!1szh-TW!2stw!4v1473325484439" width="100%" height="600px" frameborder="0" style="border:0" allowfullscreen></iframe>
                </div>

                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <div>
                        <asp:Image ID="Image5" runat="server" ImageUrl="~/images/p01.jpg" CssClass="p01" />
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/images/p01.jpg" CssClass="p01" />
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/p01.jpg" CssClass="p01" />
                        <asp:Image ID="Image8" runat="server" ImageUrl="~/images/p01.jpg" CssClass="p01" />
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>--%>
                <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
                <asp:Panel ID="javaplace_formap" runat="server"></asp:Panel>
                <asp:Panel ID="javaplace" runat="server" >
                </asp:Panel>
    </form>
                    <div id="dialog" title="場所">
                    <table style="width: 100%;" align="center">
                        <tr>
                            <td>
                                <INPUT id="txt2" TYPE="text" onKeydown="Javascript: if (event.which == 13 || event.keyCode == 13) runScript();" placeholder=" 住所を入力" style="width: 100%;" title="【Enter】キーを押してください"><br />
                            <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="div_showMap" style="width: 250px; height: 250px">
                                 </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="place_select" type="button" value="この場所で決定" class="file-upload1" style="width: 100%" />
                            </td>
                        </tr>
                    </table>
</div>
     <div id="share_div" title="SHARE">
</div>
     <script type='text/javascript'>
         $("#share_div").jsSocials({
             showLabel: false,
             showCount: false,
             shares: ["email", "twitter", "facebook", "googleplus", "linkedin", "pinterest", "stumbleupon", "whatsapp", "telegram", "line"],
             url: "http://.jp/",
             text: "地域のいい情報をGETしました！",
             shareIn: "popup"
         });
</script>
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

        //$(document).ready(function () {

        //    google.maps.event.addListener(map, "idle", function () {
        //        google.maps.event.trigger(map, 'resize');
        //    });
        //});

        function ShowSidebarLeft() {
            var menu = document.getElementById('menu_right');
            var map = document.getElementById('show_map_area');
            var div = document.getElementById('sidebar_left');
            if (div.style.display == 'block') {
                div.setAttribute("style", "display:none");


            }
            else {
                $('.icon_map i').addClass("fa-location-arrow");
                div.setAttribute("style", "display:block;height:100%");
                map.setAttribute("style", "visibility:hidden;");
                menu.setAttribute("style", "display:none;");

            }
        }


        function ShowMap() {
            var div = document.getElementById('sidebar_left');
            var sidebarR = document.getElementById('sidebar_right');
            var cover = document.getElementById('div_sidebar_right');
            var menu = document.getElementById('menu_right');
            var map = document.getElementById('show_map_area');
            if (map.style.visibility == 'visible') {
                $('.icon_map i').addClass("fa-location-arrow");
                sidebarR.setAttribute("style", "z-index:-1;");
                map.setAttribute("style", "visibility:hidden;");
                cover.setAttribute("style", "width:40%;");

            }
            else {
                div.setAttribute("style", "display:none");

                $('.icon_map i').removeClass("fa-location-arrow");
                map.setAttribute("style", "visibility:visible;");
                menu.setAttribute("style", "display:none;");
                cover.setAttribute("style", "display:block;width:100%;");
                sidebarR.setAttribute("style", "z-index:2;");


            }
        }

        function ShowRightMenu() {
            var div = document.getElementById('sidebar_left');
            var sidebarR = document.getElementById('sidebar_right');
            var menu = document.getElementById('menu_right');
            var map = document.getElementById('show_map_area');
            if (menu.style.display == 'block') {

                menu.setAttribute("style", "display:none;");
                sidebarR.setAttribute("style", "z-index:-1;");

            }
            else {
                div.setAttribute("style", "display:none");
                $('.icon_map i').addClass("fa-location-arrow");
                menu.setAttribute("style", "display:block");
                map.setAttribute("style", "visibility:hidden;");
                sidebarR.setAttribute("style", "z-index:2;");


            }
        }

        $('#content').click(function () {
            var div = document.getElementById('sidebar_left');
            var menu = document.getElementById('menu_right');
            if (div.style.display == 'block' || menu.style.display == 'block') {
                div.setAttribute("style", "display:none");
                menu.setAttribute("style", "display:none");
            }
        });


</script>
    </body>
</html>
