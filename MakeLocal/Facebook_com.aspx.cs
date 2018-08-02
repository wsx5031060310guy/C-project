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
using Facebook;
using HtmlAgilityPack;

public partial class Facebook_com : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["seak_FB"] != null)
        {
            if (Session["seak_FB"].ToString() == "true")
            {
                gc = new GCP_MYSQL();
                ListBox2.Items.Clear();
                ListBox3.Items.Clear();
                ListBox4.Items.Clear();
                ListBox5.Items.Clear();
                ListBox6.Items.Clear();
                ListBox7.Items.Clear();
                List<Posts> posts = new List<Posts>();

                posts = getFBPosts();

                List<facebook_message_group> fbg_list = new List<facebook_message_group>();
                facebook_message_group fbg = new facebook_message_group();
                for (int i = 0; i < posts.Count; i++)
                {


                    ListBox2.Items.Add(posts[i].PostMessage);

                    ListBox4.Items.Add(posts[i].created_time.ToString());
                    //if (posts[i].description != null)
                    //{
                    //    ListBox5.Items.Add(posts[i].description.ToString());
                    //}
                    //if (posts[i].PostShareLink_name != null)
                    //{
                    //    ListBox6.Items.Add(posts[i].PostShareLink_name.ToString());
                    //}
                    //System.Drawing.Image img = posts[i].PostImage;
                    //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV(posts[i].PostShareLink)));



                    //Image imgg = new Image();
                    //imgg.ImageUrl = posts[i].PostPictureUri;
                    //Panel1.Controls.Add(imgg);

                    fbg = new facebook_message_group();
                    int ind = posts[i].PostId.IndexOf("_");
                    fbg.user_id = posts[i].PostId.Substring(0, ind);
                    ListBox6.Items.Add(posts[i].PostId.Substring(0, ind));
                    fbg.mes_id = posts[i].PostId.Substring(ind + 1, posts[i].PostId.Length - ind - 1);
                    ListBox7.Items.Add(posts[i].PostId.Substring(ind + 1, posts[i].PostId.Length - ind - 1));
                    fbg.username = "保育や子育てに繋がる遊び情報サイト［ほいくる］";

                    fbg.messgae = posts[i].PostMessage.Replace("\'", "").Replace("\"", "");

                    if (posts[i].PostShareLink != null)
                    {
                        if (posts[i].PostShareLink != "")
                        {
                            fbg.sharelist = posts[i].PostShareLink;
                            ListBox3.Items.Add(posts[i].PostShareLink);
                        }
                    }

                    if (posts[i].PostPictureUri != null)
                    {
                        if (posts[i].PostPictureUri != "")
                        {
                            fbg.imglist = posts[i].PostPictureUri;
                            ListBox5.Items.Add(posts[i].PostPictureUri);
                        }
                    }

                    fbg.year = posts[i].created_time.Year;
                    fbg.month = posts[i].created_time.Month;
                    fbg.day = posts[i].created_time.Day;
                    fbg.hour = Convert.ToInt32(posts[i].created_time.ToString("HH"));
                    fbg.min = posts[i].created_time.Minute;
                    fbg.sec = posts[i].created_time.Second;

                    fbg_list.Add(fbg);

                }
                for (int i = 0; i < fbg_list.Count; i++)
                {
                    Query = "select facebook_mid";
                    Query += " from facebook_message";
                    Query += " where facebook_mid='" + fbg_list[i].mes_id + "';";

                    DataView ict_sf = gc.select_cmd(Query);
                    if (ict_sf.Count == 0)
                    {
                        Query = "insert into facebook_message(facebook_mid,facebook_uid,facebook_username,facebook_message,year,month,day,hour,minute,second)";
                        Query += " values('" + fbg_list[i].mes_id + "','" + fbg_list[i].user_id + "','" + fbg_list[i].username + "','" + fbg_list[i].messgae + "',";
                        Query += "'" + fbg_list[i].year + "','" + fbg_list[i].month + "','" + fbg_list[i].day + "','" + fbg_list[i].hour + "','" + fbg_list[i].min + "','" + fbg_list[i].sec + "');";

                        resin = gc.insert_cmd(Query);


                        Query = "select id from facebook_message";
                        Query += " where facebook_mid='" + fbg_list[i].mes_id + "' and facebook_uid='" + fbg_list[i].user_id + "' and facebook_username='" + fbg_list[i].username + "'";
                        Query += " and facebook_message='" + fbg_list[i].messgae + "' and year='" + fbg_list[i].year + "' and month='" + fbg_list[i].month + "' and day='" + fbg_list[i].day + "' and hour='" + fbg_list[i].hour + "'";
                        Query += " and minute='" + fbg_list[i].min + "' and second='" + fbg_list[i].sec + "';";
                        DataView ict_ff = gc.select_cmd(Query);
                        if (ict_ff.Count > 0)
                        {
                            string id = ict_ff.Table.Rows[0]["id"].ToString();
                            if (fbg_list[i].sharelist != "")
                            {

                                Query = "insert into facebook_message_link(fmid,share_link)";
                                Query += " values('" + id + "','" + fbg_list[i].sharelist + "');";
                                resin = gc.insert_cmd(Query);
                            }
                            if (fbg_list[i].imglist != "")
                            {
                                Query = "insert into facebook_message_image(fmid,image_url)";
                                Query += " values('" + id + "','~/" + fbg_list[i].imglist + "');";
                                resin = gc.insert_cmd(Query);
                            }

                        }

                    }

                }

                //insert into social media
                //参照元 : 保育や子育てに繋がる遊び情報サイト［ほいくる］ Facebook アカウント<br/><br/>
                //postal_code
                //place
                //type 0
                //message_type 6

                Query = "select id,facebook_message,year,month,day,hour,minute,second";
                Query += " from facebook_message";
                Query += " where smid is null;";
                DataView ict_sff = gc.select_cmd(Query);
                if (ict_sff.Count > 0)
                {
                    for (int i = 0; i < ict_sff.Count; i++)
                    {
                        string message = ict_sff.Table.Rows[i]["facebook_message"].ToString().Replace(@"\t|\n|\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");

                        Query = "select share_link";
                        Query += " from facebook_message_link";
                        Query += " where fmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

                        DataView ict_sff1 = gc.select_cmd(Query);
                        if (ict_sff1.Count > 0)
                        {
                            for (int ii = 0; ii < ict_sff1.Count; ii++)
                            {
                                message += "<br/>" + ict_sff1.Table.Rows[ii]["share_link"].ToString();
                            }
                        }
                        message += "<br/><br/>参照元 : 保育や子育てに繋がる遊び情報サイト［ほいくる］ Facebook アカウント<br/>";
                        //uid 50

                        Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                        Query += " values('52','0','6',NULL,'" + message + "','" + ict_sff.Table.Rows[i]["year"].ToString() + "','" + ict_sff.Table.Rows[i]["month"].ToString() + "'";
                        Query += ",'" + ict_sff.Table.Rows[i]["day"].ToString() + "','" + ict_sff.Table.Rows[i]["hour"].ToString() + "','" + ict_sff.Table.Rows[i]["minute"].ToString() + "','" + ict_sff.Table.Rows[i]["second"].ToString() + "',NULL)";
                        resin = gc.insert_cmd(Query);



                        string smid = "";

                        Query = "select id from status_messages";
                        Query += " where uid='52' and type='0' and message_type='6' and place is null";
                        Query += " and message='" + message + "' and year='" + ict_sff.Table.Rows[i]["year"].ToString() + "' and month='" + ict_sff.Table.Rows[i]["month"].ToString() + "' and day='" + ict_sff.Table.Rows[i]["day"].ToString() + "' and hour='" + ict_sff.Table.Rows[i]["hour"].ToString() + "'";
                        Query += " and minute='" + ict_sff.Table.Rows[i]["minute"].ToString() + "' and second='" + ict_sff.Table.Rows[i]["second"].ToString() + "' and postal_code is null;";
                        DataView ict_f = gc.select_cmd(Query);
                        if (ict_f.Count > 0)
                        {
                            smid = ict_f.Table.Rows[0]["id"].ToString();

                            Query = "select image_url";
                            Query += " from facebook_message_image";
                            Query += " where fmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                            ict_sff1 = gc.select_cmd(Query);
                            if (ict_sff1.Count > 0)
                            {
                                for (int ii = 0; ii < ict_sff1.Count; ii++)
                                {
                                    Query = "insert into status_messages_image(smid,filename)";
                                    Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + ict_sff1.Table.Rows[ii]["image_url"].ToString() + "');";
                                    resin = gc.insert_cmd(Query);
                                }
                            }
                        }

                        Query = "update facebook_message set smid='" + smid + "'";
                        Query += " where id='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                        resin = gc.update_cmd(Query);

                        ConvertUrlsInData(message);

                    }
                }


                //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("http://bit.ly/2hTRHUx")));

                //http://www.city.yokohama.lg.jp/kanagawa/kusei/kutyou/
                // Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("http://www.city.yokohama.lg.jp/kanagawa/kusei/kutyou/")));

                //https://hoiclue.jp/800002984.html?utm_content=bufferc9bbf&utm_medium=social&utm_source=twitter.com&utm_campaign=buffer
                //https://hoiclue.jp/800002984.html
                //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("https://hoiclue.jp/800002984.html")));

            }
        }

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
    public class URL_data
    {
        public string url = "";
        public string image_url = "";
        public string title = "";
        public string des = "";
        public string update_time = "";
    }
    public static void ConvertUrlsToInData(string url)
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


        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
        Query1 += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";
        string resin1 = gc1.insert_cmd(Query1);

    }
    public static void ConvertUrlsInData(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            GCP_MYSQL gc1 = new GCP_MYSQL();
            string Query1 = "select id from status_messages_link_info where link like '" + match.Value + "';";
            DataView ict1 = gc1.select_cmd(Query1);
            if (ict1.Count == 0)
            {
                ConvertUrlsToInData(match.Value);
            }
        }
    }
    static string GetHtmlPage(string strURL)
    {
        String strResult;
        WebRequest objRequest = WebRequest.Create(strURL);
        WebResponse objResponse = objRequest.GetResponse();
        using (var sr = new StreamReader(objResponse.GetResponseStream()))
        {
            strResult = sr.ReadToEnd();
            sr.Close();
        }
        return strResult;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        //TextBox2.Text = GetHtmlPage("https://www.facebook.com/pg/hoiclue/posts/");
        //MessageBox.Show(GetHtmlPage("http://www.awardwinnersonly.com"));

    }
    public class facebook_message_group
    {
        public string mes_id = "";
        public string user_id = "";
        public string username = "";
        public string messgae = "";
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public int hour = 0;
        public int min = 0;
        public int sec = 0;
        public string imglist = "";
        public string sharelist = "";

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        gc = new GCP_MYSQL();
        ListBox2.Items.Clear();
        ListBox3.Items.Clear();
        ListBox4.Items.Clear();
        ListBox5.Items.Clear();
        ListBox6.Items.Clear();
        ListBox7.Items.Clear();
        List<Posts> posts = new List<Posts>();

        posts = getFBPosts();

        List<facebook_message_group> fbg_list = new List<facebook_message_group>();
        facebook_message_group fbg = new facebook_message_group();
        for (int i = 0; i < posts.Count; i++)
        {


            ListBox2.Items.Add(posts[i].PostMessage);

            ListBox4.Items.Add(posts[i].created_time.ToString());
            //if (posts[i].description != null)
            //{
            //    ListBox5.Items.Add(posts[i].description.ToString());
            //}
            //if (posts[i].PostShareLink_name != null)
            //{
            //    ListBox6.Items.Add(posts[i].PostShareLink_name.ToString());
            //}
            //System.Drawing.Image img = posts[i].PostImage;
            //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV(posts[i].PostShareLink)));



            //Image imgg = new Image();
            //imgg.ImageUrl = posts[i].PostPictureUri;
            //Panel1.Controls.Add(imgg);

            fbg = new facebook_message_group();
            int ind = posts[i].PostId.IndexOf("_");
            fbg.user_id = posts[i].PostId.Substring(0, ind);
            ListBox6.Items.Add(posts[i].PostId.Substring(0, ind));
            fbg.mes_id = posts[i].PostId.Substring(ind + 1, posts[i].PostId.Length - ind - 1);
            ListBox7.Items.Add(posts[i].PostId.Substring(ind + 1, posts[i].PostId.Length - ind - 1));
            fbg.username = "保育や子育てに繋がる遊び情報サイト［ほいくる］";

            fbg.messgae = posts[i].PostMessage.Replace("\'", "").Replace("\"", "");

            if (posts[i].PostShareLink != null)
            {
                if (posts[i].PostShareLink != "")
                {
                    fbg.sharelist = posts[i].PostShareLink;
                    ListBox3.Items.Add(posts[i].PostShareLink);
                }
            }

            if (posts[i].PostPictureUri != null)
            {
                if (posts[i].PostPictureUri != "")
                {
                    fbg.imglist = posts[i].PostPictureUri;
                    ListBox5.Items.Add(posts[i].PostPictureUri);
                }
            }

            fbg.year = posts[i].created_time.Year;
            fbg.month = posts[i].created_time.Month;
            fbg.day = posts[i].created_time.Day;
            fbg.hour = Convert.ToInt32(posts[i].created_time.ToString("HH"));
            fbg.min = posts[i].created_time.Minute;
            fbg.sec = posts[i].created_time.Second;

            fbg_list.Add(fbg);

        }
        for (int i = 0; i < fbg_list.Count; i++)
        {
            Query = "select facebook_mid";
            Query += " from facebook_message";
            Query += " where facebook_mid='" + fbg_list[i].mes_id + "';";

            DataView ict_sf = gc.select_cmd(Query);
            if (ict_sf.Count == 0)
            {
                Query = "insert into facebook_message(facebook_mid,facebook_uid,facebook_username,facebook_message,year,month,day,hour,minute,second)";
                Query += " values('" + fbg_list[i].mes_id + "','" + fbg_list[i].user_id + "','" + fbg_list[i].username + "','" + fbg_list[i].messgae + "',";
                Query += "'" + fbg_list[i].year + "','" + fbg_list[i].month + "','" + fbg_list[i].day + "','" + fbg_list[i].hour + "','" + fbg_list[i].min + "','" + fbg_list[i].sec + "');";

                resin = gc.insert_cmd(Query);


                Query = "select id from facebook_message";
                Query += " where facebook_mid='" + fbg_list[i].mes_id + "' and facebook_uid='" + fbg_list[i].user_id + "' and facebook_username='" + fbg_list[i].username + "'";
                Query += " and facebook_message='" + fbg_list[i].messgae + "' and year='" + fbg_list[i].year + "' and month='" + fbg_list[i].month + "' and day='" + fbg_list[i].day + "' and hour='" + fbg_list[i].hour + "'";
                Query += " and minute='" + fbg_list[i].min + "' and second='" + fbg_list[i].sec + "';";
                DataView ict_ff = gc.select_cmd(Query);
                if (ict_ff.Count > 0)
                {
                    string id = ict_ff.Table.Rows[0]["id"].ToString();
                    if (fbg_list[i].sharelist != "")
                    {

                        Query = "insert into facebook_message_link(fmid,share_link)";
                        Query += " values('" + id + "','" + fbg_list[i].sharelist + "');";
                        resin = gc.insert_cmd(Query);
                    }
                    if (fbg_list[i].imglist != "")
                    {
                        Query = "insert into facebook_message_image(fmid,image_url)";
                        Query += " values('" + id + "','~/" + fbg_list[i].imglist + "');";
                        resin = gc.insert_cmd(Query);
                    }

                }

            }

        }

        //insert into social media
        //参照元 : 保育や子育てに繋がる遊び情報サイト［ほいくる］ Facebook アカウント<br/><br/>
        //postal_code
        //place
        //type 0
        //message_type 6

        Query = "select id,facebook_message,year,month,day,hour,minute,second";
        Query += " from facebook_message";
        Query += " where smid is null;";
        DataView ict_sff = gc.select_cmd(Query);
        if (ict_sff.Count > 0)
        {
            for (int i = 0; i < ict_sff.Count; i++)
            {
                string message = ict_sff.Table.Rows[i]["facebook_message"].ToString().Replace(@"\t|\n|\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");

                Query = "select share_link";
                Query += " from facebook_message_link";
                Query += " where fmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

                DataView ict_sff1 = gc.select_cmd(Query);
                if (ict_sff1.Count > 0)
                {
                    for (int ii = 0; ii < ict_sff1.Count; ii++)
                    {
                        message += "<br/>" + ict_sff1.Table.Rows[ii]["share_link"].ToString();
                    }
                }
                message += "<br/><br/>参照元 : 保育や子育てに繋がる遊び情報サイト［ほいくる］ Facebook アカウント<br/>";
                //uid 50

                Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                Query += " values('52','0','6',NULL,'" + message + "','" + ict_sff.Table.Rows[i]["year"].ToString() + "','" + ict_sff.Table.Rows[i]["month"].ToString() + "'";
                Query += ",'" + ict_sff.Table.Rows[i]["day"].ToString() + "','" + ict_sff.Table.Rows[i]["hour"].ToString() + "','" + ict_sff.Table.Rows[i]["minute"].ToString() + "','" + ict_sff.Table.Rows[i]["second"].ToString() + "',NULL)";
                resin = gc.insert_cmd(Query);



                string smid = "";

                Query = "select id from status_messages";
                Query += " where uid='52' and type='0' and message_type='6' and place is null";
                Query += " and message='" + message + "' and year='" + ict_sff.Table.Rows[i]["year"].ToString() + "' and month='" + ict_sff.Table.Rows[i]["month"].ToString() + "' and day='" + ict_sff.Table.Rows[i]["day"].ToString() + "' and hour='" + ict_sff.Table.Rows[i]["hour"].ToString() + "'";
                Query += " and minute='" + ict_sff.Table.Rows[i]["minute"].ToString() + "' and second='" + ict_sff.Table.Rows[i]["second"].ToString() + "' and postal_code is null;";
                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    smid = ict_f.Table.Rows[0]["id"].ToString();

                    Query = "select image_url";
                    Query += " from facebook_message_image";
                    Query += " where fmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                    ict_sff1 = gc.select_cmd(Query);
                    if (ict_sff1.Count > 0)
                    {
                        for (int ii = 0; ii < ict_sff1.Count; ii++)
                        {
                            Query = "insert into status_messages_image(smid,filename)";
                            Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + ict_sff1.Table.Rows[ii]["image_url"].ToString() + "');";
                            resin = gc.insert_cmd(Query);
                        }
                    }
                }

                Query = "update facebook_message set smid='" + smid + "'";
                Query += " where id='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                resin = gc.update_cmd(Query);

                ConvertUrlsInData(message);

            }
        }


        //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("http://bit.ly/2hTRHUx")));

        //http://www.city.yokohama.lg.jp/kanagawa/kusei/kutyou/
        // Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("http://www.city.yokohama.lg.jp/kanagawa/kusei/kutyou/")));

        //https://hoiclue.jp/800002984.html?utm_content=bufferc9bbf&utm_medium=social&utm_source=twitter.com&utm_campaign=buffer
        //https://hoiclue.jp/800002984.html
        //Panel2.Controls.Add(new LiteralControl(ConvertUrlsToLinks_DIV("https://hoiclue.jp/800002984.html")));


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
            sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;'>【" + wss.Title + "】</span>";
        }
        if (wss.Description != null)
        {
            sharetxt += "<br/><span style='font-size:medium;color:black;'>" + wss.Description + "</span>";
        }



        string res = "";

        res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + wss.Url + "' style='text-decoration:none'>";
        if (imgurl != "")
        {
            res += "<img src='" + imgurl + "' alt='' width='100%' height='100px' border='0'>";
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
            txt += GetMetaTagValue(match.Value) + ",";
            msg = msg.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
        }
        return msg;
    }
    public static string GetMetaTagValue(string url)
    {
        string res = "";
        //Get Title
        WebClient x = new WebClient();
        x.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        string source = x.DownloadString(url);
        res = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        return res;
    }
    class Posts
    {
        public string PostId { get; set; }
        public string PostStory { get; set; }
        public string PostMessage { get; set; }
        public string PostPictureUri { get; set; }
        public string PostShareLink { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime created_time { get; set; }
        public string description { get; set; }
        public string PostShareLink_name { get; set; }
    }

    private List<Posts> getFBPosts()
    {
        //Facebook.FacebookClient myfacebook = new Facebook.FacebookClient();
        string AppId = "";
        string AppSecret = "";
        var client = new WebClient();

        string oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", AppId, AppSecret);
        string results = String.Empty;
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(oauthUrl);
        using (var response = request.GetResponse())
        using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
        {
            results = sr.ReadToEnd();
        }
        List<Posts> postsList = new List<Posts>();
        if (results != "")
        {
            Newtonsoft.Json.Linq.JObject jArray = Newtonsoft.Json.Linq.JObject.Parse(results);




            string accessToken = jArray["access_token"].ToString();
            //string accessToken = client.DownloadString(oauthUrl).Split('=')[1];

            FacebookClient myfbclient = new FacebookClient(accessToken);
            string versio = myfbclient.Version;
            var parameters = new Dictionary<string, object>();
            parameters["fields"] = "id,message,full_picture,link,created_time,description,name";
            string myPage = "hoiclue"; // put your page name
            dynamic result = myfbclient.Get(myPage + "/posts", parameters);


            int mycount = result.data.Count;

            for (int i = 0; i < result.data.Count; i++)
            {
                Posts posts = new Posts();

                posts.PostId = result.data[i].id;

                posts.PostPictureUri = result.data[i].full_picture;
                posts.PostMessage = result.data[i].message;
                posts.PostShareLink = result.data[i].link;
                posts.created_time = Convert.ToDateTime(result.data[i].created_time);

                if (result.data[i].description != null)
                {
                    posts.description = result.data[i].description;
                }

                if (result.data[i].name != null)
                {
                    posts.PostShareLink_name = result.data[i].name;
                }

                //if (posts.PostPictureUri != null)
                //{
                //    WebRequest req = WebRequest.Create(posts.PostPictureUri);
                //    WebResponse response = req.GetResponse();
                //    Stream stream = response.GetResponseStream();
                //    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        ms.WriteTo(Response.OutputStream);
                //        posts.PostImage = image;
                //    }


                //   //// ListBox1.Items.Add(posts.PostPictureUri);
                //   // var request = WebRequest.Create(posts.PostPictureUri);
                //   // using (var response = request.GetResponse())
                //   // {
                //   //     using (Stream stream = response.GetResponseStream())
                //   //     {
                //   //         //ListBox1.Items.Add(stream.Length.ToString());
                //   //         StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                //   //         String responseString = reader.ReadToEnd();
                //   //         ListBox1.Items.Add(responseString);
                //   //         //System.Drawing.Bitmap bit = new System.Drawing.Bitmap(stream);
                //   //         //posts.PostImage = bit;
                //   //     }
                //   // }
                //}
                postsList.Add(posts);
            }
        }
        return postsList;

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
        Session["seak_FB"] = "false";
        if (DateTime.Now.Minute % 59 == 0)
        {
            if (DateTime.Now.Second > 55)
            {
                Session["seak_FB"] = "true";
                Label4.Text = DateTime.Now.ToString();
                Response.Redirect("Facebook_com.aspx");
            }
        }
    }
}
