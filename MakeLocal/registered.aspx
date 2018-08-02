<%@ Page Language="C#" AutoEventWireup="true" CodeFile="registered.aspx.cs" Inherits="registered" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="stylesheet" href="css/jquery-ui.css">
     <link rel="stylesheet" href="css/file-upload.css" type="text/css">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/registered.css">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja"></script>

    <script src="Scripts/jquery-1.12.4.js"></script>

    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {

            $("#dialog").dialog({
                autoOpen: false,
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
                document.getElementById("<%=postal_code_TextBox.ClientID%>").value = document.getElementById("txt2").value;
                $("#dialog").dialog("close");
            });

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
                        document.getElementById("txt2").value = r.Message;
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
                                     document.getElementById("txt2").value = address_component.long_name;
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

            function PopulateDays() {

                var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
            var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
            var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
            var y = ddlYear.options[ddlYear.selectedIndex].value;
            var m = ddlMonth.options[ddlMonth.selectedIndex].value != 0;
            if (ddlMonth.options[ddlMonth.selectedIndex].value != 0 && ddlYear.options[ddlYear.selectedIndex].value != 0) {
                var dayCount = 32 - new Date(ddlYear.options[ddlYear.selectedIndex].value, ddlMonth.options[ddlMonth.selectedIndex].value - 1, 32).getDate();
                ddlDay.options.length = 0;
                for (var i = 1; i <= dayCount; i++) {
                    AddOption(ddlDay, i, i);
                }
            }
        }

        function AddOption(ddl, text, value) {
            var opt = document.createElement("OPTION");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }

        function Validate(sender, args) {
            var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
        var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
        var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
        args.IsValid = (ddlDay.selectedIndex != 0 && ddlMonth.selectedIndex != 0 && ddlYear.selectedIndex != 0)
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


    $(document).ready(function () {

        var counter = 2;

        $("#addButton").click(function () {

            if (counter > 10) {
                alert("Only 10 textboxes allow");
                return false;
            }

            $("#TextBoxesGroup").append('<div id="TextBoxDiv' + counter + '"><input type="text" name="yearsold_textbox' + counter + '" id="yearsold_textbox' + counter + '" value="" placeholder="年齡を入力" style="width: 167px; position: relative; white-space: pre-wrap; background-repeat: no-repeat !important; background-attachment: local !important; background-position: 1px 9px !important; background-image: url("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKkAAAAPCAYAAACfkO/cAAAAX0lEQVRoQ+3SQREAAAjDMObfNDb6CAp2JTunQLzA4vvMU+AghSBfANL8iwyElIF8AUjzLzIQUgbyBSDNv8hASBnIF4A0/yIDIWUgXwDS/IsMhJSBfAFI8y8yEFIG8gUeZRIAEMqvLvYAAAAASUVORK5CYII=") !important; font-size: 13.3333px;"><input type="radio" name="sex' + counter + '" value="Girl">女性<input type="radio" name="sex' + counter + '" value="Boy">男性</div>');
            counter++;
        });

        $("#removeButton").click(function () {
            if (counter == 2) {
                alert("No more textbox to remove");
                return false;
            }

            counter--;

            $("#TextBoxDiv" + counter).remove();

        });


        $("#<%=Button2.ClientID %>").click(function () {
            var param1 = "<%= Session["id"]%>".toString();
            for (i = 1; i < counter; i++) {
                if ($('#yearsold_textbox' + i).val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '') != "" && document.querySelector('input[name="sex' + i + '"]:checked') != null) {
                    $.ajax({
                        type: "POST",
                        url: "registered.aspx/Save",
                        data: "{param1: '" + param1 + "' , param2 :'" + $('#yearsold_textbox' + i).val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '') + "',param3 :'" + document.querySelector('input[name="sex' + i + '"]:checked').value + "'}",
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
                }
            }

        });


        $("#<%=Button3.ClientID %>").click(function () {
            var param1 = "<%= Session["id"]%>".toString();
                for (i = 1; i < counter; i++) {
                    if ($('#yearsold_textbox' + i).val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '') != "" && document.querySelector('input[name="sex' + i + '"]:checked') != null) {
                        $.ajax({
                            type: "POST",
                            url: "registered.aspx/Save",
                            data: "{param1: '" + param1 + "' , param2 :'" + $('#yearsold_textbox' + i).val().replace("'", "").replace('"', "").replace("`", "").replace(/\s/g, '') + "',param3 :'" + document.querySelector('input[name="sex' + i + '"]:checked').value + "'}",
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
                    }
                }

            });


    });
        $(function () {
            $(document).tooltip();
        });
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
                                    <asp:Image ID="Label_logo" Style="width: 60px; height: auto;" runat="server" ImageUrl="images/logo1.png"></asp:Image></td>

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

            <table style="width: 100%; margin-top: 70px">
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
                                        Text="ご自身の情報"></asp:Label>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label4" runat="server" Text="お名前"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label9" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="60%">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:TextBox ID="real_first_name_TextBox" runat="server" Width="90%" Wrap="False" placeholder="姓"
                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="rfname_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:TextBox ID="real_second_name_TextBox" runat="server" Width="100%" Wrap="False" placeholder="名"
                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="rsname_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label5" runat="server" Text="フリガナ"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label14" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:TextBox ID="real_spell_first_name_TextBox" runat="server" Width="90%" Wrap="False" placeholder="セイ"
                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="rsfname_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:TextBox ID="real_spell_second_name_TextBox" runat="server" Width="100%" Wrap="False" placeholder="メイ"
                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="rssname_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label6" runat="server" Text="性别"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label15" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="Sex_radio" runat="server"
                                        RepeatDirection="Horizontal">

                                        <asp:ListItem Value="0">女性</asp:ListItem>
                                        <asp:ListItem Value="1">男性</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Label ID="sex_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label7" runat="server" Text="生年月日"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label16" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox" onchange="PopulateDays()"
                                        Height="20px">
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;<asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox" onchange="PopulateDays()"
                                        Height="20px">
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;<asp:DropDownList ID="ddlDay" runat="server" CssClass="textbox"
                                        Height="20px">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="Label8" runat="server" Text="就労状况"></asp:Label>
                                    <br />
                                </td>
                                <td>
                                    <asp:DropDownList ID="work_DropDownList" runat="server" Width="100%"
                                        CssClass="textbox" Height="20px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>常勤</asp:ListItem>
                                        <asp:ListItem>パート/アルバイト</asp:ListItem>
                                        <asp:ListItem>自営業</asp:ListItem>
                                        <asp:ListItem>無職</asp:ListItem>
                                        <asp:ListItem>その他</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <hr />

                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="#FF5050"
                                        Text="ご住所"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="50%">
                                                <asp:Label ID="Label12" runat="server" Text="郵便番号"></asp:Label>
                                            </td>
                                            <td width="50%">
                                                <asp:Label ID="Label21" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                                    Text="※必須"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="60%" valign="top">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td width="60%" valign="top">
                                                <asp:TextBox ID="postal_code_TextBox" runat="server" Width="100%" Wrap="False" placeholder="〒郵便番号を入力"
                                                    CssClass="textbox" Height="20px"></asp:TextBox>
                                                <br />
                                                <asp:Label ID="poc_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                            <td width="40%" valign="top">
                                                <input id="opener" type="button" value="住所検索" style="border-style: none; background-color: #E9EBEE; color: #0000FF; font-family: 游ゴシック体, 'Yu Gothic', YuGothic, 'ヒラギノ角ゴシック Pro', 'Hiragino Kaku Gothic Pro', メイリオ, Meiryo, Osaka, 'ＭＳ Ｐゴシック', 'MS PGothic', 'sans-serif !important';" /></td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table style="width: 100%;">
                            <tr>
                                <td width="50%">
                                    <asp:Label ID="Label13" runat="server" Text="都道府県"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:Label ID="Label22" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td width="60%">
                                    <asp:DropDownList ID="City_DropDownList" runat="server" Width="100%"
                                        CssClass="textbox" Height="20px">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </td>
                                <td width="40%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table style="width: 100%;">
                            <tr>
                                <td width="50%">
                                    <asp:Label ID="Label19" runat="server" Text="市区町村"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:Label ID="Label23" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:TextBox ID="add_TextBox" runat="server" Width="100%" Wrap="False"
                            CssClass="textbox" Height="20px"></asp:TextBox>
                        <br />
                        <asp:Label ID="city_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table style="width: 100%;">
                            <tr>
                                <td width="50%">
                                    <asp:Label ID="Label20" runat="server" Text="番地"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:Label ID="Label24" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:TextBox ID="add_TextBox1" runat="server" Width="100%" Wrap="False"
                            CssClass="textbox" Height="20px"></asp:TextBox>
                        <br />
                        <asp:Label ID="chome_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label26" runat="server" Text="マンション名"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="apartment_TextBox" runat="server" Width="100%" Wrap="False" placeholder="※部屋番号まで入力"
                            CssClass="textbox" Height="20px"></asp:TextBox>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label27" runat="server" Text="最寄り駅"></asp:Label>
                    </td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td width="50%">
                                    <asp:TextBox ID="train_line_TextBox" runat="server" Width="90%" Wrap="False" placeholder="路線名"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>
                                <td width="50%">
                                    <asp:TextBox ID="train_station_TextBox" runat="server" Width="100%" Wrap="False" placeholder="駅名"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <hr />
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="Label28" runat="server" Font-Bold="True" ForeColor="#FF5050"
                            Text="ご連絡先"></asp:Label>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td width="40%" valign="top">
                        <table style="width: 100%;">
                            <tr>
                                <td width="50%">
                                    <asp:Label ID="Label29" runat="server" Text="ご連絡先"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:Label ID="Label32" runat="server" Font-Size="XX-Small" ForeColor="#FF5050"
                                        Text="※必須"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="60%">
                        <asp:TextBox ID="phone_TextBox" runat="server" Width="100%" Wrap="False" placeholder="電話番号をハイフン無しで入力"
                            CssClass="textbox" Height="20px"></asp:TextBox>
                        <br />
                        <asp:Label ID="phone_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label30" runat="server" Text="緊急連絡先"></asp:Label>
                        <br />
                        <asp:Label ID="Label34" runat="server" Font-Size="XX-Small" ForeColor="#CCCCCC"
                            Text="※本人以外"></asp:Label>
                    </td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="other_phone_TextBox" runat="server" Width="100%" Wrap="False" placeholder="電話番号をハイフン無しで入力"
                                        CssClass="textbox" Height="20px"></asp:TextBox>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="50%" align="right" valign="top">
                                    <asp:Label ID="Label35" runat="server" Font-Size="XX-Small" ForeColor="Black"
                                        Text="本人との間柄"></asp:Label>
                                    &nbsp;&nbsp;</td>
                                <td width="50%">
                                    <asp:DropDownList ID="relationship_DropDownList" runat="server" Width="100%"
                                        CssClass="textbox" Height="20px" placeholder="本人との間柄">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>夫</asp:ListItem>
                                        <asp:ListItem>妻</asp:ListItem>
                                        <asp:ListItem>父（義理父）</asp:ListItem>
                                        <asp:ListItem>母（義理母）</asp:ListItem>
                                        <asp:ListItem>祖父母（義理祖父母）</asp:ListItem>
                                        <asp:ListItem>おじ/おば</asp:ListItem>
                                        <asp:ListItem>めい/おい</asp:ListItem>
                                        <asp:ListItem>子</asp:ListItem>
                                        <asp:ListItem>その他</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="style1">
                        <asp:Label ID="Label36" runat="server" Text="同居家族について"></asp:Label>
                    </td>
                    <td valign="top">
                        <%--<input type='button' value='追加' id='addButton'>--%><%--<input type='button' value='削除' id='removeButton'>--%>
                        <table style="width: 100%;">
                            <tr>
                                <td width="100%" align="left">
                                    <div id="TextBoxDiv1">
                                        <input type="text" name="yearsold_textbox1" id="yearsold_textbox1" value="" placeholder="年齡を入力" style="width: 167px" />
                                        <input type="radio" name="sex1" value="Girl" />女性<input type="radio" name="sex1" value="Boy" />男性
                                    </div>
                                    <div id="TextBoxesGroup">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" valign="bottom" style="white-space: nowrap;">
                                    <asp:HyperLink ID='addButton' runat="server" NavigateUrl="javascript:void(0);"
                                        Target="_blank" Font-Size="Small" Font-Underline="False">+追加</asp:HyperLink>
                                    <asp:HyperLink ID='removeButton' runat="server" NavigateUrl="javascript:void(0);"
                                        Target="_blank" Font-Size="Small" Font-Underline="False">-削除</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="style2" align="center">
                        <br />
                        <asp:Label ID="result_Label" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Red" Text="事前登録受付中　※現在は利用できません。"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <td class="disable_button" width="50%">
                                <asp:Button ID="Button2" runat="server" Text="サポーターになる" CssClass="file-upload"
                                    Width="95%" OnClientClick="ShowProgressBar();" OnClick="Button2_Click" Enabled="False" />
                            </td>
                            <td class="disable_button" align="right" width="50%">
                                <asp:Button ID="Button3" runat="server" Text="子どもを預けたい" CssClass="file-upload"
                                    Width="95%" OnClientClick="ShowProgressBar();" OnClick="Button3_Click" Enabled="False" />
                            </td>
                        </table>
                    </td>
                </tr>
                </td>
                            <td class="space">&nbsp;</td>
                </tr>
            <tr>
                <td class="space">&nbsp;</td>
                <td width="50%">&nbsp;</td>
                <td class="space">&nbsp;</td>
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
    <div id="dialog" title="住所検索">
        <table style="width: 100%;" align="center">
            <tr>
                <td>
                    <input id="txt2" type="text" onkeydown="Javascript: if (event.which == 13 || event.keyCode == 13) runScript();" placeholder="住所を入力" style="width: 100%;" title="【Enter】キーを押してください"><br />
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
                    <input id="place_select" type="button" value="この住所で検索" class="file-upload" style="width: 100%" />
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
