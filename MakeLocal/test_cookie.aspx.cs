using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class test_cookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Cookies["lat"].Value = "10,21,12";
        Response.Cookies["lng"].Value = "121,120,123";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string[] _strArray = Request.Cookies["lat"].Value.Split(',');
        string[] _strArray1 = Request.Cookies["lng"].Value.Split(',');
        Label1.Text = ""; Label2.Text = "";
        for (int i = 0; i < _strArray.Length; i++)
        {
            Label1.Text += _strArray[i] + "<br/>";
        }
        for (int i = 0; i < _strArray1.Length; i++)
        {
            Label2.Text += _strArray1[i] + "<br/>";
        }

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
}