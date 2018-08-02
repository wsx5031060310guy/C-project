h<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Web;

public class FileUploadHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        try
        {
            if (context.Request.QueryString["upload"] != null)
            {
                string pathrefer = context.Request.UrlReferrer.ToString();

                var postedFile = context.Request.Files[0];

                string file;

                //For IE to get file name
                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                {
                    string[] files = postedFile.FileName.Split(new char[] { '\\' });
                    file = files[files.Length - 1];
                }
                else
                {
                    file = postedFile.FileName;
                }

                //if (!System.IO.Directory.Exists(Serverpath))
                //    System.IO.Directory.CreateDirectory(Serverpath);

                if (context.Request.QueryString["fileName"] != null)
                {
                    file = context.Request.QueryString["fileName"];
                }

                string ext = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");
                file = Guid.NewGuid() + ext;

                Google.Apis.Auth.OAuth2.GoogleCredential credential=GCP_AUTH.AuthExplicit();

                string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", file, postedFile.InputStream, credential);

                //AmazonUpload aws = new AmazonUpload();
                //string imgurl = aws.AmazonUpload_file("", "upload/test", file, postedFile.InputStream);

                //fileDirectory = Serverpath + "\\" + file;

                //postedFile.SaveAs(fileDirectory);

                context.Response.AddHeader("Vary", "Accept");
                try
                {
                    if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                        context.Response.ContentType = "application/json";
                    else
                        context.Response.ContentType = "text/plain";
                }
                catch
                {
                    context.Response.ContentType = "text/plain";
                }



                context.Response.Write(imgurl);
            }
        }
        catch (Exception exp)
        {
            context.Response.Write(exp.Message);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
