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

public partial class mamaro_all : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["manager"] != null)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
            if (Session["manager"].ToString().Trim() != "")
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
                if (Session["manager"].ToString() == "")
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

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string check_mamaro(string param1, string param2, string param7)
    {
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        int QAcou = 0;
        int QAcou1 = 0;
        string result = "";
        DateTime start = Convert.ToDateTime(param1.Trim());
        DateTime end = Convert.ToDateTime(param2.Trim());

        state_detail_list = new List<state_detail>();
        string res = @"<br/><h2>年月日:</h2>";
        res += @"<br/><h2>" + start.Year + "年" + start.Month + "月" + start.Day + "日 ～ " + end.Year + "年" + end.Month + "月" + end.Day + "日" + @"</h2><br /><br />";
        if (param7 == "")
        {
            param7 = "300";
        }
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
                        state_group sg = new state_group();
                        state_detail sgg = new state_detail();

                        state_list = new List<state_group>();
                        lock_state_list = new List<state_group>();

                        string room_id = ict_place.Table.Rows[ih]["id"].ToString();
                        //mamaro QA


                        //state time
                        Query = "select * from nursing_room_state_time where nursing_room_id='" + room_id + "' and update_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "' order by update_time asc;";
                        DataView ict = gc.select_cmd(Query);
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
                        string monstr = "var MONTHS = [";
                        monstr += "];";
                        string monstt = "";
                        string arrva = "";

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
                        //Chart1.Series.Add(series1);//將線畫在圖上

                        //for (int i = 0; i < check_list.Count; i++)
                        //{
                        //    listBox1.Items.Add(check_list[i].state);
                        //    listBox2.Items.Add(check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss"));
                        //}

                        if (check_list1.Count > 0 && check_list.Count > 0)
                        {
                            sgg.answer_datetime_s = start.Year + "年" + start.Month + "月" + start.Day + "日";
                            sgg.answer_datetime_e = end.Year + "年" + end.Month + "月" + end.Day + "日";
                            sgg.name = ict_place.Table.Rows[ih]["name"].ToString();


                            double lll = havg / hcou;
                            if (hcou == 0)
                            {
                                lll = 0;
                            }
                            res += @"<br/><fieldset>
    <legend>" + ict_place.Table.Rows[ih]["name"].ToString() + @"</legend><br/>
<br/>
<h2>" + param7 + @"秒以上の利用回数（総数）:</h2>
<br/>
<h2>" + hcou.ToString() + @"</h2>
<br/>
<h2>" + param7 + @"秒以上のみでの平均利用時間（秒）（総利用者）:</h2>
<br/>
<h2>" + lll.ToString() + @"</h2>
";

                            sgg.time1 = hcou.ToString();
                            sgg.time2 = lll.ToString();

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

                            lll = havg / hcou;
                            if (hcou == 0)
                            {
                                lll = 0;
                            }
                            double avggg = avg / coun;
                            if (avg == 0)
                            {
                                avggg = 0;
                            }
                            res += @"
<br/>
<h2>利用者数:</h2>
<br/>
<h2>" + coun.ToString() + @"</h2>
<br/>
<h2>平均利用時間（秒）:</h2>
<br/>
<h2>" + avggg.ToString() + @"</h2>
<br/>
<h2>" + param7 + @"秒以上の利用回数（施錠して利用）:</h2>
<br/>
<h2>" + hcou.ToString() + @"</h2>
<br/>
<h2>" + param7 + @"秒以上のみでの平均利用時間（秒）（施錠利用者）:</h2>
<br/>
<h2>" + lll.ToString() + @"</h2>
<br/>
<br/>
  </fieldset>
";

                            sgg.time3 = coun.ToString();
                            sgg.time4 = avggg.ToString();
                            sgg.time5 = hcou.ToString();
                            sgg.time6 = lll.ToString();

                            state_detail_list.Add(sgg);

                            //                res += @"<script type='text/javascript'>
                            //
                            //</script>";


                        }

                        //result = "fail";
                    }
                }
                result = res;

                res = "";
                if (state_detail_list.Count > 0)
                {
                    res += @"<fieldset><legend style='font-size: large; font-weight: bold'>state detail</legend>";

                    res += @"<div id='dvData'>
    <table>
        <tr>
            <td>開始日</td>
            <td>終了日</td>
            <td>名前</td>
            <td>" + param7 + @"秒以上の利用回数（総数）</td>
            <td>" + param7 + @"秒以上のみでの平均利用時間（秒）（総利用者）</td>
            <td>利用者数</td>
            <td>平均利用時間（秒）</td>
            <td>" + param7 + @"秒以上の利用回数（施錠して利用）</td>
            <td>" + param7 + @"秒以上のみでの平均利用時間（秒）（施錠利用者）</td>
        </tr>";
                    for (int i = 0; i < state_detail_list.Count; i++)
                    {



                        res += @"<tr>
    <td>" + state_detail_list[i].answer_datetime_s + @"</td>
    <td>" + state_detail_list[i].answer_datetime_e + @"</td>
<td>" + state_detail_list[i].name + @"</td>
<td>" + state_detail_list[i].time1 + @"</td>
<td>" + state_detail_list[i].time2 + @"</td>
<td>" + state_detail_list[i].time3 + @"</td>
<td>" + state_detail_list[i].time4 + @"</td>
<td>" + state_detail_list[i].time5 + @"</td>
<td>" + state_detail_list[i].time6 + @"</td>
</tr>
";

                    }
                    res += @"</table>
</div>";


                    res += @"</fieldset><hr/>";
                }

                result += res;
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
