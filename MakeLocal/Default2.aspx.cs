using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        SqlDataSource2.SelectCommand = "select zip_no,iconName from tb_company where id=2;";
        SqlDataSource2.DataBind();
        DataView ou = (DataView)SqlDataSource2.Select(DataSourceSelectArguments.Empty);
        FormView form = (FormView)FindControl("FormView1");
        Label la = (Label)form.FindControl("Label1");
        Label la1 = (Label)form.FindControl("Label2");

        if (ou.Count > 0)
        {
            la.Text = ou.Table.Rows[0]["zip_no"].ToString();
            la1.Text = ou.Table.Rows[0]["iconName"].ToString();
            FormView1.Visible = true;
        }
        else
        {
            FormView1.Visible = false;
        }
    }
    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        FormView form = (FormView)FindControl("FormView1");
        TextBox txt=(TextBox)form.FindControl("company_titleTextBox");
        txt.Text="";
        TextBox txt1=(TextBox)form.FindControl("addressTextBox");
        txt1.Text="";
        TextBox txt2 = (TextBox)form.FindControl("latTextBox");
        txt2.Text = "";
        TextBox txt3 = (TextBox)form.FindControl("lngTextBox");
        txt3.Text = "";
        TextBox txt4 = (TextBox)form.FindControl("company_descTextBox");
        txt4.Text = "";
        
        FormView1.Visible = false;
    }
    protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        string add,lat,lng,company_title,company_desc;
        if (e.Command.Parameters["@address"].Value == null)
        {
            add = "";
        }
        else
        {
            add = Regex.Replace(e.Command.Parameters["@address"].Value.ToString(), @"['""]", " ");
        }
        if (e.Command.Parameters["@lat"].Value == null)
        {
            lat = "";
        }
        else
        {
            lat = Regex.Replace(e.Command.Parameters["@lat"].Value.ToString(), @"['""]", " ");
        }
        //
        if (e.Command.Parameters["@lng"].Value == null)
        {
            lng = "";
        }
        else
        {
            lng = Regex.Replace(e.Command.Parameters["@lng"].Value.ToString(), @"['""]", " ");
        }
        if (e.Command.Parameters["@company_title"].Value == null)
        {
            company_title = "";
        }
        else
        {
            company_title = Regex.Replace(e.Command.Parameters["@company_title"].Value.ToString(), @"['""]", " ");
        }
        if (e.Command.Parameters["@company_desc"].Value == null)
        {
            company_desc = "";
        }
        else
        {
            company_desc = Regex.Replace(e.Command.Parameters["@company_desc"].Value.ToString(), @"['""]", " ");
        }


        if (add == "")
        {
            if (lat == "" && lng == "")
            {
                e.Command.Parameters["@lat"].Value = 24.746728;
                e.Command.Parameters["@lng"].Value = 121.746238;
            }
        }
        else
        {
            e.Command.Parameters["@lat"].Value = 0;
            e.Command.Parameters["@lng"].Value = 0;
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}