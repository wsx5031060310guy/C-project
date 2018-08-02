using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class mamaro_grid_one : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    public class gridinfo
    {
        public string time = "";
        public string data = "";
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        List<gridinfo> gridlist=new List<gridinfo>();
        //using (StreamReader sr = new StreamReader(Server.MapPath("~/Content/treatments.json")))
        //{
        //    gridlist = JsonConvert.DeserializeObject<List<gridinfo>>(sr.ReadToEnd());
        //}



        Panel mypan = (Panel)this.FindControl("main_Panel");
        Literal lip = new Literal();
        JObject oo = JObject.Parse(File.ReadAllText(Server.MapPath("~/json/-46-export.json")));
            var eye = oo.SelectTokens("$..eye").ToList();
            var time = oo.SelectTokens("$..time").ToList();
            //lip.Text += ("eye : " + (string)eye[0]);
            //lip.Text += ("time : " + (string)time[0]);
            gridinfo gri = new gridinfo();
            int len = eye.Count;
            for (int i=0;i<len;i++)
            {
                gri = new gridinfo();
                gri.time = (string)time[i];
                gri.data = (string)eye[i];
                gridlist.Add(gri);
            }
            gridlist.Sort((x, y) => -x.time.CompareTo(y.time));
            lip.Text += @"min:<br>
  <input type='text' id='minv' value='0'><br>
  max:<br>
  <input type='text' id='maxv' value='40'><br>";
            lip.Text += @"<hr>";
            lip.Text += @"<div id=" + '"' + @"chart-container__" + '"' + @">ハッピーママフェスタ川崎</div>";
            lip.Text += @"<div id=" + '"' + @"chart-container_"+ '"' + @"></div>";
            lip.Text += @"<canvas id=" + '"' + @"chart-container" + '"' + @" width='400px' height='400px'></canvas>";
            lip.Text += @"<br id=" + '"' + @"brdiv" + '"' + @">";
            lip.Text += @"<input type='range' id=" + '"' + @"slider" +  '"' + @" min='0' max='10' step='1' value='5'>";
            lip.Text += @"<div id=" + '"' + @"sliderva" + '"' + @"></div>";

            lip.Text += @"<hr id=" + '"' + @"hrdiv" + '"' + @">";
            lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
            lip.Text += @"<script>
var cdata = [
";
            for (int i = 0; i < len; i++)
            {
                lip.Text += @"['eye':'" + gridlist[i].data + @"','time':'" + gridlist[i].time+ "'],";
            }
        lip.Text += @"];
$(function(){
var canvasElement = $('#chart-container');
var context = canvasElement[0].getContext('2d');
$('#slider').prop({'min':0,'max':cdata.length-1});
$('#slider').val(cdata.length-1);
$('#sliderva').text($('#slider').val());
var minv = parseFloat($('#minv').val());
var maxv = parseFloat($('#maxv').val());

var str=cdata[cdata.length-1]['value'].split(' ');

var index=0;

// Filled
for (var y = 0; y < 400; y += 50) {
 for (var x = 0; x < 400; x += 50) {

var v1 = parseFloat(str[index]);
if(isNaN(v1)==false){


var cc=Math.round(360*((v1-minv)/(maxv-minv)));



        context.fillStyle = 'hsl('+cc+', 100%, 50%)';
        context.fillRect(x, y, 50, 50);
}
index+=1;


    }
}
});



var slide=document.getElementById('slider');

slide.onchange=function(){
$(function(){
$('#sliderva').text($('#slider').val());
$('#chart-container_').text(cdata[$('#slider').val()]['label']);
var minv = parseFloat($('#minv').val());
var maxv = parseFloat($('#maxv').val());

var canvasElement = $('#chart-container');
var context = canvasElement[0].getContext('2d');

var str=cdata[$('#slider').val()]['value'].split(' ');

var index=0;

// Filled
for (var y = 0; y < 400; y += 50) {
 for (var x = 0; x < 400; x += 50) {

var v1 = parseFloat(str[index]);
if(isNaN(v1)==false){


var cc=Math.round(360*((v1-minv)/(maxv-minv)));



        context.fillStyle = 'hsl('+cc+', 100%, 50%)';
        context.fillRect(x, y, 50, 50);
}
index+=1;


    }
}
});
}
</script>";
            //JObject ooii = oo.GetEnumerator();
            //foreach (JObject o in oo.Children<JObject>())
            //{
            //    lip.Text += ("obj1 : " + o.ToString());
            //}

//使用JObject取值
//lip.Text +=("obj1 : " + oo.ToString());
//lip.Text +=("child1 : " + oo["eye"].ToString());
//lip.Text +=("array1 : " + oo["time"].ToString());

        //JObject o1 = JObject.Parse(File.ReadAllText(Server.MapPath("~/json/-46-export.json")));

        //JArray a = JArray.Parse(o1.ToString());
        //lip.Text += a.ToString();

        //foreach (JObject o in a.Children<JObject>())
        //{
        //    foreach (JProperty p in o.Properties())
        //    {
        //        string name = p.Name;
        //        string value = (string)p.Value;
        //        Console.WriteLine(name + " -- " + value);
        //    }
        //}
        //lip.Text = o1.ToString();
        mypan.Controls.Add(lip);
//        //// read JSON directly from a file
//        //using (StreamReader file = File.OpenText(@"c:\videogames.json"))
//        //using (JsonTextReader reader = new JsonTextReader(file))
//        //{
//        //    JObject o2 = (JObject)JToken.ReadFrom(reader);
//        //}

//        gc = new GCP_MYSQL();
//        Panel mypan = (Panel)this.FindControl("main_Panel");
//        Literal lip = new Literal();
//        Query = "select * from nursing_room where id=46;";
//        DataView ict_ff = gc.select_cmd(Query);
//        lip.Text += @"min:<br>
//  <input type='text' id='minv' value='0'><br>
//  max:<br>
//  <input type='text' id='maxv' value='40'><br>";
//        lip.Text += @"<hr>";
//        if (ict_ff.Count > 0)
//        {
//            for (int i = 0; i < ict_ff.Count; i++)
//            {
//                //lip.Text += @"<div id=" + '"' + @"chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @"><svg width=" + '"' + @"90px" + '"' + @" height=" + '"' + @"90px" + '"' + @" xmlns=" + '"' + @"http://www.w3.org/2000/svg" + '"' + @" viewBox=" + '"' + @"0 0 100 100" + '"' + @" preserveAspectRatio=" + '"' + @"xMidYMid" + '"' + @" class=" + '"' + @"lds-rolling" + '"' + @" style=" + '"' + @"background: none;" + '"' + @"><circle cx=" + '"' + @"50" + '"' + @" cy=" + '"' + @"50" + '"' + @" fill=" + '"' + @"none" + '"' + @" ng-attr-stroke=" + '"' + @"{{config.color}}" + '"' + @" ng-attr-stroke-width=" + '"' + @"{{config.width}}" + '"' + @" ng-attr-r=" + '"' + @"{{config.radius}}" + '"' + @" ng-attr-stroke-dasharray=" + '"' + @"{{config.dasharray}}" + '"' + @" stroke=" + '"' + @"#6ac1a5" + '"' + @" stroke-width=" + '"' + @"9" + '"' + @" r=" + '"' + @"33" + '"' + @" stroke-dasharray=" + '"' + @"155.50883635269477 53.83627878423159" + '"' + @" transform=" + '"' + @"rotate(324 50 50)" + '"' + @"><animateTransform attributeName=" + '"' + @"transform" + '"' + @" type=" + '"' + @"rotate" + '"' + @" calcMode=" + '"' + @"linear" + '"' + @" values=" + '"' + @"0 50 50;360 50 50" + '"' + @" keyTimes=" + '"' + @"0;1" + '"' + @" dur=" + '"' + @"1s" + '"' + @" begin=" + '"' + @"0s" + '"' + @" repeatCount=" + '"' + @"indefinite" + '"' + @"></animateTransform></circle></svg></div>";
//                lip.Text += @"<div id=" + '"' + @"chart-container__" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @">" + ict_ff.Table.Rows[i]["name"].ToString() + "</div>";
//                lip.Text += @"<div id=" + '"' + @"chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @"></div>";
//                lip.Text += @"<canvas id=" + '"' + @"chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @" width='400px' height='400px'></canvas>";
//                lip.Text += @"<br id=" + '"' + @"brdiv" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @">";
//                lip.Text += @"<input type='range' id=" + '"' + @"slider" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @" min='0' max='10' step='1' value='5'>";
//                lip.Text += @"<div id=" + '"' + @"sliderva" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @"></div>";

//                lip.Text += @"<hr id=" + '"' + @"hrdiv" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @">";
//            }
//        }
//        lip.Text += "<style type=" + '"' + @"text/css" + '"' + @">";
//        lip.Text += @".body {
//                        text-align: center;
//                    }";

//        lip.Text += "</style>";
//        lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
//        lip.Text += "<script src=" + '"' + @"https://static.fusioncharts.com/code/latest/fusioncharts.js" + '"' + @"></script>";
//        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-app.js" + '"' + @"></script>";
//        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-database.js" + '"' + @"></script>";
//        lip.Text += @" <script>
// var firebase;
//        var config = {
//            databaseURL: " + '"' + @"" + '"' + @"
//        };
//        firebase.initializeApp(config);";
//        if (ict_ff.Count > 0)
//        {
//            for (int i = 0; i < ict_ff.Count; i++)
//            {
//                lip.Text += @"
//
//        function getData" + ict_ff.Table.Rows[i]["id"].ToString() + @"(callbackIN) {
//
//        var ref = firebase.database().ref('/grid_eye/" + ict_ff.Table.Rows[i]["id"].ToString() + @"/').limitToLast(1000);
//        ref.once('value').then(function (snapshot) {
//            var cdata = [];
//            for (var i in snapshot.val()) {
//                cdata.push({
//                    label: snapshot.val()[i].time,
//                    value: snapshot.val()[i].eye
//                });
//            }
//
//            callbackIN(cdata)
//
//        });
//        ref.on('value', function (snapshot) {
//            var cdata = [];
//            for (var i in snapshot.val()) {
//                cdata.push({
//                    label: snapshot.val()[i].time,
//                    value: snapshot.val()[i].eye
//                });
//            }
//
//            callbackIN(cdata)
//        });
//    }
//   var cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @" = [];
//var slide" + ict_ff.Table.Rows[i]["id"].ToString() + @"=document.getElementById('slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//
//slide" + ict_ff.Table.Rows[i]["id"].ToString() + @".onchange=function(){
//$(function(){
//$('#sliderva" + ict_ff.Table.Rows[i]["id"].ToString() + @"').text($('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').val());
//$('#chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + @"').text(cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @"[$('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').val()]['label']);
//var minv = parseFloat($('#minv').val());
//var maxv = parseFloat($('#maxv').val());
//
//var canvasElement = $('#chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//var context = canvasElement[0].getContext('2d');
//
//var str=cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @"[$('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').val()]['value'].split(' ');
//
//var index=0;
//
//// Filled
//for (var y = 0; y < 400; y += 50) {
// for (var x = 0; x < 400; x += 50) {
//
//var v1 = parseFloat(str[index]);
//if(isNaN(v1)==false){
//
//
//var cc=Math.round(360*((v1-minv)/(maxv-minv)));
//
//
//
//        context.fillStyle = 'hsl('+cc+', 100%, 50%)';
//        context.fillRect(x, y, 50, 50);
//}
//index+=1;
//
//
//    }
//}
//});
//}
//
//    window.addEventListener(" + '"' + @"load" + '"' + @", getData" + ict_ff.Table.Rows[i]["id"].ToString() + @"(genFunction" + ict_ff.Table.Rows[i]["id"].ToString() + @"));
//    function genFunction" + ict_ff.Table.Rows[i]["id"].ToString() + @"(data) {
//      cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @" = [];
//      var len = data.length;
//      for (var i = 1; i < len; i++) {
//        cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @".push({
//          label: data[i]['label'],
//          value: data[i]['value']
//        });
//      }
//if(len==0){
//$(function(){
//$show = $('#chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show.hide();
//$show = $('#chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show.hide();
//$show = $('#chart-container__" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show.hide();
//
//$show = $('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show.hide();
//$show = $('#sliderva" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show.hide();
//
//$show1 = $('#brdiv" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show1.hide();
//$show1 = $('#hrdiv" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//$show1.hide();
//});
//}else{
//$(function(){
//$('#chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + @"').text(cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @"[cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @".length-1]['label']);
//var canvasElement = $('#chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
//var context = canvasElement[0].getContext('2d');
//
//$('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').prop({'min':0,'max':cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @".length-1});
//$('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').val(cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @".length-1);
//$('#sliderva" + ict_ff.Table.Rows[i]["id"].ToString() + @"').text($('#slider" + ict_ff.Table.Rows[i]["id"].ToString() + @"').val());
//
//var minv = parseFloat($('#minv').val());
//var maxv = parseFloat($('#maxv').val());
//
//var str=cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @"[cdata" + ict_ff.Table.Rows[i]["id"].ToString() + @".length-1]['value'].split(' ');
//
//var index=0;
//
//// Filled
//for (var y = 0; y < 400; y += 50) {
// for (var x = 0; x < 400; x += 50) {
//
//var v1 = parseFloat(str[index]);
//if(isNaN(v1)==false){
//
//
//var cc=Math.round(360*((v1-minv)/(maxv-minv)));
//
//
//
//        context.fillStyle = 'hsl('+cc+', 100%, 50%)';
//        context.fillRect(x, y, 50, 50);
//}
//index+=1;
//
//
//    }
//}
//});
//
//}
//    }";

//            }
//        }
//        lip.Text += @"</script>";
//        mypan.Controls.Add(lip);


    }
}
