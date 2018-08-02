using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;

public partial class Activation : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!this.IsPostBack)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();

            Query = "select UserId";
            Query += " from UserActivation";
            Query += " where ActivationCode='" + activationCode + "';";

             DataView ict_f = gc.select_cmd(Query);
             int id = 0;
             for (int i = 0; i < ict_f.Count; i++)
             {
                 id = Convert.ToInt32(ict_f.Table.Rows[i]["UserId"].ToString());
             }
             if (id == 0)
             {

             }
             else
             {

                 string activationCode1 = "";
                 Query = "select ActivationCode";
                 Query += " from UserActivation_registered";
                 Query += " where UserId='" + id + "';";

                 DataView ict_f1 = gc.select_cmd(Query);
                 if (ict_f1.Count > 0)
                 {
                     activationCode1 = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                 }
                 Query = "select login_name";
                 Query += " from user_login";
                 Query += " where id='" + id + "';";

                 ict_f1 = gc.select_cmd(Query);
                 if (ict_f1.Count > 0)
                 {
                     using (MailMessage mm = new MailMessage("email", ict_f1.Table.Rows[0]["login_name"].ToString()))
                     {
                         mm.Subject = "ご登録ありがとうございます！";

                         //string body = "Hello " + name + ",";

                         string body = "";

                         body += "<table width='100%'>";
                         body += "<tr>";
                         body += "<td width='20%' height='30%'>";
                         body += "</td>";
                         body += "<td width='60%' height='30%'>";


                         body += "<table width='100%' style='background: #FF9797;'>";
                         body += "<tr>";
                         body += "<td width='10%' height='10%'>";
                         body += "</td>";
                         body += "<td width='80%' height='10%'>";
                         body += "</td>";
                         body += "<td width='10%' height='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";

                         //
                         body += "<table width='100%'>";
                         body += "<tr>";
                         body += "<td width='100%'>";
                         body += "<br/><br/><span STYLE='font-size: 15pt;color : #fff;'></span><br/><br/>";
                         body += "</td>";
                         body += "</tr>";
                         body += "<tr>";
                         body += "<td width='100%' align='center'>";
                         body += "<span STYLE='font-size: 20pt;color : #fff;'>XXXへようこそ</span><br/><br/>";
                         body += "<span STYLE='font-size: 15pt;color : #fff;'>XXX</span><br/><br/>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";

                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "<tr>";
                         body += "<td width='10%' height='10%'>";
                         body += "</td>";
                         body += "<td width='80%' height='10%'>";
                         body += "</td>";
                         body += "<td width='10%' height='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";


                         body += "</td>";
                         body += "<td width='10%' height='30%'>";
                         body += "</td>";
                         body += "</tr>";

                         //second line
                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";
                         body += "<table width='100%'>";
                         body += "<tr>";

                         body += "<td height='50px' valign='bottom' align='center'>";
                         body += "<br/><br/><span STYLE='color: #F88787; font-size: 18pt;'>XXX</span><br/><br/>";
                         body += "</td>";

                         body += "</tr>";
                         body += "<tr>";
                         body += "<td align='center'>";

                         body += "<table width='100%'>";
                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";
                         body += "<span STYLE='font-size: 15pt;'>XXX</span><br/><br/>";
                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";


                         body += "</td>";
                         body += "</tr>";
                         //button サイトへ移動
                         body += "<tr>";

                         body += "<td align='center'>";
                         body += "<a style='font-size: 15pt;line-height: 50px;text-align: center;height: 50px;width: 100%; text-decoration: none;overflow: hidden;display: inline-block; position: relative;vertical-align: middle;text-align: center;color: #fff;border: 2px solid #ff7575;background: #FF9797;-moz-border-radius: 8px;-webkit-border-radius: 8px;border-radius: 8px;text-shadow: #000 1px 1px 4px;' href='" + Request.Url.AbsoluteUri.Replace("Activation.aspx?ActivationCode=" + activationCode, "main.aspx") + "'>サイトへ移動</a>";
                         body += "<br/><br/><hr/>";
                         body += "</td>";

                         body += "</tr>";
                         //button サイトへ移動

                         body += "<tr>";
                         body += "<td>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";
                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         //second line

                         //third line
                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";
                         body += "<table width='100%'>";
                         body += "<tr>";

                         body += "<td height='50px' valign='bottom' align='center'>";
                         body += "<br/><br/><span STYLE='color: #F88787; font-size: 18pt;'>XXX</span><br/><br/>";
                         body += "</td>";

                         body += "</tr>";
                         body += "<tr>";
                         body += "<td align='center'>";

                         body += "<table width='100%'>";
                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";
                         body += "<span STYLE='font-size: 15pt;'>XXX</span><br/><br/>";
                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";


                         body += "</td>";
                         body += "</tr>";
                         //button こどもの預け預かりを登録
                         body += "<tr>";

                         body += "<td align='center'>";
                         body += "<a style='font-size: 15pt;line-height: 50px;text-align: center;height: 50px;width: 100%; text-decoration: none;overflow: hidden;display: inline-block; position: relative;vertical-align: middle;text-align: center;color: #fff;border: 2px solid #ff7575;background: #FF9797;-moz-border-radius: 8px;-webkit-border-radius: 8px;border-radius: 8px;text-shadow: #000 1px 1px 4px;' href='" + Request.Url.AbsoluteUri.Replace("Activation.aspx?ActivationCode=" + activationCode, "Activation_registered.aspx?ActivationCode=" + activationCode1) + "'>こどもの預け／預かりを登録</a>";
                         body += "<br/><br/><hr/>";
                         body += "</td>";

                         body += "</tr>";
                         //button こどもの預け預かりを登録

                         body += "<tr>";
                         body += "<td>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";
                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         //third line

                         body += "<tr>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "<td width='80%'>";
                         body += "<br/><br/><span STYLE='color: #999C9D; font-size: 18pt;'>何か質問や不安点はございますか?</span><br/><br/>";
                         body += "<span STYLE='font-size: 15pt;'>詳しい利用方法に関しては</span><a style='font-size: 15pt;text-decoration: none;color: #F88787;' href='url'>ヘルプ</a><span STYLE='font-size: 15pt;'>をご参照ください。</span><br/><br/>";

                         body += "</td>";
                         body += "<td width='10%'>";
                         body += "</td>";
                         body += "</tr>";
                         body += "</table>";



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

             }
             string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
             using (MySqlConnection con = new MySqlConnection(constr))
                 {

                     using (MySqlCommand cmd = new MySqlCommand("DELETE FROM UserActivation WHERE ActivationCode ='" + activationCode + "'"))
                     {
                         using (MySqlDataAdapter sda = new MySqlDataAdapter())
                         {
                             cmd.CommandType = CommandType.Text;
                             cmd.Connection = con;
                             con.Open();
                             int rowsAffected = cmd.ExecuteNonQuery();
                             con.Close();
                             if (rowsAffected == 1)
                             {
                                 ltMessage.Text = "新規登録が完了しました。";
                             }
                             else
                             {
                                 ltMessage.Text = "新規登録に失敗しました。";
                             }
                         }
                     }
                 }

                 Label1.Text = "10";

        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        int rr = Convert.ToInt32(Label1.Text);

        if (rr == 0)
        {
            Response.Redirect("main.aspx");
        }
        else
        {
            rr -= 1;
            Label1.Text = Convert.ToString(rr);
        }
    }
}
