using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_location : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        gc = new GCP_MYSQL();
        Panel mypan = (Panel)this.FindControl("main_Panel");
        Literal lip = new Literal();
        Query = "select * from nursing_room;";
        DataView ict_ff = gc.select_cmd(Query);
        bool chee = true;

        lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
        lip.Text += @"<script type="+'"'+"text/javascript"+'"'+ @">


var numm=0;

";


        lip.Text += @"
var pinImage_off = 'https://storage.googleapis.com//upload/nursing_room/.png';
        var pinImage_on = 'https://storage.googleapis.com//upload/nursing_room/.png';
    var locations = [";


        int num = 1;
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                {
                    lip.Text += "['" + ict_ff.Table.Rows[i]["name"].ToString() + @"', " + ict_ff.Table.Rows[i]["GPS_lat"].ToString() + @", " + ict_ff.Table.Rows[i]["GPS_lng"].ToString() + @"," + ict_ff.Table.Rows[i]["id"].ToString() + @", " + num + @"],";
                    num += 1;
                }

            }
        }
        lip.Text += @"];
    var markers = [];
 var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
      zoom: 6,
      center: new google.maps.LatLng(36.452569, 138.749121),
      mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    var marker, i;

    for (i = 0; i < locations.length; i++) {
      marker = new google.maps.Marker({
        id: locations[i][3],
        position: new google.maps.LatLng(locations[i][1], locations[i][2]),
        map: map,
        icon: pinImage_on,
        scaledSize: new google.maps.Size(30, 30)
      });
      markers.push(marker);
      google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
          infowindow.setContent(locations[i][0]);
          infowindow.open(map, marker);
        }
      })(marker, i));
    }
 }
function showMarkers() {
        changeicon(map);
      }
function changeicon(map) {
        for (var i = 0; i < markers.length; i++) {
for (var ii = 0; ii < arrcheck.length; ii++) {
        if(arrcheck[ii][0]==markers[i].id)
        {
            if(arrcheck[ii][1]==0){
                markers[i].icon=pinImage_on;
            }
            else{
                markers[i].icon=pinImage_off;
            }
        }
    }
          markers[i].setMap(map);


        }
      }
  </script>

<script async defer
    src=" + '"'+"https://maps.googleapis.com/maps/api/js?key=&libraries=drawing&language=ja&callback=initMap"+'"'+@">
    </script>


";

        main_Panel.Controls.Add(lip);

    }
}
