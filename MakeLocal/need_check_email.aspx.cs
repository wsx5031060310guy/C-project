using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class need_check_email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!this.IsPostBack)
        {
            Label1.Text = "10";
            ltMessage.Text = "メールをチェックする必要があります";
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