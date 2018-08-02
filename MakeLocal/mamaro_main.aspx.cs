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

public partial class mamaro_main : System.Web.UI.Page
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
        List<string> uni_id = new List<string>();
        string Query = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        Panel1.Controls.Clear();
        Query = "select id,company_name";
        Query += " from nursing_room_from;";
        DataView ict_place = gc.select_cmd(Query);
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                bool checksame = true;

                for (int ix = 0; ix < uni_id.Count; ix++)
                {
                    if (uni_id[ix] == ict_place.Table.Rows[i]["company_name"].ToString())
                    {
                        checksame = false;
                    }

                }
                if (checksame)
                {
                    uni_id.Add(ict_place.Table.Rows[i]["company_name"].ToString());

                    Panel1.Controls.Add(new LiteralControl("<fieldset><legend style='font-size: large; font-weight: bold'>" + ict_place.Table.Rows[i]["company_name"].ToString() + "</legend>"));
                    Query = "select nursing_room_id";
                    Query += " from nursing_room_connect_from where nursing_room_from_id='" + ict_place.Table.Rows[i]["id"].ToString() + "';";
                    DataView ict_place1 = gc.select_cmd(Query);
                    if (ict_place1.Count > 0)
                    {
                        for (int ii = 0; ii < ict_place1.Count; ii++)
                        {

                            Panel1.Controls.Add(new LiteralControl(ict_place1.Table.Rows[ii]["nursing_room_id"].ToString()));
                            Panel1.Controls.Add(new LiteralControl("<br/>"));
                        }

                    }
                    Panel1.Controls.Add(new LiteralControl("</fieldset>"));
                }

            }
        }
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
    public class QA_group
    {
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
    public static string check_mamaro(string param1, string param2, string param3, string param4, string param5, string param6, string param7)
    {
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        int QAcou = 0;
        int QAcou1 = 0;
        string result = "";
        DateTime start =Convert.ToDateTime( param1.Trim());
        DateTime end = Convert.ToDateTime( param2.Trim());
        if (param7 == "")
        {
            param7 = "300";
        }
        try
        {
            if (start != null && end != null)
            {
                state_group sg = new state_group();
                QA_group qaa = new QA_group();

                state_list = new List<state_group>();
                lock_state_list = new List<state_group>();

                QA_list = new List<QA_group>();

                string room_id = param6;
                //mamaro QA
                Query = "select * from nursing_room_QA where nursing_room_id='" + room_id + "' and insert_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                DataView ict = gc.select_cmd(Query);
                QAcou = ict.Count;
                if (ict.Count > 0)
                {

                    for (int i = 0; i < ict.Count; i++)
                    {
                        qaa = new QA_group();
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
                QA_list_1 = new List<QA_group_1>();
                //mamaro new QA
                Query = "select * from nursing_room_QA1 where nursing_room_id='" + room_id + "' and insert_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                ict = gc.select_cmd(Query);
                QAcou1 = ict.Count;
                if (ict.Count > 0)
                {

                    for (int i = 0; i < ict.Count; i++)
                    {
                        qaa_1 = new QA_group_1();
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

                //state time
                Query = "select * from nursing_room_state_time where nursing_room_id='" + room_id + "' and update_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "' order by update_time asc;";
                ict = gc.select_cmd(Query);
                if (ict.Count > 0)
                {

                    for (int i = 0; i < ict.Count; i++)
                    {
                        sg = new state_group();
                        sg.state = Convert.ToInt32(ict.Table.Rows[i]["state"].ToString());
                        sg.update = Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString());

                        state_list.Add(sg);
                    }
                }

                List<state_group> check_list = new List<state_group>();

                for (int i = 0; i < state_list.Count; i++)
                {
                    if (state_list[i].state == 1)
                    {

                        sg = new state_group();
                        sg.state = state_list[i].state;
                        sg.update = state_list[i].update;
                        check_list.Add(sg);
                        int index = i + 1;
                        while (index < state_list.Count)
                        {
                            if (state_list[index].state == 0)
                            {
                                sg = new state_group();
                                sg.state = state_list[index].state;
                                sg.update = state_list[index].update;
                                check_list.Add(sg);
                                break;
                            }
                            index += 1;
                        }

                    }
                }
                //lock state time
                Query = "select * from nursing_room_lock_state_time where nursing_room_id='" + room_id + "' and update_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "' order by update_time asc;;";
                ict = gc.select_cmd(Query);
                if (ict.Count > 0)
                {
                    if (room_id == "9")
                    {
                        for (int i = 0; i < ict.Count; i++)
                        {
                            sg = new state_group();
                            if (Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString()).Year <= 2017 &&
                                Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString()).Month <= 11 &&
                                Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString()).Day <= 26)
                            {
                                if (Convert.ToInt32(ict.Table.Rows[i]["lock_state"].ToString()) == 0)
                                {
                                    sg.state = 1;
                                }
                                else
                                {
                                    sg.state = 0;
                                }
                            }
                            else
                            {
                                sg.state = Convert.ToInt32(ict.Table.Rows[i]["lock_state"].ToString());
                            }
                            sg.update = Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString());

                            lock_state_list.Add(sg);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ict.Count; i++)
                        {
                            sg = new state_group();
                            sg.state = Convert.ToInt32(ict.Table.Rows[i]["lock_state"].ToString());
                            sg.update = Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString());

                            lock_state_list.Add(sg);
                        }
                    }

                }


                List<state_group> check_list1 = new List<state_group>();

                for (int i = 0; i < lock_state_list.Count; i++)
                {
                    if (lock_state_list[i].state == 0)
                    {

                        sg = new state_group();
                        sg.state = lock_state_list[i].state;
                        sg.update = lock_state_list[i].update;
                        check_list1.Add(sg);
                        int index = i + 1;
                        while (index < lock_state_list.Count)
                        {
                            if (lock_state_list[index].state == 1)
                            {
                                sg = new state_group();
                                sg.state = lock_state_list[index].state;
                                sg.update = lock_state_list[index].update;
                                check_list1.Add(sg);
                                break;
                            }
                            index += 1;
                        }

                    }
                }
                //Chart1.Series.Clear();  //每次使用此function前先清除圖表
                //Series series1 = new Series("use time(second)", 1200); //初始畫線條(名稱，最大值)
                //series1.Color = Color.Blue; //設定線條顏色
                //series1.Font = new System.Drawing.Font("新細明體", 10); //設定字型
                //series1.ChartType = SeriesChartType.Line; //設定線條種類
                //Chart1.ChartAreas[0].AxisY.Minimum = 0;//設定Y軸最小值
                //Chart1.ChartAreas[0].AxisY.Maximum = 1200;//設定Y軸最大值
                ////chart1.ChartAreas[0].AxisY.Enabled= AxisEnabled.False; //隱藏Y 軸標示
                ////chart1.ChartAreas[0].AxisY.MajorGrid.Enabled= true;  //隱藏Y軸標線
                //series1.IsValueShownAsLabel = true; //是否把數值顯示在線上
                string monstr="var MONTHS = [";
                monstr+="];";
                string monstt="";
                string arrva="";

                double avg = 0, coun = 0, hcou = 0, havg = 0;
                double compute_day = -1;
                DateTime first_d, second_d;
                for (int i = 0; i < check_list.Count; i++)
                {
                    compute_day = -1;
                    if (check_list[i].state == 1)
                    {
                        first_d = Convert.ToDateTime(check_list[i].update);

                        int index = i + 1;
                        while (index < check_list.Count)
                        {
                            if (check_list[index].state == 0)
                            {
                                second_d = Convert.ToDateTime(check_list[index].update);
                                compute_day = (second_d - first_d).TotalSeconds;

                                break;
                            }
                            index += 1;
                        }
                        if (compute_day > -1)
                        {
                            //monstt+=check_list[i].update.ToString("yyyyMMddHHmmss")+",";
                            //arrva +=compute_day+ ",";

                            if (compute_day < 1801)
                            {
                                coun += 1;
                                avg += compute_day;
                                monstt += check_list[i].update.ToString("yyyyMMddHHmmss") + ",";
                                arrva += compute_day + ",";
                            }
                            if (compute_day >=Convert.ToInt32( param7) && compute_day < 1801)
                            {
                                hcou += 1;
                                havg += compute_day;
                            }
                            //series1.Points.AddXY(i.ToString(), compute_day);


                            //listBox3.Items.Add("Start time: " + check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss") + " ~");
                            //listBox3.Items.Add("End time: " + check_list[index].update.ToString("yyyy-MM-dd HH:mm:ss"));
                            //listBox3.Items.Add("Total sec: " + compute_day);

                        }
                        else
                        {
                            //DateTime todate = DateTime.Now;
                            //double compute_day1 = (todate - first_d).TotalSeconds;
                            //listBox3.Items.Add("Start time: " + check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss") + " ~");
                            //listBox3.Items.Add("End time: NO");
                            //listBox3.Items.Add("Total sec: NO");
                        }

                    }
                }
                if (monstt!="")
                {
                    monstt = monstt.Substring(0, monstt.Length-1);
                }
                if (arrva != "")
                {
                    arrva = arrva.Substring(0, arrva.Length - 1);
                }
                //Chart1.Series.Add(series1);//將線畫在圖上

                //for (int i = 0; i < check_list.Count; i++)
                //{
                //    listBox1.Items.Add(check_list[i].state);
                //    listBox2.Items.Add(check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss"));
                //}

                string res = @"<fieldset>
    <legend>State Time</legend><br/>
<script type='text/javascript'>
         var config = {
             type: 'line',
             data: {
                 labels: [" + monstt + @"],
                 datasets: [{
                     label: 'Use time dataset',
                     fill: false,
                     backgroundColor: 'rgb(" + param3 + @", " + param4 + @", " + param5 + @")',
                     borderColor: 'rgb(" + param3 + @", " + param4 + @", " + param5 + @")',
                     data: [" + arrva + @"
                     ],

                 }]
             },
             options: {
                 responsive: true,
                 title: {
                     display: true,
                     text: 'Use Time Line Chart'
                 },
                 tooltips: {
                     mode: 'index',
                     intersect: false,
                 },
                 hover: {
                     mode: 'nearest',
                     intersect: true
                 },
                 scales: {
                     xAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: 'DateTime'
                         }
                     }],
                     yAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: 'Time(second)'
                         }
                     }]
                 }
             }
         };
 var ctx = document.getElementById('canvas').getContext('2d');
             window.myLine = new Chart(ctx, config);
</script>
<canvas id='canvas'></canvas>
<br/>
<h2>Total users:</h2>
<br/>
<h2>" + coun.ToString() + @"</h2>
<br/>
<h2>AVG use time (second):</h2>
<br/>
<h2>" + (avg /= coun).ToString() + @"</h2>
<br/>
<h2>Long users(after " + param7 + @" second):</h2>
<br/>
<h2>" + hcou.ToString() + @"</h2>
<br/>
<h2>Long users AVG use time(after " + param7 + @" second)(second):</h2>
<br/>
<h2>" + (havg /= hcou).ToString() + @"</h2>
  </fieldset>
";


                monstr = "var MONTHS = [";
                monstr += "];";
                monstt = "";
                arrva = "";

                avg = 0; coun = 0; hcou = 0; havg = 0;
                compute_day = -1;
                for (int i = 0; i < check_list1.Count; i++)
                {
                    compute_day = -1;
                    if (check_list1[i].state == 0)
                    {
                        first_d = Convert.ToDateTime(check_list1[i].update);

                        int index = i + 1;
                        while (index < check_list1.Count)
                        {
                            if (check_list1[index].state == 1)
                            {
                                second_d = Convert.ToDateTime(check_list1[index].update);
                                compute_day = (second_d - first_d).TotalSeconds;

                                break;
                            }
                            index += 1;
                        }
                        if (compute_day > -1)
                        {
                            //monstt += check_list1[i].update.ToString("yyyyMMddHHmmss") + ",";
                            //arrva += compute_day + ",";

                            if (compute_day < 1801)
                            {
                                coun += 1;
                                avg += compute_day;
                                monstt += check_list1[i].update.ToString("yyyyMMddHHmmss") + ",";
                                arrva += compute_day + ",";
                            }
                            if (compute_day >= Convert.ToInt32(param7) && compute_day < 1801)
                            {
                                hcou += 1;
                                havg += compute_day;
                            }
                            //series1.Points.AddXY(i.ToString(), compute_day);


                            //listBox3.Items.Add("Start time: " + check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss") + " ~");
                            //listBox3.Items.Add("End time: " + check_list[index].update.ToString("yyyy-MM-dd HH:mm:ss"));
                            //listBox3.Items.Add("Total sec: " + compute_day);

                        }
                        else
                        {
                            //DateTime todate = DateTime.Now;
                            //double compute_day1 = (todate - first_d).TotalSeconds;
                            //listBox3.Items.Add("Start time: " + check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss") + " ~");
                            //listBox3.Items.Add("End time: NO");
                            //listBox3.Items.Add("Total sec: NO");
                        }

                    }
                }
                if (monstt != "")
                {
                    monstt = monstt.Substring(0, monstt.Length - 1);
                }
                if (arrva != "")
                {
                    arrva = arrva.Substring(0, arrva.Length - 1);
                }
                res += @"<fieldset>
    <legend>Lock State Time</legend><br/>
<script type='text/javascript'>
         var config1 = {
             type: 'line',
             data: {
                 labels: [" + monstt + @"],
                 datasets: [{
                     label: 'Lock Use time dataset',
                     fill: false,
                     backgroundColor: 'rgb(" + param3 + @", " + param4 + @", " + param5 + @")',
                     borderColor: 'rgb(" + param3 + @", " + param4 + @", " + param5 + @")',
                     data: [" + arrva + @"
                     ],

                 }]
             },
             options: {
                 responsive: true,
                 title: {
                     display: true,
                     text: 'Lock Use Time Line Chart'
                 },
                 tooltips: {
                     mode: 'index',
                     intersect: false,
                 },
                 hover: {
                     mode: 'nearest',
                     intersect: true
                 },
                 scales: {
                     xAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: 'DateTime'
                         }
                     }],
                     yAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: 'Time(second)'
                         }
                     }]
                 }
             }
         };
 var ctx1 = document.getElementById('canvas1').getContext('2d');
             window.myLine = new Chart(ctx1, config1);
</script>
<canvas id='canvas1'></canvas>
<br/>
<h2>Total users:</h2>
<br/>
<h2>" + coun.ToString() + @"</h2>
<br/>
<h2>AVG use time (second):</h2>
<br/>
<h2>" + (avg /= coun).ToString() + @"</h2>
<br/>
<h2>Long users(after " + param7 + @" second):</h2>
<br/>
<h2>" + hcou.ToString() + @"</h2>
<br/>
<h2>Long users AVG use time(after " + param7 + @" second)(second):</h2>
<br/>
<h2>" + (havg /= hcou).ToString() + @"</h2>
<br/>
<br/>
<h2>QA count:</h2>
<br/>
<h2>" + QAcou.ToString() + @"</h2>
<br/>
<h2>new QA count:</h2>
<br/>
<h2>" + QAcou1.ToString() + @"</h2>
<br/>
  </fieldset>
";
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
        </tr>";

                //QA detail
                for (int i = 0; i < QA_list.Count; i++)
                {
                    res += @"<tr>
    <td>" + QA_list[i].answer_datetime.ToString("yyyy-MM-dd HH:mm:ss") + @"</td>
<td>" + QA_list[i].lan + @"</td>
<td>" + QA_list[i].baby_y + @"歳 " + QA_list[i].baby_m + @"ヶ月</td>
<td>" + QA_list[i].parent + @"</td>
<td>" + QA_list[i].Q2_choice.Replace(",",".") + @"</td>
<td>" + QA_list[i].Q3_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].Q4_choice.Replace(",", ".") + @"</td>
<td>" + QA_list[i].Q5_choice.Replace(",", ".") + @"</td>
</tr>
";
                }


                res += @"</table>
</div>";


                res += @"</fieldset><hr/>";


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
</tr>
";
                }


                res += @"</table>
</div>";


                res += @"</fieldset><hr/>";

//                res += @"<script type='text/javascript'>
//
//</script>";


                result = res;


                //result = "fail";
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
