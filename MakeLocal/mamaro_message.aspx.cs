using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_message : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        if (this.Request.QueryString["mid"] != null)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["mid"]) ? Request.QueryString["mid"] : Guid.Empty.ToString();
            if (activationCode != "")
            {
                Session["mamaro_id"] = activationCode;

                Response.Redirect("mamaro_message.aspx");
            }
        }
        else
        {

        }
        try
        {
            string touid =RemoveSpecialCharacters( Session["mamaro_id"].ToString());

            gc = new GCP_MYSQL();
            Literal lip = new Literal();
            Query = "select id from nursing_room where QRcode='"+touid+"';";
            DataView ict_ff = gc.select_cmd(Query);
            if (ict_ff.Count > 0)
            {
                Session["real_id"] = ict_ff.Table.Rows[0]["id"].ToString();
            }
            else
            {
                Response.Redirect(@"http:///");
            }



        }
        catch (Exception ex)
        {
            Response.Redirect(@"http:///");
        }

    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string write_message(string param1)
    {
        string id = RemoveSpecialCharacters(HttpContext.Current.Session["real_id"].ToString());

        string res =  id ;

        if (id != null)
        {
            if (id.Trim() != "")
            {
                GCP_MYSQL gc = new GCP_MYSQL();
                string Query1 = "insert into nursing_room_message(nursing_room_id,type,message,update_time)";
                Query1 += " values('" + id + "',0,'" + param1 + "',NOW());";
                string resin = gc.insert_cmd(Query1);
            }
        }

        return res;
    }
}
