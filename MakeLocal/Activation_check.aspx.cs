using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Activation_check : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!this.IsPostBack)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();

            Query = "select uid,supp_id,uiacdid";
            Query += " from user_appointment_check";
            Query += " where ActivationCode='" + activationCode + "';";

            DataView ict_f = gc.select_cmd(Query);
            int id = 0, supid = 0, uiacdid = 0;
            for (int i = 0; i < ict_f.Count; i++)
            {
                id = Convert.ToInt32(ict_f.Table.Rows[i]["uid"].ToString());
                supid = Convert.ToInt32(ict_f.Table.Rows[i]["supp_id"].ToString());
                uiacdid = Convert.ToInt32(ict_f.Table.Rows[i]["uiacdid"].ToString());
            }
            //now time
            string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
            string startm = DateTime.Now.Minute.ToString();
            string starts = DateTime.Now.Second.ToString();
            string start = startd + " " + starth + ":" + startm + ":" + starts;
            //now time
            if (Session["id"] != null)
            {
                if (Session["id"].ToString() == supid.ToString())
                {
                    if (id == 0)
                    {

                    }
                    else
                    {
                        Query = "select id";
                        Query += " from user_information_appointment_check_deal";
                        Query += " where uid='" + id + "' and id='" + uiacdid + "';";

                        DataView ict_f_user = gc.select_cmd(Query);
                        for (int i = 0; i < ict_f_user.Count; i++)
                        {

                            Query = "update user_information_appointment_check_deal";
                            Query += " set check_success='1',second_check_time='" + start + "' where id='" + ict_f_user.Table.Rows[i]["id"].ToString() + "';";

                            resin = gc.update_cmd(Query);

                        }

                    }

                    string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(constr))
                    {

                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM user_appointment_check WHERE ActivationCode ='" + activationCode + "'"))
                        {
                            using (MySqlDataAdapter sda = new MySqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Connection = con;
                                con.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();
                                con.Close();
                                if (rowsAffected == 1)
                                {
                                    ltMessage.Text = "予約の確認が完了しました。";
                                }
                                else
                                {
                                    ltMessage.Text = "予約の確認に失敗しました。";
                                }
                            }
                        }
                    }
                }
                else
                {
                    ltMessage.Text = "このアカウントはサポーター登録されていません。";
                }
            }
            else
            {
                ltMessage.Text = "失敗しました。";
            }

            Label1.Text = "10";

        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        int rr = Convert.ToInt32(Label1.Text);

        if (rr == 0)
        {
            Response.Redirect("main.aspx");
        }
        else
        {
            rr -= 1;
            Label1.Text = Convert.ToString(rr);
        }
    }
}