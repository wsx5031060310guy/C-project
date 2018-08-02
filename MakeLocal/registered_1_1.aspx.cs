using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class registered_1_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!IsPostBack)
        {

        }
    }
    List<string> impath = new List<string>();
    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument.HasFile)
        {
            Panel1.Attributes.Add("style", "display:block");
            impath = new List<string>();
            Panel1.Controls.Clear();
            foreach (HttpPostedFile postedFile in fuDocument.PostedFiles)
            {
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
                    int fileSize = postedFile.ContentLength;

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

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = imgurl;
                        impath.Add(@"~/" + imgurl);
                        this.Panel1.Controls.Add(im);

                        //Image1.ImageUrl = Server.MapPath("fileplace") + "\\" + filename;


                        //upload_files.Text += Server.MapPath("fileplace") + "\\" + filename + ",";
                        //upload_files0.Text += postedFile.FileName.ToString() + ",";
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




            ViewState["myData"] = impath;
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


                string title = title_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string myself_content = myself_content_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");

                SqlDataSource sql_update = new SqlDataSource();
                sql_update.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_update.UpdateCommand = "update user_information_store";
                sql_update.UpdateCommand += " set title=N'" + title + "',myself_content=N'" + myself_content + "'";
                sql_update.UpdateCommand += " where id='" + uisid + "';";
                sql_update.Update();


                sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from user_information_store_images";
                sql_f.SelectCommand += " where uisid='" + uisid + "';";
                sql_f.DataBind();
                ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count == 0)
                {
                    impath = (List<string>)ViewState["myData"];
                    //ScriptManager.RegisterStartupScript(Button2, Button2.GetType(), "alert", "alert('" + impath.Count + "')", true);
                    for (int i = 0; i < impath.Count; i++)
                    {
                        SqlDataSource sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into user_information_store_images(uisid,filename)";
                        sql_insert.InsertCommand += " values('" + uisid + "','" + impath[i] + "');";
                        sql_insert.Insert();
                    }

                    title_TextBox.Text = "";
                    myself_content_TextBox.Text = "";
                    Panel1.Controls.Clear();
                    impath.Clear();


                    result_Label.Text = "Success registered.";
                    Response.Redirect("registered_1_2.aspx");
                }

            }
            else
            {
                result_Label.Text = "登録に失敗しました。";
            }
        }
    }
}
