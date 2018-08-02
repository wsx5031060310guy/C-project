using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class nursing_room_counter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public class state_group_res
    {
        public DateTime firstdate;
        public DateTime seconddate;
        public double diff = 0;
        public string content = "";
    }
    public class state_group
    {
        public DateTime update;
        public int id = 0;
    }
    static List<state_group> state_list = new List<state_group>();
    static List<state_group_res> state_list_res = new List<state_group_res>();
    static List<state_group_res> state_list_user = new List<state_group_res>();
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string check_counter(string param1, string param2)
    {
        int howmany = 0;
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        int QAcou = 0;
        string result = "";
        DateTime start = Convert.ToDateTime(param1.Trim());
        DateTime end = Convert.ToDateTime(param2.Trim());
        try
        {
            if (start != null && end != null)
            {
                state_group sg = new state_group();
                state_group_res gs = new state_group_res();
                state_list = new List<state_group>();
                state_list_res = new List<state_group_res>();
                state_list_user = new List<state_group_res>();

                //state time
                Query = "select * from nursing_room_normal_counter where (nursing_room_normal_id=1 or nursing_room_normal_id=2) and update_time between '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "' order by update_time asc;";
                DataView ict = gc.select_cmd(Query);
                if (ict.Count > 0)
                {

                    for (int i = 0; i < ict.Count; i++)
                    {
                        if (Convert.ToInt32(ict.Table.Rows[i]["state"].ToString()) == 1)
                        {
                            sg = new state_group();
                            sg.id = Convert.ToInt32(ict.Table.Rows[i]["nursing_room_normal_id"].ToString());
                            //sg.state = Convert.ToInt32(ict.Table.Rows[i]["state"].ToString());
                            sg.update = Convert.ToDateTime(ict.Table.Rows[i]["update_time"].ToString());

                            state_list.Add(sg);
                        }
                    }
                }

                DateTime firstdate;
                DateTime seconddate;
                 double compute_day=0;
                for (int i = 0; i < state_list.Count; i++)
                {
                    if (state_list[i].id == 1)
                    {
                        firstdate = Convert.ToDateTime(state_list[i].update);
                        for (int ii = i+1; ii < state_list.Count; ii++)
                        {
                            if (state_list[ii].id == 2)
                            {
                                seconddate = Convert.ToDateTime(state_list[ii].update);
                                compute_day= (seconddate - firstdate).TotalSeconds;
                                if (compute_day < 15)
                                {
                                    gs = new state_group_res();
                                    gs.firstdate = firstdate;
                                    gs.seconddate = seconddate;
                                    gs.content = "Out to In";
                                    gs.diff = compute_day;
                                    state_list_res.Add(gs);
                                    howmany += 1;
                                    i = ii;
                                    break;
                                }
                            }
                        }
                      
                    }
                    else if (state_list[i].id == 2)
                    {
                        firstdate = Convert.ToDateTime(state_list[i].update);
                        for (int ii = i + 1; ii < state_list.Count; ii++)
                        {
                            if (state_list[ii].id == 1)
                            {
                                seconddate = Convert.ToDateTime(state_list[ii].update);
                                compute_day = (seconddate - firstdate).TotalSeconds;
                                if (compute_day < 15)
                                {
                                    gs = new state_group_res();
                                    gs.firstdate = firstdate;
                                    gs.seconddate = seconddate;
                                    gs.content = "In to Out";
                                    gs.diff = compute_day;
                                    state_list_res.Add(gs);
                                    howmany += 1;
                                    i = ii;
                                    break;
                                }
                            }
                        }
                    }
                }
                int howmany_u = 0;
                result += @"<fieldset>
    <legend>Door State</legend><br/>";

                result += @"<span>Total door times:</span>
<br/>
<h2>" + howmany.ToString() + @"</h2>
<br/>";
                for (int i = 0; i < state_list_res.Count; i++)
                {
                    if (state_list_res[i].content == "Out to In")
                    {
                        firstdate = Convert.ToDateTime(state_list_res[i].firstdate);
                        for (int ii = i + 1; ii < state_list_res.Count; ii++)
                        {
                            if (state_list_res[ii].content == "In to Out")
                            {
                                seconddate = Convert.ToDateTime(state_list_res[ii].seconddate);
                                compute_day = (seconddate - firstdate).TotalSeconds;
                                gs = new state_group_res();
                                gs.firstdate = firstdate;
                                gs.seconddate = seconddate;
                                gs.content = "Set";
                                gs.diff = compute_day;
                                state_list_user.Add(gs);
                                howmany_u += 1;
                                i = ii;
                                break;
                            }
                        }
                    }
                }
                result += @"<span>Total users:</span>
<br/>
<h2>" + howmany_u.ToString() + @"</h2>
<br/>";
                for (int i = 0; i < state_list_user.Count; i++)
                {
                    result += @"<span>Start : " + state_list_user[i].firstdate + @" ~ End : " + state_list_user[i].seconddate + @"</span><br/>";
                    result += @"<span>" + state_list_user[i].content + @" : use " + state_list_user[i].diff + @" seconds</span><br/>";
                }

  result += @"</fieldset>";


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