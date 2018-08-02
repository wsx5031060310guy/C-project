using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class upload : System.Web.UI.Page
{
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpFileCollection files = Request.Files;
        string msg = string.Empty;
        string error = string.Empty;
        string imgurl;
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        if (files.Count > 0)
        {
            //files[0].SaveAs(Server.MapPath("/") + System.IO.Path.GetFileName(files[0].FileName));

            msg = "File size:" + files[0].ContentLength;

            input = files[0].FileName;
            stringindex = input.LastIndexOf(@".");
            cut = input.Length - stringindex;
            DirRoot = input.Substring(stringindex + 1, cut - 1);

            // Allow only files less than (16 MB)=16777216 bytes to be uploaded.

            filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

            Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
            imgurl = GCP_AUTH.upload_file_stream("", "upload/nursing_room_customer", filename, files[0].InputStream, credential);


            string res =imgurl ;
            Response.Write(res);
            Response.End();


        }


    }
}
