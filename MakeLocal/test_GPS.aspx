<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_GPS.aspx.cs" Inherits="test_GPS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>MY GPS</title>
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja&sensor=true"></script>
         <script src="Scripts/jquery-1.12.4.js"></script>
    <script>
        var displayCloseFoo = function (position) {
            var lat = position.coords.latitude;
            var lon = position.coords.longitude;
            alert(lat + "," + lon);
        };

        var displayError = function (error) {
            var errors = {
                1: 'Permission denied',
                2: 'Position unavailable',
                3: 'Request timeout'
            };
            alert("Error: " + errors[error.code]);
        };

        var runGeo = function () {
            if (navigator.geolocation) {
                var timeoutVal = 10 * 1000 * 1000;
                navigator.geolocation.getCurrentPosition(
                        displayCloseFoo,
                        displayError,
                        { enableHighAccuracy: true, timeout: timeoutVal, maximumAge: 0 }
                );
            }
            else {
                alert("Geolocation is not supported by this browser");
            }
        };
        runGeo();

        function geoFindMe() {
            var output = document.getElementById("out");

            if (!navigator.geolocation) {
                output.innerHTML = "<p>Geolocation is not supported by your browser</p>";
                return;
            }

            function success(position) {
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;

                output.innerHTML = '<p>Latitude is ' + latitude + '° <br>Longitude is ' + longitude + '°</p>';

                var img = new Image();
                img.src = "https://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=13&size=300x300&sensor=false";

                output.appendChild(img);
            };

            function error() {
                output.innerHTML = "Unable to retrieve your location";
            };

            output.innerHTML = "<p>Locating…</p>";

            navigator.geolocation.getCurrentPosition(success, error);
        }
    </script>
</head>
<body>
    <p><button onclick="geoFindMe()">Show my location</button></p>
<div id="out"></div>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
