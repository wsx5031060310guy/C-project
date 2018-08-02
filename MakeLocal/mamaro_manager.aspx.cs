using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_manager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
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

            string usernames = RemoveSpecialCharacters(username);
            string passwords = RemoveSpecialCharacters(password);
            if (usernames != "" && passwords != "")
            {

                GCP_MYSQL gc = new GCP_MYSQL();
                string Query = "select id from nursing_room_manager where account='" + usernames + "';";
                DataView ict_ff = gc.select_cmd(Query);
                if (ict_ff.Count > 0)
                {
                    string Query1 = "select id from nursing_room_manager where account='" + usernames + "' and password='" + passwords + "';";
                    DataView ict_ff1 = gc.select_cmd(Query1);
                    if (ict_ff1.Count > 0)
                    {
                        HttpContext.Current.Session["manager_page"] = ict_ff1.Table.Rows[0]["id"].ToString();
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
                    result = "アカウントが間違っています。";
                }
                


               
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