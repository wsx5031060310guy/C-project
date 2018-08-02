using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manager_page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["mamaro"] != null)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamaro"]) ? Request.QueryString["mamaro"] : Guid.Empty.ToString();
            if (activationCode != "")
            {
                Session["Keyfor"] = activationCode;

                Response.Redirect("manager_page.aspx");
            }
        }
        else
        {
            if (Session["Keyfor"] != null)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
                if (Session["Keyfor"].ToString().Trim() != "")
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
                    if (Session["Keyfor"].ToString() == "Man")
                    {

                    }
                    else
                    {
                        Session.Clear();
                        Response.Redirect(@"http://.jp/");
                    }
                }
                else
                {
                    Session.Clear();
                    Response.Redirect(@"http://.jp/");
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect(@"http://.jp/");
            }
        }
    }
    [WebMethod(EnableSession = true)]
    public static string check_login(string param1, string param2)
    {
        //string result = param1 + "," + param2;
        string result = "";
        try
        {
            string username = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string password = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            if (username != "" && password != "")
            {
                if (username == "trim")
                {
                    if (password == "0000")
                    {
                        HttpContext.Current.Session["manager"] = "trim";
                        //result = HttpContext.Current.Session["manager"].ToString();
                        result = "ログインできました。";
                    }
                    else
                    {
                        result = "パスワードが間違っています。";
                    }
                }
                else
                {
                    result = "メールアドレスが間違っています。";
                }
                //SqlDataSource sql_f = new SqlDataSource();
                //sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //sql_f.SelectCommand = "select id from user_login";
                //sql_f.SelectCommand += " where login_name='" + username + "';";
                //DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                //if (ict_f.Count > 0)
                //{

                //    SqlDataSource sql_f1 = new SqlDataSource();
                //    sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //    sql_f1.SelectCommand = "select id from user_login";
                //    sql_f1.SelectCommand += " where login_name='" + username + "' and login_password='" + password + "';";
                //    DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                //    if (ict_f1.Count > 0)
                //    {
                //        SqlDataSource sql_f_check = new SqlDataSource();
                //        sql_f_check.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //        sql_f_check.SelectCommand = "select UserId from UserActivation";
                //        sql_f_check.SelectCommand += " where UserId='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                //        DataView ict_check = (DataView)sql_f_check.Select(DataSourceSelectArguments.Empty);
                //        if (ict_check.Count > 0)
                //        {
                //            result = "メールをご確認ください。";
                //        }
                //        else
                //        {
                //            SqlDataSource sql_f_login = new SqlDataSource();
                //            sql_f_login.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                //            sql_f_login.UpdateCommand = "update user_login set LastLoginDate=SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
                //            sql_f_login.UpdateCommand += " where id='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
                //            sql_f_login.Update();

                //            HttpContext.Current.Session["id"] = ict_f1.Table.Rows[0]["id"].ToString();
                //            HttpContext.Current.Session["loginname"] = username;
                //            result = "ログインできました。";
                //        }
                //    }
                //    else
                //    {
                //        result = "パスワードが間違っています。";
                //    }

                //}
                //else
                //{
                //    result = "メールアドレスが間違っています。";
                //}
            }



        }
        catch (Exception ex)
        {
            result = "ログインできませんでした。";

            //return result;
            throw ex;
        }
        return result;

    }
}
