using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.VisualBasic;
using System.Data;

public partial class Narrow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        this.SqlDataSource1.SelectCommand = "select address from tb_company where id=2;"; //下SQL語法
        DataView dvvv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);//撈取SqlDataSource1 裡面的資料
        for (int i = 0; i < dvvv.Count; i++)
        {
            Label1.Text = dvvv.Table.Rows[0]["address"].ToString();
        }

    }
}