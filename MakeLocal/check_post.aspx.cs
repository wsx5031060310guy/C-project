using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class check_post : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["manager"] != null)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
            if (Session["manager"].ToString().Trim() != "")
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
                if (Session["manager"].ToString() == "trim")
                {
                    
                }
                else
                {
                    Session.Clear();
                    Response.Redirect("manager_page.aspx");
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect("manager_page.aspx");
            }
        }
        else
        {
            Session.Clear();
            Response.Redirect("manager_page.aspx");
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {

        Panel1.Controls.Clear();
        string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 0"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView1.DataSource = dt;
        //                GridView1.DataBind();
        //            }
        //        }
        //    }
        //}
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 2"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView2.DataSource = dt;
        //                GridView2.DataBind();
        //            }
        //        }
        //    }
        //}
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 3"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView3.DataSource = dt;
        //                GridView3.DataBind();
        //            }
        //        }
        //    }
        //}
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 4"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView4.DataSource = dt;
        //                GridView4.DataBind();
        //            }
        //        }
        //    }
        //}
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 5"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView5.DataSource = dt;
        //                GridView5.DataBind();
        //            }
        //        }
        //    }
        //}
        //using (MySqlConnection con = new MySqlConnection(constr))
        //{
        //    using (MySqlCommand cmd = new MySqlCommand("SELECT message_type,postal_code,place FROM status_messages WHERE message_type = 6"))
        //    {
        //        using (MySqlDataAdapter da = new MySqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            da.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                da.Fill(dt);
        //                GridView6.DataSource = dt;
        //                GridView6.DataBind();
        //            }
        //        }
        //    }
        //}
        Query = "select id";
        Query += " from user_login;";
        DataView ict_place = gc.select_cmd(Query);
        Label5.Text = ict_place.Count.ToString();
        Query = "select id";
        Query += " from status_messages;";
        ict_place = gc.select_cmd(Query);
        Label6.Text = ict_place.Count.ToString();

        //area count
        Query = "select zipcode_old from zipcode_f_01 where addr_pref like '神奈川県' and zipcode like '%%%0000';";
        ict_place = gc.select_cmd(Query);
        if (ict_place.Count > 0)
        {
            string qustr = " and ( postal_code like '" + ict_place.Table.Rows[0]["zipcode_old"].ToString() + "-%%%%'";
            for (int i = 1; i < ict_place.Count; i++)
            {
                qustr += " or postal_code like '" + ict_place.Table.Rows[i]["zipcode_old"].ToString() + "-%%%%'";
            }
            qustr += ")";
            Query = "select count(id) as howmany,postal_code";
            Query += " from status_messages where 1=1";
            Query += qustr;
            Query += " group by postal_code ORDER BY howmany desc;";
            DataView ict_place1 = gc.select_cmd(Query);
            DataView ict_place2;
            if (ict_place1.Count > 0)
            {
                Label laa = new Label();
                Label laa1 = new Label();
                Label laa2 = new Label();
                string placename = "";
                for (int i = 0; i < ict_place1.Count; i++)
                {
                    placename = "";
                    laa = new Label();
                    laa.Font.Size = FontUnit.Point(14);
                    laa1 = new Label();
                    laa1.Font.Size = FontUnit.Point(14);
                    laa2 = new Label();
                    laa2.Font.Size = FontUnit.Point(14);
                    Query = "select addr_pref,addr_city,addr_town from zipcode_f_01 where zipcode='" + ict_place1.Table.Rows[i]["postal_code"].ToString().Replace("-", "") + "';";
                    ict_place2 = gc.select_cmd(Query);
                    if (ict_place2.Count > 0)
                    {
                        placename += ict_place2.Table.Rows[0]["addr_pref"].ToString() + ict_place2.Table.Rows[0]["addr_city"].ToString() + ict_place2.Table.Rows[0]["addr_town"].ToString();
                    }
                    Panel1.Controls.Add(new LiteralControl("<fieldset><legend style='font-size: large; font-weight: bold'>" + ict_place1.Table.Rows[i]["postal_code"].ToString() + "</legend>"));

                    laa1.Text = "所在地:" + placename;
                    Panel1.Controls.Add(laa1);
                    Panel1.Controls.Add(new LiteralControl("<br/>"));
                    laa.Text = "郵便番号:" + ict_place1.Table.Rows[i]["postal_code"].ToString() ;
                    Panel1.Controls.Add(laa);
                    Panel1.Controls.Add(new LiteralControl("<br/>"));
                    laa2.Text = ict_place1.Table.Rows[i]["howmany"].ToString() + "回";
                    Panel1.Controls.Add(laa2);
                    Panel1.Controls.Add(new LiteralControl("<br/>"));

                    Panel1.Controls.Add(new LiteralControl("</fieldset>"));
                }
            }
        }

        //post type count
        Query = "select id from status_messages where message_type='0';";
        ict_place = gc.select_cmd(Query);
        Label7.Text = ict_place.Count.ToString() + "回";
        Query = "select id from status_messages where message_type='2';";
        ict_place = gc.select_cmd(Query);
        Label8.Text = ict_place.Count.ToString() + "回";
        Query = "select id from status_messages where message_type='3';";
        ict_place = gc.select_cmd(Query);
        Label9.Text = ict_place.Count.ToString() + "回";
        Query = "select id from status_messages where message_type='4';";
        ict_place = gc.select_cmd(Query);
        Label10.Text = ict_place.Count.ToString() + "回";
        Query = "select id from status_messages where message_type='5';";
        ict_place = gc.select_cmd(Query);
        Label11.Text = ict_place.Count.ToString() + "回";
        Query = "select id from status_messages where message_type='6';";
        ict_place = gc.select_cmd(Query);
        Label12.Text = ict_place.Count.ToString() + "回";




    }
    protected string GetFormattedName(string value)
    {
        // you can split full name here and swap first name, last name
        // depending on how you store it in DB 
        string res = "";
        if (value == "0")
        {
            res = "お食事";
        }
        else if (value == "1")
        {
            res = "人気スポット";
        }
        else if (value == "2")
        {
            res = "イベント";
        }
        else if (value == "3")
        {
            res = "病院";
        }
        else if (value == "4")
        {
            res = "公園／レジャー";
        }
        else if (value == "5")
        {
            res = "授乳室";
        }
        else if (value == "6")
        {
            res = "指定なし";
        }
        return res;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Find the value in the c04_oprogrs column. You'll have to use
            // some trial and error here to find the right control. The line
            // below may provide the desired value but I'm not entirely sure.
            //string value = e.Row.Cells[0].Text;
            TableCell statusCell = e.Row.Cells[0];
            if (statusCell.Text == "0")
            {
                statusCell.Text = "お食事";
            }
            else if (statusCell.Text == "1")
            {
                statusCell.Text = "人気スポット";
            }
            else if (statusCell.Text == "2")
            {
                statusCell.Text = "イベント";
            }
            else if (statusCell.Text == "3")
            {
                statusCell.Text = "病院";
            }
            else if (statusCell.Text == "4")
            {
                statusCell.Text = "公園／レジャー";
            }
            else if (statusCell.Text == "5")
            {
                statusCell.Text = "授乳室";
            }
            else if (statusCell.Text == "6")
            {
                statusCell.Text = "指定なし";
            }


            // Next find the label in the template field.
            Label myLabel = (Label)e.Row.FindControl("Label5");
            //if (value == "0")
            //{
            //    myLabel.Text = "お食事";
            //}
            //else if (value == "1")
            //{
            //    myLabel.Text = "人気スポット";
            //}
            //else if (value == "2")
            //{
            //    myLabel.Text = "イベント";
            //}
            //else if (value == "3")
            //{
            //    myLabel.Text = "病院";
            //}
            //else if (value == "4")
            //{
            //    myLabel.Text = "公園／レジャー";
            //}
            //else if (value == "5")
            //{
            //    myLabel.Text = "授乳室";
            //}
            //else if (value == "6")
            //{
            //    myLabel.Text = "指定なし";
            //}
        }
    }


    [WebMethod]
    public static string search_grid(string param1, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";
        Query1 = "SELECT message_type,postal_code,place FROM status_messages WHERE message_type =" + param1 + @" LIMIT " + (Convert.ToInt32( param3) * 10) + ",10;";
        DataView ict_f = gc1.select_cmd(Query1);
        result += "<table width='100%'>";
        result += "<tr>";
        result += "<td>カテゴリー</td>";
        result += "<td>郵便番号</td>";
        result += "<td>所在地</td>";
        result += "</tr>";
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                result += "<tr>";
                result += "<td>";
                string type = "";
                if (ict_f.Table.Rows[i]["message_type"].ToString() == "0")
                {
                    result += "お食事";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "1")
                {
                    result += "人気スポット";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "2")
                {
                    result += "イベント";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "3")
                {
                    result += "病院";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "4")
                {
                    result += "公園／レジャー";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "5")
                {
                    result += "授乳室";
                }
                else if (ict_f.Table.Rows[i]["message_type"].ToString() == "6")
                {
                    result += "指定なし";
                }
                result += "</td>";
                result += "<td>" + ict_f.Table.Rows[i]["postal_code"].ToString() + @"</td>";
                result += "<td>" + ict_f.Table.Rows[i]["place"].ToString() + @"</td>";
                result += "</tr>";
            }


        }

        return result;
    }

   
}