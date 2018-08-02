<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered0.aspx.cs" Inherits="registered0" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link rel="stylesheet" href="css/jquery-ui.css">
    <link rel="stylesheet" href="css/bootstrap.css">
     <link rel="stylesheet" href="css/file-upload.css" type="text/css">
    <link rel="stylesheet" href="css/style.css" />
    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <script src="Scripts/jquery-ui.js"></script>


    <link rel="stylesheet" href="css/registered0.css">
    <script src="https://maps.googleapis.com/maps/api/js?key=&sensor=false&language=ja&region=JP"></script>


    <script type="text/javascript">
        if ($('#Image1').css('display') != 'none') {
            console.log("aaa");
            $('.file-upload').removeAttr('style').css({ "background-color": "##FF9797", "color": "#fff", "border": "2px solid #ff7575" })

            $('.file-upload').css({ "background-color": "#f7f3f3 ", "color": "#e8c8c8", "border": "none" });

            console.log("bbb");
        }

        function initMap() {
            // 地圖初始設定
            //var mapOptions = {
            //    center: new google.maps.LatLng(35.447824, 139.6416613),
            //    zoom: 16,
            //    mapTypeId: google.maps.MapTypeId.ROADMAP
            //};
            //var mapElement = document.getElementById("div_showMap");
            //// Google 地圖初始化
            //map = new google.maps.Map(mapElement, mapOptions);
        }


        var len = 0;
        var chec = true;
        function init() {
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
                    if (r.lat == 0 && r.lng == 0) {
                        //$('#div_showMap').hide();
                    } else {
                        //$('#div_showMap').show();
                        MarkerOne(r);
                    }
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
                                 document.getElementById("txt2").value = address_component.long_name;
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
        $(function () {

            $("#dialog").dialog({
                autoOpen: false,
                height: 500,
                width: 300,
                show: {
                    effect: "blind",
                    duration: 100
                },
                hide: {
                    effect: "explode",
                    duration: 100
                },
                closeText: "",
                open: function () {
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
                                    init();
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

            $("#postalcode").on("click", function () {
                $.ajax({
                    type: "POST",
                    url: "registered0.aspx/Save_first",
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
                $("#dialog").dialog("open");
            });


        });

        function removediv(click_id) {

            var ind = click_id.indexOf('v');
            var cut = click_id.substr(ind + 1, click_id.length - ind - 1);
            $("#TextBoxDiv" + cut).remove();
        }

        $(document).ready(function () {
            initMap();
            var counter = 2;

            $("#place_select").click(function () {

                init();
                if (document.getElementById("txt3").value != "") {
                    var myHidden = document.getElementById('<%= HiddenField_postal_one.ClientID %>');
                    if (myHidden)//checking whether it is found on DOM, but not necessary
                    {
                        myHidden.value = document.getElementById("txt2").value.replace("'", "").replace('"', "").replace("`", "").trim();
                    }
                    $("#place_group").append('<div id="TextBoxDiv' + counter + '"><li id="place_single' + counter + '">' + document.getElementById("txt3").value + '</li><img id="remov' + counter + '" onclick="removediv(this.id)" style="cursor: pointer;" src="images/crash.png"><div id="place_postalcode' + counter + '" style="visibility:hidden;">' + document.getElementById("txt2").value + '</div></div>');
                    counter++;
                }
            });

            $("#place_finish").click(function () {
                $('#postal_DIV').empty();

                //ver 1
                var postal_code = "";
                var myHidden = document.getElementById('<%= HiddenField_postal_one.ClientID %>');
                if (myHidden)//checking whether it is found on DOM, but not necessary
                {
                    myHidden.value = document.getElementById("txt2").value.replace("'", "").replace('"', "").replace("`", "").trim();
                    postal_code = myHidden.value;
                }
                if (document.getElementById("txt3").value != "") {
                    $.ajax({
                        type: "POST",
                        url: "registered0.aspx/Save_place",
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
                //for (i = 1; i < counter; i++) {

                //    if ($('#place_single' + i) != null && $('#place_postalcode' + i) != null) {
                //        if ($('#place_single' + i).text().replace("'", "").replace('"', "").replace("`", "") != "" && $('#place_postalcode' + i).text().replace("'", "").replace('"', "").replace("`", "") != "") {
                //            $.ajax({
                //                type: "POST",
                //                url: "registered0.aspx/Save_place",
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
                //            }

                //    }
                //}


                $("#dialog").dialog("close");
            });


            //for (i = 1; i < counter; i++) {
            //    if ($('#yearsold_textbox' + i) != null) {

            //    }
            //}


        });

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
<body>
    <form id="form1" runat="server">
        <div id="header">
            <table style="width: 100%; height: 100%">
                <tr>
                    <td align="left" width="15%">

                        <table style="width: 100%;">
                            <tr>
                                <td width="5%">&nbsp;</td>

                                <td class="rin">
                                    <asp:Image ID="Label_logo" Style="width: 60px; height: auto; cursor: pointer; margin-left: 3px" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>
                                <!--hehe-->


                            </tr>
                        </table>
                    </td>
                    <td width="55%">&nbsp;</td>
                    <td width="30%">
                        <table style="width: 100%;">
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>

            <table style="width: 100%; margin-top: 20px">
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">&nbsp;</td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="基本情報"></asp:Label>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                    <br />
                                    <asp:HiddenField ID="FB_id_hidd" runat="server" />
                                    <asp:HiddenField ID="FB_cov_hidd" runat="server" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label2" runat="server" Text="二ックネーム"></asp:Label>
                                                <br />
                                                <asp:Label ID="Label3" runat="server" Font-Size="XX-Small" ForeColor="#CCCCCC"
                                                    Text="※公開されます"></asp:Label>
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label7" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="60%">
                                    <asp:TextBox ID="name_TextBox" runat="server" Width="100%" Wrap="False" placeholder="二ックネーム"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="name_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label4" runat="server" Text="メールアドレス"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label6" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="60%">
                                    <asp:TextBox ID="loginname_TextBox" runat="server" Width="100%" Wrap="False" placeholder=" メールアドレスを入力"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="loginname_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label8" runat="server" Text="地域の設定"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label9" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>

                                    <asp:HiddenField ID="HiddenField_postal_one" runat="server" />

                                    <br />
                                    <div id="postalcode" style="border-style: solid; border-width: thin; width: 100%; height: 8%; text-align: center; cursor: pointer;">
                                        <div id="postal_DIV"></div>
                                        <span style="color: blue;">表示エリアの設定．変更</span>
                                    </div>
                                    <br />
                                    <asp:Label ID="c_place_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label10" runat="server" Text="アイコンの写真"></asp:Label>
                                                <br />
                                                <asp:Label ID="Label17" runat="server" Font-Size="XX-Small" ForeColor="#CCCCCC"
                                                    Text="※公開されます"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label12" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center">


                                    <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" OnClick="UploadDocument" Style="display: none;" OnClientClick="ShowProgressBar();" />
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                                    <asp:Image ID="Image1" runat="server" Height="100px" Width="100px" />

                                    <br />
                                    <%-- <asp:Label ID="Label25" runat="server" Font-Size="XX-Small"
                                     Text="もしくは 画像 を ドロップ"></asp:Label>--%>
                                    <label class="file-upload file-upload-custom" style="width: 8em; height: 33px">
                                        <span style="width: 8em"><strong style="font-size: 15px;">画像を登録</strong></span>
                                        <asp:FileUpload ID="fuDocument" runat="server" onchange="UploadFile(this);" />
                                    </label>
                                    <br />
                                    <br />
                                    <asp:Label ID="photo_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>

                        </table>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label37" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ログインパスワードの設定"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label39" runat="server" Text="パスワード"></asp:Label>
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label38" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" width="60%">
                                    <asp:TextBox ID="password_TextBox" runat="server" Width="100%" Wrap="False"
                                        CssClass="textbox" Height="20px" TextMode="Password"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="password_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label11" runat="server" Text="パスワード再確認"></asp:Label>
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label13" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="password_TextBox_check" runat="server" Width="100%" Wrap="False"
                                        CssClass="textbox" Height="20px" TextMode="Password"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="c_password_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="style2" align="center">
                                    <asp:Button ID="Button1" runat="server" CssClass="file-upload" Height="40px"
                                        Text="地域SNSの利用のみではじめる" Width="100%" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                    <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <%--<td  colspan="2" class="style2" align="center"><table style="width:100%">
                                <tbody>
                                    <tr>
                            <td style="width:49%">
                                <asp:Button ID="Button4" runat="server" CssClass="file-upload" Width="100%" Height="40px"
                                    Text="子育てサポーターになる" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                            </td>
                                        <td style="width:2%"></td>
                                <td style="width:49%">
                                <asp:Button ID="Button5" runat="server" CssClass="file-upload" Width="100%" Height="40px"
                                    Text="こどもを預ける" OnClientClick="ShowProgressBar();" OnClick="Button1_Click" />
                                    <br />
                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                            </td>
                        </tr>
                                </tbody>
                                </table></td>--%>
                            </tr>


                            <tr>
                                <td colspan="2" width="100%" align="center">
                                    <asp:Label ID="Label5" runat="server" Text="事前登録受付中　※現在は利用できません。" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
                                    <asp:Button ID="Button2" runat="server" Text="こどもの預け・預かりサービスを利用する" CssClass="file-upload"
                                        Width="100%" Style="background-color: #CCC;" OnClientClick="ShowProgressBar();" OnClick="Button2_Click" Enabled="False" />
                                </td>
                                <br />

                            </tr>
                        </table>
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
                <tr>
                    <td class="space">&nbsp;</td>
                    <td width="50%">
                        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Button" Visible="False" />
                    </td>
                    <td class="space">&nbsp;</td>
                </tr>
            </table>

        </div>
        <div id="dialog" title="">
            <table style="width: 100%;" align="center">

                <tr>
                    <td align="center">
                        <div id="div_showMap" style="width: 270px; height: 210px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="visibility: hidden;">
                            <span>現在選択中のエリア</span>
                            <hr />
                            <div id="place_group" style="width: 100%; height: 1px; overflow: scroll;">
                                <%--<div id="place_group" style="width:100%;height:90px;overflow: scroll;">--%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="txt2" type="text" onkeydown="Javascript: if (event.which == 13 || event.keyCode == 13) init();" placeholder="〒郵便番号追加" style="width: 100%"><br />
                        <input id="txt3" type="text" value="" style="width: 100%" readonly>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="place_select" type="button" value="変更" class="file-upload" style="width: 100%" /><br />
                        <br />
                        <input id="place_finish" type="button" value="設定完了" class="file-upload" style="width: 100%" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" />
            <br />
            <font color="#95989A" size="2px">読み込み中</font>
        </div>
        <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
        </div>
    </form>
</body>
</html>
