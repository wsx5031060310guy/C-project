using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_analysis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        string Query = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        javascriptPanel.Controls.Clear();
        Query = "select a.video_des,a.company_name,count(a.id) as totalplay";
        Query += " from nursing_room_video_info as a";
        Query += " inner join nursing_room_video_play_detail as b";
        Query += " on a.id=b.nursing_room_video_info_id";
        Query += " group by a.id";
        DataView ict_place = gc.select_cmd(Query);

        string res = "";

        //mamaro video total play times
        res += @"<script>
window.onload = function () {

var options = {
	animationEnabled: true,
	title: {
		text: " + '"' + @"mamaro Video" + '"' + @"
	},
	data: [{
		type: " + '"' + @"doughnut" + '"' + @",
		innerRadius: " + '"' + @"40%" + '"' + @",
		showInLegend: true,
		legendText: " + '"' + @"{label}" + '"' + @",
		indexLabel: " + '"' + @"{label}: #percent%" + '"' + @",
		dataPoints: [";
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                res += @" { label: " + '"' + ict_place.Table.Rows[i]["video_des"].ToString() + '"' + @", y: " + ict_place.Table.Rows[i]["totalplay"].ToString() + @" },";
            }
        }

        res += @"]
	}]
};
$(" + '"' + @"#chartContainer" + '"' + @").CanvasJSChart(options);";

        //mamaro time analysis
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        Color randomColor;
        String strHtmlColor;
        tg = new List<time_group>();
        time_group tig = new time_group();

        res += @"options = {
	animationEnabled: true,
	theme: " + '"' + @"light2" + '"' + @",
	title:{
		text: " + '"' + @" time analysis" + '"' + @"
	},
	axisY2:{
		prefix: " + '"' + @"" + '"' + @",
		lineThickness: 0
	},
	toolTip: {
		shared: true
	},
	legend:{
		verticalAlign: " + '"' + @"top" + '"' + @",
		horizontalAlign: " + '"' + @"center" + '"' + @"
	},
	data: [";


        Query = "select video_des,company_name,id";
        Query += " from nursing_room_video_info;";
        ict_place = gc.select_cmd(Query);
        DataView ict_pl;
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                tig = new time_group();
                tig.id = ict_place.Table.Rows[i]["id"].ToString();
                randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                strHtmlColor = System.Drawing.ColorTranslator.ToHtml(randomColor);
                tig.stu += @"
	{
		type: " + '"' + @"stackedBar" + '"' + @",
		showInLegend: true,
		name: " + '"' + ict_place.Table.Rows[i]["video_des"].ToString() + '"' + @",
		axisYType: " + '"' + @"secondary" + '"' + @",
		color: " + '"' + strHtmlColor + '"' + @",
		dataPoints: [";

                tg.Add(tig);
            }
        }
        for (int i = 9; i < 24; i++)
        {
            string timestr = i+"時-"+(i+1)+"時";
            Query = "select a.id,a.video_des,a.company_name,count(a.id) as totalplay";
            Query += " from nursing_room_video_info as a";
            Query += " inner join nursing_room_video_play_detail as b";
            Query += " on a.id=b.nursing_room_video_info_id";
            Query += " where HOUR(b.update_time) between "+i+" and "+(i+1)+"";
            Query += " group by a.id";
            ict_pl = gc.select_cmd(Query);
            if (ict_pl.Count > 0)
            {
                for (int j = 0; j < ict_pl.Count; j++)
                {
                    for (int ii = 0; ii < tg.Count; ii++)
                    {
                        if (tg[ii].id == ict_pl.Table.Rows[j]["id"].ToString())
                        {
                            tg[ii].stu += @"{ y: " + ict_pl.Table.Rows[j]["totalplay"].ToString() + ", label: " + '"' + timestr + '"' + @" },";
                        }
                    }
                }
            }

        }

        for (int i = 0; i < tg.Count; i++)
        {
            tg[i].stu+= @"]
	},";
        }

        for (int i = 0; i < tg.Count; i++)
        {
            res+=tg[i].stu ;
        }


        res += @"]
};";

        res += @"$(" + '"' + @"#chartContainer1" + '"' + @").CanvasJSChart(options);
";

        //mamaro play how many video
        tgm_list = new List<time_group_mamaro>();
        time_group_mamaro tgm = new time_group_mamaro();
        Query = "select c.id as nid,c.name,a.id,a.video_des,a.company_name,count(a.id) as totalplay";
        Query += " from nursing_room_video_info as a";
        Query += " inner join nursing_room_video_play_detail as b";
        Query += " on a.id=b.nursing_room_video_info_id";
        Query += " inner join nursing_room as c";
        Query += " on b.nursing_room_id=c.id";
        Query += " group by c.id,a.id";
        ict_place = gc.select_cmd(Query);
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                tgm = new time_group_mamaro();
                tgm.id = ict_place.Table.Rows[i]["nid"].ToString();
                tgm.name = ict_place.Table.Rows[i]["name"].ToString();
                tgm.video_des = ict_place.Table.Rows[i]["video_des"].ToString();
                tgm.total = Convert.ToInt32(ict_place.Table.Rows[i]["totalplay"].ToString());
                tgm_list.Add(tgm);
            }
        }

        res+=@"options = {
	animationEnabled: true,
	title: {
		text: " + '"' + @" Play Video Times" + '"' + @"
	},
	data: [{
		type: " + '"' + @"doughnut" + '"' + @",
		innerRadius: " + '"' + @"40%" + '"' + @",
		showInLegend: true,
		legendText: " + '"' + @"{label}" + '"' + @",
		indexLabel: " + '"' + @"{label}: #percent%" + '"' + @",
		dataPoints: [";

        string smalljava = "";

        string choice = "";
//        string choice = @"<script>
//  $( function() {
//    $( " + '"' + @"#accordion" + '"' + @" ).accordion({
//      heightStyle: " + '"' + @"content" + '"' + @"
//    });
//  } );
//  </script>
//<div id=" + '"' + @"accordion" + '"' + @">";
        Query = "select id,name";
        Query += " from nursing_room;";
        ict_place = gc.select_cmd(Query);
        if (ict_place.Count > 0)
        {
            for (int i = 0; i < ict_place.Count; i++)
            {
                int total = 0;
                string name = ict_place.Table.Rows[i]["name"].ToString();
                bool chc = false;
                string cho = @"
options = {
	animationEnabled: true,
	title: {
		text: " + '"' + name + '"' + @"
	},
	data: [{
		type: " + '"' + @"doughnut" + '"' + @",
		innerRadius: " + '"' + @"40%" + '"' + @",
		showInLegend: true,
		legendText: " + '"' + @"{label}" + '"' + @",
		indexLabel: " + '"' + @"{label}: #percent%" + '"' + @",
		dataPoints: [";

                for(int ii=0;ii<tgm_list.Count;ii++)
                {
                    if (ict_place.Table.Rows[i]["id"].ToString() == tgm_list[ii].id)
                    {
                        total += tgm_list[ii].total;

                        //mamaro detail
                        cho += @" { label: " + '"' + tgm_list[ii].video_des + '"' + @", y: " + tgm_list[ii].total + @" },";
                        chc = true;
                    }
                }
                cho += @"]
	}]
};
$(" + '"' + @"#chartContainer_" + i.ToString() + '"' + @").CanvasJSChart(options);";

                if (chc)
                {
                    smalljava += cho;
                }
                if (total > 0)
                {
                    res += @" { label: " + '"' + name + '"' + @", y: " + total + @" },";

                    //mamaro detail
                    //choice += "<h3>" + name + "</h3><div style=" + '"' + @"height: 300px; width: 100%;" + '"' + @"><div id=" + '"' + @"chartContainer_" + i.ToString() + '"' + @" style=" + '"' + @"height: 300px; width: 100%;" + '"' + @"></div></div>";
                    choice += "<h3>" + name + "</h3><div id=" + '"' + @"chartContainer_" + i.ToString() + '"' + @" style=" + '"' + @"height: 300px; width: 100%;" + '"' + @"></div>";
                }

            }
        }
        //choice += "</div>";


        res += @"]
	}]
};
$(" + '"' + @"#chartContainer2" + '"' + @").CanvasJSChart(options);";


        res += smalljava;

        res += @"
}
</script>
";
        detailPanel.Controls.Add(new LiteralControl(choice));
        javascriptPanel.Controls.Add(new LiteralControl(res));


    }
    public List<time_group> tg = new List<time_group>();
    public class time_group
    {
        public string id = "";
        public string stu = "";

    }
    public List<time_group_mamaro> tgm_list = new List<time_group_mamaro>();
    public class time_group_mamaro
    {
        public string id = "";
        public string name = "";
        public string video_des = "";
        public int total = 0;

    }
}
