using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;


public partial class mamaro_QA_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["manager"] != null)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
            if (Session["manager"].ToString().Trim() != "")
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
                if (Session["manager"].ToString() == "trim")
                {

                }
                else
                {
                    Session.Clear();
                    Response.Redirect("manager_page.aspx");
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect("manager_page.aspx");
            }
        }
        else
        {
            Session.Clear();
            Response.Redirect("manager_page.aspx");
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {

    }
    public class state_group
    {
        public int state = -1;
        public DateTime update;
    }
    static List<state_group> state_list = new List<state_group>();
    static List<state_group> lock_state_list = new List<state_group>();
    /// <summary>
    /// old QA
    /// </summary>
    public class state_detail
    {
        public string answer_datetime_s;
        public string answer_datetime_e;
        public string name = "";
        public string time1 = "";
        public string time2 = "";
        public string time3 = "";
        public string time4 = "";
        public string time5 = "";
        public string time6 = "";
    }
    static List<state_detail> state_detail_list = new List<state_detail>();
    /// <summary>
    /// old QA
    /// </summary>
    public class QA_group
    {
        public string roomid = "";
        public DateTime answer_datetime;
        public string lan = "";
        public string baby_y = "";
        public string baby_m = "";
        public string parent = "";
        public string Q2_choice = "";
        public string Q3_choice = "";
        public string Q4_choice = "";
        public string Q5_choice = "";
    }
    static List<QA_group> QA_list = new List<QA_group>();

    /// <summary>
    /// new QA
    /// </summary>
    public class QA_group_1
    {
        public string roomid = "";
        public DateTime answer_datetime;
        public string lan = "";
        public string baby_y_m = "";
        public string parent = "";
        public string Q2_choice = "";
        public string Q3_choice = "";
        public string Q4_choice = "";
        public string Q5_choice = "";
        public string Q6_choice = "";
    }
    static List<QA_group_1> QA_list_1 = new List<QA_group_1>();

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string check_mamaro(string param1, string param2)
    {
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        int QAcou = 0;
        int QAcou1 = 0;
        string result = "";
        DateTime start = Convert.ToDateTime(param1.Trim());
        DateTime end = Convert.ToDateTime(param2.Trim());

        state_detail_list = new List<state_detail>();
        QA_list = new List<QA_group>();
        QA_list_1 = new List<QA_group_1>();
        string res = @"<br/><h2>年月日:</h2>";
        res += @"<br/><h2>" + start.Year + "年" + start.Month + "月" + start.Day + "日 ～ " + end.Year + "年" + end.Month + "月" + end.Day + "日" + @"</h2><br /><br />";
        try
        {

            if (start != null && end != null)
            {
                Query = "select id,name";
                Query += " from nursing_room;";
                DataView ict_place = gc.select_cmd(Query);
                if (ict_place.Count > 0)
                {
                    for (int ih = 0; ih < ict_place.Count; ih++)
                    {
                        QA_group qaa = new QA_group();

                        string room_id = ict_place.Table.Rows[ih]["id"].ToString();
                        //mamaro QA
                        Query = "select * from nursing_room_QA where nursing_room_id='" + room_id + "' and insert_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        DataView ict = gc.select_cmd(Query);
                        QAcou = ict.Count;
                        if (ict.Count > 0)
                        {

                            for (int i = 0; i < ict.Count; i++)
                            {
                                qaa = new QA_group();
                                qaa.roomid = room_id;
                                qaa.answer_datetime = Convert.ToDateTime(ict.Table.Rows[i]["insert_time"].ToString());
                                qaa.baby_m = ict.Table.Rows[i]["Q1_baby_month"].ToString();
                                qaa.baby_y = ict.Table.Rows[i]["Q1_baby_year"].ToString();
                                qaa.lan = ict.Table.Rows[i]["language"].ToString();
                                qaa.parent = ict.Table.Rows[i]["Q1_parent"].ToString();
                                qaa.Q2_choice = ict.Table.Rows[i]["Q2_choice"].ToString();
                                qaa.Q3_choice = ict.Table.Rows[i]["Q3_choice"].ToString();
                                qaa.Q4_choice = ict.Table.Rows[i]["Q4_choice"].ToString();
                                qaa.Q5_choice = ict.Table.Rows[i]["Q5_choice"].ToString();

                                QA_list.Add(qaa);
                            }
                        }


                        QA_group_1 qaa_1 = new QA_group_1();
                        //mamaro new QA
                        Query = "select * from nursing_room_QA1 where nursing_room_id='" + room_id + "' and insert_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        ict = gc.select_cmd(Query);
                        QAcou1 = ict.Count;
                        if (ict.Count > 0)
                        {

                            for (int i = 0; i < ict.Count; i++)
                            {
                                qaa_1 = new QA_group_1();
                                qaa_1.roomid = room_id;
                                qaa_1.answer_datetime = Convert.ToDateTime(ict.Table.Rows[i]["insert_time"].ToString());
                                qaa_1.baby_y_m = ict.Table.Rows[i]["Q1_baby_year_month"].ToString();
                                qaa_1.lan = ict.Table.Rows[i]["language"].ToString();
                                qaa_1.parent = ict.Table.Rows[i]["Q1_parent"].ToString();
                                qaa_1.Q2_choice = ict.Table.Rows[i]["Q2_choice"].ToString();
                                qaa_1.Q3_choice = ict.Table.Rows[i]["Q3_choice"].ToString();
                                qaa_1.Q4_choice = ict.Table.Rows[i]["Q4_choice"].ToString();
                                qaa_1.Q5_choice = ict.Table.Rows[i]["Q5_choice"].ToString();
                                qaa_1.Q6_choice = ict.Table.Rows[i]["Q6_choice"].ToString();
                                QA_list_1.Add(qaa_1);
                            }
                        }
                        //result = "fail";
                    }
                }

                res = "";
                if (QA_list.Count > 0)
                {

                    res += @"<fieldset><legend style='font-size: large; font-weight: bold'>QA detail</legend>";

                    res += @"<div id='dvData'>
    <table>
        <tr>
            <td>日時</td>
            <td>言語 (language)</td>
            <td>お子さまのご年齢は</td>
            <td>あなたは</td>
            <td>mamaroがどこにあったら便利? ( 複数選択可 )</td>
            <td>mamaroをどこで知った? ( 複数選択可 )</td>
            <td>mamaroに 「もっとこんな機能が欲しい。こんなのがあったら嬉しい。」というものはありますか？</td>
            <td>授乳室は個室派？わいわい派？</td>
            <td>mamaro id</td>
        </tr>";

                    //QA detail
                    for (int i = 0; i < QA_list.Count; i++)
                    {
                        res += @"<tr>
    <td>" + QA_list[i].answer_datetime.ToString("yyyy-MM-dd HH:mm:ss") + @"</td>
<td>" + QA_list[i].lan + @"</td>
<td>" + QA_list[i].baby_y + @"歳 " + QA_list[i].baby_m + @"ヶ月</td>
<td>" + QA_list[i].parent + @"</td>
<td>" + QA_list[i].Q2_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].Q3_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].Q4_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].Q5_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].roomid.Replace(",", ".") + @"</td>
</tr>
";
                    }


                    res += @"</table>
</div>";


                    res += @"</fieldset><hr/>";
                }

                if (QA_list_1.Count > 0)
                {


                    //new QA
                    res += @"<fieldset><legend style='font-size: large; font-weight: bold'>new QA detail</legend>";

                    res += @"<div id='dvData_new'>
    <table>
        <tr>
            <td>日時</td>
            <td>言語 (language)</td>
            <td>お子さまのご年齢は</td>
            <td>お子さまとの関係</td>
            <td>mamaroのご利用は何回目ですが？</td>
            <td>mamaroをどのようにして知りましたか？</td>
            <td>mamaroの空室状況をBabymapというアプリで調べることができるのを知っていましたか？</td>
            <td>mamaroで" + '"' + @"気に入っているポイント" + '"' + @"を教えてください。（複数選択可）</td>
            <td>mamaroの満足度を教えてください。</td>
            <td>mamaro id</td>
        </tr>";

                    //QA detail
                    for (int i = 0; i < QA_list_1.Count; i++)
                    {
                        res += @"<tr>
    <td>" + QA_list_1[i].answer_datetime.ToString("yyyy-MM-dd HH:mm:ss") + @"</td>
<td>" + QA_list_1[i].lan + @"</td>
<td>" + QA_list_1[i].baby_y_m + @"</td>
<td>" + QA_list_1[i].parent + @"</td>
<td>" + QA_list_1[i].Q2_choice.Replace(",", ".") + @"</td>
<td>" + QA_list_1[i].Q3_choice.Replace(",", ".") + @"</td>
<td>" + QA_list_1[i].Q4_choice.Replace(",", ".") + @"</td>
<td>" + QA_list_1[i].Q5_choice.Replace(",", ".") + @"</td>
<td>" + QA_list_1[i].Q6_choice.Replace(",", ".") + @"</td>
<td>" + QA_list_1[i].roomid.Replace(",", ".") + @"</td>
</tr>
";
                    }


                    res += @"</table>
</div>";


                    res += @"</fieldset><hr/>";

                }

                result += res;

                if (result == "")
                {
                    result = "なし";
                }
            }
        }
        catch (Exception ex)
        {
            //result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
}