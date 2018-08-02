using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class password_forget : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string loginname = loginname_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        bool check_mail = CheckEmailFormat(loginname);
        if (loginname != "")
        {
            if (check_mail)
            {

                Query1 = "select id,username from user_login";
                Query1 += " where login_name='" + loginname + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    result_Label.Text = "";
                    string activationCode = Guid.NewGuid().ToString();

                    Query1 = "update user_login set login_password='" + activationCode + "'";
                    Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";

                    resin = gc1.update_cmd(Query1);
                    SendActivationEmail(loginname, ict_f.Table.Rows[0]["username"].ToString(), activationCode);
                    result_Label.Text = "メールをご確認ください。";
                }
                else
                {

                    result_Label.Text = "このメールアドレスはまだ登録されていません。";
                }

            }
            else
            {
                result_Label.Text = "無効なメールアドレスです。";
            }
        }
        else
        {
            result_Label.Text = "未記入もしくは使用できない単語です。";
        }
    }
    private void SendActivationEmail(string email, string name, string newpassword)
    {
        using (MailMessage mm = new MailMessage("", email))
        {
            mm.Subject = "パスワードの変更手続き";

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
            body += "<span STYLE='font-size: 15pt;'>Make Localアカウントのパスワードを一時的に下記に変更しました。</span><br/><br/>";

            body += "<span STYLE='font-size: 15pt;'>Account ID：</span><span STYLE='font-size: 15pt;'>" + email + "</span><br/>";
            body += "<span STYLE='font-size: 15pt;'>Password：</span><span STYLE='font-size: 15pt;'>" + newpassword + "</span><br/><br/>";


            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td align='center'>";
            //button メール アドレス を 確認
            body += "<a style='font-size: 15pt;line-height: 50px;text-align: center;height: 50px;width: 100%; text-decoration: none;overflow: hidden;display: inline-block; position: relative;vertical-align: middle;text-align: center;color: #fff;border: 2px solid #ff7575;background: #FF9797;-moz-border-radius: 8px;-webkit-border-radius: 8px;border-radius: 8px;text-shadow: #000 1px 1px 4px;' href='" + Request.Url.AbsoluteUri.Replace("password_forget.aspx", "main.aspx") + "' class='file-upload'>ログイン</a>";
            body += "</td>";
            body += "</tr>";
            body += "<tr>";
            body += "<td>";
            //どうぞよろしくお願いします。 RiN 運営局
            body += "<br/><br/><span STYLE='font-size: 15pt;'>ログイン後、マイページより新たなパスワードに変更してください。</span><br/>";
            body += "<span STYLE='font-size: 15pt;'> 運営局</span><br/>";
            body += "<span STYLE='font-size: 15pt;'>■お問い合わせ</span><br/>";
            body += "<a href='' target='_blank'></a><br/><br/>";
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
            NetworkCredential NetworkCred = new NetworkCredential("", "");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
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
}
