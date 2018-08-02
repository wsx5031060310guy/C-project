using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_cam : System.Web.UI.Page
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
        lip.Text += @"<hr>";
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                //lip.Text += @"<div id=" + '"' + @"chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @"><svg width=" + '"' + @"90px" + '"' + @" height=" + '"' + @"90px" + '"' + @" xmlns=" + '"' + @"http://www.w3.org/2000/svg" + '"' + @" viewBox=" + '"' + @"0 0 100 100" + '"' + @" preserveAspectRatio=" + '"' + @"xMidYMid" + '"' + @" class=" + '"' + @"lds-rolling" + '"' + @" style=" + '"' + @"background: none;" + '"' + @"><circle cx=" + '"' + @"50" + '"' + @" cy=" + '"' + @"50" + '"' + @" fill=" + '"' + @"none" + '"' + @" ng-attr-stroke=" + '"' + @"{{config.color}}" + '"' + @" ng-attr-stroke-width=" + '"' + @"{{config.width}}" + '"' + @" ng-attr-r=" + '"' + @"{{config.radius}}" + '"' + @" ng-attr-stroke-dasharray=" + '"' + @"{{config.dasharray}}" + '"' + @" stroke=" + '"' + @"#6ac1a5" + '"' + @" stroke-width=" + '"' + @"9" + '"' + @" r=" + '"' + @"33" + '"' + @" stroke-dasharray=" + '"' + @"155.50883635269477 53.83627878423159" + '"' + @" transform=" + '"' + @"rotate(324 50 50)" + '"' + @"><animateTransform attributeName=" + '"' + @"transform" + '"' + @" type=" + '"' + @"rotate" + '"' + @" calcMode=" + '"' + @"linear" + '"' + @" values=" + '"' + @"0 50 50;360 50 50" + '"' + @" keyTimes=" + '"' + @"0;1" + '"' + @" dur=" + '"' + @"1s" + '"' + @" begin=" + '"' + @"0s" + '"' + @" repeatCount=" + '"' + @"indefinite" + '"' + @"></animateTransform></circle></svg></div>";
                lip.Text += @"<div id=" + '"' + @"chart-container__" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @">" + ict_ff.Table.Rows[i]["name"].ToString() + "</div>";
                lip.Text += @"<div id=" + '"' + @"chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @"></div>";
                lip.Text += @"<img id=" + '"' + @"chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @" width='400px' height='400px'/>";

                lip.Text += @"<hr id=" + '"' + @"hrdiv" + ict_ff.Table.Rows[i]["id"].ToString() + '"' + @">";
            }
        }
        lip.Text += "<style type=" + '"' + @"text/css" + '"' + @">";
        lip.Text += @".body {
                        text-align: center;
                    }";

        lip.Text += "</style>";
        lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"https://static.fusioncharts.com/code/latest/fusioncharts.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-app.js" + '"' + @"></script>";
        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-database.js" + '"' + @"></script>";
        lip.Text += @" <script>
 var firebase;
        var config = {
            databaseURL: " + '"' + @"" + '"' + @"
        };
        firebase.initializeApp(config);";
        if (ict_ff.Count > 0)
        {
            for (int i = 0; i < ict_ff.Count; i++)
            {
                lip.Text += @"

        function getData" + ict_ff.Table.Rows[i]["id"].ToString() + @"(callbackIN) {

        var ref = firebase.database().ref('/camera/" + ict_ff.Table.Rows[i]["id"].ToString() + @"/').limitToLast(10);
        ref.once('value').then(function (snapshot) {
            var cdata = [];
            for (var i in snapshot.val()) {
                cdata.push({
                    label: snapshot.val()[i].time,
                    value: snapshot.val()[i].imgurl
                });
            }

            callbackIN(cdata)

        });
        ref.on('value', function (snapshot) {
            var cdata = [];
            for (var i in snapshot.val()) {
                cdata.push({
                    label: snapshot.val()[i].time,
                    value: snapshot.val()[i].imgurl
                });
            }

            callbackIN(cdata)
        });
    }

    window.addEventListener(" + '"' + @"load" + '"' + @", getData" + ict_ff.Table.Rows[i]["id"].ToString() + @"(genFunction" + ict_ff.Table.Rows[i]["id"].ToString() + @"));
    function genFunction" + ict_ff.Table.Rows[i]["id"].ToString() + @"(data) {
      var cdata = [];
      var len = data.length;
      for (var i = 1; i < len; i++) {
        cdata.push({
          label: data[i]['label'],
          value: data[i]['value']
        });
      }
if(len==0){
$(function(){
$show = $('#chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show.hide();
$show = $('#chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show.hide();
$show = $('#chart-container__" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show.hide();
$show1 = $('#hrdiv" + ict_ff.Table.Rows[i]["id"].ToString() + @"');
$show1.hide();
});
}else{
$(function(){
$('#chart-container_" + ict_ff.Table.Rows[i]["id"].ToString() + @"').text(cdata[cdata.length-1]['label']);
$('#chart-container" + ict_ff.Table.Rows[i]["id"].ToString() + @"').attr('src',cdata[cdata.length-1]['value']);


});

}
    }";

            }
        }
        lip.Text += @"</script>";
        mypan.Controls.Add(lip);


    }
}
