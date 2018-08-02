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
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using MySql.Data.MySqlClient;
namespace Amazon.S3.Transfer { };

public partial class main : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!Page.ClientScript.IsStartupScriptRegistered("alert"))
        {
            if (Session["id"] == null || Session["id"].ToString() == "")
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "showDialog_login();", true);
            }
        }

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            Session["id"] = "";
        }
        Session["top_count"] = 10;

        ///////
        Panel pdn_j = (Panel)this.FindControl("javaplace_formap");
        pdn_j.Controls.Clear();
        List<string> postal_code_list = new List<string>();
        List<string> check_pos = new List<string>();

        //find poster GPS
        Query = "select postal_code";
        Query += " from user_login_address";
        Query += " where uid='" + Session["id"].ToString() + "';";
        DataView ict_place = gc.select_cmd(Query);
        int couuu = 0;
        Literal lip = new Literal();
        Literal lip1 = new Literal();
        Literal lip2 = new Literal();
        if (ict_place.Count > 0)
        {

            lip1.Text = "";
            lip2.Text = "";
            lip2.Text += @"var bounds = new google.maps.LatLngBounds();";
            for (int i = 0; i < ict_place.Count; i++)
            {
                postal_code_list.Add(ict_place.Table.Rows[i]["postal_code"].ToString());

                string result = "";
                var url = new Uri("https://postcode.teraren.com/postcodes/" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[i]["postal_code"].ToString().Replace("-", "")) + ".json");


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
            }

        }
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


        if (Session["id"].ToString() != "")
        {
            //search friends
            Query = "select c.id,c.username,c.photo";
            Query += " from user_friendship as a";
            Query += " inner join user_login as b on b.id=a.first_uid";
            Query +=" inner join user_login as c on c.id=a.second_uid";
            Query += " where b.id='" + Session["id"].ToString()+"'";
            Query += " and first_check_connect=1 and second_check_connect=1;";

            DataView ict_sf = gc.select_cmd(Query);

            List<string> fri = new List<string>();

            for (int i = 0; i < ict_sf.Count; i++)
            {
                fri.Add(ict_sf.Table.Rows[i]["id"].ToString());
            }

            Query = "select b.id,b.username,b.photo";
            Query += " from user_friendship as a";
            Query += " inner join user_login as b on b.id=a.first_uid";
            Query += " inner join user_login as c on c.id=a.second_uid";
            Query += " where c.id='" + Session["id"].ToString() + "'";
            Query += " and first_check_connect=1 and second_check_connect=1;";

            DataView ict_f1 = gc.select_cmd(Query);

            for (int i = 0; i < ict_f1.Count; i++)
            {
                fri.Add(ict_f1.Table.Rows[i]["id"].ToString());
            }
            //search friends
            //friend only type message
            List<string> not_friend = new List<string>();
            Query = "select id";
            Query += " from status_messages";
            Query += " where type='2'";

            if (fri.Count > 0)
            {
                Query += " and uid not in (select uid from status_messages where type='2'";
                Query += " and (uid='" + fri[0] + "'";
                for (int iff = 1; iff < fri.Count; iff++)
                {
                    Query += " or uid='" + fri[iff] + "'";
                }
                Query += "))";
            }
            DataView ict_not_friend = gc.select_cmd(Query);
            if (ict_not_friend.Count > 0)
            {
                for (int inot = 0; inot < ict_not_friend.Count; inot++)
                {
                    not_friend.Add(ict_not_friend.Table.Rows[inot]["id"].ToString());
                }
            }


            //friend only type message
            //like counter
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
            /////
            int totalran = Convert.ToInt32(Session["top_count"].ToString());
            //string addstr = " and a.id not in (select t.id from (select a.id from status_messages as a use index (IX_status_messages_1)";
            //addstr += " inner join user_login as b on b.id=a.uid where 1=1";
            Query = "select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query += "from status_messages as a use index (IX_status_messages_1)";
            Query += " inner join user_login as b on b.id=a.uid where 1=1";

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
                        //addstr += " and a.message_type='" + Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                //addstr += " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                    //addstr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                //addstr += ")";
                Query += qustr;
            }
            //not friend
            if (not_friend.Count > 0)
            {
                string qustr = " and ( a.id!='" + not_friend[0] + "'";
                //addstr += " and ( a.id!='" + not_friend[0] + "'";
                for (int i = 1; i < not_friend.Count; i++)
                {
                    qustr += " or a.id!='" + not_friend[i] + "'";
                    //addstr += " or a.id!='" + not_friend[i] + "'";
                }
                qustr += ")";
                //addstr += ")";
                Query += qustr;
            }
            //not friend
            if (check_sort_pop)
            {
                //addstr += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query += addstr;
                Query += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran-10) +",10;" ;
            }
            else
            {
                //addstr += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query += addstr;
                Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
            }
            ict_place = gc.select_cmd(Query);

            ////count max post

            Query = "select a.id ";
            Query += "from status_messages as a use index (IX_status_messages_1)";
            Query += " inner join user_login as b on b.id=a.uid where 1=1";
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
            //not friend
            if (not_friend.Count > 0)
            {
                string qustr = " and ( a.id!='" + not_friend[0] + "'";
                for (int i = 1; i < not_friend.Count; i++)
                {
                    qustr += " or a.id!='" + not_friend[i] + "'";
                }
                qustr += ")";
                Query += qustr;
            }
            //not friend
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
                        metype = "授乳室、";
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


                    content = '"' + @"<div><table width='100%'><tr><td width='20%' valign='top'><img src='" + cutstr3 + @"' height='50px' width='50px'></td><td width='80%' valign='top' style='word-wrap: break-word;'><a href='user_home_friend.aspx?=" + ict_place.Table.Rows[sorst_list1[i].id]["uid"].ToString() + @"' style='text-decoration: none;'>" + ict_place.Table.Rows[sorst_list1[i].id]["username"].ToString() + @"</a><br/><span style='color:#CCCCCC;'>" + ict_place.Table.Rows[sorst_list1[i].id]["year"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["month"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span><br/><span>" + mess + @"</span><br/></td></tr></table></div>" + '"';
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
                            string result = "";

                            var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString());

                            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                            using (var response = request.GetResponse())
                            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                result = sr.ReadToEnd();
                            }

                            JObject jArray = JObject.Parse(result);
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

            //addstr = " and a.id not in (select t.id from (select a.id from status_messages as a use index (IX_status_messages_1)";
            //addstr += " inner join user_login as b on b.id=a.uid where 1=1";

            Query = " select a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query += "from status_messages as a use index (IX_status_messages_1)";
            Query += " inner join user_login as b on b.id=a.uid where 1=1";

            //if want to class by type use type=0,1,2 ; message_type=0,1,2

            ////Before today's message
            //Query1 += " where a.year<=" + DateTime.Now.Year.ToString() + " and a.month<=" + DateTime.Now.Month.ToString();
            //Query1 += " and a.day<=" + DateTime.Now.Day.ToString() + " ";


            ////type message select
            if (Session["message_type"] != null)
            {
                if (Session["message_type"].ToString() != "")
                {
                    if (Session["message_type"].ToString() != "1")
                    {
                        Query += " and a.message_type='" + Session["message_type"].ToString() + "'";
                        //addstr += " and a.message_type='" + Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                //addstr += " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                    //addstr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                //addstr += ")";
                Query += qustr;
            }
            //not friend
            if (not_friend.Count > 0)
            {
                string qustr = " and ( a.id!='" + not_friend[0] + "'";
               // addstr += " and ( a.id!='" + not_friend[0] + "'";
                for (int i = 1; i < not_friend.Count; i++)
                {
                    qustr += " or a.id!='" + not_friend[i] + "'";
                    //addstr += " or a.id!='" + not_friend[i] + "'";
                }
                qustr += ")";
                //addstr += ")";
                Query += qustr;
            }
            //not friend
            if (check_sort_pop)
            {
                //addstr += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query += addstr;
                Query += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
            }
            else
            {
               // addstr += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query += addstr;
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
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
            })

            $('.likehidde" + i + @"').click(function () {
                $('.likebox" + i + @"').toggle();
                $('.likehidde" + i + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
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
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",1);
            })

            $('.big_mess_hidde" + i + @"').click(function () {
                $('.big_mess_box" + i + @"').toggle();
                $('.big_mess_hidde" + i + @"').toggle(false);
                $('.status_message_box" + i + @"').toggle();
                $('.status_message_hidde" + i + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",1);
            })

            $('.status_message_hidde" + i + @"').toggle(false);


";

//                SqlDataSource sql3 = new SqlDataSource();
//                sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                Query1 = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                DataView ict2 = gc1.select_cmd(Query1);;
//                if (ict2.Count > 3)
//                {
//                    li.Text += @"
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
//                }
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
                hy.NavigateUrl = "~/user_home_friend.aspx?=" + ict.Table.Rows[i]["uid"].ToString();
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
                    la.Text += "授乳室、";
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
                string shareimg = "";
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
                        if (shareimg == "")
                        {
                            shareimg = cutstr1;
                        }
                        ////test image grid
                        //if (ii > 5)
                        //{
                        //    pdn2.Controls.Add(new LiteralControl("<div style='visibility:hidden;'>"));
                        //    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:0px; height:0px;outline : none;'>"));
                        //    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' width='0px' height='0px'/>"));
                        //    pdn2.Controls.Add(new LiteralControl("</a>"));
                        //    pdn2.Controls.Add(new LiteralControl("</div>"));
                        //}
                        //else
                        //{

                        //    Random rand = new Random(Guid.NewGuid().GetHashCode());

                        //    int w = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));
                        //    rand = new Random(Guid.NewGuid().GetHashCode());
                        //    int h = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));
                        //    pdn2.Controls.Add(new LiteralControl("<div class='cell' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px; background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                        //    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px;outline : none;'>"));
                        //    pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' width='" + (w * 100) + "px' height='" + (h * 100) + "px'/>"));

                        //    pdn2.Controls.Add(new LiteralControl("</a>"));
                        //    pdn2.Controls.Add(new LiteralControl("</div>"));
                        //}


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
                        if (shareimg == "")
                        {
                            shareimg = cutstr1;
                        }
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

                                        var wall"+i+@" = new Freewall('#freewall" + i + @"');
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
                Label laa = new Label();
                laa.Font.Size = FontUnit.Point(10);
                laa.Text = "いいね";
                Image img1 = new Image();
                if (check_li)
                {
                    img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like.png";
                    img1.Attributes["onclick"] = "like(this.id)";
                    laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
                    laa.ID = "lalike_but" + ict.Table.Rows[i]["id"].ToString();
                    laa.Attributes["onclick"] = "like(this.id)";
                }
                else
                {
                    img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like_b.png";
                    img1.Attributes["onclick"] = "blike(this.id)";
                    laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                    laa.ID = "lablike_but" + ict.Table.Rows[i]["id"].ToString();
                    laa.Attributes["onclick"] = "blike(this.id)";

                }
                pdn2.Controls.Add(img1);

                pdn2.Controls.Add(laa);

                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likehidde" + i + "'>"));
                img1 = new Image();
                laa = new Label();
                laa.Font.Size = FontUnit.Point(10);
                laa.Text = "いいね";
                if (check_li)
                {

                    img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like_b.png";
                    img1.Attributes["onclick"] = "blike(this.id)";
                    laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                    laa.ID = "lablike_but" + ict.Table.Rows[i]["id"].ToString();
                    laa.Attributes["onclick"] = "blike(this.id)";

                }
                else
                {
                    img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like.png";
                    img1.Attributes["onclick"] = "like(this.id)";
                    laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
                    laa.ID = "lalike_but" + ict.Table.Rows[i]["id"].ToString();
                    laa.Attributes["onclick"] = "like(this.id)";

                }
                pdn2.Controls.Add(img1);

                pdn2.Controls.Add(laa);
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
                laa = new Label();
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
                pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' style='cursor: pointer'>"));
                //pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
                img1 = new Image();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/share_b.png";
                pdn2.Controls.Add(img1);
                laa = new Label();

                laa.Font.Size = FontUnit.Point(10);
                laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                laa.Text = "シェア";
                pdn2.Controls.Add(laa);

                pdn2.Controls.Add(new LiteralControl("</div>"));
                int len = ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length;
                if (ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length > 99)
                {
                    len = 99;
                }
                li = new Literal();
                li.Text = @"
                       <div id='share_div" + i + @"' title='シェア' style='display:none;'><table width='100%'><tr><td><div id='facebook_share" + i + @"' class='jssocials-share jssocials-share-facebook'><a href='#' class='jssocials-share-link'><i class='fa fa-facebook jssocials-share-logo'></i></a></div></div></td><td><div id='share_div_" + i + @"'></div></td></tr><tr><td colspan='2'><div id='share_div__" + i + @"'></div></td></tr></table>

                       <script type='text/javascript'>
  $(function() {
$('#share_div_" + i + @"').jsSocials({
            showLabel: false,
            showCount: false,
            shares: ['email', 'twitter', 'googleplus', 'line'],
            url: 'http://.jp/',
            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
            shareIn: 'popup'
        });
$('#share_div" + i + @"').dialog({
                autoOpen: false,
                show: {
                    effect: 'fold',
                    duration: 100
                },
                hide: {
                    effect: 'fold',
                    duration: 100
                }
            });
   $('#sharebox" + i + @"').on('click', function () {
                $('#share_div" + i + @"').dialog('open');
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",2);

           });
$('#facebook_share" + i + @"').on('click', function () {
               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "") + @"”');

           });
 });
</script>

    ";
                pdn2.Controls.Add(li);
                //facebook
                //                li = new Literal();
                //                li.Text = @"
                //
                //                       <script type='text/javascript'>
                //  $(function() {
                //   $('#sharebox" + i + @"').on('click', function () {
                //               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString() + @"”');
                //
                //           });
                // });
                //</script>
                //
                //    ";
                //                pdn2.Controls.Add(li);

                //                li = new Literal();
                //                li.Text = @"
                //
                //                       <script type='text/javascript'>
                //  $(document).ready(function() {
                //    Tipped.create('#sharebox" + i + @"'," + '"' + "<div id = 'html-content" + i + @"'></div><script type='text/javascript'> $('#html-content" + i + @"').jsSocials({ showLabel: false, showCount: false, shares: ['email', 'twitter', 'facebook', 'googleplus', 'linkedin', 'pinterest', 'stumbleupon', 'whatsapp', 'telegram', 'line'], url: 'http://.jp/', text: '地域のいい情報をGETしました！', shareIn: 'popup' }); </script>" + '"' + @"
                //,{
                //  position: 'bottomleft'
                //});
                //
                //  });
                //
                //</script>
                //    ";
                //                pdn2.Controls.Add(li);
                //                li = new Literal();
                //                li.Text = @"
                //
                //                       <script type='text/javascript'>
                //        $('#html-content" + i + @"').jsSocials({
                //            showLabel: false,
                //            showCount: false,
                //            shares: ['email', 'twitter', 'facebook', 'googleplus', 'linkedin', 'pinterest', 'stumbleupon', 'whatsapp', 'telegram', 'line'],
                //            url: 'http://.jp/',
                //            text: '地域のいい情報をGETしました！',
                //            shareIn: 'popup'
                //        });
                //</script>
                //    ";
                //                pdn2.Controls.Add(li);

                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("</tr>"));
                pdn2.Controls.Add(new LiteralControl("</table>"));

                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("<td></td>"));
                pdn2.Controls.Add(new LiteralControl("</tr>"));
                pdn2.Controls.Add(new LiteralControl("</table>"));
                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("<td style='vertical-align: top';>"));

                //report
                Image report = new Image();
                report.ID = "reportstate_" + ict.Table.Rows[i]["id"].ToString();
                report.ImageUrl = "~/images/report.png";
                report.Style.Add("cursor", "pointer");
                report.Attributes["onclick"] = "report_mess(this.id)";
                li = new Literal();

                //report div
                li.Text = @"
<div id='dlgbox_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlg'>
            <div id='dlg-header_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgh'>問題の内容についてお聞かせください</div>
            <div id='dlg-body_report_" + ict.Table.Rows[i]["id"].ToString() + @"' style='height: 200px; overflow: auto' class='dlgb'>
                <table style=' width: 100%;'>
                    <tr>
                        <td>
                            <table  class='report_dlg'  style='width: 100%;'>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span style='font-weight: bold;font-size:medium;'>詳細を入力してください。</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='不快または面白くない' style='margin-right: 15px;'> <span style='font-size:medium;'>不快または面白くない</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='に載せるべきではないと思う' style='margin-right: 15px;'><span style='font-size:medium;'>に載せるべきではないと思う</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='スパムである' style='margin-right: 15px;'><span style='font-size:medium;'>スパムである</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span id='reportla_" + ict.Table.Rows[i]["id"].ToString() + @"' style='color:red;font-size:medium;'></span><br/>
                                    </td>
                                </tr>
                            </table>
                            <hr/>
                        </td>
</tr>
                </table>
            </div>
            <div id='dlg-footer_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgf'>
<table style=' width: 100%;'>
<tr>
<td width='50%' align='left'>
<input id='reportstatebutcancel_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='取り消す' onclick='dlgrecanel(this.id)' class='file-upload1'/>
</td>
<td width='50%' align='right'>
                <input id='reportstatebutedit_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='保存' onclick='dlgreport(this.id)' class='file-upload1'/>
</td>
            </tr>
</table>
</div>
        </div>
";

                pdn2.Controls.Add(report);
                pdn2.Controls.Add(li);

                //report
                pdn2.Controls.Add(new LiteralControl("</td>"));
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
                        hyy.NavigateUrl = "~/user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString();
                        hyy.Target = "_blank";
                        hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                        hyy.Font.Underline = false;

                        pdn2.Controls.Add(hyy);
                        pdn2.Controls.Add(new LiteralControl("、"));
                    }
                    pdn2.Controls.Add(new LiteralControl("<a id='listlike_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='check_like_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;'>他" + (ict3.Count - 2) + "人</a>"));


                    //hyy = new HyperLink();
                    //hyy.NavigateUrl = "javascript:void(0);";
                    //hyy.Target = "_blank";
                    //hyy.Text = "他" + (ict3.Count - 2) + "人";
                    //hyy.Font.Underline = false;
                    //pdn2.Controls.Add(hyy);

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
                        hyy.NavigateUrl = "~/user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString();
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
                Query +=" on a.id=c.smid inner join user_login as b on b.id=c.uid";
                Query +=" inner join status_messages_user_talk as e on e.smuid=c.id";
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
                    hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[0].uid.ToString();
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

                    hyy = new HyperLink();
                    hyy.ID = "wholike_" + talk_list_tmp[0].id + "_s";
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "いいね!";

                    Query = "select good_status from status_messages_user_talk_like";
                    Query += " where smutid='" + talk_list_tmp[0].id + "' and uid='" + Session["id"].ToString() + "';";

                    DataView ict_who_like = gc.select_cmd(Query);
                    if (ict_who_like.Count > 0)
                    {
                        if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                        {
                            hyy.Style.Add("color", "#4183C4");
                            hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                        }
                        else
                        {
                            hyy.Style.Add("color", "#D84C4B");
                            hyy.Attributes["onclick"] = "slike_who_answer(this.id)";
                        }
                    }
                    else
                    {
                        hyy.Style.Add("color", "#4183C4");
                        hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                    }
                    pdn2.Controls.Add(hyy);


                    pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "返信";
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);

                    //who like who answer post message

                    Query = "select count(*) as howmany from status_messages_user_talk_like";
                    Query += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                    //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";

                    ict_who_like = gc.select_cmd(Query);
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
                        hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString();
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

                        hyy = new HyperLink();
                        hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "いいね!";

                        Query = "select good_status from status_messages_user_talk_like";
                        Query += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + Session["id"].ToString() + "';";

                        ict_who_like = gc.select_cmd(Query);
                        if (ict_who_like.Count > 0)
                        {
                            if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                            {
                                hyy.Style.Add("color", "#4183C4");
                                hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                            }
                            else
                            {
                                hyy.Style.Add("color", "#D84C4B");
                                hyy.Attributes["onclick"] = "like_who_answer(this.id)";
                            }
                        }
                        else
                        {
                            hyy.Style.Add("color", "#4183C4");
                            hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                        }
                        pdn2.Controls.Add(hyy);
                        pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        hyy = new HyperLink();
                        hyy.ID = "whowantans_" + talk_list_tmp[iiii].id;
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "返信";
                        hyy.Attributes["onclick"] = "who_answer(this.id)";
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);

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
                            hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString();
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
                            hyy = new HyperLink();
                            hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = "いいね!";

                            Query = "select good_status from status_messages_user_talk_like";
                            Query += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + Session["id"].ToString() + "';";
                            DataView ict_who_like = gc.select_cmd(Query);
                            if (ict_who_like.Count > 0)
                            {
                                if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                                {
                                    hyy.Style.Add("color", "#4183C4");
                                    hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                                }
                                else
                                {
                                    hyy.Style.Add("color", "#D84C4B");
                                    hyy.Attributes["onclick"] = "like_who_answer(this.id)";
                                }
                            }
                            else
                            {
                                hyy.Style.Add("color", "#4183C4");
                                hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                            }
                            pdn2.Controls.Add(hyy);
                            pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                            hyy = new HyperLink();
                            hyy.ID = "whowantans_" + talk_list_tmp[iiii].id;
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = "返信";
                            hyy.Attributes["onclick"] = "who_answer(this.id)";
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);

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


                Query = "select photo,username from user_login ";
                Query += " where id='" + Session["id"].ToString() + "';";
                DataView ict2 = gc.select_cmd(Query);
                string userr = "";
                if (ict2.Count > 0)
                {
                    cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    userr = ict2.Table.Rows[0]["username"].ToString();
                }

                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:32px;height:32px;'>"));
                pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='32' height='32' />"));
                pdn2.Controls.Add(new LiteralControl("</a>"));

                pdn2.Controls.Add(new LiteralControl("</div>"));

                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("<td width='85%'>"));

                //user answer
                pdn2.Controls.Add(new LiteralControl("<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_" + i + "' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;' title='【Enter】キーを押してください'>"));
                //TextBox tex = new TextBox();
                //tex.Width = Unit.Percentage(95);
                //tex.Height = 30;
                //tex.ID = "send" + ict.Table.Rows[i]["id"].ToString();
                //tex.Attributes["onKeydown"] = "Javascript: if (event.which == 13 || event.keyCode == 13) sendmessage(this.id);";
                //tex.Attributes.Add("placeholder", "コメントする");
                //pdn2.Controls.Add(tex);

                //pdn2.Controls.Add(new LiteralControl("<br/>"));

                pdn2.Controls.Add(new LiteralControl(@"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload" + i + @"' />
</label>
<br />
            <div id='progressbar" + i + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place" + i + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image" + i + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>
"));




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


            //Query1 = "select date,starttime,endtime from fordate where date='" + startd + "' and starttime<'" + start + "' and endtime>'" + start + "';";
            //DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
            //this.form1.Controls.Add(sql1);
            bool check_sup = true;
            if (Session["id"] != null)
            {


                Query = "select * from user_information_store where uid='" + Session["id"].ToString() + "';";
                //sql_list.SelectCommand += " where id=" + Session["id"].ToString() + ";";
                DataView ict_check_sup = gc.select_cmd(Query);
                if (ict_check_sup.Count > 0)
                {
                    check_sup = false;
                }
            }
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
                //message_type0_Button.Style.Add("background-color", "transparent");
                //message_type1_Button.Style.Add("background-color", "transparent");
                //message_type2_Button.Style.Add("background-color", "transparent");
                //message_type3_Button.Style.Add("background-color", "transparent");
                //message_type4_Button.Style.Add("background-color", "transparent");
                //message_type5_Button.Style.Add("background-color", "transparent");
            }




            Panel pdn_list = new Panel();
            pdn_list = (Panel)FindControl("Panel_for_support_list");
            pdn_list.Controls.Clear();
            li = new Literal();

            li.Text = @"<script>
$(function () {
";
            li.Text += @"
$('#select_1').dropdown();
$('#select_2').dropdown();
$('#select_3').dropdown();
$('#select_4').dropdown();
";

            li.Text += @"
                        });";
            li.Text += @"</script>";

            pdn_list.Controls.Add(li);
            pdn_list.Controls.Add(new LiteralControl("<table align='center' width='100%' height='100%' style='background-color:#D0D0D0;'>"));
            pdn_list.Controls.Add(new LiteralControl("<tr>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("</tr>"));
            //select search
            pdn_list.Controls.Add(new LiteralControl("<tr>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            li = new Literal();
            li.Text = @" <select name='search_type_1' class='ui dropdown' id='select_1'>
     <option value=" + '"' + '"' + @">日付</option>
      <option value='0'>お食事</option>
</select>";
            pdn_list.Controls.Add(li);

            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            li = new Literal();
            li.Text = @" <select name='search_type_2' class='ui dropdown' id='select_2'>
     <option value=" + '"' + '"' + @">預け時刻</option>
      <option value='0'>お食事</option>
</select>";
            pdn_list.Controls.Add(li);
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            li = new Literal();
            li.Text = @" <select name='search_type_3' class='ui dropdown' id='select_3'>
     <option value=" + '"' + '"' + @">お迎え時刻</option>
      <option value='0'>お食事</option>
</select>";
            pdn_list.Controls.Add(li);
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            li = new Literal();
            li.Text = @" <select name='search_type_4' class='ui dropdown' id='select_4'>
     <option value=" + '"' + '"' + @">依頼内容</option>
      <option value='0'>お食事</option>
</select>";
            pdn_list.Controls.Add(li);
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            Button but_sea = new Button();
            but_sea.ID = "search_list_button";
            but_sea.CssClass = "file-upload1";
            but_sea.Text = "探す";
            pdn_list.Controls.Add(but_sea);
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("<td>"));
            pdn_list.Controls.Add(new LiteralControl("</td>"));
            pdn_list.Controls.Add(new LiteralControl("</tr>"));
            //select search
            pdn_list.Controls.Add(new LiteralControl("<tr>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='3%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='22%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='18%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("<td width='3%' height='5%'></td>"));
            pdn_list.Controls.Add(new LiteralControl("</tr>"));
            pdn_list.Controls.Add(new LiteralControl("</table>"));


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

    //protected void UploadDocumentToAWS(object sender, EventArgs e)
    //{
    //    string accessKey = "AKIAJ3M3PCXJGLTDFGJA";
    //    string secretKey = "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";
    //    AmazonS3Config asConfig = new AmazonS3Config()
    //    {
    //        ServiceURL = "s3-us-west-2.amazonaws.com",
    //    };
    //    IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, asConfig);
    //    TransferUtility utility = new TransferUtility(client);
    //    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

    //}

    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument.HasFile)
        {
            foreach (HttpPostedFile postedFile in fuDocument.PostedFiles)
            {
                input = fuDocument.FileName;
                stringindex = input.LastIndexOf(@".");
                cut = input.Length - stringindex;
                DirRoot = input.Substring(stringindex + 1, cut - 1);


                Query = "select id,name from filename_extension";

                DataView ou1 = gc.select_cmd(Query);
                for (int i = 0; i < ou1.Count; i++)
                {
                    if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
                    {
                        check = true;
                    }
                }
                if (check)
                {
                    int fileSize = postedFile.ContentLength;

                    // Allow only files less than (16 MB)=16777216 bytes to be uploaded.
                    if (fileSize < 16777216)
                    {

                        filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                        Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                        string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                        //AmazonUpload aws = new AmazonUpload();
                        //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = imgurl;
                        this.Panel1.Controls.Add(im);

                        image_HiddenField.Value += ",~/" + imgurl;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('File is out of memory 16MB!')", true);
                    }


                }
                else
                {
                    ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('filename extension is not in role!')", true);
                }
            }






        }
    }
    [WebMethod(EnableSession = true)]
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
            if (username != "" && password != "")
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

                            Query1 = "update user_login set LastLoginDate=NOW()";
                            Query1 += " where id='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                            resin = gc1.update_cmd(Query1);

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
        Response.Redirect("main.aspx");
    }
    protected void message_type1_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 1;
        Response.Redirect("main.aspx");
    }
    protected void message_type2_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 2;
        Response.Redirect("main.aspx");
    }
    protected void message_type3_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 3;
        Response.Redirect("main.aspx");
    }
    protected void message_type4_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 4;
        Response.Redirect("main.aspx");
    }
    protected void message_type5_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 5;
        Response.Redirect("main.aspx");
    }
    protected void post_message_Button_Click(object sender, EventArgs e)
    {
        if (Session["id"] != null || Session["id"].ToString() != "")
        {
            string id = Session["id"].ToString();
            string message = post_message_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");
            string message_type = Request.Form["post_type"].ToString();
            if (message_type == "")
            {
                message_type = "6";
            }
            string type = "";
            if (type_div.Value == "一般公開")
            {
                type = "0";
            }
            else if (type_div.Value == "地域限定")
            {
                type = "1";
            }
            else if (type_div.Value == "友達")
            {
                type = "2";
            }
            else if (type_div.Value == "")
            {
                type = "0";
            }

            string place = place_va.Value;
            string postal_code = postcode_HiddenField.Value;

            if (postal_code == "")
            {

                Query = "select postal_code from user_login_address";
                Query += " where uid='" + id + "';";

                DataView ict_f_sel = gc.select_cmd(Query);
                if (ict_f_sel.Count > 0)
                {
                    postal_code = ict_f_sel.Table.Rows[0]["postal_code"].ToString();
                }
            }

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
            string min = DateTime.Now.Minute.ToString();
            string sec = DateTime.Now.Second.ToString();

            if (postal_code != "")
            {
                if (post_message_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() != "")
                {

                    //ScriptManager.RegisterStartupScript(post_message_Button, post_message_Button.GetType(), "alert", "alert('@" + place + "')", true);


                    Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                    Query += " values('" + id + "','" + type + "','" + message_type + "','" + place.Trim() + "','" + message + "','" + year + "','" + month + "'";
                    Query += ",'" + day + "','" + hour + "','" + min + "','" + sec + "','" + postal_code + "')";

                    resin = gc.insert_cmd(Query);



                    string upid = "";

                    Query = "select id from status_messages";
                    Query += " where uid='" + id + "' and type='" + type + "' and message_type='" + message_type + "' and place='" + place + "'";
                    Query += " and message='" + message + "' and year='" + year + "' and month='" + month + "' and day='" + day + "' and hour='" + hour + "'";
                    Query += " and minute='" + min + "' and second='" + sec + "' and postal_code='" + postal_code + "';";
                    DataView ict_f = gc.select_cmd(Query);
                    if (ict_f.Count > 0)
                    {
                        upid = ict_f.Table.Rows[0]["id"].ToString();
                        string image_path = image_HiddenField.Value;

                        string me = @",";
                        List<int> image_find = new List<int>();

                        Regex ItemRegex = new Regex(me, RegexOptions.Compiled);
                        foreach (Match ItemMatch in ItemRegex.Matches(image_path))
                        {
                            image_find.Add(ItemMatch.Index);
                        }

                        int len = 0; string naa = "";
                        if (image_find.Count > 0)
                        {
                            for (int i = 0; i < image_find.Count - 1; i++)
                            {
                                len = image_find[i + 1] - image_find[i];
                                naa = image_path.Substring(image_find[i] + 1, len - 1);

                                Query = "insert into status_messages_image(smid,filename)";
                                Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + naa + "');";
                                resin = gc.insert_cmd(Query);

                            }
                            len = image_path.Length - image_find[image_find.Count - 1];
                            naa = image_path.Substring(image_find[image_find.Count - 1] + 1, len - 1);

                            Query = "insert into status_messages_image(smid,filename)";
                            Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + naa + "');";
                            resin = gc.insert_cmd(Query);
                        }

                        post_message_TextBox.Text = "";
                        image_HiddenField.Value = "";
                    }

                    if (upid != "")
                    {
                        //GPS
                        string place_lat = lat_HiddenField.Value;
                        string place_lng = lng_HiddenField.Value;
                        if (place_lat != null && place_lng != null)
                        {
                            if (place_lat != "" && place_lng != "")
                            {

                                Query = "update status_messages set place_lat='" + place_lat + "',place_lng='" + place_lng + "'";
                                Query += " where id='" + upid + "';";
                                resin = gc.update_cmd(Query);
                            }
                        }
                    }
                    ConvertUrlsInData(message);
                    Response.Redirect("main.aspx");


                }
            }

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
    public static void ConvertUrlsToInData(string url)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
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
        if (wss.Title != null)
        {
            urld.title = wss.Title;
        }
        if (wss.Description != null)
        {
            urld.des = wss.Description;
        }

        if (imgurl != "")
        {
            if (UrlExists(imgurl))
            {
                urld.image_url = imgurl;
            }
        }



        Query1 = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
        Query1 += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";
        resin = gc1.insert_cmd(Query1);

    }
    public static void ConvertUrlsInData(string msg)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {

            Query1 = "select id from status_messages_link_info where link like '" + match.Value + "';";
            DataView ict1 = gc1.select_cmd(Query1);
            if (ict1.Count == 0)
            {
                ConvertUrlsToInData(match.Value);
            }
        }
    }
    protected void video_list_Button_Click(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static string like_or_not(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3;

        Query1 = "select id from status_messages_user_like";
        Query1 += " where uid='" + param1 + "' and smid='" + param2 + "';";

        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            Query1 = "update status_messages_user_like set good_status='" + param3 + "'";
            Query1 += ",year='" + DateTime.Now.Year + "',month='" + DateTime.Now.Month + "',day='" + DateTime.Now.Day + "',hour='" + DateTime.Now.ToString("HH") + "',minute='" + DateTime.Now.Minute + "',second='" + DateTime.Now.Second + "'";
            Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);

        }
        else
        {
            Query1 = "insert into status_messages_user_like(uid,smid,good_status,year,month,day,hour,minute,second)";
            Query1 += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            resin = gc1.insert_cmd(Query1);
        }


        return result;
    }

    [WebMethod]
    public static string like_who_ans(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param3;
        Query1 = "select id from status_messages_user_talk_like";
        Query1 += " where uid='" + param1 + "' and smutid='" + param2 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            Query1 = "update status_messages_user_talk_like set good_status='" + param3 + "'";
            Query1 += ",year='" + DateTime.Now.Year + "',month='" + DateTime.Now.Month + "',day='" + DateTime.Now.Day + "',hour='" + DateTime.Now.ToString("HH") + "',minute='" + DateTime.Now.Minute + "',second='" + DateTime.Now.Second + "'";
            Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);

        }
        else
        {
            Query1 = "insert into status_messages_user_talk_like(uid,smutid,good_status,year,month,day,hour,minute,second)";
            Query1 += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            resin = gc1.insert_cmd(Query1);
        }


        return result;
    }

    [WebMethod]
    public static string who_ans(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";

        Query1 = "select photo,username from user_login";
        Query1 += " where id='" + param1 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            result += "<table width='100%'>";
            result += "<tr>";
            result += "<td width='5%'></td>";
            result += "<td width='10%' valign='top' align='right'>";
            //user photo
            result += "<div class='zoom-gallery'>";
            string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            result += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict_f.Table.Rows[0]["username"].ToString() + "' style='width:25px;height:25px;'>";
            result += "<img src='" + cutstr3 + "' width='25' height='25' />";
            result += "</a>";
            result += "</div>";
            result += "</td>";
            result += "<td width='85%'>";

            //user answer
            result += "<input type='text' id='whysmal" + param2 + "_" + param2 + "' onkeypress='small_sendmessage(event,this.id)'  placeholder='コメントする' style='width: 50%;height:30px;' title='【Enter】キーを押してください'>";

            result += @"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload_" + param2 + @"' />
</label>
<br />
            <div id='progressbar_" + param2 + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place_" + param2 + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image_" + param2 + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>";



            result += "</td>";
            result += "</tr>";
            result += "</table>";

            result += @"<script>

$(function () {
$('#btnFileUpload_" + param2 + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar_" + param2 + @"').show();
        $('#image_place_" + param2 + @"').hide();
        $('#image_place_" + param2 + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar_" + param2 + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar_" + param2 + @"').hide();
        $('#progressbar_" + param2 + @" div').css('width', '0%');
        $('#image_place_" + param2 + @"').show();
        document.getElementById('make-image_" + param2 + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar_" + param2 + @"').hide();
        $('#progressbar_" + param2 + @" div').css('width', '0%');
        $('#image_place_" + param2 + @"').hide();
        $('#image_place_" + param2 + @" div').css('width', '0%');
        console.log('error', error);
    }
});});</script>";


        }
        return result;
    }

    [WebMethod]
    public static string small_sendtopost(string param1, string param2, string param3, string param4)
    {

        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3 + "," + param4;

        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string smuid = "";
        int structure_level = 0;

        Query1 = "select smuid,structure_level from status_messages_user_talk";
        Query1 += " where id='" + param2 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            smuid = ict_f.Table.Rows[0]["smuid"].ToString();
            structure_level = Convert.ToInt32(ict_f.Table.Rows[0]["structure_level"].ToString());
            //result = modified.ToString();
            string impath = "~/" + param4;
            if (param4 == "")
            {
                impath = "";
            }
            Query1 = "insert into status_messages_user_talk(smuid,message,filename,pointer_message_id,pointer_user_id,structure_level,year,month,day,hour,minute,second)";
            Query1 += " values('" + smuid + "','" + param3 + "','" + impath + "','" + param2 + "','" + param1 + "','" + (structure_level + 1) + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            resin = gc1.insert_cmd(Query1);
        }


        return result;
    }

    [WebMethod(EnableSession = true)]
    public static string sendtopost(string param1, string param2, string param3, string param4)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3 + "," + param4;

        string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
        int modified = 0;

        using (MySqlConnection con = new MySqlConnection(constr))
        {
            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO status_messages_user(uid,smid) VALUES('" + param1 + "','" + param2 + "');", con))
            {
                con.Open();

                cmd.ExecuteNonQuery();
                modified = System.Convert.ToInt32(cmd.LastInsertedId);

                con.Close();
            }
        }
        //result = modified.ToString();
        string impath = "~/" + param4;
        if (param4 == "")
        {
            impath = "";
        }
        Query1 = "insert into status_messages_user_talk(smuid,message,filename,pointer_message_id,pointer_user_id,structure_level,year,month,day,hour,minute,second)";
        Query1 += " values('" + modified + "','" + param3 + "','" + impath + "','0','0','0','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
        resin = gc1.insert_cmd(Query1);



        //
        //sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //Query1 = "select id from status_messages_user_like";
        //Query1 += " where uid='" + param1 + "' and smid='" + param2 + "';";
        //sql_f.DataBind();
        //DataView ict_f = gc1.select_cmd(Query1);
        //if (ict_f.Count > 0)
        //{

        //    SqlDataSource sql_f_update = new SqlDataSource();
        //    sql_f_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //    Query1 = "update status_messages_user_like set good_status='" + param3 + "'";
        //    Query1 += ",year='" + DateTime.Now.Year + "',month='" + DateTime.Now.Month + "',day='" + DateTime.Now.Day + "',hour='" + DateTime.Now.ToString("HH") + "',minute='" + DateTime.Now.Minute + "',second='" + DateTime.Now.Second + "'";
        //    Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
        //    sql_f_update.Update();

        //}
        //else
        //{
        //    SqlDataSource sql_f_insert = new SqlDataSource();
        //    sql_f_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //   Query1 = "insert into status_messages_user_like(uid,smid,good_status,year,month,day,hour,minute,second)";
        //   Query1 += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
        //    sql_f_insert.Insert();
        //}


        return result;
    }
    protected void supporter_list_Click(object sender, EventArgs e)
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
    protected void supporter_manger_Click(object sender, EventArgs e)
    {
        Response.Redirect("user_date_manger.aspx");
    }
    protected void social_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = null;
        Response.Redirect("main.aspx");

        //post_message_panel.Visible = true;
        //Panel2.Visible = true;
        //Panel_for_support_list.Visible = false;
        //Panel_for_supplist.Visible = false;
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
    public static string changetodate(string param1, string param2)
    {
        string result = "";
        HttpContext.Current.Session["id"] = param1;
        HttpContext.Current.Session["sup_id"] = param2;
        return result;
    }
    protected void Button_for_kid_Click(object sender, EventArgs e)
    {
        post_message_panel.Visible = false;
        Panel2.Visible = false;

        Panel_for_support_list.Visible = false;
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
    [WebMethod]
    public static string friend_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='2';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','2',NOW());";
            resin = gc1.insert_cmd(Query1);
        }

        Query1 = "select a.id,a.first_uid,b.username,b.photo,a.first_date_year,a.first_date_month,a.first_date_day,a.first_date_hour,a.first_date_minute,a.first_date_second ";
        Query1 += "from user_friendship as a inner join user_login as b on a.first_uid=b.id where a.second_uid='" + param1 + "' and a.second_check_connect='0'";
        Query1 += " ORDER BY a.first_date_year desc,a.first_date_month desc,a.first_date_day desc,a.first_date_hour desc,a.first_date_minute desc,a.first_date_second desc;";
        DataView ict_h_fri_notice =gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_second"].ToString());
                string howdate = "";
                if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                {
                    hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                    min = DateTime.Now.Minute - min;
                    sec = DateTime.Now.Second - sec;
                    if (min < 0)
                    {
                        min += 60;
                        hour -= 1;
                    }
                    if (sec < 0)
                    {
                        sec += 60;
                        min -= 1;
                    }
                    string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                    if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                    if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                    if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                    if (hour == 0)
                    {
                        fh = "";
                    }
                    if (min == 0 && hour == 0)
                    {
                        fmin = "";
                    }
                    howdate = fh + fmin + fsec + "前";
                }
                else
                {
                    string fm = month.ToString(), fd = day.ToString();
                    if (month < 10) { fm = "0" + month.ToString(); }
                    if (day < 10) { fd = "0" + day.ToString(); }
                    howdate = year + "年" + fm + "月" + fd + "日";

                }

                string cutstr2 = ict_h_fri_notice.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='40%'>
<a href='user_home_friend.aspx?=" + ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() + @"' style='text-decoration:none;'>" + ict_h_fri_notice.Table.Rows[i]["username"].ToString() + @"</a>
                                        <br/>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
<td width='5%'>

<input id='friendcheck_" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' type='button' value='確認' onclick='dlgcheckfriend(this.id)' class='file-upload1' style='width:100% !important;'/>

</td>
<td width='35%'>

<input id='frienddelete_" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' type='button' value='リクエストを削除' onclick='dlgcheckfriend_del(this.id)' class='file-upload1' style='width:100% !important;'/>

</td>
</tr>
</table><hr/>";
            }
        }


        return result;
    }
    public class friend_user
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
        public int howmany = 0;
    }
    [WebMethod(EnableSession = true)]
    public static string search_friend_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        Query1 = "select id,username,photo ";
        Query1 += "from user_login";
        Query1 += " where id!='" + param1.Trim() + "';";
        DataView ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int i = 0; i < ict_h_find_user.Count; i++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_h_find_user.Table.Rows[i]["id"].ToString());
                fu.username = ict_h_find_user.Table.Rows[i]["username"].ToString();
                string cutstr2 = ict_h_find_user.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                check_same1.Add(fu);
            }
        }
        Query1 = "select donotfind_uid ";
        Query1 += "from user_friendship_donotfind";
        Query1 += " where uid='" + param1.Trim() + "';";
        ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                bool checksam = true;
                for (int i = 0; i < ict_h_find_user.Count; i++)
                {
                    if (ict_h_find_user.Table.Rows[i]["donotfind_uid"].ToString() == check_same1[ii].id.ToString())
                    {
                        checksam = false;
                    }
                }
                if (checksam)
                {
                    fu = new friend_user();
                    fu.id = check_same1[ii].id;
                    fu.username = check_same1[ii].username;
                    fu.photo = check_same1[ii].photo;
                    output_friend.Add(fu);
                }
            }
        }
        else
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = check_same1[ii].id;
                fu.username = check_same1[ii].username;
                fu.photo = check_same1[ii].photo;
                output_friend.Add(fu);
            }
        }

        //SqlDataSource sql_h_fri_notice = new SqlDataSource();
        //sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //Query1 = "select first_uid,second_uid ";
        //Query1 += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        //if (ict_h_fri_notice.Count > 0)
        //{
        //    for (int ii = 0; ii < check_same.Count; ii++)
        //    {
        //        bool checksam = true;
        //        for (int i = 0; i < ict_h_fri_notice.Count; i++)
        //        {
        //            if (ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //            if (ict_h_fri_notice.Table.Rows[i]["second_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //        }
        //        if (checksam)
        //        {
        //            fu = new friend_user();
        //            fu.id = check_same[ii].id;
        //            fu.username = check_same[ii].username;
        //            fu.photo = check_same[ii].photo;
        //            output_friend.Add(fu);
        //        }
        //    }
        //}
        List<string> user_friend = new List<string>();

        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }


        for (int i = 0; i < output_friend.Count; i++)
        {
            int howto = 0;

            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }

            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f1.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            output_friend[i].howmany = howto;

        }


        //set up count
        HttpContext.Current.Session["friend_for_count"] = 10;

        Random rnd = new Random();

        //  宣告用來儲存亂數的陣列
        int[] ValueString = new int[Convert.ToInt32( HttpContext.Current.Session["friend_for_count"].ToString())];

        //  亂數產生
        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()); i++)
        {
            ValueString[i] = rnd.Next(0, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));

            //  檢查是否存在重複
            while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
            {
                ValueString[i] = rnd.Next(0, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));
            }
        }
        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()); i++)
        {
            result += @"<div id='friendpanel_" + i + @"' width='100%'><table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + output_friend[ValueString[i]].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + output_friend[ValueString[i]].id + @"' style='text-decoration:none;'>" + output_friend[ValueString[i]].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>";
            if (output_friend[ValueString[i]].howmany > 0)
            {
                result += @"<a id='listtofri_" + output_friend[ValueString[i]].id + @"' onclick='check_tofriend_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;color:#90949c;'>共通の友達" + output_friend[ValueString[i]].howmany + @"人</a>";
            }

            result += @"</td>
        <td width='30%'>

        <input id='addfriend_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload1' style='width:98% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload1 addfrienddelete' style='width:100% !important;'/>

        </td>
        </tr>
        </table><hr/></div>";
        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string search_friend_notice_list_scroll(string param1)
    {

        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        Query1 = "select id,username,photo ";
        Query1 += "from user_login";
        Query1 += " where id!='" + param1.Trim() + "';";
        DataView ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int i = 0; i < ict_h_find_user.Count; i++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_h_find_user.Table.Rows[i]["id"].ToString());
                fu.username = ict_h_find_user.Table.Rows[i]["username"].ToString();
                string cutstr2 = ict_h_find_user.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                check_same1.Add(fu);
            }
        }
        Query1 = "select donotfind_uid ";
        Query1 += "from user_friendship_donotfind";
        Query1 += " where uid='" + param1.Trim() + "';";
        ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                bool checksam = true;
                for (int i = 0; i < ict_h_find_user.Count; i++)
                {
                    if (ict_h_find_user.Table.Rows[i]["donotfind_uid"].ToString() == check_same1[ii].id.ToString())
                    {
                        checksam = false;
                    }
                }
                if (checksam)
                {
                    fu = new friend_user();
                    fu.id = check_same1[ii].id;
                    fu.username = check_same1[ii].username;
                    fu.photo = check_same1[ii].photo;
                    output_friend.Add(fu);
                }
            }
        }
        else
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = check_same1[ii].id;
                fu.username = check_same1[ii].username;
                fu.photo = check_same1[ii].photo;
                output_friend.Add(fu);
            }
        }

        //SqlDataSource sql_h_fri_notice = new SqlDataSource();
        //sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //Query1 = "select first_uid,second_uid ";
        //Query1 += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        //if (ict_h_fri_notice.Count > 0)
        //{
        //    for (int ii = 0; ii < check_same.Count; ii++)
        //    {
        //        bool checksam = true;
        //        for (int i = 0; i < ict_h_fri_notice.Count; i++)
        //        {
        //            if (ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //            if (ict_h_fri_notice.Table.Rows[i]["second_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //        }
        //        if (checksam)
        //        {
        //            fu = new friend_user();
        //            fu.id = check_same[ii].id;
        //            fu.username = check_same[ii].username;
        //            fu.photo = check_same[ii].photo;
        //            output_friend.Add(fu);
        //        }
        //    }
        //}
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }


        for (int i = 0; i < output_friend.Count; i++)
        {
            int howto = 0;

            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f1.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            output_friend[i].howmany = howto;

        }


        //set up count
        if (HttpContext.Current.Session["friend_for_count"] != null)
        {
            if (HttpContext.Current.Session["friend_for_count"].ToString() != "")
            {
                int count_bf = Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString());
                int count_f = Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString());
                count_f += 10;
                if (count_f < output_friend.Count)
                {
                    HttpContext.Current.Session["friend_for_count"] = count_f;
                    Random rnd = new Random();

                    //  宣告用來儲存亂數的陣列
                    int[] ValueString = new int[count_f - count_bf];

                    //  亂數產生
                    for (int i = 0; i < count_f - count_bf; i++)
                    {
                        ValueString[i] = rnd.Next(count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));

                        //  檢查是否存在重複
                        while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
                        {
                            ValueString[i] = rnd.Next(count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));
                        }
                    }
                    for (int i = 0; i < count_f - count_bf; i++)
                    {
                        result += @"<div id='friendpanel_" + (i + count_bf) + @"' width='100%'><table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + output_friend[ValueString[i]].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + output_friend[ValueString[i]].id + @"' style='text-decoration:none;'>" + output_friend[ValueString[i]].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>";
                        if (output_friend[ValueString[i]].howmany > 0)
                        {
                            result += @"<a id='listtofri_" + output_friend[ValueString[i]].id + @"' onclick='check_tofriend_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;color:#90949c;'>共通の友達" + output_friend[ValueString[i]].howmany + @"人</a>";
                        }

                        result += @"</td>
        <td width='30%'>

        <input id='addfriend_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload1' style='width:98% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload1 addfrienddelete' style='width:100% !important;'/>

        </td>
        </tr>
        </table><hr/></div>";
                    }
                }
            }
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_addfind(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;

        result = "";

        string year = DateTime.Now.Year.ToString();
        string month = DateTime.Now.Month.ToString();
        string day = DateTime.Now.Day.ToString();
        int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
        string min = DateTime.Now.Minute.ToString();
        string sec = DateTime.Now.Second.ToString();

        string upid = "";
        bool chec = true;

        Query1 = "select a.id";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        Query1 += " and c.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            upid = ict_f.Table.Rows[0]["id"].ToString();
            chec = true;
        }

        Query1 = "select a.id";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        Query1 += " and b.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            upid = ict_f1.Table.Rows[0]["id"].ToString();
            chec = false;
        }

        if (upid != "")
        {
            if (chec)
            {
                Query1 = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                Query1 += ",first_date_year='" + year + "',first_date_month='" + month + "',first_date_day='" + day + "',first_date_hour='" + hour + "',first_date_minute='" + min + "',first_date_second='" + sec + "'";
                Query1 += " where id='" + upid + "';";
                resin = gc1.update_cmd(Query1);
            }
            else
            {
                Query1 = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                Query1 += ",second_date_year='" + year + "',second_date_month='" + month + "',second_date_day='" + day + "',second_date_hour='" + hour + "',second_date_minute='" + min + "',second_date_second='" + sec + "'";
                Query1 += " where id='" + upid + "';";
                resin = gc1.update_cmd(Query1);
            }

        }
        else
        {
            Query1 = "insert into user_friendship(first_uid,first_check_connect,second_uid,second_check_connect";
            Query1 += ",first_date_year,first_date_month,first_date_day,first_date_hour,first_date_minute,first_date_second)";
            Query1 += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','1','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','0'";
            Query1 += ",'" + year + "','" + month + "','" + day + "','" + hour + "','" + min + "','" + sec + "');";
            resin = gc1.insert_cmd(Query1);
        }



        return result;
    }
    [WebMethod]
    public static string friend_notice_donotfind(string param1,string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;

        result = "";

        Query1 = "insert into user_friendship_donotfind(uid,donotfind_uid)";
        Query1 += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "');";
        resin = gc1.insert_cmd(Query1);

        return result;
    }
    [WebMethod]
    public static string toget_friend_list(string param1,string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //fu = new friend_user();
        //fu.id = check_same[ii].id;
        //fu.username = check_same[ii].username;
        //fu.photo = check_same[ii].photo;
        //output_friend.Add(fu);
        List<friend_user> user_friend = new List<friend_user>();
        friend_user fu = new friend_user();

        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                fu.username = ict_f.Table.Rows[ii]["username"].ToString();
                string cutstr2 = ict_f.Table.Rows[ii]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                user_friend.Add(fu);
            }
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                fu.username = ict_f1.Table.Rows[ii]["username"].ToString();
                string cutstr2 = ict_f1.Table.Rows[ii]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                user_friend.Add(fu);
            }
        }

        List<friend_user> user_to_friend = new List<friend_user>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            int howto = 0;

            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + param2.Trim() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                    for (int ii = 0; ii < ict_f.Count; ii++)
                    {
                        if (user_friend[i].id.ToString() == ict_f.Table.Rows[ii]["id"].ToString())
                        {
                            fu = new friend_user();
                            fu.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                            fu.username = ict_f.Table.Rows[ii]["username"].ToString();
                            string cutstr2 = ict_f.Table.Rows[ii]["photo"].ToString();
                            int ind2 = cutstr2.IndexOf(@"/");
                            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            fu.photo = cutstr3;
                            user_to_friend.Add(fu);
                        }
                    }

            }

            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + param2.Trim() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                    for (int ii = 0; ii < ict_f1.Count; ii++)
                    {
                        if (user_friend[i].id.ToString() == ict_f1.Table.Rows[ii]["id"].ToString())
                        {
                            fu = new friend_user();
                            fu.id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                            fu.username = ict_f1.Table.Rows[ii]["username"].ToString();
                            string cutstr2 = ict_f1.Table.Rows[ii]["photo"].ToString();
                            int ind2 = cutstr2.IndexOf(@"/");
                            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            fu.photo = cutstr3;
                            user_to_friend.Add(fu);
                        }
                    }
                }

        }

        for (int i = 0; i < user_to_friend.Count; i++)
        {
            result += @"<table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + user_to_friend[i].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + user_to_friend[i].id + @"' style='text-decoration:none;'>" + user_to_friend[i].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>


                                            </td>
        <td width='30%'>



        </td>
        <td width='10%'>


        </td>
        </tr>
        </table><hr/>";
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_check(string param1)
    {

        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        Query1 = "update user_friendship set second_check_connect='1',second_date_year='" + DateTime.Now.Year + "',second_date_month='" + DateTime.Now.Month + "',second_date_day='" + DateTime.Now.Day + "'";
        Query1 += ",second_date_hour='" + DateTime.Now.ToString("HH") + "',second_date_minute='" + DateTime.Now.Minute + "',second_date_second='" + DateTime.Now.Second + "' ";
        Query1 += "where id='" + param1 + "';";
        resin = gc1.update_cmd(Query1);

        return result;
    }
    [WebMethod]
    public static string friend_notice_check_del(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        if (param1.Trim() != "")
        {

            Query1 = "DELETE FROM user_friendship ";
            Query1 += "where id='" + param1 + "';";
            resin = gc1.delete_cmd(Query1);
        }

        return result;
    }
    public class friend_list_chat
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public int hour = 0;
        public int min = 0;
        public int sec = 0;
        public string mesg = "";
        public DateTime comdate = new DateTime();
    }
    [WebMethod]
    public static string chat_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='1';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','1',NOW());";
            resin = gc1.insert_cmd(Query1);
        }

        Query1 = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1 + "'";
        Query1 += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);

        List<friend_list_chat> fri = new List<friend_list_chat>();
        friend_list_chat frii = new friend_list_chat();
        int tempid = 0;
        for (int i = 0; i < ict_f.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
        }

        Query1 = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where c.id=" + param1 + "";
        Query1 += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        tempid = 0;
        for (int i = 0; i < ict_f1.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f1.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f1.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f1.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
        }

        fri = fri.OrderBy(c => c.id).ToList();

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //        .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();

        List<friend_list_chat> tmp_fri = new List<friend_list_chat>();
        List<friend_list_chat> fri_total = new List<friend_list_chat>();
        frii = new friend_list_chat();
        List<int> fri_ind = new List<int>();
        tempid = 0;
        for (int i = 0; i < fri.Count; i++)
        {
            if (tempid != fri[i].id)
            {
                tempid = fri[i].id;
                fri_ind.Add(tempid);
            }
        }
        for (int i = 0; i < fri_ind.Count; i++)
        {
            tmp_fri = new List<friend_list_chat>();
            for (int ii = 0; ii < fri.Count; ii++)
            {
                if (fri_ind[i] == fri[ii].id)
                {
                    tmp_fri.Add(fri[ii]);
                }
            }
            tmp_fri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            fri_total.Add(tmp_fri[tmp_fri.Count - 1]);
        }
        fri_total.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //       .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();
        fri = fri_total;
        for (int i = 0; i < fri.Count; i++)
        {

            int year = fri[i].year;
            int month = fri[i].month;
            int day = fri[i].day;
            int hour = fri[i].hour;
            int min = fri[i].min;
            int sec = fri[i].sec;
            string howdate = "";
            if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                min = DateTime.Now.Minute - min;
                sec = DateTime.Now.Second - sec;
                if (min < 0)
                {
                    min += 60;
                    hour -= 1;
                }
                if (sec < 0)
                {
                    sec += 60;
                    min -= 1;
                }
                string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                if (hour == 0)
                {
                    fh = "";
                }
                if (min == 0 && hour == 0)
                {
                    fmin = "";
                }
                howdate = fh + fmin + fsec + "前";
            }
            else
            {
                string fm = month.ToString(), fd = day.ToString();
                if (month < 10) { fm = "0" + month.ToString(); }
                if (day < 10) { fd = "0" + day.ToString(); }
                howdate = year + "年" + fm + "月" + fd + "日";

            }

            string cutstr2 = fri[i].photo;
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            string mess = "";
            if (fri[i].mesg.Length < 20)
            {
                mess = fri[i].mesg;
            }
            else
            {
                mess = fri[i].mesg.Substring(0, 19) + "‧‧‧";
            }
            result += @"<div id='chat_" + fri[i].id + @"' style='cursor: pointer;' onclick='chat_notice_click(this.id)'><table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + fri[i].id + @"' style='text-decoration:none;'>" + fri[i].username + @"</a>
                                        <br/>
<br/>
<span style='color:#000;'>" + mess + @"</span>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";


        }

        return result;
    }
    [WebMethod]
    public static string new_state_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','0',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

        //status message
        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f1.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f1.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message


        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer
                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message
        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {
                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }

                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //count
        HttpContext.Current.Session["new_state_for_count"] = 10;
        if (status_mess_like.Count < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()))
        {
            HttpContext.Current.Session["new_state_for_count"] = status_mess_like.Count;
        }

        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()); i++)
        {

            int year = status_mess_like[i].comdate.Year;
            int month = status_mess_like[i].comdate.Month;
            int day = status_mess_like[i].comdate.Day;
            int hour = status_mess_like[i].comdate.Hour;
            int min = status_mess_like[i].comdate.Minute;
            int sec = status_mess_like[i].comdate.Second;
            string howdate = "";
            if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                min = DateTime.Now.Minute - min;
                sec = DateTime.Now.Second - sec;
                if (min < 0)
                {
                    min += 60;
                    hour -= 1;
                }
                if (sec < 0)
                {
                    sec += 60;
                    min -= 1;
                }
                string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                if (hour == 0)
                {
                    fh = "";
                }
                if (min == 0 && hour == 0)
                {
                    fmin = "";
                }
                howdate = fh + fmin + fsec + "前";
            }
            else
            {
                string fm = month.ToString(), fd = day.ToString();
                if (month < 10) { fm = "0" + month.ToString(); }
                if (day < 10) { fd = "0" + day.ToString(); }
                howdate = year + "年" + fm + "月" + fd + "日";

            }
            if (status_mess_like[i].type == 2)
            {
                //friend post
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].uid + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                    int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    string mess = "";
                    if (status_mess_like[i].like_message.Length < 20)
                    {
                        mess = status_mess_like[i].like_message;
                    }
                    else
                    {
                        mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                    }
                    //check
                    result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが近況を更新しました「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                }
            }
            else if (status_mess_like[i].type == 3)
            {
                //friend like
                //other person name
                string othfri = "";
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    othfri = ict_f.Table.Rows[0]["username"].ToString();
                }
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].uid + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                    int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    string mess = "";
                    if (status_mess_like[i].like_message.Length < 20)
                    {
                        mess = status_mess_like[i].like_message;
                    }
                    else
                    {
                        mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                    }
                    //status_mess_like[i].like_idlist[0]
                    //check
                    result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが</span>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + othfri + @"</a>
<span>さんの投稿について「いいね！」と言っています: 「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                }
            }
            else
            {
                if (status_mess_like[i].uid == 0)
                {
                    if (status_mess_like[i].like_idlist.Count > 0)
                    {
                        Query1 = "select username,photo";
                        Query1 += " from user_login";
                        Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                        ict_f = gc1.select_cmd(Query1);
                        if (ict_f.Count > 0)
                        {
                            string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                            int ind2 = cutstr2.IndexOf(@"/");
                            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            string mess = "";
                            if (status_mess_like[i].like_message.Length < 20)
                            {
                                mess = status_mess_like[i].like_message;
                            }
                            else
                            {
                                mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                            }
                            //check
                            if (status_mess_like[i].type > 0)
                            {
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                            }
                            else
                            {
                                result += @"<div id='newstatus_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                            }
                            result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さん他" + (status_mess_like[i].like_idlist.Count - 1) + @"人があなたの投稿に「いいね」と言っています:「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                        }
                    }
                }
                else
                {
                    Query1 = "select username,photo";
                    Query1 += " from user_login";
                    Query1 += " where id='" + status_mess_like[i].uid + "';";
                    ict_f = gc1.select_cmd(Query1);
                    if (ict_f.Count > 0)
                    {
                        string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        string mess = "";
                        if (status_mess_like[i].message.Length < 20)
                        {
                            mess = status_mess_like[i].message;
                        }
                        else
                        {
                            mess = status_mess_like[i].message.Substring(0, 19) + "‧‧‧";
                        }

                        result += @"<div id='newstatus_" + status_mess_like[i].id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                        result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんがあなたの投稿に返信をしました:「" + mess + @"」。</span>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                    }
                }
            }

        }

        return result;
    }
    [WebMethod]
    public static string new_state_notice_list_scroll(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','0',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

        //status message
        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";

            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";

            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f1.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f1.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message


        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer

                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message
        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {
                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }

                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //count
        if (HttpContext.Current.Session["new_state_for_count"] != null)
        {
            if (HttpContext.Current.Session["new_state_for_count"].ToString() != "")
            {
                int count_bf = Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString());
                int count_f = Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString());
                count_f += 10;
                if (count_f < status_mess_like.Count)
                {
                    HttpContext.Current.Session["new_state_for_count"] = count_f;

                    for (int i = count_bf; i < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()); i++)
                    {

                        int year = status_mess_like[i].comdate.Year;
                        int month = status_mess_like[i].comdate.Month;
                        int day = status_mess_like[i].comdate.Day;
                        int hour = status_mess_like[i].comdate.Hour;
                        int min = status_mess_like[i].comdate.Minute;
                        int sec = status_mess_like[i].comdate.Second;
                        string howdate = "";
                        if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                        {
                            hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                            min = DateTime.Now.Minute - min;
                            sec = DateTime.Now.Second - sec;
                            if (min < 0)
                            {
                                min += 60;
                                hour -= 1;
                            }
                            if (sec < 0)
                            {
                                sec += 60;
                                min -= 1;
                            }
                            string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                            if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                            if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                            if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                            if (hour == 0)
                            {
                                fh = "";
                            }
                            if (min == 0 && hour == 0)
                            {
                                fmin = "";
                            }
                            howdate = fh + fmin + fsec + "前";
                        }
                        else
                        {
                            string fm = month.ToString(), fd = day.ToString();
                            if (month < 10) { fm = "0" + month.ToString(); }
                            if (day < 10) { fd = "0" + day.ToString(); }
                            howdate = year + "年" + fm + "月" + fd + "日";

                        }
                        if (status_mess_like[i].type == 2)
                        {
                            //friend post
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].uid + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                int ind2 = cutstr2.IndexOf(@"/");
                                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                string mess = "";
                                if (status_mess_like[i].like_message.Length < 20)
                                {
                                    mess = status_mess_like[i].like_message;
                                }
                                else
                                {
                                    mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                }
                                //check
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが近況を更新しました「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                            }
                        }
                        else if (status_mess_like[i].type == 3)
                        {
                            //friend like
                            //other person name
                            string othfri = "";
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                othfri = ict_f.Table.Rows[0]["username"].ToString();
                            }
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].uid + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                int ind2 = cutstr2.IndexOf(@"/");
                                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                string mess = "";
                                if (status_mess_like[i].like_message.Length < 20)
                                {
                                    mess = status_mess_like[i].like_message;
                                }
                                else
                                {
                                    mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                }
                                //status_mess_like[i].like_idlist[0]
                                //check
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが</span>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + othfri + @"</a>
<span>さんの投稿について「いいね！」と言っています: 「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                            }
                        }
                        else
                        {
                            if (status_mess_like[i].uid == 0)
                            {
                                if (status_mess_like[i].like_idlist.Count > 0)
                                {
                                    Query1 = "select username,photo";
                                    Query1 += " from user_login";
                                    Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                                    ict_f = gc1.select_cmd(Query1);
                                    if (ict_f.Count > 0)
                                    {
                                        string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                        int ind2 = cutstr2.IndexOf(@"/");
                                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                        string mess = "";
                                        if (status_mess_like[i].like_message.Length < 20)
                                        {
                                            mess = status_mess_like[i].like_message;
                                        }
                                        else
                                        {
                                            mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                        }
                                        //check
                                        if (status_mess_like[i].type > 0)
                                        {
                                            result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                        }
                                        else
                                        {
                                            result += @"<div id='newstatus_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                                        }
                                        result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さん他" + (status_mess_like[i].like_idlist.Count - 1) + @"人があなたの投稿に「いいね」と言っています:「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                                    }
                                }
                            }
                            else
                            {
                                Query1 = "select username,photo";
                                Query1 += " from user_login";
                                Query1 += " where id='" + status_mess_like[i].uid + "';";
                                ict_f = gc1.select_cmd(Query1);
                                if (ict_f.Count > 0)
                                {
                                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                    int ind2 = cutstr2.IndexOf(@"/");
                                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                    string mess = "";
                                    if (status_mess_like[i].message.Length < 20)
                                    {
                                        mess = status_mess_like[i].message;
                                    }
                                    else
                                    {
                                        mess = status_mess_like[i].message.Substring(0, 19) + "‧‧‧";
                                    }

                                    result += @"<div id='newstatus_" + status_mess_like[i].id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんがあなたの投稿に返信をしました:「" + mess + @"」。</span>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                                }
                            }
                        }

                    }
                }
            }
        }



        return result;
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
    [WebMethod]
    public static string like_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";

        Query1 = "select b.id,b.username,b.photo,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
        Query1 += " where a.smid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["second"].ToString());
                string howdate = "";
                if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                {
                    hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                    min = DateTime.Now.Minute - min;
                    sec = DateTime.Now.Second - sec;
                    if (min < 0)
                    {
                        min += 60;
                        hour -= 1;
                    }
                    if (sec < 0)
                    {
                        sec += 60;
                        min -= 1;
                    }
                    string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                    if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                    if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                    if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                    if (hour == 0)
                    {
                        fh = "";
                    }
                    if (min == 0 && hour == 0)
                    {
                        fmin = "";
                    }
                    howdate = fh + fmin + fsec + "前";
                }
                else
                {
                    string fm = month.ToString(), fd = day.ToString();
                    if (month < 10) { fm = "0" + month.ToString(); }
                    if (day < 10) { fd = "0" + day.ToString(); }
                    howdate = year + "年" + fm + "月" + fd + "日";

                }

                string cutstr2 = ict_h_fri_notice.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='40%'>
<a href='user_home_friend.aspx?=" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' style='text-decoration:none;'>" + ict_h_fri_notice.Table.Rows[i]["username"].ToString() + @"</a>
                                        <br/>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
<td>

</td>
</tr>
</table><hr/>";
            }
        }


        return result;
    }
    [WebMethod]
    public static string report_bad(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";

        Query1 = "select id";
        Query1 += " from status_messages_report";
        Query1 += " where smid='" + param2 + "' and uid='" + param1 + "';";
        DataView ict_h_rep = gc1.select_cmd(Query1);
        if (ict_h_rep.Count > 0)
        {
            Query1 = "update status_messages_report";
            Query1 += " set report_mess='" + param3 + "',report_time=NOW()";
            Query1 += " where id='" + ict_h_rep.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into status_messages_report(smid,uid,report_mess,report_time)";
            Query1 += " values('" + param2 + "','" + param1 + "','" + param3 + "',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        result = "問題の内容が受けました。なるべく早くご返事いたします。";

        return result;
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
        List<string> check_pos = new List<string>();

        //find poster GPS
        Query1 = "select postal_code";
        Query1+= " from user_login_address";
        Query1 += " where uid='" + HttpContext.Current.Session["id"].ToString() + "';";
        DataView ict_place =gc1.select_cmd(Query1);
        int couuu = 0;
        Literal lip = new Literal();
        Literal lip1 = new Literal();
        Literal lip2 = new Literal();
        if (ict_place.Count > 0)
        {

            lip1.Text = "";
            lip2.Text = "";
            lip2.Text += @"var bounds = new google.maps.LatLngBounds();";
            for (int i = 0; i < ict_place.Count; i++)
            {
                postal_code_list.Add(ict_place.Table.Rows[i]["postal_code"].ToString());

                string result = "";
                var url = new Uri("https://postcode.teraren.com/postcodes/" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[i]["postal_code"].ToString().Replace("-", "")) + ".json");


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
            }

        }
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


        if (HttpContext.Current.Session["id"].ToString() != "")
        {
            //search friends
            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + HttpContext.Current.Session["id"].ToString() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            DataView ict_sf = gc1.select_cmd(Query1);

            List<string> fri = new List<string>();

            for (int i = 0; i < ict_sf.Count; i++)
            {
                fri.Add(ict_sf.Table.Rows[i]["id"].ToString());
            }

            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + HttpContext.Current.Session["id"].ToString() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            DataView ict_f1 = gc1.select_cmd(Query1);

            for (int i = 0; i < ict_f1.Count; i++)
            {
                fri.Add(ict_f1.Table.Rows[i]["id"].ToString());
            }
            //search friends
            //friend only type message
            List<string> not_friend = new List<string>();
            Query1 = "select id";
            Query1 += " from status_messages";
            Query1 += " where type='2'";
            if (fri.Count > 0)
            {
                Query1 += " and uid not in (select uid from status_messages where type='2'";
                Query1 += " and (uid='" + fri[0] + "'";
                for (int iff = 1; iff < fri.Count; iff++)
                {
                    Query1 += " or uid='" + fri[iff] + "'";
                }
                Query1 += "))";
            }
            DataView ict_not_friend = gc1.select_cmd(Query1);
            if (ict_not_friend.Count > 0)
            {
                for (int inot = 0; inot < ict_not_friend.Count; inot++)
                {
                    not_friend.Add(ict_not_friend.Table.Rows[inot]["id"].ToString());
                }
            }
            //friend only type message
            /////
            bool check_sort_pop = false;
            //string addstr = " and a.id not in (select t.id from (select a.id from status_messages as a use index (IX_status_messages_1)";
            //addstr += " inner join user_login as b on b.id=a.uid where 1=1";
            Query1 = " select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query1 += "from status_messages as a use index (IX_status_messages_1)";
            Query1 += " inner join user_login as b on b.id=a.uid where 1=1";
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
                        //addstr += " and a.message_type='" + HttpContext.Current.Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                //addstr += " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                    //addstr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                //addstr += ")";
                Query1 += qustr;
            }
            //not friend
            if (not_friend.Count > 0)
            {
                string qustr = " and ( a.id!='" + not_friend[0] + "'";
                //addstr += " and ( a.id!='" + not_friend[0] + "'";
                for (int i = 1; i < not_friend.Count; i++)
                {
                    qustr += " or a.id!='" + not_friend[i] + "'";
                    //addstr += " or a.id!='" + not_friend[i] + "'";
                }
                qustr += ")";
               // addstr += ")";
                Query1 += qustr;
            }
            //not friend
            if (check_sort_pop)
            {
                //addstr += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query1 += addstr;
                Query1 += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn -10) + ",10;";
            }
            else
            {
               // addstr += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT 0) as t)";
                //Query1 += addstr;
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }
            ict_place = gc1.select_cmd(Query1);

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
                        metype = "授乳室、";
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


                    content = '"' + @"<div><table width='100%'><tr><td width='20%' valign='top'><img src='" + cutstr3 + @"' height='50px' width='50px'></td><td width='80%' valign='top' style='word-wrap: break-word;'><a href='user_home_friend.aspx?=" + ict_place.Table.Rows[sorst_list1[i].id]["uid"].ToString() + @"' style='text-decoration: none;'>" + ict_place.Table.Rows[sorst_list1[i].id]["username"].ToString() + @"</a><br/><span style='color:#CCCCCC;'>" + ict_place.Table.Rows[sorst_list1[i].id]["year"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["month"].ToString() + "." + ict_place.Table.Rows[sorst_list1[i].id]["day"].ToString() + @"</span>&nbsp;&nbsp;<span style='color:#CCCCCC;'>" + metype + @"</span><br/><span>" + mess + @"</span><br/></td></tr></table></div>" + '"';
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
                            string result = "";

                            var url = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(ict_place.Table.Rows[sorst_list1[i].id]["place"].ToString());

                            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                            using (var response = request.GetResponse())
                            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                result = sr.ReadToEnd();
                            }

                            JObject jArray = JObject.Parse(result);
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
                var map1;
var allMarkers = [];
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


            //addstr = " and a.id not in (select t.id from (select a.id from status_messages as a use index (IX_status_messages_1)";
            //addstr += " inner join user_login as b on b.id=a.uid where 1=1";
            Query1 = "select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query1 += "from status_messages as a use index (IX_status_messages_1)";
            Query1 += " inner join user_login as b on b.id=a.uid where 1=1";

            //if want to class by type use type=0,1,2 ; message_type=0,1,2

            ////Before today's message
            //Query1 += " where a.year<=" + DateTime.Now.Year.ToString() + " and a.month<=" + DateTime.Now.Month.ToString();
            //Query1 += " and a.day<=" + DateTime.Now.Day.ToString() + " ";


            ////type message select
            if (HttpContext.Current.Session["message_type"] != null)
            {
                if (HttpContext.Current.Session["message_type"].ToString() != "")
                {
                    if (HttpContext.Current.Session["message_type"].ToString() != "1")
                    {
                        Query1 += " and a.message_type='" + HttpContext.Current.Session["message_type"].ToString() + "'";
                        //addstr += " and a.message_type='" + HttpContext.Current.Session["message_type"].ToString() + "'";
                    }
                }
            }
            if (postal_code_list.Count > 0)
            {
                string qustr = " and ( a.postal_code='" + postal_code_list[0] + "'";
                //addstr += " and ( a.postal_code='" + postal_code_list[0] + "'";
                for (int i = 1; i < postal_code_list.Count; i++)
                {
                    qustr += " or a.postal_code='" + postal_code_list[i] + "'";
                    //addstr += " or a.postal_code='" + postal_code_list[i] + "'";
                }
                //qustr += " or a.postal_code=''";
                //addstr += " or a.postal_code=''";
                qustr += ")";
                //addstr += ")";
                Query1 += qustr;
            }
            //not friend
            if (not_friend.Count > 0)
            {
                string qustr = " and ( a.id!='" + not_friend[0] + "'";
                //addstr += " and ( a.id!='" + not_friend[0] + "'";
                for (int i = 1; i < not_friend.Count; i++)
                {
                    qustr += " or a.id!='" + not_friend[i] + "'";
                    //addstr += " or a.id!='" + not_friend[i] + "'";
                }
                qustr += ")";
                //addstr += ")";
                Query1 += qustr;
            }
            //not friend
            if (check_sort_pop)
            {
                //addstr += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ") as t)";
                //Query1 += addstr;
                Query1 += " ORDER BY a.likecount desc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            }
            else
            {
                //addstr += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ") as t)";
                //Query1 += addstr;
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

$('#btnFileUpload" + ((counn - 10) + i) + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar" + ((counn - 10) + i) + @"').show();
        $('#image_place" + ((counn - 10) + i) + @"').hide();
        $('#image_place" + ((counn - 10) + i) + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar" + ((counn - 10) + i) + @"').hide();
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', '0%');
        $('#image_place" + ((counn - 10) + i) + @"').show();
        document.getElementById('make-image" + ((counn - 10) + i) + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar" + ((counn - 10) + i) + @"').hide();
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', '0%');
        $('#image_place" + ((counn - 10) + i) + @"').hide();
        $('#image_place" + ((counn - 10) + i) + @" div').css('width', '0%');
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

$('.hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.box" + ((counn - 10) + i) + @"').click(function () {
                $('.hidde" + ((counn - 10) + i) + @"').toggle();
                $('.box" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.likehidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.likebox" + ((counn - 10) + i) + @"').click(function () {
                $('.likehidde" + ((counn - 10) + i) + @"').toggle();
                $('.likebox" + ((counn - 10) + i) + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
            })

            $('.likehidde" + ((counn - 10) + i) + @"').click(function () {
                $('.likebox" + ((counn - 10) + i) + @"').toggle();
                $('.likehidde" + ((counn - 10) + i) + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
            })

            $('.mess_hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.mess_box" + ((counn - 10) + i) + @"').click(function () {
                $('.mess_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.mess_box" + ((counn - 10) + i) + @"').toggle(false);
            })


            $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.big_mess_box" + ((counn - 10) + i) + @"').click(function () {
                $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.big_mess_box" + ((counn - 10) + i) + @"').toggle(false);
                $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.status_message_box" + ((counn - 10) + i) + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",1);
            })

            $('.big_mess_hidde" + ((counn - 10) + i) + @"').click(function () {
                $('.big_mess_box" + ((counn - 10) + i) + @"').toggle();
                $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle(false);
                $('.status_message_box" + ((counn - 10) + i) + @"').toggle();
                $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle(false);
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",1);
            })

            $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle(false);


";

//                SqlDataSource sql3 = new SqlDataSource();
//                sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                Query1 = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                DataView ict2 = gc1.select_cmd(Query1);;
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
                result_res += "<div id='state_mess_" + ((counn - 10) + i) + "' style='background-color: #FFF;'onmouseover='hover(" + ((counn - 10) + i) + ")' onmouseout='out(" + ((counn - 10) + i) + ")'>";

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
                    result_res += "授乳室、";
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
                result_res += "<br/><div class='box" + ((counn - 10) + i) + "'>";
                if (ict.Table.Rows[i]["message"].ToString().Length < 37)
                {
                    result_res += main.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString());
                }
                else
                {
                    result_res += ict.Table.Rows[i]["message"].ToString().Substring(0, 37) + "‧‧‧";
                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>もっと見る</a>";
                }


                result_res += "</div>";
                result_res += "<div class='hidde" + ((counn - 10) + i) + "'>";

                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + main.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString()) + "</span>";

                result_res += "<br/>";


                //if (ict.Table.Rows[i]["message"].ToString().Length > 36)
                //{
                //    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>たたむ</a>";

                //}


                result_res += "</div>";
                result_res += "<div>";
                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + main.ConvertUrlsToLinks_DIV(ict.Table.Rows[i]["message"].ToString()) + "</span>";
                result_res += "</div>";
                result_res += "</td>";
                result_res += "</tr>";
                //poster images
                string shareimg = "";
                result_res += "<tr>";
                result_res += "<td colspan='2' width='90%' align='center'><br/><br/>";

                Query1 = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict1 = gc1.select_cmd(Query1);
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                int typ = Convert.ToInt32(rand.Next(0, ict1.Count));
                if (ict1.Count > 3)
                {
                    result_res += "<div class='imbox" + ((counn - 10) + i) + "'>";
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
                    result_res += "<div id='freewall" + ((counn - 10) + i) + "'>";
                    result_res +="<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
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
                        if (shareimg == "")
                        {
                            shareimg = cutstr1;
                        }
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
                                result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                                result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";

                                result_res +="</a>";
                                result_res +="</div>";
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
                    result_res +="</div>";
                    //
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>
                                        var wall" + ((counn - 10) + i) + @" = new Freewall('#freewall" + ((counn - 10) + i) + @"');
                    			wall" + ((counn - 10) + i) + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + ((counn - 10) + i) + @".fitWidth();
                    				}
                    			});
                    			wall" + ((counn - 10) + i) + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res +=litjs.Text;


                    result_res +="</div>";

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
                    if (shareimg == "")
                    {
                        shareimg = cutstr1;
                    }
                    if (ict1.Count == 1)
                    {
                        result_res +="<div class='zoom-gallery'>";
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr1 + "' src='images/loading.gif' style='width:100%;height:100%;'/>";
                        result_res +="</a>";
                        result_res +="</div>";
                    }
                    else if (ict1.Count == 2)
                    {
                        result_res +="<div id='freewall" +  ((counn - 10) + i) + "'>";
                        result_res +="<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res +="<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";
                        }
                        result_res +="</div>";
                        result_res +="</div>";
                        result_res +="</div>";
                    }
                    else if (ict1.Count == 3)
                    {
                        result_res +="<div id='freewall" + ((counn - 10) + i) + "'>";
                        result_res +="<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res +="<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                        }
                        else if (typ == 1)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";
                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res +="<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res +="<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res +="</a>";
                            result_res +="</div>";
                        }
                        result_res +="</div>";
                        result_res +="</div>";
                        result_res +="</div>";
                    }
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>

                                        var wall" + ((counn - 10) + i) + @" = new Freewall('#freewall" + ((counn - 10) + i) + @"');
                    			wall" + ((counn - 10) + i) + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + ((counn - 10) + i) + @".fitWidth();
                    				}
                    			});
                    			wall" + ((counn - 10) + i) + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res +=litjs.Text;

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
                        DataView ict_f_like = gc1.select_cmd(Query1);;
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
                result_res += "<div style='cursor: pointer' class='likebox" + ((counn - 10) + i) + "'>";

                Image img1 = new Image();
                if (check_li)
                {
                    string cutstr = "~/images/like.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lalike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' style='color:#F06767;font-size:10pt;'>いいね</span>";
                }
                else
                {
                    string cutstr = "~/images/like_b.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lablike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' style='color:#CCCCCC;font-size:10pt;'>いいね</span>";
                }


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='likehidde" + ((counn - 10) + i) + "'>";
                img1 = new Image();
                if (check_li)
                {
                    string cutstr = "~/images/like_b.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lablike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' style='color:#CCCCCC;font-size:10pt;'>いいね</span>";
                }
                else
                {
                    string cutstr = "~/images/like.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lalike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' style='color:#F06767;font-size:10pt;'>いいね</span>";
                }
                result_res += "</div>";
                result_res += "</td>";




                result_res += "<td>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td align='center'><br/><br/>";
                result_res += "<div style='cursor: pointer' class='big_mess_box" + ((counn - 10) + i) + "'>";

                string cutstr_m = "~/images/mess_b.png";
                int ind_m = cutstr_m.IndexOf(@"/");
                string cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#CCCCCC;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='big_mess_hidde" + ((counn - 10) + i) + "'>";

                cutstr_m = "~/images/mess.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#676767;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "</td>";
                result_res += "<td align='left'><br/><br/>";
                result_res += "<div id='sharebox" + ((counn - 10) + i) + "' style='cursor: pointer'>";
                //pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
                cutstr_m = "~/images/share_b.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#CCCCCC;font-size:10pt;'>シェア</span>";

                result_res += "</div>";
                int len = ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length;
                if (ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length > 99)
                {
                    len = 99;
                }
                li = new Literal();
                li.Text = @"
                       <div id='share_div" + ((counn - 10) + i) + @"' title='シェア' style='display:none;'><table width='100%'><tr><td><div id='facebook_share" + ((counn - 10) + i) + @"' class='jssocials-share jssocials-share-facebook'><a href='#' class='jssocials-share-link'><i class='fa fa-facebook jssocials-share-logo'></i></a></div></div></td><td><div id='share_div_" + ((counn - 10) + i) + @"'></div></td></tr><tr><td colspan='2'><div id='share_div__" + ((counn - 10) + i) + @"'></div></td></tr></table>

                       <script type='text/javascript'>
  $(function() {
$('#share_div_" + ((counn - 10) + i) + @"').jsSocials({
            showLabel: false,
            showCount: false,
            shares: ['email', 'twitter', 'googleplus', 'line'],
            url: 'http://.jp/',
            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
            shareIn: 'popup'
        });
$('#share_div" + ((counn - 10) + i) + @"').dialog({
                autoOpen: false,
                show: {
                    effect: 'fold',
                    duration: 100
                },
                hide: {
                    effect: 'fold',
                    duration: 100
                }
            });
   $('#sharebox" + ((counn - 10) + i) + @"').on('click', function () {
                $('#share_div" + i + @"').dialog('open');
                webaction(" + ict.Table.Rows[i]["id"].ToString() + @",2);
           });
$('#facebook_share" + ((counn - 10) + i) + @"').on('click', function () {
               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "") + @"”');

           });
 });
</script>

    ";
                result_res += li.Text;

                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";

                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</td>";
                result_res += "<td style='vertical-align: top';>";

                //report
                cutstr_m = "~/images/report.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img id='" + "reportstate_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='report_mess(this.id)' src='" + cutstr_m1 + "' style='cursor: pointer'>";
                li = new Literal();

                //report div
                li.Text = @"
<div id='dlgbox_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlg'>
            <div id='dlg-header_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgh'>問題の内容についてお聞かせください</div>
            <div id='dlg-body_report_" + ict.Table.Rows[i]["id"].ToString() + @"' style='height: 200px; overflow: auto' class='dlgb'>
                <table style=' width: 100%;'>
                    <tr>
                        <td>
                            <table style='width: 100%;'>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span style='font-weight: bold;font-size:medium;'>詳細を入力してください。</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='不快または面白くない' style='margin-right: 15px;'> <span style='font-size:medium;'>不快または面白くない</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='に載せるべきではないと思う' style='margin-right: 15px;'><span style='font-size:medium;'>に載せるべきではないと思う</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='スパムである' style='margin-right: 15px;'><span style='font-size:medium;'>スパムである</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span id='reportla_" + ict.Table.Rows[i]["id"].ToString() + @"' style='color:red;font-size:medium;'></span><br/>
                                    </td>
                                </tr>
                            </table>
                            <hr/>
                        </td>
</tr>
                </table>
            </div>
            <div id='dlg-footer_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgf'>
<table style=' width: 100%;'>
<tr>
<td width='50%' align='left'>
<input id='reportstatebutcancel_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='取り消す' onclick='dlgrecanel(this.id)' class='file-upload1'/>
</td>
<td width='50%' align='right'>
                <input id='reportstatebutedit_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='保存' onclick='dlgreport(this.id)' class='file-upload1'/>
</td>
            </tr>
</table>
</div>
        </div>
";
                result_res += li.Text;

                //report
                result_res += "</td>";
                result_res += "</tr>";

                result_res += "<tr style='background-color:#F6F7F9;'>";
                result_res += "<td></td>";
                result_res += "<td>";



                result_res += "<div class='status_message_box" + ((counn - 10) + i) + "' style='background-color: #ffffff'>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "<td width='90%' height='5%'></td>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</div >";
                result_res += "<div class='status_message_hidde" + ((counn - 10) + i) + "' style='background-color: #dddddd'>";


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
                        result_res += "<a href='user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

                        result_res += "、";
                    }
                    result_res += "<a id='listlike_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='check_like_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;'>他" + (ict3.Count - 2) + "人</a>";


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
                        result_res += "<a href='user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

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
                        DataView ict5 =gc1.select_cmd(Query1);
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
                    result_res += "<div class='mess_box" + ((counn - 10) + i) + "'>";
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

                    Query1 = "select good_status from status_messages_user_talk_like";
                    Query1 += " where smutid='" + talk_list_tmp[0].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                    DataView ict_who_like = gc1.select_cmd(Query1);
                    if (ict_who_like.Count > 0)
                    {
                        if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='sblike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                        }
                        else
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='slike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";
                        }
                    }
                    else
                    {
                        result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='sblike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                    }


                    result_res += "&nbsp;&nbsp;";

                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

                    //who like who answer post message
                    Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                    Query1 += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                    //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_who_like = gc1.select_cmd(Query1);
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
                    result_res += "<div class='mess_hidde" + ((counn - 10) + i) + "'>";
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

                        result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";
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
                        Query1 = "select good_status from status_messages_user_talk_like";
                        Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                        ict_who_like = gc1.select_cmd(Query1);
                        if (ict_who_like.Count > 0)
                        {
                            if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                            }
                            else
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='like_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";
                            }
                        }
                        else
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                        }
                        result_res += "&nbsp;&nbsp;";
                        result_res += "<a id='" + "whowantans_" + talk_list_tmp[iiii].id + "' onclick='who_answer(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

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
                            result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";

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

                            Query1 = "select good_status from status_messages_user_talk_like";
                            Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                            DataView ict_who_like = gc1.select_cmd(Query1);
                            if (ict_who_like.Count > 0)
                            {
                                if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                                {
                                    result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                                }
                                else
                                {
                                    result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='like_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";

                                }
                            }
                            else
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                            }
                            result_res += "&nbsp;&nbsp;";
                            result_res += "<a id='" + "whowantans_" + talk_list_tmp[iiii].id + "' onclick='who_answer(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

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

                Query1 = "select photo,username from user_login ";
                Query1 += " where id='" + HttpContext.Current.Session["id"].ToString() + "';";
                DataView ict2 = gc1.select_cmd(Query1);
                string userr = "";
                if (ict2.Count > 0)
                {
                    cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    userr = ict2.Table.Rows[0]["username"].ToString();
                }

                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:32px;height:32px;'>";
                result_res += "<img src='" + cutstr3 + "' width='32' height='32' />";
                result_res += "</a>";

                result_res += "</div>";

                result_res += "</td>";
                result_res += "<td width='85%'>";

                //user answer
                result_res += "<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_" + ((counn - 10) + i) + "' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;' title='【Enter】キーを押してください'>";
                //TextBox tex = new TextBox();
                //tex.Width = Unit.Percentage(95);
                //tex.Height = 30;
                //tex.ID = "send" + ict.Table.Rows[i]["id"].ToString();
                //tex.Attributes["onKeydown"] = "Javascript: if (event.which == 13 || event.keyCode == 13) sendmessage(this.id);";
                //tex.Attributes.Add("placeholder", "コメントする");
                //pdn2.Controls.Add(tex);

                //pdn2.Controls.Add(new LiteralControl("<br/>"));

                result_res += @"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload" + ((counn - 10) + i) + @"' />
</label>
<br />
            <div id='progressbar" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image" + ((counn - 10) + i) + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>
";




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
        }



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
    [WebMethod(EnableSession = true)]
    public static string FB_res(string param1, string param2, string param3, string param4, string param5)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result_res = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5;

                Query1 = "select id,username from user_login";
                Query1 += " where FBID='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                        Query1 = "select UserId from UserActivation";
                        Query1 += " where UserId='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                        DataView ict_check = gc1.select_cmd(Query1);
                        if (ict_check.Count > 0)
                        {
                            result_res = "three";
                        }
                        else
                        {
                            Query1 = "update user_login set LastLoginDate=NOW()";
                            Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                            resin = gc1.update_cmd(Query1);

                            HttpContext.Current.Session["id"] = ict_f.Table.Rows[0]["id"].ToString();
                            HttpContext.Current.Session["loginname"] = ict_f.Table.Rows[0]["username"].ToString();
                            result_res = "one";
                        }
                }
                else
                {
                    result_res = "two";
                    HttpContext.Current.Session["FB_name"] = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                    HttpContext.Current.Session["FB_id"] = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                    HttpContext.Current.Session["FB_email"] = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                    HttpContext.Current.Session["FB_pic"] = @"http://graph.facebook.com/" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + @"/picture";
                    //HttpContext.Current.Session["FB_pic"] = param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                    HttpContext.Current.Session["FB_cov"] = param5.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                }





        return result_res;
    }
    [WebMethod(EnableSession = true)]
    public static string user_action(string param1, string param2)
    {
          GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";
        if (HttpContext.Current.Session["id"] != null)
        {
            string uid = HttpContext.Current.Session["id"].ToString();
            if (uid.Trim() != "")
            {
                string type = "";
                if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "0")
                {
                    type = "user_like";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "1")
                {
                    type = "user_message";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "2")
                {
                    type = "user_share";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "3")
                {
                    type = "user_answer";
                }




                Query1 = "select id from user_action";
                Query1 += " where uid='" + uid + "' and smid='" + param1 + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    //update
                    //ict_f.Table.Rows[0]["id"].ToString()
                    Query1 = "update user_action set " + type + "=" + type + "+1";
                    Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                    resin = gc1.update_cmd(Query1);
                }
                else
                {
                    //insert
                   Query1 = "insert into user_action(smid,uid,user_like,user_message,user_share,user_answer)";
                   Query1 += " values('" + param1 + "','" + uid + "','0','0','0','0');";
                   resin = gc1.insert_cmd(Query1);

                    Query1 = "select id from user_action";
                    Query1 += " where uid='" + uid + "' and smid='" + param1 + "';";

                    DataView ict_f1 = gc1.select_cmd(Query1);
                    if (ict_f1.Count > 0)
                    {
                        Query1 = "update user_action set " + type + "=" + type + "+1";
                        Query1 += " where id='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                        resin = gc1.update_cmd(Query1);
                    }


                }

            }
        }

        return result;
    }
    [WebMethod]
    public static string[] count_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result = param1;
        string[] result_res = new string[3];
        result = "";
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1_f = gc1.select_cmd(Query1);
        if (ict_f1_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f1_f.Count; ii++)
            {
                user_friend.Add(ict_f1_f.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message
        //status message

        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();
        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            DataView ict_f12 = gc1.select_cmd(Query1);
            if (ict_f12.Count > 0)
            {
                for (int ii = 0; ii < ict_f12.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f12.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f12.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f12.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f12 = gc1.select_cmd(Query1);
            if (ict_f12.Count > 0)
            {
                for (int ii = 0; ii < ict_f12.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f12.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f12.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f12.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f12.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f12.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message
        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer

                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message

        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {

                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }


                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));
        DateTime nowtime = DateTime.Now;
        DateTime clicktime = new DateTime(2000,1,1);
        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";

        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newmessage = 0;
        for (int i = 0; i < status_mess_like.Count; i++)
        {
            int year = status_mess_like[i].comdate.Year;
            int month = status_mess_like[i].comdate.Month;
            int day = status_mess_like[i].comdate.Day;
            int hour = status_mess_like[i].comdate.Hour;
            int min = status_mess_like[i].comdate.Minute;
            int sec = status_mess_like[i].comdate.Second;
            DateTime mesgdate = new DateTime(year,month,day,hour,min,sec);
            if (mesgdate > clicktime && mesgdate < nowtime)
            {
                newmessage += 1;
            }
        }
        result_res[0] = newmessage.ToString();


        //chat list count

        Query1 = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1 + "'";
        Query1 += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f = gc1.select_cmd(Query1);

        List<friend_list_chat> fri = new List<friend_list_chat>();
        friend_list_chat frii = new friend_list_chat();
        int tempid = 0;
        for (int i = 0; i < ict_f.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
        }


        Query1 = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where c.id=" + param1 + "";
        Query1 += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f1 = gc1.select_cmd(Query1);
        tempid = 0;
        for (int i = 0; i < ict_f1.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f1.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f1.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f1.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
        }

        fri = fri.OrderBy(c => c.id).ToList();

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //        .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();

        List<friend_list_chat> tmp_fri = new List<friend_list_chat>();
        List<friend_list_chat> fri_total = new List<friend_list_chat>();
        frii = new friend_list_chat();
        List<int> fri_ind = new List<int>();
        tempid = 0;
        for (int i = 0; i < fri.Count; i++)
        {
            if (tempid != fri[i].id)
            {
                tempid = fri[i].id;
                fri_ind.Add(tempid);
            }
        }
        for (int i = 0; i < fri_ind.Count; i++)
        {
            tmp_fri = new List<friend_list_chat>();
            for (int ii = 0; ii < fri.Count; ii++)
            {
                if (fri_ind[i] == fri[ii].id)
                {
                    tmp_fri.Add(fri[ii]);
                }
            }
            tmp_fri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            fri_total.Add(tmp_fri[tmp_fri.Count - 1]);
        }
        fri_total.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //       .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();
        fri = fri_total;
        nowtime = DateTime.Now;
        clicktime = new DateTime(2000, 1, 1);

        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='1';";

        ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newchat = 0;
        for (int i = 0; i < fri.Count; i++)
        {

            int year = fri[i].year;
            int month = fri[i].month;
            int day = fri[i].day;
            int hour = fri[i].hour;
            int min = fri[i].min;
            int sec = fri[i].sec;
            DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
            if (mesgdate > clicktime && mesgdate < nowtime)
            {
                newchat += 1;
            }
        }
        result_res[1] = newchat.ToString();
        nowtime = DateTime.Now;
        clicktime = new DateTime(2000, 1, 1);

        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='2';";

        ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newfri = 0;
        Query1 = "select a.id,a.first_uid,b.username,b.photo,a.first_date_year,a.first_date_month,a.first_date_day,a.first_date_hour,a.first_date_minute,a.first_date_second ";
        Query1 += "from user_friendship as a inner join user_login as b on a.first_uid=b.id where a.second_uid='" + param1 + "' and a.second_check_connect='0'";
        Query1 += " ORDER BY a.first_date_year desc,a.first_date_month desc,a.first_date_day desc,a.first_date_hour desc,a.first_date_minute desc,a.first_date_second desc;";
        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_second"].ToString());
                DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
                if (mesgdate > clicktime && mesgdate < nowtime)
                {
                    newfri += 1;
                }
            }
        }
        result_res[2] = newfri.ToString();



        return result_res;
    }


}
