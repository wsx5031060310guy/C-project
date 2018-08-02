using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;

public partial class Date_Calendar_guest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] dayNames = { "日", "月", "火", "水", "木", "金", "土" };
        CultureInfo culture = new CultureInfo("ja-JP");
        culture.DateTimeFormat.AbbreviatedDayNames = dayNames;
        Thread.CurrentThread.CurrentCulture = culture;
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main_guest.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!IsPostBack)
	    {

	    }
    }
    List<user_information> video_list = new List<user_information>();
    //List<holiday> date_holiday = new List<holiday>();
    List<dayofweek> date_week = new List<dayofweek>();
    protected void Page_Init(object sender, EventArgs e)
    {
            if ( Session["sup_id"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sorry you stay too long!')", true);
                Response.Redirect("main_guest.aspx");
            }
            else
            {
                string sup_id = Session["sup_id"].ToString();


                ////get Japan Holidays from this year
                //var url = "http://japanese-holiday.herokuapp.com/holidays?year=" + DateTime.Now.Year.ToString();
                //string result = "";
                //System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //using (var response = request.GetResponse())
                //using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                //{
                //    result = sr.ReadToEnd();
                //}

                //date_holiday = new List<holiday>();
                //holiday hol = new holiday();
                //DateTime date = new DateTime();

                //JObject jArray = JObject.Parse(result);

                //foreach (var item in jArray["holidays"])
                //{
                //    hol = new holiday();
                //    hol.name = (string)item["name"];
                //    date = (DateTime)item["date"];
                //    hol.day=date.Day;
                //    hol.month = date.Month;
                //    hol.year = date.Year;

                //    date_holiday.Add(hol);
                //}
                //ViewState["myHoliday"] = date_holiday;
                ////get Japan Holidays from this year


                ////get user select day of week
                date_week = new List<dayofweek>();

                dayofweek dow=new dayofweek();

                SqlDataSource sql_f_w = new SqlDataSource();
                sql_f_w.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_w.SelectCommand = "select b.checked,b.week_of_day,b.start_hour,b.start_minute,b.end_hour,b.end_minute";
                sql_f_w.SelectCommand += " from user_information_store as a";
                sql_f_w.SelectCommand += " inner join user_information_store_week_appointment as b on a.id=b.uisid";
                sql_f_w.SelectCommand += " where a.uid='" + sup_id + "';";
                sql_f_w.DataBind();
                DataView ict_f_w = (DataView)sql_f_w.Select(DataSourceSelectArguments.Empty);
                for (int i = 0; i < ict_f_w.Count; i++)
                {
                    if(ict_f_w.Table.Rows[i]["checked"].ToString()=="1")
                    {
                        dow=new dayofweek();
                        dow.day =Convert.ToInt32( ict_f_w.Table.Rows[i]["week_of_day"].ToString());
                        dow.shour = ict_f_w.Table.Rows[i]["start_hour"].ToString();
                        dow.smin = ict_f_w.Table.Rows[i]["start_minute"].ToString();
                        dow.ehour = ict_f_w.Table.Rows[i]["end_hour"].ToString();
                        dow.emin = ict_f_w.Table.Rows[i]["end_minute"].ToString();
                        date_week.Add(dow);
                    }

                }
                ViewState["day_of_week"] = date_week;
                ////get user select day of week




                Panel pdn = (Panel)this.FindControl("left_view");
                SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select a.photo,a.username,a.CreatedDate,b.id,b.money,b.gov_choice,b.choice1_1,b.choice1_2,b.choice1_3,b.choice1_4,b.howmany,b.age_range_start_year";
                sql_f.SelectCommand += ",b.age_range_start_month,b.age_range_end_year,b.age_range_end_month,b.choice2_1,b.choice2_2,b.choice2_3";
                sql_f.SelectCommand += ",b.choice2_4,b.choice2_5,b.choice2_6,b.choice3_1,b.choice3_2,b.choice3_3,b.choice3_4,b.choice3_5";
                sql_f.SelectCommand += ",b.baby_rule,b.baby_notice,b.choice4_1,b.choice4_2,b.choice4_3,b.choice4_4,b.title,b.myself_content";
                sql_f.SelectCommand += " from user_login as a";
                sql_f.SelectCommand += " inner join user_information_store as b on b.uid=a.id";
                sql_f.SelectCommand += " where a.id='" + sup_id + "';";
                sql_f.DataBind();
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                string cutstr = "", cutstr1="";
                int ind = 0;
                if (ict_f.Count > 0)
                {
                    money_HiddenField.Value = ict_f.Table.Rows[0]["money"].ToString();
                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));

                    //user image
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td width='100%'>"));

                    pdn.Controls.Add(new LiteralControl("<div id='slider' class='flexslider'><ul class='slides'>"));

                    SqlDataSource sql_f1 = new SqlDataSource();
                    sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f1.SelectCommand = "select filename";
                    sql_f1.SelectCommand += " from user_information_store_images";
                    sql_f1.SelectCommand += " where uisid='" + ict_f.Table.Rows[0]["id"].ToString() +"';";
                    sql_f1.DataBind();
                    DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                    for (int i = 0; i < ict_f1.Count; i++)
                    {
                        cutstr = ict_f1.Table.Rows[i]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        pdn.Controls.Add(new LiteralControl("<li><img src='" + cutstr1 + "' /></li>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</ul></div><div id='carousel' class='flexslider'><ul class='slides'>"));
                    for (int i = 0; i < ict_f1.Count; i++)
                    {
                        cutstr = ict_f1.Table.Rows[i]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        pdn.Controls.Add(new LiteralControl("<li><img src='" + cutstr1 + "' /></li>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</ul></div>"));

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    //user information
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td class='main_content'>"));
                    pdn.Controls.Add(new LiteralControl("<hr/>"));

                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    //user photo
                    pdn.Controls.Add(new LiteralControl("<td width='10%'>"));

                    pdn.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                    cutstr = ict_f.Table.Rows[0]["photo"].ToString();
                    ind = cutstr.IndexOf(@"/");
                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    pdn.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict_f.Table.Rows[0]["username"].ToString() + "' style='width:100px;height:100px;'>"));
                    pdn.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='100' height='100' />"));
                    pdn.Controls.Add(new LiteralControl("</a>"));
                    pdn.Controls.Add(new LiteralControl("</div>"));

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    //user name and title
                    pdn.Controls.Add(new LiteralControl("<td width='75%'>"));

                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    HyperLink hy = new HyperLink();
                    hy.NavigateUrl = "javascript:void(0);";
                    hy.Target = "_blank";
                    hy.Text = ict_f.Table.Rows[0]["username"].ToString();
                    hy.Font.Underline = false;
                    pdn.Controls.Add(hy);

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));
                    Label la = new Label();
                    la.Text = ict_f.Table.Rows[0]["title"].ToString();
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    la.Font.Size = 20;
                    pdn.Controls.Add(la);

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    //user what time register
                    pdn.Controls.Add(new LiteralControl("<td width='15%'>"));

                    string d = ict_f.Table.Rows[0]["CreatedDate"].ToString();
                    DateTime dt = Convert.ToDateTime(d);

                    if ((DateTime.Now - dt).TotalDays < 14)
                    {
                        Image img = new Image();
                        img.ImageUrl = "~/images/home_images/new.png";
                        //img.Attributes.Add("onload", "if (this.width<50) this.width=50;");
                        pdn.Controls.Add(img);
                    }

                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));

                    string m = dt.Month.ToString();
                    if (dt.Month < 10)
                    {
                        m = "0" + dt.Month.ToString();
                    }
                    string dd = dt.Day.ToString();
                    if (dt.Day < 10)
                    {
                        dd = "0" + dt.Day.ToString();
                    }
                    string date_re = dt.Year+"."+m+"."+dd;
                    la = new Label();
                    la.Text = date_re;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));
                    pdn.Controls.Add(new LiteralControl("<hr/>"));

                    //check user's ID CARD
                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td width='5%'>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    Image img1 = new Image();
                    if (ict_f.Table.Rows[0]["choice2_1"].ToString() == "1")
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_Child-rearing experience.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/non-active_Child-rearing experience.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    //自治體
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    if (ict_f.Table.Rows[0]["gov_choice"].ToString() == "1")
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_Municipality.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/non-active_Municipality.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    if (ict_f.Table.Rows[0]["choice2_2"].ToString() == "1")
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_Infants.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/non-active_Infants.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    bool check_m = false;

                    if (ict_f.Table.Rows[0]["choice2_3"].ToString() == "1")
                    {
                        check_m = true;
                    }
                    else if (ict_f.Table.Rows[0]["choice2_5"].ToString() == "1")
                    {
                        check_m = true;
                    }

                    if (check_m)
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_TeacherQualification.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/non-active_TeacherQualification.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    if (ict_f.Table.Rows[0]["choice2_6"].ToString() == "1")
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_DoctorQualification.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/non-active_DoctorQualification.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%'>"));
                    if (ict_f.Table.Rows[0]["choice2_4"].ToString() == "1")
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/active_handicapped.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    else
                    {
                        img1 = new Image();
                        img1.ImageUrl = "~/images/date_calendar/handicapped.png";
                        pdn.Controls.Add(img1);
                        pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td width='5%'>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));

                    //check user's ID CARD
                    //check user's ID CARD
                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td width='5%'>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    if (ict_f.Table.Rows[0]["choice2_1"].ToString() == "1")
                    {
                        la = new Label();
                        la.Text = "子育て経験";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "子育て経験";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    //自治體
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    if (ict_f.Table.Rows[0]["gov_choice"].ToString() == "1")
                    {
                        la = new Label();
                        la.Text = "自治体認定";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "自治体認定";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    if (ict_f.Table.Rows[0]["choice2_2"].ToString() == "1")
                    {
                        la = new Label();
                        la.Text = "保育士資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "保育士資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    check_m = false;

                    if (ict_f.Table.Rows[0]["choice2_3"].ToString() == "1")
                    {
                        check_m = true;
                    }
                    else if (ict_f.Table.Rows[0]["choice2_5"].ToString() == "1")
                    {
                        check_m = true;
                    }

                    if (check_m)
                    {
                        la = new Label();
                        la.Text = "幼稚園/小学校教員資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "幼稚園/小学校教員資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    if (ict_f.Table.Rows[0]["choice2_6"].ToString() == "1")
                    {
                        la = new Label();
                        la.Text = "医師看護士資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "医師看護士資格";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center' width='15%' valign='top'>"));
                    if (ict_f.Table.Rows[0]["choice2_4"].ToString() == "1")
                    {
                        la = new Label();
                        la.Text = "障害児預かり経験";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CF8B58");
                        pdn.Controls.Add(la);
                    }
                    else
                    {
                        la = new Label();
                        la.Text = "障害児預かり経験";
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                        pdn.Controls.Add(la);
                    }
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("<td width='5%'>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));
                    pdn.Controls.Add(new LiteralControl("<hr/>"));
                    //check user's ID CARD


                    //user take care of range and how to get there
                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    string str = "";
                    if (ict_f.Table.Rows[0]["choice1_1"].ToString() == "1")
                    {
                        str += "送迎、";
                    }
                    if (ict_f.Table.Rows[0]["choice1_2"].ToString() == "1")
                    {
                        str += "利用宅で預かる、";
                    }
                    if (ict_f.Table.Rows[0]["choice1_3"].ToString() == "1")
                    {
                        str += "自宅で預かる、";
                    }
                    if (ict_f.Table.Rows[0]["choice1_4"].ToString() == "1")
                    {
                        str += "乳児預かり、";
                    }
                    if (str.Length > 0)
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    la = new Label();
                    la.Text = "サポート内容 : " + str;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    str = "";

                    str += ict_f.Table.Rows[0]["age_range_start_year"].ToString() + "歳 " + ict_f.Table.Rows[0]["age_range_start_month"].ToString() + "ヶ月";
                    str += " ～ " + ict_f.Table.Rows[0]["age_range_end_year"].ToString() + "歳 " + ict_f.Table.Rows[0]["age_range_end_month"].ToString() + "ヶ月";
                    la = new Label();
                    la.Text = "サポート年齢 : " + str;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);
                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));
                    pdn.Controls.Add(new LiteralControl("<hr/>"));
                    //user take care of range and how to get there


                    //user content
                    pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center'>"));
                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));

                    la = new Label();
                    la.Text = "自己紹介";
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF9797");
                    la.Font.Size = 20;
                    pdn.Controls.Add(la);
                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    la = new Label();
                    la.Text = ict_f.Table.Rows[0]["myself_content"].ToString();
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);

                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "ルール";
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF9797");
                    la.Font.Size = 20;
                    pdn.Controls.Add(la);

                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    la = new Label();
                    la.Text = ict_f.Table.Rows[0]["baby_rule"].ToString();
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);


                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    //
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center'>"));


                    la = new Label();
                    la.Text = "注意事項";
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF9797");
                    la.Font.Size = 20;
                    pdn.Controls.Add(la);
                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));

                    la = new Label();
                    la.Text = ict_f.Table.Rows[0]["baby_notice"].ToString();
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#848484");
                    pdn.Controls.Add(la);


                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    //

                    //
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td align='center'>"));


                    la = new Label();
                    la.Text = "レビュー";
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF9797");
                    la.Font.Size = 20;
                    pdn.Controls.Add(la);
                    pdn.Controls.Add(new LiteralControl("<br/><br/>"));
                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));


                    Panel npan = new Panel();
                    npan.ID = "supporter_good_panel";
                    pdn.Controls.Add(npan);

                    ////user say something good for this supporter

                    Panel pan_vid1 = (Panel)this.FindControl("supporter_good_panel");

                    pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
                    pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                    pan_vid1.Controls.Add(new LiteralControl("<td>"));
                    pan_vid1.Controls.Add(new LiteralControl("<hr/>"));

                    video_list = new List<user_information>();
                    user_information vi = new user_information();

                    SqlDataSource sql_f_u = new SqlDataSource();
                    sql_f_u.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f_u.SelectCommand = "select c.id,a.uid,b.username,b.photo,c.score_message,c.check_time";
                    sql_f_u.SelectCommand += " from user_information_appointment_check_deal as a";
                    sql_f_u.SelectCommand += " inner join user_login as b on b.id=a.uid";
                    sql_f_u.SelectCommand += " inner join user_information_appointment_check_deal_score as c on c.uiacdid=a.id";
                    sql_f_u.SelectCommand += " where a.suppid='" + sup_id + "';";
                    sql_f_u.DataBind();
                    DataView ict_f_u = (DataView)sql_f_u.Select(DataSourceSelectArguments.Empty);
                    if (ict_f_u.Count > 0)
                    {
                        for (int i = 0; i < ict_f_u.Count; i++)
                        {
                            vi = new user_information();
                            vi.good_id = ict_f_u.Table.Rows[i]["id"].ToString();
                            vi.uid = ict_f_u.Table.Rows[i]["uid"].ToString();
                            vi.username = ict_f_u.Table.Rows[i]["username"].ToString();
                            vi.photo = ict_f_u.Table.Rows[i]["photo"].ToString();
                            vi.score_message = ict_f_u.Table.Rows[i]["score_message"].ToString();
                            vi.check_time =Convert.ToDateTime(ict_f_u.Table.Rows[i]["check_time"].ToString());
                            video_list.Add(vi);

                        }
                    }
                    int couuuu = ict_f_u.Count;
                    if (couuuu > 10)
                    {
                        couuuu = 10;
                    }
                    pan_vid1.Controls.Add(new LiteralControl("</td>"));
                    pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                    //each video
                    for (int i = 0; i < couuuu; i++)
                    {
                        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("<td>"));
                        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
                        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                        //Poster photo
                        pan_vid1.Controls.Add(new LiteralControl("<td width='10%' rowspan='3' valign='top'>"));
                        pan_vid1.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                        string cutstr2 = ict_f_u.Table.Rows[i]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        pan_vid1.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict_f_u.Table.Rows[i]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                        pan_vid1.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                        pan_vid1.Controls.Add(new LiteralControl("</a>"));
                        pan_vid1.Controls.Add(new LiteralControl("</div>"));
                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        //poster username
                        pan_vid1.Controls.Add(new LiteralControl("<td width='10%'>"));
                        hy = new HyperLink();
                        hy.NavigateUrl = "javascript:void(0);";

                        hy.Target = "_blank";
                        hy.Text = ict_f_u.Table.Rows[i]["username"].ToString();
                        hy.Font.Underline = false;
                        pan_vid1.Controls.Add(hy);
                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        //poster message type and time
                        pan_vid1.Controls.Add(new LiteralControl("<td align='right' width='80%'>"));
                        DateTime post_date = Convert.ToDateTime(ict_f_u.Table.Rows[i]["check_time"].ToString());
                        la = new Label();
                        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        la.Text = "";
                        la.Text += post_date.Year + "." + post_date.ToString("MM") + "." + post_date.ToString("dd");
                        pan_vid1.Controls.Add(la);
                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("<td colspan='2'>"));
                        Literal li = new Literal();
                        li.Text = @"<script>
$(function () {
$('.hidde" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').toggle(false);

            $('.box" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').click(function () {
                $('.hidde" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').toggle();
                $('.box" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').toggle(false);
            })

            $('.hidde" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').click(function () {
                $('.box" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').toggle();
                $('.hidde" + ict_f_u.Table.Rows[i]["id"].ToString() + @"').toggle(false);
            })
";
                        li.Text += @"
                        });";
                        li.Text += @"</script>";
                        pan_vid1.Controls.Add(li);

                        pan_vid1.Controls.Add(new LiteralControl("<div class='box" + ict_f_u.Table.Rows[i]["id"].ToString() + "'>"));
                        HyperLink hyy1;
                        if (ict_f_u.Table.Rows[i]["score_message"].ToString().Length < 37)
                        {
                            pan_vid1.Controls.Add(new LiteralControl(ict_f_u.Table.Rows[i]["score_message"].ToString()));
                        }
                        else
                        {
                            pan_vid1.Controls.Add(new LiteralControl(ict_f_u.Table.Rows[i]["score_message"].ToString().Substring(0, 37) + "‧‧‧"));
                            hyy1 = new HyperLink();
                            hyy1.NavigateUrl = "javascript:void(0);";
                            hyy1.Target = "_blank";
                            hyy1.Text = "詳細を見る";
                            hyy1.Font.Underline = false;
                            pan_vid1.Controls.Add(hyy1);
                        }


                        pan_vid1.Controls.Add(new LiteralControl("</div>"));
                        pan_vid1.Controls.Add(new LiteralControl("<div class='hidde" + ict_f_u.Table.Rows[i]["id"].ToString() + "'>"));

                        Label la1 = new Label();
                        la1.Style.Add("word-break", "break-all");
                        la1.Style.Add("over-flow", "hidden");
                        la1.Text = ict_f_u.Table.Rows[i]["score_message"].ToString();
                        pan_vid1.Controls.Add(la1);
                        pan_vid1.Controls.Add(new LiteralControl("<br/>"));


                        if (ict_f_u.Table.Rows[i]["score_message"].ToString().Length > 36)
                        {
                            hyy1 = new HyperLink();
                            hyy1.NavigateUrl = "javascript:void(0);";
                            hyy1.Target = "_blank";
                            hyy1.Text = "たたむ";
                            hyy1.Font.Underline = false;
                            pan_vid1.Controls.Add(hyy1);
                        }


                        pan_vid1.Controls.Add(new LiteralControl("</div>"));

                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("<td colspan='2' align='right'>"));
                        //who like who answer post message
                        SqlDataSource sql_who_like = new SqlDataSource();
                        sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_who_like.SelectCommand = "select count(*) as howmany from user_information_appointment_check_deal_score_like";
                        sql_who_like.SelectCommand += " where uiacdsid='" + ict_f_u.Table.Rows[i]["id"].ToString() + "' and good_status='1';";
                        //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                        sql_who_like.DataBind();
                        DataView ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                        if (ict_who_like.Count > 0)
                        {
                            img1 = new Image();
                            img1.Width = 15; img1.Height = 15;
                            img1.ImageUrl = "~/images/like_b_1.png";
                            pan_vid1.Controls.Add(img1);
                            hyy1 = new HyperLink();
                            hyy1.ID = "likecount" + ict_f_u.Table.Rows[i]["id"].ToString();
                            hyy1.NavigateUrl = "javascript:void(0);";
                            hyy1.Target = "_blank";
                            hyy1.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                            hyy1.Font.Underline = false;
                            pan_vid1.Controls.Add(hyy1);
                        }
                        pan_vid1.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                        //hyy1 = new HyperLink();
                        //hyy1.ID = "whouse_" + ict_f_u.Table.Rows[i]["id"].ToString() ;
                        //hyy1.NavigateUrl = "javascript:void(0);";
                        //hyy1.Target = "_blank";
                        //hyy1.Text = "役に立った!";
                        //hyy1.Font.Underline = false;

                        //sql_who_like = new SqlDataSource();
                        //sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        //sql_who_like.SelectCommand = "select good_status from user_information_appointment_check_deal_score_like";
                        //sql_who_like.SelectCommand += " where uiacdsid='" + ict_f_u.Table.Rows[i]["id"].ToString() + "' and uid='" + Session["id"].ToString() + "';";
                        //sql_who_like.DataBind();
                        //ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                        //if (ict_who_like.Count > 0)
                        //{
                        //    if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                        //    {
                        //        hyy1.Style.Add("color", "#4183C4");
                        //        hyy1.Attributes["onclick"] = "buseful_who_answer(this.id)";
                        //    }
                        //    else
                        //    {
                        //        hyy1.Style.Add("color", "#D84C4B");
                        //        hyy1.Attributes["onclick"] = "useful_who_answer(this.id)";
                        //    }
                        //}
                        //else
                        //{
                        //    hyy1.Style.Add("color", "#4183C4");
                        //    hyy1.Attributes["onclick"] = "buseful_who_answer(this.id)";
                        //}
                        //pan_vid1.Controls.Add(hyy1);


                        //pan_vid1.Controls.Add(new LiteralControl("<input id='likepost_" + ict_f_u.Table.Rows[i]["id"].ToString() + @"' type='button' value='LIKE' onclick='like(this.id)' class='file-upload'/>"));

                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                        pan_vid1.Controls.Add(new LiteralControl("</table>"));
                        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
                        pan_vid1.Controls.Add(new LiteralControl("</td>"));
                        pan_vid1.Controls.Add(new LiteralControl("</tr>"));

                    }
                    pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                    pan_vid1.Controls.Add(new LiteralControl("<td align='center'>"));

                    Panel pagepan = new Panel();
                    pagepan.ScrollBars = ScrollBars.Auto;
                    pagepan.Width = Unit.Percentage(100);
                    pagepan.Height = 100;
                    Button but_page = new Button();
                    int page_count = ict_f_u.Count / 10;
                    for (int i = 0; i < page_count + 1; i++)
                    {
                        but_page = new Button();
                        but_page.ID = "page_" + i;
                        but_page.Click += new System.EventHandler(this.change_page);
                        but_page.Text = "" + (i + 1);
                        but_page.BorderStyle = BorderStyle.None;
                        but_page.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                        but_page.OnClientClick = "ShowProgressBar();";
                        but_page.Style["cursor"] = "pointer";
                        pagepan.Controls.Add(but_page);
                        pagepan.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }
                    pan_vid1.Controls.Add(pagepan);

                    pan_vid1.Controls.Add(new LiteralControl("</td>"));
                    pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                    pan_vid1.Controls.Add(new LiteralControl("</table>"));


                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    //

                    Panel pel = (Panel)FindControl("left_view1");
                    pel.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //map
                    pel.Controls.Add(new LiteralControl("<tr>"));
                    pel.Controls.Add(new LiteralControl("<td align='center'>"));
                    la = new Label();
                    la.Text = "サポーター所在地";
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF9797");
                    la.Font.Size = 20;
                    pel.Controls.Add(la);
                    pel.Controls.Add(new LiteralControl("</td>"));
                    pel.Controls.Add(new LiteralControl("</tr>"));
                    pel.Controls.Add(new LiteralControl("<tr>"));
                    pel.Controls.Add(new LiteralControl("<td align='center'>"));

                    SqlDataSource sql_f2 = new SqlDataSource();
                    sql_f2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f2.SelectCommand = "select country,city,Chome";
                    sql_f2.SelectCommand += " from user_information";
                    sql_f2.SelectCommand += " where uid='" + sup_id + "';";
                    sql_f2.DataBind();
                    DataView ict_f2 = (DataView)sql_f2.Select(DataSourceSelectArguments.Empty);
                    string add = "";
                    if (ict_f2.Count > 0)
                    {
                        add = ict_f2.Table.Rows[0]["country"].ToString() + ict_f2.Table.Rows[0]["city"].ToString() + ict_f2.Table.Rows[0]["Chome"].ToString();
                        string result1 = String.Empty;

                            var url1 = "http://maps.google.com/maps/api/geocode/json?sensor=true&address=" + HttpContext.Current.Server.UrlEncode(add);

                            System.Net.HttpWebRequest request1 = (HttpWebRequest)HttpWebRequest.Create(url1);
                            using (var response = request1.GetResponse())
                            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                result1 = sr.ReadToEnd();
                            }
                            JObject jArray1 = JObject.Parse(result1);
                            string lat = (string)jArray1["results"][0]["geometry"]["location"]["lat"];
                            string lng = (string)jArray1["results"][0]["geometry"]["location"]["lng"];

                        string addjava=@"<script type='text/javascript' src='https://maps.googleapis.com/maps/api/js?key=AIzaSyBAgFM6PSlUcmIZR6a9AfuAgBQcC1hAVdQ&libraries=drawing&language=ja'></script>
                        <script>
        $(function () {
            //定義經緯度位置: 以政大校園的八角亭為例
            var latlng = new google.maps.LatLng(" + lat + @", " + lng + @");
            //設定地圖參數
            var mapOptions = {
                zoom: 16, //初始放大倍數
                center: latlng, //中心點所在位置
                mapTypeId: google.maps.MapTypeId.ROADMAP //正常2D道路模式
            };
            //在指定DOM元素中嵌入地圖
            var map = new google.maps.Map(
                document.getElementById('map_for_user'), mapOptions);
            //加入標示點(Marker)
            var marker = new google.maps.Marker({
                position: latlng, //經緯度
                title: '" + ict_f.Table.Rows[0]["username"].ToString() +@"', //顯示文字
                map: map //指定要放置的地圖對象
            });
        });
    </script>";
                        pel.Controls.Add(new LiteralControl(addjava));
                        pel.Controls.Add(new LiteralControl("<div id='map_for_user' style='width:100%; height:500px'></div>"));

                    }

                    pel.Controls.Add(new LiteralControl("</td>"));
                    pel.Controls.Add(new LiteralControl("</tr>"));
                    //map
                    pel.Controls.Add(new LiteralControl("</table>"));


                    pdn.Controls.Add(new LiteralControl("<tr>"));
                    pdn.Controls.Add(new LiteralControl("<td>"));




                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));

                    pdn.Controls.Add(new LiteralControl("</table>"));
                    //user content


                    pdn.Controls.Add(new LiteralControl("</td>"));
                    pdn.Controls.Add(new LiteralControl("</tr>"));
                    pdn.Controls.Add(new LiteralControl("</table>"));




                    //right View
                    Panel pdn_l = (Panel)this.FindControl("right_view");

                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));

                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='45%' height='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='40%' height='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));

                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));

                    //supporter choice
                    pdn_l.Controls.Add(new LiteralControl("<td width='45%'>"));

                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "ご依頼内容";
                    la.Font.Size = 20;
                    pdn_l.Controls.Add(la);

                    la = new Label();
                    la.Text = "※複数可";
                    la.Font.Size = FontUnit.XXSmall;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF5050");
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));

                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='40%'>"));
                    CheckBox check_get_there = new CheckBox();

                    if (ict_f.Table.Rows[0]["choice1_1"].ToString() == "1")
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "送迎";
                        check_get_there.ID = "choice1_1";
                        check_get_there.InputAttributes["value"] = "送迎";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        check_get_there.Enabled = true;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    else
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "送迎";
                        check_get_there.ID = "choice1_1";
                        check_get_there.InputAttributes["value"] = "送迎";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D0D0D0");
                        check_get_there.Enabled = false;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='50%'>"));
                    if (ict_f.Table.Rows[0]["choice1_3"].ToString() == "1")
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "利用宅で預かる";
                        check_get_there.ID = "choice1_3";
                        check_get_there.InputAttributes["value"] = "利用宅で預かる";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        check_get_there.Enabled = true;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    else
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "利用宅で預かる";
                        check_get_there.ID = "choice1_3";
                        check_get_there.InputAttributes["value"] = "利用宅で預かる";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D0D0D0");
                        check_get_there.Enabled = false;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='40%'>"));
                    if (ict_f.Table.Rows[0]["choice1_2"].ToString() == "1")
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "自宅で預かる";
                        check_get_there.ID = "choice1_2";
                        check_get_there.InputAttributes["value"] = "自宅で預かる";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        check_get_there.Enabled = true;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    else
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "自宅で預かる";
                        check_get_there.ID = "choice1_2";
                        check_get_there.InputAttributes["value"] = "自宅で預かる";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D0D0D0");
                        check_get_there.Enabled = false;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='50%'>"));
                    if (ict_f.Table.Rows[0]["choice1_4"].ToString() == "1")
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "乳児預かり";
                        check_get_there.ID = "choice1_4";
                        check_get_there.InputAttributes["value"] = "乳児預かり";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        check_get_there.Enabled = true;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    else
                    {
                        check_get_there = new CheckBox();
                        check_get_there.Text = "乳児預かり";
                        check_get_there.ID = "choice1_4";
                        check_get_there.InputAttributes["value"] = "乳児預かり";
                        check_get_there.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D0D0D0");
                        check_get_there.Enabled = false;
                        pdn_l.Controls.Add(check_get_there);
                    }
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><br/><br/>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    //supporter choice



                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='40%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));

                    //second line
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td class='main' width='45%'>"));

                    //one day or more day
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%' style='background-color: #EFEFEF;'>"));
                    //title
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "単発/定期";
                    la.Font.Size = 20;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));

                    pdn_l.Controls.Add(new LiteralControl("<div class='radio-toolbar'>"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));


                    pdn_l.Controls.Add(new LiteralControl("<input type='radio' id='radio1' name='radios' value='one' checked>"));
                    pdn_l.Controls.Add(new LiteralControl("<label for='radio1'>単発の予約リクエスト</label>"));


                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));

                    pdn_l.Controls.Add(new LiteralControl("<input type='radio' id='radio2' name='radios' value='more'>"));
                    pdn_l.Controls.Add(new LiteralControl("<label for='radio2'>定期の予約リクエスト</label>"));


                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    pdn_l.Controls.Add(new LiteralControl("</div><br/><br/>"));

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));


                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    //one day or more day

                    //one select day
                    pdn_l.Controls.Add(new LiteralControl("<div style='background-color: #EFEFEF;'>"));
                    Panel pan_selct = new Panel();
                    pan_selct.ID = "pan_selct";
                    pdn_l.Controls.Add(pan_selct);

                    //select date
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "お預け希望の日にちは？";
                    la.Font.Size = 20;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.ID = "select_date_text";
                    la.Text = "";
                    la.Font.Size = 15;
                    pan_selct.Controls.Add(la);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;"));
                    pan_selct.Controls.Add(new LiteralControl("<input onclick='showDialog()' type='button' value='変更する' style='width: 20%;cursor: pointer;text-align: center;' class='file-upload'/>"));

                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));


                    pan_selct.Controls.Add(new LiteralControl("</table><hr/>"));
                    //select date

                    //select date time
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    la = new Label();
                    la.Text = "お預け希望時間は？";
                    la.Font.Size = 20;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "開始時間";
                    la.Font.Size = 15;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));

                    DropDownList dropl = new DropDownList();
                    dropl.ID = "select_start_time";
                    pan_selct.Controls.Add(dropl);

                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    la = new Label();
                    la.Text = "時";
                    la.Font.Size = 12;
                    pan_selct.Controls.Add(la);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    dropl = new DropDownList();
                    dropl.ID = "select_start_time_minute";
                    ListItem lt = new ListItem();
                    string min = "";
                    for (int i = 0; i < 59; i+=15)
                    {
                        min = i.ToString();
                        if (i < 10) { min = "0" + i.ToString(); }
                        lt = new ListItem();
                        lt.Text = min;
                        lt.Value = min;
                        dropl.Items.Add(lt);
                    }
                    //dropl.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
                    pan_selct.Controls.Add(dropl);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    la = new Label();
                    la.Text = "分";
                    la.Font.Size = 12;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/>"));
                    la = new Label();
                    la.Text = "終了時間";
                    la.Font.Size = 15;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));

                    dropl = new DropDownList();
                    dropl.ID = "select_end_time";
                    pan_selct.Controls.Add(dropl);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    la = new Label();
                    la.Text = "時";
                    la.Font.Size = 12;
                    pan_selct.Controls.Add(la);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    dropl = new DropDownList();
                    dropl.ID = "select_end_time_minute";
                    lt = new ListItem();
                    for (int i = 0; i < 59; i+=15)
                    {
                        min = i.ToString();
                        if (i < 10) { min = "0" + i.ToString(); }
                        lt = new ListItem();
                        lt.Text = min;
                        lt.Value = min;
                        dropl.Items.Add(lt);
                    }
                    pan_selct.Controls.Add(dropl);
                    pan_selct.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                    la = new Label();
                    la.Text = "分";
                    la.Font.Size = 12;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/>"));
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td width='20%'>"));
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center' style='background-color: #EFEEEE;'>"));
                    la = new Label();
                    la.Text = "最低サポート時間目安：1時間以上";
                    la.Font.Size = 12;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                    pan_selct.Controls.Add(la);
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td width='20%'>"));
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("</table>"));

                    pan_selct.Controls.Add(new LiteralControl("</td>"));

                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("</table><hr/>"));
                    //select date time



                    //定期panel
                    Panel pan_selct_div = new Panel();
                    pan_selct_div.ID = "pan_selct_div";
                    pdn_l.Controls.Add(pan_selct_div);

                    pdn_l.Controls.Add(new LiteralControl("</div>"));

                    //week day select which can choice
                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "お預け可能目安";
                    la.Font.Size = 20;
                    pan_selct_div.Controls.Add(la);

                    pan_selct_div.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'>"));

                    //choice week can select
                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    SqlDataSource sql_f_week = new SqlDataSource();
                    sql_f_week.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f_week.SelectCommand = "select b.id,b.week_of_day,b.week_of_day_jp,b.start_hour,b.start_minute,b.end_hour,b.end_minute from user_information_store as a inner join user_information_store_week_appointment as b on b.uisid=a.id";
                    sql_f_week.SelectCommand += " where b.checked=1 and a.uid='" + sup_id + "';";
                    sql_f_week.DataBind();
                    DataView ict_f_week = (DataView)sql_f_week.Select(DataSourceSelectArguments.Empty);
                    List<dayofweek> weekday = new List<dayofweek>();
                    dayofweek aofw = new dayofweek();
                    int[] arrcheck=new int[7];
                    int[] arrcheck_id = new int[7];
                    for (int i = 0; i < 7; i++)
                    {
                        arrcheck[i] = 0;
                        arrcheck_id[i] = 0;
                    }
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "日";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "月";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "火";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "水";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "木";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "金";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "土";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);
                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    if (ict_f_week.Count > 0)
                    {
                        for (int i = 0; i < ict_f_week.Count; i++)
                        {
                            arrcheck[Convert.ToInt32(ict_f_week.Table.Rows[i]["week_of_day"].ToString()) - 1] = 1;
                            arrcheck_id[Convert.ToInt32(ict_f_week.Table.Rows[i]["week_of_day"].ToString()) - 1] =Convert.ToInt32( ict_f_week.Table.Rows[i]["id"].ToString());
                            aofw = new dayofweek();
                            aofw.jpweek = ict_f_week.Table.Rows[i]["week_of_day_jp"].ToString();
                            aofw.day = Convert.ToInt32(ict_f_week.Table.Rows[i]["week_of_day"].ToString());
                            aofw.shour = ict_f_week.Table.Rows[i]["start_hour"].ToString();
                            aofw.smin = ict_f_week.Table.Rows[i]["start_minute"].ToString();
                            aofw.ehour = ict_f_week.Table.Rows[i]["end_hour"].ToString();
                            aofw.emin = ict_f_week.Table.Rows[i]["end_minute"].ToString();
                            weekday.Add(aofw);
                        }
                    }
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    if (arrcheck[6] == 0)
                    {
                        pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                        la = new Label();
                        la.Text = "X";
                        la.Font.Size = 15;
                        pan_selct_div.Controls.Add(la);
                        pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    }
                    else
                    {
                        for (int ii = 0; ii < weekday.Count; ii++)
                        {
                            if (weekday[ii].day - 1 == 6)
                            {
                                pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                                la = new Label();
                                la.Text = "O";
                                la.Font.Size = 15;
                                pan_selct_div.Controls.Add(la);
                                pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                            }
                        }
                    }
                    for (int i = 0; i < 6; i++)
                    {

                        if (arrcheck[i] == 0)
                        {
                            pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                            la = new Label();
                            la.Text = "X";
                            la.Font.Size = 15;
                            pan_selct_div.Controls.Add(la);
                            pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                        }
                        else
                        {
                            for (int ii = 0; ii < weekday.Count; ii++)
                            {
                                if (weekday[ii].day - 1 == i)
                                {
                                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                                    la = new Label();
                                    la.Text = "O";
                                    la.Font.Size = 15;
                                    pan_selct_div.Controls.Add(la);
                                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                                }
                            }
                        }
                    }
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</table>"));



                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td><br/><br/>"));

                    la = new Label();
                    la.Text = "O";
                    la.Font.Size = 15;
                    la.ForeColor =System.Drawing.ColorTranslator.FromHtml("#999999");
                    pan_selct_div.Controls.Add(la);
                    la = new Label();
                    la.Text = "の曜日がご依頼を受けやすい日程です。";
                    la.Font.Size = 12;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                    pan_selct_div.Controls.Add(la);

                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</table><hr/>"));
                    //week day select which can choice



                    //week day select which can choice
                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    la = new Label();
                    la.Text = "定期予約ご希望曜日・時間は？";
                    la.Font.Size = 20;
                    pan_selct_div.Controls.Add(la);

                    pan_selct_div.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'>"));


                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    CheckBox check_week = new CheckBox();
                    DropDownList drop_list = new DropDownList();
                    string strr="";
                    if (arrcheck[6] == 0)
                    {
                        pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                        pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'>"));
                        pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                        pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                        check_week = new CheckBox();
                        check_week.ID = "select_weekday_"+6;
                        check_week.Text = "日";
                        check_week.InputAttributes["value"] = arrcheck_id[6].ToString();

                        check_week.Enabled = false;
                        pan_selct_div.Controls.Add(check_week);
                        pan_selct_div.Controls.Add(new LiteralControl("&nbsp;"));
                        //start hour
                        drop_list = new DropDownList();
                        drop_list.ID = "select_weekday_drop_start_hour" + 6;
                        lt = new ListItem();
                        lt.Text = "00";
                        lt.Value = "00";
                        drop_list.Items.Add(lt);
                        drop_list.Enabled = false;
                        pan_selct_div.Controls.Add(drop_list);
                        la = new Label();
                        la.Text = " 時 ";
                        la.Font.Size = 12;
                        pan_selct_div.Controls.Add(la);
                        //start minute
                        drop_list = new DropDownList();
                        drop_list.ID = "select_weekday_drop_start_minute" + 6;
                        lt = new ListItem();
                        lt.Text = "00";
                        lt.Value = "00";
                        drop_list.Items.Add(lt);
                        drop_list.Enabled = false;
                        pan_selct_div.Controls.Add(drop_list);
                        la = new Label();
                        la.Text = " 分 ～ ";
                        la.Font.Size = 12;
                        pan_selct_div.Controls.Add(la);

                        //end hour
                        drop_list = new DropDownList();
                        drop_list.ID = "select_weekday_drop_end_hour" + 6;
                        lt = new ListItem();
                        lt.Text = "00";
                        lt.Value = "00";
                        drop_list.Items.Add(lt);
                        drop_list.Enabled = false;
                        pan_selct_div.Controls.Add(drop_list);
                        la = new Label();
                        la.Text = " 時 ";
                        la.Font.Size = 12;
                        pan_selct_div.Controls.Add(la);
                        //end minute
                        drop_list = new DropDownList();
                        drop_list.ID = "select_weekday_drop_end_minute" + 6;
                        lt = new ListItem();
                        lt.Text = "00";
                        lt.Value = "00";
                        drop_list.Items.Add(lt);
                        drop_list.Enabled = false;
                        pan_selct_div.Controls.Add(drop_list);
                        la = new Label();
                        la.Text = " 分";
                        la.Font.Size = 12;
                        pan_selct_div.Controls.Add(la);


                        pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                        pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    }
                    else
                    {
                        for (int ii = 0; ii < weekday.Count; ii++)
                        {
                            if (weekday[ii].day - 1 == 6)
                            {
                                pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                                pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'>"));
                                pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                                pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                                check_week = new CheckBox();
                                check_week.ID = "select_weekday_" + 6;
                                check_week.Text = weekday[ii].jpweek;
                                check_week.InputAttributes["value"] = arrcheck_id[6].ToString();

                                pan_selct_div.Controls.Add(check_week);

                                pan_selct_div.Controls.Add(new LiteralControl("&nbsp;"));
                                //start hour
                                drop_list = new DropDownList();
                                drop_list.ID = "select_weekday_drop_start_hour" + 6;

                                for (int k = Convert.ToInt32(weekday[ii].shour); k <= Convert.ToInt32(weekday[ii].ehour); k++)
                                {
                                    strr=k.ToString();
                                    if (k < 10)
                                    {
                                        strr = "0" + k.ToString();
                                    }
                                    lt = new ListItem();
                                    lt.Text = strr;
                                    lt.Value = strr;
                                    drop_list.Items.Add(lt);
                                }


                                pan_selct_div.Controls.Add(drop_list);
                                la = new Label();
                                la.Text = " 時 ";
                                la.Font.Size = 12;
                                pan_selct_div.Controls.Add(la);
                                //start minute
                                drop_list = new DropDownList();
                                drop_list.ID = "select_weekday_drop_start_minute" + 6;

                                for (int k = 0; k < 59; k+=15)
                                {
                                    strr = k.ToString();
                                    if (k < 10)
                                    {
                                        strr = "0" + k.ToString();
                                    }
                                    lt = new ListItem();
                                    lt.Text = strr;
                                    lt.Value = strr;
                                    drop_list.Items.Add(lt);
                                }
                                drop_list.Items.FindByValue(weekday[ii].smin).Selected = true;
                                pan_selct_div.Controls.Add(drop_list);
                                la = new Label();
                                la.Text = " 分 ～ ";
                                la.Font.Size = 12;
                                pan_selct_div.Controls.Add(la);

                                //end hour
                                drop_list = new DropDownList();
                                drop_list.ID = "select_weekday_drop_end_hour" + 6;
                                for (int k = Convert.ToInt32(weekday[ii].shour); k <= Convert.ToInt32(weekday[ii].ehour); k++)
                                {
                                    strr = k.ToString();
                                    if (k < 10)
                                    {
                                        strr = "0" + k.ToString();
                                    }
                                    lt = new ListItem();
                                    lt.Text = strr;
                                    lt.Value = strr;
                                    drop_list.Items.Add(lt);
                                }
                                strr = Convert.ToInt32(weekday[ii].ehour).ToString();
                                if (Convert.ToInt32(weekday[ii].ehour) < 10)
                                {
                                    strr = "0" + Convert.ToInt32(weekday[ii].ehour).ToString();
                                }

                                drop_list.Items.FindByValue(strr).Selected = true;
                                pan_selct_div.Controls.Add(drop_list);
                                la = new Label();
                                la.Text = " 時 ";
                                la.Font.Size = 12;
                                pan_selct_div.Controls.Add(la);
                                //end minute
                                drop_list = new DropDownList();
                                drop_list.ID = "select_weekday_drop_end_minute" + 6;
                                for (int k = 0; k < 59; k+=15)
                                {
                                    strr = k.ToString();
                                    if (k < 10)
                                    {
                                        strr = "0" + k.ToString();
                                    }
                                    lt = new ListItem();
                                    lt.Text = strr;
                                    lt.Value = strr;
                                    drop_list.Items.Add(lt);
                                }
                                drop_list.Items.FindByValue(weekday[ii].emin).Selected = true;
                                pan_selct_div.Controls.Add(drop_list);
                                la = new Label();
                                la.Text = " 分";
                                la.Font.Size = 12;
                                pan_selct_div.Controls.Add(la);





                                pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                                pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                            }
                        }
                    }
                    for (int i = 0; i < 6; i++)
                    {

                        if (arrcheck[i] == 0)
                        {
                            pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                            pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'>"));
                            pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                            pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                            check_week = new CheckBox();
                            check_week.ID = "select_weekday_" + i;
                            check_week.InputAttributes["value"] = arrcheck_id[i].ToString();
                            if (i == 0) { check_week.Text = "月"; }
                            else if (i == 1) { check_week.Text = "火"; }
                            else if (i == 2) { check_week.Text = "水"; }
                            else if (i == 3) { check_week.Text = "木"; }
                            else if (i == 4) { check_week.Text = "金"; }
                            else if (i == 5) { check_week.Text = "土"; }


                            check_week.Enabled = false;
                            pan_selct_div.Controls.Add(check_week);
                            pan_selct_div.Controls.Add(new LiteralControl("&nbsp;"));
                            //start hour
                            drop_list = new DropDownList();
                            drop_list.ID = "select_weekday_drop_start_hour" + i;
                            lt = new ListItem();
                            lt.Text = "00";
                            lt.Value = "00";
                            drop_list.Items.Add(lt);
                            drop_list.Enabled = false;
                            pan_selct_div.Controls.Add(drop_list);
                            la = new Label();
                            la.Text = " 時 ";
                            la.Font.Size = 12;
                            pan_selct_div.Controls.Add(la);
                            //start minute
                            drop_list = new DropDownList();
                            drop_list.ID = "select_weekday_drop_start_minute" + i;
                            lt = new ListItem();
                            lt.Text = "00";
                            lt.Value = "00";
                            drop_list.Items.Add(lt);
                            drop_list.Enabled = false;
                            pan_selct_div.Controls.Add(drop_list);
                            la = new Label();
                            la.Text = " 分 ～ ";
                            la.Font.Size = 12;
                            pan_selct_div.Controls.Add(la);

                            //end hour
                            drop_list = new DropDownList();
                            drop_list.ID = "select_weekday_drop_end_hour" + i;
                            lt = new ListItem();
                            lt.Text = "00";
                            lt.Value = "00";
                            drop_list.Items.Add(lt);
                            drop_list.Enabled = false;
                            pan_selct_div.Controls.Add(drop_list);
                            la = new Label();
                            la.Text = " 時 ";
                            la.Font.Size = 12;
                            pan_selct_div.Controls.Add(la);
                            //end minute
                            drop_list = new DropDownList();
                            drop_list.ID = "select_weekday_drop_end_minute" + i;
                            lt = new ListItem();
                            lt.Text = "00";
                            lt.Value = "00";
                            drop_list.Items.Add(lt);
                            drop_list.Enabled = false;
                            pan_selct_div.Controls.Add(drop_list);
                            la = new Label();
                            la.Text = " 分";
                            la.Font.Size = 12;
                            pan_selct_div.Controls.Add(la);


                            pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                            pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                        }
                        else
                        {
                            for (int ii = 0; ii < weekday.Count; ii++)
                            {
                                if (weekday[ii].day - 1 == i)
                                {
                                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                                    pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'>"));
                                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));
                                    check_week = new CheckBox();
                                    check_week.ID = "select_weekday_" + i;
                                    check_week.Text = weekday[ii].jpweek;
                                    check_week.InputAttributes["value"] = arrcheck_id[i].ToString();
                                    pan_selct_div.Controls.Add(check_week);

                                    pan_selct_div.Controls.Add(new LiteralControl("&nbsp;"));
                                    //start hour
                                    drop_list = new DropDownList();
                                    drop_list.ID = "select_weekday_drop_start_hour" + i;

                                    for (int k = Convert.ToInt32(weekday[ii].shour); k <= Convert.ToInt32(weekday[ii].ehour); k++)
                                    {
                                        strr = k.ToString();
                                        if (k < 10)
                                        {
                                            strr = "0" + k.ToString();
                                        }
                                        lt = new ListItem();
                                        lt.Text = strr;
                                        lt.Value = strr;
                                        drop_list.Items.Add(lt);
                                    }


                                    pan_selct_div.Controls.Add(drop_list);
                                    la = new Label();
                                    la.Text = " 時 ";
                                    la.Font.Size = 12;
                                    pan_selct_div.Controls.Add(la);
                                    //start minute
                                    drop_list = new DropDownList();
                                    drop_list.ID = "select_weekday_drop_start_minute" + i;

                                    for (int k = 0; k < 59; k+=15)
                                    {
                                        strr = k.ToString();
                                        if (k < 10)
                                        {
                                            strr = "0" + k.ToString();
                                        }
                                        lt = new ListItem();
                                        lt.Text = strr;
                                        lt.Value = strr;
                                        drop_list.Items.Add(lt);
                                    }
                                    drop_list.Items.FindByValue(weekday[ii].smin).Selected = true;
                                    pan_selct_div.Controls.Add(drop_list);
                                    la = new Label();
                                    la.Text = " 分 ～ ";
                                    la.Font.Size = 12;
                                    pan_selct_div.Controls.Add(la);

                                    //end hour
                                    drop_list = new DropDownList();
                                    drop_list.ID = "select_weekday_drop_end_hour" + i;
                                    for (int k = Convert.ToInt32(weekday[ii].shour); k <= Convert.ToInt32(weekday[ii].ehour); k++)
                                    {
                                        strr = k.ToString();
                                        if (k < 10)
                                        {
                                            strr = "0" + k.ToString();
                                        }
                                        lt = new ListItem();
                                        lt.Text = strr;
                                        lt.Value = strr;
                                        drop_list.Items.Add(lt);
                                    }
                                    strr=Convert.ToInt32(weekday[ii].ehour).ToString();
                                    if (Convert.ToInt32(weekday[ii].ehour) < 10)
                                    {
                                        strr ="0"+ Convert.ToInt32(weekday[ii].ehour).ToString();
                                    }

                                    drop_list.Items.FindByValue(strr).Selected = true;

                                    drop_list.SelectedIndex = drop_list.Items.Count;
                                    pan_selct_div.Controls.Add(drop_list);
                                    la = new Label();
                                    la.Text = " 時 ";
                                    la.Font.Size = 12;
                                    pan_selct_div.Controls.Add(la);
                                    //end minute
                                    drop_list = new DropDownList();
                                    drop_list.ID = "select_weekday_drop_end_minute" + i;
                                    for (int k = 0; k < 59; k+=15)
                                    {
                                        strr = k.ToString();
                                        if (k < 10)
                                        {
                                            strr = "0" + k.ToString();
                                        }
                                        lt = new ListItem();
                                        lt.Text = strr;
                                        lt.Value = strr;
                                        drop_list.Items.Add(lt);
                                    }
                                    drop_list.Items.FindByValue(weekday[ii].emin).Selected = true;
                                    pan_selct_div.Controls.Add(drop_list);
                                    la = new Label();
                                    la.Text = " 分";
                                    la.Font.Size = 12;
                                    pan_selct_div.Controls.Add(la);



                                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                                }
                            }
                        }
                    }

                    pan_selct_div.Controls.Add(new LiteralControl("</table>"));

                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</table><hr/>"));
                    //week day select which can choice



                    //week day select start date
                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    la = new Label();
                    la.Text = "いつから定期予約をご希望ですか？";
                    la.Font.Size = 20;
                    pan_selct_div.Controls.Add(la);

                    pan_selct_div.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td align='center'>"));

                    //start date to end date
                    pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //start date
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));

                    la = new Label();
                    la.Text = "開始";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);

                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));

                    pan_selct_div.Controls.Add(new LiteralControl("<p><input type='text' id='datepicker_start' placeholder='2016/09/01' class='textbox' readonly></p>"));

                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    //end date
                    pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td width='20%'></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));

                    la = new Label();
                    la.Text = "終了";
                    la.Font.Size = 15;
                    pan_selct_div.Controls.Add(la);


                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("<td>"));

                    pan_selct_div.Controls.Add(new LiteralControl("<p><input type='text' id='datepicker_end' placeholder='2016/09/01' class='textbox' readonly></p>"));

                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</table>"));


                    pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct_div.Controls.Add(new LiteralControl("</table><hr/>"));
                    //week day select start date



                    ////week day select which can choice
                    //pan_selct_div.Controls.Add(new LiteralControl("<table width='100%'>"));
                    ////title
                    //pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    //la = new Label();
                    //la.Text = "お預け可能目安";
                    //la.Font.Size = 20;
                    //pan_selct_div.Controls.Add(la);

                    //pan_selct_div.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("<tr>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("<td align='center'>"));
                    //TextBox tex = new TextBox();
                    //tex.TextMode = TextBoxMode.MultiLine;
                    //tex.Width = Unit.Percentage(100);
                    //tex.Height = 100;
                    //tex.Attributes["placeholder"] = "例) 子どもは人見知りはしますがおとなしいですぜひお願いします。";
                    //pan_selct_div.Controls.Add(tex);

                    //pan_selct_div.Controls.Add(new LiteralControl("</td>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("</tr>"));
                    //pan_selct_div.Controls.Add(new LiteralControl("</table>"));
                    ////week day select which can choice




                    //don't move
                    pan_selct = (Panel)this.FindControl("right_view");

                    //select which kid
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    la = new Label();
                    la.Text = "どのお子様を預けますか？";
                    la.Font.Size = 20;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td>"));

                    //kid choice
                    CheckBox check_kid = new CheckBox();
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    SqlDataSource sql_f_user = new SqlDataSource();
                    sql_f_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f_user.SelectCommand = "select id,Sex,birthday_year,birthday_month,birthday_day";
                    sql_f_user.SelectCommand += " from user_information_school_children";
                    //sql_f_user.SelectCommand += " where uid='" + id + "';";
                    sql_f_user.DataBind();
                    DataView ict_f_user = (DataView)sql_f_user.Select(DataSourceSelectArguments.Empty);
                    if (ict_f_user.Count > 0)
                    {
                        howmany_kid_HiddenField.Value = ict_f_user.Count.ToString();
                        for (int i = 0; i < ict_f_user.Count; i++)
                        {
                            if (i == 0)
                            {
                                pan_selct.Controls.Add(new LiteralControl("<tr>"));
                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                                }
                            }
                            pan_selct.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                            pan_selct.Controls.Add(new LiteralControl("<td width='40%'>"));
                            check_kid = new CheckBox();
                            check_kid.ID = "kid_" + i;
                            check_kid.InputAttributes["value"] = ict_f_user.Table.Rows[i]["id"].ToString();
                            string kidstr = "";
                            if (ict_f_user.Table.Rows[i]["Sex"].ToString() == "0")
                            {
                                kidstr += "女の子     ";
                            }
                            else
                            {
                                kidstr += "男の子     ";
                            }
                            int byear = Convert.ToInt32(ict_f_user.Table.Rows[i]["birthday_year"].ToString());
                            int bmon = Convert.ToInt32(ict_f_user.Table.Rows[i]["birthday_month"].ToString());
                            int bday = Convert.ToInt32(ict_f_user.Table.Rows[i]["birthday_day"].ToString());

                            int toyear = DateTime.Now.Year;
                            int tomon = DateTime.Now.Month;

                            int comyear = toyear - byear;
                            int common = tomon - bmon;

                            if (common < 0)
                            {
                                comyear -= 1;
                                common += 12;
                            }
                            kidstr += comyear + " 歳 " + common + "ヶ月";

                            check_kid.Text = kidstr;
                            pan_selct.Controls.Add(check_kid);
                            pan_selct.Controls.Add(new LiteralControl("</td>"));
                        }
                        if (ict_f_user.Count % 2 == 1)
                        {
                            pan_selct.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                            pan_selct.Controls.Add(new LiteralControl("<td width='40%'></td>"));
                            pan_selct.Controls.Add(new LiteralControl("</tr>"));
                        }
                        else
                        {
                            pan_selct.Controls.Add(new LiteralControl("</tr>"));
                        }
                    }
                    pan_selct.Controls.Add(new LiteralControl("</table>"));


                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));

                    //受入可能人数：最大 3人
                    //保育可能年齢：0歳4ヶ月〜15歳12ヶ月
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td width='20%'>"));
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center' style='background-color: #EFEEEE;'>"));

                    la = new Label();
                    la.Text = "受入可能人数：最大" + ict_f.Table.Rows[0]["howmany"].ToString() + "人";
                    la.Font.Size = 12;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                    pan_selct.Controls.Add(la);
                    pan_selct.Controls.Add(new LiteralControl("<br/>"));
                    la = new Label();
                    la.Text = "保育可能年齢：" + ict_f.Table.Rows[0]["age_range_start_year"].ToString() + "歳" + ict_f.Table.Rows[0]["age_range_start_month"].ToString() + "ヶ月〜";
                    la.Text += ict_f.Table.Rows[0]["age_range_end_year"].ToString() + "歳" + ict_f.Table.Rows[0]["age_range_end_month"].ToString() + "ヶ月";
                    la.Font.Size = 12;
                    la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                    pan_selct.Controls.Add(la);


                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td width='20%'>"));
                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("</table>"));

                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("</table><hr/>"));
                    //select which kid


                    //user content
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));

                    la = new Label();
                    la.Text = "リクエストコメント";
                    la.Font.Size = 20;
                    pan_selct.Controls.Add(la);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'>"));


                    TextBox tex = new TextBox();
                    tex.ID = "notice_textbox";
                    tex.TextMode = TextBoxMode.MultiLine;
                    tex.Width = Unit.Percentage(100);
                    tex.Height = 100;
                    tex.Attributes["placeholder"] = "例) 子どもは人見知りはしますがおとなしいですぜひお願いします。";
                    pan_selct.Controls.Add(tex);

                    pan_selct.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));


                    pan_selct.Controls.Add(new LiteralControl("</table>"));
                    //user content


                    //money down
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%' class='bill2' style='background-color: #EFEFEF;'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' align='center'><br/><br/>"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "お見積合計";
                    la.Font.Size = 20;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_money1";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><hr/>"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "基本料金";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "simple_money1";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "申込み時間";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_hour1";
                    la.Text = "0時間";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "手数料";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_tax1";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><hr/>"));
                    pdn_l.Controls.Add(new LiteralControl("<br/><br/><table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td align='center'>"));

                    HyperLink hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "ご登録のクレジットカードの再確認 >";
                    hyy.Font.Underline = false;
                    pdn_l.Controls.Add(hyy);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><br/><br/>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));



                    //SUBMIT
                    pan_selct.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //title
                    pan_selct.Controls.Add(new LiteralControl("<tr>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));

                    pan_selct.Controls.Add(new LiteralControl("<input id='but_one' type='button' value='予約する' class='file-upload' style='width:50%;' onclick='ShowProgressBar();' />"));

                    //Button but_one = new Button();
                    //but_one.ID = "but_one";
                    //but_one.Text = "予約する";
                    //but_one.Click += sent_Button_Click;
                    //but_one.Attributes["width"] = "50%";
                    //but_one.Attributes["cursor"] = "pointer";
                    //but_one.Attributes["text-align"] = "center";
                    //but_one.CssClass = "file-upload";

                    //pan_selct.Controls.Add(but_one);

                    pan_selct.Controls.Add(new LiteralControl("<br/><br/></td>"));
                    pan_selct.Controls.Add(new LiteralControl("</tr>"));

                    pan_selct.Controls.Add(new LiteralControl("</table>"));
                    //SUBMIT



                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td class='space' width='40%'>"));

                    pdn_l.Controls.Add(new LiteralControl("<table width='100%' class='bill1' style='background-color: #EFEFEF;'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' align='center'><br/><br/>"));

                    //total money
                    //pdn_l.Controls.Add(new LiteralControl("<div id='money_view_o'>"));
                    //pdn_l.Controls.Add(new LiteralControl("<div id='money_view' >"));
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "お見積合計";
                    la.Font.Size = 20;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_money";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><hr/>"));
                    //total money


                    //total money detail
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "基本料金";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "simple_money";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    //total money detail


                    //total money detail
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "申込み時間";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_hour";
                    la.Text = "0時間";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));
                    //total money detail


                    //total money detail
                    pdn_l.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td>"));
                    la = new Label();
                    la.Text = "手数料";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='30%'>"));
                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pan_selct.Controls.Add(new LiteralControl("<td align='right'>"));

                    la = new Label();
                    la.ID = "total_tax";
                    la.Text = "0円";
                    la.Font.Size = 15;
                    pdn_l.Controls.Add(la);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><hr/>"));
                    //total money detail



                    //total money detail
                    pdn_l.Controls.Add(new LiteralControl("<br/><br/><table width='100%'>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td align='center'>"));

                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "ご登録のクレジットカードの再確認 >";
                    hyy.Font.Underline = false;
                    pdn_l.Controls.Add(hyy);

                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table><br/><br/>"));
                    //total money detail
                    //pdn_l.Controls.Add(new LiteralControl("</div>"));
                    //pdn_l.Controls.Add(new LiteralControl("</div>"));




                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<tr>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    pdn_l.Controls.Add(new LiteralControl("</table>"));


                    pdn_l.Controls.Add(new LiteralControl("</td>"));
                    pdn_l.Controls.Add(new LiteralControl("<td width='5%'></td>"));
                    pdn_l.Controls.Add(new LiteralControl("</tr>"));
                    //second line

                    pdn_l.Controls.Add(new LiteralControl("</table>"));



                }




            }

    }
    public class user_information
    {
        public string good_id = "";
        public string uid = "";
        public string username = "";
        public string photo = "";
        public string score_message = "";
        public DateTime check_time = new DateTime();
    }
    public class holiday
    {
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public string name = "";
    }
    public class dayofweek
    {
        public string jpweek = "";
        public int day = 0;
        public string shour = "";
        public string smin = "";
        public string ehour = "";
        public string emin = "";
    }
    private void CreateCalendar(int Year, int Month, DataTable DT)
    {
        DateTime whichDate = new DateTime(Year, Month, 1);


        string strCalendar = "";

        // 每月日數陣列
        int[] monthDays = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        // 閏年判斷
        int myYear = whichDate.Year;
        int myMonth = whichDate.Month;
        if (((myYear % 4 == 0) && (myYear % 100 != 0))
        || (myYear % 400 == 0))
            monthDays[1] = 29;


        string[] myArray =
        new string[7] {"Sunday","Monday","Tuesday","Wednesday"
				,"Thursday","Friday","Saturday"};

        string[] myArray2 = new string[7] { "0", "1", "2", "3", "4", "5", "6" };


        // 該月第一天的星期
        int day =
        Convert.ToInt32(myArray2[Array.IndexOf(myArray, whichDate.DayOfWeek.ToString())]);


        // 計算秀出時需要的格數
        int total = monthDays[myMonth - 1] + day;
        int totalCells = total + ((total % 7 != 0) ? 7 - total % 7 : 0);


        // 標題列
        strCalendar = "<TABLE id=\"myCalendarTable\" class=\"myGrid\" style=\"border-collapse:collapse\" cellSpacing=\"0\" cellPadding=\"4\" align=\"center\" width=\"100%\" border=\"0\">";
        strCalendar += "<TR>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期日</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期一</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期二</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期三</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期四</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期五</TD>";
        strCalendar += "<TD class=\"GridHeading\" width=\"14%\">星期六</TD>";
        strCalendar += "</TR>";


        for (int i = 0; i < totalCells; i++)
        {
            if (i % 7 == 0)
            {
                strCalendar += "</TR><TR height=\"70px\">";
            }


            if (i >= day && i < total)
            {
                int myDay = ((i - day) + 1);
                string myDateStr = whichDate.ToString("yyyy/MM/") + myDay.ToString("D2");
                if (!DT.Rows.Contains(new DateTime(myYear, myMonth, myDay)))
                {
                    strCalendar += "<TD class=\"myItem\" onclick=\"addClass(this);\" >" +
                    myMonth.ToString("D2") + "/" + myDay.ToString("D2");
                }
                else
                {
                    string myContentStr = "0";
                    DataRow rowFound;

                    // 設定DataTable的PrimaryKey，這樣之後才可以對這個DataTable作尋找內容的動作
                    DataColumn[] ColumnPrimaryKey = new DataColumn[1];
                    ColumnPrimaryKey[0] = DT.Columns["myDate"];
                    DT.PrimaryKey = ColumnPrimaryKey;

                    // 因為已經設定myDate為主鍵，他是DateTime型別的，故這邊用DateTime搜尋
                    rowFound = DT.Rows.Find(new DateTime(myYear, myMonth, myDay));
                    if (rowFound != null)
                    {
                        myContentStr = rowFound["whichNumber"].ToString();
                    }


                    strCalendar += "<TD class=\"myItem\" onclick=\"addClass(this);\">" +
                                    myMonth.ToString("D2") + "/" + myDay.ToString("D2");
                }
            }
            else
            {
                strCalendar += "<TD class=\"myItem\">&nbsp;";
            }


            strCalendar += "</TD>";
        }

        calendarDiv.InnerHtml = strCalendar;
    }

    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        //if (!e.Day.IsOtherMonth && !e.Day.IsWeekend)
        //{
        //    e.Cell.BackColor = System.Drawing.Color.Gray;
        //}
        e.Cell.BackColor = System.Drawing.Color.White;
        e.Cell.Controls.Clear();
        Label date = new Label();
        date.Text = e.Day.DayNumberText;
        e.Cell.Controls.Add(date);
        if (e.Day.IsOtherMonth || e.Day.Date < DateTime.Now.Date)
        {

            Label table_top = new Label();
            table_top.Text = "</br>";
            table_top.Text += "<table><tr><td>";
            Image im = new Image();
            im.ImageUrl = "~/images/weekend.PNG";
            im.Attributes.Add("width", "100%");
            im.Attributes.Add("height", "50%");
            Label table_down = new Label();
            table_down.Text = "</td></tr></table>";
            e.Cell.Controls.Add(table_top);
            e.Cell.Controls.Add(im);
            e.Cell.Controls.Add(table_down);

        }
        List<int> check_date=new List<int>();
        List<int> no_check_date = new List<int>();

         if (Session["id"] == null || Session["sup_id"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sorry you stay too long!')", true);
                Response.Redirect("main.aspx");
            }
            else
            {
        string sup_id = Session["sup_id"].ToString();
        string id = Session["id"].ToString();


        SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        SqlDataSource1.SelectCommand = "select checked,day from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
        SqlDataSource1.SelectCommand += " and month='"+e.Day.Date.Month+"';";
        SqlDataSource1.DataBind();
        DataView ou1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        for (int i = 0; i < ou1.Count; i++)
        {
            if (ou1.Table.Rows[i]["checked"].ToString() == "1")
            {
                check_date.Add(Convert.ToInt32(ou1.Table.Rows[i]["day"].ToString()));
            }
            else
            {
                no_check_date.Add(Convert.ToInt32(ou1.Table.Rows[i]["day"].ToString()));
            }
        }

        //week of day
        //ViewState["day_of_week"]

        int weekday=0;
        date_week = (List<dayofweek>)ViewState["day_of_week"];

        //DayOfWeek.Thursday

        //     e.Day.Date.DayOfWeek


        ////祝日
        //date_holiday = (List<holiday>)ViewState["myHoliday"];
        //foreach (var item in date_holiday)
        //{
        //    ress += item.name;
        //    ress += item.year;
        //    ress += item.month;
        //    ress += item.day;
        //}


        if (!e.Day.IsOtherMonth && e.Day.Date >= DateTime.Now.Date)
        {
            Label table_top = new Label();
            table_top.Text = "</br>";
            table_top.Text += "<table><tr><td align='center'>";
            bool check_day = false;
            for (int i = 0; i < check_date.Count; i++)
            {
                if (e.Day.Date.Day == check_date[i])
                {
                    check_day = true;
                }
            }
            bool no_check_day = false;
            for (int i = 0; i < no_check_date.Count; i++)
            {
                if (e.Day.Date.Day == no_check_date[i])
                {
                    no_check_day = true;
                }
            }

            bool check_week_day = false;
            if (e.Day.Date.DayOfWeek == DayOfWeek.Monday)
            {
                weekday = 1;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Tuesday)
            {
                weekday = 2;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Wednesday)
            {
                weekday = 3;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Thursday)
            {
                weekday = 4;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Friday)
            {
                weekday = 5;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                weekday = 6;
            }
            if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                weekday = 7;
            }
            dayofweek dow = new dayofweek();
            for (int i = 0; i < date_week.Count; i++)
            {
                if (date_week[i].day == weekday)
                {
                    check_week_day = true;
                    dow.day = date_week[i].day;
                    dow.shour = date_week[i].shour;
                    dow.smin = date_week[i].smin;
                    dow.ehour = date_week[i].ehour;
                    dow.emin = date_week[i].emin;

                }
            }




            Label la = new Label();
            la.Text="X";

            la.CssClass = "button_no button_no_day";


            Panel date_click = new Panel();
            string dateid = e.Day.Date.Year.ToString() + ".";
            if (e.Day.Date.Month < 10)
            {
                dateid += "0" + e.Day.Date.Month + ".";
            }
            else
            {
                dateid += e.Day.Date.Month + ".";
            }
            if (e.Day.Date.Day < 10)
            {
                dateid += "0" + e.Day.Date.Day;
            }
            else
            {
                dateid += e.Day.Date.Day;
            }

            date_click.ID = dateid;
            date_click.CssClass = "button button_yes";
            //date_click.Attributes["onclick"] = "selectdate(this.id)";

            Label table_down = new Label();
            table_down.Text = "</td></tr></table>";
            e.Cell.Controls.Add(table_top);

            if (check_week_day)
            {
                if (check_day)
                {
                    SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    SqlDataSource1.SelectCommand = "select * from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
                    SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "' and day='" + e.Day.Date.Day + "';";
                    SqlDataSource1.DataBind();
                    DataView ou2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                    for (int i = 0; i < ou2.Count; i++)
                    {
                        Label laa = new Label();
                        string txt = ou2.Table.Rows[i]["start_hour"].ToString() + ":";

                            txt += ou2.Table.Rows[i]["start_minute"].ToString();

                        txt += @"</br>|</br>" + ou2.Table.Rows[i]["end_hour"].ToString() + ":";

                            txt += ou2.Table.Rows[i]["end_minute"].ToString();

                        laa.Text = txt;
                        date_click.Controls.Add(laa);
                    }
                    e.Cell.Controls.Add(date_click);
                }
                else if (no_check_day)
                {
                    e.Cell.Controls.Add(la);
                }
                else
                {
                    Label laa = new Label();
                    string txt = dow.shour + ":"+dow.smin;
                    txt += @"</br>|</br>" + dow.ehour + ":"+dow.emin;
                    laa.Text = txt;
                    date_click.Controls.Add(laa);

                    e.Cell.Controls.Add(date_click);
                }

            }
            else if (check_day)
            {
                SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                SqlDataSource1.SelectCommand = "select * from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
                SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "' and day='" + e.Day.Date.Day + "';";
                SqlDataSource1.DataBind();
                DataView ou2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                for (int i = 0; i < ou2.Count; i++)
                {
                    Label laa = new Label();
                    string txt = ou2.Table.Rows[i]["start_hour"].ToString() + ":";

                        txt += ou2.Table.Rows[i]["start_minute"].ToString();

                    txt += @"</br>|</br>" + ou2.Table.Rows[i]["end_hour"].ToString() + ":";

                        txt += ou2.Table.Rows[i]["end_minute"].ToString();

                    laa.Text = txt;
                    date_click.Controls.Add(laa);
                }
                e.Cell.Controls.Add(date_click);
            }
            else
            {
                e.Cell.Controls.Add(la);
            }




            e.Cell.Controls.Add(table_down);
            //e.Cell.BackColor = System.Drawing.Color.Gray;

        }

        SelectedDatesCollection theDates = Calendar1.SelectedDates;


        //Button time_click = new Button();
        //time_click.Width = 50; time_click.Height = 50;
        //time_click.ID = "";


        //if (e.Day.IsWeekend )
        //{
        //    e.Cell.Controls.Clear();

        //}
         }
    }
    protected void butim_Click(object sender, EventArgs e)
    {
        Button temp = (Button)sender;
        if (temp.ID == "butim")
        {
        }

    }
    protected void Calendar2_DayRender(object sender, DayRenderEventArgs e)
    {
        //if (!e.Day.IsOtherMonth && !e.Day.IsWeekend)
        //{
        //    e.Cell.BackColor = System.Drawing.Color.Gray;
        //}


        e.Cell.BackColor = System.Drawing.Color.White;
        e.Cell.Controls.Clear();



        Label date = new Label();
        date.Text = e.Day.DayNumberText;
        e.Cell.Controls.Add(date);
        if (e.Day.IsOtherMonth || e.Day.Date < DateTime.Now.Date)
        {

            Label table_top = new Label();
            table_top.Text = "</br>";
            table_top.Text += "<table class='calendar_size'><tr><td>";

            Label table_down = new Label();
            table_down.Text = "</td></tr></table>";
            e.Cell.Controls.Add(table_top);

            e.Cell.Controls.Add(table_down);

        }
        List<int> check_date = new List<int>();
        List<int> no_check_date = new List<int>();

        if (Session["id"] == null || Session["sup_id"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("main.aspx");
        }
        else
        {
            string sup_id = Session["sup_id"].ToString();
            string id = Session["id"].ToString();


            SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource1.SelectCommand = "select checked,day from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
            SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "';";
            SqlDataSource1.DataBind();
            DataView ou1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ou1.Count; i++)
            {
                if (ou1.Table.Rows[i]["checked"].ToString() == "1")
                {
                    check_date.Add(Convert.ToInt32(ou1.Table.Rows[i]["day"].ToString()));
                }
                else
                {
                    no_check_date.Add(Convert.ToInt32(ou1.Table.Rows[i]["day"].ToString()));
                }
            }

            //week of day
            //ViewState["day_of_week"]

            int weekday = 0;
            date_week = (List<dayofweek>)ViewState["day_of_week"];

            //DayOfWeek.Thursday

            //     e.Day.Date.DayOfWeek


            ////祝日
            //date_holiday = (List<holiday>)ViewState["myHoliday"];
            //foreach (var item in date_holiday)
            //{
            //    ress += item.name;
            //    ress += item.year;
            //    ress += item.month;
            //    ress += item.day;
            //}


            if (!e.Day.IsOtherMonth && e.Day.Date >= DateTime.Now.Date)
            {
                Label table_top = new Label();
                table_top.Text = "</br>";
                table_top.Text += "<table><tr><td align='center'>";
                bool check_day = false;
                for (int i = 0; i < check_date.Count; i++)
                {
                    if (e.Day.Date.Day == check_date[i])
                    {
                        check_day = true;
                    }
                }
                bool no_check_day = false;
                for (int i = 0; i < no_check_date.Count; i++)
                {
                    if (e.Day.Date.Day == no_check_date[i])
                    {
                        no_check_day = true;
                    }
                }

                bool check_week_day = false;
                if (e.Day.Date.DayOfWeek == DayOfWeek.Monday)
                {
                    weekday = 1;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Tuesday)
                {
                    weekday = 2;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Wednesday)
                {
                    weekday = 3;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Thursday)
                {
                    weekday = 4;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Friday)
                {
                    weekday = 5;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Saturday)
                {
                    weekday = 6;
                }
                if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekday = 7;
                }
                dayofweek dow = new dayofweek();
                for (int i = 0; i < date_week.Count; i++)
                {
                    if (date_week[i].day == weekday)
                    {
                        check_week_day = true;
                        dow.day = date_week[i].day;
                        dow.shour = date_week[i].shour;
                        dow.smin = date_week[i].smin;
                        dow.ehour = date_week[i].ehour;
                        dow.emin = date_week[i].emin;

                    }
                }




                Label la = new Label();
                la.Text = "X";

                la.CssClass = "button_no button_no_day";


                Panel date_click = new Panel();
                string dateid = e.Day.Date.Year.ToString()+".";
                if (e.Day.Date.Month < 10)
                {
                    dateid += "0" + e.Day.Date.Month + ".";
                }
                else
                {
                    dateid += e.Day.Date.Month + ".";
                }
                if (e.Day.Date.Day < 10)
                {
                    dateid += "0" + e.Day.Date.Day;
                }
                else
                {
                    dateid += e.Day.Date.Day;
                }

                date_click.ID = dateid;
                date_click.CssClass = "button button_yes";
                date_click.Attributes["onclick"] = "selectdate(this.id)";

                Label table_down = new Label();
                table_down.Text = "</td></tr></table>";
                e.Cell.Controls.Add(table_top);

                if (check_week_day)
                {
                    if (check_day)
                    {
                        SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        SqlDataSource1.SelectCommand = "select * from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
                        SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "' and day='" + e.Day.Date.Day + "';";
                        SqlDataSource1.DataBind();
                        DataView ou2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                        for (int i = 0; i < ou2.Count; i++)
                        {
                            Label laa = new Label();
                            string txt = ou2.Table.Rows[i]["start_hour"].ToString() + ":";

                                txt += ou2.Table.Rows[i]["start_minute"].ToString();

                            txt += @"</br>|</br>" + ou2.Table.Rows[i]["end_hour"].ToString() + ":";

                                txt += ou2.Table.Rows[i]["end_minute"].ToString();

                            laa.Text = txt;
                            date_click.Controls.Add(laa);
                        }
                        e.Cell.Controls.Add(date_click);
                    }
                    else if (no_check_day)
                    {
                        e.Cell.Controls.Add(la);
                    }
                    else
                    {
                        Label laa = new Label();
                        string txt = dow.shour + ":" + dow.smin;
                        txt += @"</br>|</br>" + dow.ehour + ":" + dow.emin;
                        laa.Text = txt;
                        date_click.Controls.Add(laa);

                        e.Cell.Controls.Add(date_click);
                    }

                }
                else if (check_day)
                {
                    SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    SqlDataSource1.SelectCommand = "select * from appointment where uid='" + sup_id + "' and year='" + e.Day.Date.Year + "'";
                    SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "' and day='" + e.Day.Date.Day + "';";
                    SqlDataSource1.DataBind();
                    DataView ou2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                    for (int i = 0; i < ou2.Count; i++)
                    {
                        Label laa = new Label();
                        string txt = ou2.Table.Rows[i]["start_hour"].ToString() + ":";

                            txt += ou2.Table.Rows[i]["start_minute"].ToString();

                        txt += @"</br>|</br>" + ou2.Table.Rows[i]["end_hour"].ToString() + ":";

                            txt += ou2.Table.Rows[i]["end_minute"].ToString();

                        laa.Text = txt;
                        date_click.Controls.Add(laa);
                    }
                    e.Cell.Controls.Add(date_click);
                }
                else
                {
                    e.Cell.Controls.Add(la);
                }




                e.Cell.Controls.Add(table_down);
                //e.Cell.BackColor = System.Drawing.Color.Gray;

            }

            SelectedDatesCollection theDates = Calendar2.SelectedDates;


            //Button time_click = new Button();
            //time_click.Width = 50; time_click.Height = 50;
            //time_click.ID = "";


            //if (e.Day.IsWeekend )
            //{
            //    e.Cell.Controls.Clear();

            //}
        }
    }
    [WebMethod]
    public static string search_time(string param1, string param2, string param3, string param4)
    {
        string result = "";
        int year =Convert.ToInt32( param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim());
        int month =Convert.ToInt32( param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim());
        int day = Convert.ToInt32(param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim());
        DateTime date_select = new DateTime(year,month,day);
        string sup_id = param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        if (sup_id == "")
        {

        }
        else
        {
            try
            {
                bool check_date = true;
                SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select start_hour,end_hour from appointment";
                sql_f.SelectCommand += " where checked=1 and uid='" + sup_id + "' and year='"+year+"' and month='"+month+"' and day='"+day+"';";
                sql_f.DataBind();
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count > 0)
                {
                    check_date = false;

                    result = ict_f.Table.Rows[0]["start_hour"].ToString() + "." + ict_f.Table.Rows[0]["end_hour"].ToString();
                }
                int whichday=0;
                if(date_select.DayOfWeek==DayOfWeek.Monday)
                {
                    whichday = 1;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Tuesday)
                {
                    whichday = 2;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Wednesday)
                {
                    whichday = 3;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Thursday)
                {
                    whichday = 4;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Friday)
                {
                    whichday = 5;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Saturday)
                {
                    whichday = 6;
                }
                else if (date_select.DayOfWeek == DayOfWeek.Sunday)
                {
                    whichday = 7;
                }
                if (check_date)
                {
                    sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select b.start_hour,b.end_hour from user_information_store as a inner join user_information_store_week_appointment as b on b.uisid=a.id";
                    sql_f.SelectCommand += " where b.checked=1 and a.uid='" + sup_id + "' and b.week_of_day='" + whichday + "';";
                    sql_f.DataBind();
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                    if (ict_f.Count > 0)
                    {
                        result = ict_f.Table.Rows[0]["start_hour"].ToString() + "." + ict_f.Table.Rows[0]["end_hour"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
                //return result;
                throw ex;
            }
        }
        return result;
    }
    [WebMethod]
    public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14)
    {
        string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8 + "," + param9 + "," + param10 + "," + param11 + "," + param12 + "," + param13+","+param14;
        result = "";

        if (param1 != "" && param2 != "" && param14!="")
        {
            string kid_str = param13;

            // Instantiate the regular expression object.
            Regex r = new Regex(@",");

            // Match the regular expression pattern against a text string.
            Match m = r.Match(kid_str);
            List<int> indexlist = new List<int>();
            while (m.Success)
            {
                indexlist.Add(m.Index);
                m = m.NextMatch();
            }
            List<string> kidlist = new List<string>();
            for (int i = 0; i < indexlist.Count; i++)
            {
                string kid = "";
                if (i - 1 < 0)
                {
                    kid = kid_str.Substring(0, indexlist[i]);
                    kidlist.Add(kid);
                }
                else
                {
                    kid = kid_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                    kidlist.Add(kid);
                }
            }
            string request_kid = "";
            for (int i = 0; i < kidlist.Count; i++)
            {
                SqlDataSource sql_f_user = new SqlDataSource();
                sql_f_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_user.SelectCommand = "select id,Sex,birthday_year,birthday_month,birthday_day";
                sql_f_user.SelectCommand += " from user_information_school_children";
                sql_f_user.SelectCommand += " where id='" + kidlist[i] + "';";
                sql_f_user.DataBind();
                DataView ict_f_user = (DataView)sql_f_user.Select(DataSourceSelectArguments.Empty);
                if (ict_f_user.Count > 0)
                {
                    string kidstr = "";
                    for (int ii = 0; ii < ict_f_user.Count; ii++)
                    {
                        //if (ict_f_user.Table.Rows[ii]["Sex"].ToString() == "0")
                        //{
                        //    kidstr += "女の子     ";
                        //}
                        //else
                        //{
                        //    kidstr += "男の子     ";
                        //}
                        int byear = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_year"].ToString());
                        int bmon = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_month"].ToString());
                        int bday = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_day"].ToString());

                        int toyear = DateTime.Now.Year;
                        int tomon = DateTime.Now.Month;

                        int comyear = toyear - byear;
                        int common = tomon - bmon;

                        if (common < 0)
                        {
                            comyear -= 1;
                            common += 12;
                        }
                        kidstr += comyear + " 歳 " + common + " ヶ月 ( 1 名)";
                        request_kid += kidstr + "、";
                    }
                }
            }
            request_kid = request_kid.Substring(0, request_kid.Length-1);



            int cut = param3.IndexOf('.');
            string year = param3.Substring(0, cut);
            string sub = param3.Substring(cut + 1, param3.Length - cut - 1);
            cut = sub.IndexOf('.');
            string month = sub.Substring(0, cut);
            string day = sub.Substring(cut + 1, sub.Length - cut - 1);


            string week="";

            DateTime indate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            int whichdy = 0;
            if (indate.DayOfWeek == DayOfWeek.Monday) { whichdy = 1;week="月"; }
            else if (indate.DayOfWeek == DayOfWeek.Tuesday) { whichdy = 2;week="火"; }
            else if (indate.DayOfWeek == DayOfWeek.Wednesday) { whichdy = 3;week="水"; }
            else if (indate.DayOfWeek == DayOfWeek.Thursday) { whichdy = 4;week="木"; }
            else if (indate.DayOfWeek == DayOfWeek.Friday) { whichdy = 5;week="金"; }
            else if (indate.DayOfWeek == DayOfWeek.Saturday) { whichdy = 6;week="土"; }
            else if (indate.DayOfWeek == DayOfWeek.Sunday) { whichdy = 7;week="日"; }

            string which = "単発", wdate = year + "年" + month + "月" + day + "日(" + week + ") " + param4 + ":" + param5 + "~" + param6 + ":" + param7;



            string request = "【依頼願い】";
            request +="<ul>";
            request += "<li>単発/定期&nbsp;&nbsp;" + which + "</li>";
                 request += "<li>日時&nbsp;&nbsp;" + wdate + "</li>";
                 request += "<li>依頼内容&nbsp;&nbsp;" + param14 + "</li>";
                 request += "<li>お願いするお子様&nbsp;&nbsp;" + request_kid + "</li>";
                 request +="<li>現在のステータス&nbsp;&nbsp;確認待ち</li>";



            //now time
                 string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
                 string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
                 string startm = DateTime.Now.Minute.ToString();
                 string starts = DateTime.Now.Second.ToString();
                 string start = startd + " " + starth + ":" + startm + ":" + starts;
            //now time
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id,start_hour,end_hour from appointment";
            sql_f.SelectCommand += " where checked=1 and uid='" + param2 + "' and year='" + year + "' and month='" + month + "' and day='" + day + "';";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            SqlDataSource sql_insert = new SqlDataSource();
            SqlDataSource sql_f_user_f = new SqlDataSource();
            DataView ict_f_user_f;
            if (ict_f.Count > 0)
            {
                string check_case = "";
                for (int i = 0; i < kidlist.Count; i++)
                {
                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_appointment(appid,uid,start_hour,start_minute,end_hour,end_minute,money_hour,hour,total_money,commission,notice_content,uisccid,howtoget_there,insert_time)";
                    sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + param1 + "','" + param4 + "','" + param5 + "','" + param6 + "','" + param7 + "','" + param10 + "','" + param9 + "','" + param12 + "','" + param11 + "'";
                    sql_insert.InsertCommand += ",'" + param8 + "','" + kidlist[i] + "','" + param14 + "','" + start + "')";
                    sql_insert.Insert();

                    sql_f_user_f = new SqlDataSource();
                    sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f_user_f.SelectCommand = "select id";
                    sql_f_user_f.SelectCommand += " from user_appointment";
                    sql_f_user_f.SelectCommand += " where appid='" + ict_f.Table.Rows[0]["id"].ToString() + "' and uid='" + param1 + "' and uisccid='" + kidlist[i] + "' and insert_time='" + start + "';";
                    sql_f_user_f.DataBind();
                    ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                    if (ict_f_user_f.Count > 0)
                    {
                        check_case += ict_f_user_f.Table.Rows[0]["id"].ToString() + ",";
                    }
                }


                HttpContext.Current.Session["check_case"] = check_case;


                //now time
                startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
                starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
                startm = DateTime.Now.Minute.ToString();
                starts = DateTime.Now.Second.ToString();
                start = startd + " " + starth + ":" + startm + ":" + starts;
                //now time

                //insert deal case
                sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_information_appointment_check_deal(uid,suppid,type,check_success,first_check_time)";
                sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','0','0','" + start + "')";
                sql_insert.Insert();

                sql_f_user_f = new SqlDataSource();
                sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_user_f.SelectCommand = "select id";
                sql_f_user_f.SelectCommand += " from user_information_appointment_check_deal";
                sql_f_user_f.SelectCommand += " where uid='" + param1 + "' and suppid='" + param2 + "' and first_check_time='" + start + "';";
                sql_f_user_f.DataBind();
                ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                string dealid = "";
                if (ict_f_user_f.Count > 0)
                {
                    dealid = ict_f_user_f.Table.Rows[0]["id"].ToString();
                }
                string caseid = HttpContext.Current.Session["check_case"].ToString();
                r = new Regex(@",");

                // Match the regular expression pattern against a text string.
                m = r.Match(caseid);
                List<int> indexlist_1 = new List<int>();
                while (m.Success)
                {
                    indexlist_1.Add(m.Index);
                    m = m.NextMatch();
                }
                List<string> caseidlist = new List<string>();
                for (int i = 0; i < indexlist_1.Count; i++)
                {
                    string ca = "";
                    if (i - 1 < 0)
                    {
                        ca = caseid.Substring(0, indexlist_1[i]);
                        caseidlist.Add(ca);
                    }
                    else
                    {
                        ca = caseid.Substring(indexlist_1[i - 1] + 1, indexlist_1[i] - indexlist_1[i - 1] - 1);
                        caseidlist.Add(ca);
                    }
                }
                for (int i = 0; i < caseidlist.Count; i++)
                {
                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_information_appointment_check_connect_deal(uiacdid,uaid)";
                    sql_insert.InsertCommand += " values('" + dealid + "','" + caseidlist[i] + "')";
                    sql_insert.Insert();
                }




                request += "<li>確認用URL&nbsp;&nbsp;<a href='user_date_manger.aspx'>確認用URL</a></li>";
                //request += "<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL(param1, param2, dealid) + ">確認用URL</a></li>";
                request += "</ul>";

                sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
                sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','" + request + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
                sql_insert.Insert();
                HttpContext.Current.Session["check_case"] = "";

                result = "Success!";
            }
            else
            {
                SqlDataSource sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select a.id from user_information_store_week_appointment as a inner join user_information_store as b on b.id=a.uisid";
                sql_f1.SelectCommand += " where checked=1 and b.uid='" + param2 + "' and week_of_day='" + whichdy + "';";
                sql_f1.DataBind();
                DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    string check_case = "";
                    for (int i = 0; i < kidlist.Count; i++)
                    {
                        sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_week_appointment_check(uiswaid,uid,start_hour,start_minute,end_hour,end_minute,money_hour,hour,total_money,commission,notice_content,uisccid,start_date,end_date,howtoget_there,insert_time)";
                        sql_insert.InsertCommand += " values('" + ict_f1.Table.Rows[0]["id"].ToString() + "','" + param1 + "','" + param4 + "','" + param5 + "','" + param6 + "','" + param7 + "','" + param10 + "','" + param9 + "','" + param12 + "','" + param11 + "'";
                        sql_insert.InsertCommand += ",'" + param8 + "','" + kidlist[i] + "','" + indate.ToString("yyyy-MM-dd") + "','" + indate.ToString("yyyy-MM-dd") + "','" + param14 + "','"+start+"')";
                        sql_insert.Insert();

                        sql_f_user_f = new SqlDataSource();
                        sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_f_user_f.SelectCommand = "select id";
                        sql_f_user_f.SelectCommand += " from user_information_store_week_appointment_check";
                        sql_f_user_f.SelectCommand += " where uiswaid='" + ict_f1.Table.Rows[0]["id"].ToString() + "' and uid='" + param1 + "' and uisccid='" + kidlist[i] + "' and insert_time='" + start + "';";
                        sql_f_user_f.DataBind();
                        ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                        if (ict_f_user_f.Count > 0)
                        {
                            check_case += ict_f_user_f.Table.Rows[0]["id"].ToString() + ",";
                        }
                    }


                    HttpContext.Current.Session["check_case"] = check_case;


                    //now time
                    startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
                    startm = DateTime.Now.Minute.ToString();
                    starts = DateTime.Now.Second.ToString();
                    start = startd + " " + starth + ":" + startm + ":" + starts;
                    //now time

                    //insert deal case
                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_information_appointment_check_deal(uid,suppid,type,check_success,first_check_time)";
                    sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','1','0','" + start + "')";
                    sql_insert.Insert();

                    sql_f_user_f = new SqlDataSource();
                    sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f_user_f.SelectCommand = "select id";
                    sql_f_user_f.SelectCommand += " from user_information_appointment_check_deal";
                    sql_f_user_f.SelectCommand += " where uid='" + param1 + "' and suppid='" + param2 + "' and first_check_time='" + start + "';";
                    sql_f_user_f.DataBind();
                    ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                    string dealid = "";
                    if (ict_f_user_f.Count > 0)
                    {
                        dealid = ict_f_user_f.Table.Rows[0]["id"].ToString();
                    }
                    string caseid = HttpContext.Current.Session["check_case"].ToString();
                    r = new Regex(@",");

                    // Match the regular expression pattern against a text string.
                    m = r.Match(caseid);
                    List<int> indexlist_1 = new List<int>();
                    while (m.Success)
                    {
                        indexlist_1.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> caseidlist = new List<string>();
                    for (int i = 0; i < indexlist_1.Count; i++)
                    {
                        string ca = "";
                        if (i - 1 < 0)
                        {
                            ca = caseid.Substring(0, indexlist_1[i]);
                            caseidlist.Add(ca);
                        }
                        else
                        {
                            ca = caseid.Substring(indexlist_1[i - 1] + 1, indexlist_1[i] - indexlist_1[i - 1] - 1);
                            caseidlist.Add(ca);
                        }
                    }
                    for (int i = 0; i < caseidlist.Count; i++)
                    {
                        sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_appointment_check_connect_deal(uiacdid,uiswacid)";
                        sql_insert.InsertCommand += " values('" + dealid + "','" + caseidlist[i] + "')";
                        sql_insert.Insert();
                    }





                    request += "<li>確認用URL&nbsp;&nbsp;<a href='user_date_manger.aspx'>確認用URL</a></li>";
                    //request += "<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week1(param1, param2, dealid) + ">確認用URL</a></li>";
                    request += "</ul>";

                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
                    sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','" + request + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
                    sql_insert.Insert();

                    result = "Success!";
                }
            }
        }





        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string Save_long(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10, string param11, string param12, string param13, string param14, string param15, string param16,string param17,string param18)
    {
        string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8 + "," + param9 + "," + param10 + "," + param11 + "," + param12 + "," + param13 + "," + param14 + "," + param15 + "," + param16;
        result = "";

        if (param1 != "" && param2 != "" && param14 != "" && param15 != "" && param16!="")
        {
            string kid_str = param13;

            // Instantiate the regular expression object.
            Regex r = new Regex(@",");

            // Match the regular expression pattern against a text string.
            Match m = r.Match(kid_str);
            List<int> indexlist = new List<int>();
            while (m.Success)
            {
                indexlist.Add(m.Index);
                m = m.NextMatch();
            }
            List<string> kidlist = new List<string>();
            for (int i = 0; i < indexlist.Count; i++)
            {
                string kid = "";
                if (i - 1 < 0)
                {
                    kid = kid_str.Substring(0, indexlist[i]);
                    kidlist.Add(kid);
                }
                else
                {
                    kid = kid_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                    kidlist.Add(kid);
                }
            }
            string request_kid = "";
            for (int i = 0; i < kidlist.Count; i++)
            {
                SqlDataSource sql_f_user = new SqlDataSource();
                sql_f_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_user.SelectCommand = "select id,Sex,birthday_year,birthday_month,birthday_day";
                sql_f_user.SelectCommand += " from user_information_school_children";
                sql_f_user.SelectCommand += " where id='" + kidlist[i] + "';";
                sql_f_user.DataBind();
                DataView ict_f_user = (DataView)sql_f_user.Select(DataSourceSelectArguments.Empty);
                if (ict_f_user.Count > 0)
                {
                    string kidstr = "";
                    for (int ii = 0; ii < ict_f_user.Count; ii++)
                    {
                        //if (ict_f_user.Table.Rows[ii]["Sex"].ToString() == "0")
                        //{
                        //    kidstr += "女の子     ";
                        //}
                        //else
                        //{
                        //    kidstr += "男の子     ";
                        //}
                        int byear = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_year"].ToString());
                        int bmon = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_month"].ToString());
                        int bday = Convert.ToInt32(ict_f_user.Table.Rows[ii]["birthday_day"].ToString());

                        int toyear = DateTime.Now.Year;
                        int tomon = DateTime.Now.Month;

                        int comyear = toyear - byear;
                        int common = tomon - bmon;

                        if (common < 0)
                        {
                            comyear -= 1;
                            common += 12;
                        }
                        kidstr += comyear + " 歳 " + common + " ヶ月 ( 1 名)";
                        request_kid += kidstr + "、";
                    }
                }
            }
            request_kid = request_kid.Substring(0, request_kid.Length - 1);

            string wdate = "";
            SqlDataSource sql_f_user_f = new SqlDataSource();
            sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_user_f.SelectCommand = "select week_of_day_jp";
            sql_f_user_f.SelectCommand += " from user_information_store_week_appointment";
            sql_f_user_f.SelectCommand += " where id='" + param3 + "';";
            sql_f_user_f.DataBind();
            DataView ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f_user_f.Count > 0)
            {
                wdate = ict_f_user_f.Table.Rows[0]["week_of_day_jp"].ToString();
            }
            wdate += "曜日&nbsp"+param4 + ":" + param5 + "~" + param6 + ":" + param7;

            string which = "定期";
            string request = "";

            //now time
            string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
            string startm = DateTime.Now.Minute.ToString();
            string starts = DateTime.Now.Second.ToString();
            string start = startd + " " + starth + ":" + startm + ":" + starts;
            //now time

            string check_case = "";

            SqlDataSource sql_insert = new SqlDataSource();
            for (int i = 0; i < kidlist.Count; i++)
            {
                sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_information_store_week_appointment_check(uiswaid,uid,start_hour,start_minute,end_hour,end_minute,money_hour,hour,total_money,commission,notice_content,uisccid,start_date,end_date,howtoget_there,insert_time)";
                sql_insert.InsertCommand += " values('" + param3 + "','" + param1 + "','" + param4 + "','" + param5 + "','" + param6 + "','" + param7 + "','" + param10 + "','" + param9 + "','" + param12 + "','" + param11 + "'";
                sql_insert.InsertCommand += ",'" + param8 + "','" + kidlist[i] + "','" + param15 + "','" + param16 + "','" + param14 + "','"+start+"')";
                sql_insert.Insert();

                sql_f_user_f = new SqlDataSource();
                sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_user_f.SelectCommand = "select id";
                sql_f_user_f.SelectCommand += " from user_information_store_week_appointment_check";
                sql_f_user_f.SelectCommand += " where uiswaid='" + param3 + "' and uid='" + param1 + "' and uisccid='" + kidlist[i] + "' and insert_time='" + start + "';";
                sql_f_user_f.DataBind();
                ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f_user_f.Count > 0)
                {
                    check_case += ict_f_user_f.Table.Rows[0]["id"].ToString()+",";
                }
            }

            HttpContext.Current.Session["check_case"] += check_case;


            if (param17 == "0")
            {
                request = "【依頼願い】";
                request += "<ul>";
                request += "<li>単発/定期&nbsp;&nbsp;" + which + "</li>";
                request += "<li>定期&nbsp;&nbsp;" + param15 + "&nbsp;-&nbsp;" + param16 + "</li>";
                request += "<li>曜日&nbsp;&nbsp;" + wdate + "</li>";
                //HttpContext.Current.Session["key_f"] = SendActivationURL_week(param1, param2, param3);
                //SendActivationURL_week_in(param1,param3 , HttpContext.Current.Session["key_f"].ToString());
                HttpContext.Current.Session["req"] += request;
            }
            else if (param17 != param18)
            {
                request = "<li>曜日&nbsp;&nbsp;" + wdate + "</li>";
                //SendActivationURL_week_in(param1, param3, HttpContext.Current.Session["key_f"].ToString());
                HttpContext.Current.Session["req"] += request;
            }
            else if (param17 == param18)
            {



                //now time
                startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
                starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
                startm = DateTime.Now.Minute.ToString();
                starts = DateTime.Now.Second.ToString();
                start = startd + " " + starth + ":" + startm + ":" + starts;
                //now time

                //insert deal case
                sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_information_appointment_check_deal(uid,suppid,type,check_success,first_check_time)";
                sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','2','0','" + start + "')";
                sql_insert.Insert();

                sql_f_user_f = new SqlDataSource();
                sql_f_user_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_user_f.SelectCommand = "select id";
                sql_f_user_f.SelectCommand += " from user_information_appointment_check_deal";
                sql_f_user_f.SelectCommand += " where uid='" + param1 + "' and suppid='" + param2 + "' and first_check_time='" + start + "';";
                sql_f_user_f.DataBind();
                ict_f_user_f = (DataView)sql_f_user_f.Select(DataSourceSelectArguments.Empty);
                string dealid = "";
                if (ict_f_user_f.Count > 0)
                {
                    dealid = ict_f_user_f.Table.Rows[0]["id"].ToString();
                }
                string caseid = HttpContext.Current.Session["check_case"].ToString();
                r = new Regex(@",");

                // Match the regular expression pattern against a text string.
                m = r.Match(caseid);
                List<int> indexlist_1 = new List<int>();
                while (m.Success)
                {
                    indexlist_1.Add(m.Index);
                    m = m.NextMatch();
                }
                List<string> caseidlist = new List<string>();
                for (int i = 0; i < indexlist_1.Count; i++)
                {
                    string ca = "";
                    if (i - 1 < 0)
                    {
                        ca = caseid.Substring(0, indexlist_1[i]);
                        caseidlist.Add(ca);
                    }
                    else
                    {
                        ca = caseid.Substring(indexlist_1[i - 1] + 1, indexlist_1[i] - indexlist_1[i - 1] - 1);
                        caseidlist.Add(ca);
                    }
                }
                for (int i = 0; i < caseidlist.Count; i++)
                {
                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_information_appointment_check_connect_deal(uiacdid,uiswacid)";
                    sql_insert.InsertCommand += " values('" + dealid + "','" + caseidlist[i] + "')";
                    sql_insert.Insert();
                }


                request = "<li>曜日&nbsp;&nbsp;" + wdate + "</li>";
                request += "<li>依頼内容&nbsp;&nbsp;" + param14 + "</li>";
                request += "<li>お願いするお子様&nbsp;&nbsp;" + request_kid + "</li>";
                request += "<li>現在のステータス&nbsp;&nbsp;確認待ち</li>";
                request += "<li>確認用URL&nbsp;&nbsp;<a href='user_date_manger.aspx'>確認用URL</a></li>";
                //request += "<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week(param1, param2, dealid) + ">確認用URL</a></li>";
                request += "</ul>";
                HttpContext.Current.Session["req"] += request;


                sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
                sql_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','" + HttpContext.Current.Session["req"].ToString() + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
                sql_insert.Insert();
                HttpContext.Current.Session["req"] = "";
                //HttpContext.Current.Session["key_f"] = "";
                HttpContext.Current.Session["check_case"] = "";
            }

            result = request;

        }




        return result;
    }
    public static string SendActivationURL(string userId, string suppid, string uiacdid)
    {
        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO user_appointment_check VALUES(@uid,@supp_id,@uiacdid, @ActivationCode)"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@supp_id", suppid);
                    cmd.Parameters.AddWithValue("@uiacdid", uiacdid);
                    cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        string res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("Date_Calendar.aspx/Save", "Activation_check.aspx?ActivationCode=" + activationCode);
        return res;
    }

    public static string SendActivationURL_week1(string userId, string suppid, string uiacdid)
    {
        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO user_information_store_week_appointment_check_check VALUES(@uid,@supp_id,@uiacdid, @ActivationCode)"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@supp_id", suppid);
                    cmd.Parameters.AddWithValue("@uiacdid", uiacdid);
                    cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        string res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("Date_Calendar.aspx/Save", "Activation_check_week.aspx?ActivationCode=" + activationCode);
        return res;
    }

    public static string SendActivationURL_week(string userId, string suppid, string uiacdid)
    {
        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO user_information_store_week_appointment_check_check VALUES(@uid,@supp_id,@uiacdid, @ActivationCode)"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@supp_id", suppid);
                    cmd.Parameters.AddWithValue("@uiacdid", uiacdid);
                    cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        string res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("Date_Calendar.aspx/Save_long", "Activation_check_week.aspx?ActivationCode=" + activationCode);
        return res;
    }
    protected void sent_Button_Click(object sender, EventArgs e)
    {

    }
    protected void change_page(object sender, EventArgs e)
    {
        Button temp = (Button)sender;
        string cutstr2 = temp.ID;
        int ind2 = cutstr2.IndexOf(@"_");
        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);

        Panel pan_vid1 = (Panel)this.FindControl("supporter_good_panel");
        pan_vid1.Controls.Clear();

        if (Session["id"] != null)
        {


            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<hr/>"));

            int totalcount = 0;

            totalcount = video_list.Count;

            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("</tr>"));
            //each video
            int endva = 0;
            if (video_list.Count - (10 * Convert.ToInt32(cutstr3)) < (10 * (Convert.ToInt32(cutstr3) + 1)) - (10 * Convert.ToInt32(cutstr3)))
            {
                endva = video_list.Count;
            }
            else
            {
                endva = 10 * (Convert.ToInt32(cutstr3) + 1);
            }
            for (int i = 10 * (Convert.ToInt32(cutstr3)); i < endva; i++)
            {
                pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                pan_vid1.Controls.Add(new LiteralControl("<td>"));
                pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
                pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                //Poster photo
                pan_vid1.Controls.Add(new LiteralControl("<td width='10%' rowspan='3' valign='top'>"));
                pan_vid1.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                cutstr2 = video_list[i].photo;
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                pan_vid1.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + video_list[i].username + "' style='width:50px;height:50px;'>"));
                pan_vid1.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                pan_vid1.Controls.Add(new LiteralControl("</a>"));
                pan_vid1.Controls.Add(new LiteralControl("</div>"));
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                //poster username
                pan_vid1.Controls.Add(new LiteralControl("<td width='10%'>"));
                HyperLink hy = new HyperLink();
                hy.NavigateUrl = "~/user_home_friend.aspx?=" + video_list[i].uid;

                hy.Target = "_blank";
                hy.Text = video_list[i].username;
                hy.Font.Underline = false;
                pan_vid1.Controls.Add(hy);
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                //poster message type and time
                pan_vid1.Controls.Add(new LiteralControl("<td align='right' width='80%'>"));
                DateTime post_date = Convert.ToDateTime(video_list[i].check_time);
                Label la = new Label();
                la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                la.Text = "";
                la.Text += post_date.Year + "." + post_date.ToString("MM") + "." + post_date.ToString("dd");
                pan_vid1.Controls.Add(la);
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                pan_vid1.Controls.Add(new LiteralControl("<td colspan='2'>"));
                Literal li = new Literal();
                li.Text = @"<script>
$(function () {
$('.hidde" + video_list[i].good_id + @"').toggle(false);

            $('.box" + video_list[i].good_id + @"').click(function () {
                $('.hidde" + video_list[i].good_id + @"').toggle();
                $('.box" + video_list[i].good_id + @"').toggle(false);
            })

            $('.hidde" + video_list[i].good_id + @"').click(function () {
                $('.box" + video_list[i].good_id + @"').toggle();
                $('.hidde" + video_list[i].good_id + @"').toggle(false);
            })
";
                li.Text += @"
                        });";
                li.Text += @"</script>";
                pan_vid1.Controls.Add(li);

                pan_vid1.Controls.Add(new LiteralControl("<div class='box" + video_list[i].good_id + "'>"));
                HyperLink hyy1;
                if (video_list[i].score_message.Length < 37)
                {
                    pan_vid1.Controls.Add(new LiteralControl(video_list[i].score_message));
                }
                else
                {
                    pan_vid1.Controls.Add(new LiteralControl(video_list[i].score_message.Substring(0, 37) + "‧‧‧"));
                    hyy1 = new HyperLink();
                    hyy1.NavigateUrl = "javascript:void(0);";
                    hyy1.Target = "_blank";
                    hyy1.Text = "詳細を見る";
                    hyy1.Font.Underline = false;
                    pan_vid1.Controls.Add(hyy1);
                }


                pan_vid1.Controls.Add(new LiteralControl("</div>"));
                pan_vid1.Controls.Add(new LiteralControl("<div class='hidde" + video_list[i].good_id + "'>"));

                Label la1 = new Label();
                la1.Style.Add("word-break", "break-all");
                la1.Style.Add("over-flow", "hidden");
                la1.Text = video_list[i].score_message;
                pan_vid1.Controls.Add(la1);
                pan_vid1.Controls.Add(new LiteralControl("<br/>"));


                if (video_list[i].score_message.ToString().Length > 36)
                {
                    hyy1 = new HyperLink();
                    hyy1.NavigateUrl = "javascript:void(0);";
                    hyy1.Target = "_blank";
                    hyy1.Text = "たたむ";
                    hyy1.Font.Underline = false;
                    pan_vid1.Controls.Add(hyy1);
                }


                pan_vid1.Controls.Add(new LiteralControl("</div>"));

                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                pan_vid1.Controls.Add(new LiteralControl("<tr>"));
                pan_vid1.Controls.Add(new LiteralControl("<td colspan='2' align='right'>"));
                //who like who answer post message
                SqlDataSource sql_who_like = new SqlDataSource();
                sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_who_like.SelectCommand = "select count(*) as howmany from user_information_appointment_check_deal_score_like";
                sql_who_like.SelectCommand += " where uiacdsid='" + video_list[i].good_id + "' and good_status='1';";
                //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                sql_who_like.DataBind();
                DataView ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                if (ict_who_like.Count > 0)
                {
                    Image img1 = new Image();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like_b_1.png";
                    pan_vid1.Controls.Add(img1);
                    hyy1 = new HyperLink();
                    hyy1.ID = "likecount" + video_list[i].good_id;
                    hyy1.NavigateUrl = "javascript:void(0);";
                    hyy1.Target = "_blank";
                    hyy1.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                    hyy1.Font.Underline = false;
                    pan_vid1.Controls.Add(hyy1);
                }
                pan_vid1.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                hyy1 = new HyperLink();
                hyy1.ID = "whouse_" + video_list[i].good_id;
                hyy1.NavigateUrl = "javascript:void(0);";
                hyy1.Target = "_blank";
                hyy1.Text = "役に立った!";
                hyy1.Font.Underline = false;

                sql_who_like = new SqlDataSource();
                sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_who_like.SelectCommand = "select good_status from user_information_appointment_check_deal_score_like";
                sql_who_like.SelectCommand += " where uiacdsid='" + video_list[i].good_id + "' and uid='" + Session["id"].ToString() + "';";
                sql_who_like.DataBind();
                ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                if (ict_who_like.Count > 0)
                {
                    if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                    {
                        hyy1.Style.Add("color", "#4183C4");
                        hyy1.Attributes["onclick"] = "buseful_who_answer(this.id)";
                    }
                    else
                    {
                        hyy1.Style.Add("color", "#D84C4B");
                        hyy1.Attributes["onclick"] = "useful_who_answer(this.id)";
                    }
                }
                else
                {
                    hyy1.Style.Add("color", "#4183C4");
                    hyy1.Attributes["onclick"] = "buseful_who_answer(this.id)";
                }
                pan_vid1.Controls.Add(hyy1);


                //pan_vid1.Controls.Add(new LiteralControl("<input id='likepost_" + ict_f_u.Table.Rows[i]["id"].ToString() + @"' type='button' value='LIKE' onclick='like(this.id)' class='file-upload'/>"));

                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("</tr>"));
                pan_vid1.Controls.Add(new LiteralControl("</table>"));
                pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("</tr>"));
            }
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td align='center'>"));

            Panel pagepan = new Panel();
            pagepan.ScrollBars = ScrollBars.Auto;
            pagepan.Width = Unit.Percentage(100);
            pagepan.Height = 100;
            Button but_page = new Button();
            int page_count = video_list.Count / 10;
            for (int i = 0; i < page_count + 1; i++)
            {
                but_page = new Button();
                but_page.ID = "page_" + i;
                but_page.Click += new System.EventHandler(this.change_page);
                but_page.Text = "" + (i + 1);
                but_page.BorderStyle = BorderStyle.None;
                but_page.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                but_page.OnClientClick = "ShowProgressBar();";
                but_page.Style["cursor"] = "pointer";
                pagepan.Controls.Add(but_page);
                pagepan.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            pan_vid1.Controls.Add(pagepan);

            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("</tr>"));





            pan_vid1.Controls.Add(new LiteralControl("</table>"));
        }


    }
    [WebMethod]
    public static string like_who_ans(string param1, string param2, string param3)
    {
        string result = param3;

        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;

        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select id from user_information_appointment_check_deal_score_like";
        sql_f.SelectCommand += " where uid='" + param1 + "' and uiacdsid='" + param2 + "';";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {

            SqlDataSource sql_f_update = new SqlDataSource();
            sql_f_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_update.UpdateCommand = "update user_information_appointment_check_deal_score_like set good_status='" + param3 + "'";
            sql_f_update.UpdateCommand += ",check_date='" + start + "'";
            sql_f_update.UpdateCommand += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
            sql_f_update.Update();

        }
        else
        {
            SqlDataSource sql_f_insert = new SqlDataSource();
            sql_f_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_insert.InsertCommand = "insert into user_information_appointment_check_deal_score_like(uid,uiacdsid,good_status,check_date)";
            sql_f_insert.InsertCommand += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + start + "');";
            sql_f_insert.Insert();
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_list(string param1)
    {
        string result = param1;
        result = "";
        SqlDataSource sql_h_fri_notice = new SqlDataSource();
        sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_fri_notice.SelectCommand = "select a.id,a.first_uid,b.username,b.photo,a.first_date_year,a.first_date_month,a.first_date_day,a.first_date_hour,a.first_date_minute,a.first_date_second ";
        sql_h_fri_notice.SelectCommand += "from user_friendship as a inner join user_login as b on a.first_uid=b.id where a.second_uid='" + param1 + "' and a.second_check_connect='0'";
        sql_h_fri_notice.SelectCommand += " ORDER BY a.first_date_year desc,a.first_date_month desc,a.first_date_day desc,a.first_date_hour desc,a.first_date_minute desc,a.first_date_second desc;";
        sql_h_fri_notice.DataBind();
        DataView ict_h_fri_notice = (DataView)sql_h_fri_notice.Select(DataSourceSelectArguments.Empty);
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
<td>

<input id='friendcheck_" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' type='button' value='友達承認' onclick='dlgcheckfriend(this.id)' class='file-upload'/>

</td>
</tr>
</table><hr/>";
            }
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_check(string param1)
    {
        string result = param1;
        result = "";
        SqlDataSource sql_h_fri_notice = new SqlDataSource();
        sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_fri_notice.UpdateCommand = "update user_friendship set second_check_connect='1',second_date_year='" + DateTime.Now.Year + "',second_date_month='" + DateTime.Now.Month + "',second_date_day='" + DateTime.Now.Day + "'";
        sql_h_fri_notice.UpdateCommand += ",second_date_hour='" + DateTime.Now.ToString("HH") + "',second_date_minute='" + DateTime.Now.Minute + "',second_date_second='" + DateTime.Now.Second + "' ";
        sql_h_fri_notice.UpdateCommand += "where id='" + param1 + "';";
        sql_h_fri_notice.Update();

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
        string result = param1;
        result = "";

        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        sql_f.SelectCommand += " from user_chat_room as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1 + "'";
        sql_f.SelectCommand += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

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

        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        sql_f1.SelectCommand = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        sql_f1.SelectCommand += " from user_chat_room as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id=" + param1 + "";
        sql_f1.SelectCommand += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
        string result = param1;
        result = "";

        //status message
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select a.id,a.message";
        sql_f.SelectCommand += " from status_messages as a";
        sql_f.SelectCommand += " where a.uid='" + param1 + "'";
        sql_f.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
        for (int i = 0; i < smlist_ind.Count; i++)
        {
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            sql_f.SelectCommand += " from status_messages as a";
            sql_f.SelectCommand += " inner join status_messages_user_like as b on a.id=b.smid";
            sql_f.SelectCommand += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            sql_f.SelectCommand += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            sql_f.SelectCommand += " from status_messages as a";
            sql_f.SelectCommand += " inner join status_messages_user as b on a.id=b.smid";
            sql_f.SelectCommand += " inner join status_messages_user_talk as c on b.id=c.smuid";
            sql_f.SelectCommand += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            sql_f.SelectCommand += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
                    sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    sql_f.SelectCommand += " from status_messages_user_talk as a";
                    sql_f.SelectCommand += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    sql_f.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

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
                    sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    sql_f.SelectCommand += " from status_messages_user_talk_like as a";
                    sql_f.SelectCommand += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    sql_f.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

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
        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        sql_f.SelectCommand += " from status_messages_user_talk as c";
        sql_f.SelectCommand += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        sql_f.SelectCommand += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                sql_f.SelectCommand += " from status_messages_user_talk_like as a";
                sql_f.SelectCommand += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                sql_f.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

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

                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                sql_f.SelectCommand += " from status_messages_user_talk as c";
                sql_f.SelectCommand += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                sql_f.SelectCommand += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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

        for (int i = 0; i < status_mess_like.Count; i++)
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
            if (status_mess_like[i].uid == 0)
            {
                if (status_mess_like[i].like_idlist.Count > 0)
                {
                    sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select username,photo";
                    sql_f.SelectCommand += " from user_login";
                    sql_f.SelectCommand += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select username,photo";
                sql_f.SelectCommand += " from user_login";
                sql_f.SelectCommand += " where id='" + status_mess_like[i].uid + "';";
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
}
