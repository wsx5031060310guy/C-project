using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_user : System.Web.UI.Page
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
        string Query = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        Panel1.Controls.Clear();
        Query = "select * from nursing_room;";
        DataView ict_place = gc.select_cmd(Query);
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {

                Panel1.Controls.Add(new LiteralControl("<input name='user_active_col[]' type='checkbox' value='"+ict_place.Table.Rows[i]["id"].ToString()+"'> "+ict_place.Table.Rows[i]["name"].ToString()+""));
                Panel1.Controls.Add(new LiteralControl("<br/>"));
            }
        }
       
    }
    
    public class user_group
    {
        public int sec = 0;
        public int total = 0;
    }
    static List<user_group> usercou_list = new List<user_group>();
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
    public static string check_mamaro(string param1, string param2, string param3, string param4,string param5)
    {
        //test
        //int comid = 26;
        //List<int> comlist = new List<int>();
        //comlist.Add(comid);
        //comlist.Add(Convert.ToInt32( param6));

        string[] liness = param3.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


        List<string> res_list = new List<string>();
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        int QAcou = 0;
        int QAcou1 = 0;
        string result = "";
        DateTime start = Convert.ToDateTime(param1.Trim());
        DateTime end = Convert.ToDateTime(param2.Trim());
        if (param4 == "")
        {
            param4 = "30";
        }
        try
        {
            if (start != null && end != null)
            {
                string label = "";
                string res ="";
                 string res1 ="";
                 for (int kk = 0; kk < liness.Length; kk++)
                {
                    state_group sg = new state_group();
                    QA_group qaa = new QA_group();

                    state_list = new List<state_group>();
                    lock_state_list = new List<state_group>();

                    QA_list = new List<QA_group>();

                    string room_id = liness[kk];
                    //mamaro name
                    string mamaname = "";
                    Query = "select * from nursing_room where id='" + room_id + "';";
                    DataView ict = gc.select_cmd(Query);
                    if (ict.Count > 0)
                    {
                        mamaname = ict.Table.Rows[0]["name"].ToString();

                    }



                    //mamaro QA
                    Query = "select * from nursing_room_QA where nursing_room_id='" + room_id + "' and insert_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                    ict = gc.select_cmd(Query);
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
                    List<int> count_list = new List<int>();


                   

                    usercou_list = new List<user_group>();
                    user_group ug = new user_group();
                    int basesec = Convert.ToInt32(param4);
                    int coun_start = 0;
                    int coun_end = basesec;
                    int couuuu = 0;

                   
                    //Chart1.Series.Add(series1);//將線畫在圖上

                    //for (int i = 0; i < check_list.Count; i++)
                    //{
                    //    listBox1.Items.Add(check_list[i].state);
                    //    listBox2.Items.Add(check_list[i].update.ToString("yyyy-MM-dd HH:mm:ss"));
                    //}
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    int r = rnd.Next(256);
                    rnd = new Random(Guid.NewGuid().GetHashCode());
                    int g = rnd.Next(256);
                    rnd = new Random(Guid.NewGuid().GetHashCode());
                    int b = rnd.Next(256);

                    


                    monstr = "var MONTHS = [";
                    monstr += "];";
                    monstt = "";
                    arrva = "";

                    avg = 0; coun = 0; hcou = 0; havg = 0;
                    compute_day = -1;
                    count_list = new List<int>();
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
                                    //monstt += check_list1[i].update.ToString("yyyyMMddHHmmss") + ",";
                                    //arrva += compute_day + ",";
                                    count_list.Add(Convert.ToInt32(compute_day));
                                }
                                if (compute_day >= Convert.ToInt32(param4) && compute_day < 1801)
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

                    usercou_list = new List<user_group>();
                    ug = new user_group();
                    basesec = Convert.ToInt32(param4);
                    coun_start = 0;
                    coun_end = basesec;
                    couuuu = 0;
                    for (int i = 0; i < Convert.ToInt32(param5); i += basesec)
                    {
                        couuuu = 0;
                        ug = new user_group();
                        ug.sec = i;
                        for (int ii = 0; ii < count_list.Count; ii++)
                        {
                            if (count_list[ii] > i)
                            {
                                couuuu += 1;
                            }
                            //if (count_list[ii] > i && count_list[ii] < (i + basesec))
                            //{
                            //    couuuu += 1;
                            //}
                        }
                        ug.total = couuuu;
                        usercou_list.Add(ug);
                    }
                    for (int i = 0; i < usercou_list.Count; i++)
                    {
                        
                        //monstt += usercou_list[i].sec.ToString() + ",";
                        if (usercou_list[i].sec % 60 == 0)
                        {
                            monstt += (usercou_list[i].sec / 60).ToString() + ",";
                        }
                        else
                        {
                            monstt += " ,";
                        }

                        arrva += usercou_list[i].total.ToString() + ",";
                    }

                    if (monstt != "")
                    {
                        monstt = monstt.Substring(0, monstt.Length - 1);
                    }
                    if (arrva != "")
                    {
                        arrva = arrva.Substring(0, arrva.Length - 1);
                    }
                    rnd = new Random(Guid.NewGuid().GetHashCode());
                    r = rnd.Next(256);
                    rnd = new Random(Guid.NewGuid().GetHashCode());
                    g = rnd.Next(256);
                    rnd = new Random(Guid.NewGuid().GetHashCode());
                    b = rnd.Next(256);
                    res1 += @"{
                     label: '" + mamaname + @"',
                     fill: false,
                     backgroundColor: 'rgb(" + r + @", " + g + @", " + b + @")',
                     borderColor: 'rgb(" + r + @", " + g + @", " + b + @")',
                     data: [" + arrva + @"
                     ],
                     
                 },";
                    res = @"{
                     label: '" + mamaname + @"',
                     fill: false,
                     backgroundColor: 'rgb(" + r + @", " + g + @", " + b + @")',
                     borderColor: 'rgb(" + r + @", " + g + @", " + b + @")',
                     data: [" + arrva + @"
                     ],
                     
                 },";
                    label = monstt;




                    string ress = @"<fieldset>
    <legend>" + mamaname + @"</legend><br/>
<script type='text/javascript'>
         var config"+kk+@" = {
             type: 'line',
             data: {
                 labels: [" + label + @"],
                 datasets: [" + res + @"]
             },
             options: {
                 responsive: true,
                 title: {
                     display: true,
                     text: '" + mamaname + @"'
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
                             labelString: '滞在時間（分）'
                         }
                     }],
                     yAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: '利用人数（人）'
                         }
                     }]
                 },
animation: {
    onComplete: function () {
        var ctx = this.chart.ctx;
        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
        ctx.fillStyle = 'black';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'bottom';

        this.data.datasets.forEach(function (dataset)
        {
            for (var i = 0; i < dataset.data.length; i++) {
                for(var key in dataset._meta)
                {
                    var model = dataset._meta[key].data[i]._model;
                    ctx.fillText(dataset.data[i], model.x, model.y - 5);
                }
            }
        });
        done" + kk + @"();
    }
}
             }

         };
 var ctx" + kk + @" = document.getElementById('canvas" + kk + @"').getContext('2d');
             window.myLine = new Chart(ctx" + kk + @", config" + kk + @");

function done" + kk + @"(){
  var url_base64 = document.getElementById('canvas" + kk + @"').toDataURL('image/png');
  link" + kk + @".href = url_base64;
}
</script>
<canvas id='canvas" + kk + @"'></canvas>
<br/>
<a id='link" + kk + @"' download='" + mamaname + @".png'>Save as Image</a>
<br/>
  </fieldset>
";

                    res_list.Add(ress);
                    //                res += @"<script type='text/javascript'>
                    //
                    //</script>";        


                    //result = res;


                    //result = "fail";
                }
                result = @"<fieldset>
    <legend>ALL mamaro</legend><br/>
<script type='text/javascript'>
         var config = {
             type: 'line',
             data: {
                 labels: [" + label + @"],
                 datasets: [" + res1 + @"]
             },
             options: {
                 responsive: true,
                 title: {
                     display: true,
                     text: 'ALL mamaro'
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
                             labelString: '滞在時間（分）'
                         }
                     }],
                     yAxes: [{
                         display: true,
                         scaleLabel: {
                             display: true,
                             labelString: '利用人数（人）'
                         }
                     }]
                 },
                 animation: {
                     onComplete: done
                 }
             }
         };
 var ctx = document.getElementById('canvas').getContext('2d');
             window.myLine = new Chart(ctx, config);
function done(){
  var url_base64 = document.getElementById('canvas').toDataURL('image/png');
  link.href = url_base64;
}
</script>
<canvas id='canvas'></canvas>
<br/>
<a id='link' download='allmamaro.png'>Save as Image</a>
<br/><br/>
  </fieldset>
";

                for (int hh = 0; hh < res_list.Count; hh++)
                {
                    result += res_list[hh];
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