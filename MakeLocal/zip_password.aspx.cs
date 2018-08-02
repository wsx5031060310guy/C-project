using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class zip_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pass=RandomString(20);
        Label1.Text = pass;
        Label2.Text = DateTime.Now.ToString("yyyyMMddHHmmssffff");
    }
    private static Random random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}