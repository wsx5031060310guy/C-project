<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main_guest_light.aspx.cs" Inherits="main_guest_light" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
     <link rel="stylesheet" href="css/jquery-ui.css">

    <link rel="stylesheet" href="css/tipped.css">
    <link rel="stylesheet" href="css/file-upload_fb.css" type="text/css" />
      <!-- semantic core CSS file -->
    <link rel="stylesheet" type="text/css" href="css/semantic.css">
    <!-- Magnific Popup core CSS file -->
    <link rel="stylesheet" href="css/magnific-popup.css">
     <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="stylesheet" type="text/css" href="css/main_guest.css">


    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?key=key&libraries=drawing&language=ja"></script>
    <script src="Scripts/jquery-1.12.4.js"></script>
        <script src="js/semantic.js"></script>
    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery.fileupload.css">
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
    <!-- Magnific Popup core JS file -->
    <script src="js/jquery.magnific-popup.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/tipped.js"></script>
    <script src="Scripts/freewall.js"></script>
     <script src="js/jquery.nicescroll.js"></script>


    <%--<link href="https://fonts.googleapis.com/earlyaccess/hannari.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/earlyaccess/notosansjapanese.css" rel="stylesheet" />--%>

    <script>
        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });
            $("body").niceScroll();
        });

        //change to supporter page
        function gotodate(click_id) {
            var sup_str = "";
            var cut = click_id.indexOf('_');
            sup_str = click_id.substr(cut + 1, click_id.length - cut - 1);
            $.ajax({
                type: "POST",
                url: "main_guest_light.aspx/changetodate",
                data: "{ param2 :'" + sup_str + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (result) {
                    //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                    //console.log(result.d);
                    window.location.href = "Date_Calendar_guest.aspx";
                    //window.location.href = "Date_Calendar_success.aspx";
                },
                error: function (result) {
                    //console.log(result.Message);
                    //alert(result.d);
                }
            });
        }


        $(function () {

            var body = document.body, html = document.documentElement;

            var height = Math.max(body.scrollHeight, body.offsetHeight,
                                   html.clientHeight, html.scrollHeight, html.offsetHeight);

            //$("#sidebar_left").height(height);

            $("#salutation").selectmenu({
                width: 150
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

        $(document).ready(function () {
            initMap();

        });

        function initMap() {
            // 地圖初始設定
            var mapOptions = {
                center: new google.maps.LatLng(35.447824, 139.6416613),
                zoom: 100,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var mapElement = document.getElementById("div_showMap");
            // Google 地圖初始化
            map = new google.maps.Map(mapElement, mapOptions);
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
                        zoom: 100,
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
                        $.ajax({
                            type: "POST",
                            url: "main_guest_light.aspx/search_new_post",
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
                                if (result.d == "0") {
                                } else {
                                    $('#javaplace_formap').empty();
                                    $('#Panel2').empty();
                                    $('#Panel2').append(result.d);
                                }
                                window.scrollTo(0, 0);
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
    </script>
     <style type="text/css">
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
<body onload="initMap()">
    <script src="js/jquery.lazy.js"></script>
    <form id="form1" runat="server">
        <div id="white-background">
        </div>
        <div id="sitebody">
            <div id="header">
                <table style="width: 100%; height: 100%">
                    <tr>
                        <td class="header-left">

                            <table style="width: 100%;">
                                <tr>
                                    <td width="5%">&nbsp;</td>
                                    <td class="icon_left"><span onclick="ShowSidebarLeft()"><i class="fa fa-bars" style="font-size: 3.5em; color: white; cursor: pointer;"></i></span></td>

                                    <td class="rin">
                                        <asp:Image ID="Label_logo" Style="height: auto; cursor: pointer;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                    <!--hehe-->

                                </tr>
                            </table>
                        </td>
                        <td class="header-midle">&nbsp;</td>
                        <td class="header-right">
                            <table style="width: 100%;">
                                <tr>
                                    <ul id="menu_list">
                                        <li class="topnav_right"></li>
                                        <li class="topnav_right"></li>
                                        <li class="topnav_right"></li>
                                        <li class="topnav_right"></li>
                                        <li class="topnav_right"></li>
                                        <td class="icon_map"><span onclick="ShowMap()"><i class="fa fa-location-arrow fa-undo " style="font-size: 30px; color: white; cursor: pointer;"></i></span></td>
                                        <td class="icon_right"></td>
                                    </ul>
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

                                <br />
                                <br />
                                <span>
                                    <img src="images/new.png" style="height: 25px; width: 25px;"></span><asp:Button ID="social_Button" runat="server" Text="二ュースフィード" BorderStyle="None" BackColor="transparent" OnClick="social_Button_Click" OnClientClick="ShowProgressBar();" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>

                            <td class="hidden">
                                <asp:Button ID="Button_for_kid" runat="server" Text="子育てサポート" BorderStyle="None" BackColor="transparent" Enabled="False" OnClientClick="ShowProgressBar();" OnClick="Button_for_kid_Click" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>

                            <td class="hidden">
                                <table style="width: 100%;">
                                    <tr>

                                        <td>
                                            <asp:Button ID="supporter_list" runat="server" Text="サポーター" BorderStyle="None" BackColor="transparent" OnClick="supporter_list_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Button ID="video_list_Button" runat="server" Text="講習動画" BorderStyle="None" BackColor="transparent" OnClick="video_list_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Button ID="supporter_manger" runat="server" Text="サポート管理" BorderStyle="None" BackColor="transparent" OnClick="supporter_manger_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>

                            <td>&nbsp;&nbsp;<asp:Button ID="Button_for_social" Style="font-size: 20px; font-weight: bold" runat="server" Text="工リア情報" BorderStyle="None" BackColor="transparent" Enabled="False" />

                            </td>
                        </tr>


                        <tr>
                            <td>
                                <table style="width: 100%; margin-top: 20px">
                                    <tr>

                                        <td><span>
                                            <img src="images/food.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type0_Button" runat="server" Text="お食事" BorderStyle="None" BackColor="transparent" OnClick="message_type0_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span>
                                            <img src="images/supporter.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type1_Button" runat="server" Text="人気スポット" BorderStyle="None" BackColor="transparent" OnClick="message_type1_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span>
                                            <img src="images/event.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type2_Button" runat="server" Text="イベント" BorderStyle="None" BackColor="transparent" OnClick="message_type2_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span>
                                            <img src="images/hospital.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type3_Button" runat="server" Text="病院" BorderStyle="None" BackColor="transparent" OnClick="message_type3_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span>
                                            <img src="images/park.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type4_Button" runat="server" Text="公園／レジャー" BorderStyle="None" BackColor="transparent" OnClick="message_type4_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td><span>
                                            <img src="images/milk_room.png" style="height: 25px; width: 25px;"></span><asp:Button ID="message_type5_Button" runat="server" Text="" BorderStyle="None" BackColor="transparent" OnClick="message_type5_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br />
                                            <br />
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

                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d208050.53811007156!2d139.60380047594595!3d35.43524026117918!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x60185becbbb66509%3A0x69683f660285400!2z5pel5pys56We5aWI5bed57ij5qmr5r-x5biC!5e0!3m2!1szh-TW!2stw!4v1473325484439" width="100%" height="100%" frameborder="0" style="border: 0" allowfullscreen></iframe>

                    </div>

                    <div id="menu_right" width="100%" height="100%">
                        <table style="width: 100%; margin-top: 6em;">
                            <tr>
                                <td>
                                    <button type="button" onclick="gotomy()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">マイページ</button>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <button type="button" onclick="friend_notice()" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">友達</button>
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
                                    <button type="button" style="border-style: none; background-color: #E7E7E7; margin-top: 15px; cursor: pointer;">ヘルプ</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--End menu_right-->

                </div>
            </div>

            <div id="content">
                <table style="width: 100%; height: 100%; margin-top: -20px">
                    <tr>
                        <td height="20%" valign="top">
                            <table class="top_notify">
                                <tr>
                                    <td align="center" valign="middle">
                                        <br />
                                        <span style="font-size: large;">コメントの投稿や、全ての情報を見るには</span><br />
                                        <br />
                                        <span style="font-size: large;"><a href="registered0.aspx">新規登録</a>か<a href="main.aspx">ログイン</a>が必要です。</span>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="post_message_panel" runat="server" Visible="False">


                                <table style="border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px; width: 100%; height: 100%;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; height: 100%;">
                                                <tr>
                                                    <td class="auto-style1">&nbsp;</td>
                                                    <td align="right" width="5%" valign="bottom">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/photo.png" Height="20px" Width="20px" />
                                                        <br />
                                                    </td>
                                                    <td align="left" valign="bottom">
                                                        <label class="file-upload" style="font-family: 游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif;">
                                                            <span>写真</span>
                                                            <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);" AllowMultiple="True" />
                                                        </label>
                                                        <br />
                                                        <%-- <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />--%>
                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>

                                                    </td>
                                                    <td valign="bottom" width="10%">
                                                        <br />

                                                    </td>
                                                </tr>
                                            </table>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%; white-space: nowrap;">
                                                <tr>
                                                    <td class="auto-style1">&nbsp;</td>
                                                    <td align="left" height="60px" width="8%">
                                                        <asp:Panel ID="Panel3" runat="server"></asp:Panel>
                                                        <div id="userphoto_div" class="zoom-gallery">
                                                        </div>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:TextBox ID="post_message_TextBox" runat="server" Width="100%" placeholder="地域の情報を書き込もう" Height="100%" TextMode="MultiLine" BorderStyle="None"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style1"></td>
                                                    <td colspan="2">
                                                        <asp:Panel ID="Panel1" runat="server">
                                                            <asp:HiddenField ID="image_HiddenField" runat="server" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="auto-style1">&nbsp;</td>
                                                    <td align="left" width="30%">

                                                        <img alt="" src="images/tag.png" height="20px" width="20px" />
                                                        <select name="post_type" id="select">
                                                            <option value="">カテゴリー</option>
                                                            <option value="0">ランド</option>
                                                            <option value="2">イペント</option>
                                                            <option value="3">病院</option>
                                                            <option value="4">公園</option>
                                                            <option value="5"></option>
                                                        </select>


                                                    </td>
                                                    <td width="30%">
                                                        <div id="addmsg"></div>
                                                        <asp:HiddenField ID="place_va" runat="server" />
                                                        <asp:HiddenField ID="postcode_HiddenField" runat="server" />
                                                        <img alt="" src="images/pin.png" height="20px" width="20px" />
                                                        <input id="opener" type="button" value="位置情報" style="border-style: none; background-color: #FFFFFF; color: #CCCCCC;" />

                                                    </td>
                                                    <td width="25%">
                                                        <span>
                                                            <div class="ui inline dropdown">
                                                                <div class="text">
                                                                    <img class="ui avatar image" src="images/icon/public.png">
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
                                                        <%-- <asp:Button ID="post_message_Button" runat="server" BackColor="#000099" BorderStyle="None" ForeColor="White" Text="投稿" Width="95%" OnClick="post_message_Button_Click" Height="20px" />--%>
                                                    </td>
                                                    <caption>
                                                        <br />
                                                        <br />
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
    </form>
    <div id="dialog" title="場所">
        <table style="width: 100%;" align="center">
            <tr>
                <td>
                    <input id="txt2" type="text" onkeydown="Javascript: if (event.which == 13 || event.keyCode == 13) runScript();" placeholder="住所を入力" style="width: 100%"><br />
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
    <script type="text/javascript">
        /*$('#sidebar_left').attr('style', 'display:none');
        $('#div_sidebar_right').attr('style', 'display:none');
        $('#sidebar_left').attr('style', 'display:none');*/
        function Hide_PostProgressBar() {
            var progress = $('#post_progress');
            progress.hide();
        }
        // 顯示讀取畫面
        function display_PostProgress() {
            var progress = $('#post_progress');
            progress.show();
        }
        $(document).ready(function () {

            google.maps.event.addListener(map, "idle", function () {
                google.maps.event.trigger(map, 'resize');
            });
        });
        $('#content').click(function () {
            var div = document.getElementById('sidebar_left');
            var menu = document.getElementById('menu_right');
            if (div.style.display == 'block' || menu.style.display == 'block') {
                div.setAttribute("style", "display:none");
                menu.setAttribute("style", "display:none");
            }
        });

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





    </script>
</body>
</html>
