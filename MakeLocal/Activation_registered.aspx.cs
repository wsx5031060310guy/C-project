using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Activation_registered : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();


            Query = "select UserId";
            Query += " from UserActivation_registered";
            Query += " where ActivationCode='" + activationCode + "';";

            DataView ict_f = gc.select_cmd(Query);
            int id = 0;
            for (int i = 0; i < ict_f.Count; i++)
            {
                id = Convert.ToInt32(ict_f.Table.Rows[i]["UserId"].ToString());
            }
            if (id == 0)
            {
                Response.Redirect("main.aspx");
            }
            else
            {
                Session["id"] = id;
                Response.Redirect("registered.aspx");
            }
        }
    
    }
}