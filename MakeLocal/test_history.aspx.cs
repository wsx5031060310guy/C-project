using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_history : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        string activationCode = !string.IsNullOrEmpty(Request.QueryString["mamaro"]) ? Request.QueryString["mamaro"] : Guid.Empty.ToString();
        string check_url = activationCode;
      
        Panel1.Controls.Add(new LiteralControl("<iframe id='maincontent' src=" + '"' + check_url + '"' + " width='100%' height='100%' frameborder='0' sandbox='allow-same-origin allow-scripts'></iframe>"));

    }
}
