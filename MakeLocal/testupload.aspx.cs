using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
//aws
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using System.Data;
using Amazon.S3.Transfer;


public partial class testupload : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
    {
        string mimeType = "application/unknown";
        try
        {
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
        }
        catch (Exception ex)
        {

        }
        return mimeType;
    }
    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;


        if (fuDocument.HasFile)
        {
            foreach (HttpPostedFile postedFile in fuDocument.PostedFiles)
            {
                DirRoot = System.IO.Path.GetExtension(postedFile.FileName).ToUpper().Replace(".", "");


                Query = "select id,name from filename_extension";
                DataView ou1 = gc.select_cmd(Query);
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

                        filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;


                        //AmazonUpload aws = new AmazonUpload();
                        //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                        Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                        string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                        //AmazonUpload aws = new AmazonUpload();
                        //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = imgurl;
                        this.Panel1.Controls.Add(im);

                        image_HiddenField.Value += ",~/" + imgurl;



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






        }

    }
    //public bool UploadFileToS3(string uploadAsFileName, Stream ImageStream, S3CannedACL filePermission, S3StorageClass storageType, string toWhichBucketName, string s3ServiceUrl)
    //{
    //    string accessKey = "AKIAJ3M3PCXJGLTDFGJA";
    //    string secretAccessKey = "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";
    //    try
    //    {
    //        AmazonS3Config config = new AmazonS3Config();
    //        config.ServiceURL = s3ServiceUrl;
    //        IAmazonS3 client;
    //        using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretAccessKey, config))
    //        {
    //            PutObjectRequest request = new PutObjectRequest();
    //            request.Key="upload" + "/" + uploadAsFileName;
    //            request.InputStream=ImageStream;
    //            request.BucketName=toWhichBucketName;
    //            request.CannedACL = filePermission;
    //            request.StorageClass = storageType;

    //            client.PutObject(request);
    //        }
    //    }
    //    catch (AmazonS3Exception s3Exception)
    //    {
    //        ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('" + s3Exception.InnerException.ToString() + "')", true);
    //        //Console.WriteLine(s3Exception.Message, s3Exception.InnerException);
    //        return false;
    //        //Console.ReadKey();
    //    }
    //    //catch(Exception ex)
    //    //{
    //    //    ScriptManager.RegisterStartupScript(fuDocument, fuDocument.GetType(), "alert", "alert('" + ex .InnerException.ToString()+ "')", true);
    //    //    return false;
    //    //}
    //    return true;
    //}
    public bool UploadToAWS( string bucketName, string subDirectoryInBucket, string fileNameInS3, Stream ImageStream)
    {

        string accessKey = "AKIAJ3M3PCXJGLTDFGJA";
        string secretKey = "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";
        AmazonS3Config asConfig = new AmazonS3Config()
        {
            ServiceURL = "s3-us-west-2.amazonaws.com",
            RegionEndpoint = Amazon.RegionEndpoint.USWest2
        };
        IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, asConfig);
        TransferUtility utility = new TransferUtility(client);
        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

        if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
        {
            request.BucketName = bucketName; //no subdirectory just bucket name
        }
        else
        {   // subdirectory and bucket name
            request.BucketName = bucketName + @"/" + subDirectoryInBucket;
        }
        request.Key = fileNameInS3; //file name up in S3
        //request.FilePath = localFilePath; //local file name
        request.InputStream = ImageStream;
        request.Headers.CacheControl = "public";
        request.Headers.Expires = DateTime.Now.AddYears(3);
        request.Headers.ContentEncoding = "gzip";
        utility.Upload(request); //commensing the transfer

        return true;
    }
}
