using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Get_data_from_URL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    int count = 0;
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Label3.Text = DateTime.Now.ToString();
        count += 1;
        //if (DateTime.Now.Minute % 10 == 0)
        //{
        //    Response.Redirect("twitter_gov.aspx");
        //}
        //Session["seak_URL"] = "false";
        //if (DateTime.Now.Minute % 59 == 0)
        //{
        //    if (DateTime.Now.Second > 55)
        //    {
        //        Session["seak_URL"] = "true";
        //        Label3.Text = DateTime.Now.ToString();
        //        Response.Redirect("Get_data_from_URL.aspx");
        //    }
        //}
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        ListBox2.Items.Clear();
        ListBox3.Items.Clear();
        ListBox4.Items.Clear();
        ListBox5.Items.Clear();
        ListBox6.Items.Clear();

        //SqlDataSource sql2 = new SqlDataSource();
        //sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sql2.SelectCommand = "select message from status_messages where id>=53234;";
        //sql2.DataBind();
        //DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
        //List<URL_data> URL_info = new List<URL_data>();
        //if (ict1.Count > 0)
        //{
        //    for (int ix = 0; ix < 100; ix++)
        //    {
        //       ListBox5.Items.Add(ict1.Table.Rows[ix]["message"].ToString());
        //       List<URL_data> URL_info_new = new List<URL_data>();
        //       URL_info_new = ConvertUrlsToLinks_DIV(ict1.Table.Rows[ix]["message"].ToString());
        //       //SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')

        //       //if ((DateTime.Now - dt).TotalDays < 14)
        //       //{
        //       //    Image img = new Image();
        //       //    img.ImageUrl = "~/images/home_images/new.png";
        //       //    pdn_list1.Controls.Add(img);
        //       //}
        //       for (int i = 0; i < URL_info_new.Count; i++)
        //       {
        //           URL_info.Add(URL_info_new[i]);
        //       }
        //    }
        //    for (int i = 0; i < URL_info.Count; i++)
        //    {
        //        ListBox1.Items.Add(URL_info[i].url);
        //        ListBox2.Items.Add(URL_info[i].title);
        //        ListBox3.Items.Add(URL_info[i].des);
        //        ListBox4.Items.Add(URL_info[i].image_url);
        //    }
        //}

        SqlDataSource sql3 = new SqlDataSource();
        sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql3.SelectCommand = "select * from status_messages where id>=53234;";
        sql3.DataBind();
        DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
        List<URL_data> URL_info = new List<URL_data>();
        if (ict2.Count > 0)
        {
            for (int ix = 600; ix < ict2.Count; ix++)
            {
                ListBox5.Items.Add(ict2.Table.Rows[ix]["message"].ToString());
                List<URL_data> URL_info_new = new List<URL_data>();
                URL_info_new = ConvertUrlsToLinks_DIV(ict2.Table.Rows[ix]["message"].ToString());
                //SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')

                //if ((DateTime.Now - dt).TotalDays < 14)
                //{
                //    Image img = new Image();
                //    img.ImageUrl = "~/images/home_images/new.png";
                //    pdn_list1.Controls.Add(img);
                //}
                string indx=ict2.Table.Rows[ix]["year"].ToString()+"-"+ict2.Table.Rows[ix]["month"].ToString()+"-"+ict2.Table.Rows[ix]["day"].ToString()
                    +" "+ict2.Table.Rows[ix]["hour"].ToString()+":"+ict2.Table.Rows[ix]["minute"].ToString()+":"+ict2.Table.Rows[ix]["second"].ToString();
                DateTime indate = Convert.ToDateTime(indx);
                for (int i = 0; i < URL_info_new.Count; i++)
                {
                    URL_info_new[i].update_time = indate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                for (int i = 0; i < URL_info_new.Count; i++)
                {
                    SqlDataSource sql2 = new SqlDataSource();
                    sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql2.SelectCommand = "select id from status_messages_link_info where link like '" + URL_info_new[i].url + "';";
                    sql2.DataBind();
                    DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
                    if (ict1.Count == 0)
                    {
                        SqlDataSource sql2_in = new SqlDataSource();
                        sql2_in.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql2_in.InsertCommand = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
                        sql2_in.InsertCommand += " values(N'" + URL_info_new[i].url + "',N'" + URL_info_new[i].image_url + "',N'" + URL_info_new[i].title + "',N'" + URL_info_new[i].des + "','" + URL_info_new[i].update_time + "');";
                        sql2_in.Insert();
                    }

                    URL_info.Add(URL_info_new[i]);
                }
            }
            for (int i = 0; i < URL_info.Count; i++)
            {
                ListBox1.Items.Add(URL_info[i].url);
                ListBox2.Items.Add(URL_info[i].title);
                ListBox3.Items.Add(URL_info[i].des);
                ListBox4.Items.Add(URL_info[i].image_url);
                ListBox6.Items.Add(URL_info[i].update_time);
            }
        }

        //ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString())
    }
    public static bool UrlExists(string url)
    {
        try
        {
            new System.Net.WebClient().DownloadData(url);
            return true;
        }
        catch (System.Net.WebException e)
        {
            return false;
            throw;
        }
    }
    public static URL_data ConvertUrlsToDIV(string url)
    {
        WebService.LinkDetails wss = new WebService.LinkDetails();
        WebService ws = new WebService();
        wss = ws.GetDetails(url);
        string imgurl = "";
        if (wss.Image != null)
        {
            imgurl = wss.Image.Url;
        }
        else if (wss.Images != null)
        {
            if (wss.Images.Count > 0)
            {
                imgurl = wss.Images[0].Url;
            }
        }
        URL_data urld = new URL_data();
        urld.url = url;
        if (wss.Title != null)
        {
            urld.title = wss.Title;
        }
        if (wss.Description != null)
        {
            urld.des = wss.Description;
        }

        if (imgurl != "")
        {
            if (UrlExists(imgurl))
            {
                urld.image_url = imgurl;
            }
        }

        return urld;
    }
    public class URL_data
    {
        public string url = "";
        public string image_url = "";
        public string title = "";
        public string des = "";
        public string update_time = "";
    }
    public static List<URL_data> ConvertUrlsToLinks_DIV(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        List<URL_data> txt = new List<URL_data>();
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            SqlDataSource sql2 = new SqlDataSource();
            sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql2.SelectCommand = "select id from status_messages_link_info where link like '" + match.Value + "';";
            sql2.DataBind();
            DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
            if (ict1.Count == 0)
            {
                txt.Add(ConvertUrlsToDIV(match.Value));
            }
        }
        return txt;
    }
    public static string ConvertUrlsToLinks(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);

        MatchCollection mactches = r.Matches(msg);
        string txt = "";
        foreach (Match match in mactches)
        {
            //txt += GetMetaTagValue(match.Value) + ",";
            msg = msg.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
        }
        return msg;
        //return txt;

        //        msg = Regex.Replace(
        //msg,
        //@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])",
        //delegate(Match match)
        //{
        //    return GetMetaTagValue(match.ToString());

        //    //return string.Format("{0}", match.ToString());
        //});

        //        return msg;

        //return r.Replace(msg, "$1");

        //return GetMetaTagValue(r.Replace(msg, "$1"));

        //return r.Replace(msg, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
    }
    public static string GetMetaTagValue(string url)
    {
        string res = "";
        //Get Title
        WebClient x = new WebClient();
        string source = x.DownloadString(url);
        res = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        return res;
    }
}