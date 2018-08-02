using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_QA : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string result_cmd = "";
    public class img_group
    {
        public string id = "";
        public string url = "";
    }
    protected void Page_Load(object sender, EventArgs e)
    {



        List<img_group> lis = new List<img_group>();
        img_group imf = new img_group();

        Query = "select t.id,CONCAT('https://storage.googleapis.com//nursing_room/',t.foo) as fooz from( SELECT SUBSTRING_INDEX(url,'/nursing_room/',-1) as foo,id FROM MakeLocaldb1.nursing_room_grid_eye where url like 'https://s3-ap-northeast-1.amazonaws.com//nursing_room/%' ) as t;";
        DataView ict2 = gc.select_cmd(Query);
        if (ict2.Count > 0)
        {
            for (int i = 0; i < ict2.Count; i++)
            {
                imf = new img_group();
                imf.id = ict2.Table.Rows[i]["id"].ToString();
                imf.url = ict2.Table.Rows[i]["fooz"].ToString();
                lis.Add(imf);
            }

            for (int i = 0; i < lis.Count; i++)
            {
                Panel1.Controls.Add(new LiteralControl("id:" + lis[i].id + ",url:" + lis[i].url + "</br>"));

                Query = "update nursing_room_grid_eye set url='" + lis[i].url + "' where id='" + lis[i].id + "';";
                result_cmd = gc.update_cmd(Query);
            }
        }

    }
}
