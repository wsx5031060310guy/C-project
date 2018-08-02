using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] Getsearch(string prefix)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        List<string> customers = new List<string>();

        Query1 = "select id,username,photo from user_login where username like '" + prefix.Replace("'", "").Replace(@"""", "") + "%'";
        DataView ict_sf = gc1.select_cmd(Query1);

        for (int i = 0; i < ict_sf.Count; i++)
        {
            string cutstr = ict_sf.Table.Rows[i]["photo"].ToString();
            int ind = cutstr.IndexOf(@"/");
            string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
            customers.Add(string.Format("{0};{1};{2}", ict_sf.Table.Rows[i]["username"], ict_sf.Table.Rows[i]["id"], cutstr1));
        }
        return customers.ToArray();
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] Getsearch_friend(string prefix,string who)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        List<string> fri = new List<string>();
        //search friends
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + who + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_sf = gc1.select_cmd(Query1);

        for (int i = 0; i < ict_sf.Count; i++)
        {
            fri.Add(ict_sf.Table.Rows[i]["id"].ToString());
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + who + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);

        for (int i = 0; i < ict_f1.Count; i++)
        {
            fri.Add(ict_f1.Table.Rows[i]["id"].ToString());
        }
        //search friends
        List<string> customers = new List<string>();
        if (fri.Count > 0)
        {
            Query1 = "select id,username,photo from user_login where username like '" + prefix.Replace("'", "").Replace(@"""", "") + "%'";
            Query1 += " and (";
            Query1 += " id='" + fri[0] + "'";
            for (int i = 1; i < fri.Count; i++)
            {
                Query1 += " or id='" + fri[i] + "'";
            }
            Query1 += ")";
            DataView ict_sf11 = gc1.select_cmd(Query1);

            for (int i = 0; i < ict_sf.Count; i++)
            {
                string cutstr = ict_sf11.Table.Rows[i]["photo"].ToString();
                int ind = cutstr.IndexOf(@"/");
                string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                customers.Add(string.Format("{0};{1};{2}", ict_sf11.Table.Rows[i]["username"], ict_sf11.Table.Rows[i]["id"], cutstr1));
            }
        }

      
        return customers.ToArray();
    }
}