using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class givesee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();

        //////test chatroom.aspx
        //Session["username"] = 1;

        //Session["loginname"] = "admin";
        //Session["id"] = 1;

        //Response.Redirect("chatroom.aspx");


        //Session.Clear();
           // Session["id"] = 10;
             //Response.Redirect("registered_1_4.aspx");


        ////test user_date_manger.aspx
        //Session["id"] = 10;
        //Session["sup_id"] = 1;
        //Response.Redirect("user_date_manger.aspx");

        ////test Date_Calendar.aspx
        //Session["id"] = 1;
        //Session["sup_id"] = 10;
        //Response.Redirect("Date_Calendar.aspx");

        ////test user_home.aspx img
        //Session["id"] = 15;
        //Response.Redirect("user_home.aspx");

        ////test user_home.aspx url
        //Session["id"] = 19;
        //Response.Redirect("user_home.aspx");

        ////test main.aspx
        //Session["id"] = 1;
        //Response.Redirect("main.aspx");

        ////test main.aspx
        //Session["id"] = 9;
        //Response.Redirect("main.aspx");

        ////test main.aspx
        //Session["id"] = 18;
        //Response.Redirect("main.aspx");

        //test main.aspx
        Session["id"] = 15;
        Response.Redirect("photo.aspx");



    }
}
