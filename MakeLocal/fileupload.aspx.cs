using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class fileupload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string input = "",DirRoot = "",filename="";
        int stringindex=0,cut=0;
        Boolean check = false;
        if (FileUpload1.HasFile)
        {
            input = FileUpload1.FileName;
            stringindex = input.LastIndexOf(@".");
            cut = input.Length - stringindex;
            DirRoot = input.Substring(stringindex + 1, cut - 1);

            SqlDataSource2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource2.SelectCommand = "select id,name from filename_extension";
            SqlDataSource2.DataBind();
            DataView ou1 = (DataView)SqlDataSource2.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ou1.Count; i++)
            {
                if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
                {
                    check = true;
                }
            }
            if (check)
            {
                SqlDataSource sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                sql_insert.InsertCommand = "insert into filename_detail(filename,name)";
                sql_insert.InsertCommand += " values('~/fileplace/" + filename + "','" + FileUpload1.FileName.ToString() + "')";
                sql_insert.Insert();
                Label1.Text += FileUpload1.FileName.ToString() + "  finish<br>";
                FileUpload1.SaveAs(Server.MapPath("fileplace") + "\\" + filename);
                Image1.ImageUrl = "~/fileplace/" + filename;
                GridView1.DataBind();
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(upload, upload.GetType(), "alert", "alert('filename extension is not in role!')", true);
            }


        }
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        Session["ImageBytes"] = FileUpload1.FileBytes;
        Image1.ImageUrl = "~/ImageHandler.ashx";
    }
}