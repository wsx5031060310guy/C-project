using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using MySql.Data.MySqlClient;

public partial class main_guest_light : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='home.aspx';");
        Label_logo.Style["cursor"] = "pointer";

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            Session["id"] = "";
        }
        string activationCode = !string.IsNullOrEmpty(Request.QueryString[""]) ? Request.QueryString[""] : Guid.Empty.ToString();
        string check_postal_code=activationCode.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        if (Session["postcode"] == null)
        {
            if (check_postal_code == null)
            {
                Session.Clear();
                Response.Redirect("home.aspx");
            }
            else if (check_postal_code == "")
            {
                Session.Clear();
                Response.Redirect("home.aspx");
            }
            else
            {
                Session["postcode"] = check_postal_code;
                Response.Redirect("main_guest_light.aspx");
            }
        }
        else if (Session["postcode"].ToString() == "")
        {
            Session.Clear();
            Response.Redirect("home.aspx");
        }
        post_message_panel.Visible = false;
        Session["top_count"] = 10;

        ///////
        Panel pdn_j = (Panel)this.FindControl("javaplace_formap");
        pdn_j.Controls.Clear();

        List<string> postal_code_list = new List<string>();

        int couuu = 0;
        Literal lip = new Literal();
        Literal lip1 = new Literal();
        Literal lip2 = new Literal();

            lip1.Text = "";
            lip2.Text = "";
            lip2.Text += @"var bounds = new google.maps.LatLngBounds();";
            List<string> check_pos = new List<string>();
            postal_code_list.Add(Session["postcode"].ToString());

                string result = "";
                try
                {
                    var url = new Uri("https://postcode.teraren.com/postcodes/" + HttpContext.Current.Server.UrlEncode(Session["postcode"].ToString().Replace("-", "")) + ".json");


                    System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    using (var response = request.GetResponse())
                    using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                    //TextBox1.Text = result;


                    JObject jArray = JObject.Parse(result);
                    string jis = (string)jArray["jis"];
                    bool chc_same = true;
                    if (check_pos.Count > 0)
                    {
                        for (int io = 0; io < check_pos.Count; io++)
                        {
                            if (jis == check_pos[io])
                            {
                                chc_same = false;
                            }
                        }

                    }
                    else
                    {
                        check_pos.Add(jis);
                    }
                    if (chc_same)
                    {
                        check_pos.Add(jis);
                        //string state = (string)jArray["prefecture"];
                        //string city = (string)jArray["city"];
                        //string town = (string)jArray["suburb"];
                        Query = "select c.con_id";
                        Query += " from JP_City as b use index (IX_JP_City)";
                        Query += " inner join JP_City_G as c use index (IX_JP_City_G_1) on b.jis=c.jis";
                        Query += " where b.jis='" + jis + "';";
                        DataView ict2 = gc.select_cmd(Query);
                        if (ict2.Count > 0)
                        {
                            for (int ii = 0; ii < ict2.Count; ii++)
                            {
                                //
                                Query = "select e.lat,e.lng";
                                Query += " from JP_City_G as c";
                                Query += " inner join JP_City_Town_G as d use index (IX_JP_City_Town_G_1) on c.con_id=d.City_id";
                                Query += " inner join test_GPS as e use index (IX_test_GPS) on d.place_id=e.place_id";
                                Query += " where c.con_id='" + ict2.Table.Rows[ii]["con_id"].ToString() + "';";
                                DataView ict1 = gc.select_cmd(Query);
                                if (ict1.Count > 0)
                                {
                                    //                               lip2.Text += @"var bounds" + couuu + @" = new google.maps.LatLngBounds();
                                    //bounds" + couuu + @".extend(center);";
                                    lip1.Text += @"var triangleCoords" + couuu + @" = [
";
                                    string str = "";
                                    for (int ix = 0; ix < ict1.Count; ix++)
                                    {
                                        str += "{ lat: " + ict1.Table.Rows[ix]["lat"].ToString() + ", lng: " + ict1.Table.Rows[ix]["lng"].ToString() + " },";
                                        lip2.Text += @"bounds.extend(new google.maps.LatLng(" + ict1.Table.Rows[ix]["lat"].ToString() + ", " + ict1.Table.Rows[ix]["lng"].ToString() + "));";
                                    }
                                    if (str.Length > 0)
                                    {
                                        str.Substring(0, str.Length - 1);
                                    }
                                    //lip2.Text += @"map1.fitBounds(bounds" + couuu + @");";
                                    lip1.Text += str;
                                    lip1.Text += @"];

            // Construct the polygon.
            var bermudaTriangle" + couuu + @" = new google.maps.Polygon({
                paths: triangleCoords" + couuu + @",
                strokeColor: '#FF0000',
                strokeOpacity: 0.20,
                strokeWeight: 1,
                fillColor: '#FF0000',
                fillOpacity: 0.05
            });
            bermudaTriangle" + couuu + @".setMap(map1);
";

                                    couuu += 1;



                                }
                                //
                            }
                        }
                    }



                    //TextBox1.Text = jis ;



                    //////////
                    for (int ji = 0; ji < check_pos.Count; ji++)
                   {
                       Query = "select zipcode";
                       Query += " from zipcode_f_01 use index (IX_zipcode_f_01)";
                       Query += " where pref_jis like '" + check_pos[ji] + "%';";
                       DataView ict_jis = gc.select_cmd(Query);
                       if (ict_jis.Count > 0)
                       {
                           for (int ii = 0; ii < ict_jis.Count; ii++)
                           {
                               string zip_for = ict_jis.Table.Rows[ii]["zipcode"].ToString().Substring(0, 3) + "-" + ict_jis.Table.Rows[ii]["zipcode"].ToString().Substring(3, 4);
                               postal_code_list.Add(zip_for);
                           }
                       }
                   }

//////////////////
                    if (Session["message_type"] != null)
                    {
                        if (Session["message_type"].ToString() != "")
                        {
                            if (Session["message_type"].ToString() == "1")
                            {
                                Query = "select count(good_status) as likecount,smid";
                                Query += " from status_messages_user_like";
                                Query += " group by smid;";

                                DataView ict_likecount = gc.select_cmd(Query);
                                if (ict_likecount.Count > 0)
                                {
                                    for (int ilike = 0; ilike < ict_likecount.Count; ilike++)
                                    {
                                        Query = "select id";
                                        Query += " from status_messages";
                                        Query += " where id='" + ict_likecount.Table.Rows[ilike]["smid"].ToString() + "';";

                                        DataView ict_likecount_1 = gc.select_cmd(Query);
                                        if (ict_likecount_1.Count > 0)
                                        {
                                            Query = "update status_messages set likecount='" + ict_likecount.Table.Rows[ilike]["likecount"].ToString() + "' where id='" + ict_likecount.Table.Rows[ilike]["smid"].ToString() + "';";
                                            resin = gc.update_cmd(Query);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    bool check_sort_pop = false;
                    int totalran = Convert.ToInt32(Session["top_count"].ToString());

                    /////
                    Query = "select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
                    Query += "from status_messages as a use index (IX_status_messages_1)";
                    Query += " inner join user_login as b on b.id=a.uid where 1=1 and a.type='0'";
                    if (Session["message_type"] != null)
                    {
                        if (Session["message_type"].ToString() != "")
                        {
                            if (Session["message_type"].ToString() == "1")
                            {
                                check_sort_pop = true;
                            }
                            else
                            {
                                Query += " and a.message_type='" + Session["message_type"].ToString() + "'";
                            }
                        }
                    }
                    if (postal_code_list.Count > 0)
                    {
                        string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                        for (int i = 1; i < postal_code_list.Count; i++)
                        {
                            qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                        }
                        //qustr += " or a.postal_code=''";
                        //addstr += " or a.postal_code=''";
                        qustr += ")";
                        Query += qustr;
                    }
                    if (check_sort_pop)
                    {
                        Query += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
                    }
                    else
                    {
                        Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
                    }

                    DataView ict_place = gc.select_cmd(Query);
                    ////count max post
                    Query = "select a.id ";
                    Query += "from status_messages as a use index (IX_status_messages_1)";
                    Query += " inner join user_login as b on b.id=a.uid where 1=1 and a.type='0'";
                    if (Session["message_type"] != null)
                    {
                        if (Session["message_type"].ToString() != "")
                        {
                            if (Session["message_type"].ToString() == "1")
                            {
                                check_sort_pop = true;
                            }
                            else
                            {
                                Query += " and a.message_type='" + Session["message_type"].ToString() + "'";
                            }
                        }
                    }
                    if (postal_code_list.Count > 0)
                    {
                        string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                        for (int i = 1; i < postal_code_list.Count; i++)
                        {
                            qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                        }
                        //qustr += " or a.postal_code=''";
                        //addstr += " or a.postal_code=''";
                        qustr += ")";
                        Query += qustr;
                    }
                    if (check_sort_pop)
                    {
                        Query += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    }
                    else
                    {
                        Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    }

                    DataView ict_place_max = gc.select_cmd(Query);
                    Session["max_count_post"] = ict_place_max.Count.ToString();
                    ////count max post

                    if (ict_place.Count > 0)
                    {
                        HashSet<sortstate> bands = new HashSet<sortstate>(new sortstateComparer());
                        for (int i = 0; i < ict_place.Count; i++)
                        {
                            bands.Add(new sortstate { id = i, latlng = ict_place.Table.Rows[i]["place_lat"].ToString() + "," + ict_place.Table.Rows[i]["place_lng"].ToString() });
                        }
                        List<sortstate> sorst_list1 = bands.ToList<sortstate>();


                        lip.Text = @"<script>

var locations = [
                    ";
                        string llllstr = "";
                        string flat = "", flng = "";
                        string content = "";
                        for (int i = 0; i < sorst_list1.Count; i++)
                        {
                            string icon_type = "'images/map_pin.png'";

                            string cutstr2 = ict_place.Table.Rows[sorst_list1[i].id]["photo"].ToString();
                            int ind2 = cutstr2.IndexOf(@"/");
                            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            string metype = "";
                            if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 0)
                            {
                                icon_type = "'images/map_icon/food.png'";
                                metype = "お食事、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 1)
                            {
                                metype = "人気スポット、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 2)
                            {
                                icon_type = "'images/map_icon/event.png'";
                                metype = "イベント、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 3)
                            {
                                icon_type = "'images/map_icon/hosp.png'";
                                metype = "病院、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 4)
                            {
                                icon_type = "'images/map_icon/park.png'";
                                metype = "公園／レジャー、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 5)
                            {
                                icon_type = "'images/map_icon/milk.png'";
                                metype = "、";
                            }
                            else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 6)
                            {
                                metype = "指定なし、";
                            }
                            string mess = "";
                            if (ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString().Length < 37)
                            {
                                mess = ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString();
                            }
                            else
                            {
                                mess = ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString().Substring(0, 37) + "‧‧‧";
                            }


                            content = '"' + @"<div><table width='100%'><tr><td width='20%' valign='top'><img src='" + cutstr3 + @"' height='50px' width='50px'></td><td width='80%' valign='top' style='word-wrap: break-word;'><a href='javascript:void(0);' style='text-decoration: none;'>" + ict_place.Table.Rows[sorst_list1[i].id]["username"].ToString() + @"</a><br/><span style='color:#CCCCCC;'>" + ict_place.Table.Rows[sorst_list1[i].id]["year"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["month"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span><br/><span>" + mess + @"</span><br/></td></tr></table></div>" + '"';
                            //                content = '"'+@"<div><table width='100%'>
                            //<tr>
                            //<td width='20%' valign='top'>
                            //<img src='" + cutstr3 + @"' height='50px' width='50px'>
                            //</td>
                            //<td width='80%' valign='top' style='word-wrap: break-word;'>
                            //<a href='javascript: void(0)' style='text-decoration: none;'>" + ict_place.Table.Rows[i]["username"].ToString() + @"</a>
                            //<br/>
                            //<span style='color:#CCCCCC;'>" + ict_place.Table.Rows[i]["year"].ToString() + "." + ict_place.Table.Rows[i]["month"].ToString() + "." + ict_place.Table.Rows[i]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span>
                            //<br/>
                            //<span>" + mess + @"</span>
                            //<br/>
                            //</td>
                            //</tr>
                            //</table>
                            //</div>" + '"';


                            if (ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() != "" && ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() != "")
                            {
                                flat = ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString();
                                flng = ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString();
                                llllstr += @"[" + content + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() + ", " + sorst_list1[i].id + "," + icon_type + "],";
                                lip2.Text += @"bounds.extend(new google.maps.LatLng(" + ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() + "));";
                            }
                            else
                            {
                                if (ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString().Trim() != "")
                                {
                                    result = "";

                                    url = new Uri("http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString()));

                                    request = (HttpWebRequest)HttpWebRequest.Create(url);
                                    using (var response = request.GetResponse())
                                    using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                                    {
                                        result = sr.ReadToEnd();
                                    }

                                    jArray = JObject.Parse(result);
                                    string lat = (string)jArray["results"][0]["geometry"]["location"]["lat"];
                                    string lng = (string)jArray["results"][0]["geometry"]["location"]["lng"];
                                    llllstr += @"[" + content + "," + lat + ", " + lng + ", " + i + "," + icon_type + "],";
                                    lip2.Text += @"bounds.extend(new google.maps.LatLng(" + lat + ", " + lng + "));";
                                    flat = lat;
                                    flng = lng;
                                    Query = "update status_messages set place_lat='" + lat + "',place_lng='" + lng + "'";
                                    Query += " where id='" + ict_place.Table.Rows[sorst_list1[i].id]["id"].ToString() + "';";
                                    resin = gc.update_cmd(Query);


                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                        }
                        if (llllstr.Length > 0)
                        {
                            llllstr.Substring(0, llllstr.Length - 1);
                            lip.Text += llllstr;
                        }
                        if (flat == "" && flng == "")
                        {
                            flat = "35.447824";
                            flng = "139.6416613";
                        }
                        lip.Text += @"
            ];
var center;
if(locations.length>0)
{
center=new google.maps.LatLng(locations[locations.length-1][1], locations[locations.length-1][2]);
}
var allMarkers = [];
                var map1;
             function initMap1() {
                        map1 = new google.maps.Map(document.getElementById('show_map_area'), {
                            zoom: 14,
                            center: { lat: " + flat + @", lng: " + flng + @" },
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        });";
                        lip.Text += @"
                    var infoWindow1= new google.maps.InfoWindow();

                var marker, i;
var imageUrl = 'images/map_pin.png';
    for (i = 0; i < locations.length; i++) {
      marker = new google.maps.Marker({

        position: new google.maps.LatLng(locations[i][1], locations[i][2]),
        map: map1,
       id:locations[i][3],
        icon: locations[i][4]
      });
allMarkers.push(marker);

Element.prototype.documentOffsetTop = function () {
    return this.offsetTop + ( this.offsetParent ? this.offsetParent.documentOffsetTop() : 0 );
};


      google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
var top = document.getElementById('state_mess_'+locations[i][3]).documentOffsetTop() - (window.innerHeight / 2 );
 $('html, body').animate({
        scrollTop: top
    }, 500);


setTimeout(function(){ document.getElementById('state_mess_'+locations[i][3]).style.backgroundColor = '#FFFFFF'; }, 1000);
for (ii = 0; ii < locations.length; ii++) {
if(ii==i)
{
document.getElementById('state_mess_'+locations[ii][3]).style.backgroundColor = '#FFD0D0';
}else
{
document.getElementById('state_mess_'+locations[ii][3]).style.backgroundColor = '#FFFFFF';
}
}
          infoWindow1.setContent(locations[i][0]);
          infoWindow1.open(map1, marker);
        }
      })(marker, i));
}


";
                        lip.Text += lip1.Text;
                        lip.Text += lip2.Text;
                        lip.Text += @"

        map1.setCenter(bounds.getCenter());

}
window.onload = initMap1;
//MACRO
//Function called when hover the div
function hover(id) {

    for ( var i = 0; i< allMarkers.length; i++) {
        if (id === allMarkers[i].id) {
var icon = {
    url: locations[i][4],
    scaledSize: new google.maps.Size(50, 50),

};


           allMarkers[i].setIcon(icon);

           break;
        }
   }
}
  $(function () {
            $('.lazy').Lazy({
                threshold: 200,
                effect: 'fadeIn',
                visibleOnly: true,
                effect_speed: 'fast',
                onError: function (element) {
                    console.log('error loading ' + element.data('src'));
                }
            });
        });
//Function called when out the div
function out(id) {
    for ( var i = 0; i< allMarkers.length; i++) {
        if (id === allMarkers[i].id) {
var icon = {
    url: locations[i][4],
    scaledSize: new google.maps.Size(30, 30),

};
            allMarkers[i].setIcon(icon);
           break;
        }
   }
}
            </script>
            ";
                        pdn_j.Controls.Add(lip);

                    }

                    /////




                    //////autocomplete



                    //////autocomplete
                    totalran = Convert.ToInt32(Session["top_count"].ToString());
                    Query = " select a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
                    Query += "from status_messages as a use index (IX_status_messages_1)";
                    Query += " inner join user_login as b on b.id=a.uid where 1=1 and a.type='0'";

                    //if want to class by type use type=0,1,2 ; message_type=0,1,2

                    ////Before today's message
                    //sql1.SelectCommand += " where a.year<=" + DateTime.Now.Year.ToString() + " and a.month<=" + DateTime.Now.Month.ToString();
                    //sql1.SelectCommand += " and a.day<=" + DateTime.Now.Day.ToString() + " ";


                    ////type message select
                    if (Session["message_type"] != null)
                    {
                        if (Session["message_type"].ToString() != "")
                        {
                            if (Session["message_type"].ToString() != "1")
                            {
                                Query += " and a.message_type='" + Session["message_type"].ToString() + "'";
                            }
                        }
                    }
                    if (postal_code_list.Count > 0)
                    {
                        string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                        for (int i = 1; i < postal_code_list.Count; i++)
                        {
                            qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                        }
                        //qustr += " or a.postal_code=''";
                        //addstr += " or a.postal_code=''";
                        qustr += ")";
                        Query += qustr;
                    }
                    if (check_sort_pop)
                    {
                        Query += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
                    }
                    else
                    {
                        Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
                    }

                    DataView ict = gc.select_cmd(Query);


                    Literal li = new Literal();

                    li.Text = @"<script type='text/javascript'>

$(function () {

";

                    for (int i = 0; i < ict.Count; i++)
                    {
                        li.Text += @"

$('#btnFileUpload" + i + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar" + i + @"').show();
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar" + i + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').show();
        document.getElementById('make-image" + i + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        console.log('error', error);
    }
});






$('.hidde" + i + @"').toggle(false);

            $('.box" + i + @"').click(function () {
                $('.hidde" + i + @"').toggle();
                $('.box" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').toggle(false);

            $('.likebox" + i + @"').click(function () {
                $('.likehidde" + i + @"').toggle();
                $('.likebox" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').click(function () {
                $('.likebox" + i + @"').toggle();
                $('.likehidde" + i + @"').toggle(false);
            })

            $('.mess_hidde" + i + @"').toggle(false);

            $('.mess_box" + i + @"').click(function () {
                $('.mess_hidde" + i + @"').toggle();
                $('.mess_box" + i + @"').toggle(false);
            })


            $('.big_mess_hidde" + i + @"').toggle(false);

            $('.big_mess_box" + i + @"').click(function () {
                $('.big_mess_hidde" + i + @"').toggle();
                $('.big_mess_box" + i + @"').toggle(false);
                $('.status_message_hidde" + i + @"').toggle();
                $('.status_message_box" + i + @"').toggle(false);
            })

            $('.big_mess_hidde" + i + @"').click(function () {
                $('.big_mess_box" + i + @"').toggle();
                $('.big_mess_hidde" + i + @"').toggle(false);
                $('.status_message_box" + i + @"').toggle();
                $('.status_message_hidde" + i + @"').toggle(false);
            })

            $('.status_message_hidde" + i + @"').toggle(false);


";

//                        SqlDataSource sql3 = new SqlDataSource();
//                        sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                        sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//                        sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                        DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//                        if (ict2.Count > 3)
//                        {
//                            li.Text += @"
//$('.imhidde" + i + @"').toggle(false);
//$('.imhiddee" + i + @"').toggle(false);
//
//            $('.imbox" + i + @"').click(function () {
//                $('.imhidde" + i + @"').toggle();
//                $('.imhiddee" + i + @"').toggle();
//                $('.imbox" + i + @"').toggle(false);
//            })
//
//            $('.imhiddee" + i + @"').click(function () {
//                $('.imbox" + i + @"').toggle();
//                $('.imhidde" + i + @"').toggle(false);
//                $('.imhiddee" + i + @"').toggle(false);
//            })
//
//
//";
//                        }
                    }

                    li.Text += @"
                        });";
                    li.Text += @"</script>";

                    pdn_j = (Panel)this.FindControl("javaplace");
                    //pdn_j.Controls.Clear();
                    pdn_j.Controls.Add(li);

                    //this.Page.Controls.Add(li);


                    //this.Page.Header.Controls.Add(li);
                    ////添加至指定位置
                    //this.Page.Header.Controls.AddAt(0, li);
                    Literal litCss = new Literal();
                    litCss.Text = @"
                <style type='text/css'>
                    #post_message_panel{
                    background-color:#fff;
                    border: thick solid #E9EBEE;
                            }
                 </style>";
                    pdn_j.Controls.Add(litCss);




                    Panel pdn2 = (Panel)this.FindControl("Panel2");
                    pdn2.Controls.Clear();

                    for (int i = 0; i < ict.Count; i++)
                    {
                        pdn2.Controls.Add(new LiteralControl("<div id='state_mess_" + i + "' style='background-color: #FFF;'onmouseover='hover(" + i + ")' onmouseout='out(" + i + ")'>"));

                        pdn2.Controls.Add(new LiteralControl("<table width='100%' style='border: thick solid #E9EBEE;'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td>"));
                        //big message place
                        pdn2.Controls.Add(new LiteralControl("<table width='100%' style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'><br/></td><td width='90%' height='5%'><br/></td><td width='5%' height='5%'><br/></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td>"));
                        //new message place
                        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        //Poster photo
                        pdn2.Controls.Add(new LiteralControl("<td width='10%' rowspan='2' valign='top'>"));

                        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                        string cutstr2 = ict.Table.Rows[i]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:40px;height:40px;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='40' height='40' />"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));



                        //Image img = new Image();
                        //img.Width = 50; img.Height = 50;
                        //img.ImageUrl = ict.Table.Rows[i]["photo"].ToString();
                        //pdn2.Controls.Add(img);


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        //poster username
                        pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));

                        HyperLink hy = new HyperLink();
                        hy.NavigateUrl = "javascript:void(0);";
                        hy.Target = "_blank";
                        hy.Text = ict.Table.Rows[i]["username"].ToString();
                        hy.Font.Underline = false;
                        pdn2.Controls.Add(hy);
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //poster message type and time
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));
                        Label la = new Label();
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        la.Text = "";
                        if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 0)
                        {
                            la.Text += "お食事、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 1)
                        {
                            la.Text += "人気スポット、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 2)
                        {
                            la.Text += "イベント、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 3)
                        {
                            la.Text += "病院、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 4)
                        {
                            la.Text += "公園／レジャー、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 5)
                        {
                            la.Text += "、";
                        }
                        else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 6)
                        {
                            la.Text += "指定なし、";
                        }


                        la.Text += ict.Table.Rows[i]["place"].ToString() + " ";
                        la.Text += ict.Table.Rows[i]["year"].ToString() + "." + ict.Table.Rows[i]["month"].ToString() + "." + ict.Table.Rows[i]["day"].ToString();
                        pdn2.Controls.Add(la);
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //poster message
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td colspan='2' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">"));
                        pdn2.Controls.Add(new LiteralControl("<br/><div class='box" + i + "'>"));
                        HyperLink hyy;
                        if (ict.Table.Rows[i]["message"].ToString().Length < 37)
                        {
                            pdn2.Controls.Add(new LiteralControl(ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString())));
                        }
                        else
                        {
                            pdn2.Controls.Add(new LiteralControl(ict.Table.Rows[i]["message"].ToString().Substring(0, 37) + "‧‧‧"));
                            hyy = new HyperLink();
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = "もっと見る";
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);
                        }


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("<div class='hidde" + i + "'>"));

                        Label la1 = new Label();
                        la1.Style.Add("word-break", "break-all");
                        la1.Style.Add("over-flow", "hidden");
                        la1.Text = ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString());

                        pdn2.Controls.Add(la1);
                        pdn2.Controls.Add(new LiteralControl("<br/>"));


                        //if (ict.Table.Rows[i]["message"].ToString().Length > 36)
                        //{
                        //    hyy = new HyperLink();
                        //    hyy.NavigateUrl = "javascript:void(0);";
                        //    hyy.Target = "_blank";
                        //    hyy.Text = "たたむ";
                        //    hyy.Font.Underline = false;
                        //    pdn2.Controls.Add(hyy);
                        //}


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("<div>"));
                        pdn2.Controls.Add(new LiteralControl("<span style='word-break:break-all;over-flow:hidden;'>" + ConvertUrlsToLinks_DIV(ict.Table.Rows[i]["message"].ToString()) + "</span>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //poster images
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td colspan='2' width='90%' align='center'><br/><br/>"));
                        Query = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
                        Query += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
                        DataView ict1 = gc.select_cmd(Query);
                        Random rand = new Random(Guid.NewGuid().GetHashCode());
                        int typ = Convert.ToInt32(rand.Next(0, ict1.Count));
                        if (ict1.Count > 3)
                        {
                            pdn2.Controls.Add(new LiteralControl("<div class='imbox" + i + "'>"));
                            //for (int ii = 0; ii < 3; ii++)
                            //{
                            //    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                            //    int ind = cutstr.IndexOf(@"/");
                            //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            //    Image imgg = new Image();
                            //    imgg.Width = 100; imgg.Height = 100;
                            //    imgg.ImageUrl = cutstr1;
                            //    pdn2.Controls.Add(imgg);
                            //    pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                            //}
                            //pdn2.Controls.Add(new LiteralControl("<br/>"));
                            //hyy = new HyperLink();
                            //hyy.NavigateUrl = "javascript:void(0);";
                            //hyy.Target = "_blank";
                            //hyy.Text = "もっと見る";
                            //hyy.Font.Underline = false;
                            //pdn2.Controls.Add(hyy);
                            //pdn2.Controls.Add(new LiteralControl("</div>"));
                            //pdn2.Controls.Add(new LiteralControl("<div class='imhidde" + i + "'>"));
                            pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                            pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                            string morefour = "";
                            int countimg = 0;
                            for (int ii = 0; ii < ict1.Count; ii++)
                            {
                                //if (ii > 0 && ii % 3 == 0)
                                //{
                                //    pdn2.Controls.Add(new LiteralControl("<br/>"));
                                //}

                                string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                                int ind = cutstr.IndexOf(@"/");
                                string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                                //block grid
                                if (ii > 3)
                                {
                                    countimg += 1;
                                    pdn2.Controls.Add(new LiteralControl("<div style='visibility:hidden;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:0px; height:0px;outline : none;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' width='0px' height='0px'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));
                                }
                                else
                                {
                                    if (ii == 3)
                                    {

                                        morefour += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                                        morefour += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                                    }
                                    else
                                    {
                                        //Random rand = new Random(Guid.NewGuid().GetHashCode());

                                        //int w = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));
                                        //rand = new Random(Guid.NewGuid().GetHashCode());
                                        //int h = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));

                                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));

                                        //pdn2.Controls.Add(new LiteralControl("<div class='cell' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px; background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));

                                        pdn2.Controls.Add(new LiteralControl("</a>"));
                                        pdn2.Controls.Add(new LiteralControl("</div>"));
                                    }
                                }




                                //Image imgg = new Image();
                                //imgg.Width = 100; imgg.Height = 100;
                                //imgg.ImageUrl = ict1.Table.Rows[ii]["filename"].ToString();
                                //pdn2.Controls.Add(imgg);
                                //pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                            }
                            //countimg
                            if (countimg > 0)
                            {
                                morefour += "<img src='images/test.png' style='background-color: #000; opacity: 0.8; width: 100%; height: 100%; text-align: center;'/>";
                                morefour += "<span style='color: white;position: absolute;top:50%;left:40%;font-size:xx-large;'>+" + countimg + "</span>";
                            }
                            else
                            {
                                morefour += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            }
                            morefour += "</a>";
                            morefour += "</div>";
                            //string blockimg="<div style='position: absolute; background-color: #000; z-index: 999997; opacity: 0.8; width: 100%; height: 100%; text-align: center;'>";
                            pdn2.Controls.Add(new LiteralControl(morefour));
                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            //                    Literal litjs = new Literal();
                            //                    litjs.Text = @"
                            //                                    <script type='text/javascript'>
                            //                                        var wall = new Freewall('#freewall" + i + @"');
                            //                    			wall.reset({
                            //                    				selector: '.cell',
                            //                    				animate: true,
                            //                    				cellW: 100,
                            //                    				cellH: 100,
                            //                    				onResize: function() {
                            //                    					wall.fitWidth();
                            //                    				}
                            //                    			});
                            //                    			wall.fitWidth();
                            //                    $(window).trigger('resize');
                            //                                     </script>";
                            //                    pdn2.Controls.Add(litjs);


                            //
                            Literal litjs = new Literal();
                            litjs.Text = @"
                                    <script type='text/javascript'>
                                        var wall" + i + @" = new Freewall('#freewall" + i + @"');
                    			wall" + i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" + i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                            pdn2.Controls.Add(litjs);


                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            //pdn2.Controls.Add(new LiteralControl("<div class='imhiddee" + i + "'>"));
                            //pdn2.Controls.Add(new LiteralControl("<br/>"));
                            //hyy = new HyperLink();
                            //hyy.NavigateUrl = "javascript:void(0);";
                            //hyy.Target = "_blank";
                            //hyy.Text = "たたむ";
                            //hyy.Font.Underline = false;
                            //pdn2.Controls.Add(hyy);
                            //pdn2.Controls.Add(new LiteralControl("</div>"));
                        }
                        else if (ict1.Count > 0)
                        {
                            string cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            int ind = cutstr.IndexOf(@"/");
                            string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                            if (ict1.Count == 1)
                            {
                                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                                cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr1 + "' src='images/loading.gif' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                            }
                            else if (ict1.Count == 2)
                            {
                                pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                                pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                                if (typ == 0)
                                {
                                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                }
                                else
                                {
                                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));
                                }
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                            }
                            else if (ict1.Count == 3)
                            {
                                pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                                pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                                if (typ == 0)
                                {
                                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                }
                                else if (typ == 1)
                                {
                                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));
                                }
                                else
                                {
                                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                    ind = cutstr.IndexOf(@"/");
                                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));
                                    pdn2.Controls.Add(new LiteralControl("</div>"));
                                }
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));
                            }




                            //pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>"));
                            //pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='100' height='100' />"));
                            //pdn2.Controls.Add(new LiteralControl("</a>"));



                            //pdn2.Controls.Add(new LiteralControl("</div>"));

                            //
                            Literal litjs = new Literal();
                            litjs.Text = @"
                                    <script type='text/javascript'>

                                        var wall" + i + @" = new Freewall('#freewall" + i + @"');
                    			wall" + i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" + i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                            pdn2.Controls.Add(litjs);
                        }

                        string id = "";
                        bool check_li = false;
                        if (Session["id"] != null)
                        {
                            if (Session["id"].ToString() != "")
                            {
                                id = Session["id"].ToString();


                                Query = "select id from status_messages_user_like";
                                Query += " where uid='" + id + "' and smid='" + ict.Table.Rows[i]["id"].ToString() + "';";

                                DataView ict_f_like = gc.select_cmd(Query);
                                if (ict_f_like.Count > 0)
                                {
                                    check_li = true;
                                }
                            }
                        }


                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        //poster message like and share
                        pdn2.Controls.Add(new LiteralControl("<td width='15%' style='white-space: nowrap;' align='right'><br/><br/>"));
                        pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likebox" + i + "'>"));

                        Image img1 = new Image();
                        //if (check_li)
                        //{
                        //    img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                        //    img1.Width = 25; img1.Height = 25;
                        //    img1.ImageUrl = "~/images/like.png";
                        //    img1.Attributes["onclick"] = "like(this.id)";
                        //}
                        //else
                        //{
                        //    img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                        //    img1.Width = 25; img1.Height = 25;
                        //    img1.ImageUrl = "~/images/like_b.png";
                        //    img1.Attributes["onclick"] = "blike(this.id)";
                        //}
                        //pdn2.Controls.Add(img1);
                        //Label laa = new Label();
                        //laa.Font.Size = FontUnit.Point(10);
                        //laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        //laa.Text = "いいね";
                        //pdn2.Controls.Add(laa);


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likehidde" + i + "'>"));
                        //img1 = new Image();
                        //if (check_li)
                        //{

                        //    img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                        //    img1.Width = 25; img1.Height = 25;
                        //    img1.ImageUrl = "~/images/like_b.png";
                        //    img1.Attributes["onclick"] = "blike(this.id)";
                        //}
                        //else
                        //{
                        //    img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                        //    img1.Width = 25; img1.Height = 25;
                        //    img1.ImageUrl = "~/images/like.png";
                        //    img1.Attributes["onclick"] = "like(this.id)";
                        //}
                        //pdn2.Controls.Add(img1);
                        //laa = new Label();
                        //laa.Font.Size = FontUnit.Point(10);
                        //laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
                        //laa.Text = "いいね";
                        //pdn2.Controls.Add(laa);
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));




                        pdn2.Controls.Add(new LiteralControl("<td>"));
                        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));
                        pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='big_mess_box" + i + "'>"));
                        img1 = new Image();
                        img1.Width = 15; img1.Height = 15;
                        img1.ImageUrl = "~/images/mess_b.png";
                        pdn2.Controls.Add(img1);
                        Label laa = new Label();
                        laa.Font.Size = FontUnit.Point(10);
                        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        laa.Text = "コメント";
                        pdn2.Controls.Add(laa);




                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='big_mess_hidde" + i + "'>"));
                        img1 = new Image();
                        img1.Width = 15; img1.Height = 15;
                        img1.ImageUrl = "~/images/mess.png";
                        pdn2.Controls.Add(img1);
                        laa = new Label();
                        laa.Font.Size = FontUnit.Point(10);
                        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#676767");
                        laa.Text = "コメント";
                        pdn2.Controls.Add(laa);


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td align='left'><br/><br/>"));
                        pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "'>"));
                        //pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
                        //img1 = new Image();
                        //img1.Width = 25; img1.Height = 25;
                        //img1.ImageUrl = "~/images/share_b.png";
                        //pdn2.Controls.Add(img1);
                        //laa = new Label();

                        //laa.Font.Size = FontUnit.Point(10);
                        //laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        //laa.Text = "シェア";
                        //pdn2.Controls.Add(laa);

                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        //            li = new Literal();
                        //            li.Text = @"
                        //
                        //                       <script type='text/javascript'>
                        //  $(document).ready(function() {
                        //    Tipped.create('#sharebox" + i + @"'," + '"' + "<div id = 'html-content" + i + @"'><div id='shareforpost_" + ict.Table.Rows[i]["id"].ToString() + @"' style='cursor: pointer;'>そのままジェア (公開) </div><br/><div id='shareforpost_b" + ict.Table.Rows[i]["id"].ToString() + @"' style='cursor: pointer'>コメントとして投稿</div></div>" + '"' + @"
                        //,{
                        //  position: 'bottomleft'
                        //});
                        //  });
                        //</script>
                        //    ";
                        //            pdn2.Controls.Add(li);
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));

                        pdn2.Controls.Add(new LiteralControl("<tr style='background-color:#F6F7F9;'>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td>"));



                        pdn2.Controls.Add(new LiteralControl("<div class='status_message_box" + i + "' style='background-color: #ffffff'>"));
                        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));
                        pdn2.Controls.Add(new LiteralControl("</div >"));
                        pdn2.Controls.Add(new LiteralControl("<div class='status_message_hidde" + i + "' style='background-color: #dddddd'>"));


                        pdn2.Controls.Add(new LiteralControl("<table width='100%' align='left'>"));
                        //first space way
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        //second space way
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        //someone answer
                        pdn2.Controls.Add(new LiteralControl("<td>"));


                        pdn2.Controls.Add(new LiteralControl("<table width='100%' align='left'>"));
                        //who like this message
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='10px'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='90%' height='10px'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='10px'></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='90%'>"));
                        Query = "select b.username,b.id,a.id as smulid from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
                        Query += " where a.smid=" + ict.Table.Rows[i]["id"].ToString() + "";
                        Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";

                        DataView ict3 = gc.select_cmd(Query);
                        if (ict3.Count > 2)
                        {
                            img1 = new Image();
                            img1.Width = 15; img1.Height = 15;
                            img1.ImageUrl = "~/images/like_b_1.png";
                            pdn2.Controls.Add(img1);
                            for (int iii = 0; iii < 2; iii++)
                            {
                                hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.Target = "_blank";
                                hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                                hyy.Font.Underline = false;
                                pdn2.Controls.Add(hyy);
                                pdn2.Controls.Add(new LiteralControl("、"));
                            }
                            hyy = new HyperLink();
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = "他" + (ict3.Count - 2) + "人";
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);

                        }
                        else if (ict3.Count > 0)
                        {
                            img1 = new Image();
                            img1.Width = 15; img1.Height = 15;
                            img1.ImageUrl = "~/images/like_b_1.png";
                            pdn2.Controls.Add(img1);
                            for (int iii = 0; iii < ict3.Count; iii++)
                            {
                                hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.Target = "_blank";
                                hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                                hyy.Font.Underline = false;
                                pdn2.Controls.Add(hyy);
                                if (iii != ict3.Count - 1)
                                {
                                    pdn2.Controls.Add(new LiteralControl("、"));
                                }
                            }
                        }

                        pdn2.Controls.Add(new LiteralControl("<hr/>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //who talk about this status message before
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='95%'>"));



                        Query = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level,c.uid";
                        Query += " from status_messages as a inner join status_messages_user as c";
                        Query += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                        Query += " inner join status_messages_user_talk as e on e.smuid=c.id";
                        Query += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + "";
                        Query += " ORDER BY e.year desc,e.month desc,e.day desc,e.hour desc,e.minute desc,e.second desc;";

                        ict3 = gc.select_cmd(Query);
                        List<sorttalk> talk_list = new List<sorttalk>();
                        sorttalk so = new sorttalk();
                        for (int iy = 0; iy < ict3.Count; iy++)
                        {
                            so = new sorttalk();
                            so.id = Convert.ToInt32(ict3.Table.Rows[iy]["id"].ToString());
                            so.level = Convert.ToInt32(ict3.Table.Rows[iy]["structure_level"].ToString());
                            so.point_id = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_message_id"].ToString());
                            so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_user_id"].ToString());
                            so.filename = ict3.Table.Rows[iy]["filename"].ToString();
                            so.mess = ict3.Table.Rows[iy]["message"].ToString();

                            if (ict3.Table.Rows[iy]["pointer_user_id"].ToString() == "0")
                            {

                                so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["uid"].ToString());
                                so.username = ict3.Table.Rows[iy]["username"].ToString();
                                so.photo = ict3.Table.Rows[iy]["photo"].ToString();
                            }
                            else
                            {

                                Query = "select username,photo from user_login";
                                Query += " where id=" + ict3.Table.Rows[iy]["pointer_user_id"].ToString() + ";";

                                DataView ict5 = gc.select_cmd(Query);
                                so.username = ict5.Table.Rows[0]["username"].ToString();
                                so.photo = ict5.Table.Rows[0]["photo"].ToString();
                            }
                            talk_list.Add(so);
                        }

                        Query = "select max(e.structure_level) as maxlevel";
                        //Query1 = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
                        Query += " from status_messages as a inner join status_messages_user as c";
                        Query += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                        Query += " inner join status_messages_user_talk as e on e.smuid=c.id";
                        Query += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + ";";

                        DataView ict4 = gc.select_cmd(Query);

                        int maxlevel = 0;
                        if (ict4.Table.Rows[0]["maxlevel"].ToString() != "")
                        {
                            maxlevel = Convert.ToInt32(ict4.Table.Rows[0]["maxlevel"].ToString());
                        }

                        List<sorttalk> talk_list_tmp = new List<sorttalk>();
                        so = new sorttalk();
                        for (int ik = 0; ik < talk_list.Count; ik++)
                        {
                            if (talk_list[ik].level == 0)
                            {
                                so = new sorttalk();
                                so.id = talk_list[ik].id;
                                so.level = talk_list[ik].level;
                                so.filename = talk_list[ik].filename;
                                so.mess = talk_list[ik].mess;
                                so.photo = talk_list[ik].photo;
                                so.point_id = talk_list[ik].point_id;
                                so.uid = talk_list[ik].uid;
                                so.username = talk_list[ik].username;
                                talk_list_tmp.Add(so);
                            }
                        }
                        talk_list.Sort((a, b) => a.id.CompareTo(b.id));
                        for (int ik = 0; ik < talk_list.Count; ik++)
                        {
                            for (int le = 1; le < maxlevel + 1; le++)
                            {
                                if (talk_list[ik].level == le)
                                {
                                    so = new sorttalk();
                                    so.id = talk_list[ik].id;
                                    so.level = talk_list[ik].level;
                                    so.filename = talk_list[ik].filename;
                                    so.mess = talk_list[ik].mess;
                                    so.photo = talk_list[ik].photo;
                                    so.point_id = talk_list[ik].point_id;
                                    so.uid = talk_list[ik].uid;
                                    so.username = talk_list[ik].username;
                                    for (int ikk = 0; ikk < talk_list_tmp.Count; ikk++)
                                    {
                                        if (talk_list_tmp[ikk].id == talk_list[ik].point_id)
                                        {
                                            talk_list_tmp.Insert(ikk + 1, so);
                                        }
                                    }
                                }
                            }
                        }
                        Image img2 = new Image();
                        if (ict3.Count > 1)
                        {
                            //show div
                            pdn2.Controls.Add(new LiteralControl("<div class='mess_box" + i + "'>"));
                            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                            pdn2.Controls.Add(new LiteralControl("<tr>"));
                            pdn2.Controls.Add(new LiteralControl("<td width='100%' align='left' colspan='2'>"));

                            hyy = new HyperLink();
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = "以前のコメントを見る";
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);

                            pdn2.Controls.Add(new LiteralControl("</td>"));
                            pdn2.Controls.Add(new LiteralControl("</tr>"));
                            pdn2.Controls.Add(new LiteralControl("<tr>"));
                            pdn2.Controls.Add(new LiteralControl("<td width='10%' rowspan='2' valign='top'>"));


                            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                            cutstr2 = talk_list_tmp[0].photo;
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:32px;height:32px;'>"));
                            pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                            pdn2.Controls.Add(new LiteralControl("</a>"));

                            pdn2.Controls.Add(new LiteralControl("</div>"));


                            pdn2.Controls.Add(new LiteralControl("</td>"));
                            pdn2.Controls.Add(new LiteralControl("<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">"));

                            hyy = new HyperLink();
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = talk_list_tmp[0].username.ToString();
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);
                            pdn2.Controls.Add(new LiteralControl("<br/>"));
                            pdn2.Controls.Add(new LiteralControl(ict3.Table.Rows[0]["message"].ToString()));
                            pdn2.Controls.Add(new LiteralControl("<br/>"));

                            if (talk_list_tmp[0].filename != "")
                            {
                                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                                cutstr2 = talk_list_tmp[0].filename;
                                ind2 = cutstr2.IndexOf(@"/");
                                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));

                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                pdn2.Controls.Add(new LiteralControl("<br/>"));

                            }

                            pdn2.Controls.Add(new LiteralControl("</td>"));
                            pdn2.Controls.Add(new LiteralControl("</tr>"));
                            pdn2.Controls.Add(new LiteralControl("<tr>"));
                            pdn2.Controls.Add(new LiteralControl("<td>"));
                            //who talk about status message and who like

                            pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                            //who like who answer post message
                            Query = "select count(*) as howmany from status_messages_user_talk_like";
                            Query += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                            DataView ict_who_like = gc.select_cmd(Query);
                            if (ict_who_like.Count > 0)
                            {
                                img1 = new Image();
                                img1.Width = 15; img1.Height = 15;
                                img1.ImageUrl = "~/images/like_b_1.png";
                                pdn2.Controls.Add(img1);
                                hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.Target = "_blank";
                                hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                                hyy.Font.Underline = false;
                                pdn2.Controls.Add(hyy);
                            }
                            //who like who answer post message



                            pdn2.Controls.Add(new LiteralControl("</td>"));
                            pdn2.Controls.Add(new LiteralControl("</tr>"));
                            pdn2.Controls.Add(new LiteralControl("</table>"));
                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            //hidde message
                            pdn2.Controls.Add(new LiteralControl("<div class='mess_hidde" + i + "'>"));
                            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                            for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                            {

                                pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                                pdn2.Controls.Add(new LiteralControl("<tr>"));
                                int wid = (10 + (10 * talk_list_tmp[iiii].level));
                                if (wid > 90) { wid = 90; }
                                pdn2.Controls.Add(new LiteralControl("<td width='" + wid + "%' align='right' rowspan='2' valign='top'>"));

                                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                                cutstr2 = talk_list_tmp[iiii].photo;
                                ind2 = cutstr2.IndexOf(@"/");
                                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));

                                pdn2.Controls.Add(new LiteralControl("</div>"));


                                pdn2.Controls.Add(new LiteralControl("</td>"));

                                pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));
                                hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.Target = "_blank";
                                hyy.Text = talk_list_tmp[iiii].username.ToString();
                                hyy.Font.Underline = false;
                                pdn2.Controls.Add(hyy);
                                pdn2.Controls.Add(new LiteralControl("<br/>"));
                                pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].mess.ToString()));
                                pdn2.Controls.Add(new LiteralControl("<br/>"));

                                if (talk_list_tmp[iiii].filename.ToString() != "")
                                {
                                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                                    cutstr2 = talk_list_tmp[iiii].filename.ToString();
                                    ind2 = cutstr2.IndexOf(@"/");
                                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));

                                    pdn2.Controls.Add(new LiteralControl("</div>"));
                                    pdn2.Controls.Add(new LiteralControl("<br/>"));
                                }

                                pdn2.Controls.Add(new LiteralControl("</td>"));
                                pdn2.Controls.Add(new LiteralControl("</tr>"));
                                pdn2.Controls.Add(new LiteralControl("<tr>"));
                                pdn2.Controls.Add(new LiteralControl("<td>"));


                                //who talk about status message and who like

                                pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                                //who like who answer post message
                                Query = "select count(*) as howmany from status_messages_user_talk_like";
                                Query += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                                //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                                ict_who_like = gc.select_cmd(Query);
                                if (ict_who_like.Count > 0)
                                {
                                    img1 = new Image();
                                    img1.Width = 15; img1.Height = 15;
                                    img1.ImageUrl = "~/images/like_b_1.png";
                                    pdn2.Controls.Add(img1);
                                    hyy = new HyperLink();
                                    hyy.ID = "likecount" + talk_list_tmp[iiii].id;
                                    hyy.NavigateUrl = "javascript:void(0);";
                                    hyy.Target = "_blank";
                                    hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                                    hyy.Font.Underline = false;
                                    pdn2.Controls.Add(hyy);
                                }
                                //who like who answer post message

                                pdn2.Controls.Add(new LiteralControl("</td>"));
                                pdn2.Controls.Add(new LiteralControl("</tr>"));


                                pdn2.Controls.Add(new LiteralControl("</table>"));
                                pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));
                            }
                            pdn2.Controls.Add(new LiteralControl("</div>"));

                        }
                        else
                        {
                            if (ict3.Count > 0)
                            {
                                for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                                {

                                    pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                                    int wid = (10 + (10 * talk_list_tmp[iiii].level));
                                    if (wid > 90) { wid = 90; }
                                    pdn2.Controls.Add(new LiteralControl("<td width='" + wid + "%' align='right' rowspan='2' valign='top'>"));


                                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                                    cutstr2 = talk_list_tmp[iiii].photo;
                                    ind2 = cutstr2.IndexOf(@"/");
                                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>"));
                                    pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                                    pdn2.Controls.Add(new LiteralControl("</a>"));

                                    pdn2.Controls.Add(new LiteralControl("</div>"));

                                    pdn2.Controls.Add(new LiteralControl("</td>"));

                                    pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));
                                    hyy = new HyperLink();
                                    hyy.NavigateUrl = "javascript:void(0);";
                                    hyy.Target = "_blank";
                                    hyy.Text = talk_list_tmp[iiii].username.ToString();
                                    hyy.Font.Underline = false;
                                    pdn2.Controls.Add(hyy);
                                    pdn2.Controls.Add(new LiteralControl("<br/>"));
                                    pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].mess.ToString()));
                                    pdn2.Controls.Add(new LiteralControl("<br/>"));

                                    if (talk_list_tmp[iiii].filename.ToString() != "")
                                    {
                                        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                                        cutstr2 = talk_list_tmp[iiii].filename.ToString();
                                        ind2 = cutstr2.IndexOf(@"/");
                                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                                        pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                                        pdn2.Controls.Add(new LiteralControl("</a>"));

                                        pdn2.Controls.Add(new LiteralControl("</div>"));
                                        pdn2.Controls.Add(new LiteralControl("<br/>"));
                                    }

                                    pdn2.Controls.Add(new LiteralControl("</td>"));
                                    pdn2.Controls.Add(new LiteralControl("</tr>"));
                                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                                    pdn2.Controls.Add(new LiteralControl("<td>"));


                                    //who talk about status message and who like

                                    pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                                    //who like who answer post message
                                    Query = "select count(*) as howmany from status_messages_user_talk_like";
                                    Query += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                                    //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                                    DataView ict_who_like = gc.select_cmd(Query);
                                    if (ict_who_like.Count > 0)
                                    {
                                        img1 = new Image();
                                        img1.Width = 15; img1.Height = 15;
                                        img1.ImageUrl = "~/images/like_b_1.png";
                                        pdn2.Controls.Add(img1);
                                        hyy = new HyperLink();
                                        hyy.ID = "likecount" + talk_list_tmp[iiii].id;
                                        hyy.NavigateUrl = "javascript:void(0);";
                                        hyy.Target = "_blank";
                                        hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                                        hyy.Font.Underline = false;
                                        pdn2.Controls.Add(hyy);
                                    }
                                    //who like who answer post message

                                    pdn2.Controls.Add(new LiteralControl("</td>"));
                                    pdn2.Controls.Add(new LiteralControl("</tr>"));


                                    pdn2.Controls.Add(new LiteralControl("</table>"));

                                    //user answer user answer
                                    pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));



                                }
                            }
                        }

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));


                        pdn2.Controls.Add(new LiteralControl("</table>"));


                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        //second space way
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //third space way
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td>"));

                        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));
                        //user photo

                        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                        SqlDataSource sql3 = new SqlDataSource();
                        //sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        //sql3.SelectCommand = "select photo,username from user_login ";
                        //sql3.SelectCommand += " where id='" + Session["id"].ToString() + "';";
                        //DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
                        //string userr = "";
                        //if (ict2.Count > 0)
                        //{
                        //    cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                        //    ind2 = cutstr2.IndexOf(@"/");
                        //    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        //    userr = ict2.Table.Rows[0]["username"].ToString();
                        //}

                        //pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:32px;height:32px;'>"));
                        //pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='32' height='32' />"));
                        //pdn2.Controls.Add(new LiteralControl("</a>"));

                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='85%'>"));

                        //user answer
                        //pdn2.Controls.Add(new LiteralControl("<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_"+i+"' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;'>"));
                        //TextBox tex = new TextBox();
                        //tex.Width = Unit.Percentage(95);
                        //tex.Height = 30;
                        //tex.ID = "send" + ict.Table.Rows[i]["id"].ToString();
                        //tex.Attributes["onKeydown"] = "Javascript: if (event.which == 13 || event.keyCode == 13) sendmessage(this.id);";
                        //tex.Attributes.Add("placeholder", "コメントする");
                        //pdn2.Controls.Add(tex);

                        //pdn2.Controls.Add(new LiteralControl("<br/>"));






                        //pdn2.Controls.Add(new LiteralControl(@"<label class='file-upload'><span><strong>画像を登録</strong></span>"));

                        //FileUpload fi=new FileUpload();
                        //fi.ID="fuDocument_"+i;
                        //fi.Attributes.Add("onchange", "UploadFile(this,this.id);");
                        //pdn2.Controls.Add(fi);
                        //pdn2.Controls.Add(new LiteralControl(@"</label><br />"));



                        //Button but = new Button();
                        //but.ID = "btnUploadDoc_" + i;
                        //but.Text = "Upload";
                        //but.Click += new System.EventHandler(this.UploadDocument);
                        //but.OnClientClick = "ShowProgressBar();";
                        //but.Style["display"] = "none";
                        //pdn2.Controls.Add(but);

                        //img1 = new Image();
                        //img1.Width = 100; img1.Height = 150;
                        //img1.ID = "Image_" + i;
                        //img1.Visible = false;


                        //pdn2.Controls.Add(img1);



                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));


                        pdn2.Controls.Add(new LiteralControl("</div>"));


                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        //fourth space way
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'><br/></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'><br/></td>"));
                        pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'><br/></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("<td></td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));



                        pdn2.Controls.Add(new LiteralControl("</table>"));
                        //pdn2.Controls.Add(new LiteralControl("<br/><br/>"));
                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("</table>"));



                        pdn2.Controls.Add(new LiteralControl("</div>"));
                    }





                    //this.form1.Controls.Add(pdn2);



                    //pdn.Controls.Add(new LiteralControl("<table align='center'>"));
                    //pdn.Controls.Add(new LiteralControl("<tr><td align='center'>"));
                    //pdn.Controls.Add(la);


                    //string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    //string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
                    //string startm = DateTime.Now.Minute.ToString();
                    //string starts = DateTime.Now.Second.ToString();
                    //string d = Convert.ToString(DateTime.Now.ToLocalTime());
                    //string start = startd + " " + starth + ":" + startm + ":" + starts;
                    //string star = starth + ":" + startm + ":" + starts;
                    //Session["student_starttimeh"] = starth;
                    //Session["student_starttimem"] = startm;
                    //Session["student_starttimes"] = starts;


                    //sql1.SelectCommand = "select date,starttime,endtime from fordate where date='" + startd + "' and starttime<'" + start + "' and endtime>'" + start + "';";
                    //DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
                    //this.form1.Controls.Add(sql1);
                    bool check_sup = true;
                    //if (Session["id"] != null)
                    //{

                    //    SqlDataSource sql_check_sup = new SqlDataSource();
                    //    sql_check_sup.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //    sql_check_sup.SelectCommand = "select * from user_information_store where uid='" + Session["id"].ToString() + "';";
                    //    //sql_list.SelectCommand += " where id=" + Session["id"].ToString() + ";";
                    //    DataView ict_check_sup = (DataView)sql_check_sup.Select(DataSourceSelectArguments.Empty);
                    //    if (ict_check_sup.Count > 0)
                    //    {
                    //        check_sup = false;
                    //    }
                    //}
                    if (check_sup)
                    {
                        Button_for_kid.Enabled = true;
                        supporter_list.Visible = false;
                        video_list_Button.Visible = false;
                        supporter_manger.Visible = false;
                    }

                    if (Session["message_type"] == null)
                    {

                        social_Button.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");


                        Button_for_kid.Style.Add("background-color", "transparent");
                        supporter_list.Style.Add("background-color", "transparent");
                        video_list_Button.Style.Add("background-color", "transparent");
                        supporter_manger.Style.Add("background-color", "transparent");
                        Button_for_social.Style.Add("background-color", "transparent");
                        message_type0_Button.Style.Add("background-color", "transparent");
                        message_type1_Button.Style.Add("background-color", "transparent");
                        message_type2_Button.Style.Add("background-color", "transparent");
                        message_type3_Button.Style.Add("background-color", "transparent");
                        message_type4_Button.Style.Add("background-color", "transparent");
                        message_type5_Button.Style.Add("background-color", "transparent");
                    }
                    else
                    {
                        social_Button.Style.Add("background-color", "transparent");
                        Button_for_kid.Style.Add("background-color", "transparent");
                        supporter_list.Style.Add("background-color", "transparent");
                        video_list_Button.Style.Add("background-color", "transparent");
                        supporter_manger.Style.Add("background-color", "transparent");

                        Button but = new Button();
                        for (int i = 0; i <= 5; i++)
                        {
                            if (Session["message_type"].ToString() == i.ToString())
                            {
                                but = (Button)FindControl("message_type" + i + "_Button");
                                but.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");
                                but.Style.Add("color", "#244786");
                            }
                            else
                            {
                                but = (Button)FindControl("message_type" + i + "_Button");
                                but.Style.Add("background-color", "transparent");
                            }
                        }
                        //message_type0_Button.Style.Add("color", "#D0D0D0");
                        //message_type1_Button.Style.Add("color", "#D0D0D0");
                        //message_type2_Button.Style.Add("color", "#D0D0D0");
                        //message_type3_Button.Style.Add("color", "#D0D0D0");
                        //message_type4_Button.Style.Add("color", "#D0D0D0");
                        //message_type5_Button.Style.Add("color", "#D0D0D0");
                    }




                    //        Panel pdn_list = new Panel();
                    //        pdn_list = (Panel)FindControl("Panel_for_support_list");
                    //        pdn_list.Controls.Clear();
                    //        li = new Literal();

                    //        li.Text = @"<script>
                    //$(function () {
                    //";
                    //        li.Text += @"
                    //$('#select_1').dropdown();
                    //$('#select_2').dropdown();
                    //$('#select_3').dropdown();
                    //$('#select_4').dropdown();
                    //";

                    //        li.Text += @"
                    //                        });";
                    //        li.Text += @"</script>";

                    //        pdn_list.Controls.Add(li);
                    //        pdn_list.Controls.Add(new LiteralControl("<table align='center' width='100%' height='100%' style='background-color:#D0D0D0;'>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<tr>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</tr>"));
                    //        //select search
                    //        pdn_list.Controls.Add(new LiteralControl("<tr>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        li = new Literal();
                    //        li.Text = @" <select name='search_type_1' class='ui dropdown' id='select_1'>
                    //     <option value="+'"'+'"'+ @">日付</option>
                    //      <option value='0'>お食事</option>
                    //</select>";
                    //        pdn_list.Controls.Add(li);

                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        li = new Literal();
                    //        li.Text = @" <select name='search_type_2' class='ui dropdown' id='select_2'>
                    //     <option value=" + '"' + '"' + @">預け時刻</option>
                    //      <option value='0'>お食事</option>
                    //</select>";
                    //        pdn_list.Controls.Add(li);
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        li = new Literal();
                    //        li.Text = @" <select name='search_type_3' class='ui dropdown' id='select_3'>
                    //     <option value=" + '"' + '"' + @">お迎え時刻</option>
                    //      <option value='0'>お食事</option>
                    //</select>";
                    //        pdn_list.Controls.Add(li);
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        li = new Literal();
                    //        li.Text = @" <select name='search_type_4' class='ui dropdown' id='select_4'>
                    //     <option value=" + '"' + '"' + @">依頼内容</option>
                    //      <option value='0'>お食事</option>
                    //</select>";
                    //        pdn_list.Controls.Add(li);
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        Button but_sea=new Button();
                    //        but_sea.ID="search_list_button";
                    //        but_sea.CssClass = "file-upload1";
                    //        but_sea.Text="探す";
                    //        pdn_list.Controls.Add(but_sea);
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</tr>"));
                    //        //select search
                    //        pdn_list.Controls.Add(new LiteralControl("<tr>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='3%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='22%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("<td width='3%' height='5%'></td>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</tr>"));
                    //        pdn_list.Controls.Add(new LiteralControl("</table>"));


                    //        string message_type = Request.Form["search_type_1"].ToString();
                    //            if (message_type == "")
                    //            {
                    //                message_type = "0";
                    //            }

                    Panel pdn_list1 = new Panel();
                    pdn_list1 = (Panel)FindControl("Panel_for_supplist");
                    pdn_list1.Controls.Clear();
                    Query = "select *,YEAR(CreatedDate) 'Year Part',DATE_FORMAT(CreatedDate, '%m') 'Month Part',DATE_FORMAT(CreatedDate, '%d') 'Day Part' from db1.user_information_store ORDER BY CreatedDate desc;";
                    //sql_list.SelectCommand += " where id=" + Session["id"].ToString() + ";";
                    DataView ict_list = gc.select_cmd(Query);
                    if (ict_list.Count > 0)
                    {
                        for (int i = 0; i < ict_list.Count; i++)
                        {
                            pdn_list1.Controls.Add(new LiteralControl("<table align='center' width='100%'>"));
                            pdn_list1.Controls.Add(new LiteralControl("<tr>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='50%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                            pdn_list1.Controls.Add(new LiteralControl("</tr>"));
                            Query = "select photo,username from user_login where id='" + ict_list.Table.Rows[i]["uid"].ToString() + "';";
                            //sql_list.SelectCommand += " where id=" + Session["id"].ToString() + ";";
                            DataView ict_list1 = gc.select_cmd(Query);
                            if (ict_list1.Count > 0)
                            {


                                pdn_list1.Controls.Add(new LiteralControl("<tr>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                                //supporter photo
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));
                                pdn_list1.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));


                                string cutstr = ict_list1.Table.Rows[0]["photo"].ToString();
                                int ind = cutstr.IndexOf(@"/");
                                string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                                pdn_list1.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict_list1.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                                pdn_list1.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='50' height='50' />"));
                                pdn_list1.Controls.Add(new LiteralControl("</a>"));


                                pdn_list1.Controls.Add(new LiteralControl("</div>"));
                                pdn_list1.Controls.Add(new LiteralControl("</td>"));
                                //supporter name and information
                                pdn_list1.Controls.Add(new LiteralControl("<td width='50%' valign='top'>"));
                                HyperLink hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.Target = "_blank";
                                hyy.Text = ict_list1.Table.Rows[0]["username"].ToString();
                                hyy.Font.Underline = false;
                                pdn_list1.Controls.Add(hyy);
                                pdn_list1.Controls.Add(new LiteralControl("<br/>"));
                                Label la = new Label();
                                la.Text = "料金 : " + ict_list.Table.Rows[i]["money"].ToString() + "円 / 時";
                                la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000");
                                pdn_list1.Controls.Add(la);
                                pdn_list1.Controls.Add(new LiteralControl("<br/>"));
                                la = new Label();
                                la.Text = "サポート内容 : ";
                                la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000");

                                string str = "";
                                if (ict_list.Table.Rows[i]["choice1_1"].ToString() == "1")
                                {
                                    str += "送迎、";
                                }
                                if (ict_list.Table.Rows[i]["choice1_2"].ToString() == "1")
                                {
                                    str += "利用宅で預かる、";
                                }
                                if (ict_list.Table.Rows[i]["choice1_3"].ToString() == "1")
                                {
                                    str += "自宅で預かる、";
                                }
                                if (ict_list.Table.Rows[i]["choice1_4"].ToString() == "1")
                                {
                                    str += "乳児預かり、";
                                }
                                if (str.Length > 0)
                                {
                                    str = str.Substring(0, str.Length - 1);
                                }
                                la.Text += str;
                                pdn_list1.Controls.Add(la);
                                pdn_list1.Controls.Add(new LiteralControl("<br/>"));
                                la = new Label();
                                la.Text = "資格‧経験 : ";
                                la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000");
                                str = "";
                                if (ict_list.Table.Rows[i]["choice2_1"].ToString() == "1")
                                {
                                    str += "子育て経験、";
                                }
                                //自治體
                                if (ict_list.Table.Rows[i]["gov_choice"].ToString() == "1")
                                {
                                    str += "自治体認定、";
                                }
                                if (ict_list.Table.Rows[i]["choice2_2"].ToString() == "1")
                                {
                                    str += "保育士資格、";
                                }
                                bool check_m = false;

                                if (ict_list.Table.Rows[i]["choice2_3"].ToString() == "1")
                                {
                                    check_m = true;
                                }
                                else if (ict_list.Table.Rows[i]["choice2_5"].ToString() == "1")
                                {
                                    check_m = true;
                                }

                                if (check_m)
                                {
                                    str += "幼稚園/小学校教員資格、";
                                }
                                if (ict_list.Table.Rows[i]["choice2_6"].ToString() == "1")
                                {
                                    str += "医師看護士資格、";
                                }
                                if (ict_list.Table.Rows[i]["choice2_4"].ToString() == "1")
                                {
                                    str += "障害児預かり経験、";
                                }
                                if (str.Length > 0)
                                {
                                    str = str.Substring(0, str.Length - 1);
                                }
                                la.Text += str;
                                pdn_list1.Controls.Add(la);

                                pdn_list1.Controls.Add(new LiteralControl("</td>"));
                                //supporter new
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));

                                string d = ict_list.Table.Rows[i]["CreatedDate"].ToString();
                                DateTime dt = Convert.ToDateTime(d);

                                if ((DateTime.Now - dt).TotalDays < 14)
                                {
                                    Image img = new Image();
                                    img.ImageUrl = "~/images/home_images/new.png";
                                    pdn_list1.Controls.Add(img);
                                }


                                pdn_list1.Controls.Add(new LiteralControl("</td>"));
                                //supporter create date
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));
                                la = new Label();
                                la.Text = ict_list.Table.Rows[i]["Year Part"].ToString() + "." + ict_list.Table.Rows[i]["Month Part"].ToString() + "." + ict_list.Table.Rows[i]["Day Part"].ToString();
                                la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D0D0D0");
                                pdn_list1.Controls.Add(la);

                                pdn_list1.Controls.Add(new LiteralControl("</td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("</tr>"));


                                pdn_list1.Controls.Add(new LiteralControl("<tr>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='50%' height='5%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'>"));
                                hyy = new HyperLink();
                                hyy.NavigateUrl = "javascript:void(0);";
                                hyy.ID = "datedetail_" + ict_list.Table.Rows[i]["uid"].ToString();
                                hyy.Target = "_blank";
                                hyy.Text = "詳細を見る";
                                hyy.Attributes["onclick"] = "gotodate(this.id)";
                                hyy.Font.Underline = false;
                                pdn_list1.Controls.Add(hyy);
                                pdn_list1.Controls.Add(new LiteralControl("</td>"));
                                pdn_list1.Controls.Add(new LiteralControl("<td width='10%' height='5%'></td>"));
                                pdn_list1.Controls.Add(new LiteralControl("</tr>"));
                                pdn_list1.Controls.Add(new LiteralControl("</table>"));
                                pdn_list1.Controls.Add(new LiteralControl("<hr/>"));
                            }

                        }

                    }
                    //SqlDataSource sql_list = new SqlDataSource();
                    //sql_list.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //sql_list.SelectCommand = "select photo from user_login ";
                    //sql_list.SelectCommand += " where id=" + Session["id"].ToString() + ";";
                    //DataView ict_list = (DataView)sql_list.Select(DataSourceSelectArguments.Empty);
                    //if (ict_list.Count > 0)
                    //{
                    //    cutstr2 = ict_list.Table.Rows[0]["photo"].ToString();
                    //    ind2 = cutstr2.IndexOf(@"/");
                    //    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    //}
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        ScriptManager.RegisterStartupScript(message_type0_Button, typeof(Button), "alert", "alert('" + reader.ReadToEnd() + "');", true);
                        Session.Clear();
                        Response.Redirect("home.aspx");
                    }
                }


        Panel_for_support_list.Visible = false;
        Panel_for_supplist.Visible = false;




                }
    public class sortstateComparer : IEqualityComparer<sortstate>
    {
        public bool Equals(sortstate x, sortstate y)
        {
            return x.latlng.Equals(y.latlng, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(sortstate obj)
        {
            return obj.latlng.GetHashCode();
        }
    }
    public class sortstate
    {
        public int id { get; set; }
        public string latlng { get; set; }
    }

    public class sorttalk
    {
        public int id = 0;
        public int point_id = 0;
        public int level = 0;
        public string mess = "";
        public string filename = "";
        public int uid = 0;
        public string username = "";
        public string photo = "";
    }


    [WebMethod(EnableSession=true)]
    public static string check_login(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        //string result = param1 + "," + param2;
        string result = "";
        try
        {
            string username = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string password = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            if (username != "" && password != "" )
            {
                Query1 = "select id from user_login";
                Query1 += " where login_name='" + username + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {

                    Query1 = "select id from user_login";
                    Query1 += " where login_name='" + username + "' and login_password='" + password + "';";
                    DataView ict_f1 = gc1.select_cmd(Query1);
                    if (ict_f1.Count > 0)
                    {
                        Query1 = "select UserId from UserActivation";
                        Query1 += " where UserId='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                        DataView ict_check = gc1.select_cmd(Query1);
                        if (ict_check.Count > 0)
                        {
                            result = "メールをご確認ください。";
                        }
                        else
                        {
                            HttpContext.Current.Session["id"] = ict_f1.Table.Rows[0]["id"].ToString();
                            HttpContext.Current.Session["loginname"] = username;
                            result = "ログインできました。";
                        }
                    }
                    else
                    {
                        result = "パスワードが間違っています。";
                    }

                }
                else
                {
                    result = "メールアドレスが間違っています。";
                }
            }



        }
        catch (Exception ex)
        {
            result = "ログインできませんでした。";

            //return result;
            throw ex;
        }
        return result;

    }
    [WebMethod(EnableSession = true)]
    public static string check_photo(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        //string result = param1 + "," + param2;
        string result = "";
        string id = param1;
        Query1 = "select photo from user_login";
        Query1 += " where id='" + id + "';";
        DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                   int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    result = cutstr3;
                }

        return result;
    }

    protected void register_Button_Click(object sender, EventArgs e)
    {
        Response.Redirect("registered0.aspx");
    }
    protected void message_type0_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 0;
        Response.Redirect("main_guest.aspx");
    }
    protected void message_type1_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 1;
        Response.Redirect("main_guest.aspx");
    }
    protected void message_type2_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 2;
        Response.Redirect("main_guest.aspx");
    }
    protected void message_type3_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 3;
        Response.Redirect("main_guest.aspx");
    }
    protected void message_type4_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 4;
        Response.Redirect("main_guest.aspx");
    }
    protected void message_type5_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 5;
        Response.Redirect("main_guest.aspx");
    }

    protected void video_list_Button_Click(object sender, EventArgs e)
    {

    }

    protected void supporter_list_Click(object sender, EventArgs e)
    {
        post_message_panel.Visible = false;
        Panel2.Visible = false;

        social_Button.Style.Add("background-color", "transparent");
        Button_for_kid.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");

        supporter_list.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");

        video_list_Button.Style.Add("background-color", "transparent");
        supporter_manger.Style.Add("background-color", "transparent");
        Button_for_social.Style.Add("background-color", "transparent");
        message_type0_Button.Style.Add("background-color", "transparent");
        message_type1_Button.Style.Add("background-color", "transparent");
        message_type2_Button.Style.Add("background-color", "transparent");
        message_type3_Button.Style.Add("background-color", "transparent");
        message_type4_Button.Style.Add("background-color", "transparent");
        message_type5_Button.Style.Add("background-color", "transparent");
    }
    protected void supporter_manger_Click(object sender, EventArgs e)
    {
        Response.Redirect("user_date_manger.aspx");
    }
    protected void social_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = null;
        Response.Redirect("main_guest_light.aspx");

        ////post_message_panel.Visible = true;
        //Panel2.Visible = true;
        //Panel_for_supplist.Visible = false;
        //Panel_for_support_list.Visible = false;
        //social_Button.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");

        //Button_for_kid.Style.Add("background-color", "transparent");
        //supporter_list.Style.Add("background-color", "transparent");
        //video_list_Button.Style.Add("background-color", "transparent");
        //supporter_manger.Style.Add("background-color", "transparent");
        //Button_for_social.Style.Add("background-color", "transparent");
        //message_type0_Button.Style.Add("background-color", "transparent");
        //message_type1_Button.Style.Add("background-color", "transparent");
        //message_type2_Button.Style.Add("background-color", "transparent");
        //message_type3_Button.Style.Add("background-color", "transparent");
        //message_type4_Button.Style.Add("background-color", "transparent");
        //message_type5_Button.Style.Add("background-color", "transparent");
    }
    [WebMethod(EnableSession = true)]
    public static string changetodate(string param2)
    {
        string result = "";
        HttpContext.Current.Session["sup_id"] = param2;
        return result;
    }
    protected void Button_for_kid_Click(object sender, EventArgs e)
    {
        post_message_panel.Visible = false;
        Panel2.Visible = false;

        Panel_for_support_list.Visible = true;
        Panel_for_supplist.Visible = true;
        social_Button.Style.Add("background-color", "transparent");
        Button_for_kid.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");

        supporter_list.Style.Add("background-color", "rgba(232, 203, 203, 0.66)");

        video_list_Button.Style.Add("background-color", "transparent");
        supporter_manger.Style.Add("background-color", "transparent");
        Button_for_social.Style.Add("background-color", "transparent");
        message_type0_Button.Style.Add("background-color", "transparent");
        message_type1_Button.Style.Add("background-color", "transparent");
        message_type2_Button.Style.Add("background-color", "transparent");
        message_type3_Button.Style.Add("background-color", "transparent");
        message_type4_Button.Style.Add("background-color", "transparent");
        message_type5_Button.Style.Add("background-color", "transparent");
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] Getsearch(string prefix)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        List<string> customers = new List<string>();

        Query1 = "select id,username,photo from user_login where username like '" + prefix.Replace("'", "").Replace(@"""", "") + "%'";
        DataView ict_sf = gc1.select_cmd(Query1);

        for (int i = 0; i < ict_sf.Count; i++)
        {
            string cutstr = ict_sf.Table.Rows[i]["photo"].ToString();
            int ind = cutstr.IndexOf(@"/");
            string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
            customers.Add(string.Format("{0};{1};{2}", ict_sf.Table.Rows[i]["username"], ict_sf.Table.Rows[i]["id"], cutstr1));
        }
        return customers.ToArray();
    }

    public class status_mess_list
    {
        public int id = 0;
        public string message = "";
    }
    public class status_mess_list_like
    {
        public int type = 0;

        public int id = 0;
        public int uid = 0;
        public string message = "";


        public int like_id = 0;
        public string like_message = "";
        public List<int> like_idlist = new List<int>();
        public DateTime comdate = new DateTime();
    }
    [WebMethod(EnableSession = true)]
    public static string search_new_post(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result_res = "";
        string javascr = "";
        int counn = Convert.ToInt32(param1);
        //counn += 10;
        HttpContext.Current.Session["top_count"] = counn;
        //check max post
        if (HttpContext.Current.Session["max_count_post"] != null)
        {
            if (HttpContext.Current.Session["max_count_post"].ToString().Trim() != "")
            {
                if ((counn - 9) > Convert.ToInt32(HttpContext.Current.Session["max_count_post"].ToString()))
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
        //Panel pdn_j = (Panel)System.Web.UI.Control.FindControl("javaplace");
        List<string> postal_code_list = new List<string>();

        int couuu = 0;
        Literal lip = new Literal();
        Literal lip1 = new Literal();
        Literal lip2 = new Literal();

            lip1.Text = "";
            lip2.Text = "";
            lip2.Text += @"var bounds = new google.maps.LatLngBounds();";
            List<string> check_pos = new List<string>();
            postal_code_list.Add(HttpContext.Current.Session["postcode"].ToString());

                string result = "";

                var url = new Uri("https://postcode.teraren.com/postcodes/" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Session["postcode"].ToString().Replace("-", "")) + ".json");


                    System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    using (var response = request.GetResponse())
                    using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                    //TextBox1.Text = result;


                    JObject jArray = JObject.Parse(result);
                    string jis = (string)jArray["jis"];
                    bool chc_same = true;
                    if (check_pos.Count > 0)
                    {
                        for (int io = 0; io < check_pos.Count; io++)
                        {
                            if (jis == check_pos[io])
                            {
                                chc_same = false;
                            }
                        }

                    }
                    else
                    {
                        check_pos.Add(jis);
                    }
                    if (chc_same)
                    {
                        check_pos.Add(jis);
                        //string state = (string)jArray["prefecture"];
                        //string city = (string)jArray["city"];
                        //string town = (string)jArray["suburb"];
                        Query1 = "select c.con_id";
                        Query1 += " from JP_City as b use index (IX_JP_City)";
                        Query1 += " inner join JP_City_G as c use index (IX_JP_City_G_1) on b.jis=c.jis";
                        Query1 += " where b.jis='" + jis + "';";
                        DataView ict2 = gc1.select_cmd(Query1);
                        if (ict2.Count > 0)
                        {
                            for (int ii = 0; ii < ict2.Count; ii++)
                            {
                                //
                                Query1 = "select e.lat,e.lng";
                                Query1 += " from JP_City_G as c use index (IX_JP_City_G)";
                                Query1 += " inner join JP_City_Town_G as d use index (IX_JP_City_Town_G_1) on c.con_id=d.City_id";

                                Query1 += " inner join test_GPS as e use index (IX_test_GPS) on d.place_id=e.place_id";
                                Query1 += " where c.con_id='" + ict2.Table.Rows[ii]["con_id"].ToString() + "';";
                                DataView ict1 = gc1.select_cmd(Query1);
                                if (ict1.Count > 0)
                                {
                                    //                               lip2.Text += @"var bounds" + couuu + @" = new google.maps.LatLngBounds();
                                    //bounds" + couuu + @".extend(center);";
                                    lip1.Text += @"var triangleCoords" + couuu + @" = [
";
                                    string str = "";
                                    for (int ix = 0; ix < ict1.Count; ix++)
                                    {
                                        str += "{ lat: " + ict1.Table.Rows[ix]["lat"].ToString() + ", lng: " + ict1.Table.Rows[ix]["lng"].ToString() + " },";
                                        lip2.Text += @"bounds.extend(new google.maps.LatLng(" + ict1.Table.Rows[ix]["lat"].ToString() + ", " + ict1.Table.Rows[ix]["lng"].ToString() + "));";
                                    }
                                    if (str.Length > 0)
                                    {
                                        str.Substring(0, str.Length - 1);
                                    }
                                    //lip2.Text += @"map1.fitBounds(bounds" + couuu + @");";
                                    lip1.Text += str;
                                    lip1.Text += @"];

            // Construct the polygon.
            var bermudaTriangle" + couuu + @" = new google.maps.Polygon({
                paths: triangleCoords" + couuu + @",
                strokeColor: '#FF0000',
                strokeOpacity: 0.20,
                strokeWeight: 1,
                fillColor: '#FF0000',
                fillOpacity: 0.05
            });
            bermudaTriangle" + couuu + @".setMap(map1);
";

                                    couuu += 1;



                                }
                                //
                            }
                        }
                    }



                    //TextBox1.Text = jis ;



                    //////////
                    for (int ji = 0; ji < check_pos.Count; ji++)
                   {
                       Query1 = "select zipcode";
                       Query1 += " from zipcode_f_01 use index (IX_zipcode_f_01)";
                       Query1 += " where pref_jis like '" + check_pos[ji] + "%';";
                       DataView ict_jis = gc1.select_cmd(Query1);
                       if (ict_jis.Count > 0)
                       {
                           for (int ii = 0; ii < ict_jis.Count; ii++)
                           {
                               string zip_for = ict_jis.Table.Rows[ii]["zipcode"].ToString().Substring(0, 3) + "-" + ict_jis.Table.Rows[ii]["zipcode"].ToString().Substring(3, 4);
                               postal_code_list.Add(zip_for);
                           }
                       }
                   }

            /////
            bool check_sort_pop = false;
            Query1 = " select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query1 += "from status_messages as a use index (IX_status_messages_1)";
            Query1 += " inner join user_login as b on b.id=a.uid where 1=1 and a.type='0'";
            if (HttpContext.Current.Session["message_type"] != null)
            {
                if (HttpContext.Current.Session["message_type"].ToString() != "")
                {
                    if (HttpContext.Current.Session["message_type"].ToString() == "1")
                    {
                        check_sort_pop = true;
                    }
                    else
                    {
                        Query1 += " and a.message_type='" + HttpContext.Current.Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                Query1 += qustr;
            }
            if (check_sort_pop)
            {
                Query1 += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }
            else
            {
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }
            DataView ict_place = gc1.select_cmd(Query1);

            if (ict_place.Count > 0)
            {
                HashSet<sortstate> bands = new HashSet<sortstate>(new sortstateComparer());
                for (int i = 0; i < ict_place.Count; i++)
                {
                    bands.Add(new sortstate { id = i, latlng = ict_place.Table.Rows[i]["place_lat"].ToString() + "," + ict_place.Table.Rows[i]["place_lng"].ToString() });
                }
                List<sortstate> sorst_list1 = bands.ToList<sortstate>();


                lip.Text = @"<script>

var locations = [
                    ";
                string llllstr = "";
                string flat = "", flng = "";
                string content = "";
                for (int i = 0; i < sorst_list1.Count; i++)
                {
                    string icon_type = "'images/map_pin.png'";

                    string cutstr2 = ict_place.Table.Rows[sorst_list1[i].id]["photo"].ToString();
                    int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    string metype = "";
                    if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 0)
                    {
                        icon_type = "'images/map_icon/food.png'";
                        metype = "お食事、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 1)
                    {
                        metype = "人気スポット、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 2)
                    {
                        icon_type = "'images/map_icon/event.png'";
                        metype = "イベント、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 3)
                    {
                        icon_type = "'images/map_icon/hosp.png'";
                        metype = "病院、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 4)
                    {
                        icon_type = "'images/map_icon/park.png'";
                        metype = "公園／レジャー、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 5)
                    {
                        icon_type = "'images/map_icon/milk.png'";
                        metype = "、";
                    }
                    else if (Convert.ToInt32(ict_place.Table.Rows[sorst_list1[i].id]["message_type"].ToString()) == 6)
                    {
                        metype = "指定なし、";
                    }
                    string mess = "";
                    if (ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString().Length < 37)
                    {
                        mess = ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString();
                    }
                    else
                    {
                        mess = ict_place.Table.Rows[sorst_list1[i].id]["message"].ToString().Substring(0, 37) + "‧‧‧";
                    }


                    content = '"' + @"<div><table width='100%'><tr><td width='20%' valign='top'><img src='" + cutstr3 + @"' height='50px' width='50px'></td><td width='80%' valign='top' style='word-wrap: break-word;'><a href='~/user_home_friend.aspx?=" + ict_place.Table.Rows[sorst_list1[i].id]["uid"].ToString() + @"' style='text-decoration: none;'>" + ict_place.Table.Rows[sorst_list1[i].id]["username"].ToString() + @"</a><br/><span style='color:#CCCCCC;'>" + ict_place.Table.Rows[sorst_list1[i].id]["year"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["month"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span><br/><span>" + mess + @"</span><br/></td></tr></table></div>" + '"';
                    //                content = '"'+@"<div><table width='100%'>
                    //<tr>
                    //<td width='20%' valign='top'>
                    //<img src='" + cutstr3 + @"' height='50px' width='50px'>
                    //</td>
                    //<td width='80%' valign='top' style='word-wrap: break-word;'>
                    //<a href='javascript: void(0)' style='text-decoration: none;'>" + ict_place.Table.Rows[i]["username"].ToString() + @"</a>
                    //<br/>
                    //<span style='color:#CCCCCC;'>" + ict_place.Table.Rows[i]["year"].ToString() + "." + ict_place.Table.Rows[i]["month"].ToString() + "." + ict_place.Table.Rows[i]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span>
                    //<br/>
                    //<span>" + mess + @"</span>
                    //<br/>
                    //</td>
                    //</tr>
                    //</table>
                    //</div>" + '"';


                    if (ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() != "" && ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() != "")
                    {
                        flat = ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString();
                        flng = ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString();
                        llllstr += @"[" + content + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() + ", " + sorst_list1[i].id + "," + icon_type + "],";
                        lip2.Text += @"bounds.extend(new google.maps.LatLng(" + ict_place.Table.Rows[sorst_list1[i].id]["place_lat"].ToString() + ", " + ict_place.Table.Rows[sorst_list1[i].id]["place_lng"].ToString() + "));";
                    }
                    else
                    {
                        if (ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString().Trim() != "")
                        {
                            result = "";

                            url = new Uri("http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString()));

                            request = (HttpWebRequest)HttpWebRequest.Create(url);
                            using (var response = request.GetResponse())
                            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                result = sr.ReadToEnd();
                            }

                            jArray = JObject.Parse(result);
                            string lat = (string)jArray["results"][0]["geometry"]["location"]["lat"];
                            string lng = (string)jArray["results"][0]["geometry"]["location"]["lng"];
                            llllstr += @"[" + content + "," + lat + ", " + lng + ", " + i + "," + icon_type + "],";
                            lip2.Text += @"bounds.extend(new google.maps.LatLng(" + lat + ", " + lng + "));";
                            flat = lat;
                            flng = lng;
                            Query1 = "update status_messages set place_lat='" + lat + "',place_lng='" + lng + "'";
                            Query1 += " where id='" + ict_place.Table.Rows[sorst_list1[i].id]["id"].ToString() + "';";
                            resin = gc1.update_cmd(Query1);


                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                if (llllstr.Length > 0)
                {
                    llllstr.Substring(0, llllstr.Length - 1);
                    lip.Text += llllstr;
                }
                if (flat == "" && flng == "")
                {
                    flat = "35.447824";
                    flng = "139.6416613";
                }
                lip.Text += @"
            ];
/*var center;
if(locations.length>0)
{
center=new google.maps.LatLng(locations[locations.length-1][1], locations[locations.length-1][2]);
}*/
var allMarkers = [];
                var map1;
             function initMap1() {
                        map1 = new google.maps.Map(document.getElementById('show_map_area'), {

                            zoom: 14,
                            center: { lat: " + flat + @", lng: " + flng + @" },
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        });";
                lip.Text += @"
                    var infoWindow1= new google.maps.InfoWindow();

                var marker, i;
var imageUrl = 'images/map_pin.png';

    for (i = 0; i < locations.length; i++) {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][1], locations[i][2]),
        map: map1,
      id:locations[i][3],
        icon: locations[i][4]
      });
allMarkers.push(marker);


Element.prototype.documentOffsetTop = function () {
    return this.offsetTop + ( this.offsetParent ? this.offsetParent.documentOffsetTop() : 0 );
};


      google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
var top = document.getElementById('state_mess_'+locations[i][3]).documentOffsetTop() - (window.innerHeight / 2 );
 $('html, body').animate({
        scrollTop: top
    }, 500);


setTimeout(function(){ document.getElementById('state_mess_'+locations[i][3]).style.backgroundColor = '#FFFFFF'; }, 1000);
for (ii = 0; ii < locations.length; ii++) {
if(ii==i)
{
document.getElementById('state_mess_'+locations[ii][3]).style.backgroundColor = '#FFD0D0';
}else
{
document.getElementById('state_mess_'+locations[ii][3]).style.backgroundColor = '#FFFFFF';
}
}
          infoWindow1.setContent(locations[i][0]);
          infoWindow1.open(map1, marker);
        }
      })(marker, i));
}


";
                lip.Text += lip1.Text;
                lip.Text += lip2.Text;
                lip.Text += @"
map1.setCenter(bounds.getCenter());

}
initMap1();
//MACRO
//Function called when hover the div
function hover(id) {

    for ( var i = 0; i< allMarkers.length; i++) {
        if (id === allMarkers[i].id) {
var icon = {
    url: locations[i][4],
    scaledSize: new google.maps.Size(50, 50),

};


           allMarkers[i].setIcon(icon);

           break;
        }
   }
}
  $(function () {
            $('.lazy').Lazy({
                threshold: 200,
                effect: 'fadeIn',
                visibleOnly: true,
                effect_speed: 'fast',
                onError: function (element) {
                    console.log('error loading ' + element.data('src'));
                }
            });
        });
//Function called when out the div
function out(id) {
    for ( var i = 0; i< allMarkers.length; i++) {
        if (id === allMarkers[i].id) {
var icon = {
    url: locations[i][4],
    scaledSize: new google.maps.Size(30, 30),

};
            allMarkers[i].setIcon(icon);
           break;
        }
   }
}
            </script>
            ";
                javascr += lip.Text;
                //result_res += lip.Text;
            }


            /////




            //////autocomplete



            //////autocomplete


            Query1 = "select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query1 += "from status_messages as a use index (IX_status_messages_1)";
            Query1 += " inner join user_login as b on b.id=a.uid where 1=1 and a.type='0'";

            //if want to class by type use type=0,1,2 ; message_type=0,1,2

            ////Before today's message
            //sql1.SelectCommand += " where a.year<=" + DateTime.Now.Year.ToString() + " and a.month<=" + DateTime.Now.Month.ToString();
            //sql1.SelectCommand += " and a.day<=" + DateTime.Now.Day.ToString() + " ";


            ////type message select
            if (HttpContext.Current.Session["message_type"] != null)
            {
                if (HttpContext.Current.Session["message_type"].ToString() != "")
                {
                    if (HttpContext.Current.Session["message_type"].ToString() != "1")
                    {
                        Query1 += " and a.message_type='" + HttpContext.Current.Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                Query1 += qustr;
            }
            if (check_sort_pop)
            {
                Query1 += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }
            else
            {
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }

            DataView ict = gc1.select_cmd(Query1);


            Literal li = new Literal();

            li.Text = @"
<script>

$(function () {

";

            for (int i = 0; i < ict.Count; i++)
            {
                li.Text += @"

$('#btnFileUpload" + i + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar" + i + @"').show();
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar" + i + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').show();
        document.getElementById('make-image" + i + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        console.log('error', error);
    }
});";
            }
            li.Text += @"
                        });";
            li.Text += @"</script>";

            li.Text += @"
<script type='text/javascript'>

$(function () {

";
            for (int i = 0; i < ict.Count; i++)
            {
                li.Text += @"

$('.hidde" + i + @"').toggle(false);

            $('.box" + i + @"').click(function () {
                $('.hidde" + i + @"').toggle();
                $('.box" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').toggle(false);

            $('.likebox" + i + @"').click(function () {
                $('.likehidde" + i + @"').toggle();
                $('.likebox" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').click(function () {
                $('.likebox" + i + @"').toggle();
                $('.likehidde" + i + @"').toggle(false);
            })

            $('.mess_hidde" + i + @"').toggle(false);

            $('.mess_box" + i + @"').click(function () {
                $('.mess_hidde" + i + @"').toggle();
                $('.mess_box" + i + @"').toggle(false);
            })


            $('.big_mess_hidde" + i + @"').toggle(false);

            $('.big_mess_box" + i + @"').click(function () {
                $('.big_mess_hidde" + i + @"').toggle();
                $('.big_mess_box" + i + @"').toggle(false);
                $('.status_message_hidde" + i + @"').toggle();
                $('.status_message_box" + i + @"').toggle(false);
            })

            $('.big_mess_hidde" + i + @"').click(function () {
                $('.big_mess_box" + i + @"').toggle();
                $('.big_mess_hidde" + i + @"').toggle(false);
                $('.status_message_box" + i + @"').toggle();
                $('.status_message_hidde" + i + @"').toggle(false);
            })

            $('.status_message_hidde" + i + @"').toggle(false);


";

//                SqlDataSource sql3 = new SqlDataSource();
//                sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//                sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//                if (ict2.Count > 3)
//                {
//                    li.Text += @"
//$('.imhidde" + ((counn - 10) + i) + @"').toggle(false);
//$('.imhiddee" + ((counn - 10) + i) + @"').toggle(false);
//
//            $('.imbox" + ((counn - 10) + i) + @"').click(function () {
//                $('.imhidde" + ((counn - 10) + i) + @"').toggle();
//                $('.imhiddee" + ((counn - 10) + i) + @"').toggle();
//                $('.imbox" + ((counn - 10) + i) + @"').toggle(false);
//            })
//
//            $('.imhiddee" + ((counn - 10) + i) + @"').click(function () {
//                $('.imbox" + ((counn - 10) + i) + @"').toggle();
//                $('.imhidde" + ((counn - 10) + i) + @"').toggle(false);
//                $('.imhiddee" + ((counn - 10) + i) + @"').toggle(false);
//            })
//
//
//";
//                }
            }

            li.Text += @"

$('.image-link').magnificPopup({
                type: 'image',
                mainClass: 'mfp-with-zoom', // this class is for CSS animation below

                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it

                    duration: 100, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function

                    opener: function (openerElement) {
                        return openerElement.is('img') ? openerElement : openerElement.find('img');
                    }
                }

            });

            $('.zoom-gallery').each(function () { // the containers for all your galleries
                $(this).magnificPopup({
                    delegate: 'a',
                    type: 'image',
                    closeOnContentClick: false,
                    closeBtnInside: false,
                    mainClass: 'mfp-with-zoom mfp-img-mobile',
                    image: {
                        verticalFit: true,
                        titleSrc: function (item) {
                            return item.el.attr('title') + ' &middot; <a class=" + '"' + @"image-source-link" + '"' + @" href=" + '"' + @"' + item.el.attr('data-source') + '" + '"' + @" target=" + '"' + @"_blank" + '"' + @">image source</a>';
                        }
                    },
                    gallery: {
                        enabled: true
                    },
                    zoom: {
                        enabled: true,
                        duration: 100, // don't foget to change the duration also in CSS
                        opener: function (element) {
                            return element.find('img');
                        }
                    }
                });
            });

                        });";
            li.Text += @"</script>";
            javascr += li.Text;

            //result_res += li.Text;


            //this.Page.Controls.Add(li);


            //this.Page.Header.Controls.Add(li);
            ////添加至指定位置
            //this.Page.Header.Controls.AddAt(0, li);

            //            Literal litCss = new Literal();
            //            litCss.Text = @"
            //                <style type='text/css'>
            //                    #post_message_panel{
            //                    background-color:#fff;
            //                    border: thick solid #E9EBEE;
            //                            }
            //                 </style>";
            //            result_res += litCss.Text;

            for (int i = 0; i < ict.Count; i++)
            {
                result_res += "<div id='state_mess_" + i + "' style='background-color: #FFF;'onmouseover='hover(" + i + ")' onmouseout='out(" + i + ")'>";

                result_res += "<table width='100%' style='border: thick solid #E9EBEE;'>";
                result_res += "<tr>";
                result_res += "<td>";
                //big message place
                result_res += "<table width='100%' style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;'>";
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'><br/></td><td width='90%' height='5%'><br/></td><td width='5%' height='5%'><br/></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                result_res += "<td></td>";
                result_res += "<td>";
                //new message place
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                //Poster photo
                result_res += "<td width='10%' rowspan='2' valign='top'>";

                result_res += "<div class='zoom-gallery'>";

                string cutstr2 = ict.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:40px;height:40px;'>";
                result_res += "<img src='" + cutstr3 + "' width='40' height='40' />";
                result_res += "</a>";



                //Image img = new Image();
                //img.Width = 50; img.Height = 50;
                //img.ImageUrl = ict.Table.Rows[i]["photo"].ToString();
                //pdn2.Controls.Add(img);


                result_res += "</div>";
                result_res += "</td>";
                //poster username
                result_res += "<td width='100%'>";

                result_res += "<a href='user_home_friend.aspx?=" + ict.Table.Rows[i]["uid"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict.Table.Rows[i]["username"].ToString() + "</a>";
                result_res += "</td>";
                result_res += "</tr>";
                //poster message type and time
                result_res += "<tr>";
                result_res += "<td width='100%'>";
                result_res += "<span style='color:#CCCCCC;'>";
                result_res += "";
                if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 0)
                {
                    result_res += "お食事、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 1)
                {
                    result_res += "人気スポット、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 2)
                {
                    result_res += "イベント、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 3)
                {
                    result_res += "病院、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 4)
                {
                    result_res += "公園／レジャー、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 5)
                {
                    result_res += "、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 6)
                {
                    result_res += "指定なし、";
                }
                result_res += ict.Table.Rows[i]["place"].ToString() + " ";
                result_res += ict.Table.Rows[i]["year"].ToString() + "." + ict.Table.Rows[i]["month"].ToString() + "." + ict.Table.Rows[i]["day"].ToString();
                result_res += "</span>";


                result_res += "</td>";
                result_res += "</tr>";
                //poster message
                result_res += "<tr>";
                result_res += "<td colspan='2' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">";
                result_res += "<br/><div class='box" + i + "'>";
                if (ict.Table.Rows[i]["message"].ToString().Length < 37)
                {
                    result_res += main_guest_light.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString());
                }
                else
                {
                    result_res += ict.Table.Rows[i]["message"].ToString().Substring(0, 37) + "‧‧‧";
                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>もっと見る</a>";
                }


                result_res += "</div>";
                result_res += "<div class='hidde" + i + "'>";

                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + main_guest_light.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString()) + "</span>";

                result_res += "<br/>";


                //if (ict.Table.Rows[i]["message"].ToString().Length > 36)
                //{
                //    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>たたむ</a>";

                //}


                result_res += "</div>";
                result_res += "<div>";
                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + main_guest_light.ConvertUrlsToLinks_DIV(ict.Table.Rows[i]["message"].ToString()) + "</span>";
                result_res += "</div>";
                result_res += "</td>";
                result_res += "</tr>";
                //poster images
                result_res += "<tr>";
                result_res += "<td colspan='2' width='90%' align='center'><br/><br/>";
                Query1 = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict1 = gc1.select_cmd(Query1);
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                int typ = Convert.ToInt32(rand.Next(0, ict1.Count));
                if (ict1.Count > 3)
                {
                    result_res += "<div class='imbox" + i + "'>";
                    //for (int ii = 0; ii < 3; ii++)
                    //{
                    //    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                    //    int ind = cutstr.IndexOf(@"/");
                    //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    //    result_res += "<img src='" + cutstr1 + "' style='height:100px;width:100px;'>";
                    //    result_res += "&nbsp;";
                    //}
                    //result_res += "<br/>";

                    //result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>もっと見る</a>";
                    //result_res += "</div>";
                    //result_res += "<div class='imhidde" + ((counn - 10) + i) + "'>";
                    result_res += "<div id='freewall" + i + "'>";
                    result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                    result_res += "<div class='zoom-gallery'>";
                    string morefour = "";
                    int countimg = 0;
                    for (int ii = 0; ii < ict1.Count; ii++)
                    {
                        //if (ii > 0 && ii % 3 == 0)
                        //{
                        //    result_res += "<br/>";
                        //}

                        string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                        int ind = cutstr.IndexOf(@"/");
                        string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        //block grid
                        if (ii > 3)
                        {
                            countimg += 1;
                            result_res += "<div style='visibility:hidden;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:0px; height:0px;outline : none;'>";
                            result_res += "<img src='images/test.png' width='0px' height='0px'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        else
                        {
                            if (ii == 3)
                            {
                                morefour += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                                morefour += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            }
                            else
                            {
                                result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";

                                //pdn2.Controls.Add(new LiteralControl("<div class='cell' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px; background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                                result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";

                                result_res += "</a>";
                                result_res += "</div>";
                            }


                        }

                        //result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                        //result_res += "<img src='" + cutstr1 + "' width='100' height='100' />";
                        //result_res += "</a>";
                    }
                    //countimg
                    if (countimg > 0)
                    {
                        morefour += "<img src='images/test.png' style='background-color: #000; opacity: 0.8; width: 100%; height: 100%; text-align: center;'/>";
                        morefour += "<span style='color: white;position: absolute;top:50%;left:40%;font-size:xx-large;'>+" + countimg + "</span>";
                    }
                    else
                    {
                        morefour += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                    }
                    morefour += "</a>";
                    morefour += "</div>";
                    //string blockimg="<div style='position: absolute; background-color: #000; z-index: 999997; opacity: 0.8; width: 100%; height: 100%; text-align: center;'>";
                    result_res += morefour;
                    result_res += "</div>";
                    result_res += "</div>";
                    result_res += "</div>";
                    //
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>
                                        var wall" +  i + @" = new Freewall('#freewall" +  i + @"');
                    			wall" +  i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" +  i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res += litjs.Text;


                    result_res += "</div>";

                    //result_res += "<div class='imhiddee" + ((counn - 10) + i) + "'>";
                    //result_res += "<br/>";
                    //result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>たたむ</a>";
                    //result_res += "</div>";
                }
                else if (ict1.Count > 0)
                {
                    string cutstr = ict1.Table.Rows[0]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                    if (ict1.Count == 1)
                    {
                        result_res += "<div class='zoom-gallery'>";
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr1 + "' src='images/loading.gif' style='width:100%;height:100%;'/>";
                        result_res += "</a>";
                        result_res += "</div>";
                    }
                    else if (ict1.Count == 2)
                    {
                        result_res += "<div id='freewall" + i + "'>";
                        result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res += "<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        result_res += "</div>";
                        result_res += "</div>";
                        result_res += "</div>";
                    }
                    else if (ict1.Count == 3)
                    {
                        result_res += "<div id='freewall" +  i + "'>";
                        result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res += "<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                        }
                        else if (typ == 1)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        result_res += "</div>";
                        result_res += "</div>";
                        result_res += "</div>";
                    }
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>

                                        var wall" +  i + @" = new Freewall('#freewall" +  i + @"');
                    			wall" +  i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" + i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res += litjs.Text;
                }

                string id = "";
                bool check_li = false;
                if (HttpContext.Current.Session["id"] != null)
                {
                    if (HttpContext.Current.Session["id"].ToString() != "")
                    {
                        id = HttpContext.Current.Session["id"].ToString();


                        Query1 = "select id from status_messages_user_like";
                        Query1 += " where uid='" + id + "' and smid='" + ict.Table.Rows[i]["id"].ToString() + "';";
                        DataView ict_f_like = gc1.select_cmd(Query1); ;
                        if (ict_f_like.Count > 0)
                        {
                            check_li = true;
                        }
                    }
                }


                result_res += "</td>";
                result_res += "</tr>";
                result_res += "<tr>";
                //poster message like and share
                result_res += "<td width='15%' style='white-space: nowrap;' align='right'><br/><br/>";
                result_res += "<div style='cursor: pointer' class='likebox" + i + "'>";

                Image img1 = new Image();
                //if (check_li)
                //{
                //    string cutstr = "~/images/like.png";
                //    int ind = cutstr.IndexOf(@"/");
                //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                //    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:20px;width:20px;'>";
                //}
                //else
                //{
                //    string cutstr = "~/images/like_b.png";
                //    int ind = cutstr.IndexOf(@"/");
                //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                //    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:20px;width:20px;'>";
                //}
                //result_res += "<span style='color:#CCCCCC;font-size:10pt;'>いいね</span>";


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='likehidde" + i + "'>";
                //img1 = new Image();
                //if (check_li)
                //{
                //    string cutstr = "~/images/like_b.png";
                //    int ind = cutstr.IndexOf(@"/");
                //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                //    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:20px;width:20px;'>";
                //}
                //else
                //{
                //    string cutstr = "~/images/like.png";
                //    int ind = cutstr.IndexOf(@"/");
                //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                //    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:20px;width:20px;'>";

                //}
                //result_res += "<span style='color:#F06767;font-size:10pt;'>いいね</span>";
                result_res += "</div>";
                result_res += "</td>";




                result_res += "<td>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td align='center'><br/><br/>";
                result_res += "<div style='cursor: pointer' class='big_mess_box" + i + "'>";

                string cutstr_m = "~/images/mess_b.png";
                int ind_m = cutstr_m.IndexOf(@"/");
                string cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#CCCCCC;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='big_mess_hidde" + i + "'>";

                cutstr_m = "~/images/mess.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#676767;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "</td>";
                result_res += "<td align='left'><br/><br/>";
                result_res += "<div id='sharebox" + i + "' style='cursor: pointer'>";
                ////pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
                //cutstr_m = "~/images/share_b.png";
                //ind_m = cutstr_m.IndexOf(@"/");
                //cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                //result_res += "<img src='" + cutstr_m1 + "' style='height:20px;width:20px;'>";
                //result_res += "<span style='color:#CCCCCC;font-size:10pt;'>シェア</span>";

                result_res += "</div>";
//                int len = ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length;
//                if (ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length > 99)
//                {
//                    len = 99;
//                }
//                li = new Literal();
//                li.Text = @"
//                       <div id='share_div" + ((counn - 10) + i) + @"' title='シェア' style='display:none;'><table width='100%'><tr><td><div id='facebook_share" + ((counn - 10) + i) + @"' class='jssocials-share jssocials-share-facebook'><a href='#' class='jssocials-share-link'><i class='fa fa-facebook jssocials-share-logo'></i></a></div></div></td><td><div id='share_div_" + ((counn - 10) + i) + @"'></div></td></tr><tr><td colspan='2'><div id='share_div__" + ((counn - 10) + i) + @"'></div></td></tr></table>
//
//                       <script type='text/javascript'>
//  $(function() {
//$('#share_div_" + ((counn - 10) + i) + @"').jsSocials({
//            showLabel: false,
//            showCount: false,
//            shares: ['email', 'twitter', 'googleplus', 'linkedin'],
//            url: 'http://.jp/',
//            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
//            shareIn: 'popup'
//        });
//$('#share_div__" + ((counn - 10) + i) + @"').jsSocials({
//            showLabel: false,
//            showCount: false,
//            shares: ['pinterest', 'stumbleupon', 'whatsapp', 'telegram', 'line'],
//            url: 'http://.jp/',
//            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
//            shareIn: 'popup'
//        });
//$('#share_div" + ((counn - 10) + i) + @"').dialog({
//                autoOpen: false,
//                show: {
//                    effect: 'fold',
//                    duration: 100
//                },
//                hide: {
//                    effect: 'fold',
//                    duration: 100
//                }
//            });
//   $('#sharebox" + ((counn - 10) + i) + @"').on('click', function () {
//                $('#share_div" + i + @"').dialog('open');
//
//           });
//$('#facebook_share" + ((counn - 10) + i) + @"').on('click', function () {
//               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "") + @"”');
//
//           });
// });
//</script>
//
//    ";
//                result_res += li.Text;

                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";

                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</td>";
                result_res += "<td style='vertical-align: top';>";

                result_res += "</td>";
                result_res += "</tr>";

                result_res += "<tr style='background-color:#F6F7F9;'>";
                result_res += "<td></td>";
                result_res += "<td>";



                result_res += "<div class='status_message_box" +  i + "' style='background-color: #ffffff'>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "<td width='90%' height='5%'></td>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</div >";
                result_res += "<div class='status_message_hidde" +  i + "' style='background-color: #dddddd'>";


                result_res += "<table width='100%' align='left'>";
                //first space way
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "<td width='90%' height='5%'></td>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                //second space way
                result_res += "<td></td>";
                //someone answer
                result_res += "<td>";


                result_res += "<table width='100%' align='left'>";
                //who like this message
                result_res += "<tr>";
                result_res += "<td width='5%' height='10px'></td>";
                result_res += "<td width='90%' height='10px'></td>";
                result_res += "<td width='5%' height='10px'></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='90%'>";
                Query1 = "select b.username,b.id,a.id as smulid from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
                Query1 += " where a.smid=" + ict.Table.Rows[i]["id"].ToString() + "";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                DataView ict3 = gc1.select_cmd(Query1);
                if (ict3.Count > 2)
                {
                    cutstr_m = "~/images/like_b_1.png";
                    ind_m = cutstr_m.IndexOf(@"/");
                    cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                    result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                    for (int iii = 0; iii < 2; iii++)
                    {
                        result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

                        result_res += "、";
                    }
                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration: none;'>他" + (ict3.Count - 2) + "人</a>";


                    //hyy = new HyperLink();
                    //hyy.NavigateUrl = "javascript:void(0);";
                    //hyy.Target = "_blank";
                    //hyy.Text = "他" + (ict3.Count - 2) + "人";
                    //hyy.Font.Underline = false;
                    //pdn2.Controls.Add(hyy);

                }
                else if (ict3.Count > 0)
                {
                    cutstr_m = "~/images/like_b_1.png";
                    ind_m = cutstr_m.IndexOf(@"/");
                    cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                    result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                    for (int iii = 0; iii < ict3.Count; iii++)
                    {
                        result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

                        if (iii != ict3.Count - 1)
                        {
                            result_res += "、";
                        }
                    }
                }

                result_res += "<hr/>";
                result_res += "</td>";
                result_res += "<td width='5%'></td>";
                result_res += "</tr>";
                //who talk about this status message before
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='95%'>";



                Query1 = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level,c.uid";
                Query1 += " from status_messages as a inner join status_messages_user as c";
                Query1 += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                Query1 += " inner join status_messages_user_talk as e on e.smuid=c.id";
                Query1 += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + "";
                Query1 += " ORDER BY e.year desc,e.month desc,e.day desc,e.hour desc,e.minute desc,e.second desc;";
                ict3 = gc1.select_cmd(Query1);
                List<sorttalk> talk_list = new List<sorttalk>();
                sorttalk so = new sorttalk();
                for (int iy = 0; iy < ict3.Count; iy++)
                {
                    so = new sorttalk();
                    so.id = Convert.ToInt32(ict3.Table.Rows[iy]["id"].ToString());
                    so.level = Convert.ToInt32(ict3.Table.Rows[iy]["structure_level"].ToString());
                    so.point_id = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_message_id"].ToString());
                    so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_user_id"].ToString());
                    so.filename = ict3.Table.Rows[iy]["filename"].ToString();
                    so.mess = ict3.Table.Rows[iy]["message"].ToString();

                    if (ict3.Table.Rows[iy]["pointer_user_id"].ToString() == "0")
                    {

                        so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["uid"].ToString());
                        so.username = ict3.Table.Rows[iy]["username"].ToString();
                        so.photo = ict3.Table.Rows[iy]["photo"].ToString();
                    }
                    else
                    {

                        Query1 = "select username,photo from user_login";
                        Query1 += " where id=" + ict3.Table.Rows[iy]["pointer_user_id"].ToString() + ";";
                        DataView ict5 = gc1.select_cmd(Query1);
                        so.username = ict5.Table.Rows[0]["username"].ToString();
                        so.photo = ict5.Table.Rows[0]["photo"].ToString();
                    }
                    talk_list.Add(so);
                }

                Query1 = "select max(e.structure_level) as maxlevel";
                //Query1 = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
                Query1 += " from status_messages as a inner join status_messages_user as c";
                Query1 += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                Query1 += " inner join status_messages_user_talk as e on e.smuid=c.id";
                Query1 += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict4 = gc1.select_cmd(Query1);

                int maxlevel = 0;
                if (ict4.Table.Rows[0]["maxlevel"].ToString() != "")
                {
                    maxlevel = Convert.ToInt32(ict4.Table.Rows[0]["maxlevel"].ToString());
                }

                List<sorttalk> talk_list_tmp = new List<sorttalk>();
                so = new sorttalk();
                for (int ik = 0; ik < talk_list.Count; ik++)
                {
                    if (talk_list[ik].level == 0)
                    {
                        so = new sorttalk();
                        so.id = talk_list[ik].id;
                        so.level = talk_list[ik].level;
                        so.filename = talk_list[ik].filename;
                        so.mess = talk_list[ik].mess;
                        so.photo = talk_list[ik].photo;
                        so.point_id = talk_list[ik].point_id;
                        so.uid = talk_list[ik].uid;
                        so.username = talk_list[ik].username;
                        talk_list_tmp.Add(so);
                    }
                }
                talk_list.Sort((a, b) => a.id.CompareTo(b.id));
                for (int ik = 0; ik < talk_list.Count; ik++)
                {
                    for (int le = 1; le < maxlevel + 1; le++)
                    {
                        if (talk_list[ik].level == le)
                        {
                            so = new sorttalk();
                            so.id = talk_list[ik].id;
                            so.level = talk_list[ik].level;
                            so.filename = talk_list[ik].filename;
                            so.mess = talk_list[ik].mess;
                            so.photo = talk_list[ik].photo;
                            so.point_id = talk_list[ik].point_id;
                            so.uid = talk_list[ik].uid;
                            so.username = talk_list[ik].username;
                            for (int ikk = 0; ikk < talk_list_tmp.Count; ikk++)
                            {
                                if (talk_list_tmp[ikk].id == talk_list[ik].point_id)
                                {
                                    talk_list_tmp.Insert(ikk + 1, so);
                                }
                            }
                        }
                    }
                }
                Image img2 = new Image();
                if (ict3.Count > 1)
                {
                    //show div
                    result_res += "<div class='mess_box" + i + "'>";
                    result_res += "<table width='100%'>";
                    result_res += "<tr>";
                    result_res += "<td width='100%' align='left' colspan='2'>";

                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>以前のコメントを見る</a>";

                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "<tr>";
                    result_res += "<td width='10%' rowspan='2' valign='top'>";


                    result_res += "<div class='zoom-gallery'>";

                    cutstr2 = talk_list_tmp[0].photo;
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:32px;height:32px;'>";
                    result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                    result_res += "</a>";

                    result_res += "</div>";


                    result_res += "</td>";
                    result_res += "<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">";
                    result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[0].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[0].username.ToString() + "</a>";

                    result_res += "<br/>";
                    result_res += ict3.Table.Rows[0]["message"].ToString();
                    result_res += "<br/>";

                    if (talk_list_tmp[0].filename != "")
                    {
                        result_res += "<div class='zoom-gallery'>";

                        cutstr2 = talk_list_tmp[0].filename;
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                        result_res += "</a>";

                        result_res += "</div>";

                        result_res += "<br/>";

                    }

                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "<tr>";
                    result_res += "<td>";
                    //who talk about status message and who like

                    result_res += "&nbsp;&nbsp;";

                    //who like who answer post message
                    Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                    Query1 += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                    //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    DataView ict_who_like = gc1.select_cmd(Query1);
                    if (ict_who_like.Count > 0)
                    {
                        cutstr_m = "~/images/like_b_1.png";
                        ind_m = cutstr_m.IndexOf(@"/");
                        cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                        result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                        result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                    }
                    //who like who answer post message



                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "</table>";
                    result_res += "</div>";
                    //hidde message
                    result_res += "<div class='mess_hidde" +  i + "'>";
                    result_res += "<table width='100%'>";
                    for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                    {

                        result_res += "<table width='100%'>";
                        result_res += "<tr>";
                        int wid = (10 + (10 * talk_list_tmp[iiii].level));
                        if (wid > 90) { wid = 90; }
                        result_res += "<td width='" + wid + "%' align='right' rowspan='2' valign='top'>";

                        result_res += "<div class='zoom-gallery'>";

                        cutstr2 = talk_list_tmp[iiii].photo;
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                        result_res += "</a>";

                        result_res += "</div>";


                        result_res += "</td>";

                        result_res += "<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">";

                        result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";
                        result_res += "<br/>";
                        result_res += talk_list_tmp[iiii].mess.ToString();
                        result_res += "<br/>";

                        if (talk_list_tmp[iiii].filename.ToString() != "")
                        {
                            result_res += "<div class='zoom-gallery'>";

                            cutstr2 = talk_list_tmp[iiii].filename.ToString();
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>";
                            result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                            result_res += "</a>";

                            result_res += "</div>";
                            result_res += "<br/>";
                        }

                        result_res += "</td>";
                        result_res += "</tr>";
                        result_res += "<tr>";
                        result_res += "<td>";


                        //who talk about status message and who like
                        result_res += "&nbsp;&nbsp;";

                        //who like who answer post message
                        Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                        Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                        //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                        ict_who_like = gc1.select_cmd(Query1);
                        if (ict_who_like.Count > 0)
                        {
                            cutstr_m = "~/images/like_b_1.png";
                            ind_m = cutstr_m.IndexOf(@"/");
                            cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                            result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                            result_res += "<a id='" + "likecount" + talk_list_tmp[iiii].id + "' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                        }
                        //who like who answer post message

                        result_res += "</td>";
                        result_res += "</tr>";


                        result_res += "</table>";
                        result_res += "<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>";
                    }
                    result_res += "</div>";

                }
                else
                {
                    if (ict3.Count > 0)
                    {
                        for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                        {

                            result_res += "<table width='100%'>";
                            result_res += "<tr>";
                            int wid = (10 + (10 * talk_list_tmp[iiii].level));
                            if (wid > 90) { wid = 90; }
                            result_res += "<td width='" + wid + "%' align='right' rowspan='2' valign='top'>";


                            result_res += "<div class='zoom-gallery'>";

                            cutstr2 = talk_list_tmp[iiii].photo;
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>";
                            result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                            result_res += "</a>";

                            result_res += "</div>";

                            result_res += "</td>";

                            result_res += "<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">";
                            result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";

                            result_res += "<br/>";
                            result_res += talk_list_tmp[iiii].mess.ToString();
                            result_res += "<br/>";

                            if (talk_list_tmp[iiii].filename.ToString() != "")
                            {
                                result_res += "<div class='zoom-gallery'>";

                                cutstr2 = talk_list_tmp[iiii].filename.ToString();
                                ind2 = cutstr2.IndexOf(@"/");
                                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>";
                                result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                                result_res += "</a>";

                                result_res += "</div>";
                                result_res += "<br/>";
                            }

                            result_res += "</td>";
                            result_res += "</tr>";
                            result_res += "<tr>";
                            result_res += "<td>";


                            //who talk about status message and who like

                            result_res += "&nbsp;&nbsp;";

                            //who like who answer post message
                            Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                            Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                            //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                            DataView ict_who_like = gc1.select_cmd(Query1);
                            if (ict_who_like.Count > 0)
                            {
                                cutstr_m = "~/images/like_b_1.png";
                                ind_m = cutstr_m.IndexOf(@"/");
                                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                                result_res += "<a id='" + "likecount" + talk_list_tmp[iiii].id + "' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                            }
                            //who like who answer post message

                            result_res += "</td>";
                            result_res += "</tr>";


                            result_res += "</table>";

                            //user answer user answer
                            result_res += "<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>";



                        }
                    }
                }

                result_res += "</td>";
                result_res += "<td width='5%'></td>";
                result_res += "</tr>";


                result_res += "</table>";


                result_res += "</td>";
                //second space way
                result_res += "<td></td>";
                result_res += "</tr>";
                //third space way
                result_res += "<tr>";
                result_res += "<td></td>";
                result_res += "<td>";

                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='10%' valign='top'>";
                //user photo

                result_res += "<div class='zoom-gallery'>";

                SqlDataSource sql3 = new SqlDataSource();
                //sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //sql3.SelectCommand = "select photo,username from user_login ";
                //sql3.SelectCommand += " where id='" + HttpContext.Current.Session["id"].ToString() + "';";
                //DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
                //string userr = "";
                //if (ict2.Count > 0)
                //{
                //    cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                //    ind2 = cutstr2.IndexOf(@"/");
                //    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                //    userr = ict2.Table.Rows[0]["username"].ToString();
                //}

                //result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:32px;height:32px;'>";
                //result_res += "<img src='" + cutstr3 + "' width='32' height='32' />";
                //result_res += "</a>";

                result_res += "</div>";

                result_res += "</td>";
                result_res += "<td width='85%'>";

                //user answer
//                result_res += "<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_" + ((counn - 10) + i) + "' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;'>";


//                result_res += @"
//<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
//            <input type='file' name='file' id='btnFileUpload" + ((counn - 10) + i) + @"' />
//</label>
//<br />
//            <div id='progressbar" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
//                <div>
//                    読み込み中
//                </div>
//            </div>
//<br />
//                <div id='image_place" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
//                    <div>
//                        <img id='make-image" + ((counn - 10) + i) + @"' alt='' src='' width='100px' height='100px'/>
//                    </div>
//                </div>
//";




                //pdn2.Controls.Add(new LiteralControl(@"<label class='file-upload'><span><strong>アップロード</strong></span>"));

                //FileUpload fi=new FileUpload();
                //fi.ID="fuDocument_"+i;
                //fi.Attributes.Add("onchange", "UploadFile(this,this.id);");
                //pdn2.Controls.Add(fi);
                //pdn2.Controls.Add(new LiteralControl(@"</label><br />"));



                //Button but = new Button();
                //but.ID = "btnUploadDoc_" + i;
                //but.Text = "Upload";
                //but.Click += new System.EventHandler(this.UploadDocument);
                //but.OnClientClick = "ShowProgressBar();";
                //but.Style["display"] = "none";
                //pdn2.Controls.Add(but);

                //img1 = new Image();
                //img1.Width = 100; img1.Height = 150;
                //img1.ID = "Image_" + i;
                //img1.Visible = false;


                //pdn2.Controls.Add(img1);



                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";


                result_res += "</div>";


                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";
                //fourth space way
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'><br/></td>";
                result_res += "<td width='90%' height='5%'><br/></td>";
                result_res += "<td width='5%' height='5%'><br/></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";



                result_res += "</table>";
                //pdn2.Controls.Add(new LiteralControl("<br/><br/>"));
                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";



                result_res += "</div>";
            }
            result_res += javascr;




        return result_res;
    }
    public static bool UrlExists(string url)
    {
        try
        {
            new System.Net.WebClient().DownloadData(url);
            return true;
        }
        catch (System.Net.WebException e)
        {
            return false;
            throw;
        }
    }
    public class URL_data
    {
        public string url = "";
        public string image_url = "";
        public string title = "";
        public string des = "";
        public string update_time = "";
    }
    public static string ConvertUrlsToDIV(string url)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string res = "";
        Query1 = "select * from status_messages_link_info where link like '" + url + "';";
        DataView ict1 = gc1.select_cmd(Query1);
        if (ict1.Count > 0)
        {
            string sharetxt = "";
            if (ict1.Table.Rows[0]["title"].ToString() != "")
            {
                sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;line-height:30px;'>【" + ict1.Table.Rows[0]["title"].ToString() + "】</span>";
            }
            if (ict1.Table.Rows[0]["des"].ToString() != "")
            {
                sharetxt += "<br/><span style='font-size:medium;color:black;line-height:27px;'>" + ict1.Table.Rows[0]["des"].ToString() + "</span>";
            }
            res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + ict1.Table.Rows[0]["link"].ToString() + "' style='text-decoration:none'>";
            if (ict1.Table.Rows[0]["image_url"].ToString() != "")
            {
                res += "<img class='lazy' data-src='" + ict1.Table.Rows[0]["image_url"].ToString() + "' src='images/loading.gif' alt='' width='100%' height='200px' border='0' />";
            }
            if (sharetxt != "")
            {
                res += sharetxt;
            }
            res += "</a></div>";

        }
        else
        {
            WebService.LinkDetails wss = new WebService.LinkDetails();
            WebService ws = new WebService();
            wss = ws.GetDetails(url);
            string imgurl = "";
            if (wss.Image != null)
            {
                imgurl = wss.Image.Url;
            }
            else if (wss.Images != null)
            {
                if (wss.Images.Count > 0)
                {
                    imgurl = wss.Images[0].Url;
                }
            }
            URL_data urld = new URL_data();
            urld.url = wss.Url;
            string sharetxt = "";
            if (wss.Title != null)
            {
                urld.title = wss.Title;
                sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;line-height:30px;'>【" + wss.Title + "】</span>";
            }
            if (wss.Description != null)
            {
                urld.des = wss.Description;
                sharetxt += "<br/><span style='font-size:medium;color:black;line-height:27px;'>" + wss.Description + "</span>";
            }

            res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + wss.Url + "' style='text-decoration:none'>";
            if (imgurl != "")
            {
                if (UrlExists(imgurl))
                {
                    urld.image_url = imgurl;
                    res += "<img class='lazy' data-src='" + imgurl + "' src='images/loading.gif' alt='' width='100%' height='200px' border='0' />";
                }
            }
            if (sharetxt != "")
            {
                res += sharetxt;
            }
            res += "</a></div>";

            Query1 = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
            Query1 += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";
            resin = gc1.insert_cmd(Query1);

        }

        return res;
    }
    public static string ConvertUrlsToLinks_DIV(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        string txt = "";
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            txt += ConvertUrlsToDIV(match.Value);
        }
        return txt;
    }
    public static string ConvertUrlsToLinks(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);

        MatchCollection mactches = r.Matches(msg);
        string txt = "";
        foreach (Match match in mactches)
        {
            //txt += GetMetaTagValue(match.Value) + ",";
            msg = msg.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
        }
        return msg;
        //return txt;

        //        msg = Regex.Replace(
        //msg,
        //@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])",
        //delegate(Match match)
        //{
        //    return GetMetaTagValue(match.ToString());

        //    //return string.Format("{0}", match.ToString());
        //});

        //        return msg;

        //return r.Replace(msg, "$1");

        //return GetMetaTagValue(r.Replace(msg, "$1"));

        //return r.Replace(msg, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
    }
    public static string GetMetaTagValue(string url)
    {
        string res = "";
        //Get Title
        WebClient x = new WebClient();
        string source = x.DownloadString(url);
        res = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        return res;
    }
}
