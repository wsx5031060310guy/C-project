using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class registered : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //TextBox1.Text = "二ックネーム";
        //TextBox1.Attributes.Add("style", "font-size:xx-small;color:#CCCCCC");//順便改變字的大小顏色
        //TextBox1.Attributes.Add("onFocus", "this.value=''");//一點擊TextBox1後，裡面的字馬上被清空
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";

        if (!IsPostBack)
        {
            if (this.SelectedDate == DateTime.MinValue)
            {
                this.PopulateYear();
                this.PopulateMonth();
                this.PopulateDay();
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        else
        {
            if (Request.Form[ddlDay.UniqueID] != null)
            {
                this.PopulateDay();
                ddlDay.ClearSelection();
                ddlDay.Items.FindByValue(Request.Form[ddlDay.UniqueID]).Selected = true;
            }
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Query = "select city_name from City;";
        DataView ict_f = gc.select_cmd(Query);
        City_DropDownList.Items.Clear();
        for (int i = 0; i < ict_f.Count; i++)
        {
            City_DropDownList.Items.Add(ict_f.Table.Rows[i]["city_name"].ToString());
        }
        City_DropDownList.Items.FindByValue("神奈川県").Selected = true;
        City_DropDownList.Items.FindByText("神奈川県").Selected = true;

    }
    [WebMethod]
    public static string Save(string param1, string param2,string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2+","+param3;
        string id = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string age = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        string sex = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        try
        {
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
            int iage = Convert.ToInt32(age);


            Query1 = "select id from user_login";
            Query1 += " where id='" + iid + "';";
            DataView ict_f = gc1.select_cmd(Query1);


            if (ict_f.Count > 0)
            {

                Query1 = "insert into user_family(uid,family_old,sex)";
                Query1 += " values('" + iid + "','" + iage + "','" + sexx + "')";
                resin = gc1.insert_cmd(Query1);




                result = "success";
                //return result;
            }
            result = "fail";
            //return result;
        }
        catch (Exception ex)
        {
            result = "fail";
            //return result;
            throw ex;
        }
        return result;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        if ( Session["id"] == null)
        {
            ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("main.aspx");
        }
        else
        {
            string uid = Session["id"].ToString();

            string real_first_name = real_first_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_second_name = real_second_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_first_name = real_spell_first_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_second_name = real_spell_second_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            bool check_Sex_radio = false;
            string Sex = "";
            if (Sex_radio.SelectedIndex > -1)
            {
                check_Sex_radio = true;
                Sex = Sex_radio.SelectedValue;
                sex_Label.Text = "";
            }
            else
            {
                check_Sex_radio = false;
                Sex = "";
                sex_Label.Text = "性別を選択してください。";
            }
            string birth_year = ddlYear.SelectedValue;
            string birth_mon = ddlMonth.SelectedValue;
            string birth_day = ddlDay.SelectedValue;

            string work_state = work_DropDownList.SelectedValue;

            string postal_code = postal_code_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string country = City_DropDownList.SelectedValue;
            string city = add_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string chome = add_TextBox1.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string house_number = apartment_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string station_line = train_line_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string station_name = train_station_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string phone_number = phone_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string other_phone_number = other_phone_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string relationship = relationship_DropDownList.SelectedValue;

            bool check_real_first_name = false, check_real_second_name = false, check_real_spell_first_name = false
                , check_real_spell_second_name = false, check_postal_code = false
                , check_city = false, check_chome = false, check_phone_number = false;

            if (real_first_name != "")
            {
                check_real_first_name = true;
                rfname_Label.Text = "";
            }
            else
            {
                check_real_first_name = false;
                rfname_Label.Text = "姓を入力してください";
            }

            if (real_second_name != "")
            {
                check_real_second_name = true;
                rsname_Label.Text = "";
            }
            else
            {
                check_real_second_name = false;
                rsname_Label.Text = "名を入力してください。";
            }

            if (real_spell_first_name != "")
            {
                check_real_spell_first_name = true;
                rsfname_Label.Text = "";
            }
            else
            {
                check_real_spell_first_name = false;
                rsfname_Label.Text = "姓の読みをカタカナで入力してください。";
            }

            if (real_spell_second_name != "")
            {
                check_real_spell_second_name = true;
                rssname_Label.Text = "";
            }
            else
            {
                check_real_spell_second_name = false;
                rssname_Label.Text = "名の読みをカタカナで入力してください。";
            }

            if (postal_code != "")
            {
                check_postal_code = true;
                poc_Label.Text = "";
            }
            else
            {
                check_postal_code = false;
                poc_Label.Text = "郵便番号をハイフンをつけて入力してください。";
            }
            if (city != "")
            {
                check_city = true;
                city_Label.Text = "";
            }
            else
            {
                check_city = false;
                city_Label.Text = "市区町村を入力してください";
            }
            if (chome != "")
            {
                check_chome = true;
                chome_Label.Text = "";
            }
            else
            {
                check_chome = false;
                chome_Label.Text = "番地を入力してください。";
            }
            if (phone_number != "")
            {
                check_phone_number = true;
                phone_Label.Text = "";
            }
            else
            {
                check_phone_number = false;
                phone_Label.Text = "電話番号を入力してください";
            }


            if (check_Sex_radio && check_real_first_name && check_real_second_name && check_real_spell_first_name &&
                 check_real_spell_second_name && check_postal_code && check_city && check_chome && check_phone_number)
            {
                bool check_db = true;

                Query = "select uid from user_information";
                Query += " where uid='" + uid + "';";
                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    check_db = false;
                }

                if (check_db)
                {
                    Query = "insert into user_information(uid,real_first_name,real_second_name,real_spell_first_name,real_spell_second_name";
                    Query += ",Sex,birthday_year,birthday_month,birthday_day,live_postal_code,country,city,Chome,phone_number,work_state,relationship";
                    if (house_number != "")
                    {
                        Query += ",house_number";
                    }
                    if (station_line != "")
                    {
                        Query += ",station_line";
                    }
                    if (station_name != "")
                    {
                        Query += ",station_name";
                    }
                    if (other_phone_number != "")
                    {
                        Query += ",other_phone_number";
                    }

                    Query += ") values('" + uid + "','" + real_first_name + "','" + real_second_name + "','" + real_spell_first_name + "'";
                    Query += ",'" + real_spell_second_name + "','" + Sex + "','" + birth_year + "','" + birth_mon + "'";
                    Query += ",'" + birth_day + "','" + postal_code + "','" + country + "','" + city + "'";
                    Query += ",'" + chome + "','" + phone_number + "','" + work_state + "','" + relationship + "'";
                    if (house_number != "")
                    {
                        Query += ",'" + house_number + "'";
                    }
                    if (station_line != "")
                    {
                        Query += ",'" + station_line + "'";
                    }
                    if (station_name != "")
                    {
                        Query += ",'" + station_name + "'";
                    }
                    if (other_phone_number != "")
                    {
                        Query += ",'" + other_phone_number + "'";
                    }
                    Query += ")";
                    resin = gc.insert_cmd(Query);

                    Session["id"] = uid;


                    real_first_name_TextBox.Text = "";
                    real_second_name_TextBox.Text = "";
                    real_spell_first_name_TextBox.Text = "";
                    real_spell_second_name_TextBox.Text = "";

                    Sex_radio.SelectedIndex = -1;
                    this.PopulateYear();
                    this.PopulateMonth();
                    this.PopulateDay();

                    work_DropDownList.SelectedIndex = 0;
                    postal_code_TextBox.Text = "";
                    City_DropDownList.Items.FindByValue("神奈川県").Selected = true;
                    City_DropDownList.Items.FindByText("神奈川県").Selected = true;
                    add_TextBox.Text = "";
                    add_TextBox1.Text = "";
                    apartment_TextBox.Text = "";
                    train_line_TextBox.Text = "";
                    train_station_TextBox.Text = "";
                    phone_TextBox.Text = "";
                    other_phone_TextBox.Text = "";
                    relationship_DropDownList.SelectedIndex = 0;


                    result_Label.Text = "Success registered.";
                    Response.Redirect("registered_1.aspx");
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

            //Page.ClientScript.RegisterStartupScript(GetType(), Button2.ID, "save();", true);

            //ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('" + birth_year + "," + birth_mon + "," + birth_day + "')", true);



            //ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('" + Sex_radio.SelectedIndex + "')", true);

            //Page.ClientScript.RegisterStartupScript(GetType(), Button2.ID, "save();", true);

            //Response.Redirect("registered_1.aspx");
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            ScriptManager.RegisterStartupScript(Button3, Button3.GetType(), "alert", "alert('Sorry you stay too long!')", true);
            Response.Redirect("home.aspx");
        }
        else
        {
            string uid = Session["id"].ToString();

            string real_first_name = real_first_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_second_name = real_second_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_first_name = real_spell_first_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_second_name = real_spell_second_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            bool check_Sex_radio = false;
            string Sex = "";
            if (Sex_radio.SelectedIndex > -1)
            {
                check_Sex_radio = true;
                Sex = Sex_radio.SelectedValue;
                sex_Label.Text = "";
            }
            else
            {
                check_Sex_radio = false;
                Sex = "";
                sex_Label.Text = "性別を選択してください。";
            }
            string birth_year = ddlYear.SelectedValue;
            string birth_mon = ddlMonth.SelectedValue;
            string birth_day = ddlDay.SelectedValue;

            string work_state = work_DropDownList.SelectedValue;

            string postal_code = postal_code_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string country = City_DropDownList.SelectedValue;
            string city = add_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string chome = add_TextBox1.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string house_number = apartment_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string station_line = train_line_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string station_name = train_station_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string phone_number = phone_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string other_phone_number = other_phone_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string relationship = relationship_DropDownList.SelectedValue;

            bool check_real_first_name = false, check_real_second_name = false, check_real_spell_first_name = false
                , check_real_spell_second_name = false, check_postal_code = false
                , check_city = false, check_chome = false, check_phone_number = false;

            if (real_first_name != "")
            {
                check_real_first_name = true;
                rfname_Label.Text = "";
            }
            else
            {
                check_real_first_name = false;
                rfname_Label.Text = "姓を入力してください";
            }

            if (real_second_name != "")
            {
                check_real_second_name = true;
                rsname_Label.Text = "";
            }
            else
            {
                check_real_second_name = false;
                rsname_Label.Text = "名を入力してください。";
            }

            if (real_spell_first_name != "")
            {
                check_real_spell_first_name = true;
                rsfname_Label.Text = "";
            }
            else
            {
                check_real_spell_first_name = false;
                rsfname_Label.Text = "名を入力してください。";
            }

            if (real_spell_second_name != "")
            {
                check_real_spell_second_name = true;
                rssname_Label.Text = "";
            }
            else
            {
                check_real_spell_second_name = false;
                rssname_Label.Text = "名の読みをカタカナで入力してください。";
            }

            if (postal_code != "")
            {
                check_postal_code = true;
                poc_Label.Text = "";
            }
            else
            {
                check_postal_code = false;
                poc_Label.Text = "郵便番号をハイフンをつけて入力してください。";
            }
            if (city != "")
            {
                check_city = true;
                city_Label.Text = "";
            }
            else
            {
                check_city = false;
                city_Label.Text = "市区町村を入力してください";
            }
            if (chome != "")
            {
                check_chome = true;
                chome_Label.Text = "";
            }
            else
            {
                check_chome = false;
                chome_Label.Text = "番地を入力してください。";
            }
            if (phone_number != "")
            {
                check_phone_number = true;
                phone_Label.Text = "";
            }
            else
            {
                check_phone_number = false;
                phone_Label.Text = "電話番号を入力してください";
            }


            if (check_Sex_radio && check_real_first_name && check_real_second_name && check_real_spell_first_name &&
                 check_real_spell_second_name && check_postal_code && check_city && check_chome && check_phone_number)
            {

                bool check_db = true;

                Query = "select uid from user_information";
                Query += " where uid='" + uid + "';";
                DataView ict_f = gc.select_cmd(Query);
                if (ict_f.Count > 0)
                {
                    check_db = false;
                }

                if (check_db)
                {

                    Query = "insert into user_information(uid,real_first_name,real_second_name,real_spell_first_name,real_spell_second_name";
                    Query += ",Sex,birthday_year,birthday_month,birthday_day,live_postal_code,country,city,Chome,phone_number,work_state,relationship";
                    if (house_number != "")
                    {
                        Query += ",house_number";
                    }
                    if (station_line != "")
                    {
                        Query += ",station_line";
                    }
                    if (station_name != "")
                    {
                        Query += ",station_name";
                    }
                    if (other_phone_number != "")
                    {
                        Query += ",other_phone_number";
                    }

                    Query += ") values('" + uid + "','" + real_first_name + "','" + real_second_name + "','" + real_spell_first_name + "'";
                    Query += ",'" + real_spell_second_name + "','" + Sex + "','" + birth_year + "','" + birth_mon + "'";
                    Query += ",'" + birth_day + "','" + postal_code + "','" + country + "','" + city + "'";
                    Query += ",'" + chome + "','" + phone_number + "','" + work_state + "','" + relationship + "'";
                    if (house_number != "")
                    {
                        Query += ",'" + house_number + "'";
                    }
                    if (station_line != "")
                    {
                        Query += ",'" + station_line + "'";
                    }
                    if (station_name != "")
                    {
                        Query += ",'" + station_name + "'";
                    }
                    if (other_phone_number != "")
                    {
                        Query += ",'" + other_phone_number + "'";
                    }
                    Query += ")";
                    resin = gc.insert_cmd(Query);

                    Session["id"] = uid;


                    real_first_name_TextBox.Text = "";
                    real_second_name_TextBox.Text = "";
                    real_spell_first_name_TextBox.Text = "";
                    real_spell_second_name_TextBox.Text = "";

                    Sex_radio.SelectedIndex = -1;
                    this.PopulateYear();
                    this.PopulateMonth();
                    this.PopulateDay();

                    work_DropDownList.SelectedIndex = 0;
                    postal_code_TextBox.Text = "";
                    City_DropDownList.Items.FindByValue("神奈川県").Selected = true;
                    City_DropDownList.Items.FindByText("神奈川県").Selected = true;
                    add_TextBox.Text = "";
                    add_TextBox1.Text = "";
                    apartment_TextBox.Text = "";
                    train_line_TextBox.Text = "";
                    train_station_TextBox.Text = "";
                    phone_TextBox.Text = "";
                    other_phone_TextBox.Text = "";
                    relationship_DropDownList.SelectedIndex = 0;


                    result_Label.Text = "Success registered.";
                    Response.Redirect("registered_2.aspx");
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


            //Response.Redirect("registered_2.aspx");
        }
    }
    private int Day
    {
        get
        {
            if (Request.Form[ddlDay.UniqueID] != null)
            {
                return int.Parse(Request.Form[ddlDay.UniqueID]);
            }
            else
            {
                return int.Parse(ddlDay.SelectedItem.Value);
            }
        }
        set
        {
            this.PopulateDay();
            ddlDay.ClearSelection();
            ddlDay.Items.FindByValue(value.ToString()).Selected = true;
        }
    }
    private int Month
    {
        get
        {
            return int.Parse(ddlMonth.SelectedItem.Value);
        }
        set
        {
            this.PopulateMonth();
            ddlMonth.ClearSelection();
            ddlMonth.Items.FindByValue(value.ToString()).Selected = true;
        }
    }
    private int Year
    {
        get
        {
            return int.Parse(ddlYear.SelectedItem.Value);
        }
        set
        {
            this.PopulateYear();
            ddlYear.ClearSelection();
            ddlYear.Items.FindByValue(value.ToString()).Selected = true;
        }
    }

    public DateTime SelectedDate
    {
        get
        {
            try
            {
                return DateTime.Parse(this.Month + "/" + this.Day + "/" + this.Year);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        set
        {
            if (!value.Equals(DateTime.MinValue))
            {
                this.Year = value.Year;
                this.Month = value.Month;
                this.Day = value.Day;
            }
        }
    }
    private void PopulateDay()
    {
        ddlDay.Items.Clear();
        ListItem lt = new ListItem();
        //lt.Text = "DD";
        //lt.Value = "0";
        //ddlDay.Items.Add(lt);
        int days = DateTime.DaysInMonth(this.Year, this.Month);
        for (int i = 1; i <= days; i++)
        {
            lt = new ListItem();
            lt.Text = i.ToString();
            lt.Value = i.ToString();
            ddlDay.Items.Add(lt);
        }
        ddlDay.Items.FindByValue(DateTime.Now.Day.ToString()).Selected = true;
    }

    private void PopulateMonth()
    {
        ddlMonth.Items.Clear();
        ListItem lt = new ListItem();
        //lt.Text = "MM";
        //lt.Value = "0";
        //ddlMonth.Items.Add(lt);
        for (int i = 1; i <= 12; i++)
        {
            lt = new ListItem();
            lt.Text = Convert.ToDateTime(i.ToString() + "/1/1900").ToString("MM");
            lt.Value = i.ToString();
            ddlMonth.Items.Add(lt);
        }
        ddlMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
    }

    private void PopulateYear()
    {
        ddlYear.Items.Clear();
        ListItem lt = new ListItem();
        //lt.Text = "YYYY";
        //lt.Value = "0";
        //ddlYear.Items.Add(lt);
        for (int i = DateTime.Now.Year; i >= 1950; i--)
        {
            lt = new ListItem();
            lt.Text = i.ToString();
            lt.Value = i.ToString();
            ddlYear.Items.Add(lt);
        }
        ddlYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
    }
}