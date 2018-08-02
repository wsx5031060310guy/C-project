<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Date_Calendar.aspx.cs" Inherits="Date_Calendar" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false">
<head runat="server">
    <title></title>

    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
     <link rel="stylesheet" href="css/flexslider.css" type="text/css">
    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css">
     <!-- Magnific Popup core CSS file -->
    <link rel="stylesheet" href="css/magnific-popup.css">
      <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/date_calendar.css">
    <meta name="description" content="">
    <meta name="author" content="">

    <script src="Scripts/jquery-1.12.4.js"></script>

    <script src="Scripts/jquery.flexslider.js"></script>
    <!-- Syntax Highlighter -->
    <script type="text/javascript" src="Scripts/shCore.js"></script>
    <script type="text/javascript" src="Scripts/shBrushXml.js"></script>
    <script type="text/javascript" src="Scripts/shBrushJScript.js"></script>

    <script type="text/javascript" src="Scripts/modernizr.js"></script>

    <script src="Scripts/jquery-ui.js"></script>

    <script src="Scripts/datepicker-ja.js"></script>


    <!-- Magnific Popup core JS file -->
    <script src="js/jquery.magnific-popup.js"></script>

    <%--    <!-- Theme CSS -->
    <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
    <link href="css/agency.css" rel="stylesheet">
    <!-- Theme JavaScript -->
    <!-- Bootstrap Core JavaScript -->
    <script src="vendor/bootstrap/js/bootstrap.js"></script>--%>


    <script type="text/javascript">
        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });
        });
        function dlgLogin() {
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox");
            whitebg.style.display = "none";
            dlg.style.display = "none";
        }

        function showDialog() {
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox");
            whitebg.style.display = "block";
            dlg.style.display = "block";

            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg.style.left = 0 + "px";
            dlg.style.top = 60 + "px";
            $('#' + '<%=left_view.ClientID%>').hide();
        $('#' + '<%=left_view1.ClientID%>').hide();
        $('#mobile_calendar').hide();
        $('#' + '<%=right_view.ClientID%>').show();
        $('#middle_view').hide();
    }
    $(document).ready(init);
    function init() {
        $('#' + '<%=right_view.ClientID%>').hide();
    }
    function selectdate(click_id) {
        //alert(click_id);
        select_date_HiddenField.value = click_id;
        var date_str = "";
        var cut = click_id.indexOf('.');
        date_str += click_id.substr(0, cut) + "年";
        var year = click_id.substr(0, cut);

        var sub = click_id.substr(cut + 1, click_id.length - cut - 1);
        cut = sub.indexOf('.');
        date_str += sub.substr(0, cut) + "月" + sub.substr(cut + 1, sub.length - cut - 1) + "日";
        var month = sub.substr(0, cut);
        var day = sub.substr(cut + 1, sub.length - cut - 1);
        var param4 = "<%= Session["sup_id"]%>".toString();
        $.ajax({
            type: "POST",
            url: "Date_Calendar.aspx/search_time",
            data: "{param1: '" + year + "' , param2 :'" + month + "',param3 :'" + day + "',param4 :'" + param4 + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (result) {
                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                //console.log(result.d);
                var cut = result.d.indexOf('.');
                start_HiddenField.value = result.d.substr(0, cut);
                end_HiddenField.value = result.d.substr(cut + 1, result.d.length - cut - 1);

                var ddlstart = document.getElementById("select_start_time");
                var ddlend = document.getElementById("select_end_time");

                ddlstart.options.length = 0;
                ddlend.options.length = 0;
                var cou = 0;
                for (var i = result.d.substr(0, cut) ; i <= result.d.substr(cut + 1, result.d.length - cut - 1) ; i++) {

                    var ci = i;
                    if (i < 10) {
                        ci = "0" + i;
                        if (i == result.d.substr(0, cut)) {
                            ci = i;
                        }
                    }

                    AddOption(ddlstart, ci, ci);
                    AddOption(ddlend, ci, ci);
                    cou += 1;
                }


            },
            error: function (result) {
                //console.log(result.Message);
            }
        });

        $("#total_hour").text("0時間");
        var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
        $("#simple_money").text(mon + "円");
        $("#total_tax").text("0円");
        $("#total_money").text("0円");

        $("#total_hour1").text("0時間");
        $("#simple_money1").text(mon + "円");
        $("#total_tax1").text("0円");
        $("#total_money1").text("0円");


        $('#select_date_text').text(date_str);
        $('#' + '<%=left_view.ClientID%>').hide();
        $('#' + '<%=left_view1.ClientID%>').hide();
        $('#mobile_calendar').hide();
        $('#' + '<%=right_view.ClientID%>').show();
        $('#middle_view').hide();
        dlgLogin();
    }
    function noselectdate() {
        $('#' + '<%=left_view.ClientID%>').hide();
        $('#' + '<%=left_view1.ClientID%>').hide();
        $('#mobile_calendar').hide();
        $('#' + '<%=right_view.ClientID%>').show();
        $('#middle_view').hide();
    }



    $(function () {
        SyntaxHighlighter.all();
        choice_HiddenField.value = 0;
        '<%Session["req"] = ""; %>'
        '<%Session["key"] = ""; %>'
        '<%Session["check_case"] = ""; %>'
        $('#pan_selct_div').hide();

        $('#datepicker_start').on("change", function () {
            //alert($(this).val());
            var check_day = true;
            //get today's date in string
            var todayDate = new Date();
            //need to add one to get current month as it is start with 0
            var todayMonth = todayDate.getMonth() + 1;
            var todayDay = todayDate.getDate();
            var todayYear = todayDate.getFullYear();
            var todayDateText = todayYear + "/" + todayMonth + "/" + todayDay;
            //

            //get date input from SharePoint DateTime Control
            var inputDateText = $(this).val();
            //

            //Convert both input to date type
            var inputToDate = Date.parse(inputDateText);
            var todayToDate = Date.parse(todayDateText);
            //

            var warn = "";
            //compare dates
            if (inputToDate < todayToDate) {
                check_day = false;
                warn = "the input is earlier than today";
            }
            else if (inputToDate == todayToDate) {
                check_day = false;
                warn = "the input is same as today";
            }
            inputDateText = $("#datepicker_end").val();
            if (inputDateText != "") {
                inputToDate = Date.parse(inputDateText);
                if (inputToDate < todayToDate) {
                    check_day = false;
                    warn = "the input is earlier than today";
                }
                else if (inputToDate == todayToDate) {
                    check_day = false;
                    warn = "the input is same as today";
                }

                if (check_day) {
                    var start = $(this).val();
                    var end = $("#datepicker_end").val();
                    var date1 = new Date(start);
                    var date2 = new Date(end);
                    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                    var startdate;
                    if (date1 > date2) {
                        startdate = date2;
                    } else if (date1 < date2) {
                        startdate = date1;
                    } else {
                        startdate = date1;
                    }

                    var weekday = new Array(7);


                    weekday[0] = 0;
                    weekday[1] = 0;
                    weekday[2] = 0;
                    weekday[3] = 0;
                    weekday[4] = 0;
                    weekday[5] = 0;
                    weekday[6] = 0;
                    var star = startdate.getDay();
                    for (i = 0; i <= diffDays; i++) {
                        weekday[star] += parseInt(1);
                        if (star == 6) {
                            star = 0;
                        } else {
                            star += 1;
                        }
                    }
                    var howmany_h = 0;
                    var howmany_t = 0;
                    var howmany_to = 0;
                    for (i = 0; i < 7; i++) {
                        if (i == 6) {
                            if (document.getElementById("select_weekday_6").checked) {
                                var starth = $("#select_weekday_drop_start_hour6").val();
                                var endh = $("#select_weekday_drop_end_hour6").val();
                                var hour = Math.abs(endh - starth);
                                howmany_h += parseInt(hour * weekday[0]);
                                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                         $("#simple_money").text(mon + "円");

                         howmany_t += parseInt(((mon * hour) * 0.1) * weekday[0]);
                         howmany_to += parseInt(((mon * hour) + ((mon * hour) * 0.1)) * weekday[0]);
                     }

                 } else {
                     if (document.getElementById("select_weekday_" + i).checked) {
                         var starth = $("#select_weekday_drop_start_hour" + i).val();
                         var endh = $("#select_weekday_drop_end_hour" + i).val();
                         var hour = Math.abs(endh - starth);
                         howmany_h += parseInt(hour * weekday[(i + 1)]);
                         var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                         $("#simple_money").text(mon + "円");

                         howmany_t += parseInt(((mon * hour) * 0.1) * weekday[(i + 1)]);
                         howmany_to += parseInt(((mon * hour) + ((mon * hour) * 0.1)) * weekday[(i + 1)]);
                     }
                 }
             }
             $("#total_hour").text(howmany_h + "時間");
             var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
         $("#simple_money").text(mon + "円");
         $("#total_tax").text(howmany_t + "円");
         $("#total_money").text(howmany_to + "円");

         $("#total_hour1").text(howmany_h + "時間");
         $("#simple_money1").text(mon + "円");
         $("#total_tax1").text(howmany_t + "円");
         $("#total_money1").text(howmany_to + "円");




     } else {
         alert(warn);
     }
 }



        });

        $('#datepicker_end').on("change", function () {
            //alert($(this).val());
            //alert($(this).val());
            var check_day = true;
            //get today's date in string
            var todayDate = new Date();
            //need to add one to get current month as it is start with 0
            var todayMonth = todayDate.getMonth() + 1;
            var todayDay = todayDate.getDate();
            var todayYear = todayDate.getFullYear();
            var todayDateText = todayYear + "/" + todayMonth + "/" + todayDay;
            //

            //get date input from SharePoint DateTime Control
            var inputDateText = $(this).val();
            //

            //Convert both input to date type
            var inputToDate = Date.parse(inputDateText);
            var todayToDate = Date.parse(todayDateText);
            //

            var warn = "";
            //compare dates
            if (inputToDate < todayToDate) {
                check_day = false;
                warn = "the input is earlier than today";
            }
            else if (inputToDate == todayToDate) {
                check_day = false;
                warn = "the input is same as today";
            }
            inputDateText = $("#datepicker_start").val();
            if (inputDateText != "") {
                inputToDate = Date.parse(inputDateText);
                if (inputToDate < todayToDate) {
                    check_day = false;
                    warn = "the input is earlier than today";
                }
                else if (inputToDate == todayToDate) {
                    check_day = false;
                    warn = "the input is same as today";
                }

                if (check_day) {
                    var start = $(this).val();
                    var end = $("#datepicker_start").val();
                    var date1 = new Date(start);
                    var date2 = new Date(end);
                    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                    var startdate;
                    if (date1 > date2) {
                        startdate = date2;
                    } else if (date1 < date2) {
                        startdate = date1;
                    } else {
                        startdate = date1;
                    }

                    var weekday = new Array(7);


                    weekday[0] = 0;
                    weekday[1] = 0;
                    weekday[2] = 0;
                    weekday[3] = 0;
                    weekday[4] = 0;
                    weekday[5] = 0;
                    weekday[6] = 0;
                    var star = startdate.getDay();
                    for (i = 0; i <= diffDays; i++) {
                        weekday[star] += parseInt(1);
                        if (star == 6) {
                            star = 0;
                        } else {
                            star += 1;
                        }
                    }
                    var howmany_h = 0;
                    var howmany_t = 0;
                    var howmany_to = 0;
                    for (i = 0; i < 7; i++) {
                        if (i == 6) {
                            if (document.getElementById("select_weekday_6").checked) {
                                var starth = $("#select_weekday_drop_start_hour6").val();
                                var endh = $("#select_weekday_drop_end_hour6").val();
                                var hour = Math.abs(endh - starth);
                                howmany_h += parseInt(hour * weekday[0]);
                                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                                $("#simple_money").text(mon + "円");

                                howmany_t += parseInt(((mon * hour) * 0.1) * weekday[0]);
                                howmany_to += parseInt(((mon * hour) + ((mon * hour) * 0.1)) * weekday[0]);
                            }

                        } else {
                            if (document.getElementById("select_weekday_" + i).checked) {
                                var starth = $("#select_weekday_drop_start_hour" + i).val();
                                var endh = $("#select_weekday_drop_end_hour" + i).val();
                                var hour = Math.abs(endh - starth);
                                howmany_h += parseInt(hour * weekday[(i + 1)]);
                                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                         $("#simple_money").text(mon + "円");

                         howmany_t += parseInt(((mon * hour) * 0.1) * weekday[(i + 1)]);
                         howmany_to += parseInt(((mon * hour) + ((mon * hour) * 0.1)) * weekday[(i + 1)]);
                     }
                 }
             }
             $("#total_hour").text(howmany_h + "時間");
             var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
             $("#simple_money").text(mon + "円");
             $("#total_tax").text(howmany_t + "円");
             $("#total_money").text(howmany_to + "円");

             $("#total_hour1").text(howmany_h + "時間");
             $("#simple_money1").text(mon + "円");
             $("#total_tax1").text(howmany_t + "円");
             $("#total_money1").text(howmany_to + "円");




         } else {
             alert(warn);
         }
     }

        });


        var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
        $("#simple_money").text(mon + "円");

        $("#select_start_time").change(function () {
            var start = this.value;
            var end = $("#select_end_time").val();
            var hour = Math.abs(end - start);
            $("#total_hour").text(hour + "時間");
            var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
            $("#simple_money").text(mon + "円");
            $("#total_tax").text(((mon * hour) * 0.1) + "円");
            $("#total_money").text(((mon * hour) + ((mon * hour) * 0.1)) + "円");

            $("#total_hour1").text(hour + "時間");
            $("#simple_money1").text(mon + "円");
            $("#total_tax1").text(((mon * hour) * 0.1) + "円");
            $("#total_money1").text(((mon * hour) + ((mon * hour) * 0.1)) + "円");

        });

        $("#select_end_time").change(function () {
            var start = this.value;
            var end = $("#select_start_time").val();
            var hour = Math.abs(end - start);
            $("#total_hour").text(hour + "時間");
            var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
            $("#simple_money").text(mon + "円");
            $("#total_tax").text(((mon * hour) * 0.1) + "円");
            $("#total_money").text(((mon * hour) + ((mon * hour) * 0.1)) + "円");

            $("#total_hour1").text(hour + "時間");
            $("#simple_money1").text(mon + "円");
            $("#total_tax1").text(((mon * hour) * 0.1) + "円");
            $("#total_money1").text(((mon * hour) + ((mon * hour) * 0.1)) + "円");

        });





        $("#datepicker_start").datepicker({
            format: 'yyyy/mm/dd',
            language: 'ja',
            autoclose: true, // これ
            clearBtn: true,
            clear: '閉じる'
        });
        $("#datepicker_end").datepicker({
            format: 'yyyy/mm/dd',
            language: 'ja',
            autoclose: true, // これ
            clearBtn: true,
            clear: '閉じる'
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

        $('input[type=radio][name=radios]').change(function () {
            if (this.value == 'one') {
                $('#pan_selct').show();
                $('#pan_selct_div').hide();
                choice_HiddenField.value = 0;

                $("#total_hour").text("0時間");
                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                $("#simple_money").text(mon + "円");
                $("#total_tax").text("0円");
                $("#total_money").text("0円");

                $("#total_hour1").text("0時間");
                $("#simple_money1").text(mon + "円");
                $("#total_tax1").text("0円");
                $("#total_money1").text("0円");

                var ddlstart = document.getElementById("select_start_time");
                var ddlend = document.getElementById("select_end_time");
                ddlstart.selectedIndex = 0;
                ddlend.selectedIndex = 0;
            }
            else if (this.value == 'more') {
                $('#pan_selct').hide();
                $('#pan_selct_div').show();
                choice_HiddenField.value = 1;

                $("#total_hour").text("0時間");
                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                $("#simple_money").text(mon + "円");
                $("#total_tax").text("0円");
                $("#total_money").text("0円");


                $("#total_hour1").text("0時間");
                $("#simple_money1").text(mon + "円");
                $("#total_tax1").text("0円");
                $("#total_money1").text("0円");
            }
        });


        $("#but_one").click(function () {
            ShowProgressBar();
            var checkfin = true;
            var param1 = "<%= Session["id"]%>".toString();
            var param2 = "<%= Session["sup_id"]%>".toString();

            var notice = $("#notice_textbox").val().replace("'", "").replace('"', "").replace("`", "").trim();

            var kid = "";
            for (i = 0; i < document.getElementById('<%= howmany_kid_HiddenField.ClientID%>').value; i++) {
                if (document.getElementById('kid_' + i).checked) {
                    kid += document.getElementById('kid_' + i).value + ",";
                }
            }
            var howtoget = "";
            var che = false;
            for (i = 1; i < 5; i++) {
                if (document.getElementById('choice1_' + i).checked) {
                    che = true;
                    howtoget += document.getElementById('choice1_' + i).value + "、";
                }
            }
            if (che) {
                howtoget = howtoget.substr(0, howtoget.length - 1);
            }


            if (choice_HiddenField.value == 0) {

                var start = $("#select_start_time").val();
                var end = $("#select_end_time").val();
                var hour = Math.abs(end - start);
                var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                var tax = ((mon * hour) * 0.1);
                var total = (mon * hour);


                if (select_date_HiddenField.value != "" && kid != "") {

                    $.ajax({
                        type: "POST",
                        url: "Date_Calendar.aspx/Save",
                        data: "{param1: '" + param1 + "' , param2 :'" + param2 + "',param3 :'" + select_date_HiddenField.value + "',param4 :'" + $("#select_start_time").val() + "',param5 :'" + $("#select_start_time_minute").val() + "',param6 :'" + $("#select_end_time").val() + "',param7 :'" + $("#select_end_time_minute").val() + "',param8 :'" + notice + "',param9 :'" + hour + "',param10 :'" + mon + "',param11 :'" + tax + "',param12 :'" + total + "',param13 :'" + kid + "',param14 :'" + howtoget + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                            //console.log(result.d);
                            window.location.href = "Date_Calendar_success.aspx";
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });
                }



            } else if (choice_HiddenField.value == 1) {

                var cou = 0;
                for (i = 0; i < 7; i++) {
                    if (document.getElementById('select_weekday_' + i).checked) {
                        cou += 1;
                    }
                }
                cou -= 1;

                var cou1 = 0;
                for (i = 0; i < 7; i++) {
                    if (document.getElementById('select_weekday_' + i).checked) {

                        var start = $("#select_weekday_drop_start_hour" + i).val();
                        var end = $("#select_weekday_drop_end_hour" + i).val();
                        var hour = Math.abs(end - start);
                        var mon = document.getElementById('<%= money_HiddenField.ClientID%>').value;
                        var tax = ((mon * hour) * 0.1);
                        var total = (mon * hour);


                        if (document.getElementById('datepicker_start').value != "" && document.getElementById('datepicker_end').value != "" && kid != "") {

                            $.ajax({
                                type: "POST",
                                url: "Date_Calendar.aspx/Save_long",
                                data: "{param1: '" + param1 + "' , param2 :'" + param2 + "',param3 :'" + document.getElementById('select_weekday_' + i).value + "',param4 :'" + $("#select_weekday_drop_start_hour" + i).val() + "',param5 :'" + $("#select_weekday_drop_start_minute" + i).val() + "',param6 :'" + $("#select_weekday_drop_end_hour" + i).val() + "',param7 :'" + $("#select_weekday_drop_end_minute" + i).val() + "',param8 :'" + notice + "',param9 :'" + hour + "',param10 :'" + mon + "',param11 :'" + tax + "',param12 :'" + total + "',param13 :'" + kid + "',param14 :'" + howtoget + "',param15 :'" + document.getElementById('datepicker_start').value + "',param16 :'" + document.getElementById('datepicker_end').value + "',param17 :'" + cou1 + "',param18 :'" + cou + "'}",
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
                            cou1 += 1;
                        }


                    }
                }
                window.location.href = "Date_Calendar_success.aspx";
            }

            HideProgressBar();



        });


    });
$(window).load(function () {
    // The slider being synced must be initialized first
    $('#carousel').flexslider({
        animation: "slide",
        controlNav: false,
        animationLoop: false,
        slideshow: false,
        itemWidth: 210,
        itemMargin: 5,
        itemHeight: 200,
        asNavFor: '#slider'
    });

    $('#slider').flexslider({
        animation: "slide",
        controlNav: false,
        animationLoop: false,
        slideshow: false,
        sync: "#carousel"
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


function AddOption(ddl, text, value) {
    var opt = document.createElement("OPTION");
    opt.text = text;
    opt.value = value;
    ddl.options.add(opt);
}

function useful_who_answer(click_id) {
    var param1 = "<%= Session["id"]%>".toString();
        var like_str = "";
        var cut = click_id.indexOf('_');
        like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
        var yn = 0;
        $.ajax({
            type: "POST",
            url: "Date_Calendar.aspx/like_who_ans",
            data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (result) {
                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                //console.log(result.d);

                $('#' + click_id).css("color", "#4183C4");
                $('#' + click_id).attr("onclick", "buseful_who_answer(this.id)");
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

    function buseful_who_answer(click_id) {
        var param1 = "<%= Session["id"]%>".toString();
        var like_str = "";
        var cut = click_id.indexOf('_');
        like_str = click_id.substr(cut + 1, click_id.length - cut - 1);
        var yn = 1;
        $.ajax({
            type: "POST",
            url: "Date_Calendar.aspx/like_who_ans",
            data: "{param1: '" + param1 + "' , param2 :'" + like_str + "',param3 :'" + yn + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (result) {
                //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                //console.log(result.d);

                $('#' + click_id).css("color", "#D84C4B");
                $('#' + click_id).attr("onclick", "useful_who_answer(this.id)");
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
        //new state message notice
        $(window).load(function () {
            var param1 = "<%= Session["id"]%>".toString();
             if (param1 != "") {
                 $.ajax({
                     type: "POST",
                     url: "Date_Calendar.aspx/count_list",
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
            url: "Date_Calendar.aspx/new_state_list",
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
                        url: "Date_Calendar.aspx/new_state_notice_list_scroll",
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
                url: "Date_Calendar.aspx/chat_notice_list",
                data: "{param1: '" + param1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    $('#dlg-body_chat').empty();
                    $('#dlg-body_chat').append(result.d);
                    //console.log(result.d);

                    var whitebg = document.getElementById("white-background");
                    var dlg = document.getElementById("dlgbox_chat");
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
            var dlg = document.getElementById("dlgbox_chat");
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
                url: "Date_Calendar.aspx/friend_notice_list",
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
                url: "Date_Calendar.aspx/search_friend_notice_list",
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
                url: "Date_Calendar.aspx/friend_notice_check",
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
                url: "Date_Calendar.aspx/friend_notice_check_del",
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
                url: "Date_Calendar.aspx/friend_notice_donotfind",
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
                url: "Date_Calendar.aspx/friend_notice_addfind",
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
                url: "Date_Calendar.aspx/toget_friend_list",
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
                         url: "Date_Calendar.aspx/search_friend_notice_list_scroll",
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
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
            <div id="dlg-body_friend" style="height: 400px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_friend" class="dlgf">
                <input id="Button2" type="button" value="閉じる" onclick="dlgLogin_fri_close()" class="file-upload" />
            </div>
        </div>
        <div id="dlgbox_chat" class="dlg">
            <div id="dlg-header_chat" class="dlgh">メッセ一ジ</div>
            <div id="dlg-body_chat" style="height: 400px; overflow: auto" class="dlgb">
            </div>
            <div id="dlg-footer_chat" class="dlgf">
                <input id="Button3" type="button" value="閉じる" onclick="dlgLogin_chat_notice_close()" class="file-upload" />
            </div>
        </div>


        <div id="dlgbox1">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 400px; overflow: auto">
            </div>
            <div id="dlg-footer1">
                <input id="Button13" type="button" value="閉じる" onclick="dlgLogin_new_state_notice_close()" class="file-upload" />
            </div>
        </div>
        <div id="dlgbox">
            <div id="dlg-header" align="right">
                <input id="Button1" type="button" value="X" onclick="dlgLogin()" style="border-style: none; background-color: #ffffff; cursor: pointer; font-size: large;" /></div>
            <div id="dlg-body" style="overflow: auto">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Calendar ID="Calendar2" runat="server" BackColor="White"
                            BorderColor="Silver" Font-Names="Verdana"
                            Font-Size="9pt" ForeColor="Black" Height="100%" NextPrevFormat="FullMonth"
                            OnDayRender="Calendar2_DayRender" ShowGridLines="True" Width="100%" BorderWidth="1px">
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333"
                                VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                            <TitleStyle BackColor="White" Font-Bold="True"
                                Font-Size="16pt" ForeColor="#333399" BorderColor="Black"
                                BorderWidth="4px" />
                            <TodayDayStyle BackColor="#CCCCCC" />
                        </asp:Calendar>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="dlg-footer">
            </div>
        </div>
        <div id="header">
            <table style="width: 100%; height: 100%">
                <tr>
                    <td align="left" width="15%">

                        <table style="width: 100%;">
                            <tr>
                                <td width="5%">&nbsp;</td>
                                <td class="rin">
                                    <asp:Image ID="Label_logo" alt="" src="images/logo1.png" runat="server" Style="height: auto"></asp:Image></td>
                            </tr>
                        </table>
                    </td>
                    <td width="40%">&nbsp;</td>
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
                                <td class="icon_right"><i class="fa fa-bell-o" style="font-size: 30px; color: white; cursor: pointer; margin-right: 10px" onclick="ShowSidebarRight()"></i>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_sidebar_right">
            <div id="sidebar_right">
                <table style="width: 100%; margin-top: 2em;">
                    <tbody>
                        <tr>
                            <td>
                                <button type="button" onclick="gotomy()" style="border-style: none; background-color: #E7E7E7; cursor: pointer;">マイページ</button>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #E7E7E7; cursor: pointer;">友達申請</button>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <button type="button" onclick="chat_notice()" style="border-style: none; background-color: #E7E7E7; cursor: pointer;">メッセ一ジ</button>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <button type="button" onclick="new_state_notice()" style="border-style: none; background-color: #E7E7E7; cursor: pointer;">お知らせ</button>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; background-color: transparent; color: #000000; cursor: pointer;">ヘルプ</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!--End sidebar_right-->
        </div>
        <!--End div_sidebar_right-->


        <div id="calendarDiv" runat="server">

            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:connStr %>"
                SelectCommand="SELECT * FROM [appointment]"></asp:SqlDataSource>

            <asp:HiddenField ID="money_HiddenField" runat="server" />

        </div>
        <table style="width: 100%; position: relative; top: 3em;">
            <tr>
                <td class="body_left" valign="top">
                    <asp:Panel ID="left_view" runat="server" Style="width: 100%;">
                    </asp:Panel>
                    <div id="mobile_calendar" class="calendar_mobile">
                        <table style="border: thin solid #C0C0C0;">
                            <tr>
                                <td class="style1" align="center">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Calendar ID="Calendar3" runat="server" BackColor="White" BorderColor="Silver" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="100%" NextPrevFormat="FullMonth" OnDayRender="Calendar1_DayRender" ShowGridLines="True" Width="100%">
                                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                                <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="16pt" ForeColor="#333399" />
                                                <TodayDayStyle BackColor="#CCCCCC" />
                                            </asp:Calendar>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td height="50px" class="style1">
                                    <img alt="" src="images/date_calendar/color_img.png" width="15px" height="15px" />&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="予約可能" ForeColor="#999999"></asp:Label>
                                    &nbsp;
                    <img alt="" src="images/date_calendar/color_img1.PNG" width="15px" height="15px" />&nbsp;
                    <asp:Label ID="Label2" runat="server" Text="一部の時間帯で可能" ForeColor="#999999"></asp:Label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <button type="button" onclick="noselectdate()" style="width: 100%; top: 0; cursor: pointer; text-align: center;" class="file-upload">いますぐ予約する</button></td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="left_view1" runat="server">
                    </asp:Panel>
                </td>
                <td class="calendar">


                    <div id="middle_view">
                        <table style="border: thin solid #C0C0C0">
                            <tr>
                                <td class="style1">

                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Calendar ID="Calendar1" runat="server" BackColor="White"
                                                BorderColor="Silver" Font-Names="Verdana"
                                                Font-Size="8pt" ForeColor="Black" Height="100%" NextPrevFormat="FullMonth"
                                                OnDayRender="Calendar1_DayRender" ShowGridLines="True" Width="100%" BorderWidth="1px">
                                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333"
                                                    VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                                <TitleStyle BackColor="White" Font-Bold="True"
                                                    Font-Size="16pt" ForeColor="#333399" BorderColor="Black"
                                                    BorderWidth="4px" />
                                                <TodayDayStyle BackColor="#CCCCCC" />
                                            </asp:Calendar>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td height="50px" class="style1">
                                    <img alt="" src="images/date_calendar/color_img.png" width="15px" height="15px" />&nbsp;
                    <asp:Label ID="Label4" runat="server" Text="予約可能" ForeColor="#999999"></asp:Label>
                                    &nbsp;
                    <img alt="" src="images/date_calendar/color_img1.PNG" width="15px" height="15px" />&nbsp;
                    <asp:Label ID="Label5" runat="server" Text="一部の時間帯で可能" ForeColor="#999999"></asp:Label>
                                    &nbsp;
                                </td>
                            </tr>

                        </table>
                        <table width="100%">
                            <tr>
                                <td>

                                    <button type="button" onclick="noselectdate()" style="width: 100%; cursor: pointer; text-align: center;" class="file-upload">いますぐ予約する</button>
                                </td>
                            </tr>
                        </table>
                    </div>

                </td>
            </tr>
            <tr>
                <td valign="top">

                    <br />
                </td>
                <td>
                    <asp:HiddenField ID="howmany_kid_HiddenField" runat="server" />
                    <asp:HiddenField ID="choice_HiddenField" runat="server" />
                    <asp:HiddenField ID="start_HiddenField" runat="server" />
                    <asp:HiddenField ID="end_HiddenField" runat="server" />
                    <asp:HiddenField ID="select_date_HiddenField" runat="server" />
                </td>
            </tr>
        </table>

        <table style="width: 100%;">
            <tr>
                <td width="100%">
                    <asp:Panel ID="right_view" Style="margin-top: 50px;" runat="server"></asp:Panel>

                </td>
            </tr>
        </table>
        <div>
            <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
                <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
                <br />
                <font color="#1B3563" size="2px">読み込み中</font>
            </div>
            <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
            </div>
            <asp:Panel ID="javaplace" runat="server">
            </asp:Panel>

        </div>
    </form>
    <script>
        function ShowSidebarRight() {
            var div = document.getElementById('div_sidebar_right');
            if (div.style.display == 'block') {
                div.setAttribute("style", "display:none");

            }
            else {
                div.setAttribute("style", "display:block; z-index:999999");
            }
        }

        $(window).resize(function () {
            var ww = $(window).width() * 0.65;

            $('.flexslider ul li').each(function () {
                $(this).width(ww);
            });

        });



    </script>

</body>
</html>
