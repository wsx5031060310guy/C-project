using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

public partial class mamaro : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string result_cmd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session.Abandon();
        //Session.Clear();
        //if (!Page.IsPostBack)
        //{
        //    Session.Clear();
        //}
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Session.Clear();
        //string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamaro"]) ? Request.QueryString["mamaro"] : Guid.Empty.ToString();
        string text = Page.Request.QueryString.Get("id");
        string id = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-zA-Z0-9\s]", string.Empty);
        Session["mid"] = id;
        string text1 = Page.Request.QueryString.Get("QA");
        string QAon=System.Text.RegularExpressions.Regex.Replace(text1, @"[^a-zA-Z0-9\s]", string.Empty);
        if (QAon == "1")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "dlg_show();", true);
        }


        //Session["lan"] = "日本語";
        //Session["byear"] = "0";
        //Session["bmonth"] = "0";
        //Session["parent"] = "ママ";
        //Session["QA1"] = null;
        //Session["QA2"] = null;
        //Session["QA3"] = null;
        //Session["QA4"] = null;

        //time
        TimeZoneInfo TPZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        DateTime indate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TPZone);
        string week = "";
        if (indate.DayOfWeek == DayOfWeek.Monday) { week = "MON"; }
        else if (indate.DayOfWeek == DayOfWeek.Tuesday) { week = "TUE"; }
        else if (indate.DayOfWeek == DayOfWeek.Wednesday) { week = "WED"; }
        else if (indate.DayOfWeek == DayOfWeek.Thursday) { week = "THU"; }
        else if (indate.DayOfWeek == DayOfWeek.Friday) { week = "FRI"; }
        else if (indate.DayOfWeek == DayOfWeek.Saturday) { week = "SAT"; }
        else if (indate.DayOfWeek == DayOfWeek.Sunday) { week = "SUN"; }

        Label1.Text = indate.Month.ToString() + "/" + indate.Day.ToString() + "(" + week + ")";
        Label2.Text = indate.ToString("HH:mm");

        string res = LoadWeather();
        Panel1.Controls.Add(new LiteralControl(res));

        string cre = "<span class='bottom_logo'>Created by</span><img src='" + @"img\img\ver1\Trim_logo_grey.png' width='90' height='90'>";

        Panel3.Controls.Add(new LiteralControl(cre));



        //Query = "select *,TIMESTAMPDIFF(minute, update_time, NOW()) as difftime from status_messages_link_info order by difftime DESC LIMIT 50;";

        //DataView ict2 = gc.select_cmd(Query);
        //URL_data URL_info = new URL_data();
        //if (ict2.Count > 0)
        //{
        //    for (int i = 0; i < ict2.Count; i++)
        //    {
        //        if (Convert.ToInt32(ict2.Table.Rows[i]["difftime"].ToString()) > 30)
        //        {
        //        }
        //    }
        //}

        string GPS_lat = "", GPS_lng = "", result = "";

        Query = "select logo,address,GPS_lat,GPS_lng,postal_code from nursing_room where id='" + id + "';";

        DataView ict2 = gc.select_cmd(Query);
        if (ict2.Count > 0)
        {
            GPS_lat = ict2.Table.Rows[0]["GPS_lat"].ToString();
            GPS_lng = ict2.Table.Rows[0]["GPS_lng"].ToString();
            zipcode = ict2.Table.Rows[0]["postal_code"].ToString();
        }
        bool check_area_same = false;
        if (zipcode.Trim() == "")
        {
            check_area_same = true;
        }

        if (check_area_same)
        {
            bool check_gps = false;
            if (GPS_lat.Trim() == "")
            {
                check_gps = true;
            }
            if (GPS_lng.Trim() == "")
            {
                check_gps = true;
            }
            if (check_gps)
            {
                GPS_lat = "35.447821";
                GPS_lng = "139.641685";
            }
            //no GPS no weather and area
            if (GPS_lat.Trim() != "" && GPS_lng.Trim() != "")
            {
                //get location
                //get area
                addr_type addr = new addr_type();
                string city = "", state = "", country = "", city1 = "", city2 = "", city3 = "";
                result = "";
                var url_loc = "http://maps.google.com/maps/api/geocode/json?sensor=true&language=ja&address=" + GPS_lat + "," + GPS_lng;

                System.Net.HttpWebRequest request_loc = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url_loc);
                using (var response = request_loc.GetResponse())
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                if (result != "")
                {
                    Newtonsoft.Json.Linq.JObject jArray_loc = Newtonsoft.Json.Linq.JObject.Parse(result);
                    string address_for = jArray_loc["results"][0]["formatted_address"].ToString();
                    foreach (var item in jArray_loc["results"][0]["address_components"])
                    {
                        string type = "";
                        foreach (var item_cc in item["types"])
                        {
                            type += (string)item_cc + ",";
                        }
                        type = type.Substring(0, type.Length - 1);
                        if (type == "sublocality,political" || type == "locality,political" || type == "neighborhood,political" || type == "administrative_area_level_3,political")
                        {
                            addr.city = (city == "" || type == "locality,political") ? (string)item["long_name"] : city;
                        }
                        if (type == "administrative_area_level_1,political")
                        {
                            addr.state = (string)item["short_name"];
                        }
                        if (type == "postal_code" || type == "postal_code_prefix,postal_code")
                        {
                            addr.zipcode = (string)item["long_name"];
                        }
                        //if (type == "country,political") {
                        //    addr.country = address_component.long_name;
                        //}
                        if (type == "locality,political,ward")
                        {
                            addr.city1 = (city1 == "" || type == "locality,political,ward") ? (string)item["long_name"] : city1;
                        }
                        if (type == "political,sublocality,sublocality_level_1")
                        {
                            addr.city2 = (city2 == "" || type == "political,sublocality,sublocality_level_1") ? (string)item["long_name"] : city2;
                        }
                        if (type == "political,sublocality,sublocality_level_2")
                        {
                            addr.city3 = (city3 == "" || type == "political,sublocality,sublocality_level_2") ? (string)item["long_name"] : city3;
                        }
                    }
                }
                string local_addr = "", local_addr1 = "";
                local_addr = "";
                if (addr.state != null)
                {
                    local_addr += addr.state;
                }
                if (addr.city != null)
                {
                    local_addr += addr.city;
                }
                local_addr1 = "";
                if (addr.city1 != null)
                {
                    local_addr1 += addr.city1;
                }
                if (addr.city2 != null)
                {
                    local_addr1 += addr.city2;
                }
                if (addr.city3 != null)
                {
                    local_addr1 += addr.city3;
                }
                if (addr.zipcode != null)
                {
                    zipcode = addr.zipcode;
                }
                string zipcode_mm = (zipcode == "") ? "221-0851" : zipcode;

                Query = "update nursing_room set postal_code='" + zipcode_mm + "' where id='" + id + "';";

                result_cmd = gc.update_cmd(Query);

            }
        }

        //button

        com_list = new List<company_group>();
        company_group comg = new company_group();


        Query = "select b.company_url,b.company_icon,b.company_name,b.company_des";
        Query += " from nursing_room_connect_company as a inner join nursing_room_company as b";
        Query += " on a.nursing_room_company_id=b.id where a.nursing_room_id='" + id + "';";

        DataView sqldr3 = gc.select_cmd(Query);

        int ind = 0;
        string zipcode_m = (zipcode == "") ? "221-0851" : zipcode;


        comg = new company_group();
        comg.name = "Trim";
        comg.index = ind;
        ind += 1;
        comg.icon = @"img/img/ver1/Trim_icon1.png";
        //comg.des = "全ての、こどもたちへ";
        comg.des = "全ての、こどもたちへ全ての、こどもたちへ全ての、こどもたちへ全ての、こどもたちへ全ての、こどもたちへ全ての、こどもたちへ";
        comg.url = @"http:///";
        com_list.Add(comg);
        comg = new company_group();
        comg.name = "";
        comg.index = ind;
        ind += 1;
        comg.icon =  @"img/img/ver1/_Icon.png";
        comg.des = "地域のみんなとつながる子育てSNS";
        comg.url = @"https://.jp/main_guest_light.aspx?=" + zipcode_m;
        com_list.Add(comg);
        if (sqldr3.Count>0)
        {

            for (int i = 0; i < sqldr3.Count;i++ )
            {
                comg = new company_group();
                comg.name = sqldr3.Table.Rows[i]["company_name"].ToString();
                comg.index = ind;
                ind += 1;
                comg.icon = sqldr3.Table.Rows[i]["company_icon"].ToString();
                comg.des = sqldr3.Table.Rows[i]["company_des"].ToString();
                comg.url = sqldr3.Table.Rows[i]["company_url"].ToString();
                com_list.Add(comg);

            }
        }

        string res_but = "";

        ////check circle count
        //bool way = false;
        //if (com_list.Count % 2 == 1)
        //{
        //    way = true;
        //}
        //int midd = 0, last_w = 0;
        //if (way)
        //{
        //    midd = (com_list.Count - 1) / 2;
        //    last_w = ((middle_center_panel.Width / 2) - (TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 4))
        //        - (midd * 2) * (TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2);
        //    if (last_w < 0) { last_w = 0; }
        //    for (int i = 0; i < com_list.Count; i++)
        //    {

        //        picc = new PictureBox();
        //        picc.Name = "circle_icon_" + i;
        //        //pic_logo.Image = new Bitmap(@"http://openweathermap.org/img/w/" + icon + ".png");
        //        //pic.ImageLocation = @"http://openweathermap.org/img/w/" + icon + ".png";
        //        picc.Image = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\circle1.png");
        //        //if (ind == 0)
        //        //{
        //        //    picc.Image = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\circle.png");
        //        //}
        //        picc.SizeMode = PictureBoxSizeMode.StretchImage;
        //        picc.Width = TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2;
        //        picc.Height = TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2;
        //        picc.BackColor = Color.Transparent;
        //        picc.Location = new Point(last_w, middle_center_panel.Height - (middle_center_panel.Height / 10));
        //        last_w += (TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2) * 2;
        //        if (last_w > middle_center_panel.Width) { last_w = middle_center_panel.Width; }
        //        middle_center_panel.Controls.Add(picc);
        //    }
        //}
        //else
        //{
        //    midd = com_list.Count / 2;
        //    last_w = (middle_center_panel.Width / 2)
        //        - (midd * 2) * (TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2);
        //    if (last_w < 0) { last_w = 0; }
        //    for (int i = 0; i < com_list.Count; i++)
        //    {

        //        picc = new PictureBox();
        //        picc.Name = "circle_icon_" + i;
        //        //pic_logo.Image = new Bitmap(@"http://openweathermap.org/img/w/" + icon + ".png");
        //        //pic.ImageLocation = @"http://openweathermap.org/img/w/" + icon + ".png";
        //        picc.Image = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\circle1.png");
        //        //if (ind == 0)
        //        //{
        //        //    picc.Image = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\circle.png");
        //        //}
        //        picc.SizeMode = PictureBoxSizeMode.StretchImage;
        //        picc.Width = TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2;
        //        picc.Height = TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2;
        //        picc.BackColor = Color.Transparent;
        //        picc.Location = new Point(last_w, middle_center_panel.Height - (middle_center_panel.Height / 10));
        //        last_w += (TextRenderer.MeasureText(usetime_label.Text, usetime_label.Font).Height / 2) * 2;
        //        if (last_w > middle_center_panel.Width) { last_w = middle_center_panel.Width; }
        //        middle_center_panel.Controls.Add(picc);
        //    }
        //}
        //picc = middle_center_panel.Controls.Find("circle_icon_0", true).FirstOrDefault() as PictureBox;
        //picc.Image = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\circle.png");


        //<a href="javascript:void(0);" onclick="LinkClick(0);">Windows XP</a>
        //button1.Text = "";
        //button1.TabStop = false;
        //button1.BackColor = Color.Transparent;
        ////button1.BackColor = Color.FromArgb(Convert.ToInt32(255 * 0.37), 22, 53, 48);
        //button1.FlatStyle = FlatStyle.Flat;
        //button1.FlatAppearance.BorderSize = 0;
        //button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
        //button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
        //button1.Height = Convert.ToInt32(middle_center_panel.Height / 12);
        //button1.Width = Convert.ToInt32(middle_center_panel.Height / 10) / 2;
        //button1.Location = new Point(30, ((middle_center_panel.Height / 2) - (button1.Height / 2)));
        //button1.Cursor = Cursors.Hand;
        //button1.BackgroundImage = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\left2.png");
        //button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;


        //button2.Text = "";
        //button2.TabStop = false;
        //button2.BackColor = Color.Transparent;
        ////button2.BackColor = Color.FromArgb(Convert.ToInt32(255 * 0.37), 22, 53, 48);
        //button2.FlatStyle = FlatStyle.Flat;
        //button2.FlatAppearance.BorderSize = 0;
        //button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
        //button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
        //button2.Height = Convert.ToInt32(middle_center_panel.Height / 12);
        //button2.Width = Convert.ToInt32(middle_center_panel.Height / 10) / 2;
        //button2.Location = new Point(middle_center_panel.Width - 30 - button1.Width, ((middle_center_panel.Height / 2) - (button2.Height / 2)));
        //button2.Cursor = Cursors.Hand;
        //button2.BackgroundImage = new Bitmap(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\img\ver1\right2.png");
        //button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        res_but = "<div>&nbsp;</div>";
        res_but += "<table width='100%' height='100%' style='margin:50px 0px 30px 0px;'><tr><td rowspan='2' align='center' width='10%'>";
        //index_cheange = 0;
        res_but+="<a href='javascript:void(0);' onclick='LinkClick();'>";
        res_but += "<img src='"+@"img\img\ver1\left2.png' width='20px' height='40px'>";
        res_but+="</a>";
        res_but+="</td>";
        if (com_list.Count > 0)
        {
            res_but += "<td rowspan='2' width='30%' align='center'>";
            res_but += "<a id='icon_a' class='middle_icon_a' href='" + com_list[0].url + "'>";
            res_but += "<img id='icon_img' class='middle_icon' src='" + com_list[0].icon + "' width='100%' height='100%'>";
            res_but+="</a>";
            res_but += "</td>";
            res_but += "<td class='middle_text'>";
             string word_title = com_list[0].name;
             res_but += "<a id='title_a' href='" + com_list[0].url + "'>";
             res_but += "<span id='title_txt' class='middle_title'>" + word_title + "</span>";
             res_but += "</a>";

            res_but += "</td>";

            res_but += "<td rowspan='2' align='center' width='10%'>";
            res_but += "<a href='javascript:void(0);' onclick='LinkClick1();'>";
            res_but += "<img src='" + @"img\img\ver1\right2.png' width='20px' height='40px'>";
            res_but += "</a>";
            res_but += "</td>";
            res_but += "</tr>";
            res_but += "<tr>";
            res_but += "<td width='40%' class='middle_text1'>";
            string word_des = com_list[0].des;
            res_but += "<a id='des_a' href='" + com_list[0].url + "'>";
            res_but += "<span id='des_txt' class='middle_des'>" + word_des + "</span>";
            res_but += "</a>";

            res_but += "</td>";
        }

        res_but += "</tr><tr><td align='center' colspan='4' height='130px'>";

        //check circle count
        res_but += @"<img id='circle_icon_0' src='img\img\ver1\circle.png' width='10px' height='10px'>";
            for (int i = 1; i < com_list.Count; i++)
            {
                res_but += "&nbsp;<img id='circle_icon_" + i + @"' src='img\img\ver1\circle1.png' width='10px' height='10px'>";
            }

        res_but += "</td></tr></table>";
        res_but += "<div>&nbsp;</div>";
        res_but += @"<script type='text/javascript'>";
        res_but += @"function LinkClick() {
a = window.sessionStorage.getItem(['key2']);
b=0;
if(a=='null')
{
a=" + (com_list.Count - 1) + @";
window.sessionStorage.setItem(['key2'],[a]);
}else
{
b=Number(a);
b=b-1;
if(b==-1)
{
b=" + (com_list.Count-1) + @";
}
window.sessionStorage.setItem(['key2'],[b]);
}
      var elem = document.getElementById('icon_img');
        var elem_a = document.getElementById('icon_a');
var elem_a1 = document.getElementById('title_a');
var elem_a3 = document.getElementById('des_a');
console.log(b);
 switch (b) {
";
        if (com_list.Count > 0)
        {
            for (int i = 0; i < com_list.Count; i++)
            {
                int bi = i + 1;
                if (bi == com_list.Count)
                {
                    bi = 0;
                }
                res_but += @"case "+i+ @":
        var elem_cir = document.getElementById('circle_icon_"+i+ @"');
        elem_cir.src = 'img/img/ver1/circle.png';
        var elem_cir1 = document.getElementById('circle_icon_" + bi + @"');
        elem_cir1.src = 'img/img/ver1/circle1.png';
            elem_a.href = '" + com_list[i].url + @"';
          elem.src = '" + com_list[i].icon + @"';
            elem_a1.href = '" + com_list[i].url + @"';
          document.getElementById('title_txt').innerHTML = '" + com_list[i].name + @"';
            elem_a3.href = '" + com_list[i].url + @"';
          document.getElementById('des_txt').innerHTML = '" + com_list[i].des + @"';
          break;
";
            }
        }
        res_but += @"
      }
    }";
        res_but += @"function LinkClick1() {
a = window.sessionStorage.getItem(['key2']);
b=0;
if(a==null)
{
a=1;
window.sessionStorage.setItem(['key2'],['1']);
}else
{
b=Number(a);
b=b+1;
if(b==" + com_list.Count + @")
{
b=0;
}
window.sessionStorage.setItem(['key2'],[b]);
}
      var elem = document.getElementById('icon_img');
        var elem_a = document.getElementById('icon_a');
var elem_a1 = document.getElementById('title_a');
var elem_a3 = document.getElementById('des_a');
console.log(b);
 switch (b) {
";
        if (com_list.Count > 0)
        {
            for (int i = 0; i < com_list.Count; i++)
            {
                int bi = i - 1;
                if (bi == -1)
                {
                    bi = com_list.Count - 1;
                }
                res_but += @"case " + i + @":
        var elem_cir = document.getElementById('circle_icon_" + i + @"');
        elem_cir.src = 'img/img/ver1/circle.png';
        var elem_cir1 = document.getElementById('circle_icon_" + bi + @"');
        elem_cir1.src = 'img/img/ver1/circle1.png';
            elem_a.href = '" + com_list[i].url + @"';
          elem.src = '" + com_list[i].icon + @"';
            elem_a1.href = '" + com_list[i].url + @"';
          document.getElementById('title_txt').innerHTML = '" + com_list[i].name + @"';
            elem_a3.href = '" + com_list[i].url + @"';
          document.getElementById('des_txt').innerHTML = '" + com_list[i].des + @"';
          break;
";
            }
        }
        res_but += @"
      }
    }";
        res_but += "</script>";
        Panel2.Controls.Add(new LiteralControl(res_but));
    }
    public string zipcode = "";
    public class addr_type
    {
        public string state = "";
        public string city = "";
        public string city1 = "";
        public string city2 = "";
        public string city3 = "";
        public string zipcode = "";
    }
    List<company_group> com_list = new List<company_group>();
    public class company_group
    {
        public int index = 0;
        public string name = "";
        public string des = "";
        public string icon="";
        public string url = "";
    }
    class MyButton : Button
    {
        private string m_URL;

        public string URL
        {
            get { return m_URL; }
            set { m_URL = value; }
        }
    }
    class MyPanel : Panel
    {
        private string m_URL;

        public string URL
        {
            get { return m_URL; }
            set { m_URL = value; }
        }
    }
    class MyLabel : Label
    {
        private string m_URL;

        public string URL
        {
            get { return m_URL; }
            set { m_URL = value; }
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //time
        TimeZoneInfo TPZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        DateTime indate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TPZone);
        string week = "";
        if (indate.DayOfWeek == DayOfWeek.Monday) { week = "MON"; }
        else if (indate.DayOfWeek == DayOfWeek.Tuesday) { week = "TUE"; }
        else if (indate.DayOfWeek == DayOfWeek.Wednesday) { week = "WED"; }
        else if (indate.DayOfWeek == DayOfWeek.Thursday) { week = "THU"; }
        else if (indate.DayOfWeek == DayOfWeek.Friday) { week = "FRI"; }
        else if (indate.DayOfWeek == DayOfWeek.Saturday) { week = "SAT"; }
        else if (indate.DayOfWeek == DayOfWeek.Sunday) { week = "SUN"; }

        Label1.Text = indate.Month.ToString() + "/" + indate.Day.ToString() + "(" + week + ")";
        Label2.Text = indate.ToString("HH:mm");

        UpdatePanel1.Update();

        //count += 1;
        ////if (DateTime.Now.Minute % 10 == 0)
        ////{
        ////    Response.Redirect("twitter_gov.aspx");
        ////}
        //Session["seak"] = "false";
        //if (DateTime.Now.Minute % 59 == 0)
        //{
        //    if (DateTime.Now.Second > 55)
        //    {
        //        Session["seak"] = "true";
        //        Label4.Text = DateTime.Now.ToString();
        //        Response.Redirect("twitter_gov_1.aspx");
        //    }
        //}
    }
    public string LoadWeather()
    {
        string result_res = "";
        if (Session["mid"] != null)
        {
            if (Session["mid"].ToString().Trim() != "")
            {
                string id = Session["mid"].ToString();

                string GPS_lat = "", GPS_lng = "", result = "";

                Query = "select logo,address,GPS_lat,GPS_lng from nursing_room where id='" + id + "';";

                DataView ict2 = gc.select_cmd(Query);
                if (ict2.Count > 0)
                {
                    GPS_lat = ict2.Table.Rows[0]["GPS_lat"].ToString();
                    GPS_lng = ict2.Table.Rows[0]["GPS_lng"].ToString();

                }


                bool check_gps = false;
                if (GPS_lat.Trim() == "")
                {
                    check_gps = true;
                }
                if (GPS_lng.Trim() == "")
                {
                    check_gps = true;
                }
                if (check_gps)
                {
                    GPS_lat = "35.447821";
                    GPS_lng = "139.641685";
                }

                //no GPS no weather and area
                if (GPS_lat.Trim() != "" && GPS_lng.Trim() != "")
                {

                    double te = 0; int tempc = 0; string icon = "";
                    //get five day weather
                    //check database info
                    DateTime last_date;
                    TimeZoneInfo TPZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                    DateTime todate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TPZone);

                    Query = "select * from nursing_room_weather_info where nursing_room_id='" + id + "' order by dtime desc;";

                    DataView ict_date = gc.select_cmd(Query);
                    if (ict_date.Count > 0)
                    {
                        last_date = Convert.ToDateTime(ict_date.Table.Rows[0]["dtime"].ToString());
                    }
                    else
                    {
                        last_date = new DateTime(2000, 5, 7);
                    }
                    List<string> fftemp = new List<string>();
                    List<DateTime> fftime = new List<DateTime>();
                    List<string> ffweat = new List<string>();
                    double compute_day = (last_date - todate).TotalDays;
                    if (compute_day < 1)
                    {
                        //update weather information from website

                        //get weather info from website
                        var url_five = new Uri("http://api.openweathermap.org/data/2.5/forecast?lat=" + GPS_lat + "&lon=" + GPS_lng + "&units=imperial&appid=9c293c963735c31fbc771ef88e9c3455");

                        string result_five = "";
                        System.Net.HttpWebRequest request_five = (HttpWebRequest)HttpWebRequest.Create(url_five);
                        using (var response = request_five.GetResponse())
                        using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            result_five = sr.ReadToEnd();
                        }

                        Newtonsoft.Json.Linq.JObject jArray_five = JObject.Parse(result_five);

                        //dt_txt
                        bool check_same = false;
                        int tempcc = 0;
                        foreach (Newtonsoft.Json.Linq.JObject jArray_five1 in jArray_five["list"])
                        {
                            te = Convert.ToDouble((string)jArray_five1["main"]["temp"]) - 32;
                            tempcc = 0;
                            if (te != 0)
                            {
                                tempcc = Convert.ToInt32(System.Math.Round((te * 5 / 9), 0, MidpointRounding.AwayFromZero));
                            }
                            fftemp.Add(tempcc.ToString());
                            fftime.Add((DateTime)jArray_five1["dt_txt"]);
                            ffweat.Add((string)jArray_five1["weather"][0]["icon"]);

                            if (check_same == false)
                            {
                                Query = "select * from nursing_room_weather_info where nursing_room_id='" + id + "' and dtime='" + jArray_five1["dt_txt"].ToString() + "';";

                                DataView ict_check_same = gc.select_cmd(Query);
                                if (ict_check_same.Count == 0)
                                {
                                    check_same = true;
                                }
                            }
                            else
                            {

                                Query = "insert into nursing_room_weather_info (nursing_room_id,tempture,weather_icon,dtime,update_time)";
                                Query += " VALUES ('" + id + "','" + tempcc.ToString() + "','" + jArray_five1["weather"][0]["icon"].ToString() + "','" + jArray_five1["dt_txt"].ToString() + "',NOW())";
                                result_cmd = gc.insert_cmd(Query);

                            }



                        }

                    }
                    else
                    {
                        //get weather information from database
                        Query = "select * from nursing_room_weather_info where nursing_room_id='" + id + "' and dtime>'" + todate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by dtime asc;";

                        DataView ict_info = gc.select_cmd(Query);
                        if (ict_info.Count > 0)
                        {
                            for (int i = 0; i < ict_info.Count; i++)
                            {
                                fftime.Add(Convert.ToDateTime(ict_info.Table.Rows[i]["dtime"].ToString()));
                                fftemp.Add(ict_info.Table.Rows[i]["tempture"].ToString());
                                ffweat.Add(ict_info.Table.Rows[i]["weather_icon"].ToString());
                            }
                        }


                    }

                    //check log



                        DateTime indate_com = DateTime.Now;
                        int index = 0;
                        //for (int i = 0; i < fftime.Count; i++)
                        //{
                        //    int comm = DateTime.Compare(indate_com, fftime[i]);
                        //    if (comm <0)
                        //    {
                        //        index = i;
                        //        break;
                        //    }
                        //}

                        tempc = Convert.ToInt32(fftemp[0]);
                        icon = ffweat[0];

                        //if (index - 1 > -1)
                        //{
                        //    tempc = Convert.ToInt32(fftemp[index - 1]);
                        //    icon = ffweat[index - 1];
                        //}
                        //else
                        //{
                        //    tempc = Convert.ToInt32(fftemp[index]);
                        //    icon = ffweat[index];
                        //}


                        //string tempf = (string)jArray["main"]["temp"], humidity = (string)jArray["main"]["humidity"], pressure = (string)jArray["main"]["pressure"];
                        //string tempf_min = (string)jArray["main"]["temp_min"], tempf_max = (string)jArray["main"]["temp_max"];
                        //string main_w = (string)jArray["weather"][0]["main"], des = (string)jArray["weather"][0]["description"];
                        //string icon = (string)jArray["weather"][0]["icon"], speed = (string)jArray["wind"]["speed"];

                        Label la;
                        result_res = "";
                        //get 15 hours after
                        int last_h_old = 10;
                        result_res += "<table><tr>";
                        for (int i = 0; i < 5; i++)
                        {
                            result_res += "<td align='center' width='100px'>";
                            int last_h = last_h_old;
                            //PictureBox pic = new PictureBox();
                            //pic.Name = "weather_icon_label0";
                            //pic_logo.Image = new Bitmap(@"http://openweathermap.org/img/w/" + icon + ".png");
                            //pic.ImageLocation = @"http://openweathermap.org/img/w/" + icon + ".png";

                            if (i > 0)
                            {
                                result_res += "<img src='" + @"img\img\ver1\weather\" + ffweat[i] + ".png' width='50' height='50'>";

                            }
                            else
                            {
                                result_res += "<img src='" + @"img\img\ver1\weather\" + icon + ".png' width='50' height='50'>";
                            }
                            //pic.SizeMode = PictureBoxSizeMode.StretchImage;
                            //pic.Width = 60;
                            //pic.Height = 60;
                            //pic.BackColor = Color.Transparent;
                            //pic.Location = new Point(((weather_panel.Width / 5) * i), last_h);
                            //last_h = last_h + pic.Height + 5;
                            //Invoke(new Action(() =>
                            //{
                            //    weather_panel.Controls.Add(pic);
                            //}));
                            //int oldw = pic.Width + (pic.Width / 10);

                            //18
                            string temp_la = "NOW";
                            //la = new Label();
                            //la.Name = "temp_m_label" + i.ToString();
                            //la.ForeColor = ColorTranslator.FromHtml("#323837");
                            //la.Font = GetCustomFont(Properties.Resources.Khula_Regular, 12.0f, FontStyle.Regular);
                            //la.Text = "NOW";
                            //la.BackColor = Color.Transparent;
                            if (i > 0)
                            {
                                if (fftime[i].Hour >= 12)
                                {
                                    temp_la = fftime[i].Hour + "PM";
                                }
                                else
                                {
                                    temp_la = fftime[i].Hour + "AM";
                                }
                            }

                            result_res += "<br/><span class='temp_la1'>" + temp_la + "</span>";

                            //la.Width = TextRenderer.MeasureText(la.Text, la.Font).Width;
                            //la.Height = TextRenderer.MeasureText(la.Text, la.Font).Height;
                            //la.Location = new Point(((weather_panel.Width / 5) * i) + (pic.Width / 2) - (la.Width / 2), last_h);
                            //last_h = last_h + TextRenderer.MeasureText(la.Text, la.Font).Height + 5;
                            //Invoke(new Action(() =>
                            //{
                            //    weather_panel.Controls.Add(la);
                            //}));

                            temp_la = tempc.ToString() + "°";
                            if (i > 0)
                            {
                                temp_la = fftemp[i] + "°";
                            }
                            result_res += "<br/><span class='temp_la'>" + temp_la + "</span>";
                            ////18
                            //la = new Label();
                            //la.Name = "temp_m_label_0";
                            //la.ForeColor = ColorTranslator.FromHtml("#323837");
                            //la.Font = GetCustomFont(Properties.Resources.Lato_Regular, 16.0f, FontStyle.Regular);
                            //la.Text = tempc.ToString() + "°";
                            //la.BackColor = Color.Transparent;
                            //if (i > 0)
                            //{
                            //    la.Text = fftemp[index] + "°";
                            //}
                            //la.Width = TextRenderer.MeasureText(la.Text, la.Font).Width;
                            //la.Height = TextRenderer.MeasureText(la.Text, la.Font).Height;
                            //la.Location = new Point(((weather_panel.Width / 5) * i) + (pic.Width / 2) - (la.Width / 2), last_h);
                            //Invoke(new Action(() =>
                            //{
                            //    weather_panel.Controls.Add(la);
                            //}));
                            if (i > 0)
                            {
                                index += 1;
                            }

                            result_res += "</td>";
                            if (i == 0)
                            {
                                result_res += "<td align='center' width='20px'><img src='" + @"img\img\ver1\weather_line.png' width='2' height='100'></td>";
                            }
                        }
                        result_res += "</tr></table>";

                }
            }



        }

        return result_res;
    }
    [WebMethod]
    public static string save_list(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9)
    {
        string res = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        if (HttpContext.Current.Session["mid"] != null)
        {
            string id = HttpContext.Current.Session["mid"].ToString();
            string lan = "";
            if (param6 == null)
            {
                lan = "NULL";
            }
            else
            {
                if (param6.Trim() == "")
                {
                    lan = "NULL";
                }
                else
                {
                    lan = "'" + param6.Trim() + "'";
                }
            }
            string byear = "";
            if (param7 == null)
            {
                byear = "0";
            }
            else
            {
                if (param7.Trim() == "")
                {
                    byear = "0";
                }
                else
                {
                    byear = "'" + param7.Trim() + "'";
                }
            }
            string bmonth = "";
            if (param8 == null)
            {
                bmonth = "0";
            }
            else
            {
                if (param8.Trim() == "")
                {
                    bmonth = "0";
                }
                else
                {
                    bmonth = "'" + param8.Trim() + "'";
                }
            }
            string parent = "";
            if (param9 == null)
            {
                parent = "NULL";
            }
            else
            {
                if (param9.Trim() == "")
                {
                    parent = "NULL";
                }
                else
                {
                    parent = "'" + param9.Trim() + "'";
                }
            }
            string QA1 = "";
            if (param2 == null)
            {
                QA1 = "NULL";
            }
            else
            {
                if (param2.Trim() == "")
                {
                    QA1 = "NULL";
                }
                else
                {
                    QA1 = "'"+param2.Trim()+"'";
                }
            }
            string QA2 = "";
            if (param3 == null)
            {
                QA2 = "NULL";
            }
            else
            {
                if (param3.Trim() == "")
                {
                    QA2 = "NULL";
                }
                else
                {
                    QA2 = "'"+param3.Trim()+"'";
                }
            }
            string QA3 = "";
            if (param4 == null)
            {
                QA3 = "NULL";
            }
            else
            {
                if (param4.Trim() == "")
                {
                    QA3 = "NULL";
                }
                else
                {
                    QA3 = "'"+param4.Trim()+"'";
                }
            }
            string QA4 = "";
            if (param5 == null)
            {
                QA4 = "NULL";
            }
            else
            {
                if (param5.Trim() == "")
                {
                    QA4 = "NULL";
                }
                else
                {
                    QA4 = "'"+param5.Trim()+"'";
                }
            }


            if (id.Trim() != "")
            {
                Query = "insert into nursing_room_QA(nursing_room_id,language,Q1_baby_year,Q1_baby_month,Q1_parent,Q2_choice,Q3_choice,Q4_choice,Q5_choice,insert_time)";
                Query += " values('" + id.Trim() + "'," + lan + "," + byear + "," + bmonth + "," + parent + "," + QA1 + "," + QA2 + "," + QA3 + "," + QA4 + ",NOW());";

                string rescom = gc.insert_cmd(Query);
                res = rescom;
            }
        }


        return res;
    }
}
