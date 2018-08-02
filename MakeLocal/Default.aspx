<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
     <!--玩Google Map 一定要引用此js-->
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
</head>
<body>
    <form id="form1" runat="server">
     <!--地圖-->
     <div id="div_showMap" style="width: 800px; height: 800px">
    </div>
    </form>

    <script type="text/javascript">
        $(document).ready(init);
        //網頁上所有的DOM都載入後
        var len = 0;
        var chec = true;
        function init() {
            var zip_no = '100';
            len = 0;
            $.ajax(
            {
                url: 'DataTableSource1.ashx',
                type: 'post',
                async: true,
                data: { zip_no: zip_no },
                dataType: 'json',
                success: function (datas) {
                    $(datas).each(function (index, item) {
                        len += 1; //一一加入陣列
                    });
                }
            });
            t = setTimeout(function () {
                QueryDataTable();
            }, 300);
        }
        function useround(min, max) {
            return Math.round(Math.random() * (max - min) + min);
        }
        //放DataTable資料的全域變數
        var array = new Array();
        var markers = [];
        //抓出地址資料
        function QueryDataTable() {
            array = new Array();
            var ranid = 1;
            if (chec) {
                ranid = useround(1, len);
            } else {
                ranid = 1;
            }
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
        function myFunction() {
            if (chec) { chec = false; } else { chec = true; }
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(null);
            }
            markers = [];
            init();
        }
        var t;
        function repeatFunc(startIndex) {

            MarkerOne(array[startIndex]); //標記一筆在地圖上
            startIndex++;
            if (startIndex >= array.length) {
                clearTimeout(t); //停止遞迴
                return; //停止函數
            }
            t = setTimeout(function () {
                repeatFunc(startIndex);
            }, 300); //延遲300毫秒才處理下一筆

        }

        var first = true;
        var infowindow = new google.maps.InfoWindow();
        var map;
        //把地址轉成JSON格式並在地圖上加入標記點
        function MarkerOne(item) {
            $.ajax({
                url: "MarkerOne.ashx",
                type: "get",
                data: { address: item.地址,lat:item.lat,lng:item.lng },
                async: true,
                dataType: "json",
                success: function (r) {
                    var location; //取得此資料的位置
                    //建立緯經度座標物件
                    var latlng ;
                    if (item.lat == 0 && item.lng == 0) {
                        location = r.results[0].geometry.location;
                        latlng = new google.maps.LatLng(location.lat, location.lng);
                    } else {
                        latlng = new google.maps.LatLng(item.lat, item.lng);
                    }
                    if (first) {//第一次執行 
                        /*以哪個緯經度中心來產生地圖*/
                        var myOptions = {
                            zoom: 16,
                            center: latlng,
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        };
                        /*產生地圖*/
                        map = new google.maps.Map($("#div_showMap")[0], myOptions);
                        first = false;
                    } //End if (first == true) 
                    var imageUrl = ""; //空字串就會使用預設圖示
                    if (item.iconName != "")//有值
                    {
                        //可以是相對路徑也可以是http://開頭的絕對路徑，路徑錯誤的話圖示不會出來
                        imageUrl = "images/" + item.iconName;
                    }
                    if (chec) {
                        document.getElementById("txt2").value = item.名稱;
                    } else {
                        document.getElementById("txt2").value = "全部!!";
                    }
                    //加一個Marker到map中
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        title: item.名稱,
                        icon: imageUrl,
                        html: item.company_desc
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
    <button onclick="myFunction()">BinGo!</button>
    <INPUT id="txt2" TYPE="text" value=""> 
</body>
</html>