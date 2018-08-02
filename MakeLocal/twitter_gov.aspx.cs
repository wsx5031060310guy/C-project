using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class twitter_gov : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string resin = "";
    string Query = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["seak"] != null)
        {
            if (Session["seak"].ToString() == "true")
            {
                ListBox1.Items.Clear();
                ListBox2.Items.Clear();
                ListBox3.Items.Clear();
                ListBox4.Items.Clear();
                ListBox5.Items.Clear();
                ListBox6.Items.Clear();
                ListBox7.Items.Clear();
                ListBox8.Items.Clear();

                //Label4.Text = DateTime.Now.ToString();
                // We'll use WebClient class for reading HTML of web page
                WebClient MyWebClient = new WebClient();

                // Read web page HTML to byte array
                Byte[] PageHTMLBytes;

                PageHTMLBytes = MyWebClient.DownloadData("https://twitter.com/yokohama_KNGW");

                // Convert result from byte array to string
                // and display it in TextBox txtPageHTML
                UTF8Encoding oUTF8 = new UTF8Encoding();

                string allhtml = oUTF8.GetString(PageHTMLBytes);
                //TextBox2.Text = oUTF8.GetString(PageHTMLBytes);

                //get message id
                List<int> index_head_mid = new List<int>();
                string pattern1 = @"<li class=" + "\"" + "js-stream-item stream-item stream-item";
                string input1 = allhtml;
                //TextBox2.Text = pattern1;

                Match m = Regex.Match(input1, pattern1);
                while (m.Success)
                {
                    index_head_mid.Add(m.Index);
                    m = m.NextMatch();
                }
                List<string> head_str_mid = new List<string>();
                string cut_hstr_mid, cut_hstr_mid1, cut_hstr_mid2, cut_hstr_mid3;
                int indexdiv_mid, indexdiv_mid1;
                for (int i = 0; i < index_head_mid.Count - 1; i++)
                {
                    cut_hstr_mid = input1.Substring(index_head_mid[i] + 50, index_head_mid[i + 1] - index_head_mid[i] - 50);
                    indexdiv_mid = cut_hstr_mid.IndexOf(">");
                    cut_hstr_mid1 = cut_hstr_mid.Substring(0, indexdiv_mid);
                    cut_hstr_mid2 = cut_hstr_mid1.Substring(16, cut_hstr_mid1.Length - 16);
                    indexdiv_mid1 = cut_hstr_mid2.IndexOf("\"");
                    cut_hstr_mid3 = cut_hstr_mid2.Substring(0, indexdiv_mid1);
                    ListBox8.Items.Add(cut_hstr_mid3);
                    head_str_mid.Add(cut_hstr_mid3);
                }
                cut_hstr_mid = input1.Substring(index_head_mid[index_head_mid.Count - 1] + 50, input1.Length - index_head_mid[index_head_mid.Count - 1] - 50);
                indexdiv_mid = cut_hstr_mid.IndexOf(">");
                cut_hstr_mid1 = cut_hstr_mid.Substring(0, indexdiv_mid);
                cut_hstr_mid2 = cut_hstr_mid1.Substring(16, cut_hstr_mid1.Length - 16);
                indexdiv_mid1 = cut_hstr_mid2.IndexOf("\"");
                cut_hstr_mid3 = cut_hstr_mid2.Substring(0, indexdiv_mid1);
                ListBox8.Items.Add(cut_hstr_mid3);
                head_str_mid.Add(cut_hstr_mid3);


                List<int> index_head_acc = new List<int>();
                //check user id time
                pattern1 = @"<div class=" + '"' + "stream-item-header" + '"' + ">";
                input1 = allhtml;

                m = Regex.Match(input1, pattern1);
                while (m.Success)
                {
                    index_head_acc.Add(m.Index);
                    m = m.NextMatch();
                }
                List<string> head_str_uid = new List<string>();
                List<string> head_str_uname = new List<string>();
                List<string> head_str_time = new List<string>();

                string cut_hstr, cut_hstr1, cut_hstr2, cut_hstr3, cut_hstr2_n, cut_hstr3_n, cut_hstr2_t, cut_hstr3_t;
                int indexdiv, indexdiv1, indexdiv2, indexdiv1_n, indexdiv2_n, indexdiv1_t, indexdiv2_t;
                for (int i = 0; i < index_head_acc.Count - 1; i++)
                {
                    cut_hstr = input1.Substring(index_head_acc[i] + 32, index_head_acc[i + 1] - index_head_acc[i] - 32);
                    indexdiv = cut_hstr.IndexOf("</div>");
                    //head_str.Add(cut_hstr.Substring(0, indexdiv));
                    cut_hstr1 = cut_hstr.Substring(0, indexdiv);
                    //ListBox7.Items.Add(cut_hstr1);

                    //get user id
                    indexdiv1 = cut_hstr1.IndexOf("data-user-id=" + "\"");
                    cut_hstr2 = cut_hstr1.Substring(indexdiv1 + 14, cut_hstr1.Length - indexdiv1 - 14);
                    indexdiv2 = cut_hstr2.IndexOf("\"");
                    cut_hstr3 = cut_hstr2.Substring(0, indexdiv2);
                    ListBox6.Items.Add(cut_hstr3);
                    head_str_uid.Add(cut_hstr3);

                    //get user name
                    //<strong class="fullname js-action-profile-name show-popup-with-id" data-aria-label-part="">横浜市神奈川区役所</strong>
                    indexdiv1_n = cut_hstr1.IndexOf("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part>");
                    //ListBox7.Items.Add("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part=" + "\"" + "\"" + ">");
                    cut_hstr2_n = cut_hstr1.Substring(indexdiv1_n + 91, cut_hstr1.Length - indexdiv1_n - 91);
                    indexdiv2_n = cut_hstr2_n.IndexOf("</strong>");
                    cut_hstr3_n = cut_hstr2_n.Substring(0, indexdiv2_n);
                    ListBox7.Items.Add(cut_hstr3_n);
                    head_str_uname.Add(cut_hstr3_n);


                    //get time
                    indexdiv1_t = cut_hstr1.IndexOf("class=" + "\"" + "tweet-timestamp js-permalink js-nav js-tooltip" + "\"");
                    cut_hstr2_t = cut_hstr1.Substring(indexdiv1_t + 54, cut_hstr1.Length - indexdiv1_t - 54);
                    indexdiv2_t = cut_hstr2_t.IndexOf(">");
                    cut_hstr3_t = cut_hstr2_t.Substring(0, indexdiv2_t).Replace("title", "").Replace("\"", "").Replace("=", "");

                    ListBox5.Items.Add(cut_hstr3_t);
                    head_str_time.Add(cut_hstr3_t);

                }
                cut_hstr = input1.Substring(index_head_acc[index_head_acc.Count - 1] + 32, input1.Length - index_head_acc[index_head_acc.Count - 1] - 32);
                indexdiv = cut_hstr.IndexOf("</div>");
                //head_str.Add(cut_hstr.Substring(0, indexdiv));
                cut_hstr1 = cut_hstr.Substring(0, indexdiv);
                //ListBox7.Items.Add(cut_hstr1);

                //get user id
                indexdiv1 = cut_hstr1.IndexOf("data-user-id=" + "\"");
                cut_hstr2 = cut_hstr1.Substring(indexdiv1 + 14, cut_hstr1.Length - indexdiv1 - 14);
                indexdiv2 = cut_hstr2.IndexOf("\"");
                cut_hstr3 = cut_hstr2.Substring(0, indexdiv2);
                ListBox6.Items.Add(cut_hstr3);
                head_str_uid.Add(cut_hstr3);

                //get user name
                //<strong class="fullname js-action-profile-name show-popup-with-id" data-aria-label-part="">横浜市神奈川区役所</strong>
                indexdiv1_n = cut_hstr1.IndexOf("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part>");
                //ListBox7.Items.Add("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part=" + "\"" + "\"" + ">");
                cut_hstr2_n = cut_hstr1.Substring(indexdiv1_n + 91, cut_hstr1.Length - indexdiv1_n - 91);
                indexdiv2_n = cut_hstr2_n.IndexOf("</strong>");
                cut_hstr3_n = cut_hstr2_n.Substring(0, indexdiv2_n);
                ListBox7.Items.Add(cut_hstr3_n);
                head_str_uname.Add(cut_hstr3_n);

                //get time
                indexdiv1_t = cut_hstr1.IndexOf("class=" + "\"" + "tweet-timestamp js-permalink js-nav js-tooltip" + "\"");
                cut_hstr2_t = cut_hstr1.Substring(indexdiv1_t + 54, cut_hstr1.Length - indexdiv1_t - 54);
                indexdiv2_t = cut_hstr2_t.IndexOf(">");
                cut_hstr3_t = cut_hstr2_t.Substring(0, indexdiv2_t).Replace("title", "").Replace("\"", "").Replace("=", "");

                ListBox5.Items.Add(cut_hstr3_t);
                head_str_time.Add(cut_hstr3_t);


                //text url img
                List<int> index_head = new List<int>();
                string pattern = @"<div class=" + '"' + "js-tweet-text-container" + '"' + ">";
                string input = allhtml;

                m = Regex.Match(input, pattern);
                while (m.Success)
                {
                    index_head.Add(m.Index);
                    //Console.WriteLine("'{0}' found at index {1}.",
                    //                  m.Value, m.Index);
                    m = m.NextMatch();
                }

                List<int> index_foot = new List<int>();
                pattern = @"<div class=" + '"' + "stream-item-footer" + '"' + ">";
                input = allhtml;

                m = Regex.Match(input, pattern);
                while (m.Success)
                {
                    index_foot.Add(m.Index);
                    //Console.WriteLine("'{0}' found at index {1}.",
                    //                  m.Value, m.Index);
                    m = m.NextMatch();
                }

                List<string> output_res = new List<string>();
                for (int i = 0; i < index_head.Count; i++)
                {
                    output_res.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37));
                    ListBox1.Items.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37));
                    //ListBox1.Items.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--26px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
                    //    .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
                    //    .Replace("</p>", "").Replace("</div>", "")
                    //    );
                }
                List<twitter_message> mesg = new List<twitter_message>();
                twitter_message tm = new twitter_message();
                for (int i = 0; i < output_res.Count; i++)
                {
                    tm = new twitter_message();
                    input = output_res[i];
                    int strend = input.IndexOf("</p>");

                    //text
                    //clear link
                    string firtxt = input.Substring(0, strend).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--normal js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
                        .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "");
                    //ListBox2.Items.Add(input.Substring(0, strend).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--26px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
                    //    .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", ""));

                    int secclear = firtxt.IndexOf("<a");
                    string cleartxt = firtxt;
                    if (secclear > 0)
                    {
                        cleartxt = firtxt.Substring(0, secclear);
                    }
                    ListBox2.Items.Add(cleartxt);
                    tm.messgae = cleartxt;

                    //other url
                    pattern = "data-expanded-url=" + "\"";
                    //TextBox2.Text = pattern;
                    input = output_res[i];

                    List<int> url_list = new List<int>();
                    m = Regex.Match(input, pattern);
                    while (m.Success)
                    {
                        //TextBox2.Text += m.Index.ToString();
                        url_list.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> url_list_str = new List<string>();
                    string oldstr = "", cutstr = "";
                    int linkend = 0;
                    for (int ii = 0; ii < url_list.Count - 1; ii++)
                    {
                        oldstr = input.Substring(url_list[ii] + 19, url_list[ii + 1] - url_list[ii] - 19);
                        linkend = oldstr.IndexOf("\"");

                        cutstr = oldstr.Substring(0, linkend);
                        ListBox3.Items.Add(cutstr);
                        url_list_str.Add(cutstr);
                    }
                    if (url_list.Count > 0)
                    {
                        oldstr = input.Substring(url_list[url_list.Count - 1] + 19, input.Length - url_list[url_list.Count - 1] - 19);
                        linkend = oldstr.IndexOf("\"");
                        cutstr = oldstr.Substring(0, linkend);
                        ListBox3.Items.Add(cutstr);
                        url_list_str.Add(cutstr);
                    }
                    tm.sharelist = url_list_str;

                    //img
                    pattern = "data-image-url=" + "\"";
                    //TextBox2.Text = pattern;
                    input = output_res[i];

                    List<int> img_list = new List<int>();
                    m = Regex.Match(input, pattern);
                    while (m.Success)
                    {
                        //TextBox2.Text += m.Index.ToString();
                        img_list.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> img_list_str = new List<string>();
                    for (int ii = 0; ii < img_list.Count - 1; ii++)
                    {
                        oldstr = input.Substring(img_list[ii] + 16, img_list[ii + 1] - img_list[ii] - 16);
                        linkend = oldstr.IndexOf("\"");

                        cutstr = oldstr.Substring(0, linkend);
                        ListBox4.Items.Add(cutstr);
                        img_list_str.Add(cutstr);
                    }
                    if (img_list.Count > 0)
                    {
                        oldstr = input.Substring(img_list[img_list.Count - 1] + 16, input.Length - img_list[img_list.Count - 1] - 16);
                        linkend = oldstr.IndexOf("\"");
                        cutstr = oldstr.Substring(0, linkend);
                        ListBox4.Items.Add(cutstr);
                        img_list_str.Add(cutstr);
                    }
                    tm.imglist = img_list_str;
                    mesg.Add(tm);

                    //pattern = @"<div class=" + '"' + "stream-item-footer" + '"' + ">";
                    //input = output_res[i];

                    //m = Regex.Match(input, pattern);
                    //while (m.Success)
                    //{
                    //    index_foot.Add(m.Index);
                    //    //Console.WriteLine("'{0}' found at index {1}.",
                    //    //                  m.Value, m.Index);
                    //    m = m.NextMatch();
                    //}

                }
                //head_str_mid
                //head_str_time

                //DateTime dat = new DateTime(Convert.ToDateTime());
                List<twitter_time> listtime = new List<twitter_time>();
                twitter_time tt = new twitter_time();
                for (int i = 0; i < head_str_time.Count; i++)
                {
                    string oldtime = head_str_time[i].Trim();
                    //ListBox9.Items.Add(oldtime);
                    tt = new twitter_time();
                    string texttime = "";
                    int ind = oldtime.IndexOf(":");

                    int ind1 = oldtime.IndexOf(" - ");

                    int ind2 = oldtime.IndexOf("年");

                    int ind3 = oldtime.IndexOf("月");

                    int ind4 = oldtime.IndexOf("日");
                    tt.hour = Convert.ToInt32(oldtime.Substring(0, ind));
                    tt.min = Convert.ToInt32(oldtime.Substring(ind + 1, ind1 - ind - 1));
                    tt.sec = 0;
                    tt.year = Convert.ToInt32(oldtime.Substring(ind1 + 3, ind2 - ind1 - 3));
                    tt.month = Convert.ToInt32(oldtime.Substring(ind2 + 1, ind3 - ind2 - 1));
                    tt.day = Convert.ToInt32(oldtime.Substring(ind3 + 1, ind4 - ind3 - 1));
                    listtime.Add(tt);

                    ListBox9.Items.Add(texttime);
                }


                //head_str_uid
                //head_str_uname
                //mesg
                List<twitter_message_group> tmglist = new List<twitter_message_group>();
                twitter_message_group tmg = new twitter_message_group();

                for (int i = 0; i < head_str_mid.Count; i++)
                {
                    tmg = new twitter_message_group();
                    tmg.mes_id = head_str_mid[i];
                    tmg.user_id = head_str_uid[i];
                    tmg.username = head_str_uname[i];
                    tmg.year = listtime[i].year;
                    tmg.month = listtime[i].month;
                    tmg.day = listtime[i].day;
                    tmg.hour = listtime[i].hour;
                    tmg.min = listtime[i].min;
                    tmg.sec = listtime[i].sec;
                    tmg.messgae = mesg[i].messgae;
                    tmg.imglist = mesg[i].imglist;
                    tmg.sharelist = mesg[i].sharelist;

                    tmglist.Add(tmg);
                }
                for (int i = 0; i < tmglist.Count; i++)
                {
                    if (tmglist[i].user_id == "412871152")
                    {
                        gc = new GCP_MYSQL();
                        Query = "select twitter_mid";
                        Query += " from twitter_message";
                        Query += " where twitter_mid='" + tmglist[i].mes_id + "';";

                        DataView ict_sf =  gc.select_cmd(Query);
                        if (ict_sf.Count == 0)
                        {
                            Query = "insert into twitter_message(twitter_mid,twitter_uid,twitter_username,twitter_message,year,month,day,hour,minute,second)";
                            Query += " values('" + tmglist[i].mes_id + "','" + tmglist[i].user_id + "','横浜市神奈川区役所','" + tmglist[i].messgae + "',";
                            Query += "'" + tmglist[i].year + "','" + tmglist[i].month + "','" + tmglist[i].day + "','" + tmglist[i].hour + "','" + tmglist[i].min + "','" + tmglist[i].sec + "');";
                            resin = gc.insert_cmd(Query);
                            Query = "select id from twitter_message";
                            Query += " where twitter_mid='" + tmglist[i].mes_id + "' and twitter_uid='" + tmglist[i].user_id + "' and twitter_username='横浜市神奈川区役所'";
                            Query += " and twitter_message='" + tmglist[i].messgae + "' and year='" + tmglist[i].year + "' and month='" + tmglist[i].month + "' and day='" + tmglist[i].day + "' and hour='" + tmglist[i].hour + "'";
                            Query += " and minute='" + tmglist[i].min + "' and second='" + tmglist[i].sec + "';";

                            DataView ict_ff = gc.select_cmd(Query);
                            if (ict_ff.Count > 0)
                            {
                                string id = ict_ff.Table.Rows[0]["id"].ToString();
                                for (int ii = 0; ii < tmglist[i].sharelist.Count; ii++)
                                {
                                    Query = "insert into twitter_message_link(tmid,share_link)";
                                    Query += " values('" + id + "','" + tmglist[i].sharelist[ii] + "');";
                                    resin = gc.insert_cmd(Query);
                                }
                                for (int ii = 0; ii < tmglist[i].imglist.Count; ii++)
                                {
                                    Query = "insert into twitter_message_image(tmid,image_url)";
                                    Query += " values('" + id + "','~/" + tmglist[i].imglist[ii] + "');";
                                    resin = gc.insert_cmd(Query);
                                }
                            }

                        }
                    }
                }
                //insert into social media
                //参照元 : 横浜市神奈川区役所 Twitter アカウント<br/><br/>
                //postal_code 221-0824 
                //place 横浜市神奈川区広台太田町3-8
                //type 0
                //message_type 6
                gc = new GCP_MYSQL();
                Query = "select id,twitter_message,year,month,day,hour,minute,second";
                Query += " from twitter_message";
                Query += " where smid is null;";

                DataView ict_sff = gc.select_cmd(Query);
                if (ict_sff.Count > 0)
                {
                    for (int i = 0; i < ict_sff.Count; i++)
                    {
                        string message = ict_sff.Table.Rows[i]["twitter_message"].ToString().Replace(@"\t|\n|\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                        Query = "select share_link";
                        Query += " from twitter_message_link";
                        Query += " where tmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

                        DataView ict_sff1 = gc.select_cmd(Query);
                        if (ict_sff1.Count > 0)
                        {
                            for (int ii = 0; ii < ict_sff1.Count; ii++)
                            {
                                message += "<br/>" + ict_sff1.Table.Rows[ii]["share_link"].ToString();
                            }
                        }
                        message += "<br/><br/>参照元 : 横浜市神奈川区役所 Twitter アカウント<br/>";

                        //uid 50
                        Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                        Query += " values('50','0','6','横浜市神奈川区広台太田町3-8','" + message + "','" + ict_sff.Table.Rows[i]["year"].ToString() + "','" + ict_sff.Table.Rows[i]["month"].ToString() + "'";
                        Query += ",'" + ict_sff.Table.Rows[i]["day"].ToString() + "','" + ict_sff.Table.Rows[i]["hour"].ToString() + "','" + ict_sff.Table.Rows[i]["minute"].ToString() + "','" + ict_sff.Table.Rows[i]["second"].ToString() + "','221-0824')";

                        resin = gc.insert_cmd(Query);

                        string smid = "";
                        Query = "select id from status_messages";
                        Query += " where uid='50' and type='0' and message_type='6' and place='横浜市神奈川区広台太田町3-8'";
                        Query += " and message='" + message + "' and year='" + ict_sff.Table.Rows[i]["year"].ToString() + "' and month='" + ict_sff.Table.Rows[i]["month"].ToString() + "' and day='" + ict_sff.Table.Rows[i]["day"].ToString() + "' and hour='" + ict_sff.Table.Rows[i]["hour"].ToString() + "'";
                        Query += " and minute='" + ict_sff.Table.Rows[i]["minute"].ToString() + "' and second='" + ict_sff.Table.Rows[i]["second"].ToString() + "' and postal_code='221-0824';";

                        DataView ict_f = gc.select_cmd(Query);
                        if (ict_f.Count > 0)
                        {
                            smid = ict_f.Table.Rows[0]["id"].ToString();
                            Query = "select image_url";
                            Query += " from twitter_message_image";
                            Query += " where tmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

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
                        Query = "update twitter_message set smid='" + smid + "'";
                        Query += " where id='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                        resin = gc.update_cmd(Query);

                        ConvertUrlsInData(message);
                    }
                }



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
        GCP_MYSQL gc = new GCP_MYSQL();

        string Query = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
        Query += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";


        string resin = gc.insert_cmd(Query);


    }
    public static void ConvertUrlsInData(string msg)
    {
        GCP_MYSQL gc = new GCP_MYSQL();
        DataView ict1;
        string Query = "";
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            Query = "select id from status_messages_link_info where link like '" + match.Value + "';";
            ict1 = gc.select_cmd(Query);
            if (ict1.Count == 0)
            {
                ConvertUrlsToInData(match.Value);
            }
        }
    }
    public class twitter_message
    {
        public string messgae = "";
        public List<string> imglist = new List<string>();
        public List<string> sharelist = new List<string>();
    }
    public class twitter_time
    {
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public int hour = 0;
        public int min = 0;
        public int sec = 0;
    }
    public class twitter_message_group
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
        public List<string> imglist = new List<string>();
        public List<string> sharelist = new List<string>();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        ListBox2.Items.Clear();
        ListBox3.Items.Clear();
        ListBox4.Items.Clear();
        ListBox5.Items.Clear();
        ListBox6.Items.Clear();
        ListBox7.Items.Clear();
        ListBox8.Items.Clear();

        //Label4.Text = DateTime.Now.ToString();
        // We'll use WebClient class for reading HTML of web page
        WebClient MyWebClient = new WebClient();

        // Read web page HTML to byte array
        Byte[] PageHTMLBytes;

        PageHTMLBytes = MyWebClient.DownloadData("https://twitter.com/yokohama_KNGW");

        // Convert result from byte array to string
        // and display it in TextBox txtPageHTML
        UTF8Encoding oUTF8 = new UTF8Encoding();

        string allhtml = oUTF8.GetString(PageHTMLBytes);
        //TextBox2.Text = oUTF8.GetString(PageHTMLBytes);

        //get message id
        List<int> index_head_mid = new List<int>();
        string pattern1 = @"<li class=" + "\"" + "js-stream-item stream-item stream-item";
        string input1 = allhtml;
        //TextBox2.Text = pattern1;

        Match m = Regex.Match(input1, pattern1);
        while (m.Success)
        {
            index_head_mid.Add(m.Index);
            m = m.NextMatch();
        }
        List<string> head_str_mid = new List<string>();
        string cut_hstr_mid, cut_hstr_mid1, cut_hstr_mid2, cut_hstr_mid3;
        int indexdiv_mid, indexdiv_mid1;
        for (int i = 0; i < index_head_mid.Count - 1; i++)
        {
            cut_hstr_mid = input1.Substring(index_head_mid[i] + 50, index_head_mid[i + 1] - index_head_mid[i] - 50);
            indexdiv_mid = cut_hstr_mid.IndexOf(">");
            cut_hstr_mid1 = cut_hstr_mid.Substring(0, indexdiv_mid);
            cut_hstr_mid2 = cut_hstr_mid1.Substring(16, cut_hstr_mid1.Length - 16);
            indexdiv_mid1 = cut_hstr_mid2.IndexOf("\"");
            cut_hstr_mid3 = cut_hstr_mid2.Substring(0, indexdiv_mid1);
            ListBox8.Items.Add(cut_hstr_mid3);
            head_str_mid.Add(cut_hstr_mid3);
        }
        cut_hstr_mid = input1.Substring(index_head_mid[index_head_mid.Count - 1] + 50, input1.Length - index_head_mid[index_head_mid.Count - 1] - 50);
        indexdiv_mid = cut_hstr_mid.IndexOf(">");
        cut_hstr_mid1 = cut_hstr_mid.Substring(0, indexdiv_mid);
        cut_hstr_mid2 = cut_hstr_mid1.Substring(16, cut_hstr_mid1.Length - 16);
        indexdiv_mid1 = cut_hstr_mid2.IndexOf("\"");
        cut_hstr_mid3 = cut_hstr_mid2.Substring(0, indexdiv_mid1);
        ListBox8.Items.Add(cut_hstr_mid3);
        head_str_mid.Add(cut_hstr_mid3);


        List<int> index_head_acc = new List<int>();
        //check user id time
        pattern1 = @"<div class=" + '"' + "stream-item-header" + '"' + ">";
        input1 = allhtml;

        m = Regex.Match(input1, pattern1);
        while (m.Success)
        {
            index_head_acc.Add(m.Index);
            m = m.NextMatch();
        }
        List<string> head_str_uid = new List<string>();
        List<string> head_str_uname = new List<string>();
        List<string> head_str_time = new List<string>();

        string cut_hstr, cut_hstr1, cut_hstr2, cut_hstr3, cut_hstr2_n, cut_hstr3_n, cut_hstr2_t, cut_hstr3_t;
        int indexdiv, indexdiv1, indexdiv2, indexdiv1_n, indexdiv2_n, indexdiv1_t, indexdiv2_t;
        for (int i = 0; i < index_head_acc.Count - 1; i++)
        {
            cut_hstr = input1.Substring(index_head_acc[i] + 32, index_head_acc[i + 1] - index_head_acc[i] - 32);
            indexdiv = cut_hstr.IndexOf("</div>");
            //head_str.Add(cut_hstr.Substring(0, indexdiv));
            cut_hstr1 = cut_hstr.Substring(0, indexdiv);
            //ListBox7.Items.Add(cut_hstr1);

            //get user id
            indexdiv1 = cut_hstr1.IndexOf("data-user-id=" + "\"");
            cut_hstr2 = cut_hstr1.Substring(indexdiv1 + 14, cut_hstr1.Length - indexdiv1 - 14);
            indexdiv2 = cut_hstr2.IndexOf("\"");
            cut_hstr3 = cut_hstr2.Substring(0, indexdiv2);
            ListBox6.Items.Add(cut_hstr3);
            head_str_uid.Add(cut_hstr3);

            //get user name
            //<strong class="fullname js-action-profile-name show-popup-with-id" data-aria-label-part="">横浜市神奈川区役所</strong>
            indexdiv1_n = cut_hstr1.IndexOf("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part>");
            //ListBox7.Items.Add("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part=" + "\"" + "\"" + ">");
            cut_hstr2_n = cut_hstr1.Substring(indexdiv1_n + 91, cut_hstr1.Length - indexdiv1_n - 91);
            indexdiv2_n = cut_hstr2_n.IndexOf("</strong>");
            cut_hstr3_n = cut_hstr2_n.Substring(0, indexdiv2_n);
            ListBox7.Items.Add(cut_hstr3_n);
            head_str_uname.Add(cut_hstr3_n);


            //get time
            indexdiv1_t = cut_hstr1.IndexOf("class=" + "\"" + "tweet-timestamp js-permalink js-nav js-tooltip" + "\"");
            cut_hstr2_t = cut_hstr1.Substring(indexdiv1_t + 54, cut_hstr1.Length - indexdiv1_t - 54);
            indexdiv2_t = cut_hstr2_t.IndexOf(">");
            cut_hstr3_t = cut_hstr2_t.Substring(0, indexdiv2_t).Replace("title", "").Replace("\"", "").Replace("=", "");

            ListBox5.Items.Add(cut_hstr3_t);
            head_str_time.Add(cut_hstr3_t);

        }
        cut_hstr = input1.Substring(index_head_acc[index_head_acc.Count - 1] + 32, input1.Length - index_head_acc[index_head_acc.Count - 1] - 32);
        indexdiv = cut_hstr.IndexOf("</div>");
        //head_str.Add(cut_hstr.Substring(0, indexdiv));
        cut_hstr1 = cut_hstr.Substring(0, indexdiv);
        //ListBox7.Items.Add(cut_hstr1);

        //get user id
        indexdiv1 = cut_hstr1.IndexOf("data-user-id=" + "\"");
        cut_hstr2 = cut_hstr1.Substring(indexdiv1 + 14, cut_hstr1.Length - indexdiv1 - 14);
        indexdiv2 = cut_hstr2.IndexOf("\"");
        cut_hstr3 = cut_hstr2.Substring(0, indexdiv2);
        ListBox6.Items.Add(cut_hstr3);
        head_str_uid.Add(cut_hstr3);

        //get user name
        //<strong class="fullname js-action-profile-name show-popup-with-id" data-aria-label-part="">横浜市神奈川区役所</strong>
        indexdiv1_n = cut_hstr1.IndexOf("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part>");
        //ListBox7.Items.Add("<strong class=" + "\"" + "fullname js-action-profile-name show-popup-with-id" + "\"" + " data-aria-label-part=" + "\"" + "\"" + ">");
        cut_hstr2_n = cut_hstr1.Substring(indexdiv1_n + 91, cut_hstr1.Length - indexdiv1_n - 91);
        indexdiv2_n = cut_hstr2_n.IndexOf("</strong>");
        cut_hstr3_n = cut_hstr2_n.Substring(0, indexdiv2_n);
        ListBox7.Items.Add(cut_hstr3_n);
        head_str_uname.Add(cut_hstr3_n);

        //get time
        indexdiv1_t = cut_hstr1.IndexOf("class=" + "\"" + "tweet-timestamp js-permalink js-nav js-tooltip" + "\"");
        cut_hstr2_t = cut_hstr1.Substring(indexdiv1_t + 54, cut_hstr1.Length - indexdiv1_t - 54);
        indexdiv2_t = cut_hstr2_t.IndexOf(">");
        cut_hstr3_t = cut_hstr2_t.Substring(0, indexdiv2_t).Replace("title", "").Replace("\"", "").Replace("=", "");

        ListBox5.Items.Add(cut_hstr3_t);
        head_str_time.Add(cut_hstr3_t);


        //text url img
        List<int> index_head = new List<int>();
        string pattern = @"<div class=" + '"' + "js-tweet-text-container" + '"' + ">";
        string input = allhtml;

        m = Regex.Match(input, pattern);
        while (m.Success)
        {
            index_head.Add(m.Index);
            //Console.WriteLine("'{0}' found at index {1}.",
            //                  m.Value, m.Index);
            m = m.NextMatch();
        }

        List<int> index_foot = new List<int>();
        pattern = @"<div class=" + '"' + "stream-item-footer" + '"' + ">";
        input = allhtml;

        m = Regex.Match(input, pattern);
        while (m.Success)
        {
            index_foot.Add(m.Index);
            //Console.WriteLine("'{0}' found at index {1}.",
            //                  m.Value, m.Index);
            m = m.NextMatch();
        }

        List<string> output_res = new List<string>();
        for (int i = 0; i < index_head.Count; i++)
        {
            output_res.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37));
            ListBox1.Items.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37));
            //ListBox1.Items.Add(allhtml.Substring(index_head[i] + 37, index_foot[i] - index_head[i] - 37).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--26px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
            //    .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
            //    .Replace("</p>", "").Replace("</div>", "")
            //    );
        }
        List<twitter_message> mesg = new List<twitter_message>();
        twitter_message tm = new twitter_message();
        for (int i = 0; i < output_res.Count; i++)
        {
            tm = new twitter_message();
            input = output_res[i];
            int strend = input.IndexOf("</p>");

            //text
            //clear link
            string firtxt = input.Substring(0, strend).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--normal js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
                .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "");
            //ListBox2.Items.Add(input.Substring(0, strend).Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--26px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", "")
            //    .Replace("<p class=" + '"' + "TweetTextSize TweetTextSize--16px js-tweet-text tweet-text" + '"' + " lang=" + '"' + "ja" + '"' + " data-aria-label-part=" + '"' + "0" + '"' + ">", ""));

            int secclear = firtxt.IndexOf("<a");
            string cleartxt = firtxt;
            if (secclear > 0)
            {
                cleartxt = firtxt.Substring(0, secclear);
            }
            ListBox2.Items.Add(cleartxt);
            tm.messgae = cleartxt;

            //other url
            pattern = "data-expanded-url=" + "\"";
            //TextBox2.Text = pattern;
            input = output_res[i];

            List<int> url_list = new List<int>();
            m = Regex.Match(input, pattern);
            while (m.Success)
            {
                //TextBox2.Text += m.Index.ToString();
                url_list.Add(m.Index);
                m = m.NextMatch();
            }
            List<string> url_list_str = new List<string>();
            string oldstr = "", cutstr = "";
            int linkend = 0;
            for (int ii = 0; ii < url_list.Count - 1; ii++)
            {
                oldstr = input.Substring(url_list[ii] + 19, url_list[ii + 1] - url_list[ii] - 19);
                linkend = oldstr.IndexOf("\"");

                cutstr = oldstr.Substring(0, linkend);
                ListBox3.Items.Add(cutstr);
                url_list_str.Add(cutstr);
            }
            if (url_list.Count > 0)
            {
                oldstr = input.Substring(url_list[url_list.Count - 1] + 19, input.Length - url_list[url_list.Count - 1] - 19);
                linkend = oldstr.IndexOf("\"");
                cutstr = oldstr.Substring(0, linkend);
                ListBox3.Items.Add(cutstr);
                url_list_str.Add(cutstr);
            }
            tm.sharelist = url_list_str;

            //img
            pattern = "data-image-url=" + "\"";
            //TextBox2.Text = pattern;
            input = output_res[i];

            List<int> img_list = new List<int>();
            m = Regex.Match(input, pattern);
            while (m.Success)
            {
                //TextBox2.Text += m.Index.ToString();
                img_list.Add(m.Index);
                m = m.NextMatch();
            }
            List<string> img_list_str = new List<string>();
            for (int ii = 0; ii < img_list.Count - 1; ii++)
            {
                oldstr = input.Substring(img_list[ii] + 16, img_list[ii + 1] - img_list[ii] - 16);
                linkend = oldstr.IndexOf("\"");

                cutstr = oldstr.Substring(0, linkend);
                ListBox4.Items.Add(cutstr);
                img_list_str.Add(cutstr);
            }
            if (img_list.Count > 0)
            {
                oldstr = input.Substring(img_list[img_list.Count - 1] + 16, input.Length - img_list[img_list.Count - 1] - 16);
                linkend = oldstr.IndexOf("\"");
                cutstr = oldstr.Substring(0, linkend);
                ListBox4.Items.Add(cutstr);
                img_list_str.Add(cutstr);
            }
            tm.imglist = img_list_str;
            mesg.Add(tm);

            //pattern = @"<div class=" + '"' + "stream-item-footer" + '"' + ">";
            //input = output_res[i];

            //m = Regex.Match(input, pattern);
            //while (m.Success)
            //{
            //    index_foot.Add(m.Index);
            //    //Console.WriteLine("'{0}' found at index {1}.",
            //    //                  m.Value, m.Index);
            //    m = m.NextMatch();
            //}

        }
        //head_str_mid
        //head_str_time

        //DateTime dat = new DateTime(Convert.ToDateTime());
        List<twitter_time> listtime = new List<twitter_time>();
        twitter_time tt = new twitter_time();
        for (int i = 0; i < head_str_time.Count; i++)
        {
            string oldtime = head_str_time[i].Trim();
            //ListBox9.Items.Add(oldtime);
            tt = new twitter_time();
            string texttime = "";
            int ind = oldtime.IndexOf(":");

            int ind1 = oldtime.IndexOf(" - ");

            int ind2 = oldtime.IndexOf("年");

            int ind3 = oldtime.IndexOf("月");

            int ind4 = oldtime.IndexOf("日");
            tt.hour = Convert.ToInt32(oldtime.Substring(0, ind));
            tt.min = Convert.ToInt32(oldtime.Substring(ind + 1, ind1 - ind - 1));
            tt.sec = 0;
            tt.year = Convert.ToInt32(oldtime.Substring(ind1 + 3, ind2 - ind1 - 3));
            tt.month = Convert.ToInt32(oldtime.Substring(ind2 + 1, ind3 - ind2 - 1));
            tt.day = Convert.ToInt32(oldtime.Substring(ind3 + 1, ind4 - ind3 - 1));
            listtime.Add(tt);

            ListBox9.Items.Add(texttime);
        }


        //head_str_uid
        //head_str_uname
        //mesg
        List<twitter_message_group> tmglist = new List<twitter_message_group>();
        twitter_message_group tmg = new twitter_message_group();

        for (int i = 0; i < head_str_mid.Count; i++)
        {
            tmg = new twitter_message_group();
            tmg.mes_id = head_str_mid[i];
            tmg.user_id = head_str_uid[i];
            tmg.username = head_str_uname[i];
            tmg.year = listtime[i].year;
            tmg.month = listtime[i].month;
            tmg.day = listtime[i].day;
            tmg.hour = listtime[i].hour;
            tmg.min = listtime[i].min;
            tmg.sec = listtime[i].sec;
            tmg.messgae = mesg[i].messgae;
            tmg.imglist = mesg[i].imglist;
            tmg.sharelist = mesg[i].sharelist;

            tmglist.Add(tmg);
        }
        for (int i = 0; i < tmglist.Count; i++)
        {
            if (tmglist[i].user_id == "412871152")
            {
                gc = new GCP_MYSQL();
                Query = "select twitter_mid";
                Query += " from twitter_message";
                Query += " where twitter_mid='" + tmglist[i].mes_id + "';";

                DataView ict_sf = gc.select_cmd(Query);
                if (ict_sf.Count == 0)
                {
                    Query = "insert into twitter_message(twitter_mid,twitter_uid,twitter_username,twitter_message,year,month,day,hour,minute,second)";
                    Query += " values('" + tmglist[i].mes_id + "','" + tmglist[i].user_id + "','横浜市神奈川区役所','" + tmglist[i].messgae + "',";
                    Query += "'" + tmglist[i].year + "','" + tmglist[i].month + "','" + tmglist[i].day + "','" + tmglist[i].hour + "','" + tmglist[i].min + "','" + tmglist[i].sec + "');";
                    resin = gc.insert_cmd(Query);
                    Query = "select id from twitter_message";
                    Query += " where twitter_mid='" + tmglist[i].mes_id + "' and twitter_uid='" + tmglist[i].user_id + "' and twitter_username='横浜市神奈川区役所'";
                    Query += " and twitter_message='" + tmglist[i].messgae + "' and year='" + tmglist[i].year + "' and month='" + tmglist[i].month + "' and day='" + tmglist[i].day + "' and hour='" + tmglist[i].hour + "'";
                    Query += " and minute='" + tmglist[i].min + "' and second='" + tmglist[i].sec + "';";

                    DataView ict_ff = gc.select_cmd(Query);
                    if (ict_ff.Count > 0)
                    {
                        string id = ict_ff.Table.Rows[0]["id"].ToString();
                        for (int ii = 0; ii < tmglist[i].sharelist.Count; ii++)
                        {
                            Query = "insert into twitter_message_link(tmid,share_link)";
                            Query += " values('" + id + "','" + tmglist[i].sharelist[ii] + "');";
                            resin = gc.insert_cmd(Query);
                        }
                        for (int ii = 0; ii < tmglist[i].imglist.Count; ii++)
                        {
                            Query = "insert into twitter_message_image(tmid,image_url)";
                            Query += " values('" + id + "','~/" + tmglist[i].imglist[ii] + "');";
                            resin = gc.insert_cmd(Query);
                        }
                    }

                }
            }
        }
        //insert into social media
        //参照元 : 横浜市神奈川区役所 Twitter アカウント<br/><br/>
        //postal_code 221-0824 
        //place 横浜市神奈川区広台太田町3-8
        //type 0
        //message_type 6
        gc = new GCP_MYSQL();
        Query = "select id,twitter_message,year,month,day,hour,minute,second";
        Query += " from twitter_message";
        Query += " where smid is null;";

        DataView ict_sff = gc.select_cmd(Query);
        if (ict_sff.Count > 0)
        {
            for (int i = 0; i < ict_sff.Count; i++)
            {
                string message = ict_sff.Table.Rows[i]["twitter_message"].ToString().Replace(@"\t|\n|\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                Query = "select share_link";
                Query += " from twitter_message_link";
                Query += " where tmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

                DataView ict_sff1 = gc.select_cmd(Query);
                if (ict_sff1.Count > 0)
                {
                    for (int ii = 0; ii < ict_sff1.Count; ii++)
                    {
                        message += "<br/>" + ict_sff1.Table.Rows[ii]["share_link"].ToString();
                    }
                }
                message += "<br/><br/>参照元 : 横浜市神奈川区役所 Twitter アカウント<br/>";

                //uid 50
                Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                Query += " values('50','0','6','横浜市神奈川区広台太田町3-8','" + message + "','" + ict_sff.Table.Rows[i]["year"].ToString() + "','" + ict_sff.Table.Rows[i]["month"].ToString() + "'";
                Query += ",'" + ict_sff.Table.Rows[i]["day"].ToString() + "','" + ict_sff.Table.Rows[i]["hour"].ToString() + "','" + ict_sff.Table.Rows[i]["minute"].ToString() + "','" + ict_sff.Table.Rows[i]["second"].ToString() + "','221-0824')";

                resin = gc.insert_cmd(Query);

                string smid = "";
                Query = "select id from status_messages";
                Query += " where uid='50' and type='0' and message_type='6' and place='横浜市神奈川区広台太田町3-8'";
                Query += " and message='" + message + "' and year='" + ict_sff.Table.Rows[i]["year"].ToString() + "' and month='" + ict_sff.Table.Rows[i]["month"].ToString() + "' and day='" + ict_sff.Table.Rows[i]["day"].ToString() + "' and hour='" + ict_sff.Table.Rows[i]["hour"].ToString() + "'";
                Query += " and minute='" + ict_sff.Table.Rows[i]["minute"].ToString() + "' and second='" + ict_sff.Table.Rows[i]["second"].ToString() + "' and postal_code='221-0824';";

                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    smid = ict_f.Table.Rows[0]["id"].ToString();
                    Query = "select image_url";
                    Query += " from twitter_message_image";
                    Query += " where tmid='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";

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
                Query = "update twitter_message set smid='" + smid + "'";
                Query += " where id='" + ict_sff.Table.Rows[i]["id"].ToString() + "';";
                resin = gc.update_cmd(Query);

                ConvertUrlsInData(message);
            }
        }
    }

    protected void Timer2_Tick(object sender, EventArgs e)
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
        Session["seak"] = "false";
        if (DateTime.Now.Minute % 59 == 0)
        {
            if (DateTime.Now.Second > 55)
            {
                Session["seak"] = "true";
                Label4.Text = DateTime.Now.ToString();
                Response.Redirect("twitter_gov.aspx");
            }
        }
    }
}