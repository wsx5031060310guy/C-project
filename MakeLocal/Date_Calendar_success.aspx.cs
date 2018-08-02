using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Date_Calendar_success : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("main.aspx");
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
}
