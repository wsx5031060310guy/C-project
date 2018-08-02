<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_home.aspx.cs" Inherits="user_home" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja&sensor=true"></script>
         <script src="Scripts/jquery-1.12.4.js"></script>

    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/jquery.fileupload.css">

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

    <link href="css/MonthPicker.css" rel="stylesheet" type="text/css" />
    <script src="js/MonthPicker_jp.js"></script>
    <script src="Scripts/datepicker-ja.js"></script>
    <link rel="stylesheet" href="css/tipped.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
        <script src="Scripts/tipped.js"></script>
    <script src="Scripts/freewall.js"></script>
    <script src="js/jquery.nicescroll.js"></script>

    <link rel="stylesheet" type="text/css" href="css/jssocials.css" />
    <link rel="stylesheet" type="text/css" href="css/jssocials-theme-flat.css" />
    <link rel="stylesheet" href="css/style.css"/>
     <link rel="stylesheet" href="css/user_home.css">

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
         $(document).ready(function () {
             $('.image-link').magnificPopup({ type: 'image' });
             $("body").niceScroll();
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

             dlg.style.left = (winWidth / 2) + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
         function showDialog_login() {
             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox_login");
             whitebg.style.display = "block";
             dlg.style.display = "block";

             var winWidth = window.innerWidth;
             var winHeight = window.innerHeight;

             dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
         //new state message notice
         $(window).load(function () {
             var param1 = "<%= Session["id"]%>".toString();
              if (param1 != "") {
                  $.ajax({
                      type: "POST",
                      url: "user_home.aspx/count_list",
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
                 url: "user_home.aspx/new_state_list",
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
                        url: "user_home.aspx/new_state_notice_list_scroll",
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
                 url: "user_home.aspx/chat_notice_list",
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

                     dlg.style.left =0 + "px";
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
                url: "user_home.aspx/friend_notice_list",
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
                url: "user_home.aspx/search_friend_notice_list",
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
                url: "user_home.aspx/friend_notice_check",
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
                url: "user_home.aspx/friend_notice_check_del",
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
                url: "user_home.aspx/friend_notice_donotfind",
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
                url: "user_home.aspx/friend_notice_addfind",
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
                url: "user_home.aspx/toget_friend_list",
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
                         url: "user_home.aspx/search_friend_notice_list_scroll",
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


         function update_mess(click_id) {
             var up_str = "";
             var cut = click_id.indexOf('_');
             up_str = click_id.substr(cut + 1, click_id.length - cut - 1);

             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox_edit_" + up_str);
             whitebg.style.display = "block";
             dlg.style.display = "block";

             var winWidth = window.innerWidth;
             var winHeight = window.innerHeight;

             dlg.style.left = 0 + "px";
             dlg.style.top = winHeight / 10 + "px";
         }
         function dlgupcanel(click_id) {
              var up_str = "";
              var cut = click_id.indexOf('_');
              up_str = click_id.substr(cut + 1, click_id.length - cut - 1);

              var whitebg = document.getElementById("white-background");
              var dlg = document.getElementById("dlgbox_edit_" + up_str);
              whitebg.style.display = "none";
              dlg.style.display = "none";
         }
         function dlgupdate(click_id) {
             var up_str = "";
             var cut = click_id.indexOf('_');
             up_str = click_id.substr(cut + 1, click_id.length - cut - 1);

             var post_mess = document.getElementById("updateText_" + up_str).value;
             post_mess = post_mess.replace(/\r?\n/g, '<br/>');
             var post_mess_typ = document.getElementById("selectedittag_" + up_str).value;
             var post_typ = document.getElementById('<%= type_div.ClientID %>').value;
             $.ajax({
                 type: "POST",
                 url: "user_home.aspx/update_message",
                 data: "{param1: '" + up_str + "',param2: '" + post_mess + "',param3: '" + post_mess_typ + "',param4: '" + post_typ + "'}",
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



             var whitebg = document.getElementById("white-background");
             var dlg = document.getElementById("dlgbox_edit_" + up_str);
             whitebg.style.display = "none";
             dlg.style.display = "none";
         }
         //

         function blike(click_id) {
             var param1 = "<%= Session["id"]%>".toString();
             var like_str = "";
             var cut = click_id.indexOf('t');
             like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             var yn = 1;
             $.ajax({
                 type: "POST",
                 url: "user_home.aspx/like_or_not",
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
                url: "user_home.aspx/like_or_not",
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
                url: "user_home.aspx/like_who_ans",
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
                url: "user_home.aspx/like_who_ans",
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
                url: "user_home.aspx/like_who_ans",
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
                url: "user_home.aspx/like_who_ans",
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
                url: "user_home.aspx/who_ans",
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
                var imgpath = $('#make-image_' + send_img).attr("src");

                var mess = $('#' + click_id).val().replace("'", "").replace('"', "").replace("`", "").trim();
                if (mess != "") {

                    $.ajax({
                        type: "POST",
                        url: "user_home.aspx/small_sendtopost",
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


                            window.location.href = "user_home.aspx";
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
                var imgpath = $('#make-image' + send_img).attr("src");

                var mess = $('#' + click_id).val().replace("'", "").replace('"', "").replace("`", "").trim();
                if (mess != "") {

                    $.ajax({
                        type: "POST",
                        url: "user_home.aspx/sendtopost",
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

                            window.location.href = "user_home.aspx";
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
                     duration: 1000
                 },
                 hide: {
                     effect: "explode",
                     duration: 1000
                 },
                 closeText: "",
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

             $("#opener").on("click", function () {
                 $("#dialog").dialog("open");
             });

             $("#place_select").on("click", function () {
                 $("#addmsg").empty();
                 $("#addmsg").append(document.getElementById("txt2").value).show();
                 var placeHidden = document.getElementById('<%= place_va.ClientID %>');
                if (placeHidden)//checking whether it is found on DOM, but not necessary
                {
                    placeHidden.value = document.getElementById("txt2").value;
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
                            console.log(sub.trim());
                        }

                    }
                })
             ;
             $('#select')
                 .dropdown()
             ;

             $.widget("custom.iconselectmenu", $.ui.selectmenu, {
                 _renderItem: function (ul, item) {
                     var li = $("<li>"),
                       wrapper = $("<div>", { text: item.label });

                     if (item.disabled) {
                         li.addClass("ui-state-disabled");
                     }

                     $("<span>", {
                         style: item.element.attr("data-style"),
                         "class": "ui-icon " + item.element.attr("data-class")
                     })
                       .appendTo(wrapper);

                     return li.append(wrapper).appendTo(ul);
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


             $("#dialog1").dialog({
                 autoOpen: false,
                 width: "80%",
                 show: {
                     effect: "blind",
                     duration: 1000
                 },
                 hide: {
                     effect: "explode",
                     duration: 1000
                 },
                 closeText: ""
             });

             $("#postalcode").on("click", function () {
                 $.ajax({
                     type: "POST",
                     url: "user_home.aspx/Save_first",
                     data: "{param1: '0'}",
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
                     }
                 });
                 $("#dialog1").dialog("open");
             });

             if ("<%= Session["id"]%>" != null) {
                 var id = "<%= Session["id"]%>".toString();
                  var now = new Date();
                  var thisyear = now.getFullYear();
                  $('#thisyear').text(thisyear + "年");
                  var thismonth = now.getMonth() + 1;

                  $('#this_month').text(thismonth + "月");
                  var beforemonth = thismonth - 1;
                  if (beforemonth == 0) { beforemonth += 12; }
                  $('#before_month').text(beforemonth + "月");
                  var aftermonth = thismonth + 1;
                  if (aftermonth == 13) { aftermonth -= 12; }
                  $('#after_month').text(aftermonth + "月");


                  $("#date_manger_month_group").empty();

                  $.ajax({
                      type: "POST",
                      url: "user_home.aspx/search_time_u",
                      data: "{param1: '" + thisyear + "' , param2 :'" + thismonth + "', param3 :'" + id + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: true,
                      cache: false,
                      success: function (result) {
                          //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                          //console.log(result.d);
                          //window.location.href = "Date_Calendar_success.aspx";
                          //console.log(result.d);
                          $("#date_manger_month_group").append(result.d);
                      },
                      error: function (result) {
                          //console.log(result.Message);
                          //alert(result.d);
                      }

                  });
              }


             $('#datepicker_boo').MonthPicker({
                 Button: function () {
                     return $("<input type='button' value='カレンダー'>").button();
                 },
                 OnAfterChooseMonth: function () {
                     // Do something with selected JavaScript date.
                     //alert($('#datepicker_boo').MonthPicker('GetSelectedMonthYear'));

                     //alert($('#datepicker_boo').MonthPicker('GetSelectedMonth'));
                     //alert($('#datepicker_boo').MonthPicker('GetSelectedYear'));
                     if ("<%= Session["id"]%>" != null) {
                        var id = "<%= Session["id"]%>".toString();

                        var thisyear = $('#datepicker_boo').MonthPicker('GetSelectedYear');
                        $('#thisyear').text(thisyear + "年");
                        var thismonth = $('#datepicker_boo').MonthPicker('GetSelectedMonth');

                        $('#this_month').text(thismonth + "月");
                        var beforemonth = thismonth - 1;
                        if (beforemonth == 0) { beforemonth += 12; }
                        $('#before_month').text(beforemonth + "月");
                        var aftermonth = thismonth + 1;
                        if (aftermonth == 13) { aftermonth -= 12; }
                        $('#after_month').text(aftermonth + "月");


                        $("#date_manger_month_group").empty();

                        $.ajax({
                            type: "POST",
                            url: "user_home.aspx/search_time_u",
                            data: "{param1: '" + thisyear + "' , param2 :'" + thismonth + "', param3 :'" + id + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                //console.log(result.d);
                                //window.location.href = "Date_Calendar_success.aspx";
                                //console.log(result.d);
                                $("#date_manger_month_group").append(result.d);
                            },
                            error: function (result) {
                                //console.log(result.Message);
                                //alert(result.d);
                            }

                        });
                    }
                }
             });
             $("#after_month_img").on("click", function () {

                 if ("<%= Session["id"]%>" != null) {
                      var id = "<%= Session["id"]%>".toString();

                    var thisyear = parseInt($('#thisyear').text().replace("年", ""));
                    var thismonth = parseInt($('#this_month').text().replace("月", ""));
                    thismonth += 1;

                    if (thismonth == 13) { thismonth -= 12; thisyear += 1; }
                    var beforemonth = thismonth - 1;
                    if (beforemonth == 0) { beforemonth += 12; }
                    var aftermonth = thismonth + 1;
                    if (aftermonth == 13) { aftermonth -= 12; }

                    $('#this_month').text(thismonth + "月");
                    $('#before_month').text(beforemonth + "月");
                    $('#after_month').text(aftermonth + "月");
                    $('#thisyear').text(thisyear + "年");



                    $("#date_manger_month_group").empty();

                    $.ajax({
                        type: "POST",
                        url: "user_home.aspx/search_time_u",
                        data: "{param1: '" + thisyear + "' , param2 :'" + thismonth + "', param3 :'" + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            //window.location.href = "Date_Calendar_success.aspx";
                            //console.log(result.d);
                            $("#date_manger_month_group").append(result.d);
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }

                    });
                }
              });


             $("#before_month_img").on("click", function () {

                 if ("<%= Session["id"]%>" != null) {
                    var id = "<%= Session["id"]%>".toString();

                    var thisyear = parseInt($('#thisyear').text().replace("年", ""));
                    var thismonth = parseInt($('#this_month').text().replace("月", ""));
                    thismonth -= 1;

                    if (thismonth == 0) { thismonth += 12; thisyear -= 1; }
                    var beforemonth = thismonth - 1;
                    if (beforemonth == 0) { beforemonth += 12; }
                    var aftermonth = thismonth + 1;
                    if (aftermonth == 13) { aftermonth -= 12; }

                    $('#this_month').text(thismonth + "月");
                    $('#before_month').text(beforemonth + "月");
                    $('#after_month').text(aftermonth + "月");
                    $('#thisyear').text(thisyear + "年");



                    $("#date_manger_month_group").empty();

                    $.ajax({
                        type: "POST",
                        url: "user_home.aspx/search_time_u",
                        data: "{param1: '" + thisyear + "' , param2 :'" + thismonth + "', param3 :'" + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            //window.location.href = "Date_Calendar_success.aspx";
                            //console.log(result.d);
                            $("#date_manger_month_group").append(result.d);
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }

                    });
                }
             });



             var counter = $("#Icounter").val();

                 $("#addButton").click(function () {

                     if (counter > 10) {
                         alert("Only 10 textboxes allow");
                         return false;
                     }

                     $("#KidGroup").append('<div id="KidDiv' + counter + '"><table style="width:100%;"> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>お名前</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <table style="width:100%;"> <tr> <td width="50%"> <input id="real_first_name_text' + counter + '" type="text" name="real_first_name_text' + counter + '" class="textbox" placeholder="姓" style="height:20px;width:90%;" /> <br /><br /><span id="real_first_name_la' + counter + '" style="color: #FF0000"></span> </td> <td width="50%"> <input id="real_second_name_text' + counter + '" type="text" name="real_second_name_text' + counter + '" class="textbox" placeholder="名" style="height:20px;width:90%;" /> <br /><br /><span id="real_second_name_la' + counter + '" style="color: #FF0000"></span> </td> </tr> </table> </td> </tr> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>フリガナ</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <table style="width:100%;"> <tr> <td width="50%"> <input id="real_spell_first_name_text' + counter + '" type="text" name="real_spell_first_name_text' + counter + '" class="textbox" placeholder="セイ" style="height:20px;width:90%;" /> <br /><br /><span id="real_spell_first_name_la' + counter + '" style="color: #FF0000"></span> </td> <td width="50%"> <input id="real_spell_second_name_text' + counter + '" type="text" name="real_spell_second_name_text' + counter + '" class="textbox" placeholder="メイ" style="height:20px;width:90%;" /> <br /><br /><span id="real_spell_second_name_la' + counter + '" style="color: #FF0000"></span> </td> </tr> </table> </td> </tr> <tr> <td width="40%" valign="top"> <table style="width:100%;"> <tr> <td width="50%"> <span>性别</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> </td> <td width="60%"> <input type="radio" name="sex' + counter + '" value="Girl">女性 <input type="radio" name="sex' + counter + '" value="Boy">男性 <br /> <br /><span id="sex_la' + counter + '" style="color: #FF0000"></span> </td> </tr> <tr> <td width="40%" valign="center"> <table style="width:100%;"> <tr> <td width="50%"> <span>生年月日</span> </td> <td width="50%" valign="bottom"> <span style ="color: #FF5050; font-size: XX-Small;">※必須</span> </td> </tr> </table> <br /> </td> <td width="60%" valign="center"> <p><input type="text" name="datepicker' + counter + '" id="datepicker' + counter + '" class="textbox" placeholder="2016/01/01" readonly></p> <script> $(function () { $("#datepicker' + counter + '").datepicker({ format: "yyyy/mm/dd", language: "ja", changeMonth: true, changeYear: true, autoclose: true, clearBtn: true, clear: "閉じる" }); });</' + 'script><br /><span id="date_la' + counter + '" style="color: #FF0000"></span> </td> </tr> <tr> <td> <span>保育園/学校名</span> <br /> <br /> </td> <td> <input name="school_name_text' + counter + '" type="text" id="school_name_text' + counter + '" class="textbox" placeholder="通っている保育園/病院を入力" style="height:20px;width:100%;"> </td> </tr> <tr> <td> <span>かかりつけ医院名</span> <br /> <br /> </td> <td> <input name="hospital_name_text' + counter + '" type="text" id="hospital_name_text' + counter + '" class="textbox" placeholder="病院名/診療所名を入力" style="height:20px;width:100%;"> </td> </tr> <tr> <td> <span>アレルギー/持病</span> <br /> <br /> </td> <td> <textarea name="sick_name_text' + counter + '" rows="2" cols="20" wrap="off" id="sick_name_text' + counter + '" class="textbox" placeholder="例小麦アレルキーなど" style="height:50px;width:100%;"></textarea> </td> </tr> </table> <hr/></div>');



                     counter++;
                 });

                 $("#removeButton").click(function () {
                     if (counter == $("#Icounter").val()) {
                         alert("No more Kid to remove");
                         return false;
                     }

                     counter--;

                     $("#KidDiv" + counter).remove();

                 });

                 $("#update_Button2").click(function () {
                     var checkfin = true;
                     var param1 = "<%= Session["id"]%>".toString();
                     ShowProgressBar();
                for (i = 0; i < counter; i++) {
                    $('#real_first_name_la' + i).text("");
                    $('#real_second_name_la' + i).text("");
                    $('#real_spell_first_name_la' + i).text("");
                    $('#real_spell_second_name_la' + i).text("");
                    $('#date_la' + i).text("");
                    $('#sex_la' + i).text("");


                    if ($('#real_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_spell_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#real_spell_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && $('#datepicker' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() != "" && document.querySelector('input[name="sex' + i + '"]:checked') != null) {

                    } else {
                        checkfin = false;
                        if ($('#real_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $("#real_first_name_la" + i).text("未入力");
                        }
                        if ($('#real_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_second_name_la' + i).text("未入力");
                        }
                        if ($('#real_spell_first_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_spell_first_name_la' + i).text("未入力");
                        }
                        if ($('#real_spell_second_name_text' + i).val().replace("'", "").replace('"', "").replace("`", "").trim() == "") {
                            $('#real_spell_second_name_la' + i).text("未入力");
                        }
                        if ($('#datepicker' + i).val() == "") {
                            $('#date_la' + i).text("未入力");
                        }
                        if (document.querySelector('input[name="sex' + i + '"]:checked') == null) {
                            $('#sex_la' + i).text("未入力");
                        }
                    }
                }


                if (checkfin) {

                    for (i = 0 ; i < $("#Icounter").val() ; i++) {
                        $('#real_first_name_la' + i).text("");
                        $('#real_second_name_la' + i).text("");
                        $('#real_spell_first_name_la' + i).text("");
                        $('#real_spell_second_name_la' + i).text("");
                        $('#date_la' + i).text("");
                        $('#sex_la' + i).text("");
                        $.ajax({
                            type: "POST",
                            url: "user_home.aspx/Save_update",
                            data: "{param_id: '" + $("#kid"+i).val() + "',param1: '" + param1 + "' , param2 :'" + $('#real_first_name_text' + i).val() + "',param3 :'" + $('#real_second_name_text' + i).val() + "',param4 :'" + $('#real_spell_first_name_text' + i).val() + "',param5 :'" + $('#real_spell_second_name_text' + i).val() + "',param6 :'" + $('#datepicker' + i).val() + "',param7 :'" + $('#school_name_text' + i).val() + "',param8 :'" + $('#hospital_name_text' + i).val() + "',param9 :'" + $('#sick_name_text' + i).val() + "',param10 :'" + document.querySelector('input[name="sex' + i + '"]:checked').value + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                //console.log(result.d);
                                //alert(result.d);

                            },
                            error: function (result) {
                                //console.log(result.Message);
                                //alert(result.d);
                            }
                        });
                    }
                    for (i = $("#Icounter").val() ; i < counter; i++) {
                        $('#real_first_name_la' + i).text("");
                        $('#real_second_name_la' + i).text("");
                        $('#real_spell_first_name_la' + i).text("");
                        $('#real_spell_second_name_la' + i).text("");
                        $('#date_la' + i).text("");
                        $('#sex_la' + i).text("");
                        $.ajax({
                            type: "POST",
                            url: "user_home.aspx/Save",
                            data: "{param1: '" + param1 + "' , param2 :'" + $('#real_first_name_text' + i).val() + "',param3 :'" + $('#real_second_name_text' + i).val() + "',param4 :'" + $('#real_spell_first_name_text' + i).val() + "',param5 :'" + $('#real_spell_second_name_text' + i).val() + "',param6 :'" + $('#datepicker' + i).val() + "',param7 :'" + $('#school_name_text' + i).val() + "',param8 :'" + $('#hospital_name_text' + i).val() + "',param9 :'" + $('#sick_name_text' + i).val() + "',param10 :'" + document.querySelector('input[name="sex' + i + '"]:checked').value + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                                //console.log(result.d);
                                //alert(result.d);

                            },
                            error: function (result) {
                                //console.log(result.Message);
                                //alert(result.d);
                            }
                        });
                    }
                }
                HideProgressBar();

            });






         });

         function removediv(click_id) {

             var ind = click_id.indexOf('v');
             var cut = click_id.substr(ind + 1, click_id.length - ind - 1);
             $("#TextBoxDiv" + cut).remove();
         }
         var counter1 = 2;
         function DBplace(postal_code, add) {

             //ver 1
             $('#postal_DIV').empty();
             var myHidden = document.getElementById('<%= HiddenField_postal_one.ClientID %>');
             if (myHidden)//checking whether it is found on DOM, but not necessary
             {
                 myHidden.value = postal_code;
             }
             $('#postal_DIV').append('<span>' + postal_code + "," + add + '</span><br/>');
             //ver 2
             $("#place_group").append('<div id="TextBoxDiv' + counter1 + '"><li id="place_single' + counter1 + '">' + add + '</li><img id="remov' + counter1 + '" onclick="removediv(this.id)" style="cursor: pointer;" src="images/crash.png"><div id="place_postalcode' + counter1 + '" style="visibility:hidden;">' + postal_code + '</div></div>');
             counter1++;
             console.log(postal_code+","+add);
         }

         $(document).ready(function () {
             initMap();

             $("#place_add").click(function () {

                 init();
                 if (document.getElementById("txt3").value != "") {
                     var myHidden = document.getElementById('<%= HiddenField_postal_one.ClientID %>');
                     if (myHidden)//checking whether it is found on DOM, but not necessary
                     {
                         myHidden.value = document.getElementById("txt1").value.replace("'", "").replace('"', "").replace("`", "").trim();
                     }
                     $("#place_group").append('<div id="TextBoxDiv' + counter1 + '"><li id="place_single' + counter1 + '">' + document.getElementById("txt3").value + '</li><img id="remov' + counter1 + '" onclick="removediv(this.id)" style="cursor: pointer;" src="images/crash.png"><div id="place_postalcode' + counter1 + '" style="visibility:hidden;">' + document.getElementById("txt1").value + '</div></div>');
                     counter1++;
                 }
             });

             $("#place_finish").click(function () {
                 $('#postal_DIV').empty();
                 //ver 1
                 var postal_code = "";
                 var myHidden = document.getElementById('<%= HiddenField_postal_one.ClientID %>');
                if (myHidden)//checking whether it is found on DOM, but not necessary
                {
                    myHidden.value = document.getElementById("txt1").value.replace("'", "").replace('"', "").replace("`", "").trim();
                    postal_code = myHidden.value;
                }
                if (document.getElementById("txt3").value != "") {
                    $.ajax({
                        type: "POST",
                        url: "user_home.aspx/Save_place",
                        data: "{param1: '" + postal_code.replace("'", "").replace('"', "").replace("`", "") + "' , param2 :'" + document.getElementById("txt3").value + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            $('#postal_DIV').append('<span>' + result.d + '</span><br/>');
                        },
                        error: function (result) {
                            //console.log(result.Message);
                        }
                    });
                }

                 ////ver 2
                 //for (i = 1; i < counter1; i++) {

                 //    if ($('#place_single' + i) != null && $('#place_postalcode' + i) != null) {
                 //        if ($('#place_single' + i).text().replace("'", "").replace('"', "").replace("`", "") != "" && $('#place_postalcode' + i).text().replace("'", "").replace('"', "").replace("`", "") != "") {
                 //            $.ajax({
                 //                type: "POST",
                 //                url: "user_home.aspx/Save_place",
                 //                data: "{param1: '" + $('#place_postalcode' + i).text().replace("'", "").replace('"', "").replace("`", "") + "' , param2 :'" + $('#place_single' + i).text().replace("'", "").replace('"', "").replace("`", "") + "'}",
                 //                contentType: "application/json; charset=utf-8",
                 //                dataType: "json",
                 //                async: true,
                 //                cache: false,
                 //                success: function (result) {
                 //                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                 //                    //console.log(result.d);
                 //                    $('#postal_DIV').append('<span>' + result.d + '</span><br/>');
                 //                },
                 //                error: function (result) {
                 //                    //console.log(result.Message);
                 //                }
                 //            });
                 //        }

                 //    }
                 //}


                 $("#dialog1").dialog("close");
             });
             //search friend
             var element = document.getElementById('txtSearch_f');
             if (typeof (element) != 'undefined' && element != null) {
                 // exists.


                 $('#txtSearch_f').autocomplete({
                     source: function (request, response) {
                         var param1 = "<%= Session["id"]%>".toString();
                         $.ajax({
                             url: 'test_search.aspx/Getsearch_friend',
                             data: "{ 'prefix': '" + request.term + "','who':'" + param1 + "'}",
                             dataType: 'json',
                             type: 'POST',
                             contentType: 'application/json; charset=utf-8',
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
                         window.location.href = 'user_home_friend.aspx?=' + i.item.val;
                     },
                     minLength: 1
                 });

                 $('#txtSearch_f').data('ui-autocomplete')._renderItem = function (ul, item) {

                     var $li = $('<li>'),
                         $img = $('<img>');

                     $img.attr({
                         src: item.icon,
                         alt: item.label,
                         width: '50px',
                         height: '50px'
                     });
                     $li.attr('data-value', item.label);
                     $li.append('<div>');
                     $li.find('div').append($img).append(item.label);

                     return $li.appendTo(ul);
                 };
             }

         });

         function initMap() {
             // 地圖初始設定
             var mapOptions = {
                 center: new google.maps.LatLng(35.447824, 139.6416613),
                 zoom: 16,
                 mapTypeId: google.maps.MapTypeId.ROADMAP
             };
             var mapElement = document.getElementById("div_showMap");
             // Google 地圖初始化
             map = new google.maps.Map(mapElement, mapOptions);


             var mapOptions1 = {
                 center: new google.maps.LatLng(35.447824, 139.6416613),
                 zoom: 16,
                 mapTypeId: google.maps.MapTypeId.ROADMAP
             };
             var mapElement1 = document.getElementById("div_showMap1");
             // Google 地圖初始化
             map1 = new google.maps.Map(mapElement1, mapOptions1);
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

         function init() {
             var add = document.getElementById("txt1").value;
             $.ajax({
                 url: 'find_city_area.ashx',
                 type: 'post',
                 data: { address: add },
                 async: true,
                 dataType: 'json',
                 success: function (r) {
                     var mess = r.Message;
                     //document.getElementById("txt1").value = mess;
                     if (r.lat == 0 && r.lng == 0) {
                         //$('#div_showMap').hide();
                     } else {
                         //$('#div_showMap').show();
                         MarkerOne1(r);
                     }
                 }
             });

         }

         var infowindow = new google.maps.InfoWindow();
         var map, map1, geocoder, geocoder1;

         function MarkerOne1(item) {
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
                     map1 = new google.maps.Map(document.getElementById("div_showMap1"), myOptions);
                     var imageUrl = ""; //空字串就會使用預設圖示

                     //加一個Marker到map中
                     var marker = new google.maps.Marker({
                         position: latlng,
                         map: map1,
                         draggable: true,
                         icon: imageUrl,
                         html: item.lat + "," + item.lng
                     });
                     geocodePosition1(marker.getPosition());
                     google.maps.event.addListener(marker, 'dragend', function () {
                         geocodePosition1(marker.getPosition());
                     });


                     //markers.push(marker);
                 },
                 error: function (result) {
                     alert(result.responseText);
                 }

             });
         }
         function geocodePosition1(pos) {
             geocoder1 = new google.maps.Geocoder();
             geocoder1.geocode
              ({
                  latLng: pos
              },
                  function (results, status) {
                      if (status == google.maps.GeocoderStatus.OK) {
                          //document.getElementById("txt2").value = results[0].formatted_address;
                          var arrAddress = results[0].address_components;

                          var itemRoute = '';
                          var itemLocality = '';
                          var itemCountry = '';
                          var itemPc = '';
                          var itemSnumber = '';
                          var itempolitical = '';
                          var addr = {};
                          var street_number = route = street = city = state = zipcode = country = city1 = city2 = city3 = formatted_address = '';
                          // iterate through address_component array
                          $.each(arrAddress, function (i, address_component) {
                              //console.log('address_component:' + i);


                              if (address_component.types[0] == "postal_code") {
                                  console.log("pc:" + address_component.long_name);
                                  itemPc = address_component.long_name;
                                  document.getElementById("txt1").value = address_component.long_name;
                              }


                              var types = address_component.types.join(",");


                              if (types == "sublocality,political" || types == "locality,political" || types == "neighborhood,political" || types == "administrative_area_level_3,political") {
                                  addr.city = (city == '' || types == "locality,political") ? address_component.long_name : city;
                              }
                              if (types == "administrative_area_level_1,political") {
                                  addr.state = address_component.short_name;
                              }
                              if (types == "postal_code" || types == "postal_code_prefix,postal_code") {
                                  addr.zipcode = address_component.long_name;
                              }
                              if (types == "country,political") {
                                  addr.country = address_component.long_name;
                              }
                              if (types == "locality,political,ward") {
                                  addr.city1 = (city1 == '' || types == "locality,political,ward") ? address_component.long_name : city1;
                              }
                              if (types == "political,sublocality,sublocality_level_1") {
                                  addr.city2 = (city2 == '' || types == "political,sublocality,sublocality_level_1") ? address_component.long_name : city2;
                              }
                              if (types == "political,sublocality,sublocality_level_2") {
                                  addr.city3 = (city3 == '' || types == "political,sublocality,sublocality_level_2") ? address_component.long_name : city3;
                              }


                              //return false; // break the loop

                          });
                          //for (name in addr) {
                          //    console.log('### google maps api ### ' + name + ': ' + addr[name]);
                          //}
                          var resadd = "";
                          if (addr["state"] != null) {
                              resadd += addr["state"];
                          }
                          if (addr["city"] != null) {
                              resadd += addr["city"];
                          }
                          if (addr["city1"] != null) {
                              resadd += addr["city1"];
                          }
                          if (addr["city2"] != null) {
                              resadd += addr["city2"];
                          }
                          if (addr["city3"] != null) {
                              resadd += addr["city3"];
                          }
                          //console.log(resadd);
                          document.getElementById("txt3").value = resadd;

                      }
                      else {
                          alert('Cannot determine address at this location.' + status);
                      }
                  }
              );
         }

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
                             console.log('address_component:' + i);

                             if (address_component.types[0] == "route") {
                                 console.log(i + ": route:" + address_component.long_name);
                                 itemRoute = address_component.long_name;
                             }

                             if (address_component.types[0] == "locality") {
                                 console.log("town:" + address_component.long_name);
                                 itemLocality = address_component.long_name;
                             }

                             if (address_component.types[0] == "country") {
                                 console.log("country:" + address_component.long_name);
                                 itemCountry = address_component.long_name;
                             }

                             if (address_component.types[0] == "postal_code") {
                                 console.log("pc:" + address_component.long_name);
                                 itemPc = address_component.long_name;
                                 postcode_HiddenField.value = address_component.long_name;
                             }

                             if (address_component.types[0] == "street_number") {
                                 console.log("street_number:" + address_component.long_name);
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


        function check_report_page(click_id) {
             var report_str = "";
             var cut = click_id.indexOf('_');
             report_str = click_id.substr(cut + 1, click_id.length - cut - 1);
             $.ajax({
                 type: "POST",
                 url: "user_home.aspx/report_page",
                 data: "{param1: '" + report_str + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: true,
                 cache: false,
                 success: function (result) {
                     console.log(result.d);
                     window.location.href = result.d;
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
        function score_report(click_id) {
            var report_str = "";
            var cut = click_id.indexOf('_');
            report_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "user_home.aspx/score_page",
                data: "{param1: '" + report_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
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
                url: "user_home.aspx/like_list",
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

        function upload_head_img() {
            var param1 = "<%= Session["id"]%>".toString();
            $.ajax({
                type: "POST",
                url: "user_home.aspx/upload_head",
                data: "{param1: '" + param1 + "',param2: '" + upload_headimg.value + "'}",
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
                             url: "user_home.aspx/search_new_post",
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
                <input id="Button4" type="button" value="閉じる" onclick="dlgLogin_lik_close()" class="file-upload1"/>
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
                <input id="Button1" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload1"/>
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
                                         <td class="icon_left"><span onclick="ShowSidebarLeft()"><i class="fa fa-bars" style="font-size:35px; color: white;cursor: pointer;" ></i></span></td>

   <td class="rin"><asp:Image id="Label_logo" style="height:auto;cursor:pointer;margin-left:3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
<!--hehe-->

                                    </tr>
                                </table>
                            </td>
                            <td class="header-midle">
                                &nbsp;</td>
                            <td class="header-right">
                                <table style="width:100%;">
                                    <tr>
                                        <td class="topnav_right">
                                            <button type="button" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">マイページ</button>
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
                                        <td class="icon_right"><i class="fa fa-bell-o" style="font-size:30px; color: white;cursor: pointer;" onclick="ShowSidebarRight()"></i></td>
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
                         <button type="button" style="border-style: none; background-color:#E7E7E7 ;  cursor: pointer;">マイページ</button>
                    </td>
                </tr>

                <tr>
                    <td>
                         <button onclick="friend_notice()" type="button" style="border-style: none; background-color: #E7E7E7;  cursor: pointer;">友達申請</button>
                    </td>
                </tr>

                <tr>
                    <td>
                         <button onclick="chat_notice()" type="button" style="border-style: none; background-color: #E7E7E7;  cursor: pointer;">メッセ一ジ</button>
                    </td>
                </tr>

                <tr>
                    <td>
                         <button onclick="new_state_notice()" type="button" style="border-style: none; background-color: #E7E7E7; cursor: pointer;">お知らせ</button>
                    </td>
                </tr>
                <tr>
                    <td>
                         <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; background-color: transparent; color: #000000; cursor: pointer;"ヘルプ</button>
                    </td>
                </tr>
            </tbody>
         </table>


</div><!--End sidebar_right-->
</div><!--End div_sidebar_right-->
            <div id="content">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:HiddenField ID="upload_headimg" runat="server" />
                            <asp:Panel ID="head_homepage_user" runat="server"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF">
                            <asp:Panel ID="send_homepage_user" runat="server">
                                <table style="border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;width: 100%; height: 100%;">
                     <tr>
                         <td>
                             <table style="width: 100%; height: 100%;">
                                 <tr>
                                     <td class="auto-style1">&nbsp;</td>
                                     <td align="right" width="5%" valign="bottom">
                                         <asp:Image ID="Image2" runat="server" ImageUrl="~/images/photo.png" Height="20px" Width="20px" />
                                         <br/>
                                     </td>
                                         <td align="left" valign="bottom">
                                     <label class="file-upload">
            <span><strong>写真‧動画</strong></span>
            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);"  AllowMultiple="True"/>
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
                                     <td align="left" height="60px" width="8%">
                                         <asp:Panel ID="userphoto" runat="server"></asp:Panel>
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
                                     <td class="auto-style1"><img alt="" src="images/tag.png" height="15px" width="15px" /></td>
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
                                     <td width="23%">
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
  <div class="ui inline dropdown">
    <div class="text">
      <img class="ui avatar image" src="images/icon/public.png"  height="15px" width="15px" >
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
                             <asp:Button ID="post_message_Button" runat="server" BackColor="#000099" BorderStyle="None" ForeColor="White" Text="投稿" Width="95%" OnClick="post_message_Button_Click" height="25px" />
                                     </td>
                                 </tr>
                             </table>
                         </td>
                     </tr>
                 </table>



                            </asp:Panel>
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
                            <asp:Panel ID="homepage_user2" runat="server" Visible="False">

                                <table style="width: 100%; background-color: #666666;">
                                    <tr>
                                        <td style="width: 20%; background-color: #FFFFFF;border-style: solid;border-width:thin;" valign="top" align="left">
                                            <table style="width: 100%; height: 100%;">
                                                <tr>
                                                    <td height="20px">

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="userhome_Button1" runat="server" Text="基本情報" BackColor="White" BorderStyle="None" OnClientClick="ShowProgressBar();" Height="100%" Width="100%" OnClick="userhome_Button1_Click" CssClass="stbutton" />
                                                    <br/><br/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="userhome_Button2" runat="server" Text="利用者情報" BackColor="White" BorderStyle="None" OnClientClick="ShowProgressBar();" Height="100%" Width="100%" OnClick="userhome_Button2_Click" CssClass="stbutton" ForeColor="#999999" />
                                                    <br/><br/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="userhome_Button3" runat="server" Text="パスワード" BackColor="White" BorderStyle="None" OnClientClick="ShowProgressBar();" Height="100%" Width="100%" OnClick="userhome_Button3_Click" CssClass="stbutton" ForeColor="#999999" />
                                                    <br/><br/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 80%; background-color: #FFFFFF;">
                                            <asp:Panel ID="user_information" runat="server">
                                                <table style="width: 100%; border-style: solid;border-width:thin;">
                                                    <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">

                    <table style="width:100%;">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050" Text="基本情報"></asp:Label>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="40%">
                                            <asp:Label ID="Label3" runat="server" Text="二ックネーム"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label4" runat="server" Font-Size="XX-Small" ForeColor="#CCCCCC" Text="※公開されます"></asp:Label>

                            </td>
                            <td width="60%">
                                <asp:TextBox ID="name_TextBox" runat="server" CssClass="textbox" Height="20px" placeholder="二ックネーム" Width="100%" Wrap="False"></asp:TextBox>
                                <br />
                                <asp:Label ID="name_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                           <td>
                               <br />
                               <asp:Label ID="Label1" runat="server" Text="アイコンの写真"></asp:Label>
                                <br />
                                           <asp:Label ID="Label6" runat="server" Font-Size="XX-Small" ForeColor="#CCCCCC " Text="※公開されます"></asp:Label>
                           </td>
                           <td>
                               <br />
           <label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
           <input type='file' name='file' id='btnFileUpload_photo' />
</label>
<br />
           <div id='progressbar_photo' style='width:100px;display:none;'>
               <div>
                   読み込み中
               </div>
           </div>
<br />
               <div id='image_place_photo' style='width:100px;display:none;'>
                   <div>
                       <img id='make-image_photo' alt='' src='' width='100px' height='100px'/>
                       <asp:HiddenField ID="HiddenField_for_photo" runat="server" />
                   </div>
               </div>
                               <br />
                               <script>

                                   $(function () {
                                       $('#btnFileUpload_photo').fileupload({
                                           url: 'FileUploadHandler.ashx?upload=start',
                                           add: function (e, data) {
                                               console.log('add', data);
                                               $('#progressbar_photo').show();
                                               $('#image_place_photo').hide();
                                               $('#image_place_photo div').css('width', '0%');
                                               data.submit();
                                           },
                                           progress: function (e, data) {
                                               var progress = parseInt(data.loaded / data.total * 100, 10);
                                               $('#progressbar_photo div').css('width', progress + '%');
                                           },
                                           success: function (response, status) {
                                               $('#progressbar_photo').hide();
                                               $('#progressbar_photo div').css('width', '0%');
                                               $('#image_place_photo').show();
                                               document.getElementById('make-image_photo').src = response;
                                               HiddenField_for_photo.value = response;
                                               console.log('success', response);
                                           },
                                           error: function (error) {
                                               $('#progressbar_photo').hide();
                                               $('#progressbar_photo div').css('width', '0%');
                                               $('#image_place_photo').hide();
                                               $('#image_place_photo div').css('width', '0%');
                                               console.log('error', error);
                                           }
                                       });
                                   });

                               </script>
                           </td>
                       </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Label ID="Label11" runat="server" Text="地域の設定"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="HiddenField_postal_one" runat="server" />
                                <br />
                                <div id="postalcode" style="border-style: solid; border-width: thin; width: 100%; height: 8%;text-align:center; cursor: pointer;">
                                     <div id="postal_DIV"></div>
                                    <span style="color:blue;">表示エリアの設定．変更</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table style="width:100%;">
                        <tr>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                         <tr>
                                <td align="center">
                                    <asp:Label ID="result_Label0" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                    <br />
                                <asp:Button ID="update_Button1" runat="server" Text="保存" CssClass="file-upload1"
                                    Width="50%" OnClientClick="ShowProgressBar();" OnClick="update_Button1_Click" />
                             </td>
                        </tr>
                        </table>
                </td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
                                                </table>


                                            </asp:Panel>
                                            <asp:Panel ID="user_information2" runat="server" Visible="False">
                                                <table style="width: 100%;">
                                                    <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">

                    <table style="width:100%;">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="#FF5050" Text="お子様の情報"></asp:Label>
                                &nbsp;<br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                             <asp:Panel ID="KidGroup1" runat="server"></asp:Panel>

                                 <br />
                                 <br />
                                </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="KidGroup">

                                </div>
                                                <br /><br />
                             </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:HyperLink id='addButton' runat="server" NavigateUrl="javascript:void(0);"
                                    Target="_blank" Font-Size="Small" Font-Underline="False">+別のお子様を追加</asp:HyperLink>
                                                <br />
                                    <asp:HyperLink id='removeButton' runat="server" NavigateUrl="javascript:void(0);"
                                    Target="_blank" Font-Size="Small" Font-Underline="False">-別のお子様を削除</asp:HyperLink>
                                                <br /><br />
                             </td>
                        </tr>
                    </table>
                    <table style="width:100%;">
                        <tr>
                            <td>&nbsp;</td>
                            <td align="center">
                                <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                    Text="クレジットカードのご登録"></asp:Label>
                                &nbsp;
                                <asp:Label ID="Label13" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
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
                                            <td style="width: 20%">
                                                <asp:Label ID="Label16" runat="server" Text="カード番号"></asp:Label>
                                                <br />
                                                <br />
                                            </td>
                                            <td colspan="2">
                                <asp:TextBox ID="TextBox4" runat="server" Width="100%" Wrap="False" placeholder="カード番号をハイフン無しで記入"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
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
                                                <asp:DropDownList ID="DropDownList1" runat="server" Width="50px" Height="30px">
                                                </asp:DropDownList>
&nbsp;<asp:Label ID="Label20" runat="server" Text="/"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList2" runat="server" Width="50px" Height="30px">
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                            <td style="width: 50%" valign="top">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width: 40%" valign="top">
                                                <asp:Label ID="Label18" runat="server" Text="確認コード"></asp:Label>
                                                            <br />
                                                        </td>
                                                        <td style="width: 60%" valign="top">
                                <asp:TextBox ID="TextBox6" runat="server" Width="100%" Wrap="False"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label21" runat="server" Text="名義人"></asp:Label>
                                                <br />
                                                <br />
                                            </td>
                                            <td colspan="2" align="center">
                                <asp:TextBox ID="TextBox7" runat="server" Width="100%" Wrap="False" placeholder="カードの名義人を入力"
                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="Label22" runat="server" Font-Size="Small" ForeColor="#666666" Text="※力一ドに記載してあるとリ入力してください"></asp:Label>
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
                                <input id="update_Button2" type="button" value="保存" class="file-upload1" style="width:50%;" />
                                <br />
                             </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                 </td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
                                                </table>



                                            </asp:Panel>
                                            <asp:Panel ID="user_information3" runat="server" Visible="False">
                                                <table style="width: 100%;">
                                                    <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">

                    <table style="width:100%;">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label37" runat="server" Font-Bold="True" ForeColor="#FF5050" Text="ログイン パスワードの変更"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label8" runat="server" Text="現在のパスワード"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="password_TextBox_now" runat="server" CssClass="textbox" Height="20px" TextMode="Password" Width="100%" Wrap="False"></asp:TextBox>
                                <br />
                                <asp:Label ID="now_password_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="40%">
                                <asp:Label ID="Label39" runat="server" Text="新しいパスワード"></asp:Label>
                            </td>
                            <td valign="top" width="60%">
                                <asp:TextBox ID="password_TextBox" runat="server" CssClass="textbox" Height="20px" TextMode="Password" Width="100%" Wrap="False"></asp:TextBox>
                                <br />

                                <asp:Label ID="password_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label40" runat="server" Text="パスワード再確認"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="password_TextBox_check" runat="server" CssClass="textbox" Height="20px" TextMode="Password" Width="100%" Wrap="False"></asp:TextBox>
                                <br />
                                <asp:Label ID="c_password_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" width="100%" align="center">
                                <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Button ID="update_Button3" runat="server" CssClass="file-upload1" OnClientClick="ShowProgressBar();" Text="更新" Width="50%" OnClick="update_Button3_Click" />
                            </td>
                        </tr>
                    </table>

                 </td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td width="10%">
                    &nbsp;</td>
                <td width="80%">
                    &nbsp;</td>
                <td width="10%">
                    &nbsp;</td>
            </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>


                            </asp:Panel>
                            <asp:Panel ID="homepage_user3" runat="server" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="border-width: thin; border-style: solid; background-color: #DDDDDD; width: 100%; height: 50px;">
                                            <table style="width: 100%; height: 100%;">
                                                <tr>
                                                    <td style="width: 5%" align="right">&nbsp;</td>
                                                    <td style="width: 10%" align="left">

                                                        <asp:Button ID="Button13" runat="server" Text="すべての友達" BorderStyle="None" ForeColor="Black" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" />

                                                    </td>
                                                    <td style="width: 15%" align="right">

                                                        &nbsp;</td>
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
                            <asp:Panel ID="homepage_user4" runat="server" Visible="False">

                            </asp:Panel>
                        </td>
                    </tr>
                </table>


                　</div>
</div>
    <div>
    <div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
    <br />
    <font color="#95989A" size="2px">読み込み中</font>
</div>
<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
    position: absolute; top: 0px;">
</div>
        <asp:Panel ID="javaplace_formap" runat="server"></asp:Panel>
        <asp:Panel ID="javaplace" runat="server">
            </asp:Panel>
    <div id="dialog" title="場所">
                    <table style="width: 100%;" align="center">
                        <tr>
                            <td>
                                <INPUT id="txt2" TYPE="text" onKeydown="Javascript: if (event.which == 13 || event.keyCode == 13) runScript();" placeholder="住所を入力" style="width: 100%;" title="【Enter】キーを押してください"><br />
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

 <div id="dialog1" title="">
                    <table style="width: 100%;" align="center">

                        <tr>
                            <td align="center">
                                <div id="div_showMap1" style="width: 100%; height: 210px">
                                 </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                 <div style="visibility:hidden;">
                                <span>現在選択中のエリア</span>
                                <hr/>
                                <div id="place_group" style="width:100%;height:1px;overflow: scroll;">
                                <%--<div id="place_group" style="width:100%;height:90px;overflow: scroll;">--%>
                                 </div>
                                 </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <INPUT id="txt1" TYPE="text" onKeydown="Javascript: if (event.which == 13 || event.keyCode == 13) init();" placeholder="〒郵便番号追加" style="width: 100%"><br />
                                <INPUT id="txt3" TYPE="text" value="" style="width: 100%" readonly>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="place_add" type="button" value="変更" class="file-upload1" style="width: 100%" /><br/>
                                <input id="place_finish" type="button" value="設定完了" class="file-upload1" style="width: 100%" />
                            </td>
                        </tr>
                    </table>
</div>


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
                var menu = document.getElementById('div_sidebar_right');
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
