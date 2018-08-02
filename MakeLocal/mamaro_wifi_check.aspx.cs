using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_wifi_check : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        string Query = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        Panel1.Controls.Clear();

        DateTime maxtime;
        Query = "select nursing_room_id,max(update_time) as maxtime from MakeLocaldb1.nursing_room_wifi_state where TIMESTAMPDIFF(HOUR,update_time, CONVERT(NOW(),DATETIME))<24 group by nursing_room_id;";
        DataView ict = gc.select_cmd(Query);
        if (ict.Count > 0)
        {
            for (int i = 0; i < ict.Count; i++)
            {
                GCP_MYSQL gc1 = new GCP_MYSQL();
                string Query1 = "";
                maxtime = Convert.ToDateTime(ict.Table.Rows[i]["maxtime"].ToString());
                Query1 = "select a.id,b.name,b.address,b.GPS_lat,b.GPS_lng,b.company,b.company_tel,b.remote_name";
                Query1 += " from nursing_room_wifi_state as a inner join nursing_room as b on b.id=a.nursing_room_id";
                Query1 += " where a.nursing_room_id='" + ict.Table.Rows[i]["nursing_room_id"].ToString() + "' and a.update_time between'" + maxtime.ToString("yyyy-MM-dd HH:mm:ss") + ".000" + "' and '" + maxtime.ToString("yyyy-MM-dd HH:mm:ss") + ".999" + "' order by a.update_time;";
                DataView ict1 = gc1.select_cmd(Query1);
                if (ict1.Count > 0)
                {
                    for (int i1 = 0; i1 < ict1.Count; i1++)
                    {
                        Panel1.Controls.Add(new LiteralControl("<fieldset><legend style='font-size: large; font-weight: bold'>" + ict1.Table.Rows[i1]["name"].ToString() + "</legend>"));
                        Panel1.Controls.Add(new LiteralControl("Connect name: " + ict1.Table.Rows[i1]["remote_name"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                      

                        Panel1.Controls.Add(new LiteralControl("ID: " + ict.Table.Rows[i]["nursing_room_id"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("update_time: " + ict.Table.Rows[i]["maxtime"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("address: " + ict1.Table.Rows[i1]["address"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("GPS_lat: " + ict1.Table.Rows[i1]["GPS_lat"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("GPS_lng: " + ict1.Table.Rows[i1]["GPS_lng"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("company: " + ict1.Table.Rows[i1]["company"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("company_tel: " + ict1.Table.Rows[i1]["company_tel"].ToString()));
                        Panel1.Controls.Add(new LiteralControl("<br/>"));
                        Panel1.Controls.Add(new LiteralControl("</fieldset>"));
                    }
                }


            }
        }
    }
}