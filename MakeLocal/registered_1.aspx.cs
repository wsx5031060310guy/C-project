using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registered_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
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

            List<int> selected = new List<int>();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList3.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            foreach (ListItem item in CheckBoxList4.Items)
            {
                if (item.Selected)
                {
                    selected.Add(1);
                }
                else
                {
                    selected.Add(0);
                }
            }
            if (CheckBox1.Checked)
            {
                selected.Add(1);
            }
            else
            {
                selected.Add(0);
            }
            string howmany = howmany_DropDownList.SelectedValue;
            string age_range_start_year = age_range_start_year_DropDownList.SelectedValue;
            string age_range_start_month = age_range_start_month_DropDownList.SelectedValue;
            string age_range_end_year = age_range_end_year_DropDownList.SelectedValue;
            string age_range_end_month = age_range_end_month_DropDownList.SelectedValue;

            string baby_rule = baby_rule_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");
            string baby_notice = baby_notice_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");
            int money = 0;
            try
            {
                money = Convert.ToInt32(money_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim());
            }
            catch (Exception ex)
            {

                money_Label.Text = "整数を入力してください.";
                return;
                throw ex;
            }


            bool check_db = true;
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select uid from user_information_store";
            sql_f.SelectCommand += " where uid='" + uid + "';";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                check_db = false;
            }

            if (check_db)
            {
                SqlDataSource sql_insert = new SqlDataSource();
                sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_insert.InsertCommand = "insert into user_information_store(uid,choice1_1,choice1_2,choice1_3,choice1_4,choice2_1,choice2_2,choice2_3,choice2_4";
                sql_insert.InsertCommand += ",choice2_5,choice2_6,choice3_1,choice3_2,choice3_3,choice3_4,choice3_5,howmany";
                sql_insert.InsertCommand += ",age_range_start_year,age_range_start_month,age_range_end_year,age_range_end_month,money,CreatedDate";
                if (baby_rule != "")
                {
                    sql_insert.InsertCommand += ",baby_rule";
                }
                if (baby_notice != "")
                {
                    sql_insert.InsertCommand += ",baby_notice";
                }

                sql_insert.InsertCommand += ") values('" + uid + "','" + selected[0] + "','" + selected[2] + "','" + selected[1] + "'";
                sql_insert.InsertCommand += ",'" + selected[3] + "','" + selected[4] + "','" + selected[6] + "','" + selected[8] + "'";
                sql_insert.InsertCommand += ",'" + selected[5] + "','" + selected[7] + "','" + selected[9] + "','" + selected[10] + "'";
                sql_insert.InsertCommand += ",'" + selected[12] + "','" + selected[13] + "','" + selected[11] + "','" + selected[14] + "'";
                sql_insert.InsertCommand += ",'" + howmany + "'";
                sql_insert.InsertCommand += ",'" + age_range_start_year + "','" + age_range_start_month + "','" + age_range_end_year + "','" + age_range_end_month + "',N'" + money + "',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+09:00')";
                if (baby_rule != "")
                {
                    sql_insert.InsertCommand += ",N'" + baby_rule + "'";
                }
                if (baby_notice != "")
                {
                    sql_insert.InsertCommand += ",N'" + baby_notice + "'";
                }
                sql_insert.InsertCommand += ")";
                sql_insert.Insert();

                Session["id"] = uid;


                baby_rule_TextBox.Text = "";
                baby_notice_TextBox.Text = "";

                howmany_DropDownList.SelectedIndex = 0;
                age_range_start_year_DropDownList.SelectedIndex = 0;
                age_range_start_month_DropDownList.SelectedIndex = 0;
                age_range_end_year_DropDownList.SelectedIndex = 0;
                age_range_end_month_DropDownList.SelectedIndex = 0;


                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList2.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList3.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in CheckBoxList4.Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }
                if (CheckBox1.Checked)
                {
                    CheckBox1.Checked = false;
                }


                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from user_information_store";
                sql_f.SelectCommand += " where uid='" + uid + "';";
                sql_f.DataBind();
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                

                string uisid = "";
                if (ict_f.Count > 0)
                {
                    uisid = ict_f.Table.Rows[0]["id"].ToString();

                    SqlDataSource sql_f1 = new SqlDataSource();
                    sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f1.SelectCommand = "select id from user_information_store_week_appointment";
                    sql_f1.SelectCommand += " where uisid='" + uisid + "';";
                    sql_f1.DataBind();

                    DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                    if (ict_f1.Count > 0)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            CheckBox checked_week = (CheckBox)this.FindControl("week_of_day_CheckBox" + i);
                            checked_week.Checked = false;
                            DropDownList start_hour = (DropDownList)this.FindControl("start_hour_DropDownList" + i);
                            DropDownList start_minute = (DropDownList)this.FindControl("start_minute_DropDownList" + i);
                            DropDownList end_hour = (DropDownList)this.FindControl("end_hour_DropDownList" + i);
                            DropDownList end_minute = (DropDownList)this.FindControl("end_minute_DropDownList" + i);
                            start_hour.SelectedIndex = 0;
                            start_minute.SelectedIndex = 0;
                            end_hour.SelectedIndex = 0;
                            end_minute.SelectedIndex = 0;
                        }
                        result_Label.Text = "登録に失敗しました。";
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            CheckBox checked_week = (CheckBox)this.FindControl("week_of_day_CheckBox"+i);
                            int chc = 0;
                            if (checked_week.Checked)
                            {
                                chc = 1;
                            }
                            DropDownList start_hour = (DropDownList)this.FindControl("start_hour_DropDownList" + i);
                            DropDownList start_minute = (DropDownList)this.FindControl("start_minute_DropDownList" + i);
                            DropDownList end_hour = (DropDownList)this.FindControl("end_hour_DropDownList" + i);
                            DropDownList end_minute = (DropDownList)this.FindControl("end_minute_DropDownList" + i);
                            
                            sql_insert = new SqlDataSource();
                            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_insert.InsertCommand = "insert into user_information_store_week_appointment(uisid,checked,week_of_day,week_of_day_jp,start_hour,start_minute,end_hour,end_minute)";
                            sql_insert.InsertCommand += " values('" + uisid + "','" + chc + "','" + (i + 1) + "',N'" + checked_week.Text + "'";
                            sql_insert.InsertCommand += ",'" + start_hour.SelectedValue + "','" + start_minute.SelectedValue + "','" + end_hour.SelectedValue + "','" + end_minute.SelectedValue + "');";
                            sql_insert.Insert();

                        }

                        result_Label.Text = "Success registered.";
                        Response.Redirect("registered_1_1.aspx");
                    }

                }






                //result_Label.Text = "Success registered.";

                //Response.Redirect("registered_1_1.aspx");
            }
            else
            {
                result_Label.Text = "登録に失敗しました。";
            }
        }
    }
}