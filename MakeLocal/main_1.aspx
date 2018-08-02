<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main_1.aspx.cs" Inherits="main" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>MakeLocal-地域で子育てSNS</title>


    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBAgFM6PSlUcmIZR6a9AfuAgBQcC1hAVdQ&libraries=drawing&language=ja"></script>

         <script src="Scripts/jquery-1.12.4.js"></script>
    
    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fileupload.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/jquery.fileupload.css">
    <link rel="stylesheet" href="css/trim/style.css">

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
<style type="text/css">

    body {
        background-color: #E9EBEE;
        font-family:游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', sans-serif !important;
    }
fieldset {
      border: 0;
    }
    label {
      display: block;
      margin: 30px 0 0 0;
    }
    .overflow {
      height: 200px;
    }
    .textbox { 
-webkit-border-radius: 5px;
-moz-border-radius: 5px;
border-radius: 5px;
  } 
          input:focus 
  {
      border: 2px solid #AA88FF;
    background-color: #ff7575;
    }   
    .auto-style1
    {
        width: 5%;
    }


    h2 {
      margin: 30px 0 0 0;
    }
 
    #dialog{
                /*initially dialog box is hidden*/
                display: none;
            }


    .mfp-with-zoom .mfp-container,
.mfp-with-zoom.mfp-bg {
  opacity: 0;
  -webkit-backface-visibility: hidden;
  /* ideally, transition speed should match zoom duration */
  -webkit-transition: all 0.3s ease-out;
  -moz-transition: all 0.3s ease-out;
  -o-transition: all 0.3s ease-out;
  transition: all 0.3s ease-out;
}

.mfp-with-zoom.mfp-ready .mfp-container {
    opacity: 1;
}
.mfp-with-zoom.mfp-ready.mfp-bg {
    opacity: 0.8;
}

.mfp-with-zoom.mfp-removing .mfp-container,
.mfp-with-zoom.mfp-removing.mfp-bg {
  opacity: 0;
}

    </style>
    <script>

        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });
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
            dlg.style.top = winHeight/10 + "px";
        }
        function showDialog_m() {
            var whitebg = document.getElementById("white-background");
            var dlg = document.getElementById("dlgbox1");
            whitebg.style.display = "block";
            dlg.style.display = "block";

            var winWidth = window.innerWidth;
            var winHeight = window.innerHeight;

            dlg.style.left = (winWidth / 2) - 480 / 2 + "px";
            dlg.style.top = winHeight/10 + "px";
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
        $(function () {

            var body = document.body, html = document.documentElement;

            var height = Math.max(body.scrollHeight, body.offsetHeight,
                                   html.clientHeight, html.scrollHeight, html.offsetHeight);

            $("#
                
                
                
                ").height(height);

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
                }
            });

            $("#opener").on("click", function () {
                $("#dialog").dialog("open");
            });


            $("#place_select").on("click", function () {
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
                                var str = "<%= Session["id"]%>".toString();

                            if (str != "" || str != null) {
                                check_photo(result.d);
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
                            $('#userphoto_div').append('<a href="' + str + '" data-source="' + str + '" title="" style="width:100px;height:100px;"><img src="' + str + '" width="100" height="100" /></a>');
                        }
                    },
                    error: function (result) {
                        //console.log(result.Message);
                    }
                });
            }
        }

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
  </script>
</head>
<body onload="initMap()">
            <form id="form1" runat="server">
<div id="white-background">
        </div>
        <div id="dlgbox">
            <div id="dlg-header">メッセ一ジ</div>
            <div id="dlg-body" style="height: 400px; overflow: auto">
                <table style="border-style: solid; border-width: thin; width: 100%;">
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td width="20%">
                                        <img alt="" src="images/user/1.png" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:void(0);" Target="_blank" Text="たんリママさん" Font-Underline="False">たんリママさん</asp:HyperLink>
                                        <br/>
                                        <asp:Label ID="Label2" runat="server" Text="9月2日13:00から面接可能です!ど..."></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label3" runat="server" Text="3分前" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/2.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:void(0);" Target="_blank" Text="みゃままさん" Font-Underline="False">みゃままさん</asp:HyperLink>
                                        <br/>
                                        <asp:Label ID="Label4" runat="server" Text="預かり完了の報告を受領しました。..."></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label5" runat="server" Text="11時間前" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/3.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="javascript:void(0);" Target="_blank" Text="こっこちゃんさん" Font-Underline="False">こっこちゃんさん</asp:HyperLink>
                                        <br/>
                                        <asp:Label ID="Label6" runat="server" Text="こちらこそよろしくお願いします。..."></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label7" runat="server" Text="8月11日" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/4.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="javascript:void(0);" Target="_blank" Text="こっこちゃんさん" Font-Underline="False">こっこちゃんさん</asp:HyperLink>
                                        <br/>
                                        <asp:Label ID="Label8" runat="server" Text="こちらこそよろしくお願いします。..."></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label9" runat="server" Text="8月10日" ForeColor="#CCCCCC"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dlg-footer">
                <input id="Button3" type="button" value="すべて見る" onclick="dlgLogin()" class="file-upload1"/>
            </div>
        </div>


                 <div id="dlgbox1">
            <div id="dlg-header1">お知らせ</div>
            <div id="dlg-body1" style="height: 400px; overflow: auto">
                <table style="border-style: solid; border-width: thin; width: 100%;">
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td width="20%">
                                        <img alt="" src="images/user/5.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label10" runat="server" Text="このみさん他4人があなたの投稿に「いい ね」と言っています。"></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label11" runat="server" Text="3分前" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/6.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label12" runat="server" Text="みなとこきんがあなたのコメントに「いいね」と言っています。"></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label13" runat="server" Text="8月20日" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/7.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label14" runat="server" Text="港北区さんがあなたのコメントに返信をしました。"></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label15" runat="server" Text="8月15日" ForeColor="#CCCCCC"></asp:Label>
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
                                    <td width="20%">
                                         <img alt="" src="images/user/8.jpg" width="100px" height="100px" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label16" runat="server" Text="よっしー さんがあなたのコメントに返信をしました"></asp:Label>
                                        <br/>
                                        <asp:Label ID="Label17" runat="server" Text="8月10日" ForeColor="#CCCCCC"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dlg-footer1">
                <input id="Button13" type="button" value="すべて見る" onclick="dlgLogin()" class="file-upload1"/>
            </div>
        </div>


                <div id="dlgbox_login">
            <div id="dlg-header_login"></div>
            <div id="dlg-body_login" style="height: 400px; overflow: auto">
                <div id="first_login_d" style="width: 100%;height:100%">
                    <table style="width: 100%;height:100%" align="center">
                        <tr><td width="10%" height="20%"></td><td width="80%" height="20%"></td><td width="10%" height="20%"></td></tr>
                        <tr><td width="10%"></td><td width="80%">
                            <table style="width: 100%;height:100%;">
                    <tr>
                        <td style="width: 100%;height:40%;">
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
                            <a href="javascript:void(0);" target="_blank" style="text-decoration:none;">パスワードを忘れた方はこちら</a>
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
                            <td align="left" width="15%">
                                
                                <table style="width:100%;">
                                    <tr>
                                        <td width="5%">&nbsp;</td>
                                   <td class="rin"><asp:Image id="Label_logo" style="width:90%;height:auto;" runat="server" ImageUrl="images/makelocallogo.png"></asp:Image></td>                                       
<td class="rin2">  <img id="Img1" onclick="javascript:self.location='home.aspx';" src="images/logo1.png" style="width:50px;height:auto;cursor:pointer;"></td>
                                    
                                    </tr>
                                </table>
                            </td>
                            <td width="55%">
                                <asp:TextBox ID="TextBox1" runat="server" Width="50%" placeholder="友達を検索"></asp:TextBox>
                            </td>
                            <td width="30%">
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            <button type="button" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">マイページ</button>
                                        </td>
                                        <td>

                                            <button type="button" onclick="showDialog()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">メッセ一ジ</button>

                                        </td>
                                        <td>

                                            <button type="button" onclick="showDialog_m()" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">お知らせ</button>

                                        </td>
                                        <td>

                                   <button type="button" onclick="window.location.href='Help.html';" style="border-style: none; background-color: #ea9494; color: #FFFFFF; cursor: pointer;">ヘルプ</button>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div>
　<div id="sidebar_left">
      <table style="width: 100%;">
                        <tr>
                            <td style="width: 5%;">
                            </td>
                            <td>
                                <br /><br />
                                &nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="二ュースフィード" BorderStyle="None" BackColor="#e7e7e7" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button4" runat="server" Text="子育てサポート" BorderStyle="None" BackColor="#e7e7e7" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                                               <table style="width:100%;">
                                    <tr>
                                        <td width="10%">&nbsp;</td>
                                        <td><asp:Button ID="Button14" runat="server" Text="サポーター" BorderStyle="None" BackColor="#e7e7e7" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="video_list_Button" runat="server" Text="講習動画" BorderStyle="None" BackColor="#e7e7e7" OnClick="video_list_Button_Click" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="Button16" runat="server" Text="サポート管理" BorderStyle="None" BackColor="#e7e7e7" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                </table>
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="Button5" runat="server" Text="工リア情報" BorderStyle="None" BackColor="#e7e7e7" />
                                <br /><br />
                            </td>
                            </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                               <table style="width:100%;">
                                    <tr>
                                        <td width="10%">&nbsp;</td>
                                        <td><asp:Button ID="message_type0_Button" runat="server" Text="お食事" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type0_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="message_type1_Button" runat="server" Text="人気スポット" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type1_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="message_type2_Button" runat="server" Text="イベント" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type2_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="message_type3_Button" runat="server" Text="病院" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type3_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="message_type4_Button" runat="server" Text="公園／レジャー" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type4_Button_Click" OnClientClick="ShowProgressBar();" />
                                            <br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:Button ID="message_type5_Button" runat="server" Text="授乳室" BorderStyle="None" BackColor="#e7e7e7" OnClick="message_type5_Button_Click" OnClientClick="ShowProgressBar();" />
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
　<div id="sidebar_right">
      <div align="center">
<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d208050.53811007156!2d139.60380047594595!3d35.43524026117918!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x60185becbbb66509%3A0x69683f660285400!2z5pel5pys56We5aWI5bed57ij5qmr5r-x5biC!5e0!3m2!1szh-TW!2stw!4v1473325484439" width="100%" height="500px" frameborder="0" style="border:0" allowfullscreen></iframe>
                </div>
　</div>
　<div id="content">
     <table style="width: 100%;height: 100%;">
         <tr>
             <td height="20%" valign="top">
                 <table style="border: thick solid #C0C0C0; width: 100%; height: 100%;">
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
            <span><strong>写真</strong></span>
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
                                         <asp:Panel ID="Panel3" runat="server"></asp:Panel>
                                         <div id="userphoto_div" class="zoom-gallery">
                                             
                                             </div>
                                     </td>
                                     <td valign="middle">
                                         <asp:TextBox ID="post_message_TextBox" runat="server" Width="100%" placeholder="地域の情報を書き込もう" Height="100%" TextMode="MultiLine" BorderStyle="None"></asp:TextBox>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                     </td>
                                     <td colspan="2">
                                         <asp:Panel ID="Panel1" runat="server">
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
                             <table style="width: 100%;">
                                 <tr>
                                     <td class="auto-style1">&nbsp;</td>
                                     <td align="left" width="30%">

                                         <img alt="" src="images/tag.png" height="20px" width="20px" />
 <select name="post_type" class="ui dropdown" id="select">
     <option value="">カテゴリー</option>
      <option value="0">お食事</option>
      <option value="1">人気スポット</option>
      <option value="2">イベント</option>
      <option value="3">病院</option>
      <option value="4">公園／レジャー</option>
       <option value="5">授乳室</option>
</select>


                                     </td>
                                     <td width="30%">
                                         <div id="addmsg"></div>
                                         <asp:HiddenField ID="place_va" runat="server" />
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
                             <asp:Button ID="post_message_Button" runat="server" BackColor="#000099" BorderStyle="None" ForeColor="White" Text="投稿" Width="95%" OnClick="post_message_Button_Click" />
                                     </td>
                                 </tr>
                             </table>
                         </td>
                     </tr>
                 </table>



                 <br />
                 <br />
             </td>
         </tr>
         <tr>
             <td>
                 <br />
                
                 
                
        <asp:Panel ID="Panel2" runat="server" Width="100%">
        </asp:Panel>
                
                 
                
                 <br />
                 <br />
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
                <asp:Panel ID="javaplace" runat="server">
                </asp:Panel>
    </form>
                    <div id="dialog" title="場所">
                    <table style="width: 100%;" align="center">
                        <tr>
                            <td>
                                <INPUT id="txt2" TYPE="text" onKeydown="Javascript: if (event.which == 13 || event.keyCode == 13) runScript();" placeholder="住所を入力" style="width: 100%"><br />
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
    </body>
</html>
