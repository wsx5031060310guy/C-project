using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registered_2_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument.HasFile)
        {
            HttpPostedFile postedFile = fuDocument.PostedFile;

            DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");

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
                int fileSize = fuDocument.PostedFile.ContentLength;

                // Allow only files less than (16 MB)=16777216 bytes to be uploaded.
                if (fileSize < 16777216)
                {
                    SqlDataSource sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                    filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                    //AmazonUpload aws = new AmazonUpload();
                    //string imgurl = aws.AmazonUpload_file("makelocal", "upload/test", filename, postedFile.InputStream);

                    Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                    string imgurl = GCP_AUTH.upload_file_stream("makelocal", "upload/test", filename, postedFile.InputStream, credential);


                    //Label1.Text += fuDocument.FileName.ToString() + "  finish<br>";
                    //fuDocument.SaveAs(Server.MapPath("school_important_images") + "\\" + filename);
                    type0_Image.ImageUrl = imgurl;
                    Image4.Visible = true;
                    Image5.Visible = false;
                    //GridView1.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('File is out of memory 16MB!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('filename extension is not in role!')", true);
            }


        }
    }
    protected void UploadDocument1(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument1.HasFile)
        {
            HttpPostedFile postedFile = fuDocument.PostedFile;

            DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");

            SqlDataSource1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource1.SelectCommand = "select id,name from filename_extension";
            SqlDataSource1.DataBind();
            DataView ou1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ou1.Count; i++)
            {
                if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
                {
                    check = true;
                }
            }
            if (check)
            {
                int fileSize = fuDocument1.PostedFile.ContentLength;

                // Allow only files less than (16 MB)=16777216 bytes to be uploaded.
                if (fileSize < 16777216)
                {
                    SqlDataSource sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                    filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                    //AmazonUpload aws = new AmazonUpload();
                    //string imgurl = aws.AmazonUpload_file("makelocal", "upload/test", filename, postedFile.InputStream);

                    Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                    string imgurl = GCP_AUTH.upload_file_stream("makelocal", "upload/test", filename, postedFile.InputStream, credential);

                    //sql_insert.InsertCommand = "insert into filename_detail(filename,name)";
                    //sql_insert.InsertCommand += " values('~/fileplace/" + filename + "','" + fuDocument.FileName.ToString() + "')";
                    //sql_insert.Insert();

                    //Label1.Text += fuDocument.FileName.ToString() + "  finish<br>";
                    //fuDocument1.SaveAs(Server.MapPath("school_important_images") + "\\" + filename);
                    type1_Image.ImageUrl = imgurl;
                    Image6.Visible = true;
                    Image7.Visible = false;
                    //GridView1.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(fuDocument1, fuDocument1.GetType(), "alert", "alert('File is out of memory 16MB!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(fuDocument1, fuDocument1.GetType(), "alert", "alert('filename extension is not in role!')", true);
            }


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
            Session["id"] = uid;
            
                string bank_number = bank_number_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_person = bank_person_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_second_number = bank_second_number_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_year = ddlYear.SelectedValue.ToString();
                string bank_month = ddlMonth.SelectedValue.ToString();



                bool check_type0_Image = false, check_type1_Image = false , check_bank_number = false, check_bank_person = false,check_bank_second_number=false;
                if (type0_Image.ImageUrl.ToString() != "")
                {
                    check_type0_Image = true;
                    image_Label.Text = "";
                }
                else
                {
                    image_Label.Text = "未入力";
                }
                if (type1_Image.ImageUrl.ToString() != "")
                {
                    check_type1_Image = true;
                    image_Label0.Text = "";
                }
                else
                {
                    image_Label0.Text = "未入力";
                }



                //if (bank_number != "")
                //{
                //    try
                //    {
                //        int number = Convert.ToInt32(bank_number);
                //        check_bank_number = true;
                //        bank_number_Label.Text = "";
                //    }
                //    catch (Exception ex)
                //    {
                //        bank_number_Label.Text = "Bank number is not number.";
                //        return;
                //        throw ex;
                //    }
                //}
                //else
                //{
                //    bank_number_Label.Text = "未入力";
                //}

                //if (bank_second_number != "")
                //{
                //    try
                //    {
                //        int number = Convert.ToInt32(bank_second_number);
                //        check_bank_second_number = true;
                //        bank_second_number_Label.Text = "";
                //    }
                //    catch (Exception ex)
                //    {
                //        bank_second_number_Label.Text = "Bank number is not number.";
                //        return;
                //        throw ex;
                //    }
                //}
                //else
                //{
                //    bank_second_number_Label.Text = "未入力";
                //}
                //if (bank_person != "")
                //{
                //    check_bank_person = true;
                //    bank_person_Label.Text = "";
                //}
                //else
                //{
                //    bank_person_Label.Text = "未入力";
                //}


                //if (check_type0_Image && check_type1_Image && check_bank_person && check_bank_number && check_bank_second_number)
                if (check_type0_Image && check_type1_Image)
                {
                    bool check_db = true;
                    SqlDataSource sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select uid from user_information_school";
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
                        sql_insert.InsertCommand = "insert into user_information_school(uid,bank_number,bank_year,bank_month,bank_second_number,bank_person)";
                        sql_insert.InsertCommand += " values('" + uid + "','" + bank_number + "','" + bank_year + "','" + bank_month + "'";
                        sql_insert.InsertCommand += ",'" + bank_second_number + "','" + bank_person + "');";
                        sql_insert.Insert();

                        Session["id"] = uid;

                        sql_f = new SqlDataSource();
                        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_f.SelectCommand = "select id from user_information_school";
                        sql_f.SelectCommand += " where uid='" + uid + "';";
                        sql_f.DataBind();
                        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);

                        string uischid = ict_f.Table.Rows[0]["id"].ToString();

                        sql_f = new SqlDataSource();
                        sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_f.SelectCommand = "select id from user_information_school_important_image";
                        sql_f.SelectCommand += " where uischid='" + uischid + "';";
                        sql_f.DataBind();
                        ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                        if (ict_f.Count == 0)
                        {
                            sql_insert = new SqlDataSource();
                            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_insert.InsertCommand = "insert into user_information_school_important_image(uischid,type,filename)";
                            sql_insert.InsertCommand += " values('" + uischid + "','0','" + type0_Image.ImageUrl.ToString() + "');";
                            sql_insert.Insert();

                            sql_insert = new SqlDataSource();
                            sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                            sql_insert.InsertCommand = "insert into user_information_school_important_image(uischid,type,filename)";
                            sql_insert.InsertCommand += " values('" + uischid + "','1','" + type1_Image.ImageUrl.ToString() + "');";
                            sql_insert.Insert();

                            bank_number_TextBox.Text = "";
                            bank_person_TextBox.Text = "";
                            bank_second_number_TextBox.Text = "";


                            type0_Image.ImageUrl = "";
                            type1_Image.ImageUrl = "";



                            result_Label.Text = "Success registered.";
                            Response.Redirect("registered_2_2.aspx");

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
                else
                {
                    result_Label.Text = "登録に失敗しました。";
                }

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
        for (int i = DateTime.Now.Year+100; i >= DateTime.Now.Year; i--)
        {
            lt = new ListItem();
            lt.Text = i.ToString();
            lt.Value = i.ToString();
            ddlYear.Items.Add(lt);
        }
        ddlYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
    }
}