<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Narrow.aspx.cs" Inherits="Narrow"%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="Scripts/jquery-1.4.1.min.js"></script>
     <!--玩Google Map 一定要引用此js-->
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?key=&sensor=true"></script>
    <style type="text/css">
        #txt2
        {
            width: 315px;
        }
        #txt1
        {
            width: 309px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--地圖-->
    <div>

        <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button"
            Visible="False" />
        <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:connStr %>"
        SelectCommand="SELECT * FROM [tb_company]"></asp:SqlDataSource>
        <br />

    </div>
    </form>
     <INPUT id="txt2" TYPE="text" value=""><br />
    <button onclick="init()">Check!</button>
    <br />
    <INPUT id="txt1" TYPE="text" value="">
    <div id="div_showMap" style="width: 500px; height: 500px">
    </div>
    <script type="text/javascript">
        //網頁上所有的DOM都載入後
        var len = 0;
        var chec = true;
        function init() {
            var add = document.getElementById("txt2").value;
            $.ajax({
                url: 'Check_address.ashx',
                type: 'post',
                data: { address: add },
                async: true,
                dataType: 'json',
                success: function (r) {
                    var mess = r.Message;
                    document.getElementById("txt1").value = mess;
                    if (r.lat == 0 && r.lng == 0) {
                        $('#div_showMap').hide();
                    } else {
                        $('#div_showMap').show();
                        MarkerOne(r);
                    }
                }
            });

        }

        var infowindow = new google.maps.InfoWindow();
        var map;
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
                    map = new google.maps.Map($("#div_showMap")[0], myOptions);
                    var imageUrl = ""; //空字串就會使用預設圖示

                    //加一個Marker到map中
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        icon: imageUrl,
                        html: item.lat +","+ item.lng
                    });
                    google.maps.event.addListener(marker, 'click', function () {
                        infowindow.setContent(this.html);
                        infowindow.open(map, this);
                    });
                    markers.push(marker);
                },
                error: function (result) {
                    alert(result.responseText);
                }

            });
        }
    </script>

</body>
</html>
