using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class registered0 : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (Session["FB_name"] != null)
        {
            name_TextBox.Text = Session["FB_name"].ToString();
        }
        if (Session["FB_id"] != null)
        {
            FB_id_hidd.Value = Session["FB_id"].ToString();
            //Image1.ImageUrl = "https://graph.facebook.com/" + Session["FB_id"].ToString() + "/picture?type=large";
            //Image1.Attributes.Add("style", "display:block");
        }
        if (Session["FB_email"] != null)
        {
            loginname_TextBox.Text = Session["FB_email"].ToString();
        }
        if (Session["FB_pic"] != null)
        {
            Image1.ImageUrl = Session["FB_pic"].ToString();
            Image1.Attributes.Add("style", "display:block");
            Session["head_photo"] = "~/" + Session["FB_pic"].ToString();
        }
        if (Session["FB_cov"] != null)
        {
            FB_cov_hidd.Value = "~/" + Session["FB_cov"].ToString();
        }

    }
   protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        Session["head_photo"] = null;
        if (fuDocument.HasFile)
        {
            HttpPostedFile postedFile = fuDocument.PostedFile;

            DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");

            Query = "select id,name from filename_extension";
            DataView ou1 = gc.select_cmd(Query);
            for (int i = 0; i < ou1.Count; i++)
            {
                if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
                {
                    check = true;
                }
            }
            if (check)
            {
                filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);
                //AmazonUpload aws = new AmazonUpload();
                //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                //fuDocument.SaveAs(Server.MapPath("head_photo") + "\\" + filename);
                Image1.ImageUrl = imgurl;
                 Image1.Attributes.Add("style", "display:block");

                Session["head_photo"] = "~/" + imgurl;
            }
            else
            {
                ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('filename extension is not in role!')", true);
            }


        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        string FB_id = "";
        if (FB_id_hidd.Value != "")
        {
            FB_id = FB_id_hidd.Value;
        }
        string cov = "";
        if (FB_cov_hidd.Value != "")
        {
            cov = FB_cov_hidd.Value;
        }

        string name = name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string loginname = loginname_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        bool check_mail = CheckEmailFormat(loginname);
        string photo = "";
        if (Session["head_photo"] != null)
        {
            photo = Session["head_photo"].ToString();
        }

        string password = password_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string c_password = password_TextBox_check.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();


        bool check_name = false, check_loginname = false, check_photo = false, check_password = false, check_c_password = false, check_password_same = false, check_loginname_same = false;
        if (name != "")
        {
            check_name = true;
            name_Label.Text = "";
        }
        else
        {
            check_name = false;
            name_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (loginname != "")
        {
            check_loginname = true;
            if (check_mail)
            {

                Query = "select login_name from user_login";
                Query += " where login_name='" + loginname + "';";
                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    check_loginname_same = false;
                    loginname_Label.Text = "既に登録されているメールアドレスです。";
                }
                else
                {
                    check_loginname_same = true;
                    loginname_Label.Text = "";
                }

            }
            else
            {
                loginname_Label.Text = "無効なメールアドレスです";
            }
        }
        else
        {
            check_loginname = false;
            loginname_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (photo != "")
        {
            check_photo = true;
            photo_Label.Text = "";
        }
        else
        {
            check_photo = false;
            photo_Label.Text = "アイコンに使用する画像をアップロードしてください。";
        }
        if (password != "")
        {
            check_password = true;
            password_Label.Text = "";
        }
        else
        {
            check_password = false;
            password_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (c_password != "")
        {
            check_c_password = true;
            if (password == c_password)
            {
                check_password_same = true;
                c_password_Label.Text = "";
            }
            else
            {
                check_password_same = false;
                c_password_Label.Text = "Your confirm password is not equal password.";
            }
        }
        else
        {
            check_c_password = false;
            c_password_Label.Text = "パスワードが間違っています。";
        }

        bool check_place = false;
        if (Session["place_add"] != null && Session["place_postalcode"] != null)
        {
            if (Session["place_add"].ToString().Trim() != "" && Session["place_postalcode"].ToString().Trim() != "")
            {
                check_place = true;
            }
            else
            {
                check_place = false;
                c_place_Label.Text = "地域を設定してください";
            }
        }
        else
        {
            check_place = false;
            c_place_Label.Text = "地域を設定してください";
        }


        if (check_place && check_name && check_loginname && check_photo && check_password && check_c_password && check_password_same && check_mail && check_loginname_same)
        {
             bool check_db = true;

             Query = "select id from user_login";
             Query += " where login_name='" + loginname + "' and login_password='" + password + "';";
             DataView ict_f = gc.select_cmd(Query);
            if (ict_f.Count > 0)
            {
                check_db = false;
            }

            if (check_db)
            {
                Query = "insert into user_login(login_name,login_password,username,photo,CreatedDate)";
                Query += " values('" + loginname + "','" + password + "','" + name + "','" + photo + "',NOW())";
                resin = gc.insert_cmd(Query);


                Query = "select id from user_login";
                Query += " where login_name='" + loginname + "' and login_password='" + password + "' and username='" + name + "';";
                ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    if (FB_id != "")
                    {

                        Query = "update user_login";
                        Query += " set FBID='" + FB_id + "'";
                        Query += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                        resin = gc.update_cmd(Query);
                    }
                    if (cov != "")
                    {

                        Query = "update user_login";
                        Query += " set home_image='" + cov + "'";
                        Query += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                        resin = gc.update_cmd(Query);

                    }

                }

                if (ict_f.Count > 0)
                {
                    SendActivationEmail(Convert.ToInt32(ict_f.Table.Rows[0]["id"].ToString()), loginname, name);
                    Session["id"] = ict_f.Table.Rows[0]["id"].ToString();

                    if (Session["place_add"] != null && Session["place_postalcode"] != null)
                    {

                        Regex r = new Regex(@",");
                        string add_str = Session["place_add"].ToString();
                        // Match the regular expression pattern against a text string.
                        Match m = r.Match(add_str);
                        List<int> indexlist = new List<int>();
                        while (m.Success)
                        {
                            indexlist.Add(m.Index);
                            m = m.NextMatch();
                        }
                        List<string> addlist = new List<string>();
                        for (int i = 0; i < indexlist.Count; i++)
                        {
                            string add = "";
                            if (i - 1 < 0)
                            {
                                add = add_str.Substring(0, indexlist[i]);
                                addlist.Add(add);
                            }
                            else
                            {
                                add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                                addlist.Add(add);
                            }
                        }


                        add_str = Session["place_postalcode"].ToString();
                        // Match the regular expression pattern against a text string.
                        m = r.Match(add_str);
                        indexlist = new List<int>();
                        while (m.Success)
                        {
                            indexlist.Add(m.Index);
                            m = m.NextMatch();
                        }
                        List<string> addlist1 = new List<string>();
                        for (int i = 0; i < indexlist.Count; i++)
                        {
                            string add = "";
                            if (i - 1 < 0)
                            {
                                add = add_str.Substring(0, indexlist[i]);
                                addlist1.Add(add);
                            }
                            else
                            {
                                add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                                addlist1.Add(add);
                            }
                        }
                        for (int i = 0; i < addlist.Count; i++)
                        {

                            Query = "insert into user_login_address(uid,place,postal_code)";
                            Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + addlist[i] + "','" + addlist1[i] + "')";
                            resin = gc.insert_cmd(Query);
                        }
                    }
                }



                name_TextBox.Text = "";
                loginname_TextBox.Text = "";
                Session["head_photo"] = null;
                Image1.ImageUrl = "";
                password_TextBox.Text = "";
                password_TextBox_check.Text = "";
                result_Label.Text = "Success registered. Please check out your email.";

                Response.Redirect("registered.aspx");
            }
            else
            {
                result_Label.Text = "登録に失敗しました。";
            }

        }
        else
        {
            result_Label.Text = "登録に失敗しました。";
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string FB_id = "";
        if (FB_id_hidd.Value != "")
        {
            FB_id = FB_id_hidd.Value;
        }
        string cov = "";
        if (FB_cov_hidd.Value != "")
        {
            cov =  FB_cov_hidd.Value;
        }
        string name = name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string loginname = loginname_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        bool check_mail = CheckEmailFormat(loginname);
        string photo = "";
        if (Session["head_photo"] != null)
        {
            photo = Session["head_photo"].ToString();
        }

        string password = password_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string c_password = password_TextBox_check.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();


        bool check_name = false,check_loginname=false,check_photo=false,check_password=false,check_c_password=false,check_password_same=false,check_loginname_same=false;
        if (name != "")
        {
            check_name = true;
            name_Label.Text = "";
        }
        else
        {
            check_name = false;
            name_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (loginname != "")
        {
            check_loginname = true;
            if (check_mail)
            {

                Query = "select login_name from user_login";
                Query += " where login_name='" + loginname + "';";
                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    check_loginname_same = false;
                    loginname_Label.Text = "既に登録されているメールアドレスです。";
                }
                else
                {
                    check_loginname_same = true;
                    loginname_Label.Text = "";
                }

            }
            else
            {
                loginname_Label.Text = "無効なメールアドレスです";
            }
        }
        else
        {
            check_loginname = false;
            loginname_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (photo != "")
        {
            check_photo = true;
            photo_Label.Text = "";
        }
        else
        {
            check_photo = false;
            photo_Label.Text = "アイコンに使用する画像をアップロードしてください。";
        }
        if (password != "")
        {
            check_password = true;
            password_Label.Text = "";
        }
        else
        {
            check_password = false;
            password_Label.Text = "未記入もしくは使用できない単語です。";
        }
        if (c_password != "")
        {
            check_c_password = true;
            if (password == c_password)
            {
                check_password_same = true;
                c_password_Label.Text = "";
            }
            else
            {
                check_password_same = false;
                c_password_Label.Text = "Your confirm password is not equal password.";
            }
        }
        else
        {
            check_c_password = false;
            c_password_Label.Text = "パスワードが間違っています。";
        }

        bool check_place = false;
        if (Session["place_add"] != null && Session["place_postalcode"] != null)
        {
            if (Session["place_add"].ToString().Trim() != "" && Session["place_postalcode"].ToString().Trim() != "")
            {
                check_place = true;
            }
            else
            {
                check_place = false;
                c_place_Label.Text = "地域を設定してください";
            }
        }
        else
        {
            check_place = false;
            c_place_Label.Text = "地域を設定してください";
        }


        if (check_place&&check_name && check_loginname && check_photo && check_password && check_c_password && check_password_same && check_mail && check_loginname_same)
        {


            Query = "insert into user_login(login_name,login_password,username,photo,CreatedDate)";
            Query += " values('" + loginname + "','" + password + "','" + name + "','" + photo + "',NOW())";
            resin = gc.insert_cmd(Query);

            Query = "select id from user_login";
            Query += " where login_name='" + loginname + "' and login_password='" + password + "';";
            DataView ict_f = gc.select_cmd(Query);
            if (ict_f.Count > 0)
            {
                if (FB_id != "")
                {
                    Query = "update user_login";
                    Query += " set FBID='" + FB_id + "'";
                    Query += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                    resin = gc.update_cmd(Query);
                }
                if (cov != "")
                {

                    Query = "update user_login";
                    Query += " set home_image='" + cov + "'";
                    Query += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                    resin = gc.update_cmd(Query);

                }

            }

            if (ict_f.Count > 0)
            {
                SendActivationEmail(Convert.ToInt32(ict_f.Table.Rows[0]["id"].ToString()), loginname, name);
                Session["id"] = ict_f.Table.Rows[0]["id"].ToString();

                if (Session["place_add"] != null && Session["place_postalcode"] != null)
                {

                    Regex r = new Regex(@",");
                    string add_str = Session["place_add"].ToString();
                    // Match the regular expression pattern against a text string.
                    Match m = r.Match(add_str);
                    List<int> indexlist = new List<int>();
                    while (m.Success)
                    {
                        indexlist.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> addlist = new List<string>();
                    for (int i = 0; i < indexlist.Count; i++)
                    {
                        string add = "";
                        if (i - 1 < 0)
                        {
                            add = add_str.Substring(0, indexlist[i]);
                            addlist.Add(add);
                        }
                        else
                        {
                            add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                            addlist.Add(add);
                        }
                    }


                    add_str = Session["place_postalcode"].ToString();
                    // Match the regular expression pattern against a text string.
                    m = r.Match(add_str);
                    indexlist = new List<int>();
                    while (m.Success)
                    {
                        indexlist.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> addlist1 = new List<string>();
                    for (int i = 0; i < indexlist.Count; i++)
                    {
                        string add = "";
                        if (i - 1 < 0)
                        {
                            add = add_str.Substring(0, indexlist[i]);
                            addlist1.Add(add);
                        }
                        else
                        {
                            add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                            addlist1.Add(add);
                        }
                    }
                    for (int i = 0; i < addlist.Count; i++)
                    {

                        Query = "insert into user_login_address(uid,place,postal_code)";
                        Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + addlist[i] + "','" + addlist1[i] + "')";
                        resin = gc.insert_cmd(Query);
                    }
                }



            }


            name_TextBox.Text = "";
            loginname_TextBox.Text = "";
            Session["head_photo"] = null;
            Image1.ImageUrl = "";
            password_TextBox.Text = "";
            password_TextBox_check.Text = "";
            result_Label.Text = "Success registered. Please check out your email.";

            Response.Redirect("main.aspx");
        }
        else
        {
            result_Label.Text = "登録に失敗しました。";
        }
    }
    public static bool CheckEmailFormat(string email)
    {
        //Email檢查格式
        Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            else
            {
                return EmailExpression.IsMatch(email);
            }
        }
        catch (Exception ex)
        {
            //log.Error(ex.Message);
            return false;
        }
    }
    private void SendActivationEmail(int userId,string email,string name)
    {
        string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
        string activationCode = Guid.NewGuid().ToString();
        using (MySqlConnection con = new MySqlConnection(constr))
        {

            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO UserActivation VALUES('" + userId + "', '" + activationCode + "')"))
            {
                using (MySqlDataAdapter sda = new MySqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        string constr1 = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
        string activationCode1 = Guid.NewGuid().ToString();
        using (MySqlConnection con1 = new MySqlConnection(constr1))
        {
            using (MySqlCommand cmd1 = new MySqlCommand("INSERT INTO UserActivation_registered VALUES('" + userId + "', '" + activationCode1 + "')"))
            {
                using (MySqlDataAdapter sda1 = new MySqlDataAdapter())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = con1;
                    con1.Open();
                    cmd1.ExecuteNonQuery();
                    con1.Close();
                }
            }
        }
        using (MailMessage mm = new MailMessage("email", email))
        {
            mm.Subject = "メールアドレスをご確認ください";

            //string body = "Hello " + name + ",";

            string body = "";

            body += "<table width='100%'>";
            body += "<tr>";
            body += "<td width='20%' height='10%'>";
            body += "</td>";
            body += "<td width='60%' height='10%'>";
            body += "</td>";
            body += "<td width='20%' height='10%'>";
            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td width='20%'>";
            body += "</td>";
            body += "<td width='60%'>";
            body += "<table width='100%'>";
            body += "<tr>";
            body += "<td height='50px' valign='bottom' align='center'>";
            body += "<span STYLE='color: #F88787; font-size: 20pt;'></span><br/><br/>";
            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td>";
            body += "<span STYLE='font-size: 15pt;'>" + name + "さん、こんにちは。</span><br/>";
            body += "<span STYLE='font-size: 15pt;'>地域 コミュニティー" + '"' + "" + '"' + "へようこそ。</span><br/>";

            body += "<span STYLE='font-size: 15pt;'>サービスご利用の前に、メールアドレスの確認を行います。</span><br/>";
            body += "<span STYLE='font-size: 15pt;'>下記のボタンをお押しください。</span><br/><br/>";


            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td align='center'>";
            //button メール アドレス を 確認
            body += "<a style='font-size: 15pt;line-height: 50px;text-align: center;height: 50px;width: 100%; text-decoration: none;overflow: hidden;display: inline-block; position: relative;vertical-align: middle;text-align: center;color: #fff;border: 2px solid #ff7575;background: #FF9797;-moz-border-radius: 8px;-webkit-border-radius: 8px;border-radius: 8px;text-shadow: #000 1px 1px 4px;' href='" + Request.Url.AbsoluteUri.Replace("registered0.aspx", "Activation.aspx?ActivationCode=" + activationCode) + "' class='file-upload'>メールアドレスを確認</a>";
            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td>";
            //どうぞよろしくお願いします。 RiN 運営局
            body += "<br/><br/><span STYLE='font-size: 15pt;'>どうぞよろしくお願いします。</span><br/>";
            body += "<span STYLE='font-size: 15pt;'> 運営局</span><br/><br/>";
            body += "</td>";
            body += "</tr>";
            body += "</table>";
            body += "</td>";
            body += "<td width='20%'>";
            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td width='20%' height='10%'>";
            body += "</td>";
            body += "<td width='60%' height='10%'>";
            body += "</td>";
            body += "<td width='20%' height='10%'>";
            body += "</td>";
            body += "</tr>";
            body += "</table>";







            //body += "<br /><br />Please click the following link to activate your account";
            //body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("registered0.aspx", "Activation.aspx?ActivationCode=" + activationCode) + "'>Click here to activate your account.</a>";
            //body += "<br /><br />Thanks";
            mm.Body = body;
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("email", "password");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        SendActivationEmail(9, "email", "");
    }
    [WebMethod(EnableSession = true)]
    public static string Save_place(string param1, string param2)
    {
        string result = param1 + "," + param2 ;
        //ver 1
        HttpContext.Current.Session["place_postalcode"] = param1 + ",";
        HttpContext.Current.Session["place_add"] = param2 + ",";

        ////ver 2
        //HttpContext.Current.Session["place_postalcode"] += param1+",";
        //HttpContext.Current.Session["place_add"] += param2 + ",";
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string Save_first(string param1)
    {
        string result ="";
        HttpContext.Current.Session["place_postalcode"] = "";
        HttpContext.Current.Session["place_add"] = "";
        return result;
    }
}
