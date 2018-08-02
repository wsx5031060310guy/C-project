<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_google_area.aspx.cs" Inherits="test_google_area" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no">
     <meta charset="utf-8">
    <title>Polygon Arrays</title>
    <style>
      html, body { background-color:#E9EBEE;
        height: 100%;
        margin: 0;
        padding: 0;
      }
      #map {
        height: 800px;
        width: 800px;
      }
    </style>
    <script src="Scripts/jquery-1.12.4.js"></script>
        <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja"></script>
  </head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
     <div id="map">

        </div>
    <script>
        $(document).ready(function () {

            initMap();
        });
        // This example creates a simple polygon representing the Bermuda Triangle.
        // When the user clicks on the polygon an info window opens, showing
        // information about the polygon's coordinates.

        var map,geocoder, popup;;
        var infoWindow;

        function initMap() {
            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 12,
                center: { lat: 34.9198341369629, lng: 136.880279541016 },
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });

            // Define the LatLng coordinates for the polygon.
            var triangleCoords = [
               { lat: 34.9198341369629, lng: 136.880279541016 },
{ lat: 34.9211006164551, lng: 136.882369995117 },
{ lat: 34.9225196838379, lng: 136.883804321289 },
{ lat: 34.9233436584472, lng: 136.884902954102 },
{ lat: 34.9242744445802, lng: 136.885818481446 },
{ lat: 34.9251403808595, lng: 136.886825561524 },
{ lat: 34.9257888793945, lng: 136.887252807617 },
{ lat: 34.9271812438965, lng: 136.887878417969 },
{ lat: 34.9284591674805, lng: 136.888931274414 },
{ lat: 34.9288291931153, lng: 136.889129638672 },
{ lat: 34.9296569824219, lng: 136.889373779297 },
{ lat: 34.9305419921875, lng: 136.889434814453 },
{ lat: 34.9314270019532, lng: 136.889358520508 },
{ lat: 34.9322509765625, lng: 136.889114379883 },
{ lat: 34.9326171875, lng: 136.888916015625 },
{ lat: 34.9338989257814, lng: 136.887847900391 },
{ lat: 34.9353065490724, lng: 136.88720703125 },
{ lat: 34.9359664916993, lng: 136.886825561524 },
{ lat: 34.9372291564943, lng: 136.885726928711 },
{ lat: 34.9386367797852, lng: 136.885116577149 },
{ lat: 34.9393005371094, lng: 136.884719848633 },
{ lat: 34.9402542114258, lng: 136.883758544922 },
{ lat: 34.9415359497071, lng: 136.8828125 },
{ lat: 34.9422492980957, lng: 136.882171630859 },
{ lat: 34.9439811706543, lng: 136.880233764648 },
{ lat: 34.945827484131, lng: 136.8828125 },
{ lat: 34.9469261169434, lng: 136.884735107422 },
{ lat: 34.947811126709, lng: 136.885787963867 },
{ lat: 34.9485893249512, lng: 136.886901855469 },
{ lat: 34.9504051208497, lng: 136.888763427734 },
{ lat: 34.9527282714844, lng: 136.889953613281 },
{ lat: 34.9542274475099, lng: 136.891738891602 },
{ lat: 34.9546203613282, lng: 136.892517089844 },
{ lat: 34.9549179077148, lng: 136.894134521484 },
{ lat: 34.9550018310547, lng: 136.895950317383 },
{ lat: 34.9549789428711, lng: 136.90104675293 },
{ lat: 34.9548873901368, lng: 136.904205322266 },
{ lat: 34.9547119140626, lng: 136.905960083008 },
{ lat: 34.9544754028321, lng: 136.906982421875 },
{ lat: 34.9542999267579, lng: 136.907424926758 },
{ lat: 34.95361328125, lng: 136.908569335938 },
{ lat: 34.9533538818361, lng: 136.909255981445 },
{ lat: 34.9531784057618, lng: 136.90998840332 },
{ lat: 34.953109741211, lng: 136.9111328125 },
{ lat: 34.9533958435059, lng: 136.912109375 },
{ lat: 34.9539031982423, lng: 136.912948608399 },
{ lat: 34.9545402526856, lng: 136.913681030274 },
{ lat: 34.9560089111329, lng: 136.914855957031 },
{ lat: 34.9572677612305, lng: 136.916091918945 },
{ lat: 34.9626617431641, lng: 136.91943359375 },
{ lat: 34.9606170654298, lng: 136.921478271485 },
{ lat: 34.9586982727051, lng: 136.923049926758 },
{ lat: 34.9577751159669, lng: 136.924118041992 },
{ lat: 34.9559211730958, lng: 136.925643920898 },
{ lat: 34.9524765014648, lng: 136.929443359375 },
{ lat: 34.9517822265626, lng: 136.930114746094 },
{ lat: 34.9502868652345, lng: 136.931335449219 },
{ lat: 34.9485397338868, lng: 136.933486938477 },
{ lat: 34.9467353820801, lng: 136.93717956543 },
{ lat: 34.9451637268067, lng: 136.941390991211 },
{ lat: 34.9449157714844, lng: 136.942474365234 },
{ lat: 34.9444847106935, lng: 136.945220947266 },
{ lat: 34.9439163208008, lng: 136.947494506836 },
{ lat: 34.9411010742188, lng: 136.945770263672 },
{ lat: 34.939193725586, lng: 136.944030761719 },
{ lat: 34.9372711181641, lng: 136.94287109375 },
{ lat: 34.9366111755372, lng: 136.942306518555 },
{ lat: 34.9338531494141, lng: 136.939376831055 },
{ lat: 34.9316864013672, lng: 136.937866210938 },
{ lat: 34.9306945800781, lng: 136.937377929688 },
{ lat: 34.9264755249025, lng: 136.936264038086 },
{ lat: 34.9241523742676, lng: 136.936065673828 },
{ lat: 34.9194335937499, lng: 136.936264038086 },
{ lat: 34.9156799316406, lng: 136.936996459961 },
{ lat: 34.9146881103516, lng: 136.936996459961 },
{ lat: 34.9137268066406, lng: 136.936737060547 },
{ lat: 34.9132690429688, lng: 136.936477661133 },
{ lat: 34.9128379821778, lng: 136.936065673828 },
{ lat: 34.9122314453125, lng: 136.934951782227 },
{ lat: 34.9119567871095, lng: 136.933654785156 },
{ lat: 34.9118461608888, lng: 136.931579589844 },
{ lat: 34.9120254516602, lng: 136.920394897461 },
{ lat: 34.9119720458986, lng: 136.912200927734 },
{ lat: 34.9118194580078, lng: 136.910018920898 },
{ lat: 34.9113578796387, lng: 136.90657043457 },
{ lat: 34.9109764099121, lng: 136.899322509766 },
{ lat: 34.9105453491212, lng: 136.895950317383 },
{ lat: 34.9105072021484, lng: 136.894668579102 },
{ lat: 34.9105949401855, lng: 136.89338684082 },
{ lat: 34.9112091064454, lng: 136.891036987305 },
{ lat: 34.9119186401368, lng: 136.889663696289 },
{ lat: 34.9132385253907, lng: 136.887832641602 },
{ lat: 34.9158325195312, lng: 136.884750366211 },
{ lat: 34.9171600341797, lng: 136.882934570312 },
{ lat: 34.9198341369629, lng: 136.880279541016 }
            ];

            // Construct the polygon.
            var bermudaTriangle = new google.maps.Polygon({
                paths: triangleCoords,
                strokeColor: '#FF0000',
                strokeOpacity: 0.2,
                strokeWeight: 3,
                fillColor: '#FF0000',
                fillOpacity: 0.35
            });
            bermudaTriangle.setMap(map);

            // Add a listener for the click event.
            bermudaTriangle.addListener('click', showArrays);

            infoWindow = new google.maps.InfoWindow;




        }

        /** @this {google.maps.Polygon} */
        function showArrays(event) {
            // Since this polygon has only one path, we can call getPath() to return the
            // MVCArray of LatLngs.
            var vertices = this.getPath();

            var contentString = '<b>Bermuda Triangle polygon</b><br>' +
                'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
                '<br>';

            // Iterate over the vertices.
            for (var i = 0; i < vertices.getLength() ; i++) {
                var xy = vertices.getAt(i);
                contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
                    xy.lng();
            }

            // Replace the info window's content and position.
            infoWindow.setContent(contentString);
            infoWindow.setPosition(event.latLng);

            infoWindow.open(map);
        }


        function Markererer() {
            var location; //取得此資料的位置
            //建立緯經度座標物件
            var latlng = new google.maps.LatLng(34.9211006164551,136.882369995117);

            var myOptions = {
                zoom: 16,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            /*產生地圖*/
            map = new google.maps.Map(document.getElementById("map"), myOptions);
            var imageUrl = ""; //空字串就會使用預設圖示

            //加一個Marker到map中
            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                draggable: true,
                icon: imageUrl,
                html: latlng.lat + "," + latlng.lng
            });
            geocodePosition(marker.getPosition());
            google.maps.event.addListener(marker, 'dragend', function () {
                geocodePosition(marker.getPosition());
            });
        }

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




        ////insert Japan City web api
        //$(function () {
        //$("#Button1").click(function () {
        //$.ajax({
        //    type: "POST",
        //    url: "test_google_area.aspx/insert_city",
        //    data: "{param1: '0' }",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    cache: false,
        //    success: function (result) {
        //        //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
        //        console.log(result.d);
        //        //alert(result.d);

        //    },
        //    error: function (result) {
        //        //console.log(result.Message);
        //        //alert(result.d);
        //    }
        //});
        //});
        //});


    </script>
        <asp:Panel ID="Panel1" runat="server">

         </asp:Panel>
        <input id="Button1" type="button" onclick="initMap1()" value="button" />
        <input id="Button2" type="button" onclick="Markererer()" value="button1" />
    <%--<script async defer
        src="https://maps.googleapis.com/maps/api/js?key=&signed_in=true&callback=initMap"></script>--%>
    </form>
     </body>
</html>
