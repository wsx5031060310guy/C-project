using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mamaro_select : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    string room_id = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        //test
        //Session["manager_page"] = "2";

        if (Session["manager_page"] != null)
        {
            if (Session["manager_page"].ToString().Trim() != "")
            {
                room_id = Session["manager_page"].ToString().Trim();

                //select info from mamaro DB
                GCP_MYSQL gc = new GCP_MYSQL();
                Literal lip = new Literal();
                Literal lip2 = new Literal();
                //lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";

                lip2.Text += "<div id='message_div'>";


                lip.Text += "<div id='select_div'>";


                string Query = "";
                if (room_id == "2")
                {
                    Query = "select * from nursing_room;";
                }
                else
                {
                    Query = "select c.id,c.name,c.,c.QRcode,c.close from nursing_room_manager as a ";
                    Query += "inner join nursing_room_connect_manager as b on a.id=b.nursing_room_manager_id ";
                    Query += "inner join nursing_room as c on b.nursing_room_id=c.id and a.id='" + room_id + "'; ";
                }
                DataView ict_ff = gc.select_cmd(Query);
                if (ict_ff.Count > 0)
                {
                    for (int i = 0; i < ict_ff.Count; i++)
                    {
                        if (ict_ff.Table.Rows[i]["close"].ToString() == "0")
                        {

                            //for babymap
                            //get info from babymap DB
                            var url_five = new Uri("http:///api/places/get_official_info?id=" + ict_ff.Table.Rows[i][""].ToString());

                            string result_five = "";
                            System.Net.HttpWebRequest request_five = (HttpWebRequest)HttpWebRequest.Create(url_five);
                            using (var response = request_five.GetResponse())
                            using (StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                result_five = sr.ReadToEnd();
                            }
                            request_five = null;

                            Newtonsoft.Json.Linq.JObject jArray_five = JObject.Parse(result_five);

                            string tel = (string)jArray_five["result"]["Place"]["tel"];
                            string url = (string)jArray_five["result"]["Place"]["url"];
                            string usable_week_day = (string)jArray_five["result"]["Place"]["usable_week_day"];
                            string remarks = (string)jArray_five["result"]["Place"]["remarks"];

                            lip.Text += "<br><div class='center' id=" + '"' + "m_" + i + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"<br><br>
<div id=" + '"' + "write_" + i + '"' + @">
<p>営業時間:</p>
<p>
            <textarea style='width: 100%;height:100px;' id =" + '"' + "time_" + i + '"' + @"
                  rows = '3'
                  cols = '80'>" + usable_week_day + @"</textarea>
        </p>
<p>サイト:</p>
<p>
            <textarea style='width: 100%;height:100px;' id =" + '"' + "web_" + i + '"' + @"
                  rows = '3'
                  cols = '80'>" + url + @"</textarea>
        </p>
<p>電話番号:</p>
<p>
            <textarea style='width: 100%;height:100px;' id =" + '"' + "phone_" + i + '"' + @"
                  rows = '3'
                  cols = '80'>" + tel + @"</textarea>
        </p>
<p>備考:</p>
<p>
            <textarea style='width: 100%;height:300px;' id =" + '"' + "txt_" + i + '"' + @"
                  rows = '3'
                  cols = '80'>" + remarks + @"</textarea>
        </p>

<div id=" + '"' + "but_" + i + '"' + @">submit</div>
</div>
                                        </div><br>";


                            lip.Text += @"<script type=" + '"' + "text/javascript" + '"' + @">
                                        $(function(){




   $(" + '"' + @"#but_" + i + '"' + @").click(function () {
                 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_select.aspx/send_message" + '"' + @",
                     data: " + '"' + @"{param1: '" + '"' + @" + $('#txt_" + i + @"').val() + " + '"' + @"' ,param2: '" + ict_ff.Table.Rows[i]["QRcode"].ToString() + @"' ,param3: '" + '"' + @" + $('#phone_" + i + @"').val() + " + '"' + @"' ,param4: '" + '"' + @" + $('#web_" + i + @"').val() + " + '"' + @"' ,param5: '" + '"' + @" + $('#time_" + i + @"').val() + " + '"' + @"'  }" + '"' + @",
                     contentType: " + '"' + @"application/json; charset=utf-8" + '"' + @",
                     dataType: " + '"' + @"json" + '"' + @",
                     async: true,
                     cache: false,
                     success: function (result) {
                         alert(result.d);

                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });

             });




});


                                    </script>";

                            //for babymap end

                            //for mamaro report
                            lip2.Text += "<br><div class='center' id=" + '"' + "m_" + i + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"<br>";

                            lip2.Text += @"<div id=" + '"' + "write_" + i + '"' + @"> <hr>";
                            GCP_MYSQL gc1 = new GCP_MYSQL();
                            string Query1 = "select * from nursing_room_message_check where nursing_room_id='" + ict_ff.Table.Rows[i]["id"].ToString() + "';";
                            DataView ict_ff1 = gc1.select_cmd(Query1);
                            if (ict_ff1.Count > 0)
                            {
                                for (int ii = 0; ii < ict_ff1.Count; ii++)
                                {
                                    DateTime dbtime = Convert.ToDateTime(ict_ff1.Table.Rows[ii]["update_time"].ToString());
                                    lip2.Text += @"<p>" + dbtime.ToString("yyyy/MM/dd HH:mm:ss") + @"</p>
<p>" + ict_ff1.Table.Rows[ii]["message"].ToString() + @"</p>
<p>";
                                    if (ict_ff1.Table.Rows[ii]["pdf_url"].ToString() != "")
                                    {
                                        lip2.Text += @"
<a href='" + ict_ff1.Table.Rows[ii]["pdf_url"].ToString() + @"' target='_blank'><img src='./images/mamaro_manager/pdf-icon.png' title='PDF' width='100' height='100' /></a>";
                                    }
                                    if (ict_ff1.Table.Rows[ii]["csv_url"].ToString() != "")
                                    {
                                        lip2.Text += @"
<a href='" + ict_ff1.Table.Rows[ii]["csv_url"].ToString() + @"' target='_blank'><img src='./images/mamaro_manager/Files-Csv-icon.png' title='CSV' width='100' height='100' /></a>";
                                    }
                                    lip2.Text += @"</p> <hr>
";
                                }
                            }
                            lip2.Text += @"
<div id='newcon" + i + @"'></div>
</div>";
                            if (room_id == "2")
                            {
                                lip2.Text += "<div id=" + '"' + "edit_" + i + '"' + @"><hr>
<p>Content:</p>
<p>
            <textarea style='width: 100%;height:300px;' id =" + '"' + "mess_" + i + '"' + @"
                  rows = '3'
                  cols = '80'></textarea>
        </p>
<hr>
<p>PDF:</p>
<p><input type='file' id='file" + i + @"' name='file' /></p>
<img id=" + '"' + "butup_" + i + '"' + @" src='./images/mamaro_manager/upload-file.png' title='upload' width='100' height='100' />
    <div id='img" + i + @"' style='width:100%;word-break: break-all;'></div>
<hr>
 <script type='text/javascript'>
$(function () {
            $('#butup_" + i + @"').click(function () {
                ajaxFileUpload" + i + @"();
            })
        })
   function ajaxFileUpload" + i + @"() {
            $.ajaxFileUpload
            (
                {
                    url: '/upload.aspx',
                    secureuri: false,
                    fileElementId: 'file" + i + @"',
                    dataType: 'JSON',
                    success: function (data, status)
                    {
                        console.log(data);
                        $('#img" + i + @"').empty();
                        $('#img" + i + @"').append(data);


                    },
                    error: function (data, status, e)
                    {
                        alert(e);
                    }
                }
            )
            return false;
        }
    </script>
<p>CSV:</p>
<p><input type='file' id='file_" + i + @"' name='file' /></p>
<img id=" + '"' + "butup__" + i + '"' + @" src='./images/mamaro_manager/upload-file.png' title='upload' width='100' height='100' />

    <div id='imgg" + i + @"' style='width:100%;word-break: break-all;'></div>

<hr>
<div id=" + '"' + "butt_" + i + '"' + @">submit</div><hr>
 <script type='text/javascript'>
$(function () {
            $('#butup__" + i + @"').click(function () {
                ajaxFileUpload_" + i + @"();
            })
        })
   function ajaxFileUpload_" + i + @"() {
            $.ajaxFileUpload
            (
                {
                    url: '/upload.aspx',
                    secureuri: false,
                    fileElementId: 'file_" + i + @"',
                    dataType: 'JSON',
                    success: function (data, status)
                    {
                        console.log(data);
                        $('#imgg" + i + @"').empty();
                        $('#imgg" + i + @"').append(data);


                    },
                    error: function (data, status, e)
                    {
                        alert(e);
                    }
                }
            )
            return false;
        }
$(function(){



 $(" + '"' + @"#butt_" + i + '"' + @").click(function () {
                 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_select.aspx/send_message1" + '"' + @",
                     data: " + '"' + @"{param1: '" + '"' + @" + $('#mess_" + i + @"').val() + " + '"' + @"' ,param2: '" + ict_ff.Table.Rows[i]["QRcode"].ToString() + @"' ,param3: '" + '"' + @" + $('#img" + i + @"').text() + " + '"' + @"' ,param4: '" + '"' + @" + $('#imgg" + i + @"').text() + " + '"' + @"'  }" + '"' + @",
                     contentType: " + '"' + @"application/json; charset=utf-8" + '"' + @",
                     dataType: " + '"' + @"json" + '"' + @",
                     async: true,
                     cache: false,
                     success: function (result) {
                        $('#mess_" + i + @"').val('');

                         $('#imgg" + i + @"').empty();
                         $('#img" + i + @"').empty();
                        if (result.d.length > 0){
                            alert(result.d[0]);
                            $('#newcon" + i + @"').append(result.d[1]);


                            }
                            else{
                              alert('error');
                            }

                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });

             });


});




    </script>
";



                                lip2.Text += "</div>";
                            }

                            lip2.Text += @"<script type='text/javascript'>
$(function(){

$show1 = $('#message_div');
$show1.hide();

$show2 = $('#select_div');
$show2.show();
$('#babymap_but').css('background-color','#191970');
$('#babymap_but').css('color','#FFFFFF');
$('#data_but').css('background-color','#FFFFFF');
$('#data_but').css('color','#191970');

$('#data_but').click(function () {
$('#data_but').css('background-color','#191970');
$('#data_but').css('color','#FFFFFF');

$('#babymap_but').css('background-color','#FFFFFF');
$('#babymap_but').css('color','#191970');

$show1 = $('#message_div');
$show1.show();

$show2 = $('#select_div');
$show2.hide();

});

$('#babymap_but').click(function () {
$('#babymap_but').css('background-color','#191970');
$('#babymap_but').css('color','#FFFFFF');

$('#data_but').css('background-color','#FFFFFF');
$('#data_but').css('color','#191970');

$show1 = $('#select_div');
$show1.show();

$show2 = $('#message_div');
$show2.hide();

});
});
    </script>
";


                            lip2.Text += @"</div><br>";






                            //for mamaro report end



                        }


                    }

                }

                lip.Text += "</div>";

                lip2.Text += "</div>";

                main_Panel.Controls.Add(lip);
                main_Panel.Controls.Add(lip2);
            }
            else
            {
                Session.Clear();
                Response.Redirect("mamaro_manager.aspx");
            }
        }
        else
        {
            Session.Clear();
            Response.Redirect("mamaro_manager.aspx");
        }
    }
    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string send_message(string param1, string param2,string param3,string param4,string param5)
    {
        //string result = param1 + "," + param2;
        string result = "";
        string QR = RemoveSpecialCharacters(param2);

        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "select * from nursing_room where QRcode='" + QR + "';";
                DataView ict_ff = gc.select_cmd(Query);
                if (ict_ff.Count > 0)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        try
                        {
                            wc.Encoding = Encoding.UTF8;

                            NameValueCollection nc = new NameValueCollection();
                            nc["id"] = ict_ff.Table.Rows[0][""].ToString().Trim();
                            nc["tel"] = param3;
                            nc["url"] = param4;
                            nc["usable_week_day"] = param5;
                            nc["remarks"] = param1;


                            byte[] bResult = wc.UploadValues("http:///api/places/edit_official_info", nc);

                            string resultXML = Encoding.UTF8.GetString(bResult);
                            if (resultXML != "" || resultXML != "null")
                            {
                                Newtonsoft.Json.Linq.JObject jArray_loc = Newtonsoft.Json.Linq.JObject.Parse(resultXML);
                                string res = jArray_loc["result"].ToString();
                                result=res;

                                jArray_loc = null;
                            }
                            else
                            {

                            }
                            nc = null;
                            bResult = null;
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        catch (WebException ex)
                        {

                        }
                    }
                }

        //try
        //{
        //    string username = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        //    string password = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        //    string usernames = RemoveSpecialCharacters(username);
        //    string passwords = RemoveSpecialCharacters(password);
        //    if (usernames != "" && passwords != "")
        //    {

        //        GCP_MYSQL gc = new GCP_MYSQL();
        //        string Query = "select id from nursing_room_manager where account='" + usernames + "';";
        //        DataView ict_ff = gc.select_cmd(Query);
        //        if (ict_ff.Count > 0)
        //        {
        //            string Query1 = "select id from nursing_room_manager where account='" + usernames + "' and password='" + passwords + "';";
        //            DataView ict_ff1 = gc.select_cmd(Query1);
        //            if (ict_ff1.Count > 0)
        //            {
        //                HttpContext.Current.Session["manager_page"] = ict_ff1.Table.Rows[0]["id"].ToString();
        //                //result = HttpContext.Current.Session["manager"].ToString();
        //                result = "ログインできました。";
        //            }
        //            else
        //            {
        //                result = "パスワードが間違っています。";
        //            }
        //        }
        //        else
        //        {
        //            result = "アカウントが間違っています。";
        //        }




        //    }



        //}
        //catch (Exception ex)
        //{
        //    result = "ログインできませんでした。";

        //    //return result;
        //    throw ex;
        //}
        return result;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static Object[] send_message1(string param1, string param2, string param3, string param4)
    {
        //string result = param1 + "," + param2;
        string result = "";
        string QR = RemoveSpecialCharacters(param2);
        Object[] nc = new Object[5];
        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "select * from nursing_room where QRcode='" + QR + "';";
        DataView ict_ff = gc.select_cmd(Query);
        if (ict_ff.Count > 0)
        {
            string Query1 = "insert into nursing_room_message_check(nursing_room_id,message,pdf_url,csv_url,update_time)";
            Query1 += " values('" + ict_ff.Table.Rows[0]["id"].ToString() + "','" + param1 + "','" + param3 + "','" + param4 + "',NOW());";
            GCP_MYSQL gc1 = new GCP_MYSQL();
            result = gc1.insert_cmd(Query1);

            DateTime nowdate = DateTime.Now;
            DateTime utc = nowdate.ToUniversalTime();
            TimeZoneInfo jst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime now_jst = TimeZoneInfo.ConvertTimeFromUtc(utc, jst);
            nc[0] = result;
            string html = @"<p>" + now_jst.ToString("yyyy/MM/dd HH:mm:ss") + @"</p><p>" + param1 + @"</p><p>";
            if(param3!="")
            {
                html+=@"
<a href='" + param3 + @"' target='_blank'><img src='./images/mamaro_manager/pdf-icon.png' title='PDF' width='100' height='100' /></a>";

            }
             if(param4!="")
            {
                 html+= @"
<a href='" + param4 + @"' target='_blank'><img src='./images/mamaro_manager/Files-Csv-icon.png' title='CSV' width='100' height='100' /></a>";
             }
            html+=@"</p><hr>";
            nc[1] = html;


            ////var url_five = new Uri("http:///api/places/get_official_info?id=" + ict_ff.Table.Rows[0][""].ToString());
            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //    try
            //    {
            //        wc.Encoding = Encoding.UTF8;

            //        NameValueCollection nc = new NameValueCollection();
            //        nc["id"] = ict_ff.Table.Rows[0][""].ToString().Trim();
            //        nc["tel"] = param3;
            //        nc["url"] = param4;
            //        nc["usable_week_day"] = param5;
            //        nc["remarks"] = param1;


            //        byte[] bResult = wc.UploadValues("http:///api/places/edit_official_info", nc);

            //        string resultXML = Encoding.UTF8.GetString(bResult);
            //        if (resultXML != "" || resultXML != "null")
            //        {
            //            Newtonsoft.Json.Linq.JObject jArray_loc = Newtonsoft.Json.Linq.JObject.Parse(resultXML);
            //            string res = jArray_loc["result"].ToString();
            //            result = res;

            //            jArray_loc = null;
            //        }
            //        else
            //        {

            //        }
            //        nc = null;
            //        bResult = null;
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //    }
            //    catch (WebException ex)
            //    {

            //    }
            //}
        }

        //try
        //{
        //    string username = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
        //    string password = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

        //    string usernames = RemoveSpecialCharacters(username);
        //    string passwords = RemoveSpecialCharacters(password);
        //    if (usernames != "" && passwords != "")
        //    {

        //        GCP_MYSQL gc = new GCP_MYSQL();
        //        string Query = "select id from nursing_room_manager where account='" + usernames + "';";
        //        DataView ict_ff = gc.select_cmd(Query);
        //        if (ict_ff.Count > 0)
        //        {
        //            string Query1 = "select id from nursing_room_manager where account='" + usernames + "' and password='" + passwords + "';";
        //            DataView ict_ff1 = gc.select_cmd(Query1);
        //            if (ict_ff1.Count > 0)
        //            {
        //                HttpContext.Current.Session["manager_page"] = ict_ff1.Table.Rows[0]["id"].ToString();
        //                //result = HttpContext.Current.Session["manager"].ToString();
        //                result = "ログインできました。";
        //            }
        //            else
        //            {
        //                result = "パスワードが間違っています。";
        //            }
        //        }
        //        else
        //        {
        //            result = "アカウントが間違っています。";
        //        }




        //    }



        //}
        //catch (Exception ex)
        //{
        //    result = "ログインできませんでした。";

        //    //return result;
        //    throw ex;
        //}
        return nc;

    }
}
