using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class phone_key : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string check_mamaro(string param1)
    {
        string res = "out";
        if (param1 == "tt")
        {
            HttpContext.Current.Session["Keyfor"] = "Man";
        }
        return res;
    }

}