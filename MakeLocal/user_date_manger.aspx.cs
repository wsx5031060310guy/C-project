using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_date_manger : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        changeicon("reservation");
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!IsPostBack)
        {
            //if (Session["aduser"] != null)
            //{
            date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            date_manger_b1.Font.Bold = true;
            date_manger_im1.Visible = true;
            date_manger_im2.Visible = false;
            date_manger_im3.Visible = false;
            date_manger_im4.Visible = false;
            date_manger_im5.Visible = false;
            date_manger_Panel1.Visible = true;
            date_manger_Panel2.Visible = false;
            date_manger_Panel3.Visible = false;
            date_manger_Panel4.Visible = false;
            date_manger_Panel5.Visible = false;
            //}


        }
        string[] dayNames = { "日", "月", "火", "水", "木", "金", "土" };
        CultureInfo culture = new CultureInfo("ja-JP");
        culture.DateTimeFormat.AbbreviatedDayNames = dayNames;
        Thread.CurrentThread.CurrentCulture = culture;
    }
    List<dayofweek> date_week = new List<dayofweek>();
    protected void Page_Init(object sender, EventArgs e)
    {
        string id = Session["id"].ToString();

        ////get user select day of week
        date_week = new List<dayofweek>();

        dayofweek dow = new dayofweek();

        SqlDataSource sql_f_w = new SqlDataSource();
        sql_f_w.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_w.SelectCommand = "select b.checked,b.week_of_day,b.start_hour,b.start_minute,b.end_hour,b.end_minute";
        sql_f_w.SelectCommand += " from user_information_store as a";
        sql_f_w.SelectCommand += " inner join user_information_store_week_appointment as b on a.id=b.uisid";
        sql_f_w.SelectCommand += " where a.uid='" + id + "';";
        sql_f_w.DataBind();
        DataView ict_f_w = (DataView)sql_f_w.Select(DataSourceSelectArguments.Empty);
        for (int i = 0; i < ict_f_w.Count; i++)
        {
            if (ict_f_w.Table.Rows[i]["checked"].ToString() == "1")
            {
                dow = new dayofweek();
                dow.day = Convert.ToInt32(ict_f_w.Table.Rows[i]["week_of_day"].ToString());
                dow.shour = ict_f_w.Table.Rows[i]["start_hour"].ToString();
                dow.smin = ict_f_w.Table.Rows[i]["start_minute"].ToString();
                dow.ehour = ict_f_w.Table.Rows[i]["end_hour"].ToString();
                dow.emin = ict_f_w.Table.Rows[i]["end_minute"].ToString();
                date_week.Add(dow);
            }

        }
        ViewState["day_of_week"] = date_week;
        ////get user select day of week



        SqlDataSource sql_h = new SqlDataSource();
        sql_h.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h.SelectCommand = "select id,username,photo,home_image ";
        sql_h.SelectCommand += "from user_login where id='" + id + "';";
        sql_h.DataBind();
        DataView ict_h = (DataView)sql_h.Select(DataSourceSelectArguments.Empty);

        Panel p_head = (Panel)this.FindControl("date_manger_Panel1");

        string cutstr_h = "";
        int ind_h = 0;
        string cutstr_h1 = "";
        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100px' style='background-color: #EA9494;'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='60%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' height='10%'></td>"));
        //p_head.Controls.Add(new LiteralControl("<td width='35%' rowspan='3'>"));


        //p_head.Controls.Add(new LiteralControl("<table width='100%' height='100%' style='background-color: #EA9494;'>"));
        //p_head.Controls.Add(new LiteralControl("<tr>"));
        //p_head.Controls.Add(new LiteralControl("<td width='20%' align='center'>"));
        ////how many user close today's case
        //p_head.Controls.Add(new LiteralControl("<span style='font-weight:bold; font-size: x-large; color: #FFFFFF;'>"));
        //p_head.Controls.Add(new LiteralControl("直近の予約"));
        //p_head.Controls.Add(new LiteralControl("</span><br/>"));
        //p_head.Controls.Add(new LiteralControl("<span style='font-weight:bold; font-size: x-large; color: #FFFFFF;'>"));
        //p_head.Controls.Add(new LiteralControl("1件"));
        //p_head.Controls.Add(new LiteralControl("</span>"));
        //p_head.Controls.Add(new LiteralControl("</td>"));
        //p_head.Controls.Add(new LiteralControl("</tr>"));
        //p_head.Controls.Add(new LiteralControl("</table>"));

        //p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));




        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='60%'>"));


        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td align='right' width='10%'>"));

        //user photo
        p_head.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
        cutstr_h = ict_h.Table.Rows[0]["photo"].ToString();
        ind_h = cutstr_h.IndexOf(@"/");
        cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
        p_head.Controls.Add(new LiteralControl("<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_h.Table.Rows[0]["username"].ToString() + "' style='width:100px;height:100px;'>"));
        p_head.Controls.Add(new LiteralControl("<img src='" + cutstr_h1 + "' width='100' height='100' />"));
        p_head.Controls.Add(new LiteralControl("</a>"));
        p_head.Controls.Add(new LiteralControl("</div>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%'>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='50%' align='center'>"));
        //user name
        p_head.Controls.Add(new LiteralControl("<span style='font-size: x-large; color: #FFFFFF;'>"));
        p_head.Controls.Add(new LiteralControl("直近の予約"));
        p_head.Controls.Add(new LiteralControl("</span>"));

        //p_head.Controls.Add(new LiteralControl("<span style='font-weight:bold; font-size: x-large;'>"));
        ////p_head.Controls.Add(new LiteralControl(ict_h.Table.Rows[0]["username"].ToString()));
        //p_head.Controls.Add(new LiteralControl("たんぽぽママさん こんにちは。<br/> いつも地域を支えてくださってありがとうございます"));
        //p_head.Controls.Add(new LiteralControl("</span>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%'>"));
        p_head.Controls.Add(new LiteralControl("<span style='font-weight:bold; font-size: xx-large; color: #FFFFFF;'>"));
        SqlDataSource sql_date = new SqlDataSource();
        sql_date.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_date.SelectCommand = "select count(*) as howmany ";
        sql_date.SelectCommand += "from user_information_appointment_check_deal ";
        sql_date.SelectCommand += "where suppid='" + id + "' and check_success='0' and DATEPART(month, first_check_time)='" + DateTime.Now.Month + "' and DATEPART(year, first_check_time)='" + DateTime.Now.Year + "';";
        sql_date.DataBind();
        DataView ict_date = (DataView)sql_date.Select(DataSourceSelectArguments.Empty);
        if (ict_date.Count > 0)
        {
            p_head.Controls.Add(new LiteralControl(ict_date.Table.Rows[0]["howmany"].ToString() + "件"));
        }


        p_head.Controls.Add(new LiteralControl("</span>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));



        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%'></td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='60%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));

        p_head.Controls.Add(new LiteralControl("</table>"));


        //select month
        int thismonth = DateTime.Now.Month;
        int thisyear = DateTime.Now.Year;
        //this_month_HiddenField.Value = thismonth.ToString();
        //this_year_HiddenField.Value = thisyear.ToString();
        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100px' style='border-style: solid; border-width: thin; background-color: #DDDDDD;'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));

        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%' align='center'>"));
        Label laa = new Label();
        laa.ID = "thisyear";
        laa.Text = thisyear.ToString() + "年";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));


        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' align='right'>"));


        ImageButton imgbut = new ImageButton();
        //imgbut.ImageUrl = "~/images/left.png";
        //imgbut.Width = 50;
        //imgbut.Height = 50;
        //imgbut.OnClientClick = "ShowProgressBar();";
        //imgbut.BackColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
        //p_head.Controls.Add(imgbut);

        p_head.Controls.Add(new LiteralControl("<img src='images/left.png' id='before_month_img' alt='before month' style='width:40px;height:40px;cursor:pointer;background-color:#DDDDDD;'>"));


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%'>"));

        //month select

        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100%' align='center' valign='middle'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' align='center' valign='middle'>"));

        laa = new Label();
        laa.ID = "before_month";
        laa.Text = (thismonth - 1) + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);


        //Button date_man_but = new Button();
        //date_man_but.ID = "date_man_but1";
        //date_man_but.Text = (thismonth-1)+"月";
        ////date_man_but.Click += new System.EventHandler(this.change_panel);
        //date_man_but.OnClientClick = "ShowProgressBar();";
        //date_man_but.Style["border-style"] = "none";
        //date_man_but.Style["width"] = "100%";
        //date_man_but.Style["height"] = "100%";
        //date_man_but.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        //date_man_but.Enabled = false;
        //p_head.Controls.Add(date_man_but);


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='40%' align='center' valign='middle'>"));

        laa = new Label();
        laa.ID = "this_month";
        laa.Text = thismonth + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);

        //date_man_but = new Button();
        //date_man_but.ID = "date_man_but2";
        //date_man_but.Text = thismonth+"月";
        ////date_man_but.Click += new System.EventHandler(this.change_panel);
        //date_man_but.OnClientClick = "ShowProgressBar();";
        //date_man_but.Style["border-style"] = "none";
        //date_man_but.Style["width"] = "100%";
        //date_man_but.Style["height"] = "80%";
        //date_man_but.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        //date_man_but.Enabled = false;
        //p_head.Controls.Add(date_man_but);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' align='center' valign='middle'>"));
        // p_head.Controls.Add(new LiteralControl("<input type='text' id='datepicker_boo' placeholder='09/2016' readonly>"));
        //p_head.Controls.Add(new LiteralControl("<input name='datepicker_boo' id='datepicker_boo' />"));


        laa = new Label();
        laa.ID = "after_month";
        laa.Text = (thismonth + 1) + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);


        //date_man_but = new Button();
        //date_man_but.ID = "date_man_but3";
        //date_man_but.Text = (thismonth+1)+"月";
        ////date_man_but.Click += new System.EventHandler(this.change_panel);
        //date_man_but.OnClientClick = "ShowProgressBar();";
        //date_man_but.Style["border-style"] = "none";
        //date_man_but.Style["width"] = "100%";
        //date_man_but.Style["height"] = "100%";
        //date_man_but.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        //date_man_but.Enabled = false;
        //p_head.Controls.Add(date_man_but);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%'>"));

        //imgbut = new ImageButton();
        //imgbut.ImageUrl = "~/images/right.png";
        //imgbut.Width = 50;
        //imgbut.Height = 50;
        //imgbut.OnClientClick = "ShowProgressBar();";
        //imgbut.BackColor = System.Drawing.ColorTranslator.FromHtml("#DDDDDD");
        //p_head.Controls.Add(imgbut);


        p_head.Controls.Add(new LiteralControl("<img src='images/right.png' id='after_month_img' alt='after month' style='width:40px;height:40px;cursor:pointer;background-color:#DDDDDD;'>"));

        p_head.Controls.Add(new LiteralControl("</td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' align='left' valign='top'>"));
        //カレンダー
        p_head.Controls.Add(new LiteralControl("<input type='text' id='datepicker_boo' placeholder='09/2016' style='visibility:hidden;' readonly>"));

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));


        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));


        Panel date_manger_month = new Panel();
        date_manger_month.ID = "date_manger_month";
        p_head.Controls.Add(date_manger_month);


        date_manger_month.Controls.Add(new LiteralControl("<table width='100%' height='100%' style='background-color: #DDDDDD;'>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%'>"));
        Panel date_manger_month_group = new Panel();
        date_manger_month_group.ID = "date_manger_month_group";
        date_manger_month.Controls.Add(date_manger_month_group);
        date_manger_month.Controls.Add(new LiteralControl("</td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("</table>"));










        Panel pan_mon = (Panel)this.FindControl("date_man_total_money");
        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        //case
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td colspan='2'>"));
        Label la = new Label();
        la.Text = DateTime.Now.Month.ToString() + "月の依頼件数";
        la.Font.Size = 24;
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        //all case
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));


        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));
        //special date one day
        SqlDataSource sql_h1 = new SqlDataSource();
        sql_h1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h1.SelectCommand = "select c.money_hour,c.hour,c.total_money,c.commission,a.check_success,e.year,e.month,e.day,e.end_hour,e.end_minute ";
        sql_h1.SelectCommand += "from user_information_appointment_check_deal as a ";
        sql_h1.SelectCommand += "inner join user_information_appointment_check_connect_deal as b ";
        sql_h1.SelectCommand += "on a.id=b.uiacdid ";
        sql_h1.SelectCommand += "inner join user_appointment as c ";
        sql_h1.SelectCommand += "on b.uaid=c.id ";
        sql_h1.SelectCommand += "inner join appointment as e ";
        sql_h1.SelectCommand += "on c.appid=e.id ";
        sql_h1.SelectCommand += "where a.suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and e.month='" + DateTime.Now.Month + "' and e.year='" + DateTime.Now.Year + "';";
        sql_h1.DataBind();


        ////定期 and week one day
        //SqlDataSource sql_h1 = new SqlDataSource();
        //sql_h1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sql_h1.SelectCommand = "select * ";
        //sql_h1.SelectCommand += "from user_information_appointment_check_deal as a ";
        //sql_h1.SelectCommand += "inner join user_information_appointment_check_connect_deal as b ";
        //sql_h1.SelectCommand += "on a.id=b.uiacdid ";
        //sql_h1.SelectCommand += "inner join user_information_store_week_appointment_check as d ";
        //sql_h1.SelectCommand += "on b.uiswacid=d.id ";
        //sql_h1.SelectCommand += "where a.suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and a.check_success='1' and DATEPART(month, d.start_date)='" + DateTime.Now.Month + "' and DATEPART(year, d.start_date)='" + DateTime.Now.Year + "';";
        //sql_h1.DataBind();


        DataView ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);
        int total = 0;
        int feature = 0;
        int cancel_case = 0;
        int success_case = 0;
        for (int i = 0; i < ict_h1.Count; i++)
        {
            if (DateTime.Now.Day >= Convert.ToInt32(ict_h1.Table.Rows[i]["day"].ToString()))
            {
                if (Convert.ToInt32(ict_h1.Table.Rows[i]["check_success"].ToString()) == 1)
                {
                    total += Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString()) - Convert.ToInt32(ict_h1.Table.Rows[i]["commission"].ToString());
                    success_case += 1;
                }
                else if (Convert.ToInt32(ict_h1.Table.Rows[i]["check_success"].ToString()) == 2)
                {
                    cancel_case += 1;
                }
            }
            else
            {
                feature += Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString());
            }
        }

        //定期 and week one day
        sql_h1 = new SqlDataSource();
        sql_h1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h1.SelectCommand = "select f.week_of_day,d.money_hour,d.hour,d.total_money,d.commission,a.check_success,d.start_date,d.end_date ";
        sql_h1.SelectCommand += "from user_information_appointment_check_deal as a ";
        sql_h1.SelectCommand += "inner join user_information_appointment_check_connect_deal as b ";
        sql_h1.SelectCommand += "on a.id=b.uiacdid ";
        sql_h1.SelectCommand += "inner join user_information_store_week_appointment_check as d ";
        sql_h1.SelectCommand += "on b.uiswacid=d.id ";
        sql_h1.SelectCommand += "inner join user_information_store_week_appointment as f ";
        sql_h1.SelectCommand += "on d.uiswaid=f.id ";
        sql_h1.SelectCommand += "where a.suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and a.check_success='1' and DATEPART(month, d.start_date)='" + DateTime.Now.Month + "' and DATEPART(year, d.start_date)='" + DateTime.Now.Year + "';";
        sql_h1.DataBind();

        ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);

        DateTime sdate = new DateTime();
        DateTime edate = new DateTime();
        if (ict_h1.Count > 0)
        {
            sdate = Convert.ToDateTime(ict_h1.Table.Rows[0]["start_date"].ToString());
            edate = Convert.ToDateTime(ict_h1.Table.Rows[0]["end_date"].ToString());
            DateTime datestart = new DateTime();
            int com = DateTime.Compare(sdate, edate);
            if (com < 0)
            {
                datestart = sdate;
            }
            else
            {
                datestart = edate;
            }
            int howmany = Math.Abs(Convert.ToInt32((edate - sdate).TotalDays));
            int[] week = new int[7];
            int[] fweek = new int[7];
            SqlDataSource sql_h_com = new SqlDataSource();
            sql_h_com.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h_com.SelectCommand = "select year,month,day ";
            sql_h_com.SelectCommand += "from appointment ";
            sql_h_com.SelectCommand += "where uid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and checked='0' and month='" + DateTime.Now.Month + "' and year='" + DateTime.Now.Year + "';";
            sql_h_com.DataBind();
            DataView ict_h_com = (DataView)sql_h_com.Select(DataSourceSelectArguments.Empty);
            List<DateTime> check_day = new List<DateTime>();
            if (ict_h_com.Count > 0)
            {
                for (int ix = 0; ix < ict_h_com.Count; ix++)
                {
                    string day = ict_h_com.Table.Rows[ix]["year"].ToString();
                    if (Convert.ToInt32(ict_h_com.Table.Rows[ix]["month"].ToString()) < 10)
                    {
                        day += "-0" + ict_h_com.Table.Rows[ix]["month"].ToString();
                    }
                    else
                    {
                        day += "-" + ict_h_com.Table.Rows[ix]["month"].ToString();
                    }
                    if (Convert.ToInt32(ict_h_com.Table.Rows[ix]["day"].ToString()) < 10)
                    {
                        day += "-0" + ict_h_com.Table.Rows[ix]["day"].ToString();
                    }
                    else
                    {
                        day += "-" + ict_h_com.Table.Rows[ix]["day"].ToString();
                    }
                    check_day.Add(Convert.ToDateTime(day));
                }
            }

            if (howmany == 0)
            {
                bool check_ok = true;
                for (int iy = 0; iy < check_day.Count; iy++)
                {
                    if (DateTime.Compare(check_day[iy], datestart) == 0)
                    {
                        check_ok = false;
                    }
                }
                if (check_ok)
                {
                    if (DateTime.Now.Day >= datestart.Day)
                    {
                        week[(int)datestart.DayOfWeek] += 1;
                    }
                    else
                    {
                        fweek[(int)datestart.DayOfWeek] += 1;
                    }
                }

            }
            else
            {
                bool check_ok = true;
                for (int ix = 0; ix < howmany; ix++)
                {
                    check_ok = true;
                    for (int iy = 0; iy < check_day.Count; iy++)
                    {
                        if (DateTime.Compare(check_day[iy], datestart) == 0)
                        {
                            check_ok = false;
                        }
                    }
                    if (check_ok)
                    {
                        if (DateTime.Now.Day >= datestart.Day)
                        {
                            week[(int)datestart.DayOfWeek] += 1;
                        }
                        else
                        {
                            fweek[(int)datestart.DayOfWeek] += 1;
                        }
                    }
                    datestart = datestart.AddDays(1);

                }
            }
            for (int i = 0; i < ict_h1.Count; i++)
            {
                int whichday = Convert.ToInt32(ict_h1.Table.Rows[i]["week_of_day"].ToString());
                if (whichday == 7)
                {
                    whichday = 0;
                }

                total += (Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString()) - Convert.ToInt32(ict_h1.Table.Rows[i]["commission"].ToString())) * week[whichday];
                feature += (Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString()) - Convert.ToInt32(ict_h1.Table.Rows[i]["commission"].ToString())) * fweek[whichday];

                success_case += 1;



                //if (DateTime.Now.Day >= sdate.Day)
                //{
                //    if (Convert.ToInt32(ict_h1.Table.Rows[i]["check_success"].ToString()) == 1)
                //    {
                //        total += Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString()) - Convert.ToInt32(ict_h1.Table.Rows[i]["commission"].ToString());
                //        success_case += 1;
                //    }
                //    else
                //    {
                //        cancel_case += 1;
                //    }
                //}
                //else
                //{
                //    feature += Convert.ToInt32(ict_h1.Table.Rows[i]["total_money"].ToString());
                //}
            }
        }
        int total_case = 0;

        sql_h1 = new SqlDataSource();
        sql_h1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h1.SelectCommand = "select a.id,c.money_hour,c.hour,c.total_money,c.commission,a.check_success,e.year,e.month,e.day,e.end_hour,e.end_minute ";
        sql_h1.SelectCommand += "from user_information_appointment_check_deal as a ";
        sql_h1.SelectCommand += "inner join user_information_appointment_check_connect_deal as b ";
        sql_h1.SelectCommand += "on a.id=b.uiacdid ";
        sql_h1.SelectCommand += "inner join user_appointment as c ";
        sql_h1.SelectCommand += "on b.uaid=c.id ";
        sql_h1.SelectCommand += "inner join appointment as e ";
        sql_h1.SelectCommand += "on c.appid=e.id ";
        sql_h1.SelectCommand += "where a.suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and e.month='" + DateTime.Now.Month + "' and e.year='" + DateTime.Now.Year + "'";
        sql_h1.SelectCommand += " order by a.id asc;";
        sql_h1.DataBind();
        ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);
        total_case = 0;
        cancel_case = 0;
        if (ict_h1.Count > 0)
        {
            int tmpid = 0;
            for (int ix = 0; ix < ict_h1.Count; ix++)
            {
                if (tmpid != Convert.ToInt32(ict_h1.Table.Rows[ix]["id"].ToString()))
                {
                    tmpid = Convert.ToInt32(ict_h1.Table.Rows[ix]["id"].ToString());
                    total_case += 1;
                    if (ict_h1.Table.Rows[ix]["check_success"].ToString() == "2")
                    {
                        cancel_case += 1;
                    }
                }
            }
        }
        sql_h1 = new SqlDataSource();
        sql_h1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h1.SelectCommand = "select a.id,f.week_of_day,d.money_hour,d.hour,d.total_money,d.commission,a.check_success,d.start_date,d.end_date ";
        sql_h1.SelectCommand += "from user_information_appointment_check_deal as a ";
        sql_h1.SelectCommand += "inner join user_information_appointment_check_connect_deal as b ";
        sql_h1.SelectCommand += "on a.id=b.uiacdid ";
        sql_h1.SelectCommand += "inner join user_information_store_week_appointment_check as d ";
        sql_h1.SelectCommand += "on b.uiswacid=d.id ";
        sql_h1.SelectCommand += "inner join user_information_store_week_appointment as f ";
        sql_h1.SelectCommand += "on d.uiswaid=f.id ";
        sql_h1.SelectCommand += "where a.suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and DATEPART(month, d.start_date)='" + DateTime.Now.Month + "' and DATEPART(year, d.start_date)='" + DateTime.Now.Year + "'";
        sql_h1.SelectCommand += " order by a.id asc;";
        sql_h1.DataBind();

        ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);
        if (ict_h1.Count > 0)
        {
            int tmpid = 0;
            for (int ix = 0; ix < ict_h1.Count; ix++)
            {
                if (tmpid != Convert.ToInt32(ict_h1.Table.Rows[ix]["id"].ToString()))
                {
                    tmpid = Convert.ToInt32(ict_h1.Table.Rows[ix]["id"].ToString());
                    total_case += 1;
                    if (ict_h1.Table.Rows[ix]["check_success"].ToString() == "2")
                    {
                        cancel_case += 1;
                    }
                }
            }
        }

        //SqlDataSource sql_h_cancel = new SqlDataSource();
        //sql_h_cancel.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sql_h_cancel.SelectCommand = "select check_success ";
        //sql_h_cancel.SelectCommand += "from user_information_appointment_check_deal ";
        //sql_h_cancel.SelectCommand += "where suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and month='" + DateTime.Now.Month + "' and year='" + DateTime.Now.Year + "';";
        //sql_h_cancel.DataBind();
        //DataView ict_h_cancel = (DataView)sql_h_cancel.Select(DataSourceSelectArguments.Empty);
        //int total_case = 0;
        //cancel_case = 0;
        //if (ict_h_com.Count > 0)
        //{
        //    for (int ix = 0; ix < ict_h_com.Count; ix++)
        //    {
        //    }
        //}


        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        //la = new Label();
        //la.Text = testweek;
        //pan_mon.Controls.Add(la);

        la = new Label();
        la.Text = "担当案件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = total_case + "件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));

        //cancel case
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));

        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "キャンセルした案件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = cancel_case + "件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        //line
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));
        pan_mon.Controls.Add(new LiteralControl("<hr/>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        //total case
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));

        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "合計";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = (total_case - cancel_case) + "件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));

        //money
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td colspan='2'>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));
        la = new Label();
        la.Text = DateTime.Now.Month.ToString() + "月の報酬";
        la.Font.Size = 24;
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));

        //all money
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));
        pan_mon.Controls.Add(new LiteralControl("<br/>"));

        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "報告完了済みの案件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "¥" + total;
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));

        //feature money
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));

        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "報告予定の案件";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "¥" + feature;
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        //line
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));
        pan_mon.Controls.Add(new LiteralControl("<hr/>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        //total money
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='10%'>"));
        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td>"));

        pan_mon.Controls.Add(new LiteralControl("<table width='100%'>"));
        pan_mon.Controls.Add(new LiteralControl("<tr>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "合計";
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("<td width='50%'>"));

        la = new Label();
        la.Text = "¥" + (feature + total);
        pan_mon.Controls.Add(la);

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));
        pan_mon.Controls.Add(new LiteralControl("</table>"));

        pan_mon.Controls.Add(new LiteralControl("</td>"));
        pan_mon.Controls.Add(new LiteralControl("</tr>"));



        pan_mon.Controls.Add(new LiteralControl("</table>"));


        Panel pan_mon1 = (Panel)this.FindControl("date_man_total_money1");

        pan_mon1.Controls.Add(new LiteralControl("<table width='100%' height='100%' style='border-style: solid; border-width: thin; background-color: #ea9494;'>"));
        pan_mon1.Controls.Add(new LiteralControl("<tr>"));
        pan_mon1.Controls.Add(new LiteralControl("<td width='100%' height='100%' align='center' valign='middle'>"));

        //int nextmon = (DateTime.Now.Month + 1), year = DateTime.Now.Year;
        //if (nextmon == 13) { nextmon = 1; year += 1; }

        //sql_h1.SelectCommand = "select b.money_hour,b.hour,b.total_money,b.commission,b.check_success,a.year,a.month,a.day,a.end_hour,a.end_minute ";
        //sql_h1.SelectCommand += "from appointment as a ";
        //sql_h1.SelectCommand += "inner join user_appointment as b ";
        //sql_h1.SelectCommand += "on a.id=b.appid ";
        //sql_h1.SelectCommand += "where a.uid=" + ict_h.Table.Rows[0]["id"].ToString() + " and  a.year>=" + year + " and a.month=" + nextmon + " and b.check_success=1;";
        //sql_h1.DataBind();
        //ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);


        la = new Label();
        la.Font.Size = 24;
        la.Text = DateTime.Now.Month.ToString() + "月のサマリー";
        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        pan_mon1.Controls.Add(la);
        pan_mon1.Controls.Add(new LiteralControl("<br/><br/>"));

        SqlDataSource sql_h_PV = new SqlDataSource();
        sql_h_PV.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_PV.SelectCommand = "select suppid,view_uid,count(view_uid) as PV";
        sql_h_PV.SelectCommand += " from user_information_store_PV_UU";
        sql_h_PV.SelectCommand += " where suppid='" + ict_h.Table.Rows[0]["id"].ToString() + "' and DATEPART(month, view_time)='" + DateTime.Now.Month + "' and DATEPART(year, view_time)='" + DateTime.Now.Year + "'";
        sql_h_PV.SelectCommand += " group by suppid,view_uid;";
        sql_h_PV.DataBind();
        DataView ict_h_PV = (DataView)sql_h_PV.Select(DataSourceSelectArguments.Empty);
        int PV = 0, UU = ict_h_PV.Count;
        if (ict_h_PV.Count > 0)
        {
            for (int i = 0; i < ict_h_PV.Count; i++)
            {
                PV += Convert.ToInt32(ict_h_PV.Table.Rows[i]["PV"].ToString());
            }
        }

        la = new Label();
        la.Font.Size = 20;
        la.Text = "PV数    " + PV;
        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        pan_mon1.Controls.Add(la);
        pan_mon1.Controls.Add(new LiteralControl("<br/>"));
        la = new Label();
        la.Font.Size = 20;
        la.Text = "UU数    " + UU;
        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        pan_mon1.Controls.Add(la);
        pan_mon1.Controls.Add(new LiteralControl("<br/>"));
        la = new Label();
        la.Font.Size = 20;
        la.Text = "依頼数    " + ict_h1.Count;
        la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        pan_mon1.Controls.Add(la);

        pan_mon1.Controls.Add(new LiteralControl("</td>"));
        pan_mon1.Controls.Add(new LiteralControl("</tr>"));
        pan_mon1.Controls.Add(new LiteralControl("</table>"));
        //ict_h.Table.Rows[0]["username"].ToString()



        //video panel
        Panel pan_vid1 = (Panel)this.FindControl("video_panel");

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));

        video_list = new List<video_information>();
        video_list1 = new List<video_information>();
        video_list2 = new List<video_information>();
        video_list3 = new List<video_information>();
        video_information vi = new video_information();

        sql_h1.SelectCommand = "select type,video,title,company,speaker,time ";
        sql_h1.SelectCommand += "from video ;";
        sql_h1.DataBind();
        ict_h1 = (DataView)sql_h1.Select(DataSourceSelectArguments.Empty);

        Label la_vi = new Label();
        la_vi.Text = "1-10件を表示/全" + ict_h1.Count + "件";

        pan_vid1.Controls.Add(la_vi);

        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
        pan_vid1.Controls.Add(new LiteralControl("</td>"));
        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
        //each video
        for (int i = 0; i < ict_h1.Count; i++)
        {
            vi = new video_information();
            vi.video = ict_h1.Table.Rows[i]["video"].ToString();
            vi.type = Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString());
            vi.title = ict_h1.Table.Rows[i]["title"].ToString();
            vi.time = Convert.ToInt32(ict_h1.Table.Rows[i]["time"].ToString());
            vi.speaker = ict_h1.Table.Rows[i]["speaker"].ToString();
            vi.company = ict_h1.Table.Rows[i]["company"].ToString();
            video_list.Add(vi);
            if (Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString()) == 0)
            {
                vi = new video_information();
                vi.video = ict_h1.Table.Rows[i]["video"].ToString();
                vi.type = Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString());
                vi.title = ict_h1.Table.Rows[i]["title"].ToString();
                vi.time = Convert.ToInt32(ict_h1.Table.Rows[i]["time"].ToString());
                vi.speaker = ict_h1.Table.Rows[i]["speaker"].ToString();
                vi.company = ict_h1.Table.Rows[i]["company"].ToString();
                video_list1.Add(vi);
            }
            if (Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString()) == 1)
            {
                vi = new video_information();
                vi.video = ict_h1.Table.Rows[i]["video"].ToString();
                vi.type = Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString());
                vi.title = ict_h1.Table.Rows[i]["title"].ToString();
                vi.time = Convert.ToInt32(ict_h1.Table.Rows[i]["time"].ToString());
                vi.speaker = ict_h1.Table.Rows[i]["speaker"].ToString();
                vi.company = ict_h1.Table.Rows[i]["company"].ToString();
                video_list2.Add(vi);
            }
            if (Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString()) == 2)
            {
                vi = new video_information();
                vi.video = ict_h1.Table.Rows[i]["video"].ToString();
                vi.type = Convert.ToInt32(ict_h1.Table.Rows[i]["type"].ToString());
                vi.title = ict_h1.Table.Rows[i]["title"].ToString();
                vi.time = Convert.ToInt32(ict_h1.Table.Rows[i]["time"].ToString());
                vi.speaker = ict_h1.Table.Rows[i]["speaker"].ToString();
                vi.company = ict_h1.Table.Rows[i]["company"].ToString();
                video_list3.Add(vi);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
            //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
            pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + ict_h1.Table.Rows[i]["video"].ToString() + " src='http://img.youtube.com/vi/" + ict_h1.Table.Rows[i]["video"].ToString() + "/1.jpg' title='" + ict_h1.Table.Rows[i]["title"].ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
            pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + ict_h1.Table.Rows[i]["video"].ToString() + "' title='" + ict_h1.Table.Rows[i]["title"].ToString() + "' style='text-decoration: none;'>" + ict_h1.Table.Rows[i]["title"].ToString() + "</a>"));
            pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
            la_vi = new Label();
            la_vi.Text = "講師: &nbsp;" + ict_h1.Table.Rows[i]["company"].ToString() + "  &nbsp;&nbsp;" + ict_h1.Table.Rows[i]["speaker"].ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + ict_h1.Table.Rows[i]["time"].ToString() + "分)";

            pan_vid1.Controls.Add(la_vi);

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
        int page_count = ict_h1.Count / 10;
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
        //Session["video_but"] = null;







        //update information
        sql_h = new SqlDataSource();
        sql_h.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h.SelectCommand = "select * ";
        sql_h.SelectCommand += "from user_information_store where uid='" + id + "';";
        sql_h.DataBind();
        ict_h = (DataView)sql_h.Select(DataSourceSelectArguments.Empty);

        foreach (ListItem item in CheckBoxList1.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList2.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList3.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList4.Items)
        {
            item.Selected = false;
        }
        CheckBox1.Checked = false;


        if (ict_h.Count > 0)
        {
            //choice 1
            if (ict_h.Table.Rows[0]["choice1_1"].ToString() == "1")
            {
                CheckBoxList1.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_3"].ToString() == "1")
            {
                CheckBoxList1.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_2"].ToString() == "1")
            {
                CheckBoxList1.Items[2].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_4"].ToString() == "1")
            {
                CheckBoxList1.Items[3].Selected = true;
            }
            //choice 2
            if (ict_h.Table.Rows[0]["choice2_1"].ToString() == "1")
            {
                CheckBoxList2.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_4"].ToString() == "1")
            {
                CheckBoxList2.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_2"].ToString() == "1")
            {
                CheckBoxList2.Items[2].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_5"].ToString() == "1")
            {
                CheckBoxList2.Items[3].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_3"].ToString() == "1")
            {
                CheckBoxList2.Items[4].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_6"].ToString() == "1")
            {
                CheckBoxList2.Items[5].Selected = true;
            }
            //choice 3
            if (ict_h.Table.Rows[0]["choice3_1"].ToString() == "1")
            {
                CheckBoxList3.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_4"].ToString() == "1")
            {
                CheckBoxList3.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_2"].ToString() == "1")
            {
                CheckBoxList4.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_3"].ToString() == "1")
            {
                CheckBoxList4.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_5"].ToString() == "1")
            {
                CheckBox1.Checked = true;
            }
            //howmany kid
            howmany_DropDownList.SelectedValue = ict_h.Table.Rows[0]["howmany"].ToString();
            //how age
            age_range_start_year_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_start_year"].ToString();
            age_range_start_month_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_start_month"].ToString();
            age_range_end_year_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_end_year"].ToString();
            age_range_end_month_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_end_month"].ToString();

            baby_rule_TextBox.Text = ict_h.Table.Rows[0]["baby_rule"].ToString();
            baby_notice_TextBox.Text = ict_h.Table.Rows[0]["baby_notice"].ToString();


            title_TextBox.Text = ict_h.Table.Rows[0]["title"].ToString();
            myself_content_TextBox.Text = ict_h.Table.Rows[0]["myself_content"].ToString();

            //money
            money_TextBox.Text = ict_h.Table.Rows[0]["money"].ToString();

            //bank
            bank_type_RadioButtonList.Items[Convert.ToInt32(ict_h.Table.Rows[0]["bank_type"].ToString())].Selected = true;
            bank_type_detail_RadioButtonList.Items[Convert.ToInt32(ict_h.Table.Rows[0]["bank_type_detail"].ToString())].Selected = true;
            bank_name_TextBox.Text = ict_h.Table.Rows[0]["bank_name"].ToString();
            bank_name_detail_TextBox.Text = ict_h.Table.Rows[0]["bank_name_detail"].ToString();
            bank_number_TextBox.Text = ict_h.Table.Rows[0]["bank_number"].ToString();
            bank_person_TextBox.Text = ict_h.Table.Rows[0]["bank_person"].ToString();


            //select week
            SqlDataSource sql_h2 = new SqlDataSource();
            sql_h2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h2.SelectCommand = "select * ";
            sql_h2.SelectCommand += "from user_information_store_week_appointment where uisid='" + ict_h.Table.Rows[0]["id"].ToString() + "';";
            sql_h2.DataBind();
            DataView ict_h2 = (DataView)sql_h2.Select(DataSourceSelectArguments.Empty);
            if (ict_h2.Count > 0)
            {
                for (int i = 0; i < ict_h2.Count; i++)
                {
                    if (ict_h2.Table.Rows[i]["checked"].ToString() == "1")
                    {
                        CheckBox che = (CheckBox)FindControl("week_of_day_CheckBox" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                        che.Checked = true;

                    }
                    DropDownList dro = (DropDownList)FindControl("start_hour_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["start_hour"].ToString();
                    dro = (DropDownList)FindControl("start_minute_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["start_minute"].ToString();
                    dro = (DropDownList)FindControl("end_hour_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["end_hour"].ToString();
                    dro = (DropDownList)FindControl("end_minute_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["end_minute"].ToString();
                }
            }


            sql_h2 = new SqlDataSource();
            sql_h2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h2.SelectCommand = "select * ";
            sql_h2.SelectCommand += "from user_information_store_images where uisid='" + ict_h.Table.Rows[0]["id"].ToString() + "';";
            sql_h2.DataBind();
            ict_h2 = (DataView)sql_h2.Select(DataSourceSelectArguments.Empty);
            if (ict_h2.Count > 0)
            {
                for (int i = 0; i < ict_h2.Count; i++)
                {
                    Image im = new Image();
                    im.Width = 100;
                    im.Height = 100;
                    im.ImageUrl = ict_h2.Table.Rows[i]["filename"].ToString();
                    Panel1.Controls.Add(im);
                }
            }

        }
        //update information

    }
    List<video_information> video_list = new List<video_information>();
    List<video_information> video_list1 = new List<video_information>();
    List<video_information> video_list2 = new List<video_information>();
    List<video_information> video_list3 = new List<video_information>();
    protected void change_page(object sender, EventArgs e)
    {
        Button temp = (Button)sender;
        string cutstr2 = temp.ID;
        int ind2 = cutstr2.IndexOf(@"_");
        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);

        Panel pan_vid1 = (Panel)this.FindControl("video_panel");
        pan_vid1.Controls.Clear();

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));

        int totalcount = 0;
        int type = 0;
        if (Session["video_but"] != null)
        {
            type = Convert.ToInt32(Session["video_but"].ToString());
            List<video_information> v_temp = new List<video_information>();
            if (type == 0)
            {
                v_temp = video_list1;
            }
            if (type == 1)
            {
                v_temp = video_list2;
            }
            if (type == 2)
            {
                v_temp = video_list3;
            }
            totalcount = v_temp.Count;
            Label la_vi = new Label();
            la_vi.Text = (10 * (Convert.ToInt32(cutstr3)) + 1) + "-" + (10 * (Convert.ToInt32(cutstr3) + 1)) + "件を表示/全" + totalcount + "件";

            pan_vid1.Controls.Add(la_vi);

            pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("</tr>"));
            //each video
            int endva = 0;
            if (v_temp.Count - (10 * Convert.ToInt32(cutstr3)) < (10 * (Convert.ToInt32(cutstr3) + 1)) - (10 * Convert.ToInt32(cutstr3)))
            {
                endva = v_temp.Count;
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
                pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
                //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
                pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + v_temp[i].video.ToString() + " src='http://img.youtube.com/vi/" + v_temp[i].video.ToString() + "/1.jpg' title='" + v_temp[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
                pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + v_temp[i].video.ToString() + "' title='" + v_temp[i].title.ToString() + "' style='text-decoration: none;'>" + v_temp[i].title.ToString() + "</a>"));
                pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
                la_vi = new Label();
                la_vi.Text = "講師: &nbsp;" + v_temp[i].company.ToString() + "  &nbsp;&nbsp;" + v_temp[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + v_temp[i].time.ToString() + "分)";

                pan_vid1.Controls.Add(la_vi);

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
            int page_count = v_temp.Count / 10;
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


        }
        else
        {
            totalcount = video_list.Count;
            Label la_vi = new Label();
            la_vi.Text = (10 * (Convert.ToInt32(cutstr3)) + 1) + "-" + (10 * (Convert.ToInt32(cutstr3) + 1)) + "件を表示/全" + totalcount + "件";

            pan_vid1.Controls.Add(la_vi);

            pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
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
                pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
                //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
                pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + video_list[i].video.ToString() + " src='http://img.youtube.com/vi/" + video_list[i].video.ToString() + "/1.jpg' title='" + video_list[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
                pan_vid1.Controls.Add(new LiteralControl("</td>"));
                pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
                pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + video_list[i].video.ToString() + "' title='" + video_list[i].title.ToString() + "' style='text-decoration: none;'>" + video_list[i].title.ToString() + "</a>"));
                pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
                la_vi = new Label();
                la_vi.Text = "講師: &nbsp;" + video_list[i].company.ToString() + "  &nbsp;&nbsp;" + video_list[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + video_list[i].time.ToString() + "分)";

                pan_vid1.Controls.Add(la_vi);

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


        }


        pan_vid1.Controls.Add(new LiteralControl("</table>"));


    }

    protected void date_manger_b1_Click(object sender, EventArgs e)
    {

        changeicon("reservation");
        date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        date_manger_b1.Font.Bold = true;
        date_manger_im1.Visible = true;

        date_manger_im2.Visible = false;
        date_manger_im3.Visible = false;
        date_manger_im4.Visible = false;
        date_manger_im5.Visible = false;

        date_manger_b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b2.Font.Bold = false;
        date_manger_b3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b3.Font.Bold = false;
        date_manger_b4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b4.Font.Bold = false;
        date_manger_b5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b5.Font.Bold = false;

        date_manger_Panel1.Visible = true;
        date_manger_Panel2.Visible = false;
        date_manger_Panel3.Visible = false;
        date_manger_Panel4.Visible = false;
        date_manger_Panel5.Visible = false;
       // reservation.Attributes.Add("class", "button_bottom  reservation_w");

    }
    protected void date_manger_b2_Click(object sender, EventArgs e)
    {
        changeicon("calender");
        date_manger_b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        date_manger_b2.Font.Bold = true;
        date_manger_im2.Visible = true;

        date_manger_im1.Visible = false;
        date_manger_im3.Visible = false;
        date_manger_im4.Visible = false;
        date_manger_im5.Visible = false;


        date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b1.Font.Bold = false;
        date_manger_b3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b3.Font.Bold = false;
        date_manger_b4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b4.Font.Bold = false;
        date_manger_b5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b5.Font.Bold = false;

        date_manger_Panel1.Visible = false;
        date_manger_Panel2.Visible = true;
        date_manger_Panel3.Visible = false;
        date_manger_Panel4.Visible = false;
        date_manger_Panel5.Visible = false;

    }
    protected void date_manger_b3_Click(object sender, EventArgs e)
    {
        changeicon("user");
        date_manger_b3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        date_manger_b3.Font.Bold = true;
        date_manger_im3.Visible = true;

        date_manger_im1.Visible = false;
        date_manger_im2.Visible = false;
        date_manger_im4.Visible = false;
        date_manger_im5.Visible = false;

        date_manger_b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b2.Font.Bold = false;
        date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b1.Font.Bold = false;
        date_manger_b4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b4.Font.Bold = false;
        date_manger_b5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b5.Font.Bold = false;


        date_manger_Panel1.Visible = false;
        date_manger_Panel2.Visible = false;
        date_manger_Panel3.Visible = true;
        date_manger_Panel4.Visible = false;
        date_manger_Panel5.Visible = false;
        //ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, "ShowProgressBar(this.id)", true);
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.ClientID, "ShowProgressBar(this.id)", true);
        //ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "$(document).ready(function(){EnableControls();alert('Overrides successfully Updated.');DisableControls();});", true);
        //Page.ClientScript.RegisterStartupScript(GetType(), "mykey", "ShowProgressBar(this.id);", true);
    }
    protected void date_manger_b4_Click(object sender, EventArgs e)
    {
        changeicon("graph");
        date_manger_b4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        date_manger_b4.Font.Bold = true;
        date_manger_im4.Visible = true;

        date_manger_im2.Visible = false;
        date_manger_im3.Visible = false;
        date_manger_im1.Visible = false;
        date_manger_im5.Visible = false;

        date_manger_b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b2.Font.Bold = false;
        date_manger_b3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b3.Font.Bold = false;
        date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b1.Font.Bold = false;
        date_manger_b5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b5.Font.Bold = false;

        date_manger_Panel1.Visible = false;
        date_manger_Panel2.Visible = false;
        date_manger_Panel3.Visible = false;
        date_manger_Panel4.Visible = true;
        date_manger_Panel5.Visible = false;
    }
    protected void date_manger_b5_Click(object sender, EventArgs e)
    {
        changeicon("movie");
        Session["video_but"] = null;
        date_manger_b5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        date_manger_b5.Font.Bold = true;
        date_manger_im5.Visible = true;

        date_manger_im2.Visible = false;
        date_manger_im3.Visible = false;
        date_manger_im4.Visible = false;
        date_manger_im1.Visible = false;

        date_manger_b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b2.Font.Bold = false;
        date_manger_b3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b3.Font.Bold = false;
        date_manger_b4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b4.Font.Bold = false;
        date_manger_b1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        date_manger_b1.Font.Bold = false;

        date_manger_Panel1.Visible = false;
        date_manger_Panel2.Visible = false;
        date_manger_Panel3.Visible = false;
        date_manger_Panel4.Visible = false;
        date_manger_Panel5.Visible = true;
    }
    List<string> impath = new List<string>();
    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument.HasFile)
        {
            impath = new List<string>();
            Panel1.Controls.Clear();
            foreach (HttpPostedFile postedFile in fuDocument.PostedFiles)
            {
                DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");

                SqlDataSource2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                SqlDataSource2.SelectCommand = "select id,name from filename_extension";
                SqlDataSource2.DataBind();
                DataView ou1 = (DataView)SqlDataSource2.Select(DataSourceSelectArguments.Empty);
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
                        SqlDataSource sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                        filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                        //AmazonUpload aws = new AmazonUpload();
                        //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                        Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                        string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                        //sql_insert.InsertCommand = "insert into filename_detail(filename,name)";
                        //sql_insert.InsertCommand += " values('~/fileplace/" + filename + "','" + fuDocument.FileName.ToString() + "')";
                        //sql_insert.Insert();

                        //upload_finish_files.Text += postedFile.FileName.ToString() + "  finish<br>";

                        //postedFile.SaveAs(Server.MapPath("store_images") + "\\" + filename);

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = imgurl;
                        impath.Add(@"~/" + imgurl);
                        this.Panel1.Controls.Add(im);
                        //Image1.ImageUrl = Server.MapPath("fileplace") + "\\" + filename;


                        //upload_files.Text += Server.MapPath("fileplace") + "\\" + filename + ",";
                        //upload_files0.Text += postedFile.FileName.ToString() + ",";
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



            ViewState["myData"] = impath;

        }
    }
    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        ////if (!e.Day.IsOtherMonth && !e.Day.IsWeekend)
        ////{
        ////    e.Cell.BackColor = System.Drawing.Color.Gray;
        ////}
        //e.Cell.BackColor = System.Drawing.Color.White;
        //e.Cell.Controls.Clear();
        //Label date = new Label();
        //date.Text = e.Day.DayNumberText;
        //e.Cell.Controls.Add(date);
        //if (e.Day.IsOtherMonth || e.Day.IsWeekend)
        //{

        //    Label table_top = new Label();
        //    table_top.Text = "</br>";
        //    table_top.Text += "<table width='50px' height='80px'><tr><td>";
        //    Image im = new Image();
        //    im.ImageUrl = "~/images/weekend.PNG";
        //    Label table_down = new Label();
        //    table_down.Text = "</td></tr></table>";
        //    e.Cell.Controls.Add(table_top);
        //    e.Cell.Controls.Add(im);
        //    e.Cell.Controls.Add(table_down);
        //}
        //List<int> check_date = new List<int>();
        //SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //SqlDataSource1.SelectCommand = "select * from appointment where uid='" + Session["id"].ToString() + "' and year='" + e.Day.Date.Year + "'";
        //SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "';";
        //SqlDataSource1.DataBind();
        //DataView ou1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        //for (int i = 0; i < ou1.Count; i++)
        //{
        //    check_date.Add(Convert.ToInt32(ou1.Table.Rows[i]["day"].ToString()));
        //}


        //if (!e.Day.IsOtherMonth && !e.Day.IsWeekend)
        //{
        //    Label table_top = new Label();
        //    table_top.Text = "</br>";
        //    table_top.Text += "<table width='50px' height='50px' ><tr><td align='center'>";
        //    bool check_day = false;
        //    for (int i = 0; i < check_date.Count; i++)
        //    {
        //        if (e.Day.Date.Day == check_date[i])
        //        {
        //            check_day = true;
        //        }
        //    }
        //    Panel date_click_no = new Panel();

        //    Label la = new Label();
        //    la.Text = "X";

        //    //la.CssClass = "button_no button_no_day opener";


        //    date_click_no.CssClass = "button_no button_no_day opener";
        //    date_click_no.Controls.Add(la);

        //    Panel date_click = new Panel();
        //    string dateid = e.Day.Date.Year.ToString();
        //    if (e.Day.Date.Month < 10)
        //    {
        //        dateid += "0" + e.Day.Date.Month;
        //    }
        //    else
        //    {
        //        dateid += e.Day.Date.Month;
        //    }
        //    if (e.Day.Date.Day < 10)
        //    {
        //        dateid += "0" + e.Day.Date.Day;
        //    }
        //    else
        //    {
        //        dateid += e.Day.Date.Day;
        //    }

        //    //date_click_no.Attributes["onclick"] = "showDialog(this.id)";


        //    date_click.CssClass = "button button_yes opener";
        //    //date_click.Attributes["onclick"] = "showDialog(this.id)";

        //    Label table_down = new Label();
        //    table_down.Text = "</td></tr></table>";
        //    e.Cell.Controls.Add(table_top);
        //    if (check_day)
        //    {
        //        date_click.ID = dateid;
        //        SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //        SqlDataSource1.SelectCommand = "select * from appointment where uid=1 and year='" + e.Day.Date.Year + "'";
        //        SqlDataSource1.SelectCommand += " and month='" + e.Day.Date.Month + "' and day='" + e.Day.Date.Day + "';";
        //        SqlDataSource1.DataBind();
        //        DataView ou2 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
        //        for (int i = 0; i < ou2.Count; i++)
        //        {
        //            Label laa = new Label();
        //            string txt = ou2.Table.Rows[i]["start_hour"].ToString() + ":";
        //            if (Convert.ToInt32(ou2.Table.Rows[i]["start_minute"].ToString()) < 10)
        //            {
        //                txt += "0" + ou2.Table.Rows[i]["start_minute"].ToString();
        //            }
        //            else
        //            {
        //                txt += ou2.Table.Rows[i]["start_minute"].ToString();
        //            }
        //            txt += @"</br>|</br>" + ou2.Table.Rows[i]["end_hour"].ToString() + ":";
        //            if (Convert.ToInt32(ou2.Table.Rows[i]["end_minute"].ToString()) < 10)
        //            {
        //                txt += "0" + ou2.Table.Rows[i]["end_minute"].ToString();
        //            }
        //            else
        //            {
        //                txt += ou2.Table.Rows[i]["end_minute"].ToString();
        //            }
        //            laa.Text = txt;
        //            date_click.Controls.Add(laa);
        //        }
        //        e.Cell.Controls.Add(date_click);
        //    }
        //    else
        //    {
        //        date_click_no.ID = dateid;
        //        //e.Cell.Controls.Add(la);
        //        e.Cell.Controls.Add(date_click_no);
        //    }

        //    e.Cell.Controls.Add(table_down);
        //    //e.Cell.BackColor = System.Drawing.Color.Gray;

        //}

        //SelectedDatesCollection theDates = Calendar1.SelectedDates;


        //////////////////////////////////////////


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
        List<int> check_date = new List<int>();
        List<int> no_check_date = new List<int>();

        if (Session["id"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("main.aspx");
        }
        else
        {
            //string sup_id = Session["sup_id"].ToString();
            string id = Session["id"].ToString();


            SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource1.SelectCommand = "select checked,day from appointment where uid='" + id + "' and year='" + e.Day.Date.Year + "'";
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

                Panel date_click_no = new Panel();

                Label la = new Label();
                la.Text = "X";

                //la.CssClass = "button_no button_no_day opener";

                date_click_no.CssClass = "button_no button_no_day opener";
                date_click_no.Controls.Add(la);





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
                //date_click.CssClass = "button button_yes";
                date_click.CssClass = "button button_yes opener";
                //date_click.Attributes["onclick"] = "selectdate(this.id)";

                Label table_down = new Label();
                table_down.Text = "</td></tr></table>";
                e.Cell.Controls.Add(table_top);

                if (check_week_day)
                {
                    if (check_day)
                    {
                        SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        SqlDataSource1.SelectCommand = "select * from appointment where uid='" + id + "' and year='" + e.Day.Date.Year + "'";
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
                        date_click_no.ID = dateid;
                        e.Cell.Controls.Add(date_click_no);
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
                    SqlDataSource1.SelectCommand = "select * from appointment where uid='" + id + "' and year='" + e.Day.Date.Year + "'";
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
                    date_click_no.ID = dateid;
                    //e.Cell.Controls.Add(la);
                    e.Cell.Controls.Add(date_click_no);
                }




                e.Cell.Controls.Add(table_down);
                //e.Cell.BackColor = System.Drawing.Color.Gray;

            }

            SelectedDatesCollection theDates = Calendar1.SelectedDates;
        }


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
    public class video_information
    {
        public int type = 0;
        public int time = 0;
        public string video = "";
        public string title = "";
        public string company = "";
        public string speaker = "";
    }
    protected void data_video_Button1_Click(object sender, EventArgs e)
    {
        Session["video_but"] = 0;
        Panel pan_vid1 = (Panel)this.FindControl("video_panel");
        pan_vid1.Controls.Clear();

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));


        List<video_information> v_temp = video_list1;

        int totalcount = v_temp.Count;
        Label la_vi = new Label();
        la_vi.Text = "1-10件を表示/全" + totalcount + "件";

        pan_vid1.Controls.Add(la_vi);

        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
        pan_vid1.Controls.Add(new LiteralControl("</td>"));
        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
        //each video
        int firpage = video_list1.Count;
        if (firpage > 10)
        {
            firpage = 10;
        }

        for (int i = 0; i < firpage; i++)
        {
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
            //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
            pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + v_temp[i].video.ToString() + " src='http://img.youtube.com/vi/" + v_temp[i].video.ToString() + "/1.jpg' title='" + v_temp[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
            pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + v_temp[i].video.ToString() + "' title='" + v_temp[i].title.ToString() + "' style='text-decoration: none;'>" + v_temp[i].title.ToString() + "</a>"));
            pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
            la_vi = new Label();
            la_vi.Text = "講師: &nbsp;" + v_temp[i].company.ToString() + "  &nbsp;&nbsp;" + v_temp[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + v_temp[i].time.ToString() + "分)";

            pan_vid1.Controls.Add(la_vi);

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
        int page_count = v_temp.Count / 10;
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
    protected void data_video_Button2_Click(object sender, EventArgs e)
    {
        Session["video_but"] = 1;
        Panel pan_vid1 = (Panel)this.FindControl("video_panel");
        pan_vid1.Controls.Clear();

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));


        List<video_information> v_temp = video_list2;

        int totalcount = v_temp.Count;
        Label la_vi = new Label();
        la_vi.Text = "1-10件を表示/全" + totalcount + "件";

        pan_vid1.Controls.Add(la_vi);

        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
        pan_vid1.Controls.Add(new LiteralControl("</td>"));
        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
        //each video
        int firpage = video_list2.Count;
        if (firpage > 10)
        {
            firpage = 10;
        }

        for (int i = 0; i < firpage; i++)
        {
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
            //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
            pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + v_temp[i].video.ToString() + " src='http://img.youtube.com/vi/" + v_temp[i].video.ToString() + "/1.jpg' title='" + v_temp[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
            pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + v_temp[i].video.ToString() + "' title='" + v_temp[i].title.ToString() + "' style='text-decoration: none;'>" + v_temp[i].title.ToString() + "</a>"));
            pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
            la_vi = new Label();
            la_vi.Text = "講師: &nbsp;" + v_temp[i].company.ToString() + "  &nbsp;&nbsp;" + v_temp[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + v_temp[i].time.ToString() + "分)";

            pan_vid1.Controls.Add(la_vi);

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
        int page_count = v_temp.Count / 10;
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
    protected void data_video_Button3_Click(object sender, EventArgs e)
    {
        Session["video_but"] = 2;
        Panel pan_vid1 = (Panel)this.FindControl("video_panel");
        pan_vid1.Controls.Clear();

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));


        List<video_information> v_temp = video_list3;

        int totalcount = v_temp.Count;
        Label la_vi = new Label();
        la_vi.Text = "1-10件を表示/全" + totalcount + "件";

        pan_vid1.Controls.Add(la_vi);

        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
        pan_vid1.Controls.Add(new LiteralControl("</td>"));
        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
        //each video
        int firpage = video_list3.Count;
        if (firpage > 10)
        {
            firpage = 10;
        }

        for (int i = 0; i < firpage; i++)
        {
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
            //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
            pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + v_temp[i].video.ToString() + " src='http://img.youtube.com/vi/" + v_temp[i].video.ToString() + "/1.jpg' title='" + v_temp[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
            pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + v_temp[i].video.ToString() + "' title='" + v_temp[i].title.ToString() + "' style='text-decoration: none;'>" + v_temp[i].title.ToString() + "</a>"));
            pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
            la_vi = new Label();
            la_vi.Text = "講師: &nbsp;" + v_temp[i].company.ToString() + "  &nbsp;&nbsp;" + v_temp[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + v_temp[i].time.ToString() + "分)";

            pan_vid1.Controls.Add(la_vi);

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
        int page_count = v_temp.Count / 10;
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
    protected void data_video_Button_Click(object sender, EventArgs e)
    {
        Session["video_but"] = null;
        Panel pan_vid1 = (Panel)this.FindControl("video_panel");
        pan_vid1.Controls.Clear();

        pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
        pan_vid1.Controls.Add(new LiteralControl("<tr>"));
        pan_vid1.Controls.Add(new LiteralControl("<td>"));
        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));


        List<video_information> v_temp = video_list;

        int totalcount = v_temp.Count;
        Label la_vi = new Label();
        la_vi.Text = "1-10件を表示/全" + totalcount + "件";

        pan_vid1.Controls.Add(la_vi);

        pan_vid1.Controls.Add(new LiteralControl("<hr/>"));
        pan_vid1.Controls.Add(new LiteralControl("</td>"));
        pan_vid1.Controls.Add(new LiteralControl("</tr>"));
        //each video
        int firpage = video_list.Count;
        if (firpage > 10)
        {
            firpage = 10;
        }

        for (int i = 0; i < firpage; i++)
        {
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td>"));
            pan_vid1.Controls.Add(new LiteralControl("<table width='100%' height='100%'>"));
            pan_vid1.Controls.Add(new LiteralControl("<tr>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='20%'>"));
            //<a class="youtube" href="http://www.youtube.com/watch?v=oL75JJqfZTY" title="jQuery YouTube Popup Player Plugin TEST">Test Me</a>
            pan_vid1.Controls.Add(new LiteralControl("<img class='youtube' id=" + v_temp[i].video.ToString() + " src='http://img.youtube.com/vi/" + v_temp[i].video.ToString() + "/1.jpg' title='" + v_temp[i].title.ToString() + "' style='width: 100%; height: 100px; cursor: pointer;' />"));


            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td width='5%'>"));
            pan_vid1.Controls.Add(new LiteralControl("</td>"));
            pan_vid1.Controls.Add(new LiteralControl("<td valign='top'>"));
            pan_vid1.Controls.Add(new LiteralControl("<a class='youtube' href='http://www.youtube.com/watch?v=" + v_temp[i].video.ToString() + "' title='" + v_temp[i].title.ToString() + "' style='text-decoration: none;'>" + v_temp[i].title.ToString() + "</a>"));
            pan_vid1.Controls.Add(new LiteralControl("<br/><br/>"));
            la_vi = new Label();
            la_vi.Text = "講師: &nbsp;" + v_temp[i].company.ToString() + "  &nbsp;&nbsp;" + v_temp[i].speaker.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;(" + v_temp[i].time.ToString() + "分)";

            pan_vid1.Controls.Add(la_vi);

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
        int page_count = v_temp.Count / 10;
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
    [WebMethod]
    public static string search_time(string param1, string param2, string param3)
    {
        string result1 = "<td>" + param1 + "</td>" + param2;
        SqlDataSource sql_f = new SqlDataSource();

        //        select c.money_hour,c.hour,c.total_money,c.commission,a.check_success,e.year,e.month,e.day,e.end_hour,e.end_minute
        //from user_information_appointment_check_deal as a
        //inner join user_information_appointment_check_connect_deal as b
        //on a.id=b.uiacdid
        //inner join user_appointment as c
        //on b.uaid=c.id
        //inner join appointment as e
        //on c.appid=e.id
        //where a.suppid='10' and a.check_success='1' and e.month='9' and e.year='2016'

        string result = "";

        List<date_list> dllist = new List<date_list>();
        date_list dl = new date_list();


        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select a.id,c.appid,c.uid,f.photo,f.username,a.type,a.check_success,e.year,e.month,e.day,e.start_hour,e.start_minute,e.end_hour,e.end_minute,c.howtoget_there from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_appointment as c on b.uaid=c.id";
        sql_f.SelectCommand += " inner join appointment as e on c.appid=e.id ";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.suppid='" + param3 + "' and e.year= '" + param1 + "' and e.month= '" + param2 + "'";
        sql_f.SelectCommand += " order by e.day asc,a.first_check_time asc,c.uid asc;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        string cutstr_h = "", cutstr_h1 = "", week = "";
        int ind_h = 0;
        DateTime todate;
        string temp_appid = "", temp_uid = "", button_name = "";
        bool check_same = false;
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                check_same = false;
                result = "";
                if (i == 0)
                {
                    temp_appid = ict_f.Table.Rows[i]["appid"].ToString();
                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                    check_same = true;
                }
                else
                {
                    if (temp_appid != ict_f.Table.Rows[i]["appid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                    {
                        temp_appid = ict_f.Table.Rows[i]["appid"].ToString();
                        temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                        check_same = true;
                    }
                }
                if (check_same)
                {
                    dl = new date_list();

                    todate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()));
                    dl.date = todate;
                    if (todate.DayOfWeek == DayOfWeek.Monday)
                    {
                        week = "月曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        week = "火曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        week = "水曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Thursday)
                    {
                        week = "木曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Friday)
                    {
                        week = "金曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        week = "土曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        week = "日曜日";
                    }

                    result += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result += "<tr>";

                    result += "<td width='5%'>";
                    result += "</td>";

                    result += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + ict_f.Table.Rows[i]["month"].ToString() + " 月 " + ict_f.Table.Rows[i]["day"].ToString() + " 日, ";
                    result += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                    result += "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/><br/>";
                    result += "</td>";
                    result += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                    result += "<table width='100%'>";
                    result += "<tr>";
                    result += "<td width='10%'></td>";
                    result += "<td width='20%' valign='top'>";
                    //user photo
                    result += "<div class='zoom-gallery'>";
                    cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                    ind_h = cutstr_h.IndexOf(@"/");
                    cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                    result += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                    result += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                    result += "</a>";
                    result += "</div>";
                    result += "</td>";
                    result += "<td width='70%'>";
                    result += "<span style='color: blue;'>";
                    result += ict_f.Table.Rows[i]["username"].ToString();
                    result += "</span><br/>";
                    result += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";

                    result += "</td>";
                    result += "<td width='20%' align='center'>";
                    if (ict_f.Table.Rows[i]["check_success"].ToString() == "0")
                    {
                        button_name = "承認待ち";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='check_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "1")
                    {
                        button_name = "報告をしよう";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='report_create(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "2")
                    {
                        button_name = "お断り";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "3")
                    {
                        button_name = "報告書の確認";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "4")
                    {
                        button_name = "評価待ち";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "5")
                    {
                        button_name = "完了";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";
                    dl.element = result;
                    dllist.Add(dl);
                }
            }
        }
        //select one day

        //select 定期 one day
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.type='1' and a.suppid='" + param3 + "' and DATEPART(year, d.start_date)= '" + param1 + "' and DATEPART(month, d.start_date)= '" + param2 + "'";
        sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
        sql_f.DataBind();
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        string temp_uiswaid = "";
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                result = "";
                check_same = false;
                if (i == 0)
                {
                    temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                    check_same = true;
                }
                else
                {
                    if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                    {
                        temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                        temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                        check_same = true;
                    }
                }
                if (check_same)
                {
                    dl = new date_list();
                    DateTime.TryParse(ict_f.Table.Rows[i]["start_date"].ToString(), out todate);
                    dl.date = todate;
                    if (todate.DayOfWeek == DayOfWeek.Monday)
                    {
                        week = "月曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        week = "火曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        week = "水曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Thursday)
                    {
                        week = "木曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Friday)
                    {
                        week = "金曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        week = "土曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        week = "日曜日";
                    }

                    result += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result += "<tr>";

                    result += "<td width='5%'>";
                    result += "</td>";

                    result += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + todate.Month + " 月 " + todate.Day + " 日, ";
                    result += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                    result += "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/><br/>";
                    result += "</td>";
                    result += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                    result += "<table width='100%'>";
                    result += "<tr>";
                    result += "<td width='10%'></td>";
                    result += "<td width='20%' valign='top'>";
                    //user photo
                    result += "<div class='zoom-gallery'>";
                    cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                    ind_h = cutstr_h.IndexOf(@"/");
                    cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                    result += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                    result += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                    result += "</a>";
                    result += "</div>";
                    result += "</td>";
                    result += "<td width='70%'>";
                    result += "<span style='color: blue;'>";
                    result += ict_f.Table.Rows[i]["username"].ToString();
                    result += "</span><br/>";
                    result += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";

                    result += "</td>";
                    result += "<td width='20%' align='center'>";
                    if (ict_f.Table.Rows[i]["check_success"].ToString() == "0")
                    {
                        button_name = "承認待ち";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='check_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "1")
                    {
                        button_name = "報告をしよう";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='report_create(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "2")
                    {
                        button_name = "お断り";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "3")
                    {
                        button_name = "報告書の確認";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "4")
                    {
                        button_name = "評価待ち";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "5")
                    {
                        button_name = "完了";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";
                    dl.element = result;
                    dllist.Add(dl);
                }
            }
        }

        //select 定期 more than one day
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f1.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f1.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f1.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f1.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f1.SelectCommand += " where a.type='2' and a.suppid='" + param3 + "' and DATEPART(year, d.start_date)= '" + param1 + "' and DATEPART(month, d.start_date)= '" + param2 + "'";
        sql_f1.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,a.id asc;";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);

        temp_uiswaid = "";
        string temp_id = "", result_2 = "";
        bool same = false;

        if (ict_f1.Count > 0)
        {
            result_2 = "";
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                same = false;
                if (ii == 0)
                {
                    temp_id = ict_f1.Table.Rows[ii]["id"].ToString();
                    same = true;

                    result_2 = "";
                    dl = new date_list();
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);
                    dl.date = todate;
                    result_2 += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result_2 += "<tr>";
                    result_2 += "<td width='5%'>";
                    result_2 += "</td>";
                    result_2 += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result_2 += "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                    result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                    result_2 += todate.Month + " 月 " + todate.Day + " 日</span><br/>";
                    //result += "</td>";


                }
                else
                {
                    if (temp_id != ict_f1.Table.Rows[ii]["id"].ToString())
                    {
                        result_2 += "<br/><span>" + ict_f1.Table.Rows[ii - 1]["howtoget_there"].ToString() + "</span><br/><br/>";
                        result_2 += "</td>";
                        result_2 += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                        result_2 += "<table width='100%'>";
                        result_2 += "<tr>";
                        result_2 += "<td width='10%'></td>";
                        result_2 += "<td width='20%' valign='top'>";
                        //user photo
                        result_2 += "<div class='zoom-gallery'>";
                        cutstr_h = ict_f1.Table.Rows[ii - 1]["photo"].ToString();
                        ind_h = cutstr_h.IndexOf(@"/");
                        cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                        result_2 += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ii - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
                        result_2 += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                        result_2 += "</a>";
                        result_2 += "</div>";
                        result_2 += "</td>";
                        result_2 += "<td width='70%'>";
                        result_2 += "<span style='color: blue;'>";
                        result_2 += ict_f1.Table.Rows[ii - 1]["username"].ToString();
                        result_2 += "</span><br/>";
                        result_2 += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                        result_2 += "</td>";
                        result_2 += "</tr>";
                        result_2 += "</table>";

                        result_2 += "</td>";
                        result_2 += "<td width='20%' align='center'>";

                        if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "0")
                        {
                            button_name = "承認待ち";
                            result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ii - 1]["id"].ToString() + "' value='" + button_name + "' onclick='check_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "1")
                        {
                            button_name = "報告をしよう";
                            result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ii - 1]["id"].ToString() + "' value='" + button_name + "' onclick='report_create(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "2")
                        {
                            button_name = "お断り";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "3")
                        {
                            button_name = "報告書の確認";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "4")
                        {
                            button_name = "評価待ち";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "5")
                        {
                            button_name = "完了";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        result_2 += "</td>";
                        result_2 += "</tr>";
                        result_2 += "</table>";
                        dl.element = result_2;
                        dllist.Add(dl);


                        result_2 = "";

                        temp_id = ict_f1.Table.Rows[ii]["id"].ToString();
                        same = true;

                        dl = new date_list();
                        DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);
                        dl.date = todate;
                        result_2 += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                        result_2 += "<tr>";
                        result_2 += "<td width='5%'>";
                        result_2 += "</td>";
                        result_2 += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                        result_2 += "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                        result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";
                        DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                        result_2 += todate.Month + " 月 " + todate.Day + " 日</span><br/>";
                    }
                }
                if (same)
                {

                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select g.week_of_day_jp,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                    sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
                    sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                    sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                    sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
                    sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid ";
                    sql_f.SelectCommand += " where a.id='" + temp_id + "' and a.type='2' and a.suppid='" + param3 + "' and DATEPART(year, d.start_date)= '" + param1 + "' and DATEPART(month, d.start_date)= '" + param2 + "'";
                    sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
                    sql_f.DataBind();
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                    temp_uiswaid = ""; temp_uid = "";
                    if (ict_f.Count > 0)
                    {
                        for (int i = 0; i < ict_f.Count; i++)
                        {
                            check_same = false;
                            if (i == 0)
                            {
                                temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                check_same = true;
                            }
                            else
                            {
                                if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                                {
                                    temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                    check_same = true;
                                }
                            }
                            if (check_same)
                            {
                                week = ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + "曜日";

                                result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", ";
                                result_2 += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";



                            }
                        }
                    }


                }

            }
            result_2 += "<br/><span>" + ict_f1.Table.Rows[ict_f1.Count - 1]["howtoget_there"].ToString() + "</span><br/><br/>";
            result_2 += "</td>";
            result_2 += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

            result_2 += "<table width='100%'>";
            result_2 += "<tr>";
            result_2 += "<td width='10%'></td>";
            result_2 += "<td width='20%' valign='top'>";
            //user photo
            result_2 += "<div class='zoom-gallery'>";
            cutstr_h = ict_f1.Table.Rows[ict_f1.Count - 1]["photo"].ToString();
            ind_h = cutstr_h.IndexOf(@"/");
            cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
            result_2 += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
            result_2 += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
            result_2 += "</a>";
            result_2 += "</div>";
            result_2 += "</td>";
            result_2 += "<td width='70%'>";
            result_2 += "<span style='color: blue;'>";
            result_2 += ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString();
            result_2 += "</span><br/>";
            result_2 += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
            result_2 += "</td>";
            result_2 += "</tr>";
            result_2 += "</table>";

            result_2 += "</td>";
            result_2 += "<td width='20%' align='center'>";

            if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "0")
            {
                button_name = "承認待ち";
                result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ict_f1.Count - 1]["id"].ToString() + "' value='" + button_name + "' onclick='check_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "1")
            {
                button_name = "報告をしよう";
                result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ict_f1.Count - 1]["id"].ToString() + "' value='" + button_name + "' onclick='report_create(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "2")
            {
                button_name = "お断り";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "3")
            {
                button_name = "報告書の確認";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "4")
            {
                button_name = "評価待ち";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "5")
            {
                button_name = "完了";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }


            result_2 += "</td>";
            result_2 += "</tr>";
            result_2 += "</table>";
            dl.element = result_2;
            dllist.Add(dl);

        }

        dllist.Sort((a, b) => a.date.CompareTo(b.date));
        result1 = "";
        //result1 = dllist.Count.ToString();
        for (int i = 0; i < dllist.Count; i++)
        {
            result1 += dllist[i].element;
        }
        return result1;

    }
    public class date_list
    {
        public DateTime date;
        public string element = "";
    }
    protected void date_manger_information_b1_Click(object sender, EventArgs e)
    {

    }
    protected void date_manger_information_b2_Click(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            ScriptManager.RegisterStartupScript(date_manger_information_b2, date_manger_information_b2.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("home.aspx");
        }
        else
        {
            string uid = Session["id"].ToString();

            List<int> selected = new List<int>();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList3.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList4.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            if (CheckBox1.Checked)
            {
                selected.Add(1);
            }
            else
            {
                selected.Add(0);
            }

            string howmany = howmany_DropDownList.SelectedValue;
            string age_range_start_year = age_range_start_year_DropDownList.SelectedValue;
            string age_range_start_month = age_range_start_month_DropDownList.SelectedValue;
            string age_range_end_year = age_range_end_year_DropDownList.SelectedValue;
            string age_range_end_month = age_range_end_month_DropDownList.SelectedValue;

            string baby_rule = baby_rule_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string baby_notice = baby_notice_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            int money = 0;
            try
            {
                money = Convert.ToInt32(money_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim());
            }
            catch (Exception ex)
            {

                money_Label.Text = "整数を入力してください.";
                return;
                throw ex;
            }

            string title = title_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string myself_content = myself_content_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();



            string bank_name = bank_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string bank_name_detail = bank_name_detail_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string bank_number = bank_number_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string bank_person = bank_person_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            int bank_type = 0, bank_type_detail = 0;


            bool check_bank_type = false, check_bank_type_detail = false, check_bank_name = false, check_bank_name_detail = false
                , check_bank_number = false, check_bank_person = false;

            if (bank_type_RadioButtonList.SelectedIndex > -1)
            {
                bank_type = bank_type_RadioButtonList.SelectedIndex;

                check_bank_type = true;
                bank_type_Label.Text = "";
            }
            else
            {
                bank_type_Label.Text = "Bank type no select.";
            }

            if (bank_type_detail_RadioButtonList.SelectedIndex > -1)
            {
                bank_type_detail = bank_type_detail_RadioButtonList.SelectedIndex;
                check_bank_type_detail = true;
                bank_type_detail_Label.Text = "";
            }
            else
            {
                bank_type_detail_Label.Text = "Bank type detail no select.";
            }

            if (bank_name != "")
            {
                check_bank_name = true;
                bank_name_Label.Text = "";
            }
            else
            {
                bank_name_Label.Text = "Bank name have special word or not write.";
            }
            if (bank_name_detail != "")
            {
                check_bank_name_detail = true;
                bank_name_detail_Label.Text = "";
            }
            else
            {
                bank_name_detail_Label.Text = "Bank name detail have special word or not write.";
            }
            if (bank_number != "")
            {
                try
                {
                    int number = Convert.ToInt32(bank_number);
                    check_bank_number = true;
                    bank_number_Label.Text = "";
                }
                catch (Exception ex)
                {
                    bank_number_Label.Text = "Bank number is not number.";
                    return;
                    throw ex;
                }
            }
            else
            {
                bank_number_Label.Text = "Bank number have special word or not write.";
            }
            if (bank_person != "")
            {
                check_bank_person = true;
                bank_person_Label.Text = "";
            }
            else
            {
                bank_person_Label.Text = "Bank person have special word or not write.";
            }








            //impath = (List<string>)ViewState["myData"];
            ////ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('" + impath.Count + "')", true);
            //for (int i = 0; i < impath.Count; i++)
            //{
            //    SqlDataSource sql_insert = new SqlDataSource();
            //    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            //    sql_insert.InsertCommand = "insert into user_information_store_images(uisid,filename)";
            //    sql_insert.InsertCommand += " values('" + uisid + "','" + impath[i] + "');";
            //    sql_insert.Insert();
            //}

            //title_TextBox.Text = "";
            //myself_content_TextBox.Text = "";
            //Panel1.Controls.Clear();
            //impath.Clear();



            bool check_db = false;
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id from user_information_store";
            sql_f.SelectCommand += " where uid='" + uid + "';";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                check_db = true;
            }

            if (check_db)
            {
                ////bank start
                //if (check_bank_person && check_bank_number && check_bank_name_detail && check_bank_name && check_bank_type_detail &&
                //   check_bank_type)
                //{

                string uisid = ict_f.Table.Rows[0]["id"].ToString();

                //SqlDataSource sql_update = new SqlDataSource();
                //sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //sql_update.UpdateCommand = "update user_information_store";
                //sql_update.UpdateCommand += " set choice1_1='" + selected[0] + "',choice1_2='" + selected[2] + "',choice1_3='" + selected[1] + "'";
                //sql_update.UpdateCommand += ",choice1_4='" + selected[3] + "',choice2_1='" + selected[4] + "',choice2_2='" + selected[6] + "',choice2_3='" + selected[8] + "',choice2_4='" + selected[5] + "'";
                //sql_update.UpdateCommand += ",choice2_5='" + selected[7] + "',choice2_6='" + selected[9] + "',choice3_1='" + selected[10] + "',choice3_2='" + selected[12] + "'";
                //sql_update.UpdateCommand += ",choice3_3='" + selected[13] + "',choice3_4='" + selected[11] + "',choice3_5='" + selected[14] + "',howmany='" + howmany + "'";
                //sql_update.UpdateCommand += ",age_range_start_year='" + age_range_start_year + "',age_range_start_month='" + age_range_start_month + "',age_range_end_year='" + age_range_end_year + "',age_range_end_month='" + age_range_end_month + "',money='" + money + "'";
                //sql_update.UpdateCommand += ",bank_name='" + bank_name + "',bank_name_detail='" + bank_name_detail + "',bank_number='" + bank_number + "',bank_person='" + bank_person + "',bank_type='" + bank_type + "',bank_type_detail='" + bank_type_detail + "'";
                //sql_update.UpdateCommand += ",baby_rule='" + baby_rule + "',baby_notice='" + baby_notice + "',title='" + title + "',myself_content='" + myself_content + "'";
                //sql_update.UpdateCommand += " where id='"+uisid+"';";
                //sql_update.Update();


                SqlDataSource sql_update = new SqlDataSource();
                sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_update.UpdateCommand = "update user_information_store";
                sql_update.UpdateCommand += " set choice1_1='" + selected[0] + "',choice1_2='" + selected[2] + "',choice1_3='" + selected[1] + "'";
                sql_update.UpdateCommand += ",choice1_4='" + selected[3] + "',choice2_1='" + selected[4] + "',choice2_2='" + selected[6] + "',choice2_3='" + selected[8] + "',choice2_4='" + selected[5] + "'";
                sql_update.UpdateCommand += ",choice2_5='" + selected[7] + "',choice2_6='" + selected[9] + "',choice3_1='" + selected[10] + "',choice3_2='" + selected[12] + "'";
                sql_update.UpdateCommand += ",choice3_3='" + selected[13] + "',choice3_4='" + selected[11] + "',choice3_5='" + selected[14] + "',howmany='" + howmany + "'";
                sql_update.UpdateCommand += ",age_range_start_year='" + age_range_start_year + "',age_range_start_month='" + age_range_start_month + "',age_range_end_year='" + age_range_end_year + "',age_range_end_month='" + age_range_end_month + "',money='" + money + "'";
                sql_update.UpdateCommand += ",baby_rule='" + baby_rule + "',baby_notice='" + baby_notice + "',title='" + title + "',myself_content='" + myself_content + "'";
                sql_update.UpdateCommand += " where id='" + uisid + "';";
                sql_update.Update();

                impath = (List<string>)ViewState["myData"];
                //ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('" + impath.Count + "')", true);
                if (impath != null)
                {
                    if (impath.Count > 0)
                    {
                        SqlDataSource sql_f_im = new SqlDataSource();
                        sql_f_im.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_f_im.SelectCommand = "select id from user_information_store_images";
                        sql_f_im.SelectCommand += " where uisid='" + uisid + "';";
                        sql_f_im.DataBind();
                        DataView ict_f_im = (DataView)sql_f_im.Select(DataSourceSelectArguments.Empty);

                        if (ict_f_im.Count > 0)
                        {
                            for (int ii = 0; ii < ict_f_im.Count; ii++)
                            {
                                SqlDataSource sql_f_im_del = new SqlDataSource();
                                sql_f_im_del.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                                sql_f_im_del.DeleteCommand = "DELETE FROM user_information_store_images ";
                                sql_f_im_del.DeleteCommand += "WHERE id='" + ict_f_im.Table.Rows[ii]["id"].ToString() + "';";
                                sql_f_im_del.Delete();
                            }
                        }
                        for (int i = 0; i < impath.Count; i++)
                        {
                            SqlDataSource sql_insert = new SqlDataSource();
                            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_insert.InsertCommand = "insert into user_information_store_images(uisid,filename)";
                            sql_insert.InsertCommand += " values('" + uisid + "','" + impath[i] + "');";
                            sql_insert.Insert();
                        }
                    }

                    Panel1.Controls.Clear();
                    impath.Clear();
                }



                Session["id"] = uid;


                baby_rule_TextBox.Text = "";
                baby_notice_TextBox.Text = "";
                money_TextBox.Text = "";
                title_TextBox.Text = "";
                myself_content_TextBox.Text = "";

                howmany_DropDownList.SelectedIndex = 0;
                age_range_start_year_DropDownList.SelectedIndex = 0;
                age_range_start_month_DropDownList.SelectedIndex = 0;
                age_range_end_year_DropDownList.SelectedIndex = 0;
                age_range_end_month_DropDownList.SelectedIndex = 0;



                bank_name_TextBox.Text = "";
                bank_name_detail_TextBox.Text = "";
                bank_number_TextBox.Text = "";
                bank_person_TextBox.Text = "";

                bank_type_RadioButtonList.SelectedIndex = -1;
                bank_type_detail_RadioButtonList.SelectedIndex = -1;


                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList2.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList3.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList4.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                if (CheckBox1.Checked)
                {
                    CheckBox1.Checked = false;
                }


                SqlDataSource sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select id from user_information_store_week_appointment";
                sql_f1.SelectCommand += " where uisid='" + uisid + "' order by week_of_day asc;";
                sql_f1.DataBind();

                DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    //update
                    for (int i = 0; i < 7; i++)
                    {

                        CheckBox checked_week = (CheckBox)this.FindControl("week_of_day_CheckBox" + i);
                        int chc = 0;
                        if (checked_week.Checked)
                        {
                            chc = 1;
                        }
                        DropDownList start_hour = (DropDownList)this.FindControl("start_hour_DropDownList" + i);
                        DropDownList start_minute = (DropDownList)this.FindControl("start_minute_DropDownList" + i);
                        DropDownList end_hour = (DropDownList)this.FindControl("end_hour_DropDownList" + i);
                        DropDownList end_minute = (DropDownList)this.FindControl("end_minute_DropDownList" + i);

                        SqlDataSource sql_updtae = new SqlDataSource();
                        sql_updtae.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_updtae.UpdateCommand = "update user_information_store_week_appointment set checked='" + chc + "',start_hour='" + start_hour.SelectedValue + "',start_minute='" + start_minute.SelectedValue + "',end_hour='" + end_hour.SelectedValue + "',end_minute='" + end_minute.SelectedValue + "' ";
                        sql_updtae.UpdateCommand += " where id='" + ict_f1.Table.Rows[i]["id"].ToString() + "' and week_of_day='" + (i + 1) + "';";
                        sql_updtae.Update();

                    }
                    result_Label.Text = "Success update.";
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        CheckBox checked_week = (CheckBox)this.FindControl("week_of_day_CheckBox" + i);
                        int chc = 0;
                        if (checked_week.Checked)
                        {
                            chc = 1;
                        }
                        DropDownList start_hour = (DropDownList)this.FindControl("start_hour_DropDownList" + i);
                        DropDownList start_minute = (DropDownList)this.FindControl("start_minute_DropDownList" + i);
                        DropDownList end_hour = (DropDownList)this.FindControl("end_hour_DropDownList" + i);
                        DropDownList end_minute = (DropDownList)this.FindControl("end_minute_DropDownList" + i);

                        SqlDataSource sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_week_appointment(uisid,checked,week_of_day,week_of_day_jp,start_hour,start_minute,end_hour,end_minute)";
                        sql_insert.InsertCommand += " values('" + uisid + "','" + chc + "','" + (i + 1) + "','" + checked_week.Text + "'";
                        sql_insert.InsertCommand += ",'" + start_hour.SelectedValue + "','" + start_minute.SelectedValue + "','" + end_hour.SelectedValue + "','" + end_minute.SelectedValue + "');";
                        sql_insert.Insert();

                    }

                    result_Label.Text = "Success update.";
                }


                //}//bank end
            }
            else
            {
                result_Label.Text = "Fail.";
            }
        }


        string id = Session["id"].ToString();
        //update information
        SqlDataSource sql_h = new SqlDataSource();
        sql_h.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h.SelectCommand = "select * ";
        sql_h.SelectCommand += "from user_information_store where uid='" + id + "';";
        sql_h.DataBind();
        DataView ict_h = (DataView)sql_h.Select(DataSourceSelectArguments.Empty);

        foreach (ListItem item in CheckBoxList1.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList2.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList3.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem item in CheckBoxList4.Items)
        {
            item.Selected = false;
        }
        CheckBox1.Checked = false;


        if (ict_h.Count > 0)
        {
            //choice 1
            if (ict_h.Table.Rows[0]["choice1_1"].ToString() == "1")
            {
                CheckBoxList1.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_3"].ToString() == "1")
            {
                CheckBoxList1.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_2"].ToString() == "1")
            {
                CheckBoxList1.Items[2].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice1_4"].ToString() == "1")
            {
                CheckBoxList1.Items[3].Selected = true;
            }
            //choice 2
            if (ict_h.Table.Rows[0]["choice2_1"].ToString() == "1")
            {
                CheckBoxList2.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_4"].ToString() == "1")
            {
                CheckBoxList2.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_2"].ToString() == "1")
            {
                CheckBoxList2.Items[2].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_5"].ToString() == "1")
            {
                CheckBoxList2.Items[3].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_3"].ToString() == "1")
            {
                CheckBoxList2.Items[4].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice2_6"].ToString() == "1")
            {
                CheckBoxList2.Items[5].Selected = true;
            }
            //choice 3
            if (ict_h.Table.Rows[0]["choice3_1"].ToString() == "1")
            {
                CheckBoxList3.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_4"].ToString() == "1")
            {
                CheckBoxList3.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_2"].ToString() == "1")
            {
                CheckBoxList4.Items[0].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_3"].ToString() == "1")
            {
                CheckBoxList4.Items[1].Selected = true;
            }
            if (ict_h.Table.Rows[0]["choice3_5"].ToString() == "1")
            {
                CheckBox1.Checked = true;
            }
            //howmany kid
            howmany_DropDownList.SelectedValue = ict_h.Table.Rows[0]["howmany"].ToString();
            //how age
            age_range_start_year_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_start_year"].ToString();
            age_range_start_month_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_start_month"].ToString();
            age_range_end_year_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_end_year"].ToString();
            age_range_end_month_DropDownList.SelectedValue = ict_h.Table.Rows[0]["age_range_end_month"].ToString();

            baby_rule_TextBox.Text = ict_h.Table.Rows[0]["baby_rule"].ToString();
            baby_notice_TextBox.Text = ict_h.Table.Rows[0]["baby_notice"].ToString();


            title_TextBox.Text = ict_h.Table.Rows[0]["title"].ToString();
            myself_content_TextBox.Text = ict_h.Table.Rows[0]["myself_content"].ToString();

            //money
            money_TextBox.Text = ict_h.Table.Rows[0]["money"].ToString();

            //bank
            bank_type_RadioButtonList.Items[Convert.ToInt32(ict_h.Table.Rows[0]["bank_type"].ToString())].Selected = true;
            bank_type_detail_RadioButtonList.Items[Convert.ToInt32(ict_h.Table.Rows[0]["bank_type_detail"].ToString())].Selected = true;
            bank_name_TextBox.Text = ict_h.Table.Rows[0]["bank_name"].ToString();
            bank_name_detail_TextBox.Text = ict_h.Table.Rows[0]["bank_name_detail"].ToString();
            bank_number_TextBox.Text = ict_h.Table.Rows[0]["bank_number"].ToString();
            bank_person_TextBox.Text = ict_h.Table.Rows[0]["bank_person"].ToString();


            //select week
            SqlDataSource sql_h2 = new SqlDataSource();
            sql_h2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h2.SelectCommand = "select * ";
            sql_h2.SelectCommand += "from user_information_store_week_appointment where uisid='" + ict_h.Table.Rows[0]["id"].ToString() + "';";
            sql_h2.DataBind();
            DataView ict_h2 = (DataView)sql_h2.Select(DataSourceSelectArguments.Empty);
            if (ict_h2.Count > 0)
            {
                for (int i = 0; i < ict_h2.Count; i++)
                {
                    if (ict_h2.Table.Rows[i]["checked"].ToString() == "1")
                    {
                        CheckBox che = (CheckBox)FindControl("week_of_day_CheckBox" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                        che.Checked = true;

                    }
                    DropDownList dro = (DropDownList)FindControl("start_hour_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["start_hour"].ToString();
                    dro = (DropDownList)FindControl("start_minute_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["start_minute"].ToString();
                    dro = (DropDownList)FindControl("end_hour_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["end_hour"].ToString();
                    dro = (DropDownList)FindControl("end_minute_DropDownList" + (Convert.ToInt32(ict_h2.Table.Rows[i]["week_of_day"].ToString()) - 1));
                    dro.SelectedValue = ict_h2.Table.Rows[i]["end_minute"].ToString();
                }
            }


            sql_h2 = new SqlDataSource();
            sql_h2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h2.SelectCommand = "select * ";
            sql_h2.SelectCommand += "from user_information_store_images where uisid='" + ict_h.Table.Rows[0]["id"].ToString() + "';";
            sql_h2.DataBind();
            ict_h2 = (DataView)sql_h2.Select(DataSourceSelectArguments.Empty);
            if (ict_h2.Count > 0)
            {
                for (int i = 0; i < ict_h2.Count; i++)
                {
                    Image im = new Image();
                    im.Width = 100;
                    im.Height = 100;
                    im.ImageUrl = ict_h2.Table.Rows[i]["filename"].ToString();
                    Panel1.Controls.Add(im);
                }
            }

        }
        //update information
    }
    [WebMethod]
    public static string Save_date(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8)
    {
        string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8;
        result = "";

        int ind = param3.IndexOf("/");
        string year = param3.Substring(0, ind);
        string cut = param3.Substring(ind + 1, param3.Length - ind - 1);
        int ind1 = cut.IndexOf("/");

        string month = cut.Substring(0, ind1);
        string day = cut.Substring(ind1 + 1, cut.Length - ind1 - 1);

        int ind2 = param4.IndexOf("/");
        string year1 = param4.Substring(0, ind2);
        string cut1 = param4.Substring(ind2 + 1, param4.Length - ind2 - 1);
        int ind3 = cut1.IndexOf("/");

        string month1 = cut1.Substring(0, ind3);
        string day1 = cut1.Substring(ind3 + 1, cut1.Length - ind3 - 1);

        int check_date = 0;
        if (param2 == "yes")
        {
            check_date = 1;
        }

        DateTime date0 = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
        DateTime date1 = new DateTime(Convert.ToInt32(year1), Convert.ToInt32(month1), Convert.ToInt32(day1));
        int howmany = Math.Abs(Convert.ToInt32((date1 - date0).TotalDays));

        DateTime datestart = new DateTime();
        int com = DateTime.Compare(date0, date1);
        SqlDataSource sql_f_user = new SqlDataSource();
        DataView ict_f_user;
        SqlDataSource sql_f_update = new SqlDataSource();
        SqlDataSource sql_f_insert = new SqlDataSource();
        if (com < 0)
        {
            datestart = date0;
        }
        else
        {
            datestart = date1;
        }

        sql_f_user = new SqlDataSource();
        sql_f_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_user.SelectCommand = "select id from appointment";
        sql_f_user.SelectCommand += " where uid='" + param1 + "' and year='" + datestart.Year + "' and month='" + datestart.Month + "' and day='" + datestart.Day + "';";
        sql_f_user.DataBind();
        ict_f_user = (DataView)sql_f_user.Select(DataSourceSelectArguments.Empty);
        if (ict_f_user.Count > 0)
        {
            sql_f_update = new SqlDataSource();
            sql_f_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_update.UpdateCommand = "update appointment set checked='" + check_date + "',start_hour='" + param5 + "',start_minute='" + param6 + "',end_hour='" + param7 + "',end_minute='" + param8 + "'";
            sql_f_update.UpdateCommand += " where id='" + ict_f_user.Table.Rows[0]["id"].ToString() + "';";
            sql_f_update.Update();
        }
        else
        {
            sql_f_insert = new SqlDataSource();
            sql_f_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_insert.InsertCommand = "insert into appointment (uid,checked,year,month,day,start_hour,start_minute,end_hour,end_minute)";
            sql_f_insert.InsertCommand += " values('" + param1 + "','" + check_date + "','" + datestart.Year + "','" + datestart.Month + "','" + datestart.Day + "','" + param5 + "','" + param6 + "','" + param7 + "','" + param8 + "');";
            sql_f_insert.Insert();
        }

        for (int i = 1; i <= howmany; i++)
        {
            datestart = datestart.AddDays(1);
            sql_f_user = new SqlDataSource();
            sql_f_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_user.SelectCommand = "select id from appointment";
            sql_f_user.SelectCommand += " where uid='" + param1 + "' and year='" + datestart.Year + "' and month='" + datestart.Month + "' and day='" + datestart.Day + "';";
            sql_f_user.DataBind();
            ict_f_user = (DataView)sql_f_user.Select(DataSourceSelectArguments.Empty);
            if (ict_f_user.Count > 0)
            {
                sql_f_update = new SqlDataSource();
                sql_f_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_update.UpdateCommand = "update appointment set checked='" + check_date + "',start_hour='" + param5 + "',start_minute='" + param6 + "',end_hour='" + param7 + "',end_minute='" + param8 + "'";
                sql_f_update.UpdateCommand += " where id='" + ict_f_user.Table.Rows[0]["id"].ToString() + "';";
                sql_f_update.Update();
            }
            else
            {
                sql_f_insert = new SqlDataSource();
                sql_f_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f_insert.InsertCommand = "insert into appointment (uid,checked,year,month,day,start_hour,start_minute,end_hour,end_minute)";
                sql_f_insert.InsertCommand += " values('" + param1 + "','" + check_date + "','" + datestart.Year + "','" + datestart.Month + "','" + datestart.Day + "','" + param5 + "','" + param6 + "','" + param7 + "','" + param8 + "');";
                sql_f_insert.Insert();
            }

        }


        result = "Success";


        return result;
    }
    [WebMethod]
    public static string friend_notice_list(string param1)
    {
        string result = param1;
        result = "";
        //setup check time
        SqlDataSource sql_f_t = new SqlDataSource();
        sql_f_t.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_t.SelectCommand = "select id";
        sql_f_t.SelectCommand += " from user_notice_check";
        sql_f_t.SelectCommand += " where uid='" + param1 + "' and type='2';";
        sql_f_t.DataBind();
        DataView ict_f_t = (DataView)sql_f_t.Select(DataSourceSelectArguments.Empty);
        if (ict_f_t.Count > 0)
        {
            SqlDataSource sql_f_t_up = new SqlDataSource();
            sql_f_t_up.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_up.UpdateCommand = "update user_notice_check set check_time=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
            sql_f_t_up.UpdateCommand += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            sql_f_t_up.Update();
        }
        else
        {
            SqlDataSource sql_f_t_in = new SqlDataSource();
            sql_f_t_in.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_in.InsertCommand = "insert into user_notice_check(uid,type,check_time)";
            sql_f_t_in.InsertCommand += " values('" + param1 + "','2',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00'));";
            sql_f_t_in.Insert();
        }
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
    public class friend_user
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
        public int howmany = 0;
    }
    [WebMethod]
    public static string search_friend_notice_list(string param1)
    {
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        SqlDataSource sql_h_find_user = new SqlDataSource();
        sql_h_find_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_find_user.SelectCommand = "select id,username,photo ";
        sql_h_find_user.SelectCommand += "from user_login";
        sql_h_find_user.SelectCommand += " where id!='" + param1.Trim() + "';";
        sql_h_find_user.DataBind();
        DataView ict_h_find_user = (DataView)sql_h_find_user.Select(DataSourceSelectArguments.Empty);
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
        sql_h_find_user = new SqlDataSource();
        sql_h_find_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_find_user.SelectCommand = "select donotfind_uid ";
        sql_h_find_user.SelectCommand += "from user_friendship_donotfind";
        sql_h_find_user.SelectCommand += " where uid='" + param1.Trim() + "';";
        sql_h_find_user.DataBind();
        ict_h_find_user = (DataView)sql_h_find_user.Select(DataSourceSelectArguments.Empty);
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
        //sql_h_fri_notice.SelectCommand = "select first_uid,second_uid ";
        //sql_h_fri_notice.SelectCommand += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = (DataView)sql_h_fri_notice.Select(DataSourceSelectArguments.Empty);
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
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.id,c.username,c.photo";
        sql_f.SelectCommand += " from user_friendship as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,c.username,c.photo";
            sql_f.SelectCommand += " from user_friendship as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f.SelectCommand += " where b.id='" + output_friend[i].id + "'";
            sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select b.id,b.username,b.photo";
            sql_f1.SelectCommand += " from user_friendship as a";
            sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f1.SelectCommand += " where c.id='" + output_friend[i].id + "'";
            sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
        int[] ValueString = new int[Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString())];

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

        <input id='addfriend_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload' style='width:100% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload' style='width:100% !important;'/>

        </td>
        </tr>
        </table><hr/></div>";
        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string search_friend_notice_list_scroll(string param1)
    {
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        SqlDataSource sql_h_find_user = new SqlDataSource();
        sql_h_find_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_find_user.SelectCommand = "select id,username,photo ";
        sql_h_find_user.SelectCommand += "from user_login";
        sql_h_find_user.SelectCommand += " where id!='" + param1.Trim() + "';";
        sql_h_find_user.DataBind();
        DataView ict_h_find_user = (DataView)sql_h_find_user.Select(DataSourceSelectArguments.Empty);
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
        sql_h_find_user = new SqlDataSource();
        sql_h_find_user.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_find_user.SelectCommand = "select donotfind_uid ";
        sql_h_find_user.SelectCommand += "from user_friendship_donotfind";
        sql_h_find_user.SelectCommand += " where uid='" + param1.Trim() + "';";
        sql_h_find_user.DataBind();
        ict_h_find_user = (DataView)sql_h_find_user.Select(DataSourceSelectArguments.Empty);
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
        //sql_h_fri_notice.SelectCommand = "select first_uid,second_uid ";
        //sql_h_fri_notice.SelectCommand += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = (DataView)sql_h_fri_notice.Select(DataSourceSelectArguments.Empty);
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
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.id,c.username,c.photo";
        sql_f.SelectCommand += " from user_friendship as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,c.username,c.photo";
            sql_f.SelectCommand += " from user_friendship as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f.SelectCommand += " where b.id='" + output_friend[i].id + "'";
            sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select b.id,b.username,b.photo";
            sql_f1.SelectCommand += " from user_friendship as a";
            sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f1.SelectCommand += " where c.id='" + output_friend[i].id + "'";
            sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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

        <input id='addfriend_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload' style='width:100% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload' style='width:100% !important;'/>

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
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select a.id";
        sql_f.SelectCommand += " from user_friendship as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        sql_f.SelectCommand += " and c.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {
            upid = ict_f.Table.Rows[0]["id"].ToString();
            chec = true;
        }
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select a.id";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        sql_f1.SelectCommand += " and b.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            upid = ict_f1.Table.Rows[0]["id"].ToString();
            chec = false;
        }

        if (upid != "")
        {
            if (chec)
            {
                SqlDataSource sql_h_fri1 = new SqlDataSource();
                sql_h_fri1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_h_fri1.UpdateCommand = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                sql_h_fri1.UpdateCommand += ",first_date_year='" + year + "',first_date_month='" + month + "',first_date_day='" + day + "',first_date_hour='" + hour + "',first_date_minute='" + min + "',first_date_second='" + sec + "'";
                sql_h_fri1.UpdateCommand += " where id='" + upid + "';";
                sql_h_fri1.Update();
            }
            else
            {
                SqlDataSource sql_h_fri1 = new SqlDataSource();
                sql_h_fri1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_h_fri1.UpdateCommand = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                sql_h_fri1.UpdateCommand += ",second_date_year='" + year + "',second_date_month='" + month + "',second_date_day='" + day + "',second_date_hour='" + hour + "',second_date_minute='" + min + "',second_date_second='" + sec + "'";
                sql_h_fri1.UpdateCommand += " where id='" + upid + "';";
                sql_h_fri1.Update();
            }

        }
        else
        {
            SqlDataSource sql_h_fri1 = new SqlDataSource();
            sql_h_fri1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h_fri1.InsertCommand = "insert into user_friendship(first_uid,first_check_connect,second_uid,second_check_connect";
            sql_h_fri1.InsertCommand += ",first_date_year,first_date_month,first_date_day,first_date_hour,first_date_minute,first_date_second)";
            sql_h_fri1.InsertCommand += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','1','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','0'";
            sql_h_fri1.InsertCommand += ",'" + year + "','" + month + "','" + day + "','" + hour + "','" + min + "','" + sec + "');";
            sql_h_fri1.Insert();
        }



        return result;
    }
    [WebMethod]
    public static string friend_notice_donotfind(string param1, string param2)
    {
        string result = param1;

        result = "";
        SqlDataSource sql_h_fri_notice = new SqlDataSource();
        sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_h_fri_notice.InsertCommand = "insert into user_friendship_donotfind(uid,donotfind_uid)";
        sql_h_fri_notice.InsertCommand += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "');";
        sql_h_fri_notice.Insert();

        return result;
    }
    [WebMethod]
    public static string toget_friend_list(string param1, string param2)
    {
        string result = param1;
        result = "";
        //fu = new friend_user();
        //fu.id = check_same[ii].id;
        //fu.username = check_same[ii].username;
        //fu.photo = check_same[ii].photo;
        //output_friend.Add(fu);
        List<friend_user> user_friend = new List<friend_user>();
        friend_user fu = new friend_user();
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.id,c.username,c.photo";
        sql_f.SelectCommand += " from user_friendship as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,c.username,c.photo";
            sql_f.SelectCommand += " from user_friendship as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f.SelectCommand += " where b.id='" + param2.Trim() + "'";
            sql_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
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
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select b.id,b.username,b.photo";
            sql_f1.SelectCommand += " from user_friendship as a";
            sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
            sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            sql_f1.SelectCommand += " where c.id='" + param2.Trim() + "'";
            sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
    [WebMethod]
    public static string friend_notice_check_del(string param1)
    {
        string result = param1;
        result = "";
        if (param1.Trim() != "")
        {
            SqlDataSource sql_h_fri_notice = new SqlDataSource();
            sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_h_fri_notice.DeleteCommand = "DELETE FROM user_friendship ";
            sql_h_fri_notice.DeleteCommand += "where id='" + param1 + "';";
            sql_h_fri_notice.Delete();
        }

        return result;
    }
    [WebMethod]
    public static string chat_notice_list(string param1)
    {
        string result = param1;
        result = "";
        //setup check time
        SqlDataSource sql_f_t = new SqlDataSource();
        sql_f_t.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_t.SelectCommand = "select id";
        sql_f_t.SelectCommand += " from user_notice_check";
        sql_f_t.SelectCommand += " where uid='" + param1 + "' and type='1';";
        sql_f_t.DataBind();
        DataView ict_f_t = (DataView)sql_f_t.Select(DataSourceSelectArguments.Empty);
        if (ict_f_t.Count > 0)
        {
            SqlDataSource sql_f_t_up = new SqlDataSource();
            sql_f_t_up.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_up.UpdateCommand = "update user_notice_check set check_time=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
            sql_f_t_up.UpdateCommand += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            sql_f_t_up.Update();
        }
        else
        {
            SqlDataSource sql_f_t_in = new SqlDataSource();
            sql_f_t_in.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_in.InsertCommand = "insert into user_notice_check(uid,type,check_time)";
            sql_f_t_in.InsertCommand += " values('" + param1 + "','1',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00'));";
            sql_f_t_in.Insert();
        }
        string money_total = "";

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
        //setup check time
        SqlDataSource sql_f_t = new SqlDataSource();
        sql_f_t.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_t.SelectCommand = "select id";
        sql_f_t.SelectCommand += " from user_notice_check";
        sql_f_t.SelectCommand += " where uid='" + param1 + "' and type='0';";
        sql_f_t.DataBind();
        DataView ict_f_t = (DataView)sql_f_t.Select(DataSourceSelectArguments.Empty);
        if (ict_f_t.Count > 0)
        {
            SqlDataSource sql_f_t_up = new SqlDataSource();
            sql_f_t_up.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_up.UpdateCommand = "update user_notice_check set check_time=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
            sql_f_t_up.UpdateCommand += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            sql_f_t_up.Update();
        }
        else
        {
            SqlDataSource sql_f_t_in = new SqlDataSource();
            sql_f_t_in.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_in.InsertCommand = "insert into user_notice_check(uid,type,check_time)";
            sql_f_t_in.InsertCommand += " values('" + param1 + "','0',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00'));";
            sql_f_t_in.Insert();
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        SqlDataSource sql_ff = new SqlDataSource();
        sql_ff.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_ff.SelectCommand = "select c.id,c.username,c.photo";
        sql_ff.SelectCommand += " from user_friendship as a";
        sql_ff.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_ff.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_ff.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_ff.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_ff.DataBind();
        DataView ict_ff = (DataView)sql_ff.Select(DataSourceSelectArguments.Empty);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

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

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f1.SelectCommand += " from status_messages as a";
            sql_f1.SelectCommand += " where a.uid='" + user_friend[i] + "'";
            sql_f1.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            sql_f1.DataBind();
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            sql_f1.SelectCommand += " from status_messages as a";
            sql_f1.SelectCommand += " inner join status_messages_user_like as b on a.id=b.smid";
            sql_f1.SelectCommand += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            sql_f1.SelectCommand += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            sql_f1.DataBind();
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select username,photo";
                sql_f.SelectCommand += " from user_login";
                sql_f.SelectCommand += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count > 0)
                {
                    othfri = ict_f.Table.Rows[0]["username"].ToString();
                }
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

        }

        return result;
    }
    [WebMethod]
    public static string new_state_notice_list_scroll(string param1)
    {
        string result = param1;
        result = "";
        //setup check time
        SqlDataSource sql_f_t = new SqlDataSource();
        sql_f_t.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f_t.SelectCommand = "select id";
        sql_f_t.SelectCommand += " from user_notice_check";
        sql_f_t.SelectCommand += " where uid='" + param1 + "' and type='0';";
        sql_f_t.DataBind();
        DataView ict_f_t = (DataView)sql_f_t.Select(DataSourceSelectArguments.Empty);
        if (ict_f_t.Count > 0)
        {
            SqlDataSource sql_f_t_up = new SqlDataSource();
            sql_f_t_up.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_up.UpdateCommand = "update user_notice_check set check_time=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
            sql_f_t_up.UpdateCommand += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            sql_f_t_up.Update();
        }
        else
        {
            SqlDataSource sql_f_t_in = new SqlDataSource();
            sql_f_t_in.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f_t_in.InsertCommand = "insert into user_notice_check(uid,type,check_time)";
            sql_f_t_in.InsertCommand += " values('" + param1 + "','0',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00'));";
            sql_f_t_in.Insert();
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        SqlDataSource sql_ff = new SqlDataSource();
        sql_ff.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_ff.SelectCommand = "select c.id,c.username,c.photo";
        sql_ff.SelectCommand += " from user_friendship as a";
        sql_ff.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_ff.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_ff.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_ff.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_ff.DataBind();
        DataView ict_ff = (DataView)sql_ff.Select(DataSourceSelectArguments.Empty);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1.SelectCommand += " from user_friendship as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

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

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f1.SelectCommand += " from status_messages as a";
            sql_f1.SelectCommand += " where a.uid='" + user_friend[i] + "'";
            sql_f1.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            sql_f1.DataBind();
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            sql_f1.SelectCommand += " from status_messages as a";
            sql_f1.SelectCommand += " inner join status_messages_user_like as b on a.id=b.smid";
            sql_f1.SelectCommand += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            sql_f1.SelectCommand += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            sql_f1.DataBind();
            ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
                            sql_f = new SqlDataSource();
                            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_f.SelectCommand = "select username,photo";
                            sql_f.SelectCommand += " from user_login";
                            sql_f.SelectCommand += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                            if (ict_f.Count > 0)
                            {
                                othfri = ict_f.Table.Rows[0]["username"].ToString();
                            }
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
    [WebMethod(EnableSession = true)]
    public static reporter_table report_build(string param1, string param2)
    {

        reporter_table results = new reporter_table();


        HttpContext.Current.Session["deal_id"] = param2;
        string uid = "", selectdate = "", check_success = "", photo = "", username = "", howtoget_there = "", login_name = "";
        string cutstr_h = "", cutstr_h1 = "";
        int ind_h = 0;
        List<string> kidlist = new List<string>();
        DateTime todate = new DateTime();
        string week = "";
        List<count_money> coumonlist = new List<count_money>();
        count_money coumon = new count_money();

        //単発
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.money_hour,c.hour,c.total_money,f.login_name,c.uisccid,a.id,c.appid,c.uid,f.photo,f.username,a.type,a.check_success,e.year,e.month,e.day,e.start_hour,e.start_minute,e.end_hour,e.end_minute,c.howtoget_there from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_appointment as c on b.uaid=c.id";
        sql_f.SelectCommand += " inner join appointment as e on c.appid=e.id ";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=0";
        sql_f.SelectCommand += " order by e.day asc,a.first_check_time asc,c.uid asc;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        if (ict_f.Count > 0)
        {
            uid = ict_f.Table.Rows[0]["uid"].ToString();
            login_name = ict_f.Table.Rows[0]["login_name"].ToString();

            for (int i = 0; i < ict_f.Count; i++)
            {
                todate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()));
                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = " <br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + ict_f.Table.Rows[i]["month"].ToString() + " 月 " + ict_f.Table.Rows[i]["day"].ToString() + " 日, ";
                selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";

                howtoget_there = "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[i]["username"].ToString() + "</span><br/>";

                check_success = ict_f.Table.Rows[i]["check_success"].ToString();

                //add kid money
                coumon = new count_money();
                coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + week + @"</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                coumonlist.Add(coumon);



            }
        }

        //select 定期 one day
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,f.login_name,d.uisccid,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.type='1' and a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=0";
        sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
        sql_f.DataBind();
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        if (ict_f.Count > 0)
        {
            uid = ict_f.Table.Rows[0]["uid"].ToString();
            login_name = ict_f.Table.Rows[0]["login_name"].ToString();
            for (int i = 0; i < ict_f.Count; i++)
            {
                DateTime.TryParse(ict_f.Table.Rows[i]["start_date"].ToString(), out todate);
                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + todate.Month + " 月 " + todate.Day + " 日, ";
                selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";

                howtoget_there = "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[i]["username"].ToString() + "</span><br/>";

                check_success = ict_f.Table.Rows[i]["check_success"].ToString();


                //add kid money
                coumon = new count_money();
                coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + week + @"</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                coumonlist.Add(coumon);


            }
        }

        //select 定期 more than one day
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select a.id,f.login_name,d.uisccid,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f1.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f1.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f1.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f1.SelectCommand += " inner join user_login as f on f.id=a.uid";
        sql_f1.SelectCommand += " where a.type='2' and a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=0";
        sql_f1.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,a.id asc;";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);


        if (ict_f1.Count > 0)
        {
            uid = ict_f1.Table.Rows[0]["uid"].ToString();
            login_name = ict_f1.Table.Rows[0]["login_name"].ToString();
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);

                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";

                DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                selectdate += todate.Month + " 月 " + todate.Day + " 日</span><br/>";

                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,d.uisccid,g.week_of_day_jp,g.week_of_day,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
                sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid";
                sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid";
                sql_f.SelectCommand += " where a.id='" + param2 + "' and a.type='2' and a.suppid='" + param1 + "'";
                sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
                sql_f.DataBind();
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                string temp_uiswaid = "", temp_uid = "";
                bool check_same = false;
                if (ict_f.Count > 0)
                {
                    for (int i = 0; i < ict_f.Count; i++)
                    {

                        check_same = false;
                        if (i == 0)
                        {
                            temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                            temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                            check_same = true;
                        }
                        else
                        {
                            if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                            {
                                temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                check_same = true;
                            }
                        }
                        if (check_same)
                        {
                            week = ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + "曜日";

                            selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", ";
                            selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                        }
                    }
                }


                howtoget_there = "<br/><span>" + ict_f1.Table.Rows[ict_f1.Count - 1]["howtoget_there"].ToString() + "</span><br/><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f1.Table.Rows[ict_f1.Count - 1]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "</span><br/>";

                check_success = ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString();

            }
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,d.uisccid,g.week_of_day_jp,g.week_of_day,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
            sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
            sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
            sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
            sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid";
            sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid";
            sql_f.SelectCommand += " where a.id='" + param2 + "' and a.type='2' and a.suppid='" + param1 + "'";
            sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    //add kid money
                    coumon = new count_money();
                    coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                    coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + @"曜日</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                    coumonlist.Add(coumon);
                }
            }
        }


        //search user information
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,phone_number,other_phone_number,relationship";
        sql_f.SelectCommand += " from user_information where uid='" + uid + "';";
        sql_f.DataBind();
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        string information_pan = "", infor_pan_money = "";
        if (ict_f.Count > 0)
        {
            information_pan += @"<table width='100%' height='100%' style='background-color:white;'>
                            <tr><td width='5%' height='20px'></td><td width='90%' height='20px'></td><td width='5%' height='20px'></td></tr>
                            <tr><td width='5%'></td><td width='90%'>
            ";
            //user photo
            information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='left' valign='top'>" + photo + @"
</td><td width='10%'></td></tr>
</table>";
            //user name
            information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='left' valign='top'>";
            information_pan += username;
            information_pan += "<br/><span style='text-align: center;font-size:large;'>" + ict_f.Table.Rows[0]["real_first_name"].ToString() + "  </span>&nbsp;&nbsp;";
            information_pan += "<span style='text-align: center;font-size:large;'>  " + ict_f.Table.Rows[0]["real_second_name"].ToString() + "</span>";

            information_pan += "<br/><span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[0]["real_spell_first_name"].ToString() + "  </span>&nbsp;&nbsp;";
            information_pan += "<span style='text-align: center;font-size:medium;'>  " + ict_f.Table.Rows[0]["real_spell_second_name"].ToString() + "</span>";

            information_pan += @"<br/><br/></td><td width='10%'></td></tr>
</table>";

            //user information
            information_pan += @"<table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>メールアドレス</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + login_name + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>電話</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f.Table.Rows[0]["phone_number"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>緊急連絡先</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f.Table.Rows[0]["relationship"].ToString() + @"</span>&nbsp;&nbsp;
<span>" + ict_f.Table.Rows[0]["other_phone_number"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>


</table>";

            //search user kid
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id";
            sql_f.SelectCommand += " from user_information_school_children where uid='" + uid + "';";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    kidlist.Add(ict_f.Table.Rows[i]["id"].ToString());

                }
            }


            //uid = ict_f.Table.Rows[0]["uid"].ToString();

            string kidtotal = "";
            for (int i = 0; i < kidlist.Count; i++)
            {
                sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,Sex,birthday_year,birthday_month,birthday_day,school_name,hospital_name,sick_name";
                sql_f1.SelectCommand += " from user_information_school_children";
                sql_f1.SelectCommand += " where uid='" + uid + "' and id='" + kidlist[i] + "';";
                sql_f1.DataBind();
                ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    for (int ii = 0; ii < ict_f1.Count; ii++)
                    {
                        string sexstr = "";
                        if (ict_f1.Table.Rows[ii]["Sex"].ToString() == "0")
                        {
                            sexstr += "女の子";
                        }
                        else
                        {
                            sexstr += "男の子";
                        }
                        int byear = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_year"].ToString());
                        int bmon = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_month"].ToString());
                        int bday = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_day"].ToString());

                        int toyear = DateTime.Now.Year;
                        int tomon = DateTime.Now.Month;

                        int comyear = toyear - byear;
                        int common = tomon - bmon;

                        if (common < 0)
                        {
                            comyear -= 1;
                            common += 12;
                        }
                        string yearsold = comyear + " 歳 " + common + "ヶ月";
                        //お子様の情報
                        information_pan += @"<table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の名前</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span style='font-size:medium;'>" + ict_f1.Table.Rows[ii]["real_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span style='font-size:medium;'>  " + ict_f1.Table.Rows[ii]["real_second_name"].ToString() + @"</span>

<br/><span>" + ict_f1.Table.Rows[ii]["real_spell_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span>  " + ict_f1.Table.Rows[ii]["real_spell_second_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の年齢</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + yearsold + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の性别</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + sexstr + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様のかかりつけ病院</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f1.Table.Rows[ii]["hospital_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>預け時の留意点</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f1.Table.Rows[ii]["sick_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

</table>";
                        //add kid total element
                        kidtotal += @"<br/><div style='border-style:double;border-color:black;'><table width='100%' style='border-style:dashed;'><td >
<span style='font-size:medium;'>" + ict_f1.Table.Rows[ii]["real_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span style='font-size:medium;'>  " + ict_f1.Table.Rows[ii]["real_second_name"].ToString() + @"</span>

<br/><span>" + ict_f1.Table.Rows[ii]["real_spell_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span>  " + ict_f1.Table.Rows[ii]["real_spell_second_name"].ToString() + @"</span><br/>
</td></table>";

                        for (int iii = 0; iii < coumonlist.Count; iii++)
                        {
                            if (coumonlist[iii].uisccid == kidlist[i])
                            {
                                kidtotal += coumonlist[iii].ele;
                            }
                        }
                        kidtotal += "</div>";

                    }
                }




            }

            //
            information_pan += @"<br/><br/><br/><table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>依頼内容</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + howtoget_there + @"<br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>利用日時</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + selectdate + @"<br/>
</td><td width='20%'></td></tr>
</table>";
            //calendar
            information_pan += @"";
            //total money
            infor_pan_money += @"
<table width='100%'>
<tr><td width='5%'></td><td width='90%'>
<table width='100%'>
<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>お支払い</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<br/><br/>" + kidtotal + @"<br/>
</td><td width='20%'></td></tr>
</table>
</td><td width='5%'></td></tr>
</table>
";



            information_pan += @"</td><td width='5%'></td></tr>
<tr><td width='5%' height='10%'></td><td width='90%' height='10%'></td><td width='5%' height='10%'></td></tr>
</table>";
        }
        string date_pan = "<h3 style='color:#ea9494;'><span>【依頼内容】</span><br/>";
        date_pan += selectdate;
        date_pan += "<br/><span>にお迎えをお願いします。</span></h3>";
        results.infor_pan = information_pan;
        results.date_pan = date_pan;


        results.infor_pan_money = infor_pan_money;
        return results;
    }
    public class reporter_table
    {
        public string infor_pan = "";
        public string date_pan = "";
        public string infor_pan_money = "";
    }
    protected void Calendar2_DayRender(object sender, DayRenderEventArgs e)
    {
        e.Cell.BackColor = System.Drawing.Color.White;
        e.Cell.Controls.Clear();
        Label date = new Label();
        date.Text = e.Day.DayNumberText;
        e.Cell.Controls.Add(date);
        if (Session["deal_id"] != null && Session["id"] != null)
        {
            DateTime todate = new DateTime();

            string startdate = "", enddate = "";
            List<int> weeklist = new List<int>();
            List<string> startlist = new List<string>();
            List<string> endlist = new List<string>();

            //単発
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select e.year,e.month,e.day,e.start_hour,e.start_minute,e.end_hour,e.end_minute from user_information_appointment_check_deal as a";
            sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
            sql_f.SelectCommand += " inner join user_appointment as c on b.uaid=c.id";
            sql_f.SelectCommand += " inner join appointment as e on c.appid=e.id ";
            sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
            sql_f.SelectCommand += " where a.suppid='" + Session["id"].ToString() + "' and a.id='" + Session["deal_id"].ToString() + "'";
            sql_f.SelectCommand += " order by e.day asc,a.first_check_time asc,c.uid asc;";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    todate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()));
                    startdate = todate.Year.ToString() + "-" + todate.Month.ToString() + "-" + todate.Day.ToString();

                    startlist.Add(ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString());
                    endlist.Add(ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString());

                }
            }

            //select 定期 one day
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute";
            sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
            sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
            sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
            sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
            sql_f.SelectCommand += " where a.type='1' and a.suppid='" + Session["id"].ToString() + "' and a.id='" + Session["deal_id"].ToString() + "'";
            sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    DateTime.TryParse(ict_f.Table.Rows[i]["start_date"].ToString(), out todate);
                    startdate = todate.Year.ToString() + "-" + todate.Month.ToString() + "-" + todate.Day.ToString();

                    startlist.Add(ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString());
                    endlist.Add(ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString());

                }
            }

            //select 定期 more than one day
            SqlDataSource sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select f.login_name,d.uisccid,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
            sql_f1.SelectCommand += " from user_information_appointment_check_deal as a";
            sql_f1.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
            sql_f1.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
            sql_f1.SelectCommand += " inner join user_login as f on f.id=a.uid";
            sql_f1.SelectCommand += " where a.type='2' and a.suppid='" + Session["id"].ToString() + "' and a.id='" + Session["deal_id"].ToString() + "'";
            sql_f1.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,a.id asc;";
            sql_f1.DataBind();
            DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);


            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);
                    startdate = todate.Year.ToString() + "-" + todate.Month.ToString() + "-" + todate.Day.ToString();
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                    enddate = todate.Year.ToString() + "-" + todate.Month.ToString() + "-" + todate.Day.ToString();


                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select g.week_of_day_jp,g.week_of_day,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                    sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
                    sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                    sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                    sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
                    sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid ";
                    sql_f.SelectCommand += " where a.id='" + Session["deal_id"].ToString() + "' and a.type='2' and a.suppid='" + Session["id"].ToString() + "'";
                    sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
                    sql_f.DataBind();
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                    string temp_uiswaid = "", temp_uid = "";
                    bool check_same = false;
                    if (ict_f.Count > 0)
                    {
                        for (int i = 0; i < ict_f.Count; i++)
                        {
                            check_same = false;
                            if (i == 0)
                            {
                                check_same = true;
                            }
                            else
                            {
                                if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                                {
                                    check_same = true;
                                }
                            }
                            if (check_same)
                            {
                                weeklist.Add(Convert.ToInt32(ict_f.Table.Rows[i]["week_of_day"].ToString()));
                                startlist.Add(ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString());
                                endlist.Add(ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString());
                            }
                        }
                    }


                }
            }


            //day show
            if (enddate == "")
            {
                DateTime.TryParse(startdate, out todate);
                if (e.Day.Date == todate)
                {

                    Label table_top = new Label();
                    table_top.Text = "</br>";
                    table_top.Text += "<table width='100%' height='100%' style='border-bottom: 1px solid #000;' ><tr><td align='center'>";
                    Panel date_click = new Panel();

                    date_click.CssClass = "button_have";

                    Label table_down = new Label();
                    table_down.Text = "</td></tr></table>";
                    e.Cell.Controls.Add(table_top);
                    Label laa = new Label();
                    string txt = startlist[0];
                    txt += @"</br>|</br>" + endlist[0];

                    laa.Text = txt;
                    date_click.Controls.Add(laa);
                    e.Cell.Controls.Add(date_click);

                    e.Cell.Controls.Add(table_down);
                }
                else
                {
                    Label table_top = new Label();
                    table_top.Text = "</br>";
                    table_top.Text += "<table width='100%' height='100%' style='border-bottom: 1px solid #000;'><tr><td>";
                    Image im = new Image();
                    im.ImageUrl = "~/images/weekend.PNG";
                    im.Attributes.Add("width", "100%");
                    im.Attributes.Add("height", "100%");
                    Label table_down = new Label();
                    table_down.Text = "</td></tr></table>";
                    e.Cell.Controls.Add(table_top);
                    e.Cell.Controls.Add(im);
                    e.Cell.Controls.Add(table_down);
                }
            }
            else
            {
                int weekday = 0;
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
                DateTime.TryParse(startdate, out todate);
                DateTime enddate_check = new DateTime();
                DateTime.TryParse(enddate, out enddate_check);
                if (e.Day.Date >= todate && e.Day.Date <= enddate_check)
                {
                    bool check_week_day = false;
                    string strat = "", end = "";
                    for (int i = 0; i < weeklist.Count; i++)
                    {
                        if (weeklist[i] == weekday)
                        {
                            check_week_day = true;
                            strat = startlist[i];
                            end = endlist[i];
                        }
                    }
                    if (check_week_day)
                    {
                        Label table_top = new Label();
                        table_top.Text = "</br>";
                        table_top.Text += "<table width='100%' height='100%' style='border-bottom: 1px solid #000;' ><tr><td align='center'>";
                        Panel date_click = new Panel();

                        date_click.CssClass = "button_have";

                        Label table_down = new Label();
                        table_down.Text = "</td></tr></table>";
                        e.Cell.Controls.Add(table_top);
                        Label laa = new Label();
                        string txt = strat;
                        txt += @"</br>|</br>" + end;

                        laa.Text = txt;
                        date_click.Controls.Add(laa);
                        e.Cell.Controls.Add(date_click);

                        e.Cell.Controls.Add(table_down);
                    }
                    else
                    {
                        Label table_top = new Label();
                        table_top.Text = "</br>";
                        table_top.Text += "<table width='100%' height='100%' style='border-bottom: 1px solid #000;'><tr><td>";
                        Image im = new Image();
                        im.ImageUrl = "~/images/weekend.PNG";
                        im.Attributes.Add("width", "100%");
                        im.Attributes.Add("height", "100%");
                        Label table_down = new Label();
                        table_down.Text = "</td></tr></table>";
                        e.Cell.Controls.Add(table_top);
                        e.Cell.Controls.Add(im);
                        e.Cell.Controls.Add(table_down);
                    }


                }
                else
                {
                    Label table_top = new Label();
                    table_top.Text = "</br>";
                    table_top.Text += "<table width='100%' height='100%' style='border-bottom: 1px solid #000;'><tr><td>";
                    Image im = new Image();
                    im.ImageUrl = "~/images/weekend.PNG";
                    im.Attributes.Add("width", "100%");
                    im.Attributes.Add("height", "100%");
                    Label table_down = new Label();
                    table_down.Text = "</td></tr></table>";
                    e.Cell.Controls.Add(table_top);
                    e.Cell.Controls.Add(im);
                    e.Cell.Controls.Add(table_down);
                }

            }




        }

    }
    public class count_money
    {
        public string uisccid = "";
        public string ele = "";
    }
    [WebMethod(EnableSession = true)]
    public static string report_build_success(string param1)
    {
        string results = "";


        //HttpContext.Current.Session["deal_id"] = param2;
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;



        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.UpdateCommand = "update user_information_appointment_check_deal set check_success='1',second_check_time='" + start + "'";
        sql_f.UpdateCommand += " where id='" + param1 + "';";
        sql_f.Update();

        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select uid,suppid from user_information_appointment_check_deal";
        sql_f.SelectCommand += " where id='" + param1 + "';";
        sql_f.DataBind();

        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {
            SqlDataSource sql_insert = new SqlDataSource();
            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','【預かりを承認しました】','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
            sql_insert.Insert();
        }


        return results;
    }
    [WebMethod(EnableSession = true)]
    public static reporter_table_success report_build_report(string param1, string param2)
    {

        reporter_table_success results = new reporter_table_success();


        HttpContext.Current.Session["deal_id"] = param2;
        string uid = "", selectdate = "", check_success = "", photo = "", username = "", howtoget_there = "", login_name = "";
        string cutstr_h = "", cutstr_h1 = "";
        int ind_h = 0;
        List<string> kidlist = new List<string>();
        DateTime todate = new DateTime();
        string week = "";
        List<count_money> coumonlist = new List<count_money>();
        count_money coumon = new count_money();

        //単発
        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select c.money_hour,c.hour,c.total_money,f.login_name,c.uisccid,a.id,c.appid,c.uid,f.photo,f.username,a.type,a.check_success,e.year,e.month,e.day,e.start_hour,e.start_minute,e.end_hour,e.end_minute,c.howtoget_there from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_appointment as c on b.uaid=c.id";
        sql_f.SelectCommand += " inner join appointment as e on c.appid=e.id ";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=1";
        sql_f.SelectCommand += " order by e.day asc,a.first_check_time asc,c.uid asc;";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        if (ict_f.Count > 0)
        {
            uid = ict_f.Table.Rows[0]["uid"].ToString();
            login_name = ict_f.Table.Rows[0]["login_name"].ToString();

            for (int i = 0; i < ict_f.Count; i++)
            {
                todate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()));
                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = " <br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + ict_f.Table.Rows[i]["month"].ToString() + " 月 " + ict_f.Table.Rows[i]["day"].ToString() + " 日, ";
                selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";

                howtoget_there = "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[i]["username"].ToString() + "</span><br/>";

                check_success = ict_f.Table.Rows[i]["check_success"].ToString();

                //add kid money
                coumon = new count_money();
                coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + week + @"</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                coumonlist.Add(coumon);



            }
        }

        //select 定期 one day
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,f.login_name,d.uisccid,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
        sql_f.SelectCommand += " where a.type='1' and a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=1";
        sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
        sql_f.DataBind();
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

        if (ict_f.Count > 0)
        {
            uid = ict_f.Table.Rows[0]["uid"].ToString();
            login_name = ict_f.Table.Rows[0]["login_name"].ToString();
            for (int i = 0; i < ict_f.Count; i++)
            {
                DateTime.TryParse(ict_f.Table.Rows[i]["start_date"].ToString(), out todate);
                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + todate.Month + " 月 " + todate.Day + " 日, ";
                selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";

                howtoget_there = "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[i]["username"].ToString() + "</span><br/>";

                check_success = ict_f.Table.Rows[i]["check_success"].ToString();


                //add kid money
                coumon = new count_money();
                coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + week + @"</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                coumonlist.Add(coumon);


            }
        }

        //select 定期 more than one day
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select a.id,f.login_name,d.uisccid,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        sql_f1.SelectCommand += " from user_information_appointment_check_deal as a";
        sql_f1.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        sql_f1.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        sql_f1.SelectCommand += " inner join user_login as f on f.id=a.uid";
        sql_f1.SelectCommand += " where a.type='2' and a.suppid='" + param1 + "' and a.id='" + param2 + "' and check_success=1";
        sql_f1.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,a.id asc;";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);


        if (ict_f1.Count > 0)
        {
            uid = ict_f1.Table.Rows[0]["uid"].ToString();
            login_name = ict_f1.Table.Rows[0]["login_name"].ToString();
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);

                if (todate.DayOfWeek == DayOfWeek.Monday)
                {
                    week = "月曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    week = "火曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    week = "水曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Thursday)
                {
                    week = "木曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Friday)
                {
                    week = "金曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Saturday)
                {
                    week = "土曜日";
                }
                else if (todate.DayOfWeek == DayOfWeek.Sunday)
                {
                    week = "日曜日";
                }
                selectdate = "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";

                DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                selectdate += todate.Month + " 月 " + todate.Day + " 日</span><br/>";

                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,d.uisccid,g.week_of_day_jp,g.week_of_day,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
                sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid";
                sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid";
                sql_f.SelectCommand += " where a.id='" + param2 + "' and a.type='2' and a.suppid='" + param1 + "'";
                sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
                sql_f.DataBind();
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                string temp_uiswaid = "", temp_uid = "";
                bool check_same = false;
                if (ict_f.Count > 0)
                {
                    for (int i = 0; i < ict_f.Count; i++)
                    {

                        check_same = false;
                        if (i == 0)
                        {
                            temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                            temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                            check_same = true;
                        }
                        else
                        {
                            if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                            {
                                temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                check_same = true;
                            }
                        }
                        if (check_same)
                        {
                            week = ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + "曜日";

                            selectdate += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", ";
                            selectdate += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                        }
                    }
                }


                howtoget_there = "<br/><span>" + ict_f1.Table.Rows[ict_f1.Count - 1]["howtoget_there"].ToString() + "</span><br/><br/>";

                //user photo
                photo = "<div class='zoom-gallery'>";
                cutstr_h = ict_f1.Table.Rows[ict_f1.Count - 1]["photo"].ToString();
                ind_h = cutstr_h.IndexOf(@"/");
                cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
                photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                photo += "</a>";
                photo += "</div>";

                username = "<span style='text-align: center;font-size:medium;'>" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "</span><br/>";

                check_success = ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString();

            }
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,d.uisccid,g.week_of_day_jp,g.week_of_day,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
            sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
            sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
            sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
            sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid";
            sql_f.SelectCommand += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid";
            sql_f.SelectCommand += " where a.id='" + param2 + "' and a.type='2' and a.suppid='" + param1 + "'";
            sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    //add kid money
                    coumon = new count_money();
                    coumon.uisccid = ict_f.Table.Rows[i]["uisccid"].ToString();
                    coumon.ele = @"<br/><br/><table width='100%' style='border-bottom: 1px solid black;'>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + @"曜日</span><br/><span>" + ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + @"</span><br/><br/>
</td>
<td width='10%'>
</td>
<td width='40%'>
</td>
</tr>
<tr>
<td width='50%'><span>" + ict_f.Table.Rows[i]["money_hour"].ToString() + @"円</span>x
<span>" + ict_f.Table.Rows[i]["hour"].ToString() + @"時間</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
<br/>
<table width='100%'>
<tr>
<td width='50%'><span>利益</span><br/>
</td>
<td width='10%'></td>
<td width='40%'>
<span>¥" + ict_f.Table.Rows[i]["total_money"].ToString() + @"円</span><br/>
</td>
</tr>
</table>
";

                    coumonlist.Add(coumon);
                }
            }
        }


        //search user information
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,phone_number,other_phone_number,relationship";
        sql_f.SelectCommand += " from user_information where uid='" + uid + "';";
        sql_f.DataBind();
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        string information_pan = "", infor_pan_money = "";
        if (ict_f.Count > 0)
        {
            information_pan += @"<table width='100%' height='100%' style='background-color:white;'>
                            <tr><td width='5%' height='20px'></td><td width='90%' height='20px'></td><td width='5%' height='20px'></td></tr>
                            <tr><td width='5%'></td><td width='90%'>
            ";
            //user photo
            information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='left' valign='top'>" + photo + @"
</td><td width='10%'></td></tr>
</table>";
            //user name
            information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='left' valign='top'>";
            information_pan += username;
            information_pan += "<br/><span style='text-align: center;font-size:large;'>" + ict_f.Table.Rows[0]["real_first_name"].ToString() + "  </span>&nbsp;&nbsp;";
            information_pan += "<span style='text-align: center;font-size:large;'>  " + ict_f.Table.Rows[0]["real_second_name"].ToString() + "</span>";

            information_pan += "<br/><span style='text-align: center;font-size:medium;'>" + ict_f.Table.Rows[0]["real_spell_first_name"].ToString() + "  </span>&nbsp;&nbsp;";
            information_pan += "<span style='text-align: center;font-size:medium;'>  " + ict_f.Table.Rows[0]["real_spell_second_name"].ToString() + "</span>";

            information_pan += @"<br/><br/></td><td width='10%'></td></tr>
</table>";

            //user information
            information_pan += @"<table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>メールアドレス</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + login_name + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>電話</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f.Table.Rows[0]["phone_number"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>緊急連絡先</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f.Table.Rows[0]["relationship"].ToString() + @"</span>&nbsp;&nbsp;
<span>" + ict_f.Table.Rows[0]["other_phone_number"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>


</table>";

            //search user kid
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id";
            sql_f.SelectCommand += " from user_information_school_children where uid='" + uid + "';";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    kidlist.Add(ict_f.Table.Rows[i]["id"].ToString());

                }
            }


            //uid = ict_f.Table.Rows[0]["uid"].ToString();

            string kidtotal = "";
            for (int i = 0; i < kidlist.Count; i++)
            {
                sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,Sex,birthday_year,birthday_month,birthday_day,school_name,hospital_name,sick_name";
                sql_f1.SelectCommand += " from user_information_school_children";
                sql_f1.SelectCommand += " where uid='" + uid + "' and id='" + kidlist[i] + "';";
                sql_f1.DataBind();
                ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    for (int ii = 0; ii < ict_f1.Count; ii++)
                    {
                        string sexstr = "";
                        if (ict_f1.Table.Rows[ii]["Sex"].ToString() == "0")
                        {
                            sexstr += "女の子";
                        }
                        else
                        {
                            sexstr += "男の子";
                        }
                        int byear = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_year"].ToString());
                        int bmon = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_month"].ToString());
                        int bday = Convert.ToInt32(ict_f1.Table.Rows[ii]["birthday_day"].ToString());

                        int toyear = DateTime.Now.Year;
                        int tomon = DateTime.Now.Month;

                        int comyear = toyear - byear;
                        int common = tomon - bmon;

                        if (common < 0)
                        {
                            comyear -= 1;
                            common += 12;
                        }
                        string yearsold = comyear + " 歳 " + common + "ヶ月";
                        //お子様の情報
                        information_pan += @"<table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の名前</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span style='font-size:medium;'>" + ict_f1.Table.Rows[ii]["real_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span style='font-size:medium;'>  " + ict_f1.Table.Rows[ii]["real_second_name"].ToString() + @"</span>

<br/><span>" + ict_f1.Table.Rows[ii]["real_spell_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span>  " + ict_f1.Table.Rows[ii]["real_spell_second_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の年齢</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + yearsold + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様の性别</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + sexstr + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>お子様のかかりつけ病院</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f1.Table.Rows[ii]["hospital_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#9999A8;'>預け時の留意点</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<span>" + ict_f1.Table.Rows[ii]["sick_name"].ToString() + @"</span><br/><br/>
</td><td width='20%'></td></tr>

</table>";
                        //add kid total element
                        kidtotal += @"<br/><div style='border-style:double;border-color:black;'><table width='100%' style='border-style:dashed;'><td >
<span style='font-size:medium;'>" + ict_f1.Table.Rows[ii]["real_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span style='font-size:medium;'>  " + ict_f1.Table.Rows[ii]["real_second_name"].ToString() + @"</span>

<br/><span>" + ict_f1.Table.Rows[ii]["real_spell_first_name"].ToString() + @"  </span>&nbsp;&nbsp;
<span>  " + ict_f1.Table.Rows[ii]["real_spell_second_name"].ToString() + @"</span><br/>
</td></table>";

                        for (int iii = 0; iii < coumonlist.Count; iii++)
                        {
                            if (coumonlist[iii].uisccid == kidlist[i])
                            {
                                kidtotal += coumonlist[iii].ele;
                            }
                        }
                        kidtotal += "</div>";

                    }
                }




            }

            //
            information_pan += @"<br/><br/><br/><table width='100%'>

<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>依頼内容</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + howtoget_there + @"<br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>利用日時</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + selectdate + @"<br/>
</td><td width='20%'></td></tr>
</table>";
            //calendar
            information_pan += @"";
            //total money
            infor_pan_money += @"
<table width='100%'>
<tr><td width='5%'></td><td width='90%'>
<table width='100%'>
<tr><td width='80%' colspan='2'>
<span style='color:#EA9494;font-weight:bold;'>お支払い</span>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<br/><br/>" + kidtotal + @"<br/>
</td><td width='20%'></td></tr>
</table>
</td><td width='5%'></td></tr>
</table>
";



            information_pan += @"</td><td width='5%'></td></tr>
<tr><td width='5%' height='10%'></td><td width='90%' height='10%'></td><td width='5%' height='10%'></td></tr>
</table>";
        }
        string date_pan = "<h3><span>お預かりお疲れ様でした！</span><br/>";
        date_pan += @"<br/><span>簡単な報告書を書いて依頼を完了させましょう。</span></h3><br/><br/>
<table width='100%'>
<tr><td width='100%' align='center'>
<button type='button' onclick='report_create_page()' style='width: 50%;text-shadow: none;cursor: pointer;text-align: center;' class='file-upload'>報告書を作成する</button>
</td></tr>
</table>";
        results.infor_pan = information_pan;
        results.date_pan = date_pan;


        results.date_pan_make = @"
<table width='100%'>
<tr><td width='100%' align='center'>
<h3><span style='color:#EA9494;font-weight:bold;'>報告書</span></h3>
</td></tr>
</table>
<br/><br/><table width='100%'>

<tr><td width='80%' colspan='2'>
<h3><span style='font-weight:bold;'>預かり日時</span></h3>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + selectdate + @"<br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<h3><span style='font-weight:bold;'>預かり内容</span></h3>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
" + howtoget_there + @"<br/>
</td><td width='20%'></td></tr>

<tr><td width='80%' colspan='2'>
<h3><span style='font-weight:bold;'>預かり時の様子</span></h3>
</td><td width='20%'></td></tr>
<tr><td width='5%'></td><td width='75%'>
<textarea name='success_text' rows='5' cols='30' wrap='hard' id='success_text' class='textbox' placeholder='例）とっても元気いっぱいで、いっしょに歌って遊んでました。お昼寝もしっかり２時間とれました。汗をかいたので２回お着替えしてもらいました。体温も１時間おきに計りましたが、ずっと平熱でした。' style='height:100px;width:100%;'></textarea><br/>
</td><td width='20%'></td></tr>

</table>
<br/><br/>
<table width='100%'>
<tr><td width='100%' align='center'>
<button type='button' onclick='report_create_success()' style='width: 50%;text-shadow: none;cursor: pointer;text-align: center;' class='file-upload'>報告書を送付</button>
</td></tr>
</table>
";


        results.infor_pan_money = infor_pan_money;
        return results;
    }
    public class reporter_table_success
    {
        public string infor_pan = "";
        public string date_pan = "";
        public string infor_pan_money = "";
        public string date_pan_make = "";
    }
    [WebMethod(EnableSession = true)]
    public static string report_build_success_content(string param1, string param2)
    {
        string results = "";
        string check_content = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        //HttpContext.Current.Session["deal_id"] = param2;
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;



        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.UpdateCommand = "update user_information_appointment_check_deal set check_success='3',content_success='" + check_content + "',third_check_time='" + start + "'";
        sql_f.UpdateCommand += " where id='" + param1 + "';";
        sql_f.Update();

        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select uid,suppid from user_information_appointment_check_deal";
        sql_f.SelectCommand += " where id='" + param1 + "';";
        sql_f.DataBind();

        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        string ActivationCode = "", res = "";
        if (ict_f.Count > 0)
        {
            SqlDataSource sql_f1 = new SqlDataSource();
            sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f1.SelectCommand = "select ActivationCode from user_information_store_week_appointment_check_check";
            sql_f1.SelectCommand += " where uiacdid='" + param1 + "';";
            sql_f1.DataBind();
            DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
            if (ict_f1.Count > 0)
            {
                ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_date_manger.aspx/report_build_success_content", "makescore_w.aspx?ActivationCode=" + ActivationCode);

            }
            else
            {
                sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select ActivationCode from user_appointment_check";
                sql_f1.SelectCommand += " where uiacdid='" + param1 + "';";
                sql_f1.DataBind();
                ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                    res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_date_manger.aspx/report_build_success_content", "makescore.aspx?ActivationCode=" + ActivationCode);
                }
            }

            SqlDataSource sql_insert = new SqlDataSource();
            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認用URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
            sql_insert.Insert();
            //<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week(param1, param2, dealid) + ">確認用URL</a></li>
            results += "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            results += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
        }


        return results;
    }
    [WebMethod(EnableSession = true)]
    public static string report_build_fail(string param1, string param2, string param3)
    {
        string results = "";
        results = " " + param1 + "," + param2 + "," + param3;
        string check_content = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        //HttpContext.Current.Session["deal_id"] = param2;
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;



        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.UpdateCommand = "update user_information_appointment_check_deal set check_success='2',choice_fail='" + param2 + "',content_fail='" + check_content + "',second_check_time='" + start + "'";
        sql_f.UpdateCommand += " where id='" + param1 + "';";
        sql_f.Update();

        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select uid,suppid from user_information_appointment_check_deal";
        sql_f.SelectCommand += " where id='" + param1 + "';";
        sql_f.DataBind();

        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f.Count > 0)
        {
            SqlDataSource sql_insert = new SqlDataSource();
            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','申し訳ありませんが、" + param2 + " " + check_content + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "');";
            sql_insert.Insert();
            //<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week(param1, param2, dealid) + ">確認用URL</a></li>
            //results += "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            //results += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','申し訳ありませんが、" + param2 + " " + check_content + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
        }


        return results;
    }
    public class Userinformation
    {
        public int id = 0;
        public string loginname = "";
        public string name = "";
        public string photo = "";
        public string talkmess = "";
        public int year = 0;
        public string month = "";
        public string day = "";
        public string hour = "";
        public string min = "";
        public string sec = "";

        public DateTime comdate = new DateTime();
    }
    [WebMethod(EnableSession = true)]
    public static string chat_room_mess(string param1, string param2)
    {
        List<Userinformation> takfri = new List<Userinformation>();
        Userinformation uif = new Userinformation();
        string cutstr2, cutstr3;
        int ind2 = 0;
        string result = "";
        string uid = "", suppid = param1;
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select uid,suppid";
        sql_f1.SelectCommand += " from user_information_appointment_check_deal";
        sql_f1.SelectCommand += " where id='" + param2 + "' and suppid='" + suppid + "';";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            uid = ict_f1.Table.Rows[0]["uid"].ToString();
            //i talk to other person
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,b.login_name,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f.SelectCommand += " from user_chat_room as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";
            sql_f.SelectCommand += " where b.id='" + suppid + "' and c.id='" + uid + "'";
            sql_f.SelectCommand += " ORDER BY c.id asc,a.year asc,a.month asc,a.day asc,a.hour asc,a.minute asc,a.second asc;";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ict_f.Count; i++)
            {

                uif = new Userinformation();
                uif.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                uif.loginname = ict_f.Table.Rows[i]["login_name"].ToString();
                cutstr2 = ict_f.Table.Rows[i]["photo"].ToString();
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                uif.photo = cutstr3;
                uif.name = ict_f.Table.Rows[i]["username"].ToString();
                uif.talkmess = ict_f.Table.Rows[i]["talk_message"].ToString();
                uif.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                uif.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));



                if (Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()) < 10)
                {
                    uif.month = "0" + ict_f.Table.Rows[i]["month"].ToString();
                }
                else
                {
                    uif.month = ict_f.Table.Rows[i]["month"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()) < 10)
                {
                    uif.day = "0" + ict_f.Table.Rows[i]["day"].ToString();
                }
                else
                {
                    uif.day = ict_f.Table.Rows[i]["day"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()) < 10)
                {
                    uif.hour = "0" + ict_f.Table.Rows[i]["hour"].ToString();
                }
                else
                {
                    uif.hour = ict_f.Table.Rows[i]["hour"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()) < 10)
                {
                    uif.min = "0" + ict_f.Table.Rows[i]["minute"].ToString();
                }
                else
                {
                    uif.min = ict_f.Table.Rows[i]["minute"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()) < 10)
                {
                    uif.sec = "0" + ict_f.Table.Rows[i]["second"].ToString();
                }
                else
                {
                    uif.sec = ict_f.Table.Rows[i]["second"].ToString();
                }
                takfri.Add(uif);
            }


            //other person talk to me
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select b.id,b.login_name,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f.SelectCommand += " from user_chat_room as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";
            sql_f.SelectCommand += " where b.id='" + uid + "' and c.id='" + suppid + "'";
            sql_f.SelectCommand += " ORDER BY b.id asc,a.year asc,a.month asc,a.day asc,a.hour asc,a.minute asc,a.second asc;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

            for (int i = 0; i < ict_f.Count; i++)
            {
                uif = new Userinformation();
                uif.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                uif.loginname = ict_f.Table.Rows[i]["login_name"].ToString();
                cutstr2 = ict_f.Table.Rows[i]["photo"].ToString();
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                uif.photo = cutstr3;
                uif.name = ict_f.Table.Rows[i]["username"].ToString();
                uif.talkmess = ict_f.Table.Rows[i]["talk_message"].ToString();
                uif.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                uif.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));

                if (Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()) < 10)
                {
                    uif.month = "0" + ict_f.Table.Rows[i]["month"].ToString();
                }
                else
                {
                    uif.month = ict_f.Table.Rows[i]["month"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()) < 10)
                {
                    uif.day = "0" + ict_f.Table.Rows[i]["day"].ToString();
                }
                else
                {
                    uif.day = ict_f.Table.Rows[i]["day"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()) < 10)
                {
                    uif.hour = "0" + ict_f.Table.Rows[i]["hour"].ToString();
                }
                else
                {
                    uif.hour = ict_f.Table.Rows[i]["hour"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()) < 10)
                {
                    uif.min = "0" + ict_f.Table.Rows[i]["minute"].ToString();
                }
                else
                {
                    uif.min = ict_f.Table.Rows[i]["minute"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()) < 10)
                {
                    uif.sec = "0" + ict_f.Table.Rows[i]["second"].ToString();
                }
                else
                {
                    uif.sec = ict_f.Table.Rows[i]["second"].ToString();
                }
                takfri.Add(uif);
            }
            takfri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            for (int i = 0; i < takfri.Count; i++)
            {
                result += "<br/>";
                result += "<table width='100%'><tr><td width='10%' rowspan='2' valign='top'>";
                result += "<div class='zoom-gallery'>";
                result += "<a href='" + takfri[i].photo + "' data-source='" + takfri[i].photo + "' title='" + takfri[i].name + "' style='width:100px;height:100px;'>";
                result += "<img src='" + takfri[i].photo + "' width='100' height='100' />";
                result += "</a>";
                result += "</div>";
                result += "</td><td width='60%'>";
                result += "<span>" + takfri[i].name + "</span>";
                result += "</td><td width='20%'>";
                result += takfri[i].year + "年" + takfri[i].month + "月" + takfri[i].day + "日" + takfri[i].hour + ":" + takfri[i].min + ":" + takfri[i].sec;
                result += "</td></tr><tr><td colspan='2' style='word-break:break-all;'>";
                result += "" + takfri[i].talkmess + "";
                result += "</td></tr></table>";
                result += "<br/>";
            }



        }


        //for (int i = 0; i < ict_f.Count; i++)
        //{
        //    temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
        //}
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string chat_room_mess_send(string param1, string param2, string param3)
    {

        List<Userinformation> takfri = new List<Userinformation>();
        Userinformation uif = new Userinformation();
        string cutstr2, cutstr3;
        int ind2 = 0;
        string result = "";
        string uid = "", suppid = param1;
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select uid,suppid";
        sql_f1.SelectCommand += " from user_information_appointment_check_deal";
        sql_f1.SelectCommand += " where id='" + param2 + "' and suppid='" + suppid + "';";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            uid = ict_f1.Table.Rows[0]["uid"].ToString();
            //insert chat message
            string mess = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            if (mess != "")
            {
                int year, mon, day, hour, min, sec;
                year = DateTime.Now.Year;
                mon = DateTime.Now.Month;
                day = DateTime.Now.Day;
                hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
                min = DateTime.Now.Minute;
                sec = DateTime.Now.Second;
                SqlDataSource sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_chat_room (uid,to_uid,talk_message,year,month,day,hour,minute,second)";
                sql_insert.InsertCommand += " values('" + suppid + "','" + uid + "','" + mess + "','" + year + "','" + mon + "'";
                sql_insert.InsertCommand += ",'" + day + "','" + hour + "','" + min + "','" + sec + "');";
                sql_insert.Insert();
            }

            //i talk to other person
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select c.id,b.login_name,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f.SelectCommand += " from user_chat_room as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";
            sql_f.SelectCommand += " where b.id='" + suppid + "' and c.id='" + uid + "'";
            sql_f.SelectCommand += " ORDER BY c.id asc,a.year asc,a.month asc,a.day asc,a.hour asc,a.minute asc,a.second asc;";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ict_f.Count; i++)
            {

                uif = new Userinformation();
                uif.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                uif.loginname = ict_f.Table.Rows[i]["login_name"].ToString();
                cutstr2 = ict_f.Table.Rows[i]["photo"].ToString();
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                uif.photo = cutstr3;
                uif.name = ict_f.Table.Rows[i]["username"].ToString();
                uif.talkmess = ict_f.Table.Rows[i]["talk_message"].ToString();
                uif.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                uif.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));



                if (Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()) < 10)
                {
                    uif.month = "0" + ict_f.Table.Rows[i]["month"].ToString();
                }
                else
                {
                    uif.month = ict_f.Table.Rows[i]["month"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()) < 10)
                {
                    uif.day = "0" + ict_f.Table.Rows[i]["day"].ToString();
                }
                else
                {
                    uif.day = ict_f.Table.Rows[i]["day"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()) < 10)
                {
                    uif.hour = "0" + ict_f.Table.Rows[i]["hour"].ToString();
                }
                else
                {
                    uif.hour = ict_f.Table.Rows[i]["hour"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()) < 10)
                {
                    uif.min = "0" + ict_f.Table.Rows[i]["minute"].ToString();
                }
                else
                {
                    uif.min = ict_f.Table.Rows[i]["minute"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()) < 10)
                {
                    uif.sec = "0" + ict_f.Table.Rows[i]["second"].ToString();
                }
                else
                {
                    uif.sec = ict_f.Table.Rows[i]["second"].ToString();
                }
                takfri.Add(uif);
            }


            //other person talk to me
            sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select b.id,b.login_name,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f.SelectCommand += " from user_chat_room as a";
            sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
            sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";
            sql_f.SelectCommand += " where b.id='" + uid + "' and c.id='" + suppid + "'";
            sql_f.SelectCommand += " ORDER BY b.id asc,a.year asc,a.month asc,a.day asc,a.hour asc,a.minute asc,a.second asc;";
            sql_f.DataBind();
            ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

            for (int i = 0; i < ict_f.Count; i++)
            {
                uif = new Userinformation();
                uif.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                uif.loginname = ict_f.Table.Rows[i]["login_name"].ToString();
                cutstr2 = ict_f.Table.Rows[i]["photo"].ToString();
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                uif.photo = cutstr3;
                uif.name = ict_f.Table.Rows[i]["username"].ToString();
                uif.talkmess = ict_f.Table.Rows[i]["talk_message"].ToString();
                uif.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                uif.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));

                if (Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()) < 10)
                {
                    uif.month = "0" + ict_f.Table.Rows[i]["month"].ToString();
                }
                else
                {
                    uif.month = ict_f.Table.Rows[i]["month"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()) < 10)
                {
                    uif.day = "0" + ict_f.Table.Rows[i]["day"].ToString();
                }
                else
                {
                    uif.day = ict_f.Table.Rows[i]["day"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()) < 10)
                {
                    uif.hour = "0" + ict_f.Table.Rows[i]["hour"].ToString();
                }
                else
                {
                    uif.hour = ict_f.Table.Rows[i]["hour"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()) < 10)
                {
                    uif.min = "0" + ict_f.Table.Rows[i]["minute"].ToString();
                }
                else
                {
                    uif.min = ict_f.Table.Rows[i]["minute"].ToString();
                }
                if (Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()) < 10)
                {
                    uif.sec = "0" + ict_f.Table.Rows[i]["second"].ToString();
                }
                else
                {
                    uif.sec = ict_f.Table.Rows[i]["second"].ToString();
                }
                takfri.Add(uif);
            }
            takfri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            for (int i = 0; i < takfri.Count; i++)
            {
                result += "<br/>";
                result += "<table width='100%'><tr><td width='10%' rowspan='2' valign='top'>";
                result += "<div class='zoom-gallery'>";
                result += "<a href='" + takfri[i].photo + "' data-source='" + takfri[i].photo + "' title='" + takfri[i].name + "' style='width:100px;height:100px;'>";
                result += "<img src='" + takfri[i].photo + "' width='100' height='100' />";
                result += "</a>";
                result += "</div>";
                result += "</td><td width='60%'>";
                result += "<span>" + takfri[i].name + "</span>";
                result += "</td><td width='20%'>";
                result += takfri[i].year + "年" + takfri[i].month + "月" + takfri[i].day + "日" + takfri[i].hour + ":" + takfri[i].min + ":" + takfri[i].sec;
                result += "</td></tr><tr><td colspan='2' style='word-break:break-all;'>";
                result += "" + takfri[i].talkmess + "";
                result += "</td></tr></table>";
                result += "<br/>";
            }



        }

        return result;
    }
    [WebMethod]
    public static string[] count_list(string param1)
    {
        string result = param1;
        string[] result_res = new string[3];
        result = "";
        //friend post message
        List<string> user_friend = new List<string>();
        SqlDataSource sql_ff = new SqlDataSource();
        sql_ff.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_ff.SelectCommand = "select c.id,c.username,c.photo";
        sql_ff.SelectCommand += " from user_friendship as a";
        sql_ff.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_ff.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_ff.SelectCommand += " where b.id='" + param1.Trim() + "'";
        sql_ff.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        sql_ff.DataBind();
        DataView ict_ff = (DataView)sql_ff.Select(DataSourceSelectArguments.Empty);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        SqlDataSource sql_f1_f = new SqlDataSource();
        sql_f1_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1_f.SelectCommand = "select b.id,b.username,b.photo";
        sql_f1_f.SelectCommand += " from user_friendship as a";
        sql_f1_f.SelectCommand += " inner join user_login as b on b.id=a.first_uid";
        sql_f1_f.SelectCommand += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        sql_f1_f.SelectCommand += " where c.id='" + param1.Trim() + "'";
        sql_f1_f.SelectCommand += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1_f = (DataView)sql_f1_f.Select(DataSourceSelectArguments.Empty);
        if (ict_f1_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f1_f.Count; ii++)
            {
                user_friend.Add(ict_f1_f.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message
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
        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            SqlDataSource sql_f12 = new SqlDataSource();
            sql_f12.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f12.SelectCommand = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            sql_f12.SelectCommand += " from status_messages as a";
            sql_f12.SelectCommand += " where a.uid='" + user_friend[i] + "'";
            sql_f12.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            sql_f12.DataBind();
            DataView ict_f12 = (DataView)sql_f12.Select(DataSourceSelectArguments.Empty);
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
            sql_f12 = new SqlDataSource();
            sql_f12.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f12.SelectCommand = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            sql_f12.SelectCommand += " from status_messages as a";
            sql_f12.SelectCommand += " inner join status_messages_user_like as b on a.id=b.smid";
            sql_f12.SelectCommand += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            sql_f12.SelectCommand += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            sql_f12.DataBind();
            ict_f12 = (DataView)sql_f12.Select(DataSourceSelectArguments.Empty);
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
        DateTime nowtime = DateTime.Now;
        DateTime clicktime = new DateTime(2000, 1, 1);
        SqlDataSource sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select check_time from user_notice_check";
        sql_f1.SelectCommand += " where uid='" + param1 + "' and type='0';";
        sql_f1.DataBind();
        DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
            DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
            if (mesgdate > clicktime && mesgdate < nowtime)
            {
                newmessage += 1;
            }
        }
        result_res[0] = newmessage.ToString();


        //chat list count
        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        sql_f.SelectCommand += " from user_chat_room as a";
        sql_f.SelectCommand += " inner join user_login as b on b.id=a.uid";
        sql_f.SelectCommand += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        sql_f.SelectCommand += " where b.id='" + param1 + "'";
        sql_f.SelectCommand += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

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

        sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        sql_f1.SelectCommand = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        sql_f1.SelectCommand += " from user_chat_room as a";
        sql_f1.SelectCommand += " inner join user_login as b on b.id=a.uid";
        sql_f1.SelectCommand += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        sql_f1.SelectCommand += " where c.id=" + param1 + "";
        sql_f1.SelectCommand += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
        sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select check_time from user_notice_check";
        sql_f1.SelectCommand += " where uid='" + param1 + "' and type='1';";
        sql_f1.DataBind();
        ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
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
        sql_f1 = new SqlDataSource();
        sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f1.SelectCommand = "select check_time from user_notice_check";
        sql_f1.SelectCommand += " where uid='" + param1 + "' and type='2';";
        sql_f1.DataBind();
        ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newfri = 0;
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
    public static void changeicon(string param1)
    {

        if (param1 != null)
        {
            HttpContext.Current.Session["iconid"] = param1;
        }

    }


}
