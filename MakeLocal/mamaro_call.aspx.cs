using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_call : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
    string room_id = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        if (this.Request.QueryString["mamarocall"] != null)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamarocall"]) ? Request.QueryString["mamarocall"] : Guid.Empty.ToString();
            if (activationCode != "")
            {
                Session["tmp_call_id"] = activationCode;

                Response.Redirect("mamaro_call.aspx");
            }
        }
        if (Session["tmp_call_id"] != null)
        {
            if (Session["tmp_call_id"].ToString() != "")
            {
                ////
                string QR = RemoveSpecialCharacters(Session["tmp_call_id"].ToString());
                GCP_MYSQL gc = new GCP_MYSQL();
                Literal lip = new Literal();

                Literal lip_call = new Literal();
                string Query = "";
                Query = "select a.id,a.name,a.close,b.id as pid from nursing_room as a inner join nursing_room_phone as b ";
                Query += "on a.id=b.nursing_room_id and b.code='" + QR + "';";
                DataView ict_ff = gc.select_cmd(Query);
                if (ict_ff.Count > 0)
                {
                    if (ict_ff.Table.Rows[0]["close"].ToString() == "0")
                    {

                        GCP_MYSQL gc1 = new GCP_MYSQL();
                        string Query1 = "";
                        Query1 = "update nursing_room_phone set get_state='1',get_datetime=NOW() where id='" + ict_ff.Table.Rows[0]["pid"].ToString() + "';";
                        string rescmd = gc1.update_cmd(Query1);


                        DateTime nowdate = DateTime.Now;
                        DateTime utc = nowdate.ToUniversalTime();
                        TimeZoneInfo jst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                        DateTime now_jst = TimeZoneInfo.ConvertTimeFromUtc(utc, jst);


                        lip.Text += "<br><div class='center' id=" + '"' + "m_" + '"' + ">" + ict_ff.Table.Rows[0]["name"].ToString() + @"<br><br>
                              <img id=" + '"' + "img_" + '"' + @" src='https://storage.googleapis.com//upload/nursing_room/mamaro_off.png' alt='" + ict_ff.Table.Rows[0]["name"].ToString() + @"' style='width:100px;height:100px;'><br>
<img id='cutoff' src='./images/call_img/hotline.gif' alt='" + ict_ff.Table.Rows[0]["name"].ToString() + @"' style='width:300px;height:200px;'>";



                        lip.Text += @"<iframe id='content_if' allow='camera; microphone' src='https://appear.in/" + QR + @"' width='1' height='1' frameborder='0'></iframe>";

                        lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
                        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-app.js" + '"' + @"></script>";
                        lip.Text += "<script src=" + '"' + @"https://www.gstatic.com/firebasejs/4.6.2/firebase-database.js" + '"' + @"></script>";
                        lip.Text += @"<script type=" + '"' + "text/javascript" + '"' + @">
var firebase;
        var config = {
            databaseURL: " + '"' + @"url" + '"' + @"
        };
        firebase.initializeApp(config);


function writecallData() {
  var myRef = firebase.database().ref('/mamarocall/get/" + ict_ff.Table.Rows[0]["id"].ToString() + @"').push();
var now = new Date();

  var newData={
      mamaro_id: '" + ict_ff.Table.Rows[0]["id"].ToString() + @"',
      QRcode: '" + QR + @"',
      state: '1',
      time : '" + now_jst.ToString("yyyy-MM-dd HH:mm:ss") + @"'
   }

   myRef.push(newData);

}
writecallData();

function writecallData1() {
  var myRef = firebase.database().ref('/mamarocall/get/" + ict_ff.Table.Rows[0]["id"].ToString() + @"').push();
var now = new Date();

  var newData={
      mamaro_id: '" + ict_ff.Table.Rows[0]["id"].ToString() + @"',
      QRcode: '" + QR + @"',
      state: '3',
      time : '" + now_jst.ToString("yyyy-MM-dd HH:mm:ss") + @"'
   }

   myRef.push(newData);

}

var ref = firebase.database().ref('/mamarocall/send/" + ict_ff.Table.Rows[0]["id"].ToString() + @"/').limitToLast(10);

        ref.on('value', function (snapshot) {


            var cdata = [];
            for (var i in snapshot.val()) {
                cdata.push({
                    label: snapshot.val()[i].time,
                    value: snapshot.val()[i].state
                });
            }




            var len = cdata.length;

var checkmama=1;
       $(function(){
            $show1 = $('#img_');
            $show1.attr('src','https://storage.googleapis.com//upload/nursing_room/mamaro_off.png');
        });

for (var i = 0; i < len; i++) {
      var chec=1;
        if(cdata[i].value=='0')
        {
            if(i+1<len)
            {
                if(cdata[i+1].value=='2')
                {
                    chec=0;
                }else{
                    chec=0;
                }
            }
            if(chec>0)
            {
                 $(function(){
            $show1 = $('#img_');
            $show1.attr('src','https://storage.googleapis.com//upload/nursing_room/mamaro_on.png');
        });
            }
}
}
});

$(function(){
            $('#cutoff').click(function () {
                loadIframe('content_if', 'http://www.trim-inc.com/');
                writecallData1();
                 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_call.aspx/send_message" + '"' + @",
                     data: " + '"' + @"{param1: '" + ict_ff.Table.Rows[0]["pid"].ToString() + @"'  }" + '"' + @",
                     contentType: " + '"' + @"application/json; charset=utf-8" + '"' + @",
                     dataType: " + '"' + @"json" + '"' + @",
                     async: true,
                     cache: false,
                     success: function (result) {
                        if (result.d.length > 0){

                            alert(result.d);
                            }
                            else{
                              alert('error');
                            }

                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });

            });
            function loadIframe(iframeName, url) {
                var $iframe = $('#' + iframeName);
                if ($iframe.length) {
                    $iframe.attr('src', url);
                    return false;
                }
                return true;
            }
        });


</script>
";


                        lip.Text += @"</div>";
                    }

                }
                mainPanel.Controls.Add(lip);
                ////

            }
        }


    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string send_message(string param1)
    {
        //string result = param1 + "," + param2;
        string result = "";
        string id = RemoveSpecialCharacters(param1);
        try
        {
            int pid = Convert.ToInt32(id);
            GCP_MYSQL gc1 = new GCP_MYSQL();
            string Query1 = "";
            Query1 = "update nursing_room_phone set get_state='3',get_cut_datetime=NOW() where id='" + pid + "';";
            result = gc1.update_cmd(Query1);

        }catch (Exception ex)
        {
            result = "fail";

            //return result;
            throw ex;
        }
        return result;

    }
}
