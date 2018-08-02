<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_date_manger.aspx.cs" Inherits="user_date_manger" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>

    <script src="//code.jquery.com/jquery-latest.js"></script>

    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery.fileupload.css">
    <!-- Magnific Popup core CSS file -->
    <link rel="stylesheet" href="css/magnific-popup.css">
    <link href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="css/bootstrap-theme.css" />
    <link rel="stylesheet" type="text/css" href="css/our/style_mypage_profile_user_info.css" />

    <link href="css/MonthPicker.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/style.css" />
    <link rel="stylesheet" href="css/user_date_manager.css">
    <!-- Magnific Popup core JS file -->
    <script src="js/jquery.magnific-popup.js"></script>

    <%--<script src="Scripts/jquery-ui.js"></script>--%><%--<link rel="stylesheet" href="css/jquery-ui.css">--%>
    <script src="https://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>

    <script src="Scripts/datepicker-ja.js"></script>

    <script type="text/javascript" src="Scripts/jquery.youtubepopup.min.js"></script>


    <script src="js/MonthPicker_jp.js"></script>


    <link type="text/css" href="js/agency.js" />


    <script>

        //new state message notice
        $(window).load(function () {
            var param1 = "<%= Session["id"]%>".toString();
            if (param1 != "") {
                $.ajax({
                    type: "POST",
                    url: "user_date_manger.aspx/count_list",
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
                url: "user_date_manger.aspx/new_state_list",
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
                        url: "user_date_manger.aspx/new_state_notice_list_scroll",
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
                url: "user_date_manger.aspx/chat_notice_list",
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
                 url: "user_date_manger.aspx/friend_notice_list",
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
                 url: "user_date_manger.aspx/search_friend_notice_list",
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
                 url: "user_date_manger.aspx/friend_notice_check",
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
                 url: "user_date_manger.aspx/friend_notice_check_del",
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
                url: "user_date_manger.aspx/friend_notice_donotfind",
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
                url: "user_date_manger.aspx/friend_notice_addfind",
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
                url: "user_date_manger.aspx/toget_friend_list",
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
                        url: "user_date_manger.aspx/search_friend_notice_list_scroll",
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


        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUploadDoc.ClientID %>").click();
             }
         }
         $(function () {
             $('#report_panel').hide();
             $('#first_re_pan').hide();
             $('#second_re_pan').hide();
             $('#third_re_pan').hide();
             $('#fail_pan').hide();


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
                    url: "user_date_manger.aspx/search_time",
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



            $("#datepicker").datepicker({
                format: 'yyyy/mm/dd',
                language: 'ja',
                autoclose: true, // これ
                clearBtn: true,
                clear: '閉じる'
            });
            $("#datepicker0").datepicker({
                format: 'yyyy/mm/dd',
                language: 'ja',
                autoclose: true, // これ
                clearBtn: true,
                clear: '閉じる'
            });

            $('#datepicker_boo').MonthPicker({
                Button: function () {
                    return $("<input widht='40px' height='40px' type='button' value='カレンダー'>").button();
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
                            url: "user_date_manger.aspx/search_time",
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
                        url: "user_date_manger.aspx/search_time",
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
                        url: "user_date_manger.aspx/search_time",
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
            $("#dialog").dialog({
                autoOpen: false,
                width: 320,
                position: { my: "right top", at: "left+" + ((window.innerWidth / 2) + (window.innerWidth / 3)) + " top+300", of: "body" },
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $(".opener").on("click", function () {
                //alert($(this).attr("id"));
                select_day_HiddenField.value = $(this).attr("id");
                var str = $(this).attr("id");
                var cut = str.indexOf(".");
                var year = str.substr(0, cut);
                var str1 = str.substr(cut + 1, str.length - cut - 1);
                var cut1 = str1.indexOf(".");
                var month = str1.substr(0, cut1);
                var day = str1.substr(cut1 + 1, str1.length - cut1 - 1);
                document.getElementById('datepicker').value = year + "/" + month + "/" + day;
                document.getElementById('datepicker0').value = year + "/" + month + "/" + day;
                $("#dialog").dialog("open");

            });


            $("#place_select").on("click", function () {
                if ("<%= Session["id"]%>" != null) {
                    var param1 = "<%= Session["id"]%>".toString();

                    //$("#addmsg").append(document.getElementById("txt2").value).show();
                    //alert(select_day_HiddenField.value + "," + $("input[name=radios]:checked").val() + "," + document.getElementById('datepicker').value + "," + document.getElementById('datepicker0').value + "," + $("#app_start_hour_DropDownList").val() + "," + $("#app_start_minute_DropDownList").val() + "," + $("#app_end_hour_DropDownList").val() + "," + $("#app_end_minute_DropDownList").val());
                    $.ajax({
                        type: "POST",
                        url: "user_date_manger.aspx/Save_date",
                        data: "{param1: '" + param1 + "',param2 :'" + $("input[name=radios]:checked").val() + "',param3 :'" + document.getElementById('datepicker').value + "',param4 :'" + document.getElementById('datepicker0').value + "',param5 :'" + $("#app_start_hour_DropDownList").val() + "',param6 :'" + $("#app_start_minute_DropDownList").val() + "',param7 :'" + $("#app_end_hour_DropDownList").val() + "',param8 :'" + $("#app_end_minute_DropDownList").val() + "'}",
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
                $("#dialog").dialog("close");
                location.reload();
            });





            $("img.youtube").YouTubePopup({ idAttribute: 'id' });

            $("a.youtube").YouTubePopup({ autoplay: 1 });



            //    $("#datepicker_boo").datepicker({
            //        language: 'ja',
            //        autoclose: true, // これ
            //        clearBtn: true,
            //        clear: '閉じる',
            //        changeMonth: true,
            //changeYear: true,
            //showButtonPanel: true,
            //dateFormat: 'MM yy',
            //onClose: function(dateText, inst) {
            //        $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            //    }
            //    });


        });


        // 顯示讀取遮罩


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

        $(window).resize(function () {
            var w = $(window).width();
            var h = $(document).height();
            var maskFrame = $("#divMaskFrame");
            maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
            w = $(document).width();
            h = $(window).height();
            var progress = $('#divProgress');
            progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
        });
        function check_report(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var report_str = "";
            var cut = click_id.indexOf('_');
            report_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            var chat_dealid = document.getElementById('<%= chat_deal_id.ClientID %>');
            if (chat_dealid)//checking whether it is found on DOM, but not necessary
            {
                chat_dealid.value = report_str;
            }
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/report_build",
                data: "{param1: '" + param1 + "',param2: '" + report_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#user_infor').empty();
                    $('#user_infor').append(result.d.infor_pan);

                    $('#user_infor_money').empty();
                    $('#user_infor_money').append(result.d.infor_pan_money);

                    $('#date_infor').empty();
                    $('#date_infor').append(result.d.date_pan);


                    $('#date_infor1').empty();
                    $('#date_infor1').append(result.d.date_pan);


                    __doPostBack('<%=UpdatePanel1.ClientID%>', '');

                    //$('#report_panel').append(result.d.infor_pan);
                    //$('#report_panel').append(result.d.date_pan);


                    //console.log(result.d);
                    $('#report_panel').show();
                    $('#first_re_pan').show();

                    $('#date_manger_month').hide();
                    $('#date_manger_Panel1').hide();

                    //var whitebg = document.getElementById("white-background");
                    //var dlg = document.getElementById("dlgbox_firend");
                    //whitebg.style.display = "block";
                    //dlg.style.display = "block";


                    //var winWidth = window.innerWidth;
                    //var winHeight = window.innerHeight;

                    //dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
                    //dlg.style.top = winHeight / 10 + "px";



                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
            ShowProgressBar();
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/chat_room_mess",
                data: "{param1: '" + param1 + "',param2: '" + report_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#chat_Panel2').empty();
                    $('#chat_Panel2').append(result.d);
                    //console.log(result.d);
                    HideProgressBar();
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                    HideProgressBar();
                }
            });

        }
        function chat_insert() {
            var param1 = "<%= Session["id"]%>".toString();
            var report_str = "";
            var chat_dealid = document.getElementById('<%= chat_deal_id.ClientID %>');
            if (chat_dealid)//checking whether it is found on DOM, but not necessary
            {
                report_str = chat_dealid.value;
            }
            var mess = document.getElementById('message').value.replace(/\r?\n/g, '<br />');
            document.getElementById('message').value = "";
            //console.log(report_str);
            //console.log(mess);
            ShowProgressBar();
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/chat_room_mess_send",
                data: "{param1: '" + param1 + "',param2: '" + report_str + "',param3: '" + mess + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#chat_Panel2').empty();
                    $('#chat_Panel2').append(result.d);
                    //console.log(result.d);
                    HideProgressBar();
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                    HideProgressBar();
                }
            });

        }
        function report_fail() {
            $('#fail_pan').show();
        }
        function report_create_fail() {
            var param1 = "<%= Session["deal_id"]%>".toString();
            $('#why_la').text("");
            if (document.querySelector('input[name="why"]:checked') == null) {
                $('#why_la').text("未入力");
            } else {
                $('#why_la').text("");
                var radio_v = document.querySelector('input[name="why"]:checked').value;
                var check = true;
                console.log(radio_v);
                console.log($('#fail_text').val());
                if (radio_v == "その他") {
                    check = false;
                    var str = $('#fail_text').val();
                    console.log(str.trim());
                    console.log(check);
                    if (str.trim() != "") {
                        check = true;
                        console.log(str.trim());
                        console.log(check);
                    } else {
                        $('#why_la').text("その他 メッセージ未入力");
                    }
                }
                console.log(check);
                if (check == true) {
                    console.log("5");
                    $.ajax({
                        type: "POST",
                        url: "user_date_manger.aspx/report_build_fail",
                        data: "{param1: '" + param1 + "',param2: '" + radio_v + "',param3: '" + $('#fail_text').val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            window.location.href = "user_date_manger.aspx";
                        },
                        error: function (result) {
                            console.log(result.d);
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }
            }
        }
        function report_success() {
            var param1 = "<%= Session["deal_id"]%>".toString();
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/report_build_success",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result

                    window.location.href = "user_date_manger.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }

        function report_create(click_id) {
            var param1 = "<%= Session["id"]%>".toString();
            var report_str = "";
            var cut = click_id.indexOf('_');
            report_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/report_build_report",
                data: "{param1: '" + param1 + "',param2: '" + report_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#user_infor').empty();
                    $('#user_infor').append(result.d.infor_pan);

                    $('#user_infor_money').empty();
                    $('#user_infor_money').append(result.d.infor_pan_money);

                    $('#date_infor_report').empty();
                    $('#date_infor_report').append(result.d.date_pan);


                    $('#date_infor1').empty();
                    $('#date_infor1').append(result.d.date_pan);

                    $('#date_infor_report_make').empty();
                    $('#date_infor_report_make').append(result.d.date_pan_make);

                    __doPostBack('<%=UpdatePanel1.ClientID%>', '');

                    //$('#report_panel').append(result.d.infor_pan);
                    //$('#report_panel').append(result.d.date_pan);


                    //console.log(result.d);

                    $('#report_panel').show();
                    $('#second_re_pan').show();
                    $('#date_manger_month').hide();
                    $('#date_manger_Panel1').hide();


                    //var whitebg = document.getElementById("white-background");
                    //var dlg = document.getElementById("dlgbox_firend");
                    //whitebg.style.display = "block";
                    //dlg.style.display = "block";


                    //var winWidth = window.innerWidth;
                    //var winHeight = window.innerHeight;

                    //dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
                    //dlg.style.top = winHeight / 10 + "px";



                    //window.location.href = "Date_Calendar_success.aspx";


                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });

        }

        function report_create_page() {
            $('#date_infor1').empty();
            $('#date_infor1').append($('#date_infor_report_make').html());
            $('#report_panel').show();
            $('#second_re_pan').hide();
            $('#third_re_pan').show();
            $('#date_manger_month').hide();
            $('#date_manger_Panel1').hide();

        }
        function report_create_success() {
            var param1 = "<%= Session["deal_id"]%>".toString();
            var param2 = $('#success_text').val();
            $.ajax({
                type: "POST",
                url: "user_date_manger.aspx/report_build_success_content",
                data: "{param1: '" + param1 + "',param2: '" + param2 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    console.log(result.d);
                    window.location.href = "user_date_manger.aspx";
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }
    </script>
    <style type="text/css">
        #new_state_count {
            display: none;
        }

        #chat_count {
            display: none;
        }

        #friend_count {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="white-background">
        </div>
        <div id="dlgbox_tofriend_list" class="dlg">
            <div id="dlg-header_tofriend_list" class="dlgh">共通の友達</div>
            <div id="dlg-body_tofriend_list" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_tofriend_list" class="dlgf">
                <input id="Button5" type="button" value="閉じる" onclick="dlgLogin_tofri_close()" class="file-upload" />
            </div>
        </div>
        <div id="dlgbox_firend" class="dlg">
            <div id="dlg-header_friend" class="dlgh">友達承認</div>
            <div id="dlg-body_friend" style="height: 350px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_friend" class="dlgf">
                <input id="Button2" type="button" value="閉じる" onclick="dlgLogin_fri_close()" class="file-upload" />
            </div>
        </div>

        <div id="dlgbox">
            <div id="dlg-header">メッセ一ジ</div>
            <div id="dlg-body" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer">
                <input id="Button3" type="button" value="閉じる" onclick="dlgLogin_chat_notice_close()" class="file-upload" />
            </div>
        </div>



        <div id="dlgbox1">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 350px; overflow: auto">
            </div>
            <div id="dlg-footer1">
                <input id="Button1" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload" />
            </div>
        </div>
        <div>
            <div id="header" style="z-index: 999998;">
                <table style="width: 100%; height: 100%">
                    <tr>
                        <td class="header-left" align="left">

                            <table style="width: 100%;">
                                <tr>
                                    <td width="5%">&nbsp;</td>
                                    <td class="rin">
                                        <asp:Image ID="Label_logo" Style="height: auto;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                    <!--hehe-->
                                </tr>
                            </table>
                        </td>
                        <td class="header-midle">&nbsp;</td>
                        <td class="header-right">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="topnav_right">
                                        <button type="button" onclick="gotomy()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">マイページ</button>
                                    </td>
                                    <td class="topnav_right">
                                        <div style="position: relative;">
                                            <div id="friend_count" style="border-radius: 6px 6px 6px 0px; background-color: red; min-width: 20px; min-height: 20px; position: absolute; top: 16px; line-height: 100%; right: 5px;">&nbsp;<span style="color: white;">1</span>&nbsp;</div>
                                            <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">友達申請</button>
                                        </div>
                                    </td>
                                    <td class="topnav_right">
                                        <div style="position: relative;">
                                            <div id="chat_count" style="border-radius: 6px 6px 6px 0px; background-color: red; min-width: 20px; min-height: 20px; position: absolute; top: 16px; line-height: 100%; right: 5px;">&nbsp;<span style="color: white;">1</span>&nbsp;</div>
                                            <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">メッセ一ジ</button>
                                        </div>
                                    </td>
                                    <td class="topnav_right">
                                        <div style="position: relative;">
                                            <div id="new_state_count" style="border-radius: 6px; background-color: red; min-width: 20px; min-height: 20px; position: absolute; top: 16px; line-height: 100%; right: 5px;">&nbsp;<span style="color: white;">1</span>&nbsp;</div>
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
            <div id="header1">
                <table style="width: 100%; height: 100%;">
                    <tr>
                        <td aligh="right" width="18%">
                            <asp:Button ID="date_manger_b1" runat="server" Text="予約状況" BackColor="Black" BorderStyle="None" ForeColor="#999999" Height="85%" OnClientClick="ShowProgressBar();" Width="60%" OnClick="date_manger_b1_Click" />
                            <br />
                            <asp:Image ID="date_manger_im1" runat="server" Height="10%" ImageUrl="~/images/block.PNG" Width="60%" Visible="False" />
                        </td>
                        <td aligh="right" width="18%">
                            <asp:Button ID="date_manger_b2" runat="server" Text="予約枠の変更" BackColor="Black" BorderStyle="None" ForeColor="#999999" Height="85%" OnClientClick="ShowProgressBar();" Width="60%" OnClick="date_manger_b2_Click" />
                            <br />
                            <asp:Image ID="date_manger_im2" runat="server" Height="10%" ImageUrl="~/images/block.PNG" Width="60%" Visible="False" />
                        </td>
                        <td aligh="right" width="18%">
                            <asp:Button ID="date_manger_b3" runat="server" Text="登録情報の変更" BackColor="Black" BorderStyle="None" ForeColor="#999999" Height="85%" OnClientClick="ShowProgressBar();" Width="60%" OnClick="date_manger_b3_Click" />
                            <br />
                            <asp:Image ID="date_manger_im3" runat="server" Height="10%" ImageUrl="~/images/block.PNG" Width="60%" Visible="False" />
                        </td>
                        <td aligh="right" width="18%">
                            <asp:Button ID="date_manger_b4" runat="server" Text="実績" BackColor="Black" BorderStyle="None" ForeColor="#999999" Height="85%" OnClientClick="ShowProgressBar();" Width="60%" OnClick="date_manger_b4_Click" />
                            <br />
                            <asp:Image ID="date_manger_im4" runat="server" Height="10%" ImageUrl="~/images/block.PNG" Width="60%" Visible="False" />
                        </td>
                        <td aligh="right" width="18%">
                            <asp:Button ID="date_manger_b5" runat="server" Text="講習動画" BackColor="Black" BorderStyle="None" ForeColor="#999999" Height="85%" OnClientClick="ShowProgressBar();" Width="60%" OnClick="date_manger_b5_Click" />
                            <br />
                            <asp:Image ID="date_manger_im5" runat="server" Height="10%" ImageUrl="~/images/block.PNG" Width="60%" Visible="False" />
                        </td>
                        <td width="10%"></td>
                    </tr>
                </table>

            </div>
           <div id="div_sidebar_right">
　<div id="sidebar_right">
      <div id="show_map_area" align="center" style="width: 100%; height: 100%">
      </div>

          <div id="menu_right">
        <table style="width: 100%; margin-top: 5em;">
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
            <asp:HiddenField ID="chat_deal_id" runat="server" />
            <asp:Panel ID="date_manger_Panel1" runat="server">
            </asp:Panel>
            <asp:Panel ID="report_panel" runat="server">
                <div class="container-fluid main_body">
                    <div class="row">
                        <div class="col-md-4 div_left">
                            <div id="date_infor1"></div>

                            <div id="user_infor"></div>
                            <div id="small_calendar">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Calendar ID="Calendar2" runat="server" OnDayRender="Calendar2_DayRender" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="100%" NextPrevFormat="FullMonth" Width="100%">
                                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="text-center" />
                                            <DayStyle CssClass="calendar_size" />
                                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                            <OtherMonthDayStyle ForeColor="#999999" />
                                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                            <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                                            <TodayDayStyle BackColor="#CCCCCC" />
                                        </asp:Calendar>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <br />
                            <br />
                            <div id="user_infor_money"></div>
                        </div>
                        <!--div_left end-->


                        <div class="col-md-8">
                            <div id="third_re_pan">
                                <div class="col-md-12 ">
                                    <div class="row">
                                        <div class="col-md-12" style="text-align: center;">
                                            <br />
                                            <br />
                                            <div id="date_infor_report_make" style="text-align: left;"></div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                            <div id="second_re_pan">
                                <div class="col-md-12 ">
                                    <div class="row">
                                        <div class="col-md-12" style="text-align: center;">
                                            <br />
                                            <br />
                                            <div id="date_infor_report" style="text-align: left;"></div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                            <div id="first_re_pan">
                                <div class="col-md-12 ">
                                    <div class="row">
                                        <div class="col-md-12" style="text-align: center;">
                                            <br />
                                            <br />
                                            <div id="date_infor" style="text-align: center;"></div>

                                            <p><span>報酬のお支払い先の口座に関しては、予めご確認ください。</span><span><a style="color: #00129D" href="">確認はこちらから</a></span></p>
                                            <p>面談が済んでいない場合、やむを得ない理由でお断りをする場合は、右下のボタンから、依頼者に連絡をお願いします。</p>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-6">
                                            <button type="button" onclick="report_success()" style="width: 100%; text-shadow: none; cursor: pointer; text-align: center;" class="file-upload">確認してメッセ時を送る</button>
                                        </div>
                                        <div class="col-md-6">
                                            <button type="button" onclick="report_fail()" style="width: 100%; background-color: #95989A; color: white; border-color: #95989A; text-shadow: none; cursor: pointer; text-align: center;" class="file-upload">お断りする</button>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" id="fail_pan">
                                            <h3><span style='color: #EA9494; font-weight: bold;'>お断りする理由を選んでください。</span></h3>
                                            <br />
                                            <br />

                                            <input type="radio" name="why" value="面談がまだすんでいません。"><span>面談がまだすんでいません。</span>
                                            <br />
                                            <br />
                                            <input type="radio" name="why" value="急な予定が入ってしまったため。"><span>急な予定が入ってしまったため。</span>
                                            <br />
                                            <br />
                                            <input type="radio" name="why" value="依頼者にキャンセルを申し出られました。"><span>依頼者にキャンセルを申し出られました。</span>
                                            <br />
                                            <br />
                                            <input type="radio" name="why" value="依頼者に不安を感じる。"><span>依頼者に不安を感じる。</span>
                                            <br />
                                            <br />
                                            <input type="radio" name="why" value="その他"><span>その他</span>
                                            <br />
                                            <br />
                                            <span>メッセージ</span><br />
                                            <br />
                                            <textarea name='fail_text' rows='4' cols='20' wrap='hard' id='fail_text' class='textbox' placeholder='コメントを入力する…' style='height: 100px; width: 100%;'></textarea><br />
                                            <br />

                                            <br />
                                            <span id="why_la" style="color: #FF0000"></span>

                                            <input type='button' value='送信' onclick='report_create_fail()' style='width: 100%; text-shadow: none; cursor: pointer; text-align: center;' class='file-upload'>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                        <div class="col-md-6" height="10px">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                            <div class="row div_right_bottom">
                                <div class="col-md-12 ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td id="chat_window" class="chat_window">
                                                <div id="chat_Panel2" style="border-width: thin; border-style: solid; height: 300px; width: 100%; overflow-y: scroll;">
                                                </div>
                                                <div id="typing_box" style="background-color: #F1F1F1; height: 100%; width: 100%;">
                                                    <table style="width: 100%; height: 100%;">
                                                        <tr>
                                                            <td class="space" width="10%" height="5%">&nbsp;</td>
                                                            <td height="5%">&nbsp;</td>
                                                            <td class="space" width="10%" height="5%">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="space" width="10%" height="90%">&nbsp;</td>
                                                            <td height="90%">
                                                                <table style="border-style: solid; border-width: thin; background-color: white; width: 100%; height: 80%;">

                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%">
                                                                            <textarea id="message" style="min-height: 50px; height: 100%; width: 100%" cols="40" rows="5" class="form-control" placeholder="コメントを入力する...."></textarea>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <!--typing box end-->

                                                                <table style="border-style: solid; border-width: thin; background-color: white; width: 100%; height: 20%;">
                                                                    <tr>
                                                                        <td width="30%">

                                                                            <%--<input type="file" multiple="multiple" name="fuDocument" id="File1" onchange="UploadFile(this);">--%>

                                                                            <!-- <input type="file" multiple="multiple" name="fuDocument" id="fuDocument" onchange="UploadFile(this);" value=" ファイルを添付 " style="border-style: none;background-color: white; color: #B7B7B7;"> -->


                                                                        </td>
                                                                        <!--end attack file-->

                                                                        <td width="30%">
                                                                            <%--<input type="button" id="send" value="ビデオチャックをする" style="border-style: none;background-color: white; color:#B7B7B7;">--%>

                                                                        </td>
                                                                        <!--end video-->


                                                                        <td align="right">

                                                                            <input type="button" id="Button4" value=" 送信する " onclick="chat_insert()" style="border-style: none; background-color: #FF6666; color: #FFFFFF;">
                                                                        </td>
                                                                        <td class="space" width="5%">&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                                <!--end footer of typing box-->


                                                            </td>
                                                            <td class="space" width="10%" height="90%">&nbsp;</td>
                                                        </tr>


                                                        <tr>
                                                            <td class="space" width="10%" height="80%">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <!--div_right_bottom end-->
                            </div>
                        </div>
                        <!--div_right end-->
                    </div>
                </div>
                <!--main content end-->

            </asp:Panel>
            <asp:Panel ID="date_manger_Panel2" runat="server">
                <table>
                    <tr>
                        <td width="10%" height="50px"></td>
                        <td align="center" bgcolor="transparent" height="50px" class="style1">
                            <asp:HiddenField ID="select_day_HiddenField" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="space"></td>
                        <td>
                            <asp:Calendar ID="Calendar1" runat="server" BackColor="White"
                                BorderColor="Silver" Font-Names="Verdana"
                                Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth"
                                OnDayRender="Calendar1_DayRender" ShowGridLines="True" Width="100%" BorderWidth="1px">
                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" CssClass="text-center" />
                                <DayStyle CssClass="calendar_size1" Height="100%" />
                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333"
                                    VerticalAlign="Bottom" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                <TitleStyle BackColor="White" Font-Bold="True"
                                    Font-Size="16pt" ForeColor="#333399" BorderColor="Black"
                                    BorderWidth="4px" />
                                <TodayDayStyle BackColor="#CCCCCC" />
                            </asp:Calendar>

                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT * FROM [appointment]"></asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="date_manger_Panel3" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td class="space">&nbsp;</td>
                        <td width="50%" align="center">&nbsp;</td>
                        <td class="space">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="space">&nbsp;</td>
                        <td width="50%">
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF5050"
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
                                        <asp:Label ID="Label76" runat="server" Text="歳"></asp:Label>
                                        <asp:DropDownList ID="age_range_start_month_DropDownList" runat="server" CssClass="textbox" Height="20px">
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
                                        <asp:Label ID="Label77" runat="server" Text="ヶ月  ~"></asp:Label>
                                        <asp:DropDownList ID="age_range_end_year_DropDownList" runat="server" CssClass="textbox" Height="20px">
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
                                        <asp:Label ID="Label78" runat="server" Text="歳"></asp:Label>
                                        <asp:DropDownList ID="age_range_end_month_DropDownList" runat="server" CssClass="textbox" Height="20px">
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
                                        <asp:Label ID="Label79" runat="server" Text="ヶ月"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                            Text="ご経験と資格をお選びください"></asp:Label>
                                        &nbsp;<asp:Label ID="Label4" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
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
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                            Text="その他の特徴をお選びください"></asp:Label>
                                        &nbsp;<asp:Label ID="Label6" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
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
                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="#FF5050"
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
                            <br />
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="#FF5050"
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
                                                    <asp:Label ID="Label80" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList0" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label81" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label82" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList0" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label83" runat="server" Text="分"></asp:Label>
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
                                                    <asp:Label ID="Label84" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList1" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label85" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label86" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList1" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label87" runat="server" Text="分"></asp:Label>
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
                                                    <asp:Label ID="Label88" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList2" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label89" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label90" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList2" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label91" runat="server" Text="分"></asp:Label>
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
                                                    <asp:Label ID="Label92" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList3" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label93" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label94" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList3" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label95" runat="server" Text="分"></asp:Label>
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
                                                    <asp:Label ID="Label96" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList4" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label97" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label98" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList4" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label99" runat="server" Text="分"></asp:Label>
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
                                                    <asp:Label ID="Label100" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="start_minute_DropDownList5" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label101" runat="server" Text="分  ~"></asp:Label>
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
                                                    <asp:Label ID="Label102" runat="server" Text="時"></asp:Label>
                                                    <asp:DropDownList ID="end_minute_DropDownList5" runat="server" CssClass="textbox"
                                                        Height="20px">
                                                        <asp:ListItem>00</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>45</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label103" runat="server" Text="分"></asp:Label>
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
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="3" align="center">
                                        <br />
                                        <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="#FF5050"
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
                                        <asp:Label ID="Label22" runat="server" Text="円 / 時間"></asp:Label>
                                        <br />
                                    </td>

                                </tr>
                                <tr>
                                    <td width="20%">&nbsp;</td>
                                    <td colspan="2">
                                        <br />
                                        <asp:Label ID="money_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

                                        <br />
                                        <asp:Label ID="Label23" runat="server" Text="相場は1時間800円~1200円となっております" ForeColor="#999999"></asp:Label>
                                        <br />
                                </tr>
                            </table>
                            <br />
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Label ID="Label31" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                            Text="預かりの様子がわかる写真のアップ口ード"></asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="border-style: solid; border-width: thin">
                                        <label class="file-upload">
                                            <span><strong>画像を登録</strong></span>
                                            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);" AllowMultiple="True" />
                                        </label>
                                        <br />
                                        <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                                        <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto">
                                        </asp:Panel>

                                        <br />
                                        <asp:Label ID="Label32" runat="server" Font-Size="XX-Small"
                                            Text="もしくは 画像 を ドロップ"></asp:Label>

                                        <br />

                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Label ID="Label33" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                            Text="自己紹介文の作成"></asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label34" runat="server" Text="ひと言コメント"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="title_TextBox" runat="server" Width="100%" Wrap="False" placeholder="例)3人の子育て経験!乳幼児も見れます"
                                            CssClass="textbox" Height="20px"></asp:TextBox>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label35" runat="server" Text="自己総会文"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="myself_content_TextBox" runat="server" Width="100%" placeholder="例) はじめまして、この度自分の子どもが大学生になったので、時間 に余裕ができました。子どもが好きなので子どもに関わる仕事をした いと思って今回参加させて頂きあmした。子育て経験豊富で、乳幼児 の預りも可能です。ぜひご連絡ください。"
                                            CssClass="textbox" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 10%;"></td>
                                    <td>&nbsp;</td>
                                    <td style="width: 10%;"></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="center">
                                        <asp:Label ID="Label40" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                            Text="報酬のお支払い先"></asp:Label>
                                        &nbsp;
                                <asp:Label ID="Label41" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
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
                                                                <td style="width: 20%" valign="top">
                                                                    <asp:Label ID="Label104" runat="server" Text="種類"></asp:Label>
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
                                                                    <table style="width: 100%;">
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
                                                                    <asp:Label ID="Label105" runat="server" Text="金融機関名"></asp:Label>
                                                                    <br />
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="bank_name_TextBox" runat="server" Width="90%" Wrap="False" placeholder="例) はてな銀行"
                                                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                                                    <br />
                                                                    <asp:Label ID="bank_name_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td style="width: 35%">
                                                                                <asp:Label ID="Label106" runat="server" Text="支店名"></asp:Label>
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
                                                                    <asp:Label ID="Label107" runat="server" Text="口座番号"></asp:Label>
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
                                                                    <asp:Label ID="Label108" runat="server" Text="名義人"></asp:Label>
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
                                        <table width="100%">
                                            <tr>
                                                <td width="50%" colspan="2">
                                                    <br />

                                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                    <br />

                                                    <asp:Button ID="date_manger_information_b2" runat="server" CssClass="file-upload" OnClick="date_manger_information_b2_Click" OnClientClick="ShowProgressBar();" Text="更新" Width="95%" />

                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%">
                                                    <asp:Button ID="date_manger_information_b1" runat="server" CssClass="file-upload" OnClientClick="ShowProgressBar();" Text="プレビュー" Width="95%" OnClick="date_manger_information_b1_Click" Visible="False" />
                                                </td>
                                                <td align="right" width="50%">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>&nbsp;</td>
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
            </asp:Panel>
            <asp:Panel ID="date_manger_Panel4" runat="server">
                <table style="width: 100%; height: 100%;">
                    <tr>
                        <td class="space2" height="100px">&nbsp;</td>
                        <td width="70%" height="100px">&nbsp;</td>
                        <td class="space2" height="100px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="space2">&nbsp;</td>
                        <td width="70%">
                            <table style="width: 100%; height: 100%;">
                                <tr>
                                    <td width="40%">
                                        <asp:Panel ID="date_man_total_money" runat="server"></asp:Panel>
                                    </td>
                                    <td class="space1"></td>
                                    <td width="40%" height="100%">
                                        <asp:Panel ID="date_man_total_money1" runat="server" Height="100%" Width="100%"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="space2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="space2">&nbsp;</td>
                        <td width="70%">&nbsp;</td>
                        <td class="space2">&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="date_manger_Panel5" runat="server" Style="position: relative; display: block">
                <div style="position: absolute; background-color: #000; z-index: 999997; opacity: 0.8; width: 100%; height: 100%; text-align: center;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <span style="color: white;">クローズドβ 版。</span>
                </div>
                <table style="width: 100%; height: 100%;">
                    <tr>
                        <td width="10%" height="100px">&nbsp;</td>
                        <td width="80%" height="100px" align="center" valign="middle">
                            <asp:Label ID="Label75" runat="server" Text="講習動画一覧" Font-Bold="True" ForeColor="#FF5050"></asp:Label>
                        </td>
                        <td width="10%" height="100px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="10%">&nbsp;</td>
                        <td width="80%">
                            <table style="width: 100%; height: 100%;">
                                <tr>
                                    <td width="20%" valign="top">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <hr />
                                                    <asp:Button ID="data_video_Button" runat="server" Text="全部 >" BackColor="White" BorderStyle="None" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="data_video_Button_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <hr />
                                                    <asp:Button ID="data_video_Button1" runat="server" Text="預かり前におすすめ >" BackColor="White" BorderStyle="None" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="data_video_Button1_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <hr />
                                                    <asp:Button ID="data_video_Button2" runat="server" Text="預かり中におすすめ >" BackColor="White" BorderStyle="None" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="data_video_Button2_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <hr />
                                                    <asp:Button ID="data_video_Button3" runat="server" Text="預かり後におすすめ >" BackColor="White" BorderStyle="None" Height="100%" Width="100%" OnClientClick="ShowProgressBar();" OnClick="data_video_Button3_Click" />
                                                    <hr />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="10%"></td>
                                    <td width="70%" height="100%">
                                        <asp:Panel ID="video_panel" runat="server" Height="100%" Width="100%"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="10%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="10%">&nbsp;</td>
                        <td width="80%">&nbsp;</td>
                        <td width="10%">&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        </div>

        <div id="dialog" title=" ">
            <table style="width: 100%; font-size: small;" align="center">
                <tr>
                    <td>
                        <div class="radio-toolbar">
                            <table width="100%">
                                <tr>
                                    <td width="5%"></td>
                                    <td width="44%">
                                        <input type="radio" id="radio1" name="radios" value="yes" checked>
                                        <label for="radio1">利用可能</label>
                                    </td>
                                    <td width="2%"></td>
                                    <td width="44%">
                                        <input type="radio" id="radio2" name="radios" value="no">
                                        <label for="radio2">利用不可</label>
                                    </td>
                                    <td width="5%"></td>

                                </tr>
                            </table>
                        </div>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="5%"></td>
                                <td width="44%">
                                    <asp:Label ID="Label14" runat="server" Text="開始日" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td width="44%">
                                    <asp:Label ID="Label36" runat="server" Text="終了日" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="5%"></td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="5%"></td>
                                <td width="44%">
                                    <p>
                                        <input type="text" id="datepicker" placeholder="2016/09/01" style="font-size: small;" readonly>
                                    </p>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="Label37" runat="server" Text="~" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="44%">
                                    <p>
                                        <input type="text" id="datepicker0" placeholder="2016/09/01" style="font-size: small;" readonly>
                                    </p>
                                </td>
                                <td width="5%"></td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="5%"></td>
                                <td width="44%">
                                    <asp:Label ID="Label39" runat="server" Text="時間" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td width="44%"></td>
                                <td width="5%"></td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="5%"></td>
                                <td width="44%">
                                    <asp:DropDownList ID="app_start_hour_DropDownList" runat="server" CssClass="textbox" Style="font-size: small;">
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
                                    <asp:Label ID="Label71" runat="server" Text="時" ForeColor="Silver"></asp:Label>
                                    <asp:DropDownList ID="app_start_minute_DropDownList" runat="server" CssClass="textbox" Style="font-size: small;">
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label72" runat="server" Text="分" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="Label69" runat="server" Text="~" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="44%">
                                    <asp:DropDownList ID="app_end_hour_DropDownList" runat="server" CssClass="textbox" Style="font-size: small;">
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
                                    <asp:Label ID="Label73" runat="server" Text="時" ForeColor="Silver"></asp:Label>
                                    <asp:DropDownList ID="app_end_minute_DropDownList" runat="server" CssClass="textbox" Style="font-size: small;">
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label74" runat="server" Text="分" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="5%"></td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="5%"></td>
                                <td width="44%">
                                    <asp:Label ID="Label70" runat="server" Text="メモを追加" ForeColor="Silver"></asp:Label>
                                </td>
                                <td width="2%"></td>
                                <td width="44%"></td>
                                <td width="5%"></td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="place_select" type="button" value="更新" class="file-upload" style="width: 100%" />
                    </td>
                </tr>
            </table>
        </div>


        <div>
            <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
                <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
                <br />
                <font color="#95989A" size="2px">読み込み中</font>
            </div>
            <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
            </div>
            <asp:Panel ID="javaplace" runat="server">
            </asp:Panel>

        </div>
        <div id="footer" style="z-index: 999998;">
            <ul>
                <li>
                    <div>
                        <asp:Button ID="reservation" runat="server" Text="" class="reservation button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b1_Click" />
                        <%--<button id="reservation" runat="server" class="reservation button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b1_Click"></button>--%>
                    </div>
                </li>
                <li>
                    <div>
                        <asp:Button ID="calender" runat="server" Text="" class="calender button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b2_Click" />
                        <%-- <button id="calender" class="calender button_bottom" onclick="ShowProgressBar(this.id)"></button>--%>
                    </div>
                </li>
                <li>
                    <div>
                        <asp:Button ID="user" runat="server" Text="" class="user button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b3_Click" />
                        <%--<button id="user" class="user button_bottom" onclick="ShowProgressBar(this.id)"></button>--%>
                    </div>
                </li>
                <li>
                    <div>
                        <asp:Button ID="graph" runat="server" Text="" class="graph button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b4_Click" />
                        <%--<button id="graph" class="graph button_bottom" onclick="ShowProgressBar(this.id)"></button>--%>
                    </div>
                </li>
                <li>
                    <div>
                        <asp:Button ID="movie" runat="server" Text="" class="movie button_bottom" OnClientClick="ShowProgressBar(this.id);" OnClick="date_manger_b5_Click" />
                        <%--<button id="movie" class="movie button_bottom" onclick="ShowProgressBar(this.id)"></button>--%>
                    </div>
                </li>
            </ul>
        </div>
        <!--end footer-->
    </form>



    <script>
        function ShowSidebarRight() {
            var div = document.getElementById('div_sidebar_right');
            if (div.style.display == 'block') {
                div.setAttribute("style", "display:none");

            }
            else {
                div.setAttribute("style", "display:block; z-index:999999;");
            }
        }


        $(window).load(function () {
            var param1 = "<%= Session["iconid"]%>".toString();
            if (param1 != null) {
                console.log(param1);
                ShowProgressBar(param1);
                HideProgressBar();
            }
        });

            function ShowProgressBar(id) {
                displayProgress();
                displayMaskFrame();
                $.ajax({
                    type: "POST",
                    url: "user_date_manger.aspx/changeicon",
                    data: "{param1 :'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (result) {
                        console.log(result.d);
                    },
                    error: function (result) {
                        //console.log(result.Message);
                        //alert(result.d);
                    }

                });
                // TurnOff(id);
                // TurnOn(id);
                ChangeIcon(id);
            }



            function ChangeIcon(id) {
                var id_arr = new Array("reservation", "calender", "user", "graph", "movie");
                for (i = 0; i < id_arr.length; i++) {
                    if (id_arr[i].localeCompare(id) == 0) {
                        TurnOn(id_arr[i]);
                    }
                    else {
                        TurnOff(id_arr[i]);
                    }
                }
            }


            function TurnOn(id) {
                $("#" + id).addClass(id + "_w").removeClass(id);
            }

            function TurnOff(id) {
                $("#" + id).addClass(id).removeClass(id + "_w");
            }
            //$(window).load(function () {
            //    if ($(window).width() > 990) {
            //        $('#div_right2').css('margin-top', -($('#div_left').height() - $('#div_right1').height() + 2));
            //        $('#div_right3').css('margin-top', -($('#div_left').height() - $('#div_right1').height() - $('#div_right2').height()) + 20);
            //        console.log($('#div_left').height());
            //        console.log($('#div_right1').height());
            //        console.log($('#div_right2').height());
            //    }
            //});


            $(window).resize(function () {
                var w = $('small_calendar').width();
                if (w > 221) {
                    $('.calendar_size').css({
                        "width": w / 7.4,
                        "height": w / 7.4
                    });
                }
                $('.button').css({
                    padding: 'w/80 w/80',

                });
            });
            $(window).resize(function () {
                var w1 = $('Calendar1').width();
                if (w1 > 221) {
                    $('.calendar_size1').css({
                        "width": w1 / 7.4,
                        "height": w1 / 7.4
                    });
                }

                $('.button').css({
                    padding: 'w1/80 w1/80',

                });

            });

            $('.calendar_size').css({
                "width": $('small_calendar').width() / 7.4,
                "height": $('small_calendar').width() / 7.4
            });

            $('.calendar_size1').css({
                "width": $('Calendar1').width() / 7.4,
                "height": $('Calendar1').width() / 7.4
            });

            $('.calendar_size_pc').css({
                "width": $(window).width() / 25,
                "height": $(window).width() / 25
            });

            //if ($(window).width() < 680) {
            //    $(window).bind('mousewheel', function (event) {
            //        if (event.originalEvent.wheelDelta >= 0) {
            //            console.log('Scroll up');
            //            $('#header').css('display', 'block');
            //        }
            //        else {
            //            console.log('Scroll down');
            //            $('#header').css('display', 'none');
            //        }
            //    });
        //}

            $('#content').click(function () {
                var div = document.getElementById('sidebar_left');
                var menu = document.getElementById('menu_right');
                if (div.style.display == 'block' || menu.style.display == 'block') {
                    div.setAttribute("style", "display:none");
                    menu.setAttribute("style", "display:none");
                }
            });
    </script>
    <!--js function end-->


    <!-- Bootstrap Core JavaScript -->
    <script src="js/bootstrap.js"></script>
</body>
</html>
