using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_map : System.Web.UI.Page
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
         if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                {
                    lip.Text += "<div id=" + '"' + "mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"</div>";
                    lip.Text += "<div id=" + '"' + "mamarotime"+ict_ff.Table.Rows[i]["id"].ToString() + '"' + "></div>";

                }

            }
        }
        lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-app.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-database.js" + '"' + @"></script>";
        lip.Text += @"<script type="+'"'+"text/javascript"+'"'+ @">

 var firebase;
        var config = {
            databaseURL: " + '"' + @"" + '"' + @"
        };
        firebase.initializeApp(config);

var numm=0;

";
        string arr = "var arrcheck = [";
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                {
                    lip.Text += "var myVar" + ict_ff.Table.Rows[i]["id"].ToString() + @";";
                    arr += "[" + ict_ff.Table.Rows[i]["id"].ToString() + ",0],";
                }

            }
        }
        arr += "];";
        lip.Text += arr;
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
               if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                {
                    lip.Text += @"
$(function(){
$show1 = $('#mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show1.hide();
});
          var startDate" + ict_ff.Table.Rows[i]["id"].ToString() + @" = new Date();

        var ref = firebase.database().ref('/mamarostate/lock/" + ict_ff.Table.Rows[i]["id"].ToString() + @"/').limitToLast(10);

        ref.on('value', function (snapshot) {

$(function(){
$show1 = $('#mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show1.hide();
});

            var cdata = [];
            for (var i in snapshot.val()) {
                cdata.push({
                    label: snapshot.val()[i].time,
                    value: snapshot.val()[i].state
                });
            }




            var len = cdata.length;

var checkmama=1;


for (var i = 0; i < len; i++) {
      var chec" + ict_ff.Table.Rows[i]["id"].ToString() + @"=1;
        if(cdata[i].value=='0')
        {
            if(i+1<len)
            {
                if(cdata[i+1].value=='1')
                {
                    chec" + ict_ff.Table.Rows[i]["id"].ToString() + @"=0;
                }else{
                    chec" + ict_ff.Table.Rows[i]["id"].ToString() + @"=0;
                }
            }
            if(chec" + ict_ff.Table.Rows[i]["id"].ToString() + @">0)
            {
                checkmama=0;
                startDate" + ict_ff.Table.Rows[i]["id"].ToString() + @" = moment(cdata[i].label).format(" + '"' + @"YYYY/M/D H:m:s" + '"' + @");


$(function(){
$show1 = $('#mamaroname" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show1.show();

$show2 = $('#mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show2.show();
});

myVar" + ict_ff.Table.Rows[i]["id"].ToString() + @"=setInterval(
function(){

var endDatestr = moment(new Date());

var startda=moment(startDate" + ict_ff.Table.Rows[i]["id"].ToString() + @");

var diffTime = moment(endDatestr).diff(startDate" + ict_ff.Table.Rows[i]["id"].ToString() + @");
var duration = moment.duration(diffTime);

var hhh =duration.hours();
var mmm =duration.minutes();
var sss =duration.seconds();


$show = $('#mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show.empty();
$show.append(hhh+':'+mmm+':'+sss);
$show.append('<br>'+startDate" + ict_ff.Table.Rows[i]["id"].ToString() + @");
},1000);

            }else{

clearInterval(myVar" + ict_ff.Table.Rows[i]["id"].ToString() + @");
$(function(){
$show = $('#mamarotime" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show.empty();
$show.hide();
});



}


        }


      }

if(checkmama>0){
for (i = 0; i < arrcheck.length; i++) {
        if(arrcheck[i][0]==" + ict_ff.Table.Rows[i]["id"].ToString() + @")
        {
            arrcheck[i][1]=0;
        }
    }
showMarkers();
}else{
for (i = 0; i < arrcheck.length; i++) {
        if(arrcheck[i][0]==" + ict_ff.Table.Rows[i]["id"].ToString() + @")
        {
            arrcheck[i][1]=1;
        }
    }
showMarkers();

}
numm=0;
for (i = 0; i < arrcheck.length; i++) {
        if(arrcheck[i][1]==1)
        {
            numm+=1;
        }
    }



$(function(){
$showw = $('#mamaroonline');
$showw.show();
$showw.empty();
$showw.append('<h2>使用中:'+numm+'</h2>');
});


        });



";
                    //lip.Text += "['" + ict_ff.Table.Rows[i]["name"].ToString() + @"', " + ict_ff.Table.Rows[i]["GPS_lat"].ToString() + @", " + ict_ff.Table.Rows[i]["GPS_lng"].ToString() + @"," + ict_ff.Table.Rows[i]["id"].ToString() + @", " + num + @"],";

                }

            }
        }


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
        lip.Text += @"<h2>全部:"+(num-1)+@"</h2>";

        main_Panel.Controls.Add(lip);

    }
}
