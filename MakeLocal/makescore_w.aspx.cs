using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class makescore_w : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //user photo and name user_photo
        //預かり日時 sandbox-container
        //預かり内容 report_content
        //預かり時の様子 report_list
        string selectdate = "", photo = "", username = "", howtoget_there = "";
        string cutstr_h = "", cutstr_h1 = "";
        int ind_h = 0;
        List<string> kidlist = new List<string>();
        DateTime todate = new DateTime();
        string week = "";

        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();


        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select uid,supp_id,uiacdid";
        sql_f.SelectCommand += " from user_information_store_week_appointment_check_check";
        sql_f.SelectCommand += " where ActivationCode='" + activationCode + "';";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        string id = "", supid = "", uiacdid = "";
        for (int i = 0; i < ict_f.Count; i++)
        {
            id = ict_f.Table.Rows[i]["uid"].ToString();
            supid = ict_f.Table.Rows[i]["supp_id"].ToString();
            uiacdid = ict_f.Table.Rows[i]["uiacdid"].ToString();
        }
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;
        //now time
        if (Session["id"] != null)
        {
            if (Session["id"].ToString() == id.ToString())
            {
                bool check_again = false;
                //select 定期 one day
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select d.money_hour,d.hour,d.total_money,f.login_name,d.uisccid,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                sql_f.SelectCommand += " from user_information_appointment_check_deal as a";
                sql_f.SelectCommand += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                sql_f.SelectCommand += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                sql_f.SelectCommand += " inner join user_login as f on f.id=a.uid ";
                sql_f.SelectCommand += " where a.type='1' and a.suppid='" + supid + "' and a.id='" + uiacdid + "' and check_success=3";
                sql_f.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,d.uid asc;";
                sql_f.DataBind();
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

                if (ict_f.Count > 0)
                {
                    check_again = true;

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
                sql_f1.SelectCommand += " where a.type='2' and a.suppid='" + supid + "' and a.id='" + uiacdid + "' and check_success=3";
                sql_f1.SelectCommand += " order by DATEPART(day, d.start_date) asc,a.first_check_time asc,a.id asc;";
                sql_f1.DataBind();
                DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);


                if (ict_f1.Count > 0)
                {
                    check_again = true;
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
                        sql_f.SelectCommand += " where a.id='" + uiacdid + "' and a.type='2' and a.suppid='" + supid + "'";
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


                    }

                }

                sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select photo,username";
                sql_f1.SelectCommand += " from user_login";
                sql_f1.SelectCommand += " where id='" + supid + "';";
                sql_f1.DataBind();
                ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    //user photo
                    photo = "<div class='zoom-gallery'>";
                    cutstr_h = ict_f1.Table.Rows[0]["photo"].ToString();
                    ind_h = cutstr_h.IndexOf(@"/");
                    cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                    photo += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[0]["username"].ToString() + "' style='width:100px;height:100px;'>";
                    photo += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                    photo += "</a>";
                    photo += "</div>";
                    username = "<span style='text-align: center;font-size:medium;'>" + ict_f1.Table.Rows[0]["username"].ToString() + "</span><br/>";
                }
                //user photo and name user_photo
                //預かり日時 sandbox-container
                //預かり内容 report_content
                //預かり時の様子 report_list
                string information_pan = "";
                information_pan += @"<table width='100%' height='100%' style='background-color: #F5F5F5;'>
                            <tr><td width='5%'></td><td width='90%'>
            ";
                //user photo
                information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='center' valign='top'>" + photo + @"
</td><td width='10%'></td></tr>
</table>";
                //user name
                information_pan += @"<table width='100%'>
<tr><td width='10%'></td><td width='80%' align='center' valign='top'>";
                information_pan += username;

                information_pan += @"<br/><br/></td><td width='10%'></td></tr>
</table>";
                user_photo.Controls.Add(new LiteralControl(information_pan));
                sandboxcontainer.Controls.Add(new LiteralControl(selectdate));
                report_content.Controls.Add(new LiteralControl(howtoget_there));
                string relist = "";
                sql_f1 = new SqlDataSource();
                sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f1.SelectCommand = "select content_success";
                sql_f1.SelectCommand += " from user_information_appointment_check_deal";
                sql_f1.SelectCommand += " where id='" + uiacdid + "';";
                sql_f1.DataBind();
                ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                if (ict_f1.Count > 0)
                {
                    relist = "<span style='text-align: center;font-size:medium;'>" + ict_f1.Table.Rows[0]["content_success"].ToString() + "</span><br/>";
                }

                report_list.Controls.Add(new LiteralControl(relist));

                if (check_again)
                {
                    button_pan.Controls.Add(new LiteralControl("<input type='button' id='buttonsub_" + uiacdid + "' value='承認する' onclick='report_create_success(this.id)' style='width: 100%;text-shadow: none;cursor: pointer;text-align: center;' class='file-upload'>"));
                }
            }
            else
            {
                Response.Redirect("main.aspx");
            }
        }
        else
        {
            Response.Redirect("main.aspx");
        }

    }
    [WebMethod]
    public static string report_build_report(string param1)
    {
        string results = "";
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;

        SqlDataSource sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.UpdateCommand = "update user_information_appointment_check_deal set check_success=4,fourth_check_time='" + start + "' where id='" + param1 + "';";
        sql_f.Update();

        sql_f = new SqlDataSource();
        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql_f.SelectCommand = "select uid,suppid";
        sql_f.SelectCommand += " from user_information_appointment_check_deal";
        sql_f.SelectCommand += " where id='" + param1 + "';";
        sql_f.DataBind();
        DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
        if(ict_f.Count>0)
        {

           
            SqlDataSource sql_insert = new SqlDataSource();
            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["uid"].ToString() + "','" + ict_f.Table.Rows[0]["suppid"].ToString() + "','【報告書確認完了】','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
            sql_insert.Insert();

        }


        return results;
    }
}