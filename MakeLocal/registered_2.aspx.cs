using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registered_2 : System.Web.UI.Page
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
    [WebMethod]
    public static string Save(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
    {
        //string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8 + "," + param9 + "," + param10;
        string result = "";
        try
        {
        

        string id = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string real_first_name = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string real_second_name = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string real_spell_first_name = param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string real_spell_second_name = param5.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        string date = param6.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        string school_name = param7.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string hospital_name = param8.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string sick_name = param9.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string sex = param10.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        //get birthday year month day
        int ind = date.IndexOf(@"/"),ind2=date.LastIndexOf(@"/");
        string year = date.Substring(0,ind),month=date.Substring(ind+1,ind2-ind-1),day=date.Substring(ind2+1,date.Length-ind2-1);
        //result += ",Y:"+year+",M:"+month+",D:"+day;


        
            int sexx = 0;
            if (sex == "Girl")
            {
                sexx = 0;
            }
            if (sex == "Boy")
            {
                sexx = 1;
            }
            int iid = Convert.ToInt32(id);


            if (real_first_name != "" && real_second_name != "" && real_spell_first_name != "" && real_spell_second_name != "")
            {
                SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from user_login";
                sql_f.SelectCommand += " where id='" + iid + "';";
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);


                if (ict_f.Count > 0)
                {
                    SqlDataSource sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into user_information_school_children(uid,real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,sex,birthday_year";
                    sql_insert.InsertCommand += ",birthday_month,birthday_day,school_name,hospital_name,sick_name)";
                    sql_insert.InsertCommand += " values('" + iid + "',N'" + real_first_name + "',N'" + real_second_name + "',N'" + real_spell_first_name + "',N'" + real_spell_second_name + "','" + sexx+ "'";
                    sql_insert.InsertCommand += ",'" + year + "','" + month + "','" + day + "',N'" + school_name + "',N'" + hospital_name + "',N'" + sick_name + "')";
                    sql_insert.Insert();




                    result = "success";
                    //return result;
                }
                //result = "fail";
            }
            else
            {
                if (real_first_name == "")
                {
                    result += "1,";
                }
                if (real_second_name == "")
                {
                    result += "2,";
                }
                if (real_spell_first_name == "")
                {
                    result += "3,";
                }
                if (real_spell_second_name == "")
                {
                    result += "4,";
                }
            }

        }
        catch (Exception ex)
        {
            //result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {



        //Response.Redirect("registered_2_1.aspx");
    }
}