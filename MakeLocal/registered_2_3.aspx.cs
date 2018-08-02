using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registered_2_3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("home.aspx");
        }
        else
        {
            string uid = Session["id"].ToString();
            Session["id"] = uid;
            bool check_db = false;
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id from user_information_school";
            sql_f.SelectCommand += " where uid='" + uid + "';";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                check_db = true;
            }

            if (check_db)
            {
                string uischid = ict_f.Table.Rows[0]["id"].ToString();


                int use_rule = 0, video_rule = 0;
                if (use_rule_CheckBox.Checked)
                {
                    use_rule = 1;
                    use_rule_Label.Text = "";
                }
                else
                {
                    use_rule = 0;
                    use_rule_Label.Text = "Use rule have not select.";
                }
             
                if (use_rule_CheckBox.Checked)
                {
                    SqlDataSource sql_update = new SqlDataSource();
                    sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_update.UpdateCommand = "update user_information_school";
                    sql_update.UpdateCommand += " set use_rule='" + use_rule + "',video_rule='" + video_rule + "'";
                    sql_update.UpdateCommand += " where id='" + uischid + "';";
                    sql_update.Update();

                    result_Label.Text = "Success registered.";
                    Response.Redirect("main.aspx");
                }
                else
                {
                    result_Label.Text = "登録に失敗しました。";
                }

            }
            else
            {
                result_Label.Text = "登録に失敗しました。";
            }
        }
    }
}