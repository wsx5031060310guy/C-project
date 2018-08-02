using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class counter_data : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string result_cmd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamaro"]) ? Request.QueryString["mamaro"] : Guid.Empty.ToString();
        string text = Page.Request.QueryString.Get("id");
        string id = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-zA-Z0-9\s]", string.Empty);
        Session["mid"] = id;
        string text1 = Page.Request.QueryString.Get("state");
        string QAon = System.Text.RegularExpressions.Regex.Replace(text1, @"[^a-zA-Z0-9\s]", string.Empty);

        Query = "select id from nursing_room_normal where id='" + id + "';";

        DataView ict2 = gc.select_cmd(Query);
        if (ict2.Count > 0)
        {
            string datetimenow = "";
            Query = "SELECT NOW() as now;";
             DataView ict3 = gc.select_cmd(Query);
             if (ict3.Count > 0)
             {
                 datetimenow = ict3.Table.Rows[0]["now"].ToString();
                 Query = "SELECT * FROM db1.nursing_room_normal_counter where nursing_room_normal_id=" + id + " and update_time='" + datetimenow + "' ORDER BY update_time DESC LIMIT 1;";
                 DataView ict4 = gc.select_cmd(Query);
                 if (ict4.Count == 0)
                 {
                     Query = "insert into nursing_room_normal_counter(nursing_room_normal_id,state,update_time)";
                     Query += " VALUES ('" + id + "','" + QAon + "',NOW())";
                     result_cmd = gc.insert_cmd(Query);
                 }
             }

        }

        Panel1.Controls.Add(new LiteralControl("id:" + id + ",state:" + QAon + "," + result_cmd));
    }
}
