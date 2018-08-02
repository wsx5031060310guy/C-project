using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _3Dbox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
                    Response.Redirect(@"home");
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect(@"home");
            }
        }
        else
        {
            Session.Clear();
            Response.Redirect(@"home");
        }
    }
}
