using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registered_1_2 : System.Web.UI.Page
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
                    //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                    Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                    string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                    //fuDocument.SaveAs(Server.MapPath("important_images") + "\\" + filename);
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

            HttpPostedFile postedFile = fuDocument1.PostedFile;

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
                    //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                    Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                    string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                    //fuDocument1.SaveAs(Server.MapPath("important_images") + "\\" + filename);
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
    protected void UploadDocument2(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument2.HasFile)
        {
            HttpPostedFile postedFile = fuDocument2.PostedFile;

            DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");

            SqlDataSource3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource3.SelectCommand = "select id,name from filename_extension";
            SqlDataSource3.DataBind();
            DataView ou1 = (DataView)SqlDataSource3.Select(DataSourceSelectArguments.Empty);
            for (int i = 0; i < ou1.Count; i++)
            {
                if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
                {
                    check = true;
                }
            }
            if (check)
            {
                int fileSize = fuDocument2.PostedFile.ContentLength;

                // Allow only files less than (16 MB)=16777216 bytes to be uploaded.
                if (fileSize < 16777216)
                {

                    SqlDataSource sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                    filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                    //AmazonUpload aws = new AmazonUpload();
                    //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                    Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                    string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                    //fuDocument.SaveAs(Server.MapPath("important_images") + "\\" + filename);
                    type2_Image.ImageUrl = imgurl;
                    Image10.Visible = true;
                    Image11.Visible = false;
                    //GridView1.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(fuDocument2, fuDocument2.GetType(), "alert", "alert('File is out of memory 16MB!')", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(fuDocument2, fuDocument2.GetType(), "alert", "alert('filename extension is not in role!')", true);
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
            bool check_db = false;
            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.SelectCommand = "select id from user_information_store";
            sql_f.SelectCommand += " where uid='" + uid + "';";
            sql_f.DataBind();
            DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
            if (ict_f.Count > 0)
            {
                check_db = true;
            }

            if (check_db)
            {
                string uisid = ict_f.Table.Rows[0]["id"].ToString();

                string bank_name = bank_name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_name_detail = bank_name_detail_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_number = bank_number_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string bank_person = bank_person_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                int bank_type = 0,bank_type_detail=0;


                bool check_type0_Image = false, check_type1_Image = false, check_type2_Image = false
                    , check_bank_type = false, check_bank_type_detail = false, check_bank_name = false, check_bank_name_detail = false
                    , check_bank_number = false, check_bank_person = false;
                if (type0_Image.ImageUrl.ToString() != "")
                {
                    check_type0_Image = true;
                    image_Label.Text = "";
                }
                else
                {
                    image_Label.Text = "This image not upload.";
                }
                if (type1_Image.ImageUrl.ToString() != "")
                {
                    check_type1_Image = true;
                    image_Label0.Text = "";
                }
                else
                {
                    image_Label0.Text = "This image not upload.";
                }
                if (type2_Image.ImageUrl.ToString() != "")
                {
                    check_type2_Image = true;
                    image_Label1.Text = "";
                }
                else
                {
                    image_Label1.Text = "This image not upload.";
                }

                //if (bank_type_RadioButtonList.SelectedIndex > -1)
                //{
                //    bank_type = bank_type_RadioButtonList.SelectedIndex;

                //    check_bank_type = true;
                //    bank_type_Label.Text = "";
                //}
                //else
                //{
                //    bank_type_Label.Text = "Bank type no select.";
                //}

                //if (bank_type_detail_RadioButtonList.SelectedIndex > -1)
                //{
                //    bank_type_detail = bank_type_detail_RadioButtonList.SelectedIndex;
                //    check_bank_type_detail = true;
                //    bank_type_detail_Label.Text = "";
                //}
                //else
                //{
                //    bank_type_detail_Label.Text = "Bank type detail no select.";
                //}

                //if (bank_name != "")
                //{
                //    check_bank_name = true;
                //    bank_name_Label.Text = "";
                //}
                //else
                //{
                //    bank_name_Label.Text = "Bank name have special word or not write.";
                //}
                //if (bank_name_detail != "")
                //{
                //    check_bank_name_detail = true;
                //    bank_name_detail_Label.Text = "";
                //}
                //else
                //{
                //    bank_name_detail_Label.Text = "Bank name detail have special word or not write.";
                //}
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
                //    bank_number_Label.Text = "Bank number have special word or not write.";
                //}
                //if (bank_person != "")
                //{
                //    check_bank_person = true;
                //    bank_person_Label.Text = "";
                //}
                //else
                //{
                //    bank_person_Label.Text = "Bank person have special word or not write.";
                //}


                //if (check_type0_Image && check_type1_Image && check_type2_Image && check_bank_person &&
                //    check_bank_number && check_bank_name_detail && check_bank_name && check_bank_type_detail &&
                //    check_bank_type)
                if (check_type0_Image && check_type1_Image && check_type2_Image)
                {

                    //SqlDataSource sql_update = new SqlDataSource();
                    //sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //sql_update.UpdateCommand = "update user_information_store";
                    //sql_update.UpdateCommand += " set bank_type='" + bank_type + "',bank_type_detail='" + bank_type_detail + "',bank_name='" + bank_name + "'";
                    //sql_update.UpdateCommand += ",bank_name_detail='" + bank_name_detail + "',bank_number='" + bank_number + "',bank_person='" + bank_person + "'";
                    //sql_update.UpdateCommand += " where id='" + uisid + "';";
                    //sql_update.Update();


                    sql_f = new SqlDataSource();
                    sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f.SelectCommand = "select id from user_information_store_important_image";
                    sql_f.SelectCommand += " where uisid='" + uisid + "';";
                    sql_f.DataBind();
                    ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                    if (ict_f.Count == 0)
                    {
                        SqlDataSource sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_important_image(uisid,type,filename)";
                        sql_insert.InsertCommand += " values('" + uisid + "','0','" + type0_Image.ImageUrl.ToString() + "');";
                        sql_insert.Insert();

                        sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_important_image(uisid,type,filename)";
                        sql_insert.InsertCommand += " values('" + uisid + "','1','" + type1_Image.ImageUrl.ToString() + "');";
                        sql_insert.Insert();

                        sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_important_image(uisid,type,filename)";
                        sql_insert.InsertCommand += " values('" + uisid + "','2','" + type2_Image.ImageUrl.ToString() + "');";
                        sql_insert.Insert();



                        bank_name_TextBox.Text="";
                        bank_name_detail_TextBox.Text="";
                        bank_number_TextBox.Text="";
                        bank_person_TextBox.Text="";

                        type0_Image.ImageUrl = "";
                        type1_Image.ImageUrl = "";
                        type2_Image.ImageUrl = "";

                        bank_type_detail_RadioButtonList.SelectedIndex = -1;
                        bank_type_RadioButtonList.SelectedIndex = -1;



                        result_Label.Text = "Success registered.";
                        Response.Redirect("registered_1_3.aspx");

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
}
