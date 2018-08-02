<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search_area.aspx.cs" Inherits="search_area" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="Scripts/jquery-1.4.1.min.js"></script>
     <!--玩Google Map 一定要引用此js-->
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=&sensor=true&language=ja&region=JP"></script>
    <style type="text/css">
        #txt2
        {
            width: 315px;
        }
        #txt1
        {
            width: 309px;
        }
        #txt3
        {
            width: 308px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--地圖-->
    <div>

        <br />

    </div>
    </form>
    <INPUT id="txt2" TYPE="text" value="2310005"><br />
    <button onclick="init()">Check!</button>
    <button onclick="getLocation()">Check2!</button>
    <br />
    <INPUT id="txt1" TYPE="text" value=""> <br />
    <INPUT id="txt3" TYPE="text" value="">
    <div id="div_showMap" style="width: 500px; height: 500px">
    </div>
    <script type="text/javascript">
        //網頁上所有的DOM都載入後
        var latlng = new google.maps.LatLng(35.44871, 139.64184180000007);
        var myOptions = {
            zoom: 16,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map($("#div_showMap")[0], myOptions);

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
                         var street_number = route = street = city = state = zipcode = country =city1=city2=city3 =formatted_address= '';
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
                         console.log(resadd);
                         document.getElementById("txt3").value = resadd;

                     }
                     else {
                         alert('Cannot determine address at this location.' + status);
                     }
                 }
             );
        }




        function getLocation() {
            getAddressInfoByZip(document.getElementById("txt2").value);
        }

        function response(obj) {
            console.log(obj);
        }
        function getAddressInfoByZip(zip) {
            if (zip.length >= 5 && typeof google != 'undefined') {
                var addr = {};
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': zip }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results.length >= 1) {
                            for (var ii = 0; ii < results[0].address_components.length; ii++) {
                                var street_number = route = street = city = state = zipcode = country =city1=city2=city3 =formatted_address= '';
                                var types = results[0].address_components[ii].types.join(",");
                                if (types == "street_number") {
                                    addr.street_number = results[0].address_components[ii].long_name;
                                }
                                if (types == "route" || types == "point_of_interest,establishment") {
                                    addr.route = results[0].address_components[ii].long_name;
                                }

                                if (types == "sublocality,political" || types == "locality,political" || types == "neighborhood,political" || types == "administrative_area_level_3,political") {
                                    addr.city = (city == '' || types == "locality,political") ? results[0].address_components[ii].long_name : city;
                                }
                                if (types == "administrative_area_level_1,political") {
                                    addr.state = results[0].address_components[ii].short_name;
                                }
                                if (types == "postal_code" || types == "postal_code_prefix,postal_code") {
                                    addr.zipcode = results[0].address_components[ii].long_name;
                                }
                                if (types == "country,political") {
                                    addr.country = results[0].address_components[ii].long_name;
                                }
                                if (types == "locality,political,ward") {
                                    addr.city1 = (city1 == '' || types == "locality,political,ward") ? results[0].address_components[ii].long_name : city1;
                                }
                                if (types == "political,sublocality,sublocality_level_1") {
                                    addr.city2 = (city2 == '' || types == "political,sublocality,sublocality_level_1") ? results[0].address_components[ii].long_name : city2;
                                }
                                if (types == "political,sublocality,sublocality_level_2") {
                                    addr.city3 = (city3 == '' || types == "political,sublocality,sublocality_level_2") ? results[0].address_components[ii].long_name : city3;
                                }
                            }
                            addr.success = true;
                            for (name in addr) {
                                console.log('### google maps api ### ' + name + ': ' + addr[name]);
                            }
                            response(addr);
                        } else {
                            response({ success: false });
                        }
                    } else {
                        response({ success: false });
                    }
                });
            } else {
                response({ success: false });
            }
        }


//        show_zip(map, 2310005);

//        var markers = [];

//        function show_zip($map, $zip_code) {

//            var c, g, h; $.get("coordinates/zip/" + $zip_code + ".txt", {}, function (a) {
//                var f = 0, d, e; $(a).find("ps").each(function () {
//                    $(this).find("p").each(function () {
//                        var a = []; $(this).find("c").each(function () { c = $(this); g = c.attr("t"); h = c.attr("n"); a.push(new google.maps.LatLng(g, h)) }); e = new google.maps.Polygon({ paths: a, strokeColor: "#FF0000", strokeOpacity: 0.8, strokeWeight: 3, fillColor: "#FF0000", fillOpacity: 0.35, zIndex: 1 }); e.setMap($map);

//                        markers.push(e);

//                        f++;
//                    });
//                });
//            });

//        }





    </script>

</body>
</html>
