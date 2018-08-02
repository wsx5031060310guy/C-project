using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public static class FileInfoExtensions
{
    /// <summary>
    /// Template for a file item in multipart/form-data format.
    /// </summary>
    public const string HeaderTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";

    /// <summary>
    /// Writes a file to a stream in multipart/form-data format.
    /// </summary>
    /// <param name="file">The file that should be written.</param>
    /// <param name="stream">The stream to which the file should be written.</param>
    /// <param name="mimeBoundary">The MIME multipart form boundary string.</param>
    /// <param name="mimeType">The MIME type of the file.</param>
    /// <param name="formKey">The name of the form parameter corresponding to the file upload.</param>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown if any parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// Thrown if <paramref name="mimeBoundary" />, <paramref name="mimeType" />,
    /// or <paramref name="formKey" /> is empty.
    /// </exception>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Thrown if <paramref name="file" /> does not exist.
    /// </exception>
    public static void WriteMultipartFormData(
      this FileInfo file,
      Stream stream,
      string mimeBoundary,
      string mimeType,
      string formKey)
    {
        if (file == null)
        {
            throw new ArgumentNullException("file");
        }
        if (!file.Exists)
        {
            throw new FileNotFoundException("Unable to find file to write to stream.", file.FullName);
        }
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }
        if (mimeBoundary == null)
        {
            throw new ArgumentNullException("mimeBoundary");
        }
        if (mimeBoundary.Length == 0)
        {
            throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
        }
        if (mimeType == null)
        {
            throw new ArgumentNullException("mimeType");
        }
        if (mimeType.Length == 0)
        {
            throw new ArgumentException("MIME type may not be empty.", "mimeType");
        }
        if (formKey == null)
        {
            throw new ArgumentNullException("formKey");
        }
        if (formKey.Length == 0)
        {
            throw new ArgumentException("Form key may not be empty.", "formKey");
        }
        string header = String.Format(HeaderTemplate, mimeBoundary, formKey, file.Name, mimeType);
        byte[] headerbytes = Encoding.UTF8.GetBytes(header);
        stream.Write(headerbytes, 0, headerbytes.Length);
        using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
        }
        byte[] newlineBytes = Encoding.UTF8.GetBytes("\r\n");
        stream.Write(newlineBytes, 0, newlineBytes.Length);
    }
}
public static class DictionaryExtensions
{
    /// <summary>
    /// Template for a multipart/form-data item.
    /// </summary>
    public const string FormDataTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n";

    /// <summary>
    /// Writes a dictionary to a stream as a multipart/form-data set.
    /// </summary>
    /// <param name="dictionary">The dictionary of form values to write to the stream.</param>
    /// <param name="stream">The stream to which the form data should be written.</param>
    /// <param name="mimeBoundary">The MIME multipart form boundary string.</param>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown if <paramref name="stream" /> or <paramref name="mimeBoundary" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// Thrown if <paramref name="mimeBoundary" /> is empty.
    /// </exception>
    /// <remarks>
    /// If <paramref name="dictionary" /> is <see langword="null" /> or empty,
    /// nothing wil be written to the stream.
    /// </remarks>
    public static void WriteMultipartFormData(
      this Dictionary<string, string> dictionary,
      Stream stream,
      string mimeBoundary)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            return;
        }
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }
        if (mimeBoundary == null)
        {
            throw new ArgumentNullException("mimeBoundary");
        }
        if (mimeBoundary.Length == 0)
        {
            throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
        }
        foreach (string key in dictionary.Keys)
        {
            string item = String.Format(FormDataTemplate, mimeBoundary, key, dictionary[key]);
            byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(item);
            stream.Write(itemBytes, 0, itemBytes.Length);
        }
    }
}
public partial class get_info_from_gnavi : System.Web.UI.Page
{
    public static FileInfo ToFileFromUri(System.Uri url)
    {
        if (url.Scheme.Equals("file") == false)
        {
            return null;
        }
        else
        {
            String filename = url.PathAndQuery.Replace('/', Path.DirectorySeparatorChar);
            return new FileInfo(filename);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["update_day"] = DateTime.Now.ToString() ;
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
            //very first load//
            Session["count_sec"] = "1";
            //main
            int total_count = 0;
            string oauthUrl1 = string.Format("http://gws.gnavi.co.jp/gws/RestSearchAPI/ver2.0/?keyid=key&pref=PREF13&format=json");
            string results1 = String.Empty;
            System.Net.HttpWebRequest request1 = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(oauthUrl1);
            using (var response1 = request1.GetResponse())
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response1.GetResponseStream()))
            {
                results1 = sr.ReadToEnd();
            }
            if (results1 != "")
            {
                Newtonsoft.Json.Linq.JObject jArray1 = Newtonsoft.Json.Linq.JObject.Parse(results1);
                total_count = (int)jArray1["total_hit_count"];
            }
            //Facebook.FacebookClient myfacebook = new Facebook.FacebookClient();
            int count = 20;
            int dev = (total_count / count) + 1;
            Session["count_max"] = dev.ToString();
        }
        if (Session["seak"] != null)
        {
            if (Session["seak"].ToString() == "true")
            {


            }
        }

    }
    private static string CreateFormDataBoundary()
    {
        return "---------------------------" + DateTime.Now.Ticks.ToString("x");
    }
    public string ExecutePostRequest(
  Uri url,
  Dictionary<string, string> postData,
  FileInfo[] fileToUpload,
  string fileMimeType
)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.AbsoluteUri);
        request.Method = "POST";
        request.KeepAlive = true;
        string boundary = CreateFormDataBoundary();
        request.ContentType = "multipart/form-data; boundary=" + boundary;
        Stream requestStream = request.GetRequestStream();
        postData.WriteMultipartFormData(requestStream, boundary);
        int cou=0;
        if (fileToUpload != null)
        {
            foreach (FileInfo fo in fileToUpload)
            {
                fo.WriteMultipartFormData(requestStream, boundary, fileMimeType, "data[ReviewImage][" + cou + "][file]");
                cou += 1;
            }
        }
        byte[] endBytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "--");
        requestStream.Write(endBytes, 0, endBytes.Length);
        requestStream.Close();
        using (WebResponse response = request.GetResponse())
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            return reader.ReadToEnd();
        };
    }


    public class Posts
    {
        public string id = "";
        public string update_time="";
        public string name = "";
        public string lat = "";
        public string lng = "";
        public string address = "";
        public string tel = "";
        public string pr_short = "";
        public string image = "";
        public string url = "";

        public string open_time = "";
        public string holiday = "";

    }
    private List<Posts> get_gnavi_Posts(int index)
    {
        //Facebook.FacebookClient myfacebook = new Facebook.FacebookClient();
        int count = 20;
        int start_ind = index;
        List<Posts> postsList = new List<Posts>();
        //for (int i = 0; i < dev; i++)
        //{

            var client = new WebClient();

        //main
            string oauthUrl = string.Format("http://gws.gnavi.co.jp/gws/RestSearchAPI/ver2.0/?keyid=id&pref=PREF13&format=json&hit_per_page={0}&offset_page={1}", count, start_ind);
            //test
        //string oauthUrl = string.Format("http://gws.gnavi.co.jp/gws/RestSearchAPI/ver2.0/?keyid=api-alltrim-inc-keymaster&pref=PREF13&format=json&id=gdxt706");

            string results = String.Empty;
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(oauthUrl);
            using (var response = request.GetResponse())
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                results = sr.ReadToEnd();
            }
            Posts posts = new Posts();
            if (results != "")
            {
                Newtonsoft.Json.Linq.JObject jArray = Newtonsoft.Json.Linq.JObject.Parse(results);
                try
                {
                    foreach (var item in jArray["rest"])
                    {
                        foreach (var item_1 in item["equipments"]["equipment"])
                        {

                            if ((string)item_1["name"] == "あり")
                            {
                                int coun = 0;
                                //if((string)item["equipments"]["equipment"];)
                                posts = new Posts();
                                posts.id = (string)item["id"];
                                posts.name = (string)item["name"];
                                posts.update_time = (string)item["update_date"];
                                posts.lat = (string)item["latitude"];
                                posts.lng = (string)item["longitude"];
                                posts.address = (string)item["address"];
                                posts.tel = (string)item["tel"];
                                posts.pr_short = (string)item["pr_short"];
                                posts.image = (string)item["shop_view_image"];
                                posts.url = (string)item["url_mobile"];

                                posts.holiday = (string)item["holiday"];
                                string useable = "";
                                try
                                {
                                    foreach (var item_2 in item["opentimes"]["opentime"])
                                    {
                                        if (coun > 0)
                                        {
                                            useable += " / ";
                                        }
                                        useable += (string)item_2["time"];
                                        coun += 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    useable += item["opentimes"]["opentime"]["time"];

                                }
                                posts.open_time = useable;
                                postsList.Add(posts);
                            }


                        }

                    }
                }
                catch (Exception ex)
                {

                    foreach (var item_1 in jArray["rest"]["equipments"]["equipment"])
                    {

                        if ((string)item_1["name"] == "あり")
                        {
                            int coun = 0;
                            //if((string)item["equipments"]["equipment"];)
                            posts = new Posts();
                            posts.id = (string)jArray["rest"]["id"];
                            posts.name = (string)jArray["rest"]["name"];
                            posts.update_time = (string)jArray["rest"]["update_date"];
                            posts.lat = (string)jArray["rest"]["latitude"];
                            posts.lng = (string)jArray["rest"]["longitude"];
                            posts.address = (string)jArray["rest"]["address"];
                            posts.tel = (string)jArray["rest"]["tel"];
                            posts.pr_short = (string)jArray["rest"]["pr_short"];
                            posts.image = (string)jArray["rest"]["shop_view_image"];
                            posts.url = (string)jArray["rest"]["url_mobile"];

                            posts.holiday = (string)jArray["rest"]["holiday"];
                            string useable = "";
                            try
                            {
                                foreach (var item_2 in jArray["rest"]["opentimes"]["opentime"])
                                {
                                    if (coun > 0)
                                    {
                                        useable += " / ";
                                    }
                                    useable += (string)item_2["time"];
                                    coun += 1;
                                }
                            }
                            catch (Exception exx)
                            {
                                useable += jArray["rest"]["opentimes"]["opentime"]["time"];

                            }
                            posts.open_time = useable;
                            postsList.Add(posts);
                        }




                    }

                }

            }
        //}


        return postsList;

    }
    int count = 0;
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        gc = new GCP_MYSQL();
        Label3.Text = DateTime.Now.ToString();
        //if (DateTime.Now.Minute % 10 == 0)
        //{
        //    Response.Redirect("twitter_gov.aspx");
        //}
        //Session["seak"] = "false";
        //if (DateTime.Now.Minute % 59 == 0)
        //{
        Label4.Text = Session["count_sec"].ToString();
        if (DateTime.Now.Second %10== 0)
        {
            if (Session["count_max"] != null && Session["count_sec"] != null && Session["update_day"] != null)
            {

                int total_count = Convert.ToInt32(Session["count_max"].ToString());
                int cou = Convert.ToInt32(Session["count_sec"].ToString());
                cou += 1;

                if (cou > total_count)
                {

                    cou = 1;


                }
                Session["count_sec"] = cou.ToString();

                List<Posts> posts = new List<Posts>();

                posts = get_gnavi_Posts(cou);

                double lat, lng;
                Dictionary<string, string> kvp;
                for (int i = 0; i < posts.Count; i++)
                {
                    ListBox1.Items.Add(posts[i].id);

                    lat = System.Math.Round(Convert.ToDouble(posts[i].lat), 6, MidpointRounding.AwayFromZero);
                    lng = System.Math.Round(Convert.ToDouble(posts[i].lng), 6, MidpointRounding.AwayFromZero);


                    Query = "select id,_id from gnavi_update";
                    Query += " where gnavi_id='" + posts[i].id + "' and name='" + posts[i].name + "' and lat='" + lat.ToString() + "' and lng='" + lng.ToString() + "'";
                    //sql_f.SelectCommand += " and address=N'"+posts[i].address+"' and tel=N'"+posts[i].tel+"' and pr_short=N'"+posts[i].pr_short+"' and image=N'"+posts[i].image+"'";
                    Query += " and address='" + posts[i].address + "' and tel='" + posts[i].tel + "' and pr_short='" + posts[i].pr_short + "'";
                    Query += " and url='" + posts[i].url + "' and open_time='" + posts[i].open_time + "' and holiday='" + posts[i].holiday + "' and update_time='" + posts[i].update_time + "';";
                    DataView ict_f = gc.select_cmd(Query);
                    if (ict_f.Count == 0)
                    {

                        Query = "select id,_id from gnavi_update";
                        Query += " where gnavi_id='" + posts[i].id + "';";
                        DataView ict_ff = gc.select_cmd(Query);
                        if (ict_ff.Count > 0)
                        {
                            string baby_id = ict_ff.Table.Rows[0]["_id"].ToString();




                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                                try
                                {
                                    wc.Encoding = Encoding.UTF8;

                                    NameValueCollection nc = new NameValueCollection();
                                    nc["Place[id]"] = baby_id;
                                    nc["Place[name]"] = posts[i].name;
                                    nc["Place[lat]"] = lat.ToString();
                                    nc["Place[lon]"] = lng.ToString();
                                    nc["Review[message]"] ="";
                                    nc["Place[remarks]"] = posts[i].pr_short;
                                    nc["Place[milk_seat]"] = "1";
                                    nc["Place[url]"] = posts[i].url;
                                    nc["Place[tel]"] = posts[i].tel;
                                    nc["Place[address]"] = posts[i].address;
                                    nc["Place[usable_week_day]"] = posts[i].holiday;

                                    byte[] bResult = wc.UploadValues("api" + baby_id, nc);

                                    string resultXML = Encoding.UTF8.GetString(bResult);
                                    Label2.Text = resultXML;
                                }
                                catch (WebException ex)
                                {
                                    Label5.Text = "error!";

                                }
                            }


                            Query = "update gnavi_update set name='" + posts[i].name + "',lat='" + lat.ToString() + "',lng='" + lng.ToString() + "',address='" + posts[i].address + "'";
                            ////can not edit image
                            //sql_f_up.UpdateCommand += ",tel=N'" + posts[i].tel + "',pr_short=N'" + posts[i].pr_short + "',image=N'" + posts[i].image + "',url=N'" + posts[i].url + "'";
                            Query += ",tel='" + posts[i].tel + "',pr_short='" + posts[i].pr_short + "',url='" + posts[i].url + "'";
                            Query += ",open_time='" + posts[i].open_time + "',holiday='" + posts[i].holiday + "',update_time='" + posts[i].update_time + "',_update_time=NOW()";
                            Query += " where id='" + ict_ff.Table.Rows[0]["id"].ToString() + "';";
                            resin = gc.update_cmd(Query);


                            ListBox2.Items.Add(baby_id);

                        }
                        else
                        {
                            //add new nursing room to  DB
                            kvp = new Dictionary<string, string>();
                            kvp.Add("Place[lat]", lat.ToString());
                            kvp.Add("Place[lon]", lng.ToString());
                            kvp.Add("Place[name]", posts[i].name);
                            kvp.Add("Review[star]", "4");
                            kvp.Add("Review[message]", "");
                            kvp.Add("Place[remarks]", posts[i].pr_short);
                            kvp.Add("Place[milk_seat]", "1");
                            kvp.Add("Place[place_category_id]", "1");

                            kvp.Add("Place[is_official]", "1");

                            kvp.Add("Place[url]", posts[i].url);
                            kvp.Add("Place[tel]", posts[i].tel);
                            kvp.Add("Place[address]", posts[i].address);
                            kvp.Add("Place[usable_week_day]", posts[i].holiday);
                            kvp.Add("Place[usable_time]", posts[i].open_time);
                            string temp_name = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(posts[i].image);
                            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                            if (webresponse.StatusCode == HttpStatusCode.OK)
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(webresponse.GetResponseStream());
                                image.Save(Server.MapPath("~") + @"/images/" + temp_name + ".jpg"); //保存在本地文件夹
                                image.Dispose(); //
                            }

                            FileInfo[] file_list = new FileInfo[1];
                            file_list[0] = new FileInfo(Server.MapPath("~") + @"/images/" + temp_name + ".jpg");

                            string res = ExecutePostRequest(new Uri("api"),
                                kvp,
                    file_list,
                    "image/jpeg");
                            int babyid = Convert.ToInt32(res.Replace("{", "").Replace("}", "").Replace("result", "").Replace(":", "").Replace("\"", "").Trim());
                            Label1.Text = babyid.ToString();
                            if ((System.IO.File.Exists(Server.MapPath("~") + @"/images/" + temp_name + ".jpg")))
                            {
                                System.IO.File.Delete(Server.MapPath("~") + @"/images/" + temp_name + ".jpg");
                            }


                            Query = "insert into gnavi_update(gnavi_id,_id,name,lat,lng,address,tel,pr_short,image,url,open_time,holiday,update_time,_update_time)";
                            Query += " values('" + posts[i].id + "','" + babyid.ToString() + "','" + posts[i].name + "','" + lat.ToString() + "','" + lng.ToString() + "','" + posts[i].address + "','" + posts[i].tel + "'";
                            Query += ",'" + posts[i].pr_short + "','" + posts[i].image + "','" + posts[i].url + "','" + posts[i].open_time + "','" + posts[i].holiday + "','" + posts[i].update_time + "',NOW());";
                            resin = gc.insert_cmd(Query);

                            ListBox2.Items.Add(babyid.ToString());
                        }
                    }

                }

            }
            else
            {
                Response.Redirect("get_info_from_gnavi.aspx");
            }
        }
        //}
    }
}
