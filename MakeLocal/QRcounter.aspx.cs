using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QRcounter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Session.Clear();
        //string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamaro"]) ? Request.QueryString["mamaro"] : Guid.Empty.ToString();
        string text = Page.Request.QueryString.Get("QR");
        string QR = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-zA-Z0-9\s]", string.Empty);
        Session["QR"] = QR;
        
    }
    [WebMethod]
    public static string check_user(string param1, string param2, string param3, string param4)
    {
        string res = "";
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "";
        if (HttpContext.Current.Session["QR"] != null)
        {
            if (HttpContext.Current.Session["QR"].ToString().Trim() != "")
            {
                string QRcode = HttpContext.Current.Session["QR"].ToString();
                Query = "select id";
                Query += " from nursing_room where QRcode='" + QRcode + "';";
                DataView ict_place = gc.select_cmd(Query);
                string userinfo = "";
                string userinfo1 = "";
                string userinfo2 = "";
                string userinfo3 = "";
                if (ict_place.Count > 0)
                {
                    string id = ict_place.Table.Rows[0]["id"].ToString();
                    if (param1 == null)
                    {
                        userinfo = "NULL";
                    }
                    else
                    {
                        if (param1.Trim() == "")
                        {
                            userinfo = "NULL";
                        }
                        else
                        {
                            userinfo = "'" + param1.Trim() + "'";
                        }
                    }
                    if (param2 == null)
                    {
                        userinfo1 = "NULL";
                    }
                    else
                    {
                        if (param2.Trim() == "")
                        {
                            userinfo1 = "NULL";
                        }
                        else
                        {
                            userinfo1 = "'" + param2.Trim() + "'";
                        }
                    }
                    if (param3 == null)
                    {
                        userinfo2 = "NULL";
                    }
                    else
                    {
                        if (param3.Trim() == "")
                        {
                            userinfo2 = "NULL";
                        }
                        else
                        {
                            userinfo2 = "'" + param3.Trim() + "'";
                        }
                    }
                    if (param4 == null)
                    {
                        userinfo3 = "NULL";
                    }
                    else
                    {
                        if (param4.Trim() == "")
                        {
                            userinfo3 = "NULL";
                        }
                        else
                        {
                            userinfo3 = "'" + param4.Trim() + "'";
                        }
                    }
                    if (id.Trim() != "")
                    {
                        Query = "insert into nursing_room_QR_counter(nursing_room_id,user_os,user_os_ver,user_browser,user_browser_ver,insert_time)";
                        Query += " values('" + id.Trim() + "'," + userinfo + "," + userinfo1 + "," + userinfo2 + "," + userinfo3 + ",NOW());";

                        string rescom = gc.insert_cmd(Query);
                        res = rescom;
                    }

                }
                else
                {

                }



                
            }
           
        }


        return res;
    }
}