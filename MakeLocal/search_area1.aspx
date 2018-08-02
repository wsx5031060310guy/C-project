<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search_area1.aspx.cs" Inherits="search_area1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
    <script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?key=&sensor=true"></script>
    <script type="text/javascript">
        var poly = null;
        var map;


        function initialize() {
            var myLatLng = new google.maps.LatLng(35.450128, 139.634801);
            var myOptions = {
                zoom: 5,
                center: myLatLng,
                mapTypeId: google.maps.MapTypeId.TERRAIN
            };



           map = new google.maps.Map(document.getElementById("map-canvas"),myOptions);

            $('input:button')
  .click(function () {
      var _this = $(this),
          _prev = _this.prev(),
          _val = _prev.val();

      _this.prev('select').remove();
      _this.prev('input:text').show();
      if (_prev[0].tagName == 'SELECT') {
          placeRequest(_val);
          return;
      }
      if (!_val.match(/[a-z]{3,}/i)) {
          alert('enter a value')
      }
      else {
          //request the place-id
          $.getJSON('https://api.twitter.com/1.1/geo/search.json?callback=?',
                  { query: _val },
                  function (r) {
                      alert(r.result.places);
                      if (!r.result.places.length) {
                          alert('no place found');
                      }
                      else {
                          if (r.result.places.length > 1) {
                              var list = $('<select><optgroup label="choose a location"></optgroup></select>');
                              for (var i = 0; i < r.result.places.length; ++i) {
                                  $('<option/>')
                            .text(r.result.places[i].full_name)
                             .val(r.result.places[i].url)
                              .appendTo($('optgroup', list));
                                  _this.prev('input:text').hide();

                              }
                              _this.before(list);
                          }
                          else {
                              placeRequest(r.result.places[0].url);
                          }

                      }
                  }
                 )
      }
  }
  );

        }
        function placeRequest(url) {
            $.getJSON(url + '?callback=?',
                                function (rr) {
                                    console.log('rr', rr)
                                    if (rr.geometry.type == 'Polygon') {
                                        drawPolygon(rr.geometry.coordinates[0])
                                    }
                                    else {
                                        alert('no polygon found')
                                    }
                                }
                               );
        }
        function drawPolygon(p) {
            if (window.poly) window.poly.setMap(null);
            var pp = [], bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < p.length; ++i) {
                pp.push(new google.maps.LatLng(p[i][1], p[i][0]));
                bounds.extend(pp[pp.length - 1]);
            }
            window.poly = new google.maps.Polygon({
                paths: pp,
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 3,
                fillColor: "#FF0000",
                fillOpacity: 0.35
            });

            window.poly.setMap(map);
            map.fitBounds(bounds);

        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>

  <input value="Berlin"><input type="button" value="search">
  <div id='map-canvas' style="width: 500px; height: 500px"></div>
</body>
</html>
