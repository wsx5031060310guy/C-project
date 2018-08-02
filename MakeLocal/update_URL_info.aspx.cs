using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class update_URL_info : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string result_cmd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["seak_URLinfo"] != null)
        {
            if (Session["seak_URLinfo"].ToString() == "true")
            {
                ListBox1.Items.Clear();
                ListBox2.Items.Clear();
                ListBox3.Items.Clear();
                ListBox4.Items.Clear();
                ListBox5.Items.Clear();
                ListBox6.Items.Clear();

                gc = new GCP_MYSQL();


                Query = "select *,TIMESTAMPDIFF(minute, update_time, NOW()) as difftime from status_messages_link_info order by difftime DESC LIMIT 50;";

                DataView ict2 = gc.select_cmd(Query);
                URL_data URL_info = new URL_data();
                if (ict2.Count > 0)
                {
                    for (int i = 0; i < ict2.Count; i++)
                    {
                        if (Convert.ToInt32(ict2.Table.Rows[i]["difftime"].ToString()) > 30)
                        {

                            URL_info = new URL_data();
                            URL_info = ConvertUrlsToDIV(ict2.Table.Rows[i]["link"].ToString());

                            Query = "update status_messages_link_info set image_url='" + URL_info.image_url + "',title='" + URL_info.title + "'";
                            Query += ",des='" + URL_info.des + "',update_time=NOW() where id='" + ict2.Table.Rows[i]["id"].ToString() + "';";
                            result_cmd = gc.update_cmd(Query);

                            ListBox1.Items.Add(URL_info.url);
                            ListBox2.Items.Add(URL_info.title);
                            ListBox3.Items.Add(URL_info.des);
                            ListBox4.Items.Add(URL_info.image_url);
                            ListBox6.Items.Add(URL_info.update_time);
                        }
                    }
                }
            }
        }
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
        Session["seak_URLinfo"] = "false";
        if (DateTime.Now.Minute % 10 == 0)
        {
            if (DateTime.Now.Second > 55)
            {
                Session["seak_URLinfo"] = "true";
                Label3.Text = DateTime.Now.ToString();
                Response.Redirect("update_URL_info.aspx");
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        ListBox2.Items.Clear();
        ListBox3.Items.Clear();
        ListBox4.Items.Clear();
        ListBox5.Items.Clear();
        ListBox6.Items.Clear();
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
        WebService2.LinkDetails wss = new WebService2.LinkDetails();
        WebService2 ws = new WebService2();
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
        urld.url = wss.Url;
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
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        List<URL_data> txt = new List<URL_data>();
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            string Query1 = "select id from status_messages_link_info where link like '" + match.Value + "';";

            DataView ict1 = gc1.select_cmd(Query1);
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