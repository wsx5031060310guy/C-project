<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chatroom.aspx.cs" Inherits="chatroom" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <meta name="viewport" content="width=device-width, initial-scale=1"><meta name="description"><meta name="author">

        <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/bootstrap-theme.css" rel="stylesheet" />
    <!-- Magnific Popup core CSS file -->
<link rel="stylesheet" href="css/magnific-popup.css">
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css"/>
    <link rel="stylesheet" href="css/jquery-ui.css">
     <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/chatroom.css">

        <script src="Scripts/jquery-1.12.4.js"></script>
<%--    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>--%>

<!-- Magnific Popup core JS file -->
<script src="js/jquery.magnific-popup.js"></script>
    <!--Reference the SignalR library. -->
    <script src="Scripts/jquery.signalR-2.2.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <!--這邊滿重要的，這個參考是動態產生的，當我們 build 之後才會動態建立這個資料夾，且需引用在 jQuery 和 signalR 之後-->
    <script src="/signalr/hubs"></script>




    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-88025000-1', 'auto');
        ga('send', 'pageview');

</script>


        <script>
            $(function () {
                $('#message').keypress(function (e) {
                    var key = e.which;
                    if (key == 13)  // the enter key code
                    {
                        $('#send').click();
                        return false;
                    }
                });
            });


            var userID = '<%= Session["loginname"].ToString() %>';
            if (userID == "") {
                window.location.href = "main.aspx";
            }
            //var userID = "";
            $(window).resize(function () {
                var w = $(window).width();
                var h = $(window).height();
                var maskFrame = $("#chatbody");
                maskFrame.css({ "width": w, "height": h });
            });



            $(function () {



                var w = $(document).width();
                var h = $(window).height();
                var progress = $('#chatbody');
                progress.css({ "height": h, "width": w });



                $(".small_talk_div_leave").on("click", function () {

                    $(this).toggleClass("small_talk_div");
                    $("#title_talk_name").html($(this).find('span').text());

                    $("#title_talk_id").html($(this).find('div').text());

                    $("#chat_chat_" + $(this).find('div').text()).show();

                    $(".small_talk_div_leave").not(this).removeClass("small_talk_div");

                    $(".small_talk_div_leave").not(this).find('div').each(function () {
                        $("#chat_chat_" + $(this).text()).hide();
                    });
                    $('#chat_Panel2').scrollTop(999999999);

                });


                //$("#userName").append(userID).show();

                //建立與Server端的Hub的物件，注意Hub的開頭字母一定要為小寫
                var chat = $.connection.codingChatRoomHub;
                //取得所有上線清單
                chat.client.getList = function (userList) {
                    $.each(userList, function (i, data) {
                        var li = "<img src='images/online.png' width='10' height='10' id='" + data.id + "'>";
                        $("#online_" + data.name).append(li);
                    })
                    //registerListClick();
                }
                //新增聊天內容
                chat.client.addtalkList = function (id, name, photo, message, year, month, day, hour, minute, second) {

                    var li = "<table width='100%'><tr><td width='10%' rowspan='2' valign='top'>";
                    li += "<div class='zoom-gallery'>";
                    li += "<a href='" + photo + "' data-source='" + photo + "' title='" + name + "' style='width:100px;height:100px;'>";
                    li += "<img src='" + photo + "' width='100' height='100' />";
                    li += "</a>";
                    li += "</div>";
                    li += "</td><td width='60%'>";
                    li += "<span>" + name + "</span>";
                    li += "</td><td width='20%'>";
                    li +=  year+"年"+month+"月"+day+"日"+hour+":"+minute+":"+second ;
                    li += "</td></tr><tr><td colspan='2' style='word-break:break-all;'>";
                    li += "" + message + "";
                    li += "</td></tr></table>";


                    $("#chat_chat_" + id).append(li);

                    $("#chat_chat_" + id).hide();
                    var check_session = '<%=Session["tmp_chat_id"] != null%>';
                    if (check_session == 'True') {

                        var force_id = '<%= Session["tmp_chat_id"] %>';

                        if (force_id != '') {
                            if (id == force_id) {
                                $("#chat_chat_" + id).show();
                                $('#chat_Panel2').scrollTop(999999999);
                            }
                        }
                    }
                }

                //新增一筆上線人員
                chat.client.addList = function (id, uid) {
                    var li = "<img src='images/online.png' width='10' height='10' id='"+id+"'>";
                    $("#online_"+uid).append(li);
                    //registerListClick();
                }

                var check_session = '<%=Session["tmp_chat_id"] != null%>';
                if (check_session == 'True') {

                    var force_id = '<%= Session["tmp_chat_id"] %>';

                    if (force_id != '') {
                        var cusid_ele = document.getElementsByClassName('small_talk_div_leave');
                        var strr = "";
                        for (var i = 0; i < cusid_ele.length; ++i) {
                            var item = cusid_ele[i];
                            if ($(item).find('div').text() == force_id) {

                                $(item).toggleClass("small_talk_div");
                                $("#title_talk_name").html($(item).find('span').text());

                                $("#title_talk_id").html($(item).find('div').text());

                                $("#chat_chat_" + $(item).find('div').text()).show();
                                document.getElementById(item.id).scrollIntoView();

                                $(".small_talk_div_leave").not(item).removeClass("small_talk_div");

                                $(".small_talk_div_leave").not(item).find('div').each(function () {
                                    $("#chat_chat_" + $(item).text()).hide();
                                });
                                '<%= Session["tmp_chat_id"]=null %>';
                            }
                        }

                    }
                }

                //移除一筆上線人員
                chat.client.removeList = function (id) {
                    $("#" + id).remove();
                }

                //全體聊天
                chat.client.sendAllMessage = function (message) {
                    $("#messageList").append("<li>" + message + "</li>");
                }

                //密語聊天
                chat.client.sendMessage = function (id, name, photo, message, year, month, day, hour, minute, second,shortmesg) {
                    var li = "<table width='100%'><tr><td width='10%' rowspan='2' valign='top'>";
                    li += "<div class='zoom-gallery'>";
                    li += "<a href='" + photo + "' data-source='" + photo + "' title='" + name + "' style='width:100px;height:100px;'>";
                    li += "<img src='" + photo + "' width='100' height='100' />";
                    li += "</a>";
                    li += "</div>";
                    li += "</td><td width='60%'>";
                    li += "<span>" + name + "</span>";
                    li += "</td><td width='20%'>";
                    li += year + "年" + month + "月" + day + "日" + hour + ":" + minute + ":" + second;
                    li += "</td></tr><tr><td colspan='2' style='word-break:break-all;'>";
                    li += "" + message + "";
                    li += "</td></tr></table>";


                    $("#chat_chat_" + id).append(li);

                    //$("#chat_chat_" + id).find('strong').html("");
                    $("#chat_chat_last" + id).text(shortmesg);

                    //$("#messageList").append("<li>" + message + "</li>");

                }

                chat.client.loading = function () {
                    ShowProgressBar();
                }

                chat.client.finishload = function () {
                    HideProgressBar();
                }


                chat.client.scrollPanel= function () {
                    var panel = document.getElementById("<%=chat_Panel2.ClientID%>");
                    panel.scrollTop = panel.scrollHeight;
                }



                chat.client.hello1 = function (message) {
                    alert(message);
                }

                chat.client.hello = function (message) {
                    $("#messageList").append("<li>" + message + "</li>");
                }

                //將連線打開
                $.connection.hub.start().done(function () {
                    //當連線完成後，呼叫Server端的userConnected方法，並傳送使用者姓名給Server
                    chat.server.userConnected(userID);
                });;

                $("#send").click(function () {

                    var to = $("#title_talk_id").text();
                    var me = $("#title_pan_myid").text();

                    //當to為all代表全體聊天，否則為私密聊天
                    if (to == "") {
                        //chat.server.sendAllMessage($("#message").val());
                    }
                    else {
                        chat.server.sendMessageto(me,to, $("#message").val());
                        //$("#messageList").append("<li>" + $("#message").val() + "</li>");
                    }


                    $("#message").val("");
                });

                $.connection.hub.error(function (error) {
                    console.log('SignalR error: ' + error)
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


            function registerListClick() {
                $("#list li").unbind("click");
                $("#list li").on("click", function () {
                    var $this = $(this);
                    var id = $this.attr("id");
                    var text = $this.text();

                    $("#title_talk_name").html("");
                    $("#title_talk_name").html(text);
                    $("#title_talk_id").html("");
                    $("#title_talk_id").html(id);
                    $("#title_talk_email").html("");
                    //$("#title_talk_email").html();


                    //防止重複加入密語清單
                    if ($("#box").has("." + id).length > 0) {
                        $("#box").find("[class=" + id + "]").attr({ "selected": "selected" });
                    }
                    else {
                        var option = "<option></option>"
                        $("#box").append(option).find("option:last").val(id).text(text).attr({ "selected": "selected" }).addClass(id);
                    }

                });
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
            //new state message notice
            //new state message notice
            $(window).load(function () {
                var param1 = "<%= Session["id"]%>".toString();
            if (param1 != "") {
                $.ajax({
                    type: "POST",
                    url: "chatroom.aspx/count_list",
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
                url: "chatroom.aspx/new_state_list",
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
                        url: "chatroom.aspx/new_state_notice_list_scroll",
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
                 url: "chatroom.aspx/chat_notice_list",
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
                url: "chatroom.aspx/friend_notice_list",
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
                url: "top_WebService.asmx/search_friend_notice_list",
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
                url: "chatroom.aspx/friend_notice_check",
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
                url: "chatroom.aspx/friend_notice_check_del",
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
                url: "chatroom.aspx/friend_notice_donotfind",
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
                url: "chatroom.aspx/friend_notice_addfind",
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
                url: "chatroom.aspx/toget_friend_list",
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
                        url: "chatroom.aspx/search_friend_notice_list_scroll",
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



    <form id="form1" runat="server">
    <div id="chatbody">
        <table style="width: 100%; height: 100%;">
            <tr>
                <td class="space" width="10%" height="10%">&nbsp;</td>
                <td height="70px">&nbsp;</td>
                <td class="space" width="10%" height="10%">&nbsp;</td>
            </tr>
            <tr>
                <td class="space" width="10%" height="80%">&nbsp;</td>
                <td>
                    <table style="width: 100%; height: 100%;padding-bottom:44px;">
                        <tr class="main_header" style="height: 10px">
                            <td id="list" class="list" align="center" style="border-style: solid; border-width: thin">メッセージ一覧</td>
                            <td id="friend_name" class="friend_name" align="center" style="border-style: solid; border-width: thin">
                                <asp:Panel ID="title_myid" runat="server">
                                    </asp:Panel>
                                <div id="title_talk_name">

                                </div>
                                <div style="visibility:hidden" id="title_talk_id"></div>
                                <div style="visibility:hidden" id="title_talk_email"></div>
                            </td>
                        </tr>
                        <tr>
                            <td id="contact_list" class="contact_list" valign="top">
                                <asp:Panel ID="chat_Panel1" runat="server" style="border-width:1px;border-style:Solid;height:100%;width:100%;overflow-y:scroll;" valign="top">
                                    <div id="chatList" class="label">

                                    <p> Who is online </p>

                                    <ul id="list"></ul>
                                    </div>
                                </asp:Panel>
                             </td>
                            <td id="chat_window" class="chat_window" >
                                <asp:Panel ID="chat_Panel2" style="border-width:thin;border-style:solid;height:53vh;width:100%;overflow-y:scroll;" runat="server">
                                    <div id="chat_chat_"></div>
                                    <ul id="messageList"></ul>
                                </asp:Panel>
                                <div id="typing_box" style="background-color: #DDDDDD;width: 100%;height:30%;">
                                    <table style="width: 100%; height: 100%;">
                                        <tr>
                                            <td class="space" width="10%" height="5%">&nbsp;</td>
                                             <td height="5%">&nbsp;</td>
                                              <td class="space" width="10%" height="5%">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="space" width="10%" height="90%">&nbsp;</td>
                                            <td height="90%">
                                                <table style="border-style: solid; border-width: thin; width: 100%; height: 80%;">
                                                    <tr>
                                                        <td style="width: 100%; height: 100%">
                                                            <textarea id="message" height="100%" cols="40" rows="5" class="form-control" placeholder="コメントを入力する...."></textarea>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="border-style: solid; border-width: thin; width: 100%; height: 20%;">
                                                    <tr>
                                                        <td width="5%">&nbsp;</td>
                                                        <td width="25%">
                                                        </td>
                                                        <td width="30%">&nbsp;</td>
                                                        <td align="right">
                                                            <input type="button" id="send" value=" 送信する " style="border-style: none; background-color: #FF6666; color: #FFFFFF;" />
                                                        </td>
                                                        <td class="space" width="5%">&nbsp;</td>
                                                    </tr>
                                                </table>

                                            </td>
                                            <td class="space" width="10%" height="90%">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="space" width="10%" height="5%">&nbsp;</td>
                                <td height="5%">&nbsp;</td>
                                <td class="space" width="10%" height="5%">&nbsp;</td>
                                        </tr>
                                    </table>

            </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="space" width="10%" height="80%">&nbsp;</td>
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
            var h = $(window).height() - h1 -h2-h3-22;
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
