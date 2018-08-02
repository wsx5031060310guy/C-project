using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Panel pdn = (Panel)this.FindControl("new_message_Panel");

        Query = "select message,year,month,day ";
        Query += " from home_message";
        Query += " ORDER BY year desc,month desc,day desc,hour desc,minute desc,second desc;";
        DataView ict_f = gc.select_cmd(Query);
        pdn.Controls.Add(new LiteralControl("<table width='100%'>"));
        for (int i = 0; i < ict_f.Count; i++)
        {
            int year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
            int month = Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString());
            int day = Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString());
            string mon = month.ToString() ;
            if (month < 10)
            {
                mon = "0" + month.ToString();
            }
            string da = day.ToString();
            if (day < 10)
            {
                da = "0" + day.ToString();
            }

            pdn.Controls.Add(new LiteralControl("<tr>"));
            pdn.Controls.Add(new LiteralControl("<td width='20%' valign='top'>"));
            Label la_d = new Label();
            la_d.Text = year.ToString() + "." + mon + "." + da;

            pdn.Controls.Add(la_d);
            pdn.Controls.Add(new LiteralControl("</td>"));
            pdn.Controls.Add(new LiteralControl("<td width='60%' valign='top'>"));

            Label la = new Label();
            la.Text = "";
            la.Text = ict_f.Table.Rows[i]["message"].ToString();
            //if (ict_f.Table.Rows[i]["message"].ToString().Length < 35)
            //{
            //    la.Text = ict_f.Table.Rows[i]["message"].ToString();
            //}
            //else
            //{
            //    la.Text = ict_f.Table.Rows[i]["message"].ToString().Substring(0, 35) + "‧‧‧";
            //}
            pdn.Controls.Add(la);
            pdn.Controls.Add(new LiteralControl("</td>"));
            pdn.Controls.Add(new LiteralControl("<td width='20%' valign='top'>"));

            DateTime date=new DateTime(year,month,day);

            if ((DateTime.Now-date).TotalDays < 14)
            {

                Image img = new Image();
                img.ImageUrl = "~/images/home_images/new.png";
                //img.Width = Unit.Percentage(50);
                img.Style.Add("vertical-align", "text-top");
                pdn.Controls.Add(img);
                //pdn.Controls.Add(new LiteralControl("<img src='images/home_images/new.png' alt='' style='vertical-align: top;'>"));
            }
            pdn.Controls.Add(new LiteralControl("</td>"));
            pdn.Controls.Add(new LiteralControl("</tr>"));
        }
        pdn.Controls.Add(new LiteralControl("</table>"));


    }
    [WebMethod]
    public static string sendmail(string name, string phone, string email, string message)
    {
        //string result = param1 + "," + param2;
        string result = "";
        using (MailMessage mm = new MailMessage("", ""))
        {
            mm.Subject = "【  】Website Contact Form: "+name;

            //string body = "Hello " + name + ",";

            string body = "You have received a new message from your website contact form.<br/><br/>" + "Here are the details:<br/><br/>Name:" + name + "<br/><br/>Email: " + email + "<br/><br/>Phone: " + phone + "<br/><br/>Message:<br/>" + message;




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

        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string check_mamaro(string param1)
    {
        string res = "out";
        if (param1 == "tt")
        {
            HttpContext.Current.Session["Keyfor"] = "Man";
        }
        return res;
    }


}
