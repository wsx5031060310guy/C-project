using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_google_area : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
//        //new version
//        SqlDataSource sql1 = new SqlDataSource();
//        sql1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//        sql1.SelectCommand = "select b.jis ";
//        sql1.SelectCommand += "from JP_City as b";
//        sql1.SelectCommand += " where b.jis='01102';";
//        sql1.DataBind();
//        DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
//        Literal li = new Literal();

//        li.Text = @"<script>
//var map1;
//        var infoWindow1;
//function initMap1() {
//            map1 = new google.maps.Map(document.getElementById('map'), {
//                zoom: 12,
//                center: { lat: 34.9198341369629, lng: 136.880279541016 },
//                mapTypeId: google.maps.MapTypeId.ROADMAP
//            });
//        ";
//        int couuu = 0;
//        if (ict.Count > 0)
//        {
//            for (int i = 0; i < ict.Count; i++)
//            {
//                SqlDataSource sql2 = new SqlDataSource();
//                sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                sql2.SelectCommand = "select e.lat,e.lng";
//                sql2.SelectCommand += " from JP_City as b";
//                sql2.SelectCommand += " inner join JP_City_G as c on b.jis=c.jis";
//                sql2.SelectCommand += " inner join JP_City_Town_G as d on c.con_id=d.City_id";
//                sql2.SelectCommand += " inner join JP_Town_G as e on d.place_id=e.place_id";
//                sql2.SelectCommand += " where b.jis='" + ict.Table.Rows[i]["jis"].ToString() + "';";
//                sql2.DataBind();
//                DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
//                if (ict1.Count > 0)
//                {
//                    li.Text += @"var triangleCoords" + couuu + @" = [
//";
//                    string str = "";
//                    for (int ix = 0; ix < ict1.Count; ix++)
//                    {
//                        str += "{ lat: " + ict1.Table.Rows[ix]["lat"].ToString() + ", lng: " + ict1.Table.Rows[ix]["lng"].ToString() + " },";
//                    }
//                    if (str.Length > 0)
//                    {
//                        str.Substring(0, str.Length - 1);
//                    }
//                    li.Text += str;
//                    li.Text += @"];
//
//            // Construct the polygon.
//            var bermudaTriangle" + couuu + @" = new google.maps.Polygon({
//                paths: triangleCoords" + couuu + @",
//                strokeColor: '#FF0000',
//                strokeOpacity: 0.2,
//                strokeWeight: 3,
//                fillColor: '#FF0000',
//                fillOpacity: 0.35
//            });
//            bermudaTriangle" + couuu + @".setMap(map1);
//
//bermudaTriangle" + couuu + @".addListener('click', showArrays1);
//            infoWindow1 = new google.maps.InfoWindow;";

//                    couuu += 1;



//                }


//            }
//        }
//        li.Text += @"
// /** @this {google.maps.Polygon} */
//        function showArrays1(event) {
//            // Since this polygon has only one path, we can call getPath() to return the
//            // MVCArray of LatLngs.
//            var vertices = this.getPath();
//
//            var contentString = '<b>Bermuda Triangle polygon</b><br>' +
//                'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
//                '<br>';
//
//            // Iterate over the vertices.
//            for (var i = 0; i < vertices.getLength() ; i++) {
//                var xy = vertices.getAt(i);
//                contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
//                    xy.lng();
//            }
//
//            // Replace the info window's content and position.
//            infoWindow1.setContent(contentString);
//            infoWindow1.setPosition(event.latLng);
//
//            infoWindow1.open(map1);
//        }
//
//}
//</script>";
//        Panel1.Controls.Add(li);



        //new version too slow
        SqlDataSource sql1 = new SqlDataSource();
        sql1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql1.SelectCommand = "select b.jis ";
        sql1.SelectCommand += "from JP_State as a";
        sql1.SelectCommand += " inner join JP_City as b on a.id=b.JP_State_id";
        sql1.SelectCommand += " where a.id='14';";
        sql1.DataBind();
        DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
        Literal li = new Literal();

        li.Text = @"<script>
var map1;
        var infoWindow1;
function initMap1() {
            map1 = new google.maps.Map(document.getElementById('map'), {
                zoom: 12,
                center: { lat: 34.9198341369629, lng: 136.880279541016 },
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
        ";
        int couuu = 0;
        if (ict.Count > 0)
        {
            for (int i = 0; i < ict.Count; i++)
            {
                SqlDataSource sql3 = new SqlDataSource();
                sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql3.SelectCommand = "select c.con_id";
                sql3.SelectCommand += " from JP_City as b";
                sql3.SelectCommand += " inner join JP_City_G as c on b.jis=c.jis";
                sql3.SelectCommand += " where b.jis='" + ict.Table.Rows[i]["jis"].ToString() + "';";
                sql3.DataBind();
                DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
                if (ict2.Count > 0)
                {
                    for (int ii = 0; ii < ict2.Count; ii++)
                    {
                        //
                        SqlDataSource sql2 = new SqlDataSource();
                        sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql2.SelectCommand = "select e.lat,e.lng";
                        sql2.SelectCommand += " from JP_City_G as c";
                        sql2.SelectCommand += " inner join JP_City_Town_G as d on c.con_id=d.City_id";
                        sql2.SelectCommand += " inner join JP_Town_G as e on d.place_id=e.place_id";
                        sql2.SelectCommand += " where c.con_id='" + ict2.Table.Rows[ii]["con_id"].ToString() + "';";
                        sql2.DataBind();
                        DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
                        if (ict1.Count > 0)
                        {
                            li.Text += @"var triangleCoords" + couuu + @" = [
";
                            string str = "";
                            for (int ix = 0; ix < ict1.Count; ix++)
                            {
                                str += "{ lat: " + ict1.Table.Rows[ix]["lat"].ToString() + ", lng: " + ict1.Table.Rows[ix]["lng"].ToString() + " },";
                            }
                            if (str.Length > 0)
                            {
                                str.Substring(0, str.Length - 1);
                            }
                            li.Text += str;
                            li.Text += @"];

            // Construct the polygon.
            var bermudaTriangle" + couuu + @" = new google.maps.Polygon({
                paths: triangleCoords" + couuu + @",
                strokeColor: '#FF0000',
                strokeOpacity: 0.2,
                strokeWeight: 3,
                fillColor: '#FF0000',
                fillOpacity: 0.35
            });
            bermudaTriangle" + couuu + @".setMap(map1);

bermudaTriangle" + couuu + @".addListener('click', showArrays1);
            infoWindow1 = new google.maps.InfoWindow;";

                            couuu += 1;



                        }
                        //
                    }
                }

               




            }
        }
        li.Text += @"
 /** @this {google.maps.Polygon} */
        function showArrays1(event) {
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
            infoWindow1.setContent(contentString);
            infoWindow1.setPosition(event.latLng);

            infoWindow1.open(map1);
        }

}
</script>";
        Panel1.Controls.Add(li);



        ////old version 
//        SqlDataSource sql1 = new SqlDataSource();
//        sql1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//        sql1.SelectCommand = "select b.id ";
//        sql1.SelectCommand += "from JP_State as a";
//        sql1.SelectCommand += " inner join JP_City as b on a.id=b.JP_State_id";
//        sql1.SelectCommand += " where a.id='43';";
//        sql1.DataBind();
//        DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
//        Literal li = new Literal();

//        li.Text = @"<script>
//var map1;
//        var infoWindow1;
//function initMap1() {
//            map1 = new google.maps.Map(document.getElementById('map'), {
//                zoom: 12,
//                center: { lat: 34.9198341369629, lng: 136.880279541016 },
//                mapTypeId: google.maps.MapTypeId.ROADMAP
//            });
//        ";
//        int couuu = 0;
//        if (ict.Count > 0)
//        {
//            for (int i = 0; i < ict.Count; i++)
//            {
//                SqlDataSource sql2 = new SqlDataSource();
//                sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                sql2.SelectCommand = "select max(num) as howmany ";
//                sql2.SelectCommand += "from JP_City as b";
//                sql2.SelectCommand += " inner join JP_Town as c on b.id=c.JP_City_id";
//                sql2.SelectCommand += " where b.id='" + ict.Table.Rows[i]["id"].ToString() + "';";
//                sql2.DataBind();
//                DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
//                if (ict1.Count > 0)
//                {
//                    if (ict1.Table.Rows[0]["howmany"].ToString() != "")
//                    {
//                        int howmany = Convert.ToInt32(ict1.Table.Rows[0]["howmany"].ToString());

//                        for (int ii = 0; ii <= howmany; ii++)
//                        {
//                            SqlDataSource sql3 = new SqlDataSource();
//                            sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                            sql3.SelectCommand = "select c.lat,c.lng ";
//                            sql3.SelectCommand += "from JP_City as b";
//                            sql3.SelectCommand += " inner join JP_Town as c on b.id=c.JP_City_id";
//                            sql3.SelectCommand += " where b.id='" + ict.Table.Rows[i]["id"].ToString() + "' and c.num='" + ii + "';";
//                            sql3.DataBind();
//                            DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//                            if (ict2.Count > 0)
//                            {
//                                li.Text += @"var triangleCoords" + couuu + @" = [
//";
//                                string str = "";
//                                for (int ix = 0; ix < ict2.Count; ix++)
//                                {
//                                    str += "{ lat: " + ict2.Table.Rows[ix]["lat"].ToString() + ", lng: " + ict2.Table.Rows[ix]["lng"].ToString() + " },";
//                                }
//                                if (str.Length > 0)
//                                {
//                                    str.Substring(0, str.Length - 1);
//                                }
//                                li.Text += str;
//                                li.Text += @"];
//
//            // Construct the polygon.
//            var bermudaTriangle" + couuu + @" = new google.maps.Polygon({
//                paths: triangleCoords" + couuu + @",
//                strokeColor: '#FF0000',
//                strokeOpacity: 0.2,
//                strokeWeight: 3,
//                fillColor: '#FF0000',
//                fillOpacity: 0.35
//            });
//            bermudaTriangle" + couuu + @".setMap(map1);
//
//bermudaTriangle" + couuu + @".addListener('click', showArrays1);
//            infoWindow1 = new google.maps.InfoWindow;";

//                                couuu += 1;
//                            }

//                        }
//                    }
//                }


//            }
//        }
//        li.Text += @"
// /** @this {google.maps.Polygon} */
//        function showArrays1(event) {
//            // Since this polygon has only one path, we can call getPath() to return the
//            // MVCArray of LatLngs.
//            var vertices = this.getPath();
//
//            var contentString = '<b>Bermuda Triangle polygon</b><br>' +
//                'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
//                '<br>';
//
//            // Iterate over the vertices.
//            for (var i = 0; i < vertices.getLength() ; i++) {
//                var xy = vertices.getAt(i);
//                contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
//                    xy.lng();
//            }
//
//            // Replace the info window's content and position.
//            infoWindow1.setContent(contentString);
//            infoWindow1.setPosition(event.latLng);
//
//            infoWindow1.open(map1);
//        }
//
//}
//</script>";
//        Panel1.Controls.Add(li);




    }
    [WebMethod]
    public static string insert_city(string param1)
    {
        string result1 = "";
        string result = "";
        if (param1 == "0")
        {
            SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from JP_State;";
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                SqlDataSource sql_insert = new SqlDataSource();

                if (ict_f.Count > 0)
                {
                    for (int i = 0; i < ict_f.Count; i++)
                    {
                        int id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                        string idd = ict_f.Table.Rows[i]["id"].ToString();
                        if (id < 10)
                        {
                            idd = "0" + ict_f.Table.Rows[i]["id"].ToString();
                        }
                        var url = "http://www.land.mlit.go.jp/webland/api/CitySearch?area=" + HttpContext.Current.Server.UrlEncode(idd);
                        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                        using (var response = request.GetResponse())
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                        }
                        Newtonsoft.Json.Linq.JObject jArray = Newtonsoft.Json.Linq.JObject.Parse(result);
                        foreach (var item in jArray["data"])
                        {
                            sql_insert = new SqlDataSource();
                            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_insert.InsertCommand = "insert into JP_City(JP_State_id,jis,city_jp)";
                            sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[i]["id"].ToString() + "','" + (string)item["id"] + "','" + (string)item["name"] + "')";
                            sql_insert.Insert();
                        }



                        url = "http://www.land.mlit.go.jp/webland_english/api/CitySearch?area=" + HttpContext.Current.Server.UrlEncode(idd);
                        request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                        using (var response = request.GetResponse())
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                        }
                        jArray = Newtonsoft.Json.Linq.JObject.Parse(result);
                        foreach (var item in jArray["data"])
                        {
                            SqlDataSource sql_f1 = new SqlDataSource();
                            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_f1.SelectCommand = "select id from JP_City where JP_State_id='" + ict_f.Table.Rows[i]["id"].ToString() + "' and jis='" + (string)item["id"] + "';";
                            DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                            if (ict_f.Count > 0)
                            {
                                SqlDataSource sql_update = new SqlDataSource();
                                sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                                sql_update.UpdateCommand = "update JP_City set city_en='" + (string)item["name"] + "' where id='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                                sql_update.Update();
                            }
                        }





                    }
                }

           

        }
        return result1;
    }

}