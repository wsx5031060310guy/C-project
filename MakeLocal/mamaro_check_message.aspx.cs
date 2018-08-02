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

public partial class mamaro_check_message : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["manager_page"] != null)
        //{
        //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
        //    if (Session["manager_page"].ToString().Trim() != "")
        //    {
        //        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["manager"].ToString().Trim() + "')", true);
        //        try
        //        {
        //            int id = Convert.ToInt32(Session["manager_page"].ToString().Trim());
        //        }
        //        catch (Exception ex)
        //        {
        //            Session.Clear();
        //            Response.Redirect("mamaro_manager.aspx");
        //        }

        //    }
        //    else
        //    {
        //        Session.Clear();
        //        Response.Redirect("mamaro_manager.aspx");
        //    }
        //}
        //else
        //{
        //    Session.Clear();
        //    Response.Redirect("mamaro_manager.aspx");
        //}
    }
    string room_id = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        //test
        //Session["manager_page"] = "1";

        if (Session["manager_page"] != null)
        {
            if (Session["manager_page"].ToString().Trim() != "")
            {
                room_id = Session["manager_page"].ToString().Trim();



                //select info from mamaro DB
                GCP_MYSQL gc = new GCP_MYSQL();
                Literal lip = new Literal();
                //lip.Text += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js" + '"' + @"></script>";
                string Query = "";
                if (room_id == "2")
                {
                    Query = "select * from nursing_room;";
                }
                else
                {
                    Query = "select c.id,c.name,c.babymap_place_id,c.QRcode,c.close from nursing_room_manager as a ";
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
                            lip.Text += "<br><div class='center' id=" + '"' + "m_" + i + '"' + ">" + ict_ff.Table.Rows[i]["name"].ToString() + @"<br><br>
                              <img id=" + '"' + "img_" + i + '"' + @" src='https://storage.googleapis.com//upload/nursing_room/mamaro_on.png' alt='" + ict_ff.Table.Rows[i]["name"].ToString() + @"' style='width:100px;height:100px;'>";
                            if (room_id == "2")
                            {
                                lip.Text += "<img id=" + '"' + "imgoff_" + i + '"' + @" src='https://storage.googleapis.com//upload/nursing_room/mamaro_off.png' alt='" + ict_ff.Table.Rows[i]["name"].ToString() + @"' style='width:100px;height:100px;'>";
                            }
                            lip.Text += @"<div id=" + '"' + "write_" + i + '"' + @"> <hr>";
                            GCP_MYSQL gc1 = new GCP_MYSQL();
                            string Query1 = "select * from nursing_room_message_check where nursing_room_id='" + ict_ff.Table.Rows[i]["id"].ToString() + "';";
                            DataView ict_ff1 = gc1.select_cmd(Query1);
                            if (ict_ff1.Count > 0)
                            {
                                for (int ii = 0; ii < ict_ff1.Count; ii++)
                                {

                                    lip.Text += @"<p>" + ict_ff1.Table.Rows[ii]["update_time"].ToString() + @"</p>
<p>" + ict_ff1.Table.Rows[ii]["message"].ToString() + @"</p>
<p>";
                                    if (ict_ff1.Table.Rows[ii]["pdf_url"].ToString() != "")
                                    {
                                        lip.Text += @"
<a href='" + ict_ff1.Table.Rows[ii]["pdf_url"].ToString() + @"' target='_blank'><img src='./images/mamaro_manager/pdf-icon.png' title='PDF' width='100' height='100' /></a>";
                                    }
                                    if (ict_ff1.Table.Rows[ii]["csv_url"].ToString() != "")
                                    {
                                        lip.Text += @"
<a href='" + ict_ff1.Table.Rows[ii]["csv_url"].ToString() + @"' target='_blank'><img src='./images/mamaro_manager/Files-Csv-icon.png' title='CSV' width='100' height='100' /></a>";
                                    }
lip.Text +=@"</p> <hr>
";
                                }
                            }
                            lip.Text += @"
<div id=" + '"' + "but_" + i + '"' + @">back</div>
</div>";
                            if (room_id == "2")
                            {
                                lip.Text += "<div id=" + '"' + "edit_" + i + '"' + @"><hr>
<p>Content:</p>
<p>
            <textarea style='width: 100%;height:300px;' id =" + '"' + "mess_" + i + '"' + @"
                  rows = '3'
                  cols = '80'></textarea>
        </p>
<hr>
<p>PDF:</p>
<p><input type='file' id='file" + i+@"' name='file' /></p>
<img id=" + '"' + "butup_" + i + '"' + @" src='./images/mamaro_manager/upload-file.png' title='upload' width='100' height='100' />
    <div id='img" + i + @"' style='width:100%;word-break: break-all;'></div>
<hr>
 <script type='text/javascript'>
$(function () {
            $('#butup_" + i +@"').click(function () {
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
<div id=" + '"' + "buttb_" + i + '"' + @">back</div>
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
$show1 = $('#edit_" + i + @"');
$show1.hide();

$show2 = $('#imgoff_" + i + @"');
$show2.show();

$show3 = $('#butt_" + i + @"');
$show3.hide();

$show4 = $('#buttb_" + i + @"');
$show4.hide();

$('#imgoff_" + i + @"').click(function () {

$show1 = $('#edit_" + i + @"');
$show1.show();

$show2 = $('#imgoff_" + i + @"');
$show2.hide();

$show3 = $('#butt_" + i + @"');
$show3.show();

$show4 = $('#buttb_" + i + @"');
$show4.show();
});
$('#butt_" + i + @"').click(function () {

$show1 = $('#edit_" + i + @"');
$show1.hide();

$show2 = $('#imgoff_" + i + @"');
$show2.show();

$show3 = $('#butt_" + i + @"');
$show3.hide();

$show4 = $('#buttb_" + i + @"');
$show4.hide();
});


$('#buttb_" + i + @"').click(function () {

$show1 = $('#edit_" + i + @"');
$show1.hide();

$show2 = $('#imgoff_" + i + @"');
$show2.show();

$show3 = $('#butt_" + i + @"');
$show3.hide();

$show4 = $('#buttb_" + i + @"');
$show4.hide();
});

 $(" + '"' + @"#butt_" + i + '"' + @").click(function () {
                 $.ajax({
                     type: " + '"' + @"POST" + '"' + @",
                     url: " + '"' + @"mamaro_check_message.aspx/send_message" + '"' + @",
                     data: " + '"' + @"{param1: '" + '"' + @" + $('#mess_" + i + @"').val() + " + '"' + @"' ,param2: '" + ict_ff.Table.Rows[i]["QRcode"].ToString() + @"' ,param3: '" + '"' + @" + $('#img" + i + @"').text() + " + '"' + @"' ,param4: '" + '"' + @" + $('#imgg" + i + @"').text() + " + '"' + @"'  }" + '"' + @",
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
    </script>
";



                                lip.Text += "</div>";
                            }

                            lip.Text += @"</div><br>";


                            lip.Text += @"<script type=" + '"' + "text/javascript" + '"' + @">
                                        $(function(){
$show1 = $('#write_" + i + @"');
$show1.hide();

$show2 = $('#img_" + i + @"');
$show2.show();

$show3 = $('#but_" + i + @"');
$show3.hide();
$('#img_" + i + @"').click(function () {

$show1 = $('#write_" + i + @"');
$show1.show();

$show2 = $('#img_" + i + @"');
$show2.hide();

$show3 = $('#but_" + i + @"');
$show3.show();
});
$('#but_" + i + @"').click(function () {

$show1 = $('#write_" + i + @"');
$show1.hide();

$show2 = $('#img_" + i + @"');
$show2.show();

$show3 = $('#but_" + i + @"');
$show3.hide();


});




});


                                    </script>";




                        }




                    }

                }

                main_Panel.Controls.Add(lip);
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
    public static string send_message(string param1, string param2, string param3, string param4)
    {
        //string result = param1 + "," + param2;
        string result = "";
        string QR = RemoveSpecialCharacters(param2);

        GCP_MYSQL gc = new GCP_MYSQL();
        string Query = "select * from nursing_room where QRcode='" + QR + "';";
        DataView ict_ff = gc.select_cmd(Query);
        if (ict_ff.Count > 0)
        {
            string Query1 = "insert into nursing_room_message_check(nursing_room_id,message,pdf_url,csv_url,update_time)";
            Query1 += " values('" + ict_ff.Table.Rows[0]["id"].ToString() + "','" + param1 + "','" + param3 + "','" + param4 + "',NOW());";
            GCP_MYSQL gc1 = new GCP_MYSQL();
            result = gc1.insert_cmd(Query1);


            ////var url_five = new Uri("http://35.185.155.136/api/places/get_official_info?id=" + ict_ff.Table.Rows[0]["babymap_place_id"].ToString());
            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //    try
            //    {
            //        wc.Encoding = Encoding.UTF8;

            //        NameValueCollection nc = new NameValueCollection();
            //        nc["id"] = ict_ff.Table.Rows[0]["babymap_place_id"].ToString().Trim();
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
        return result;

    }
}
