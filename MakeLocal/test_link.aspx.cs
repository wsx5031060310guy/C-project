using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class test_link : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Panel1.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV(TextBox1.Text)));
        Label1.Text = "";
        //WebService.LinkDetails wss = new WebService.LinkDetails();
        //WebService ws = new WebService();
        //wss=ws.GetDetails(TextBox1.Text);
        //string imgurl = "";
        //if (wss.Image != null)
        //{
        //    imgurl = wss.Image.Url;
        //}
        //else if (wss.Images != null)
        //{
        //    imgurl = wss.Images[0].Url;
        //}

        //string sharetxt = "";
        //if (wss.Description != null)
        //{
        //    sharetxt = wss.Description;
        //}
        //else if (wss.Title != null)
        //{
        //    sharetxt = wss.Title;
        //}


        //Panel1.Controls.Add(new LiteralControl("<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%; '><a href='" + wss.Url + "'> <img src='" + imgurl + "' alt='Go to W3Schools!' width='100%' height='100px' border='0'><br/>" + sharetxt + "</a></div>"));
        //Label1.Text = "";

        //Label1.Text= ConvertUrlsToLinks(TextBox1.Text);
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
            //if (((System.Net.HttpWebResponse)e.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
            //    return false;
            //else
            //    throw;
        }
    }
    public static string ConvertUrlsToDIV(string url)
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

        string sharetxt = "";
        if (wss.Title != null)
        {
            sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;line-height:30px;'>【" + wss.Title + "】</span>";
        }
        if (wss.Description != null)
        {
            sharetxt += "<br/><span style='font-size:medium;color:black;line-height:27px;'>" + wss.Description + "</span>";
        }



        string res = "";

        res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + wss.Url + "' style='text-decoration:none'>";
        if (imgurl != "")
        {
            if (UrlExists(imgurl))
            {
                res += "<img src='" + imgurl + "' alt='' width='100%' height='200px' border='0'>";
            }
        }
        if (sharetxt != "")
        {
            res += sharetxt;
        }
        res += "</a></div>";



        return res;
    }
    public static string ConvertUrlsToLinks_DIV(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        string txt = "";
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            txt += ConvertUrlsToDIV(match.Value);
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
    private Encoding myDetectEncoding(byte[] data)
    {
        /*  バイト配列をASCIIエンコードで文字列に変換       */
        String s = Encoding.ASCII.GetString(data);
        /*  <meta>タグを抽出するための正規表現              */
        Match mymatch = Regex.Match(s,
            @"<metas+[^>]*charsets*=s*([-_w]+)",
            RegexOptions.IgnoreCase);
        String e = mymatch.Success ? mymatch.Groups[1].Value : "shift-jis";
        return Encoding.GetEncoding(e);
    }
    private System.Data.DataTable GetData()
    {
        System.Data.DataTable dt = new System.Data.DataTable("TestTable");
        dt.Columns.Add("SSN");
        dt.Columns.Add("Employee ID");
        dt.Columns.Add("Member Last Name");
        dt.Columns.Add("Member First Name");
        dt.Columns.Add("Patient Last Name");
        dt.Columns.Add("Patient First Name");
        dt.Columns.Add("Claim No.");
        dt.Columns.Add("Service Line No.");
        dt.Columns.Add("Error Code");
        dt.Columns.Add("Error Message");
        dt.Rows.Add(123456789, 4455, "asdf", "asdf", "sdfg", "xzcv", "dsfgdfg123", 1234, 135004, "some error");
        dt.Rows.Add(123456788, 3344, "rth", "ojoij", "poip", "wer", "aadf124", 1233, 135005, "Some Error");
        dt.Rows.Add(123456787, 2233, "dfg", "sdfg", "vcxb", "cxvb", "UHCAL125", 1223, 135006, "another error");
        return dt;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        var dataTable = GetData();
        StringBuilder builder = new StringBuilder();
        List<string> columnNames = new List<string>();
        List<string> rows = new List<string>();

        foreach (DataColumn column in dataTable.Columns)
        {
            columnNames.Add(column.ColumnName);
        }

        builder.Append(string.Join(",", columnNames.ToArray())).Append("\n");

        foreach (DataRow row in dataTable.Rows)
        {
            List<string> currentRow = new List<string>();

            foreach (DataColumn column in dataTable.Columns)
            {
                object item = row[column];

                currentRow.Add(item.ToString());
            }

            rows.Add(string.Join(",", currentRow.ToArray()));
        }

        builder.Append(string.Join("\n", rows.ToArray()));

        Response.Clear();
        Response.ContentType = "text/csv";
        Response.AddHeader("Content-Disposition", "attachment;filename=myfilename.csv");
        Response.Write(builder.ToString());
        Response.End();
    }
}
