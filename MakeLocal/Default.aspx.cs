using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*Code-Behind沒什麼要做的*/
public partial class _Default : System.Web.UI.Page
{
    protected string zip_no = "100";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)//Get Method要做的事
        {

        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default2.aspx");
    }
}