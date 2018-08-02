using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class time_page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TimeZoneInfo TPZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        DateTime NOWTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TPZone);
        Label1.Text = NOWTime.ToString("yyyyMMddHHmmssffff");

        //foreach (TimeZoneInfo timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
        //{
        //    ListBox1.Items.Add(timeZoneInfo.DisplayName);
        //    ListBox1.Items.Add(timeZoneInfo.Id);
        //} 
    }
   
}