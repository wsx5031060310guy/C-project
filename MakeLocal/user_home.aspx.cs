using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_home : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='main.aspx';");
        Label_logo.Style["cursor"] = "pointer";
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Panel pdn_j = (Panel)this.FindControl("javaplace_formap");
        pdn_j.Controls.Clear();
        Literal lip = new Literal();
        lip.Text += @"<script>
  $(function () {
            $('.lazy').Lazy({
                threshold: 200,
                effect: 'fadeIn',
                visibleOnly: true,
                effect_speed: 'fast',
                onError: function (element) {
                    console.log('error loading ' + element.data('src'));
                }
            });
        });
            </script>
            ";
        pdn_j.Controls.Add(lip);


        //check 利用者
        Query = "select id ";
        Query += "from user_information_school where uid='" + Session["id"].ToString() + "';";
        DataView ict_h_school = gc.select_cmd(Query);
        if (ict_h_school.Count == 0)
        {
            userhome_Button2.Visible = false;
        }
        else
        {
            userhome_Button2.Visible = true;
        }


        Session["top_count"] = 10;
        Query = "select id,username,photo,home_image,FBID ";
        Query += "from user_login where id='" + Session["id"].ToString() + "';";
        DataView ict_h = gc.select_cmd(Query);

        Panel p_head = (Panel)this.FindControl("head_homepage_user");
        Label laa = new Label();
        string cutstr_h = ict_h.Table.Rows[0]["home_image"].ToString();
        int ind_h = cutstr_h.IndexOf(@"/");
        string cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);


        p_head.Controls.Add(new LiteralControl("<div id='home_head_pic' style='background-image: url(" + '"' + cutstr_h1 + '"' + "); background-repeat:no-repeat; background-size: cover;'>"));
        //add upload home image button
        p_head.Controls.Add(new LiteralControl(@"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload_head' />
</label>
<br />
            <div id='progressbar_head' style='width:100%;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
            <div id='save_head' style='width:100%;display:none;'>
                <div>
                    <input type='button' value='保存' onclick='upload_head_img()' class='file-upload1' style='background: #FF9797;'/>
                </div>
            </div>
"));
        p_head.Controls.Add(new LiteralControl(@"<script>

$(function () {
$('#btnFileUpload_head').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar_head').show();
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar_head div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar_head').hide();
        $('#progressbar_head div').css('width', '0%');
 $('#save_head').show();
$('#home_head_pic').css('background-image', 'url('+response+')');
upload_headimg.value = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar_head').hide();
        $('#progressbar_head div').css('width', '0%');
        console.log('error', error);
    }
});});</script>"));
        p_head.Controls.Add(new LiteralControl("<table  class='sth' width='100%'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));

        //this
        p_head.Controls.Add(new LiteralControl("<td width='100%' height='100px'>"));

        //2 new buttons
        Button newbtn1 = new Button();
        newbtn1.ID = "newbtn1";
        newbtn1.Text = "子育てサポーターになる";
        newbtn1.Click += new System.EventHandler(this.change_panel);
        newbtn1.OnClientClick = "ShowProgressBar();";
        newbtn1.Style["border-style"] = "none";
        newbtn1.Style["font-size"] = "10px";
        newbtn1.Style["width"] = "100%";
        newbtn1.Style["height"] = "100%";
        newbtn1.Style["cursor"] = "pointer";
        newbtn1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        newbtn1.Font.Bold = true;

        Button newbtn2 = new Button();
        newbtn2.ID = "newbtn2";
        newbtn2.Text = "こどもを預ける";
        newbtn2.Click += new System.EventHandler(this.change_panel);
        newbtn2.OnClientClick = "ShowProgressBar();";
        newbtn2.Style["border-style"] = "none";
        newbtn2.Style["font-size"] = "10px";
        newbtn2.Style["width"] = "100%";
        newbtn2.Style["height"] = "100%";
        newbtn2.Style["cursor"] = "pointer";
        newbtn2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        newbtn2.Font.Bold = true;

        p_head.Controls.Add(new LiteralControl("<div>"));
        p_head.Controls.Add(new LiteralControl("<table width='100%'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td  align='center' width='75%'>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td  align='center' width='12.5%' style='border-style: solid;border-width:thin;'>"));
        p_head.Controls.Add(newbtn1);
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td  align='center' width='12.5%' style='border-style: solid;border-width:thin;'>"));
        p_head.Controls.Add(newbtn2);
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));
        p_head.Controls.Add(new LiteralControl("</div>"));
            p_head.Controls.Add(new LiteralControl("</td>"));
        //this
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td>"));


        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100px'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td class='avartar' rowspan='2' width='20%'>"));

        //user photo
        p_head.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
        cutstr_h = ict_h.Table.Rows[0]["photo"].ToString();
        ind_h = cutstr_h.IndexOf(@"/");
        cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
        p_head.Controls.Add(new LiteralControl("<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_h.Table.Rows[0]["username"].ToString() + "' style='width:100%;height:100%;'>"));
        p_head.Controls.Add(new LiteralControl("<img src='" + cutstr_h1 + "' width='100%' height='100%' style=' margin-bottom:-5px'  />"));
        p_head.Controls.Add(new LiteralControl("</a>"));
        p_head.Controls.Add(new LiteralControl("</div>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td>"));
        //user name
        p_head.Controls.Add(new LiteralControl("<table width='100%'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td valign='bottom' align='left'>"));
        p_head.Controls.Add(new LiteralControl("<br/>"));
        p_head.Controls.Add(new LiteralControl("<br/>"));
        p_head.Controls.Add(new LiteralControl("<span id='user_name' style='color:white;font-weight:bold; font-size:3em;'>"));
        p_head.Controls.Add(new LiteralControl(ict_h.Table.Rows[0]["username"].ToString()));
        p_head.Controls.Add(new LiteralControl("</span>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' valign='top' aligh='right'>"));

        p_head.Controls.Add(new LiteralControl("<input id='home_h' type='button' style='border-style: solid; border-color: #808080; width: 100%; height: 30px; ' value='本人' />"));
        p_head.Controls.Add(new LiteralControl("<div id='facebook_button_block'>"));
        if (ict_h.Table.Rows[0]["FBID"].ToString() != "")
        {
            p_head.Controls.Add(new LiteralControl("<a href='https://www.facebook.com/app_scoped_user_id/" + ict_h.Table.Rows[0]["FBID"].ToString() + "/' target='_blank' style='text-decoration:none;'><img class='_55at img' src='https://www.facebook.com/rsrc.php/v3/yR/r/teE39sffXW8.png' alt='' width='16' height='16'>  Facebook</a>"));
        }
        p_head.Controls.Add(new LiteralControl("</div>"));
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("<tr style='vertical-align: bottom'>"));

        p_head.Controls.Add(new LiteralControl("<td>"));


        //4 choice
        p_head.Controls.Add(new LiteralControl("<table class='four-choice' style='background-color: #808080;height: 50px; '>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td align='center' width='25%' style='border-style: solid;border-width:thin;'>"));

        Button buth = new Button();
        buth.ID = "home_h2";
        buth.Text = "タイムライン";
        buth.Click += new System.EventHandler(this.change_panel);
        buth.OnClientClick = "ShowProgressBar();";
        buth.Style["border-style"] = "none";
        buth.Style["width"] = "100%";
        buth.Style["height"] = "100%";
        buth.Style["cursor"] = "pointer";
        buth.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        buth.Font.Bold = true;
        p_head.Controls.Add(buth);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='25%' aligh='center' style='border-style: solid;border-width:thin;'>"));

        buth = new Button();
        buth.ID = "home_h3";
        buth.Text = "基本データ";
        buth.Click += new System.EventHandler(this.change_panel);
        buth.OnClientClick = "ShowProgressBar();";
        buth.Style["border-style"] = "none";
        buth.Style["width"] = "100%";
        buth.Style["height"] = "100%";
        buth.Style["cursor"] = "pointer";
        buth.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(buth);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='25%' aligh='center' style='border-style: solid;border-width:thin;'>"));

        buth = new Button();
        buth.ID = "home_h4";
        buth.Text = "友達一覧";
        buth.Click += new System.EventHandler(this.change_panel);
        buth.OnClientClick = "ShowProgressBar();";
        buth.Style["border-style"] = "none";
        buth.Style["width"] = "100%";
        buth.Style["height"] = "100%";
        buth.Style["cursor"] = "pointer";
        buth.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(buth);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='25%' aligh='center' style='border-style: solid;border-width:thin;'>"));

        buth = new Button();
        buth.ID = "home_h5";
        buth.Text = "お預けの履歴";
        buth.Click += new System.EventHandler(this.change_panel);
        buth.OnClientClick = "ShowProgressBar();";
        buth.Style["border-style"] = "none";
        buth.Style["width"] = "100%";
        buth.Style["height"] = "100%";
        buth.Style["cursor"] = "pointer";
        buth.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(buth);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));


        p_head.Controls.Add(new LiteralControl("</td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));



        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));
        p_head.Controls.Add(new LiteralControl("</div>"));
        ////head end



        ////head send message
        Panel p_send_m = (Panel)this.FindControl("userphoto");

        p_send_m.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
        p_send_m.Controls.Add(new LiteralControl("<a href='"+cutstr_h1+"' data-source='"+cutstr_h1+"' title='" + ict_h.Table.Rows[0]["username"].ToString() + "' style='width:40px;height:40px;'>"));
        p_send_m.Controls.Add(new LiteralControl("<img src='" + cutstr_h1 + "' width='40px' height='40px' />"));
        p_send_m.Controls.Add(new LiteralControl("</a>"));
        p_send_m.Controls.Add(new LiteralControl("</div>"));


        ////head send message end

        ////message start
        int totalran = Convert.ToInt32(Session["top_count"].ToString());
        Query = "select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
        Query += "from status_messages as a use index (IX_status_messages_1)";
        Query += " inner join user_login as b on b.id=a.uid";
        //if want to class by type use type=0,1,2 ; message_type=0,1,2

        Query += " where b.id='" + ict_h.Table.Rows[0]["id"].ToString() + "'";

        Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (totalran - 10) + ",10;";
        DataView ict = gc.select_cmd(Query);


        Literal li = new Literal();

        li.Text = @"<script>
 $(document).ready(function () {
             $('.image-link').magnificPopup({ type: 'image' });
         });

$(function () {

";

        for (int i = 0; i < ict.Count; i++)
        {
            li.Text += @"

$('#btnFileUpload" + i + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar" + i + @"').show();
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar" + i + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').show();
        document.getElementById('make-image" + i + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar" + i + @"').hide();
        $('#progressbar" + i + @" div').css('width', '0%');
        $('#image_place" + i + @"').hide();
        $('#image_place" + i + @" div').css('width', '0%');
        console.log('error', error);
    }
});






$('.hidde" + i + @"').toggle(false);

            $('.box" + i + @"').click(function () {
                $('.hidde" + i + @"').toggle();
                $('.box" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').toggle(false);

            $('.likebox" + i + @"').click(function () {
                $('.likehidde" + i + @"').toggle();
                $('.likebox" + i + @"').toggle(false);
            })

            $('.likehidde" + i + @"').click(function () {
                $('.likebox" + i + @"').toggle();
                $('.likehidde" + i + @"').toggle(false);
            })

            $('.mess_hidde" + i + @"').toggle(false);

            $('.mess_box" + i + @"').click(function () {
                $('.mess_hidde" + i + @"').toggle();
                $('.mess_box" + i + @"').toggle(false);
            })


            $('.big_mess_hidde" + i + @"').toggle(false);

            $('.big_mess_box" + i + @"').click(function () {
                $('.big_mess_hidde" + i + @"').toggle();
                $('.big_mess_box" + i + @"').toggle(false);
                $('.status_message_hidde" + i + @"').toggle();
                $('.status_message_box" + i + @"').toggle(false);
            })

            $('.big_mess_hidde" + i + @"').click(function () {
                $('.big_mess_box" + i + @"').toggle();
                $('.big_mess_hidde" + i + @"').toggle(false);
                $('.status_message_box" + i + @"').toggle();
                $('.status_message_hidde" + i + @"').toggle(false);
            })

            $('.status_message_hidde" + i + @"').toggle(false);


";

//            SqlDataSource sql3 = new SqlDataSource();
//            sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//            sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//            sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//            DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//            if (ict2.Count > 3)
//            {
//                li.Text += @"
//$('.imhidde" + i + @"').toggle(false);
//$('.imhiddee" + i + @"').toggle(false);
//
//            $('.imbox" + i + @"').click(function () {
//                $('.imhidde" + i + @"').toggle();
//                $('.imhiddee" + i + @"').toggle();
//                $('.imbox" + i + @"').toggle(false);
//            })
//
//            $('.imhiddee" + i + @"').click(function () {
//                $('.imbox" + i + @"').toggle();
//                $('.imhidde" + i + @"').toggle(false);
//                $('.imhiddee" + i + @"').toggle(false);
//            })
//
//
//";
//            }
        }

        li.Text += @"
                        })";
        li.Text += @"</script>";

        pdn_j = (Panel)this.FindControl("javaplace");
        pdn_j.Controls.Add(li);

        //this.Page.Controls.Add(li);


        //this.Page.Header.Controls.Add(li);
        ////添加至指定位置
        //this.Page.Header.Controls.AddAt(0, li);
        Literal litCss = new Literal();
        litCss.Text = @"
                <style type='text/css'>
                    #head_homepage_user{
border: 1px solid;
border-color: #e5e6e9 #dfe0e4 #d0d1d5;
border-radius: 3px;
                            }
                 </style>";
        pdn_j.Controls.Add(litCss);

        litCss = new Literal();
        litCss.Text = @"
                <style type='text/css'>
                    #send_homepage_user{
                    background-color:#fff;
                    border: thick solid #E9EBEE;
                            }
                 </style>";
        pdn_j.Controls.Add(litCss);



        Panel pdn2 = (Panel)this.FindControl("homepage_user");

        //edit


        for (int i = 0; i < ict.Count; i++)
        {
            Image edit = new Image();
            edit.ID = "editstate_" + ict.Table.Rows[i]["id"].ToString();
            edit.ImageUrl = "~/images/edit.png";
            edit.Style.Add("cursor", "pointer");
            edit.Attributes["onclick"] = "update_mess(this.id)";


            pdn2.Controls.Add(new LiteralControl("<table width='100%' style='background-color: #FFF; border: thick solid #E9EBEE;'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td>"));
            //big message place
            pdn2.Controls.Add(new LiteralControl("<table width='100%' style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'><br/></td><td width='90%' height='5%'><br/></td><td width='5%' height='5%'><br/></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td>"));

            pdn2.Controls.Add(new LiteralControl("</td>"));

            pdn2.Controls.Add(new LiteralControl("<td>"));
            //new message place
            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            //Poster photo
            pdn2.Controls.Add(new LiteralControl("<td width='10%' rowspan='2' valign='top'>"));

            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

            string cutstr2 = ict.Table.Rows[i]["photo"].ToString();
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:40px;height:40px;'>"));
            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='40' height='40' />"));
            pdn2.Controls.Add(new LiteralControl("</a>"));


            //Image img = new Image();
            //img.Width = 50; img.Height = 50;
            //img.ImageUrl = ict.Table.Rows[i]["photo"].ToString();
            //pdn2.Controls.Add(img);


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            //poster username
            pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));
            HyperLink hy = new HyperLink();
            hy.NavigateUrl = "~/user_home_friend.aspx?=" + ict.Table.Rows[i]["uid"].ToString();

            hy.Target = "_blank";
            hy.Text = ict.Table.Rows[i]["username"].ToString();
            hy.Font.Underline = false;
            pdn2.Controls.Add(hy);
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //poster message type and time
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));
            Label la = new Label();
            la.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
            la.Text = "";
            if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 0)
            {
                la.Text += "お食事、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 1)
            {
                la.Text += "人気スポット、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 2)
            {
                la.Text += "イベント、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 3)
            {
                la.Text += "病院、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 4)
            {
                la.Text += "公園／レジャー、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 5)
            {
                la.Text += "授乳室、";
            }
            else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 6)
            {
                la.Text += "指定なし、";
            }

            la.Text += ict.Table.Rows[i]["place"].ToString() + " ";
            la.Text += ict.Table.Rows[i]["year"].ToString() + "." + ict.Table.Rows[i]["month"].ToString() + "." + ict.Table.Rows[i]["day"].ToString();
            pdn2.Controls.Add(la);



            li = new Literal();

            //edit div css
            li.Text = @"<style>
           #dlgbox_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                display: none;
                position: fixed;
                width: 100%;
                z-index: 9999;
                border-radius: 10px;
                background-color: #7c7d7e;
            }
            #dlg-header_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: #f48686;
                color: white;
                font-size: 20px;
                padding: 10px;
                margin: 10px 10px 0px 10px;
                text-align: left;
            }

            #dlg-body_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: white;
                color: black;
                font-size: 14px;
                padding: 10px;
                margin: 0px 10px 0px 10px;
            }

            #dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: #f2f2f2;
                text-align: center;
                padding: 10px;
                margin: 0px 10px 10px 10px;
            }

            #dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @" button{
                background-color: #f48686;
                color: white;
                padding: 5px;
                border: 0px;
            }
</style>";



            //edit div
            li.Text += @"
<div class='dlg' id='dlgbox_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>
            <div id='dlg-header_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>編集</div>
            <div id='dlg-body_edit_" + ict.Table.Rows[i]["id"].ToString() + @"' style='height: 400px; overflow: auto'>
                <table style=' width: 100%;'>
                    <tr>
                        <td>
                            <hr/>
                            <table style='width: 100%;'>
                                <tr>
                                    <td width='20%'>
                                        <img alt='' src='" + cutstr3+ @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
                                        <textarea id='updateText_" + ict.Table.Rows[i]["id"].ToString() + @"' cols='40' rows='5' style='border-style:none;width:100%;'>" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", System.Environment.NewLine) + @"</textarea><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2' style='width: 100%;'>
<table style='width: 100%;'>
                                 <tr>
                                     <td class='auto-style1'>&nbsp;</td>
                                     <td align='left' width='30%'>

                                         <img alt='' src='images/tag.png' height='20px' width='20px' />
 <select name='selectedittagname_" + ict.Table.Rows[i]["id"].ToString() + @"' id='selectedittag_" + ict.Table.Rows[i]["id"].ToString() + @"'>
     <option value=''>カテゴリー</option>
      <option value='0'>お食事</option>
      <option value='2'>イベント</option>
      <option value='3'>病院</option>
      <option value='4'>公園／レジャー</option>
       <option value='5'>授乳室</option>
</select>


                                     </td>
 <td width='25%'>
<span>
  <div class='ui inline dropdown'>
    <div class='text'>
      <img class='ui avatar image' src='images/icon/public.png'>
      一般公開
    </div>
    <i class='dropdown icon'></i>
    <div class='menu'>
       <div class='item'>
        <img class='ui avatar image' src='images/icon/public.png'>
        一般公開
      </div>
      <div class='item'>
        <img class='ui avatar image' src='images/icon/neighborhood.png'>
        地域限定
      </div>
      <div class='item'>
        <img class='ui avatar image' src='images/icon/friend.png'>
        友達
      </div>
    </div>
  </div>
</span>
    </td>
    </tr>
    </table>
                                    </td>
                                </tr>
                            </table>
                            <hr/>
                        </td>
</tr><tr>
<td colspan='2' style='width: 100%;'>
<div id='' style='overflow-x: auto; overflow-y:auto; height:200px;text-align: center;'>
";
            Query = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
            Query += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
            DataView ict_edit = gc.select_cmd(Query);
            if (ict_edit.Count > 0)
            {
                for (int ik = 0; ik < ict_edit.Count; ik++)
                {
                    string cutstr = ict_edit.Table.Rows[ik]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    li.Text += @"<img src='" + cutstr1 + "' width='200px' height='200px' />";
                }
            }
            li.Text += @"
</div>
</td>
                    </tr>
                </table>
            </div>
            <div id='dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>
<table style=' width: 100%;'>
<tr>
<td width='50%' align='left'>
<input id='statebutcancel_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='取り消す' onclick='dlgupcanel(this.id)' class='file-upload1'/>
</td>
<td width='50%' align='right'>
                <input id='statebutedit_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='保存' onclick='dlgupdate(this.id)' class='file-upload1'/>
</td>
            </tr>
</table>
</div>
        </div>
";

            //edit div js
            li.Text += @"<script>
 $(function () {
 $('#selectedittag_" + ict.Table.Rows[i]["id"].ToString() + @"')
                 .dropdown()
             ;
});
</script>";


            pdn2.Controls.Add(li);
            //edit



            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //poster message
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td colspan='2' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">"));
            pdn2.Controls.Add(new LiteralControl("<br/>"));
            pdn2.Controls.Add(new LiteralControl("<div class='box" + i + "'>"));
            HyperLink hyy;
            if (ict.Table.Rows[i]["message"].ToString().Length < 37)
            {
                pdn2.Controls.Add(new LiteralControl(ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString())));
            }
            else
            {
                pdn2.Controls.Add(new LiteralControl(ict.Table.Rows[i]["message"].ToString().Substring(0, 37) + "‧‧‧"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "もっと見る";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
            }


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div class='hidde" + i + "'>"));

            Label la1 = new Label();
            la1.Style.Add("word-break", "break-all");
            la1.Style.Add("over-flow", "hidden");
            la1.Text = ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString());
            pdn2.Controls.Add(la1);
            pdn2.Controls.Add(new LiteralControl("<br/>"));


            //if (ict.Table.Rows[i]["message"].ToString().Length > 36)
            //{
            //    hyy = new HyperLink();
            //    hyy.NavigateUrl = "javascript:void(0);";
            //    hyy.Target = "_blank";
            //    hyy.Text = "たたむ";
            //    hyy.Font.Underline = false;
            //    pdn2.Controls.Add(hyy);
            //}


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div>"));
            pdn2.Controls.Add(new LiteralControl("<span style='word-break:break-all;over-flow:hidden;'>" + ConvertUrlsToLinks_DIV(ict.Table.Rows[i]["message"].ToString()) + "</span>"));
            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //poster images
            string shareimg = "";
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td colspan='2' width='90%' align='center'><br/><br/>"));
            Query = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
            Query += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
            DataView ict1 = gc.select_cmd(Query);
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int typ = Convert.ToInt32(rand.Next(0, ict1.Count));
            if (ict1.Count > 3)
            {
                pdn2.Controls.Add(new LiteralControl("<div class='imbox" + i + "'>"));
                //for (int ii = 0; ii < 3; ii++)
                //{
                //    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                //    int ind = cutstr.IndexOf(@"/");
                //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                //    Image imgg = new Image();
                //    imgg.Width = 100; imgg.Height = 100;
                //    imgg.ImageUrl = cutstr1;
                //    pdn2.Controls.Add(imgg);
                //    pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                //}
                //pdn2.Controls.Add(new LiteralControl("<br/>"));
                //hyy = new HyperLink();
                //hyy.NavigateUrl = "javascript:void(0);";
                //hyy.Target = "_blank";
                //hyy.Text = "もっと見る";
                //hyy.Font.Underline = false;
                //pdn2.Controls.Add(hyy);
                //pdn2.Controls.Add(new LiteralControl("</div>"));
                //pdn2.Controls.Add(new LiteralControl("<div class='imhidde" + i + "'>"));

                pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                string morefour = "";
                int countimg = 0;
                for (int ii = 0; ii < ict1.Count; ii++)
                {
                    //if (ii > 0 && ii % 3 == 0)
                    //{
                    //    pdn2.Controls.Add(new LiteralControl("<br/>"));
                    //}

                    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    if (shareimg == "")
                    {
                        shareimg = cutstr1;
                    }
                    //block grid
                    if (ii > 3)
                    {
                        countimg += 1;
                        pdn2.Controls.Add(new LiteralControl("<div style='visibility:hidden;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:0px; height:0px;outline : none;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' width='0px' height='0px'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                    }
                    else
                    {
                        if (ii == 3)
                        {

                            morefour += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            morefour += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                        }
                        else
                        {

                            pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                            pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));

                            pdn2.Controls.Add(new LiteralControl("</a>"));
                            pdn2.Controls.Add(new LiteralControl("</div>"));
                        }
                    }

                    //Image imgg = new Image();
                    //imgg.Width = 100; imgg.Height = 100;
                    //imgg.ImageUrl = ict1.Table.Rows[ii]["filename"].ToString();
                    //pdn2.Controls.Add(imgg);
                    //pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                }
                //countimg
                if (countimg > 0)
                {
                    morefour += "<img src='images/test.png' style='background-color: #000; opacity: 0.8; width: 100%; height: 100%; text-align: center;'/>";
                    morefour += "<span style='color: white;position: absolute;top:50%;left:40%;font-size:xx-large;'>+" + countimg + "</span>";
                }
                else
                {
                    morefour += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                }
                morefour += "</a>";
                morefour += "</div>";
                //string blockimg="<div style='position: absolute; background-color: #000; z-index: 999997; opacity: 0.8; width: 100%; height: 100%; text-align: center;'>";
                pdn2.Controls.Add(new LiteralControl(morefour));
                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("</div>"));

                Literal litjs = new Literal();
                litjs.Text = @"
                                    <script type='text/javascript'>
                                        var wall" + i + @" = new Freewall('#freewall" + i + @"');
                    			wall" + i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" + i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                pdn2.Controls.Add(litjs);


                pdn2.Controls.Add(new LiteralControl("</div>"));
            }
            else if (ict1.Count > 0)
            {
                string cutstr = ict1.Table.Rows[0]["filename"].ToString();
                int ind = cutstr.IndexOf(@"/");
                string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                if (shareimg == "")
                {
                    shareimg = cutstr1;
                }
                if (ict1.Count == 1)
                {
                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                    cutstr = ict1.Table.Rows[0]["filename"].ToString();
                    ind = cutstr.IndexOf(@"/");
                    cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr1 + "' src='images/loading.gif' style='width:100%;height:100%;'/>"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                }
                else if (ict1.Count == 2)
                {
                    pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                    pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                    if (typ == 0)
                    {
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[1]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                    }
                    else
                    {
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[1]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                    }
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                }
                else if (ict1.Count == 3)
                {
                    pdn2.Controls.Add(new LiteralControl("<div id='freewall" + i + "'>"));
                    pdn2.Controls.Add(new LiteralControl("<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >"));
                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                    if (typ == 0)
                    {
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[1]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[2]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                    }
                    else if (typ == 1)
                    {
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[1]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[2]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                    }
                    else
                    {
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[1]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        cutstr = ict1.Table.Rows[2]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                        pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>"));
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));
                        pdn2.Controls.Add(new LiteralControl("</div>"));
                    }
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                    pdn2.Controls.Add(new LiteralControl("</div>"));
                }




                //pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>"));
                //pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='100' height='100' />"));
                //pdn2.Controls.Add(new LiteralControl("</a>"));



                //pdn2.Controls.Add(new LiteralControl("</div>"));

                //
                Literal litjs = new Literal();
                litjs.Text = @"
                                    <script type='text/javascript'>

                                        var wall" + i + @" = new Freewall('#freewall" + i + @"');
                    			wall" + i + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + i + @".fitWidth();
                    				}
                    			});
                    			wall" + i + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                pdn2.Controls.Add(litjs);
            }

            string id = "";
            bool check_li = false;
            if (Session["id"] != null)
            {
                if (Session["id"].ToString() != "")
                {
                    id = Session["id"].ToString();


                    Query = "select id from status_messages_user_like";
                    Query += " where uid='" + id + "' and smid='" + ict.Table.Rows[i]["id"].ToString() + "';";
                    DataView ict_f_like = gc.select_cmd(Query);
                    if (ict_f_like.Count > 0)
                    {
                        check_li = true;
                    }
                }
            }


            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            //poster message like and share
            pdn2.Controls.Add(new LiteralControl("<td width='15%' style='white-space: nowrap;' align='right'><br/><br/>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likebox" + i + "'>"));
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.Text = "いいね";
            Image img1 = new Image();
            if (check_li)
            {
                img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like.png";
                img1.Attributes["onclick"] = "like(this.id)";
                laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
                laa.ID = "lalike_but" + ict.Table.Rows[i]["id"].ToString();
                laa.Attributes["onclick"] = "like(this.id)";
            }
            else
            {
                img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b.png";
                img1.Attributes["onclick"] = "blike(this.id)";
                laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                laa.ID = "lablike_but" + ict.Table.Rows[i]["id"].ToString();
                laa.Attributes["onclick"] = "blike(this.id)";

            }
            pdn2.Controls.Add(img1);

            pdn2.Controls.Add(laa);

            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likehidde" + i + "'>"));
            img1 = new Image();
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.Text = "いいね";
            if (check_li)
            {

                img1.ID = "blike_but" + ict.Table.Rows[i]["id"].ToString();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b.png";
                img1.Attributes["onclick"] = "blike(this.id)";
                laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                laa.ID = "lablike_but" + ict.Table.Rows[i]["id"].ToString();
                laa.Attributes["onclick"] = "blike(this.id)";

            }
            else
            {
                img1.ID = "like_but" + ict.Table.Rows[i]["id"].ToString();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like.png";
                img1.Attributes["onclick"] = "like(this.id)";
                laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
                laa.ID = "lalike_but" + ict.Table.Rows[i]["id"].ToString();
                laa.Attributes["onclick"] = "like(this.id)";

            }
            pdn2.Controls.Add(img1);

            pdn2.Controls.Add(laa);
            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));




            pdn2.Controls.Add(new LiteralControl("<td>"));
            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='big_mess_box" + i + "'>"));
            img1 = new Image();
            img1.Width = 15; img1.Height = 15;
            img1.ImageUrl = "~/images/mess_b.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
            laa.Text = "コメント";
            pdn2.Controls.Add(laa);




            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='big_mess_hidde" + i + "'>"));
            img1 = new Image();
            img1.Width = 15; img1.Height = 15;
            img1.ImageUrl = "~/images/mess.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#676767");
            laa.Text = "コメント";
            pdn2.Controls.Add(laa);


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td align='left'><br/><br/>"));
            pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' style='cursor: pointer'>"));
            //pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
            img1 = new Image();
            img1.Width = 15; img1.Height = 15;
            img1.ImageUrl = "~/images/share_b.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
            laa.Text = "シェア";
            pdn2.Controls.Add(laa);

            pdn2.Controls.Add(new LiteralControl("</div>"));
            int len = ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length;
            if (ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length > 99)
            {
                len = 99;
            }
            li = new Literal();
            li.Text = @"
                       <div id='share_div" + i + @"' title='シェア' style='display:none;'><table width='100%'><tr><td><div id='facebook_share" + i + @"' class='jssocials-share jssocials-share-facebook'><a href='#' class='jssocials-share-link'><i class='fa fa-facebook jssocials-share-logo'></i></a></div></div></td><td><div id='share_div_" + i + @"'></div></td></tr><tr><td colspan='2'><div id='share_div__" + i + @"'></div></td></tr></table>

                       <script type='text/javascript'>
  $(function() {
$('#share_div_" + i + @"').jsSocials({
            showLabel: false,
            showCount: false,
            shares: ['email', 'twitter', 'googleplus', 'line'],
            url: 'http://.jp/',
            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
            shareIn: 'popup'
        });
$('#share_div" + i + @"').dialog({
                autoOpen: false,
                show: {
                    effect: 'blind',
                    duration: 100
                },
                hide: {
                    effect: 'explode',
                    duration: 100
                }
            });
   $('#sharebox" + i + @"').on('click', function () {
                $('#share_div" + i + @"').dialog('open');

           });
$('#facebook_share" + i + @"').on('click', function () {
               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "") + @"”');

           });
 });
</script>

    ";
            pdn2.Controls.Add(li);
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td style='vertical-align:top;text-align: center;'>"));
            pdn2.Controls.Add(edit);
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));

            pdn2.Controls.Add(new LiteralControl("<tr style='background-color:#F6F7F9;'>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("<td>"));



            pdn2.Controls.Add(new LiteralControl("<div class='status_message_box" + i + "' style='background-color: #ffffff'>"));
            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));
            pdn2.Controls.Add(new LiteralControl("</div >"));
            pdn2.Controls.Add(new LiteralControl("<div class='status_message_hidde" + i + "' style='background-color: #dddddd'>"));


            pdn2.Controls.Add(new LiteralControl("<table width='100%' align='left'>"));
            //first space way
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            //second space way
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            //someone answer
            pdn2.Controls.Add(new LiteralControl("<td>"));


            pdn2.Controls.Add(new LiteralControl("<table width='100%' align='left'>"));
            //who like this message
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='10px'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%' height='10px'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='10px'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%'>"));

            Query = "select b.username,b.id from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
            Query += " where a.smid=" + ict.Table.Rows[i]["id"].ToString() + "";
            Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            DataView ict3 = gc.select_cmd(Query);
            if (ict3.Count > 2)
            {
                img1 = new Image();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b_1.png";
                pdn2.Controls.Add(img1);
                for (int iii = 0; iii < 2; iii++)
                {

                    hyy = new HyperLink();
                    hyy.NavigateUrl = "~/user_home_friend.aspx?="+ict3.Table.Rows[iii]["id"].ToString();
                    hyy.Target = "_blank";
                    hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);
                    pdn2.Controls.Add(new LiteralControl("、"));
                }
                pdn2.Controls.Add(new LiteralControl("<a id='listlike_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='check_like_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;'>他" + (ict3.Count - 2) + "人</a>"));

            }
            else if (ict3.Count > 0)
            {
                img1 = new Image();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b_1.png";
                pdn2.Controls.Add(img1);
                for (int iii = 0; iii < ict3.Count; iii++)
                {
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "~/user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString();
                    hyy.Target = "_blank";
                    hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);

                    if (iii != ict3.Count - 1)
                    {
                        pdn2.Controls.Add(new LiteralControl("、"));
                    }
                }
            }

            pdn2.Controls.Add(new LiteralControl("<hr/>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //who talk about this status message before
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='95%'>"));



            Query = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level,c.uid";
            Query += " from status_messages as a inner join status_messages_user as c";
            Query += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
            Query += " inner join status_messages_user_talk as e on e.smuid=c.id";
            Query += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + "";
            Query += " ORDER BY e.year desc,e.month desc,e.day desc,e.hour desc,e.minute desc,e.second desc;";
            ict3 = gc.select_cmd(Query);
            List<sorttalk> talk_list = new List<sorttalk>();
            sorttalk so = new sorttalk();
            for (int iy = 0; iy < ict3.Count; iy++)
            {
                so = new sorttalk();
                so.id = Convert.ToInt32(ict3.Table.Rows[iy]["id"].ToString());
                so.level = Convert.ToInt32(ict3.Table.Rows[iy]["structure_level"].ToString());
                so.point_id = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_message_id"].ToString());
                so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_user_id"].ToString());
                so.filename = ict3.Table.Rows[iy]["filename"].ToString();
                so.mess = ict3.Table.Rows[iy]["message"].ToString();

                if (ict3.Table.Rows[iy]["pointer_user_id"].ToString() == "0")
                {
                    so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["uid"].ToString());
                    so.username = ict3.Table.Rows[iy]["username"].ToString();
                    so.photo = ict3.Table.Rows[iy]["photo"].ToString();
                }
                else
                {

                    Query = "select username,photo from user_login";
                    Query += " where id=" + ict3.Table.Rows[iy]["pointer_user_id"].ToString() + ";";
                    DataView ict5 = gc.select_cmd(Query);
                    so.username = ict5.Table.Rows[0]["username"].ToString();
                    so.photo = ict5.Table.Rows[0]["photo"].ToString();
                }
                talk_list.Add(so);
            }

            Query = "select max(e.structure_level) as maxlevel";
            //sql5.SelectCommand = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
            Query += " from status_messages as a inner join status_messages_user as c";
            Query += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
            Query += " inner join status_messages_user_talk as e on e.smuid=c.id";
            Query += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + ";";
            DataView ict4 = gc.select_cmd(Query);

            int maxlevel = 0;
            if (ict4.Table.Rows[0]["maxlevel"].ToString() != "")
            {
                maxlevel = Convert.ToInt32(ict4.Table.Rows[0]["maxlevel"].ToString());
            }

            List<sorttalk> talk_list_tmp = new List<sorttalk>();
            so = new sorttalk();
            for (int ik = 0; ik < talk_list.Count; ik++)
            {
                if (talk_list[ik].level == 0)
                {
                    so = new sorttalk();
                    so.id = talk_list[ik].id;
                    so.level = talk_list[ik].level;
                    so.filename = talk_list[ik].filename;
                    so.mess = talk_list[ik].mess;
                    so.photo = talk_list[ik].photo;
                    so.point_id = talk_list[ik].point_id;
                    so.uid = talk_list[ik].uid;
                    so.username = talk_list[ik].username;
                    talk_list_tmp.Add(so);
                }
            }
            talk_list.Sort((a, b) => a.id.CompareTo(b.id));
            for (int ik = 0; ik < talk_list.Count; ik++)
            {
                for (int le = 1; le < maxlevel + 1; le++)
                {
                    if (talk_list[ik].level == le)
                    {
                        so = new sorttalk();
                        so.id = talk_list[ik].id;
                        so.level = talk_list[ik].level;
                        so.filename = talk_list[ik].filename;
                        so.mess = talk_list[ik].mess;
                        so.photo = talk_list[ik].photo;
                        so.point_id = talk_list[ik].point_id;
                        so.uid = talk_list[ik].uid;
                        so.username = talk_list[ik].username;
                        for (int ikk = 0; ikk < talk_list_tmp.Count; ikk++)
                        {
                            if (talk_list_tmp[ikk].id == talk_list[ik].point_id)
                            {
                                talk_list_tmp.Insert(ikk + 1, so);
                            }
                        }
                    }
                }
            }
            Image img2 = new Image();
            if (ict3.Count > 1)
            {
                //show div
                pdn2.Controls.Add(new LiteralControl("<div class='mess_box" + i + "'>"));
                pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                pdn2.Controls.Add(new LiteralControl("<tr>"));
                pdn2.Controls.Add(new LiteralControl("<td width='100%' align='left' colspan='2'>"));

                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "以前のコメントを見る";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);

                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("</tr>"));
                pdn2.Controls.Add(new LiteralControl("<tr>"));
                pdn2.Controls.Add(new LiteralControl("<td width='10%' rowspan='2' valign='top'>"));


                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                cutstr2 = talk_list_tmp[0].photo;
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:32px;height:32px;'>"));
                pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                pdn2.Controls.Add(new LiteralControl("</a>"));

                pdn2.Controls.Add(new LiteralControl("</div>"));


                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[0].uid.ToString();
                hyy.Target = "_blank";
                hyy.Text = talk_list_tmp[0].username.ToString();
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
                pdn2.Controls.Add(new LiteralControl("<br/>"));
                pdn2.Controls.Add(new LiteralControl(ict3.Table.Rows[0]["message"].ToString()));
                pdn2.Controls.Add(new LiteralControl("<br/>"));

                if (talk_list_tmp[0].filename != "")
                {
                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    cutstr2 = talk_list_tmp[0].filename;
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));

                    pdn2.Controls.Add(new LiteralControl("</div>"));

                    pdn2.Controls.Add(new LiteralControl("<br/>"));

                }

                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("</tr>"));
                pdn2.Controls.Add(new LiteralControl("<tr>"));
                pdn2.Controls.Add(new LiteralControl("<td>"));
                //who talk about status message and who like

                hyy = new HyperLink();
                hyy.ID = "wholike_" + talk_list_tmp[0].id + "_s";
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "いいね!";

                Query = "select good_status from status_messages_user_talk_like";
                Query += " where smutid='" + talk_list_tmp[0].id + "' and uid='" + Session["id"].ToString() + "';";

                DataView ict_who_like = gc.select_cmd(Query);
                if (ict_who_like.Count > 0)
                {
                    if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                    {
                        hyy.Style.Add("color", "#4183C4");
                        hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                    }
                    else
                    {
                        hyy.Style.Add("color", "#D84C4B");
                        hyy.Attributes["onclick"] = "slike_who_answer(this.id)";
                    }
                }
                else
                {
                    hyy.Style.Add("color", "#4183C4");
                    hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                }
                pdn2.Controls.Add(hyy);


                pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "返信";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);

                //who like who answer post message
                Query = "select count(*) as howmany from status_messages_user_talk_like";
                Query += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_who_like = gc.select_cmd(Query);
                if (ict_who_like.Count > 0)
                {
                    img1 = new Image();
                    img1.Width = 15; img1.Height = 15;
                    img1.ImageUrl = "~/images/like_b_1.png";
                    pdn2.Controls.Add(img1);
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);
                }
                //who like who answer post message



                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("</tr>"));
                pdn2.Controls.Add(new LiteralControl("</table>"));
                pdn2.Controls.Add(new LiteralControl("</div>"));
                //hidde message
                pdn2.Controls.Add(new LiteralControl("<div class='mess_hidde" + i + "'>"));
                pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                {

                    pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    int wid = (10 + (10 * talk_list_tmp[iiii].level));
                    if (wid > 90) { wid = 90; }
                    pdn2.Controls.Add(new LiteralControl("<td width='" + wid + "%' align='right' rowspan='2' valign='top'>"));

                    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    cutstr2 = talk_list_tmp[iiii].photo;
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));

                    pdn2.Controls.Add(new LiteralControl("</div>"));


                    pdn2.Controls.Add(new LiteralControl("</td>"));

                    pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString();
                    hyy.Target = "_blank";
                    hyy.Text = talk_list_tmp[iiii].username.ToString();
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);

                    pdn2.Controls.Add(new LiteralControl("<br/>"));
                    pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].mess.ToString()));
                    pdn2.Controls.Add(new LiteralControl("<br/>"));

                    if (talk_list_tmp[iiii].filename.ToString() != "")
                    {
                        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                        cutstr2 = talk_list_tmp[iiii].filename.ToString();
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));

                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        pdn2.Controls.Add(new LiteralControl("<br/>"));
                    }

                    pdn2.Controls.Add(new LiteralControl("</td>"));
                    pdn2.Controls.Add(new LiteralControl("</tr>"));
                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    pdn2.Controls.Add(new LiteralControl("<td>"));


                    //who talk about status message and who like

                    hyy = new HyperLink();
                    hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "いいね!";

                    Query = "select good_status from status_messages_user_talk_like";
                    Query += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + Session["id"].ToString() + "';";
                    ict_who_like = gc.select_cmd(Query);
                    if (ict_who_like.Count > 0)
                    {
                        if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                        {
                            hyy.Style.Add("color", "#4183C4");
                            hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                        }
                        else
                        {
                            hyy.Style.Add("color", "#D84C4B");
                            hyy.Attributes["onclick"] = "like_who_answer(this.id)";
                        }
                    }
                    else
                    {
                        hyy.Style.Add("color", "#4183C4");
                        hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                    }
                    pdn2.Controls.Add(hyy);
                    pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    hyy = new HyperLink();
                    hyy.ID = "whowantans_" + talk_list_tmp[iiii].id;
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "返信";
                    hyy.Attributes["onclick"] = "who_answer(this.id)";
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);

                    //who like who answer post message

                    Query = "select count(*) as howmany from status_messages_user_talk_like";
                    Query += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                    //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_who_like = gc.select_cmd(Query);
                    if (ict_who_like.Count > 0)
                    {
                        img1 = new Image();
                        img1.Width = 15; img1.Height = 15;
                        img1.ImageUrl = "~/images/like_b_1.png";
                        pdn2.Controls.Add(img1);
                        hyy = new HyperLink();
                        hyy.ID = "likecount" + talk_list_tmp[iiii].id;
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);
                    }
                    //who like who answer post message

                    pdn2.Controls.Add(new LiteralControl("</td>"));
                    pdn2.Controls.Add(new LiteralControl("</tr>"));


                    pdn2.Controls.Add(new LiteralControl("</table>"));
                    pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));
                }
                pdn2.Controls.Add(new LiteralControl("</div>"));

            }
            else
            {
                if (ict3.Count > 0)
                {
                    for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                    {

                        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        int wid = (10 + (10 * talk_list_tmp[iiii].level));
                        if (wid > 90) { wid = 90; }
                        pdn2.Controls.Add(new LiteralControl("<td width='" + wid + "%' align='right' rowspan='2' valign='top'>"));


                        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                        cutstr2 = talk_list_tmp[iiii].photo;
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));

                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        pdn2.Controls.Add(new LiteralControl("</td>"));

                        pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));

                        hyy = new HyperLink();
                        hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString();
                        hyy.Target = "_blank";
                        hyy.Text = talk_list_tmp[iiii].username.ToString();
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);

                        pdn2.Controls.Add(new LiteralControl("<br/>"));
                        pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].mess.ToString()));
                        pdn2.Controls.Add(new LiteralControl("<br/>"));

                        if (talk_list_tmp[iiii].filename.ToString() != "")
                        {
                            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                            cutstr2 = talk_list_tmp[iiii].filename.ToString();
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                            pdn2.Controls.Add(new LiteralControl("<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />"));
                            pdn2.Controls.Add(new LiteralControl("</a>"));

                            pdn2.Controls.Add(new LiteralControl("</div>"));
                            pdn2.Controls.Add(new LiteralControl("<br/>"));
                        }

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));
                        pdn2.Controls.Add(new LiteralControl("<tr>"));
                        pdn2.Controls.Add(new LiteralControl("<td>"));


                        //who talk about status message and who like
                        hyy = new HyperLink();
                        hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "いいね!";

                        Query = "select good_status from status_messages_user_talk_like";
                        Query += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + Session["id"].ToString() + "';";
                        DataView ict_who_like = gc.select_cmd(Query);
                        if (ict_who_like.Count > 0)
                        {
                            if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                            {
                                hyy.Style.Add("color", "#4183C4");
                                hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                            }
                            else
                            {
                                hyy.Style.Add("color", "#D84C4B");
                                hyy.Attributes["onclick"] = "like_who_answer(this.id)";
                            }
                        }
                        else
                        {
                            hyy.Style.Add("color", "#4183C4");
                            hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                        }
                        pdn2.Controls.Add(hyy);
                        pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        hyy = new HyperLink();
                        hyy.ID = "whowantans_" + talk_list_tmp[iiii].id;
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "返信";
                        hyy.Attributes["onclick"] = "who_answer(this.id)";
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);

                        //who like who answer post message

                        Query = "select count(*) as howmany from status_messages_user_talk_like";
                        Query += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                        //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                        ict_who_like = gc.select_cmd(Query);
                        if (ict_who_like.Count > 0)
                        {
                            img1 = new Image();
                            img1.Width = 15; img1.Height = 15;
                            img1.ImageUrl = "~/images/like_b_1.png";
                            pdn2.Controls.Add(img1);
                            hyy = new HyperLink();
                            hyy.ID = "likecount" + talk_list_tmp[iiii].id;
                            hyy.NavigateUrl = "javascript:void(0);";
                            hyy.Target = "_blank";
                            hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                            hyy.Font.Underline = false;
                            pdn2.Controls.Add(hyy);
                        }
                        //who like who answer post message

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));


                        pdn2.Controls.Add(new LiteralControl("</table>"));

                        //user answer user answer
                        pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));



                    }
                }
            }

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));


            pdn2.Controls.Add(new LiteralControl("</table>"));


            pdn2.Controls.Add(new LiteralControl("</td>"));
            //second space way
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //third space way
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("<td>"));

            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));
            //user photo

            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));


            Query = "select photo,username from user_login ";
            Query += " where id='" + Session["id"].ToString() + "';";
            DataView ict2 = gc.select_cmd(Query);
            string userr = "";
            if (ict2.Count > 0)
            {
                cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                ind2 = cutstr2.IndexOf(@"/");
                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                userr = ict2.Table.Rows[0]["username"].ToString();
            }

            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:40px;height:40px;'>"));
            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='40' height='40' />"));
            pdn2.Controls.Add(new LiteralControl("</a>"));

            pdn2.Controls.Add(new LiteralControl("</div>"));

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='85%' style='white-space: nowrap'>"));

            //user answer
            pdn2.Controls.Add(new LiteralControl("<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_" + i + "' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;' title='【Enter】キーを押してください'>"));
            //TextBox tex = new TextBox();
            //tex.Width = Unit.Percentage(95);
            //tex.Height = 30;
            //tex.ID = "send" + ict.Table.Rows[i]["id"].ToString();
            //tex.Attributes["onKeydown"] = "Javascript: if (event.which == 13 || event.keyCode == 13) sendmessage(this.id);";
            //tex.Attributes.Add("placeholder", "コメントする");
            //pdn2.Controls.Add(tex);

            //pdn2.Controls.Add(new LiteralControl("<br/>"));

            pdn2.Controls.Add(new LiteralControl(@"
<label class='file-upload2' style='top: -16px !important;'><span><img src='images/photo.png' alt='' width='20px' height='20px' ></span>
            <input type='file' name='file' id='btnFileUpload" + i + @"' />
</label>
<br />
            <div id='progressbar" + i + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place" + i + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image" + i + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>
"));




            //pdn2.Controls.Add(new LiteralControl(@"<label class='file-upload'><span><strong>画像を登録</strong></span>"));

            //FileUpload fi=new FileUpload();
            //fi.ID="fuDocument_"+i;
            //fi.Attributes.Add("onchange", "UploadFile(this,this.id);");
            //pdn2.Controls.Add(fi);
            //pdn2.Controls.Add(new LiteralControl(@"</label><br />"));



            //Button but = new Button();
            //but.ID = "btnUploadDoc_" + i;
            //but.Text = "Upload";
            //but.Click += new System.EventHandler(this.UploadDocument);
            //but.OnClientClick = "ShowProgressBar();";
            //but.Style["display"] = "none";
            //pdn2.Controls.Add(but);

            //img1 = new Image();
            //img1.Width = 100; img1.Height = 150;
            //img1.ID = "Image_" + i;
            //img1.Visible = false;


            //pdn2.Controls.Add(img1);



            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));


            pdn2.Controls.Add(new LiteralControl("</div>"));


            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //fourth space way
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));



            pdn2.Controls.Add(new LiteralControl("</table>"));


            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));
        }





        //this.form1.Controls.Add(pdn2);



        //pdn.Controls.Add(new LiteralControl("<table align='center'>"));
        //pdn.Controls.Add(new LiteralControl("<tr><td align='center'>"));
        //pdn.Controls.Add(la);


        //string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        //string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        //string startm = DateTime.Now.Minute.ToString();
        //string starts = DateTime.Now.Second.ToString();
        //string d = Convert.ToString(DateTime.Now.ToLocalTime());
        //string start = startd + " " + starth + ":" + startm + ":" + starts;
        //string star = starth + ":" + startm + ":" + starts;
        //Session["student_starttimeh"] = starth;
        //Session["student_starttimem"] = startm;
        //Session["student_starttimes"] = starts;


        //sql1.SelectCommand = "select date,starttime,endtime from fordate where date='" + startd + "' and starttime<'" + start + "' and endtime>'" + start + "';";
        //DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);
        //this.form1.Controls.Add(sql1);





        ////friend ship panel
        Panel pdn_f = (Panel)this.FindControl("friend_Panel");
        pdn_f.Controls.Add(new LiteralControl("<table width='100%'>"));
        pdn_f.Controls.Add(new LiteralControl("<tr>"));
        pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
        pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
        pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
        pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
        pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
        pdn_f.Controls.Add(new LiteralControl("</tr>"));




        Query = "select c.id,c.username,c.photo";
        Query += " from user_friendship as a";
        Query += " inner join user_login as b on b.id=a.first_uid";
        Query += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query += " where b.id=" + ict_h.Table.Rows[0]["id"].ToString();
        Query += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc.select_cmd(Query);

        List<friend_list> fri = new List<friend_list>();
        friend_list frii = new friend_list();

        for (int i = 0; i < ict_f.Count; i++)
        {
            frii = new friend_list();
            frii.id =Convert.ToInt32( ict_f.Table.Rows[i]["id"].ToString());
            frii.photo = ict_f.Table.Rows[i]["photo"].ToString();
            frii.username = ict_f.Table.Rows[i]["username"].ToString();
            fri.Add(frii);
        }

        Query = "select b.id,b.username,b.photo";
        Query += " from user_friendship as a";
        Query += " inner join user_login as b on b.id=a.first_uid";
        Query += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query += " where c.id=" + ict_h.Table.Rows[0]["id"].ToString();
        Query += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc.select_cmd(Query);

        for (int i = 0; i < ict_f1.Count; i++)
        {
            frii = new friend_list();
            frii.id = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
            frii.photo = ict_f1.Table.Rows[i]["photo"].ToString();
            frii.username = ict_f1.Table.Rows[i]["username"].ToString();
            fri.Add(frii);
        }




        pdn_f.Controls.Add(new LiteralControl("<tr>"));
        pdn_f.Controls.Add(new LiteralControl("<td class='space'></td>"));
        int iff = 0;
        for (int i = 0; i < fri.Count; i++)
        {
            pdn_f.Controls.Add(new LiteralControl("<td width='42%'>"));

            pdn_f.Controls.Add(new LiteralControl("<table width='100%' style='border-style: solid; border-width: thin;'>"));
            pdn_f.Controls.Add(new LiteralControl("<tr>"));
            pdn_f.Controls.Add(new LiteralControl("<td width='45%'>"));
            //friend photo
            pdn_f.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

            string cutstr2 = fri[i].photo.ToString();
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            pdn_f.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + fri[i].username.ToString() + "' style='width:100%;height:100px;'>"));
            pdn_f.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='100%' height='100px' />"));
            pdn_f.Controls.Add(new LiteralControl("</a>"));

            pdn_f.Controls.Add(new LiteralControl("</div>"));
            pdn_f.Controls.Add(new LiteralControl("</td>"));
            pdn_f.Controls.Add(new LiteralControl("<td width='40%' align='left' valign='middle'>"));
            HyperLink hyy = new HyperLink();
            hyy.NavigateUrl = "~/user_home_friend.aspx?=" + fri[i].id.ToString();
            hyy.Target = "_blank";
            hyy.Text = fri[i].username.ToString();
            hyy.Font.Underline = false;
            pdn_f.Controls.Add(hyy);
            pdn_f.Controls.Add(new LiteralControl("<br/>"));

            Query = "select c.id,c.username,c.photo";
            Query += " from user_friendship as a";
            Query += " inner join user_login as b on b.id=a.first_uid";
            Query += " inner join user_login as c on c.id=a.second_uid";
            Query += " where b.id=" + fri[i].id.ToString();
            Query += " and first_check_connect=1 and second_check_connect=1;";
            DataView ict_f2 = gc.select_cmd(Query);
            int how_many_f = ict_f2.Count;

            Query = "select b.id,b.username,b.photo";
            Query += " from user_friendship as a";
            Query += " inner join user_login as b on b.id=a.first_uid";
            Query += " inner join user_login as c on c.id=a.second_uid";
            Query += " where c.id=" + fri[i].id.ToString();
            Query += " and first_check_connect=1 and second_check_connect=1;";
            ict_f2 = gc.select_cmd(Query);
            how_many_f += ict_f2.Count;

            Label lab = new Label();
            lab.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            lab.Text = "友達"+how_many_f+"人";
            pdn_f.Controls.Add(lab);

            pdn_f.Controls.Add(new LiteralControl("</td>"));
            pdn_f.Controls.Add(new LiteralControl("<td width='15%' align='center' valign='middle'>"));

            Button but = new Button();
            but.Text = "✔友達";
            but.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            pdn_f.Controls.Add(but);


            pdn_f.Controls.Add(new LiteralControl("</td>"));
            pdn_f.Controls.Add(new LiteralControl("</tr>"));
            pdn_f.Controls.Add(new LiteralControl("</table>"));


            pdn_f.Controls.Add(new LiteralControl("</td>"));
            if (i % 2 == 1)
            {
                pdn_f.Controls.Add(new LiteralControl("<td class='space'></td>"));
                pdn_f.Controls.Add(new LiteralControl("</tr>"));
                iff += 1;
                pdn_f.Controls.Add(new LiteralControl("<tr>"));
                pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
                pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
                pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
                pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
                pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
                pdn_f.Controls.Add(new LiteralControl("</tr>"));
                pdn_f.Controls.Add(new LiteralControl("<tr>"));
                pdn_f.Controls.Add(new LiteralControl("<td class='space'></td>"));
            }
            else
            {
                pdn_f.Controls.Add(new LiteralControl("<td class='space'></td>"));
            }
        }

        if ((fri.Count - (2 * iff)) % 2 == 1)
        {
            pdn_f.Controls.Add(new LiteralControl("<td width='42%'></td>"));
            pdn_f.Controls.Add(new LiteralControl("<td class='space'></td>"));
            pdn_f.Controls.Add(new LiteralControl("</tr>"));
            pdn_f.Controls.Add(new LiteralControl("<tr>"));
            pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
            pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
            pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
            pdn_f.Controls.Add(new LiteralControl("<td width='42%' height='5%'><br/></td>"));
            pdn_f.Controls.Add(new LiteralControl("<td class='space' height='5%'><br/></td>"));
            pdn_f.Controls.Add(new LiteralControl("</tr>"));
        }


        //pdn_f.Controls.Add(new LiteralControl("</tr>"));
        pdn_f.Controls.Add(new LiteralControl("</table>"));



        p_head = (Panel)FindControl("homepage_user4");
        //select month
        int thismonth = DateTime.Now.Month;
        int thisyear = DateTime.Now.Year;
        //this_month_HiddenField.Value = thismonth.ToString();
        //this_year_HiddenField.Value = thisyear.ToString();
        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100px' style='border-style: solid; border-width: thin; background-color: #DDDDDD;'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));

        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%' align='center'>"));
        laa = new Label();
        laa.ID = "thisyear";
        laa.Text = thisyear.ToString() + "年";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);
        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));


        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' align='right'>"));


        p_head.Controls.Add(new LiteralControl("<img src='images/left.png' id='before_month_img' alt='before month' style='width:40px;height:40px;cursor:pointer;background-color:#DDDDDD;'>"));


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%'>"));

        //month select

        p_head.Controls.Add(new LiteralControl("<table width='100%' height='100%' align='center' valign='middle'>"));
        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' align='center' valign='middle'>"));

        laa = new Label();
        laa.ID = "before_month";
        laa.Text = (thismonth - 1) + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='40%' align='center' valign='middle'>"));

        laa = new Label();
        laa.ID = "this_month";
        laa.Text = thismonth + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='30%' align='center' valign='middle'>"));


        laa = new Label();
        laa.ID = "after_month";
        laa.Text = (thismonth + 1) + "月";
        laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        p_head.Controls.Add(laa);


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));


        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%'>"));



        p_head.Controls.Add(new LiteralControl("<img src='images/right.png' id='after_month_img' alt='after month' style='width:40px;height:40px;cursor:pointer;background-color:#DDDDDD;'>"));

        p_head.Controls.Add(new LiteralControl("</td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' align='left' valign='top'>"));
        //カレンダー
        p_head.Controls.Add(new LiteralControl("<input type='text' id='datepicker_boo' placeholder='09/2016' style='visibility:hidden;' readonly>"));

        p_head.Controls.Add(new LiteralControl("</td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("</tr>"));


        p_head.Controls.Add(new LiteralControl("<tr>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='45%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("<td width='15%' height='10%'></td>"));
        p_head.Controls.Add(new LiteralControl("<td width='5%' height='10%'></td>"));

        p_head.Controls.Add(new LiteralControl("</tr>"));
        p_head.Controls.Add(new LiteralControl("</table>"));


        Panel date_manger_month = new Panel();
        date_manger_month.ID = "date_manger_month";
        p_head.Controls.Add(date_manger_month);


        date_manger_month.Controls.Add(new LiteralControl("<table width='100%' height='100%' style='background-color: #DDDDDD;'>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%'>"));
        Panel date_manger_month_group = new Panel();
        date_manger_month_group.ID = "date_manger_month_group";
        date_manger_month.Controls.Add(date_manger_month_group);
        date_manger_month.Controls.Add(new LiteralControl("</td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<tr>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='80%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("<td width='10%' height='10%'></td>"));
        date_manger_month.Controls.Add(new LiteralControl("</tr>"));
        date_manger_month.Controls.Add(new LiteralControl("</table>"));




        //update information

        KidGroup1.Controls.Clear();

        Query = "select id,real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,Sex,birthday_year,birthday_month,birthday_day,school_name,hospital_name,sick_name from user_information_school_children";
        Query += " where uid='" + Session["id"].ToString() + "';";
        ict_f = gc.select_cmd(Query);
        li = new Literal();
        li.Text = "";
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                HiddenField hid = new HiddenField();
                hid.ID = "kid" + i;
                hid.Value = ict_f.Table.Rows[i]["id"].ToString();
                KidGroup1.Controls.Add(hid);
                li = new Literal();
                li.Text = @"<div id='KidDiv_" + i + @"'>

                <table style='width:100%;'>
                                        <tr>
                                            <td width='40%' valign='top'>
                                                <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                                <span>お名前</span>
                                        </td>
                                              <td width='50%' valign='bottom'>
                                        <span style ='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                  </td>
                                             </tr>
                                             </table>
                                            </td>
                                            <td width='60%'>
                                                <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                            <input id='real_first_name_text" + i + @"' type='text' name='real_first_name_text" + i + @"' class='textbox' placeholder='姓' style='height:20px;width:90%;' value='" + ict_f.Table.Rows[i]["real_first_name"].ToString() +@"' />
                                    <br /><br /><span id='real_first_name_la" + i + @"' style='color: #FF0000'></span>
                                            </td>
                                        <td width='50%'>
                                <input id='real_second_name_text" + i + @"' type='text' name='real_second_name_text" + i + @"' class='textbox' placeholder='名' style='height:20px;width:90%;' value='" + ict_f.Table.Rows[i]["real_second_name"].ToString() + @"' />
                                    <br /><br /><span id='real_second_name_la" + i + @"' style='color: #FF0000;'></span>
                                            </td>
                                    </tr>
                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='40%' valign='top'>
                                               <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                                <span>フリガナ</span>
                                        </td>
                                              <td width='50%' valign='bottom'>
                                        <span style ='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                  </td>
                                             </tr>
                                             </table>

                                            </td>
                                            <td width='60%'>
                                                 <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                            <input id='real_spell_first_name_text" + i + @"' type='text' name='real_spell_first_name_text" + i + @"' class='textbox' placeholder='セイ' style='height:20px;width:90%;' value='" + ict_f.Table.Rows[i]["real_spell_first_name"].ToString() + @"' />
                                    <br /><br /><span id='real_spell_first_name_la" + i + @"' style='color: #FF0000;'></span>
                                            </td>
                                        <td width='50%'>
                                <input id='real_spell_second_name_text" + i + @"' type='text' name='real_spell_second_name_text" + i + @"' class='textbox' placeholder='メイ' style='height:20px;width:90%;' value='" + ict_f.Table.Rows[i]["real_spell_second_name"].ToString() + @"' />
                                    <br /><br /><span id='real_spell_second_name_la" + i + @"' style='color: #FF0000;'></span>
                                            </td>
                                    </tr>
                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='40%' valign='top'>
                                                <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                                <span>性别</span>
                                        </td>
                                              <td width='50%' valign='bottom'>
                                        <span style ='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                  </td>
                                             </tr>
                                             </table>

                                            </td>
                                            <td width='60%'>";

                if( ict_f.Table.Rows[i]["Sex"].ToString()=="0")
                {
                    li.Text+=@"<input type='radio' name='sex" + i + @"' value='Girl' checked>女性
                                                <input type='radio' name='sex" + i + @"' value='Boy'>男性
                                                <br />";
                }else
                {
                    li.Text+=@"<input type='radio' name='sex" + i + @"' value='Girl'>女性
                                                <input type='radio' name='sex" + i + @"' value='Boy' checked>男性
                                                <br />";
                }

                                                li.Text+=@"<br /><span id='sex_la" + i + @"' style='color: #FF0000;'></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='40%' valign='center'>
                                                <table style='width:100%;'>
                                    <tr>
                                        <td width='50%'>
                                                <span>生年月日</span>
                                        </td>
                                              <td width='50%' valign='bottom'>
                                        <span style ='color: #FF5050; font-size: XX-Small;'>※必須</span>
                                                  </td>
                                             </tr>
                                             </table>
                                                <br />
                                            </td>
                                            <td width='60%' valign='center'>";
                                                string date_str = ict_f.Table.Rows[i]["birthday_year"].ToString()+"/";
                                                int mon = Convert.ToInt32(ict_f.Table.Rows[i]["birthday_month"].ToString());

                                                if (mon < 10)
                                                {
                                                    date_str += "0"+mon + "/";
                                                }
                                                else
                                                {
                                                    date_str += ict_f.Table.Rows[i]["birthday_month"].ToString() + "/";
                                                }

                                                mon = Convert.ToInt32(ict_f.Table.Rows[i]["birthday_day"].ToString());

                                                if (mon < 10)
                                                {
                                                    date_str += "0" + mon ;
                                                }
                                                else
                                                {
                                                    date_str += ict_f.Table.Rows[i]["birthday_day"].ToString();
                                                }


                                                li.Text += @" <p><input type='text' name='datepicker" + i + @"' id='datepicker" + i + @"' class='textbox' placeholder='2016/01/01' value='" + date_str + @"' readonly></p>
                                                <script>
                                                    $(function () {
                                                        $('#datepicker" + i + @"').datepicker({
                                                            format: 'yyyy/mm/dd',
                                                            language: 'ja',
                                                            changeMonth: true,
                                                            changeYear: true,
                                                            autoclose: true, // これ
                                                            clearBtn: true,
                                                            clear: '閉じる'
                                                        });
                                                    });
                                                </script>
                                <br /><span id='date_la" + i + @"' style='color: #FF0000;'></span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <span>保育園/学校名</span>
                                <br />
                                <br />
                                            </td>
                                            <td>
                                                <input name='school_name_text" + i + @"' type='text' id='school_name_text" + i + @"' class='textbox' placeholder='通っている保育園/病院を入力' style='height:20px;width:100%;' value='" + ict_f.Table.Rows[i]["school_name"].ToString() + @"'>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <span>かかりつけ医院名</span>
                                <br />
                                <br />
                                            </td>
                                            <td>
                                                <input name='hospital_name_text" + i + @"' type='text' id='hospital_name_text" + i + @"' class='textbox' placeholder='病院名/診療所名を入力' style='height:20px;width:100%;' value='" + ict_f.Table.Rows[i]["hospital_name"].ToString() + @"'>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <span>アレルギー/持病</span>
                                <br />
                                <br />
                                            </td>
                                            <td>
                                                <textarea name='sick_name_text" + i + @"' rows='2' cols='20' wrap='off' id='sick_name_text" + i + @"' class='textbox' placeholder='例小麦アレルキーなど' style='height:50px;width:100%;'>" + ict_f.Table.Rows[i]["sick_name"].ToString() + @"</textarea>
                                            </td>
                                        </tr>
                                    </table>
</div><hr/>";
                //name_TextBox.Text = ict_f.Table.Rows[0]["username"].ToString();
                KidGroup1.Controls.Add(li);
            }
            HiddenField hidd = new HiddenField();
            hidd.ID = "Icounter";
            hidd.Value = "" + ict_f.Count;
            KidGroup1.Controls.Add(hidd);
            //KidGroup.Controls.Add(li);
        }
        //update information



    }

    public class sorttalk
    {
        public int id = 0;
        public int point_id = 0;
        public int level = 0;
        public string mess = "";
        public string filename = "";
        public int uid = 0;
        public string username = "";
        public string photo = "";
    }
    public class friend_list
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
    }
    public class friend_list_chat
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
        public int year = 0;
        public int month = 0;
        public int day = 0;
        public int hour = 0;
        public int min = 0;
        public int sec = 0;
        public string mesg = "";
        public DateTime comdate = new DateTime();
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
                        SqlDataSource sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                        filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;

                        //AmazonUpload aws = new AmazonUpload();
                        //string imgurl = aws.AmazonUpload_file("", "upload/test", filename, postedFile.InputStream);

                        Google.Apis.Auth.OAuth2.GoogleCredential credential = GCP_AUTH.AuthExplicit();
                        string imgurl = GCP_AUTH.upload_file_stream("", "upload/test", filename, postedFile.InputStream, credential);

                        //sql_insert.InsertCommand = "insert into filename_detail(filename,name)";
                        //sql_insert.InsertCommand += " values('~/fileplace/" + filename + "','" + fuDocument.FileName.ToString() + "')";
                        //sql_insert.Insert();

                        //upload_finish_files.Text += postedFile.FileName.ToString() + "  finish<br>";

                        //postedFile.SaveAs(Server.MapPath("fileplace") + "\\" + filename);

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = imgurl;
                        this.Panel1.Controls.Add(im);

                        image_HiddenField.Value += ",~/" + imgurl;

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






        }
    }
    //protected void UploadDocument1(object sender, EventArgs e)
    //{
    //    string input = "", DirRoot = "", filename = "";
    //    int stringindex = 0, cut = 0;
    //    Boolean check = false;
    //    Session["head_photo"] = null;
    //    if (fuDocument1.HasFile)
    //    {
    //        input = fuDocument1.FileName;
    //        stringindex = input.LastIndexOf(@".");
    //        cut = input.Length - stringindex;
    //        DirRoot = input.Substring(stringindex + 1, cut - 1);

    //        SqlDataSource2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
    //        SqlDataSource2.SelectCommand = "select id,name from filename_extension";
    //        SqlDataSource2.DataBind();
    //        DataView ou1 = (DataView)SqlDataSource2.Select(DataSourceSelectArguments.Empty);
    //        for (int i = 0; i < ou1.Count; i++)
    //        {
    //            if (DirRoot.ToUpper() == ou1.Table.Rows[i]["name"].ToString().ToUpper())
    //            {
    //                check = true;
    //            }
    //        }
    //        if (check)
    //        {
    //            filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + @"." + DirRoot;
    //            fuDocument1.SaveAs(Server.MapPath("head_photo") + "\\" + filename);
    //            Image1.ImageUrl = "~/head_photo/" + filename;

    //            Session["head_photo"] = "~/head_photo/" + filename;
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(fuDocument1, fuDocument1.GetType(), "alert", "alert('filename extension is not in role!')", true);
    //        }


    //    }
    //}
    protected void change_panel(object sender, EventArgs e)
    {
        Button temp = (Button)sender;
        Panel pan;
        pan = (Panel)this.FindControl("homepage_user");
        pan.Visible = false;
        pan = (Panel)this.FindControl("homepage_user2");
        pan.Visible = false;
        pan = (Panel)this.FindControl("homepage_user3");
        pan.Visible = false;
        pan = (Panel)this.FindControl("homepage_user4");
        pan.Visible = false;
        Button buti;
        if (temp.ID == "home_h2")
        {
            pan = (Panel)this.FindControl("homepage_user");
            pan.Visible = true;
            temp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            temp.Font.Bold = true;
            buti = (Button)this.FindControl("home_h3");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h4");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h5");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            send_homepage_user.Visible = true;
        }
        else if (temp.ID == "home_h3")
        {
            pan = (Panel)this.FindControl("homepage_user2");
            pan.Visible = true;
            temp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            temp.Font.Bold = true;
            buti = (Button)this.FindControl("home_h2");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h4");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h5");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;

            //update information
            Query = "select username from user_login";
            Query += " where id='" + Session["id"].ToString() + "';";
            DataView ict_f = gc.select_cmd(Query);
            if (ict_f.Count > 0)
            {
                name_TextBox.Text = ict_f.Table.Rows[0]["username"].ToString();
            }
            Query = "select id,place,postal_code from user_login_address";
            Query += " where uid='" + Session["id"].ToString() + "';";
            ict_f = gc.select_cmd(Query);
            string totalstr = "";
            if (ict_f.Count > 0)
            {
                for (int i = 0; i < ict_f.Count; i++)
                {
                    totalstr += "DBplace('" + ict_f.Table.Rows[i]["postal_code"].ToString() + "','" + ict_f.Table.Rows[i]["place"].ToString() + "');";
                    //ScriptManager.RegisterStartupScript(update_Button1, update_Button1.GetType(), "DBplace", "DBplace('" + ict_f.Table.Rows[i]["postal_code"].ToString() + "','" + ict_f.Table.Rows[i]["place"].ToString() + "');", true);

                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "DBplace", totalstr, true);
            }

            //update information
            send_homepage_user.Visible = false;
        }
        else if (temp.ID == "home_h4")
        {
            pan = (Panel)this.FindControl("homepage_user3");
            pan.Visible = true;
            temp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            temp.Font.Bold = true;
            buti = (Button)this.FindControl("home_h3");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h2");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h5");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            send_homepage_user.Visible = false;
        }
        else if (temp.ID == "home_h5")
        {
            pan = (Panel)this.FindControl("homepage_user4");
            pan.Visible = true;
            temp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            temp.Font.Bold = true;
            buti = (Button)this.FindControl("home_h3");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h4");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            buti = (Button)this.FindControl("home_h2");
            buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            buti.Font.Bold = false;
            send_homepage_user.Visible = false;
        }
        else if (temp.ID == "newbtn1")
        {
            Response.Redirect("registered.aspx");
        }
        else if (temp.ID == "newbtn2")
        {
            Response.Redirect("registered.aspx");
        }

    }
    protected void userhome_Button1_Click(object sender, EventArgs e)
    {
        Panel pa = (Panel)this.FindControl("user_information");
        pa.Visible = true;
        pa = (Panel)this.FindControl("user_information2");
        pa.Visible = false;
        pa = (Panel)this.FindControl("user_information3");
        pa.Visible = false;

        Button buti = (Button)this.FindControl("userhome_Button1");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        buti = (Button)this.FindControl("userhome_Button2");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        buti = (Button)this.FindControl("userhome_Button3");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
    }
    protected void userhome_Button2_Click(object sender, EventArgs e)
    {
        Panel pa = (Panel)this.FindControl("user_information2");
        pa.Visible = true;
        pa = (Panel)this.FindControl("user_information");
        pa.Visible = false;
        pa = (Panel)this.FindControl("user_information3");
        pa.Visible = false;

        Button buti = (Button)this.FindControl("userhome_Button2");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        buti = (Button)this.FindControl("userhome_Button1");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        buti = (Button)this.FindControl("userhome_Button3");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
    }
    protected void userhome_Button3_Click(object sender, EventArgs e)
    {
        Panel pa = (Panel)this.FindControl("user_information3");
        pa.Visible = true;
        pa = (Panel)this.FindControl("user_information2");
        pa.Visible = false;
        pa = (Panel)this.FindControl("user_information");
        pa.Visible = false;

        Button buti = (Button)this.FindControl("userhome_Button3");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        buti = (Button)this.FindControl("userhome_Button1");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        buti = (Button)this.FindControl("userhome_Button2");
        buti.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
    }
    protected void post_message_Button_Click(object sender, EventArgs e)
    {
        if (Session["id"] != null || Session["id"].ToString() != "")
        {
            string id = Session["id"].ToString();
            string message = post_message_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");
            string message_type = Request.Form["post_type"].ToString();
            if (message_type == "")
            {
                message_type = "6";
            }
            string type = "";
            if (type_div.Value == "一般公開")
            {
                type = "0";
            }
            else if (type_div.Value == "地域限定")
            {
                type = "1";
            }
            else if (type_div.Value == "友達")
            {
                type = "2";
            }
            else if (type_div.Value == "")
            {
                type = "0";
            }

            string place = place_va.Value;
            string postal_code = postcode_HiddenField.Value;
            if (postal_code == "")
            {
                Query = "select postal_code from user_login_address";
                Query += " where uid='" + id + "';";
                DataView ict_f_sel = gc.select_cmd(Query);
                if (ict_f_sel.Count > 0)
                {
                    postal_code = ict_f_sel.Table.Rows[0]["postal_code"].ToString();
                }
            }

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
            string min = DateTime.Now.Minute.ToString();
            string sec = DateTime.Now.Second.ToString();

            if (postal_code != "")
            {
                if (post_message_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() != "")
                {
                    //ScriptManager.RegisterStartupScript(post_message_Button, post_message_Button.GetType(), "alert", "alert('@" + place + "')", true);

                    Query = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second,postal_code)";
                    Query += " values('" + id + "','" + type + "','" + message_type + "','" + place.Trim() + "','" + message + "','" + year + "','" + month + "'";
                    Query += ",'" + day + "','" + hour + "','" + min + "','" + sec + "','" + postal_code + "')";
                    resin = gc.insert_cmd(Query);

                    string upid = "";

                    Query = "select id from status_messages";
                    Query += " where uid='" + id + "' and type='" + type + "' and message_type='" + message_type + "' and place='" + place + "'";
                    Query += " and message='" + message + "' and year='" + year + "' and month='" + month + "' and day='" + day + "' and hour='" + hour + "'";
                    Query += " and minute='" + min + "' and second='" + sec + "' and postal_code='" + postal_code + "';";

                    DataView ict_f = gc.select_cmd(Query);
                    if (ict_f.Count > 0)
                    {
                        upid = ict_f.Table.Rows[0]["id"].ToString();
                        string image_path = image_HiddenField.Value;

                        string me = @",";
                        List<int> image_find = new List<int>();

                        Regex ItemRegex = new Regex(me, RegexOptions.Compiled);
                        foreach (Match ItemMatch in ItemRegex.Matches(image_path))
                        {
                            image_find.Add(ItemMatch.Index);
                        }

                        int len = 0; string naa = "";
                        SqlDataSource sql_insert;
                        if (image_find.Count > 0)
                        {
                            for (int i = 0; i < image_find.Count - 1; i++)
                            {
                                len = image_find[i + 1] - image_find[i];
                                naa = image_path.Substring(image_find[i] + 1, len - 1);
                                Query = "insert into status_messages_image(smid,filename)";
                                Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','~/" + naa + "');";
                                resin = gc.insert_cmd(Query);

                            }
                            len = image_path.Length - image_find[image_find.Count - 1];
                            naa = image_path.Substring(image_find[image_find.Count - 1] + 1, len - 1);

                            Query = "insert into status_messages_image(smid,filename)";
                            Query += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','~/" + naa + "');";
                            resin = gc.insert_cmd(Query);
                        }

                        post_message_TextBox.Text = "";
                        image_HiddenField.Value = "";
                    }

                    if (upid != "")
                    {
                        //GPS
                        string place_lat = lat_HiddenField.Value;
                        string place_lng = lng_HiddenField.Value;
                        if (place_lat != null && place_lng != null)
                        {
                            if (place_lat != "" && place_lng != "")
                            {
                                Query = "update status_messages set place_lat='" + place_lat + "',place_lng='" + place_lng + "'";
                                Query += " where id='" + upid + "';";
                                resin = gc.update_cmd(Query);
                            }
                        }
                    }
                    ConvertUrlsInData(message);
                    Response.Redirect("user_home.aspx");
                }
            }

        }
    }
    public class URL_data
    {
        public string url = "";
        public string image_url = "";
        public string title = "";
        public string des = "";
        public string update_time = "";
    }
    public static void ConvertUrlsToInData(string url)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        WebService.LinkDetails wss = new WebService.LinkDetails();
        WebService ws = new WebService();
        wss = ws.GetDetails(url);
        string imgurl = "";
        if (wss.Image != null)
        {
            imgurl = wss.Image.Url;
        }
        else if (wss.Images != null)
        {
            if (wss.Images.Count > 0)
            {
                imgurl = wss.Images[0].Url;
            }
        }
        URL_data urld = new URL_data();
        urld.url = wss.Url;
        if (wss.Title != null)
        {
            urld.title = wss.Title;
        }
        if (wss.Description != null)
        {
            urld.des = wss.Description;
        }

        if (imgurl != "")
        {
            if (UrlExists(imgurl))
            {
                urld.image_url = imgurl;
            }
        }



        Query1 = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
        Query1 += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";
        resin = gc1.insert_cmd(Query1);

    }
    public static void ConvertUrlsInData(string msg)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {

            Query1 = "select id from status_messages_link_info where link like '" + match.Value + "';";
            DataView ict1 = gc1.select_cmd(Query1);
            if (ict1.Count == 0)
            {
                ConvertUrlsToInData(match.Value);
            }
        }
    }
    [WebMethod(EnableSession = true)]
    public static string Save_place(string param1, string param2)
    {
        string result = param1 + "," + param2;
         //ver 1
        HttpContext.Current.Session["place_postalcode"] = param1 + ",";
        HttpContext.Current.Session["place_add"] = param2 + ",";

        ////ver 2
        //HttpContext.Current.Session["place_postalcode"] += param1+",";
        //HttpContext.Current.Session["place_add"] += param2 + ",";
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string Save_first(string param1)
    {
        string result = "";
        HttpContext.Current.Session["place_postalcode"] = "";
        HttpContext.Current.Session["place_add"] = "";
        return result;
    }
    protected void update_Button1_Click(object sender, EventArgs e)
    {
        if (Session["id"] != null)
        {
            string id = Session["id"].ToString();
            string name = name_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            result_Label0.Text = "Fail.";
            if (name != "")
            {
                name_Label.Text ="";

                Query = "update user_login";
                Query += " set username='" + name + "'";
                Query += " where id='" + id + "';";
                resin = gc.update_cmd(Query);


                if (HiddenField_for_photo.Value != "")
                {

                    Query = "update user_login";
                    Query += " set photo='~/" + HiddenField_for_photo.Value + "'";
                    Query += " where id='" + id + "';";
                    resin = gc.update_cmd(Query);
                }
                if (Session["place_add"] != null && Session["place_postalcode"] != null)
                {

                    Regex r = new Regex(@",");
                    string add_str = Session["place_add"].ToString();
                    // Match the regular expression pattern against a text string.
                    Match m = r.Match(add_str);
                    List<int> indexlist = new List<int>();
                    while (m.Success)
                    {
                        indexlist.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> addlist = new List<string>();
                    for (int i = 0; i < indexlist.Count; i++)
                    {
                        string add = "";
                        if (i - 1 < 0)
                        {
                            add = add_str.Substring(0, indexlist[i]);
                            addlist.Add(add);
                        }
                        else
                        {
                            add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                            addlist.Add(add);
                        }
                    }


                    add_str = Session["place_postalcode"].ToString();
                    // Match the regular expression pattern against a text string.
                    m = r.Match(add_str);
                    indexlist = new List<int>();
                    while (m.Success)
                    {
                        indexlist.Add(m.Index);
                        m = m.NextMatch();
                    }
                    List<string> addlist1 = new List<string>();
                    for (int i = 0; i < indexlist.Count; i++)
                    {
                        string add = "";
                        if (i - 1 < 0)
                        {
                            add = add_str.Substring(0, indexlist[i]);
                            addlist1.Add(add);
                        }
                        else
                        {
                            add = add_str.Substring(indexlist[i - 1] + 1, indexlist[i] - indexlist[i - 1] - 1);
                            addlist1.Add(add);
                        }
                    }
                    if (addlist1.Count > 0)
                    {

                        Query = "Delete from user_login_address";
                        Query += " where uid='" + id + "';";
                        resin = gc.delete_cmd(Query);

                        for (int i = 0; i < addlist.Count; i++)
                        {

                            Query = "insert into user_login_address(uid,place,postal_code)";
                            Query += " values('" + id + "','" + addlist[i] + "','" + addlist1[i] + "')";
                            resin = gc.insert_cmd(Query);
                        }
                    }

                }
                result_Label0.Text = "Success.";
            }
            else
            {
                name_Label.Text = "未記入もしくは使用できない単語です。";
            }

            //update information



        }
    }
    protected void update_Button2_Click(object sender, EventArgs e)
    {

    }
    protected void update_Button3_Click(object sender, EventArgs e)
    {
        if (Session["id"] != null)
        {
            string id = Session["id"].ToString();
            string now_password = password_TextBox_now.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string password = password_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string c_password = password_TextBox_check.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            bool check_password = false, check_c_password = false, check_password_same = false, check_password_now = false;
            result_Label.Text = "Fail.";
            if (now_password != "")
            {
                check_password_now = true;
                now_password_Label.Text = "";
            }
            else
            {
                check_password_now = false;
                now_password_Label.Text = "Your now password have special word or not write.";
            }

            if (password != "")
            {
                check_password = true;
                password_Label.Text = "";
            }
            else
            {
                check_password = false;
                password_Label.Text = "Your new password have special word or not write.";
            }
            if (c_password != "")
            {
                check_c_password = true;
                c_password_Label.Text = "";
            }
            else
            {
                check_c_password = false;
                c_password_Label.Text = "パスワードが間違っています。";
            }
            if (check_password && check_c_password && check_password_now)
            {
                if (c_password == password)
                {
                    check_password_same = true;
                    c_password_Label.Text = "";
                }
                else
                {
                    check_password_same = false;
                    c_password_Label.Text = "Your confirm password is not equal new password.";
                }
                if (check_password_same)
                {
                    Query = "select login_password from user_login";
                    Query += " where id='" + id + "' and login_password='" + now_password + "';";
                    DataView ict_i_f = gc.select_cmd(Query);
                    if (ict_i_f.Count > 0)
                    {
                        Query = "update user_login";
                        Query += " set login_password='" + password + "'";
                        Query += " where id='" + id + "';";
                        resin = gc.update_cmd(Query);
                        result_Label.Text = "Success.";
                    }
                }
            }
        }



    }

    [WebMethod]
    public static string search_time_u(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result1 = "<td>" + param1 + "</td>" + param2;

        //        select c.money_hour,c.hour,c.total_money,c.commission,a.check_success,e.year,e.month,e.day,e.end_hour,e.end_minute
        //from user_information_appointment_check_deal as a
        //inner join user_information_appointment_check_connect_deal as b
        //on a.id=b.uiacdid
        //inner join user_appointment as c
        //on b.uaid=c.id
        //inner join appointment as e
        //on c.appid=e.id
        //where a.suppid='10' and a.check_success='1' and e.month='9' and e.year='2016'

        string result = "";

        List<date_list> dllist = new List<date_list>();
        date_list dl = new date_list();

        Query1 = "select a.id,c.appid,c.uid,f.photo,f.username,a.type,a.check_success,e.year,e.month,e.day,e.start_hour,e.start_minute,e.end_hour,e.end_minute,c.howtoget_there from user_information_appointment_check_deal as a";
        Query1 += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        Query1 += " inner join user_appointment as c on b.uaid=c.id";
        Query1 += " inner join appointment as e on c.appid=e.id ";
        Query1 += " inner join user_login as f on f.id=a.suppid ";
        Query1 += " where a.uid='" + param3 + "' and e.year= '" + param1 + "' and e.month= '" + param2 + "'";
        Query1 += " order by e.day asc,a.first_check_time asc,c.uid asc;";

        DataView ict_f = gc1.select_cmd(Query1);

        string cutstr_h = "", cutstr_h1 = "", week = "";
        int ind_h = 0;
        DateTime todate;
        string temp_appid = "", temp_uid = "", button_name = "";
        bool check_same = false;
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                check_same = false;
                result = "";
                if (i == 0)
                {
                    temp_appid = ict_f.Table.Rows[i]["appid"].ToString();
                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                    check_same = true;
                }
                else
                {
                    if (temp_appid != ict_f.Table.Rows[i]["appid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                    {
                        temp_appid = ict_f.Table.Rows[i]["appid"].ToString();
                        temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                        check_same = true;
                    }
                }
                if (check_same)
                {
                    dl = new date_list();

                    todate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()));
                    dl.date = todate;
                    if (todate.DayOfWeek == DayOfWeek.Monday)
                    {
                        week = "月曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        week = "火曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        week = "水曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Thursday)
                    {
                        week = "木曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Friday)
                    {
                        week = "金曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        week = "土曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        week = "日曜日";
                    }

                    result += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result += "<tr>";

                    result += "<td width='5%'>";
                    result += "</td>";

                    result += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + ict_f.Table.Rows[i]["month"].ToString() + " 月 " + ict_f.Table.Rows[i]["day"].ToString() + " 日, ";
                    result += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                    result += "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/><br/>";
                    result += "</td>";
                    result += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                    result += "<table width='100%'>";
                    result += "<tr>";
                    result += "<td width='10%'></td>";
                    result += "<td width='20%' valign='top'>";
                    //user photo
                    result += "<div class='zoom-gallery'>";
                    cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                    ind_h = cutstr_h.IndexOf(@"/");
                    cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                    result += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                    result += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                    result += "</a>";
                    result += "</div>";
                    result += "</td>";
                    result += "<td width='70%'>";
                    result += "<span style='color: blue;'>";
                    result += ict_f.Table.Rows[i]["username"].ToString();
                    result += "</span><br/>";
                    result += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";

                    result += "</td>";
                    result += "<td width='20%' align='center'>";
                    if (ict_f.Table.Rows[i]["check_success"].ToString() == "0")
                    {
                        button_name = "依頼中";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "1")
                    {
                        button_name = "予約確定";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "2")
                    {
                        button_name = "お断り";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "3")
                    {
                        button_name = "報告書の確認";
                        //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='check_report_page(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "4")
                    {
                        button_name = "評価をしよう";
                        //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='score_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "5")
                    {
                        button_name = "完了";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }


                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";
                    dl.element = result;
                    dllist.Add(dl);
                }
            }
        }
        //select one day

        //select 定期 one day
        Query1 = "select a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        Query1 += " from user_information_appointment_check_deal as a";
        Query1 += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        Query1 += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        Query1 += " inner join user_login as f on f.id=a.suppid ";
        Query1 += " where a.type='1' and a.uid='" + param3 + "' and YEAR(d.start_date)= '" + param1 + "' and MONTH(d.start_date)= '" + param2 + "'";
        Query1 += " order by DAY(d.start_date) asc,a.first_check_time asc,d.uid asc;";

        ict_f = gc1.select_cmd(Query1);

        string temp_uiswaid = "";
        if (ict_f.Count > 0)
        {
            for (int i = 0; i < ict_f.Count; i++)
            {
                result = "";
                check_same = false;
                if (i == 0)
                {
                    temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                    check_same = true;
                }
                else
                {
                    if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                    {
                        temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                        temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                        check_same = true;
                    }
                }
                if (check_same)
                {
                    dl = new date_list();
                    DateTime.TryParse(ict_f.Table.Rows[i]["start_date"].ToString(), out todate);
                    dl.date = todate;
                    if (todate.DayOfWeek == DayOfWeek.Monday)
                    {
                        week = "月曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        week = "火曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        week = "水曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Thursday)
                    {
                        week = "木曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Friday)
                    {
                        week = "金曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        week = "土曜日";
                    }
                    else if (todate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        week = "日曜日";
                    }

                    result += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result += "<tr>";

                    result += "<td width='5%'>";
                    result += "</td>";

                    result += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>単発</span><br/>";
                    result += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", " + todate.Month + " 月 " + todate.Day + " 日, ";
                    result += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";
                    result += "<br/><span>" + ict_f.Table.Rows[i]["howtoget_there"].ToString() + "</span><br/><br/>";
                    result += "</td>";
                    result += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                    result += "<table width='100%'>";
                    result += "<tr>";
                    result += "<td width='10%'></td>";
                    result += "<td width='20%' valign='top'>";
                    //user photo
                    result += "<div class='zoom-gallery'>";
                    cutstr_h = ict_f.Table.Rows[i]["photo"].ToString();
                    ind_h = cutstr_h.IndexOf(@"/");
                    cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                    result += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                    result += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                    result += "</a>";
                    result += "</div>";
                    result += "</td>";
                    result += "<td width='70%'>";
                    result += "<span style='color: blue;'>";
                    result += ict_f.Table.Rows[i]["username"].ToString();
                    result += "</span><br/>";
                    result += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";

                    result += "</td>";
                    result += "<td width='20%' align='center'>";
                    if (ict_f.Table.Rows[i]["check_success"].ToString() == "0")
                    {
                        button_name = "依頼中";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "1")
                    {
                        button_name = "予約確定";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "2")
                    {
                        button_name = "お断り";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "3")
                    {
                        button_name = "報告書の確認";
                        //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='check_report_page(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "4")
                    {
                        button_name = "評価をしよう";
                        //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' onclick='score_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    else if (ict_f.Table.Rows[i]["check_success"].ToString() == "5")
                    {
                        button_name = "完了";
                        result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    }
                    //if (ict_f.Table.Rows[i]["check_success"].ToString() == "0")
                    //{
                    //    button_name = "承認待ち";
                    //    result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    //}
                    //else if (ict_f.Table.Rows[i]["check_success"].ToString() == "1")
                    //{
                    //    button_name = "予約確定";
                    //    result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                    //    //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                    //}
                    result += "</td>";
                    result += "</tr>";
                    result += "</table>";
                    dl.element = result;
                    dllist.Add(dl);
                }
            }
        }

        //select 定期 more than one day
        Query1 = "select a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
        Query1 += " from user_information_appointment_check_deal as a";
        Query1 += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
        Query1 += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
        Query1 += " inner join user_login as f on f.id=a.suppid ";
        Query1 += " where a.type='2' and a.uid='" + param3 + "' and YEAR(d.start_date)= '" + param1 + "' and MONTH(d.start_date)= '" + param2 + "'";
        Query1 += " order by DAY(d.start_date) asc,a.first_check_time asc,a.id asc;";

        DataView ict_f1 = gc1.select_cmd(Query1);

        temp_uiswaid = "";
        string temp_id = "", result_2 = "";
        bool same = false;

        if (ict_f1.Count > 0)
        {
            result_2 = "";
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                same = false;
                if (ii == 0)
                {
                    temp_id = ict_f1.Table.Rows[ii]["id"].ToString();
                    same = true;

                    result_2 = "";
                    dl = new date_list();
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);
                    dl.date = todate;
                    result_2 += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                    result_2 += "<tr>";
                    result_2 += "<td width='5%'>";
                    result_2 += "</td>";
                    result_2 += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                    result_2 += "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                    result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";
                    DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                    result_2 += todate.Month + " 月 " + todate.Day + " 日</span><br/>";
                    //result += "</td>";


                }
                else
                {
                    if (temp_id != ict_f1.Table.Rows[ii]["id"].ToString())
                    {
                        result_2 += "<br/><span>" + ict_f1.Table.Rows[ii - 1]["howtoget_there"].ToString() + "</span><br/><br/>";
                        result_2 += "</td>";
                        result_2 += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

                        result_2 += "<table width='100%'>";
                        result_2 += "<tr>";
                        result_2 += "<td width='10%'></td>";
                        result_2 += "<td width='20%' valign='top'>";
                        //user photo
                        result_2 += "<div class='zoom-gallery'>";
                        cutstr_h = ict_f1.Table.Rows[ii - 1]["photo"].ToString();
                        ind_h = cutstr_h.IndexOf(@"/");
                        cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
                        result_2 += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ii - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
                        result_2 += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
                        result_2 += "</a>";
                        result_2 += "</div>";
                        result_2 += "</td>";
                        result_2 += "<td width='70%'>";
                        result_2 += "<span style='color: blue;'>";
                        result_2 += ict_f1.Table.Rows[ii - 1]["username"].ToString();
                        result_2 += "</span><br/>";
                        result_2 += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
                        result_2 += "</td>";
                        result_2 += "</tr>";
                        result_2 += "</table>";

                        result_2 += "</td>";
                        result_2 += "<td width='20%' align='center'>";
                        if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "0")
                        {
                            button_name = "依頼中";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "1")
                        {
                            button_name = "予約確定";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "2")
                        {
                            button_name = "お断り";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "3")
                        {
                            button_name = "報告書の確認";
                            //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ii - 1]["id"].ToString() + "' value='" + button_name + "' onclick='check_report_page(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "4")
                        {
                            button_name = "評価をしよう";
                            //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ii - 1]["id"].ToString() + "' value='" + button_name + "' onclick='score_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "5")
                        {
                            button_name = "完了";
                            result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                            //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        }
                        //if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "0")
                        //{
                        //    button_name = "承認待ち";
                        //    result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ii - 1]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        //}
                        //else if (ict_f1.Table.Rows[ii - 1]["check_success"].ToString() == "1")
                        //{
                        //    button_name = "予約確定";
                        //    result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                        //    //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
                        //}
                        result_2 += "</td>";
                        result_2 += "</tr>";
                        result_2 += "</table>";
                        dl.element = result_2;
                        dllist.Add(dl);


                        result_2 = "";

                        temp_id = ict_f1.Table.Rows[ii]["id"].ToString();
                        same = true;

                        dl = new date_list();
                        DateTime.TryParse(ict_f1.Table.Rows[ii]["start_date"].ToString(), out todate);
                        dl.date = todate;
                        result_2 += "<br/><table width='100%' style='border-style:solid;border-width: thin;background-color:#ffffff;'>";
                        result_2 += "<tr>";
                        result_2 += "<td width='5%'>";
                        result_2 += "</td>";
                        result_2 += "<td width='45%' style='border-right-style:solid;border-width: thin;' valign='top'>";
                        result_2 += "<br/><span style='font-size:large;color:#EA9494;'>定期</span><br/>";
                        result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + todate.Month + " 月 " + todate.Day + " 日 ~ ";
                        DateTime.TryParse(ict_f1.Table.Rows[ii]["end_date"].ToString(), out todate);
                        result_2 += todate.Month + " 月 " + todate.Day + " 日</span><br/>";
                    }
                }
                if (same)
                {
                    Query1 = "select g.week_of_day_jp,a.id,d.uiswaid,d.uid,f.photo,f.username,a.type,a.check_success,d.start_date,d.end_date,d.start_hour,d.start_minute,d.end_hour,d.end_minute,d.howtoget_there";
                    Query1 += " from user_information_appointment_check_deal as a";
                    Query1 += " inner join user_information_appointment_check_connect_deal as b on a.id=b.uiacdid";
                    Query1 += " inner join user_information_store_week_appointment_check as d on b.uiswacid=d.id";
                    Query1 += " inner join user_login as f on f.id=a.suppid ";
                    Query1 += " inner join user_information_store_week_appointment as g on g.id=d.uiswaid ";
                    Query1 += " where a.id='" + temp_id + "' and a.type='2' and a.uid='" + param3 + "' and YEAR(d.start_date)= '" + param1 + "' and MONTH(d.start_date)= '" + param2 + "'";
                    Query1 += " order by DAY(d.start_date) asc,a.first_check_time asc,d.uid asc;";
                    ict_f = gc1.select_cmd(Query1);
                    temp_uiswaid = ""; temp_uid = "";
                    if (ict_f.Count > 0)
                    {
                        for (int i = 0; i < ict_f.Count; i++)
                        {
                            check_same = false;
                            if (i == 0)
                            {
                                temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                check_same = true;
                            }
                            else
                            {
                                if (temp_uiswaid != ict_f.Table.Rows[i]["uiswaid"].ToString() || temp_uid != ict_f.Table.Rows[i]["uid"].ToString())
                                {
                                    temp_uiswaid = ict_f.Table.Rows[i]["uiswaid"].ToString();
                                    temp_uid = ict_f.Table.Rows[i]["uid"].ToString();
                                    check_same = true;
                                }
                            }
                            if (check_same)
                            {
                                week = ict_f.Table.Rows[i]["week_of_day_jp"].ToString() + "曜日";

                                result_2 += "<br/><span style='font-size:large;color:#EA9494;'>" + week + ", ";
                                result_2 += ict_f.Table.Rows[i]["start_hour"].ToString() + ":" + ict_f.Table.Rows[i]["start_minute"].ToString() + "~" + ict_f.Table.Rows[i]["end_hour"].ToString() + ":" + ict_f.Table.Rows[i]["end_minute"].ToString() + "</span><br/>";



                            }
                        }
                    }


                }

            }
            result_2 += "<br/><span>" + ict_f1.Table.Rows[ict_f1.Count - 1]["howtoget_there"].ToString() + "</span><br/><br/>";
            result_2 += "</td>";
            result_2 += "<td width='30%' style='border-right-style:solid;border-width: thin;'>";

            result_2 += "<table width='100%'>";
            result_2 += "<tr>";
            result_2 += "<td width='10%'></td>";
            result_2 += "<td width='20%' valign='top'>";
            //user photo
            result_2 += "<div class='zoom-gallery'>";
            cutstr_h = ict_f1.Table.Rows[ict_f1.Count - 1]["photo"].ToString();
            ind_h = cutstr_h.IndexOf(@"/");
            cutstr_h1 = cutstr_h.Substring(ind_h + 1, cutstr_h.Length - ind_h - 1);
            result_2 += "<a href='" + cutstr_h1 + "' data-source='" + cutstr_h1 + "' title='" + ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString() + "' style='width:100px;height:100px;'>";
            result_2 += "<img src='" + cutstr_h1 + "' width='100' height='100' />";
            result_2 += "</a>";
            result_2 += "</div>";
            result_2 += "</td>";
            result_2 += "<td width='70%'>";
            result_2 += "<span style='color: blue;'>";
            result_2 += ict_f1.Table.Rows[ict_f1.Count - 1]["username"].ToString();
            result_2 += "</span><br/>";
            result_2 += "<br/><span style='color: #999999;'>メッセージを送る</span><br/>";
            result_2 += "</td>";
            result_2 += "</tr>";
            result_2 += "</table>";

            result_2 += "</td>";
            result_2 += "<td width='20%' align='center'>";
            if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "0")
            {
                button_name = "依頼中";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "1")
            {
                button_name = "予約確定";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "2")
            {
                button_name = "お断り";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "3")
            {
                button_name = "報告書の確認";
                //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ict_f1.Count - 1]["id"].ToString() + "' value='" + button_name + "' onclick='check_report_page(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "4")
            {
                button_name = "評価をしよう";
                //result += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ict_f1.Count - 1]["id"].ToString() + "' value='" + button_name + "' onclick='score_report(this.id)' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "5")
            {
                button_name = "完了";
                result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
                //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            }
            //if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "0")
            //{
            //    button_name = "承認待ち";
            //    result_2 += "<input type='button' id='deal_" + ict_f1.Table.Rows[ict_f1.Count - 1]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#EA9494;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            //}
            //else if (ict_f1.Table.Rows[ict_f1.Count - 1]["check_success"].ToString() == "1")
            //{
            //    button_name = "予約確定";
            //    result_2 += "<span style='font-size:large;color: #999999;'>" + button_name + "</span>";
            //    //result += "<input type='button' id='deal_" + ict_f.Table.Rows[i]["id"].ToString() + "' value='" + button_name + "' style='font-size:large;color:#999999;border-style:none;background-color:#ffffff;cursor: pointer;'>";
            //}
            result_2 += "</td>";
            result_2 += "</tr>";
            result_2 += "</table>";
            dl.element = result_2;
            dllist.Add(dl);

        }

        dllist.Sort((a, b) => a.date.CompareTo(b.date));
        result1 = "";
        //result1 = dllist.Count.ToString();
        for (int i = 0; i < dllist.Count; i++)
        {
            result1 += dllist[i].element;
        }
        return result1;

    }
    public class date_list
    {
        public DateTime date;
        public string element = "";
    }
    [WebMethod(EnableSession = true)]
    public static string sendtopost(string param1, string param2, string param3, string param4)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3 + "," + param4;

         string constr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqlConnectionString"].ConnectionString;
        int modified = 0;

        using (MySqlConnection con = new MySqlConnection(constr))
        {
            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO status_messages_user(uid,smid) VALUES('" + param1 + "','" + param2 + "');", con))
            {
                con.Open();

                cmd.ExecuteNonQuery();
                modified = System.Convert.ToInt32(cmd.LastInsertedId);

                con.Close();
            }
        }
        //result = modified.ToString();
        string impath = "~/" + param4;
        if (param4 == "")
        {
            impath = "";
        }

        Query1 = "insert into status_messages_user_talk(smuid,message,filename,pointer_message_id,pointer_user_id,structure_level,year,month,day,hour,minute,second)";
        Query1 += " values('" + modified + "','" + param3 + "','" + impath + "','0','0','0','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
        resin = gc1.insert_cmd(Query1);



        return result;
    }
    [WebMethod]
    public static string small_sendtopost(string param1, string param2, string param3, string param4)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result = param1 + "," + param2 + "," + param3 + "," + param4;

        string smuid = "";
        int structure_level = 0;

        Query1 = "select smuid,structure_level from status_messages_user_talk";
        Query1 += " where id='" + param2 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            smuid = ict_f.Table.Rows[0]["smuid"].ToString();
            structure_level = Convert.ToInt32(ict_f.Table.Rows[0]["structure_level"].ToString());
            //result = modified.ToString();
            string impath = "~/" + param4;
            if (param4 == "")
            {
                impath = "";
            }
            Query1 = "insert into status_messages_user_talk(smuid,message,filename,pointer_message_id,pointer_user_id,structure_level,year,month,day,hour,minute,second)";
            Query1 += " values('" + smuid + "','" + param3 + "','" + impath + "','" + param2 + "','" + param1 + "','" + (structure_level + 1) + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            gc1.insert_cmd(Query1);
        }


        return result;
    }
    [WebMethod]
    public static string who_ans(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result = "";

        Query1 = "select photo,username from user_login";
        Query1 += " where id='" + param1 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            result += "<table width='100%'>";
            result += "<tr>";
            result += "<td width='5%'></td>";
            result += "<td width='10%' valign='top' align='right'>";
            //user photo
            result += "<div class='zoom-gallery'>";
            string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            result += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict_f.Table.Rows[0]["username"].ToString() + "' style='width:25px;height:25px;'>";
            result += "<img src='" + cutstr3 + "' width='25' height='25' />";
            result += "</a>";
            result += "</div>";
            result += "</td>";
            result += "<td width='85%'>";

            //user answer
            result += "<input type='text' id='whysmal" + param2 + "_" + param2 + "' onkeypress='small_sendmessage(event,this.id)'  placeholder='コメントする' style='width: 50%;height:30px;' title='【Enter】キーを押してください'>";

            result += @"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload_" + param2 + @"' />
</label>
<br />
            <div id='progressbar_" + param2 + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place_" + param2 + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image_" + param2 + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>";



            result += "</td>";
            result += "</tr>";
            result += "</table>";

            result += @"<script>

$(function () {
$('#btnFileUpload_" + param2 + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar_" + param2 + @"').show();
        $('#image_place_" + param2 + @"').hide();
        $('#image_place_" + param2 + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar_" + param2 + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar_" + param2 + @"').hide();
        $('#progressbar_" + param2 + @" div').css('width', '0%');
        $('#image_place_" + param2 + @"').show();
        document.getElementById('make-image_" + param2 + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar_" + param2 + @"').hide();
        $('#progressbar_" + param2 + @" div').css('width', '0%');
        $('#image_place_" + param2 + @"').hide();
        $('#image_place_" + param2 + @" div').css('width', '0%');
        console.log('error', error);
    }
});});</script>";


        }
        return result;
    }

    [WebMethod]
    public static string like_who_ans(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        string result = param3;

        Query1 = "select id from status_messages_user_talk_like";
        Query1 += " where uid='" + param1 + "' and smutid='" + param2 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            Query1 = "update status_messages_user_talk_like set good_status='" + param3 + "'";
            Query1 += ",year='" + DateTime.Now.Year + "',month='" + DateTime.Now.Month + "',day='" + DateTime.Now.Day + "',hour='" + DateTime.Now.ToString("HH") + "',minute='" + DateTime.Now.Minute + "',second='" + DateTime.Now.Second + "'";
            Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into status_messages_user_talk_like(uid,smutid,good_status,year,month,day,hour,minute,second)";
            Query1 += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            resin = gc1.insert_cmd(Query1);
        }


        return result;
    }

    [WebMethod]
    public static string like_or_not(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3;
        Query1 = "select id from status_messages_user_like";
        Query1 += " where uid='" + param1 + "' and smid='" + param2 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            Query1 = "update status_messages_user_like set good_status='" + param3 + "'";
            Query1 += ",year='" + DateTime.Now.Year + "',month='" + DateTime.Now.Month + "',day='" + DateTime.Now.Day + "',hour='" + DateTime.Now.ToString("HH") + "',minute='" + DateTime.Now.Minute + "',second='" + DateTime.Now.Second + "'";
            Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into status_messages_user_like(uid,smid,good_status,year,month,day,hour,minute,second)";
            Query1 += " values('" + param1 + "','" + param2 + "','" + param3 + "','" + DateTime.Now.Year + "','" + DateTime.Now.Month + "','" + DateTime.Now.Day + "','" + DateTime.Now.ToString("HH") + "','" + DateTime.Now.Minute + "','" + DateTime.Now.Second + "');";
            resin = gc1.insert_cmd(Query1);
        }


        return result;
    }
    [WebMethod]
    public static string Save_update(string param_id, string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        int upid = Convert.ToInt32(param_id);
        //string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8 + "," + param9 + "," + param10;
        string result = "";
        try
        {
            string id = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_first_name = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_second_name = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_first_name = param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_second_name = param5.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            string date = param6.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            string school_name = param7.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string hospital_name = param8.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string sick_name = param9.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string sex = param10.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            //get birthday year month day
            int ind = date.IndexOf(@"/"), ind2 = date.LastIndexOf(@"/");
            string year = date.Substring(0, ind), month = date.Substring(ind + 1, ind2 - ind - 1), day = date.Substring(ind2 + 1, date.Length - ind2 - 1);
            //result += ",Y:"+year+",M:"+month+",D:"+day;



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


            if (real_first_name != "" && real_second_name != "" && real_spell_first_name != "" && real_spell_second_name != "")
            {
                Query1 = "select id from user_information_school_children";
                Query1 += " where id='" + upid + "';";
                DataView ict_f = gc1.select_cmd(Query1);


                if (ict_f.Count > 0)
                {
                    Query1 = "update user_information_school_children set real_first_name='" + real_first_name + "',real_second_name='" + real_second_name + "',real_spell_first_name='" + real_spell_first_name + "'";
                    Query1 += ",real_spell_second_name='" + real_spell_second_name + "',sex='" + sexx + "',birthday_year='" + year + "'";
                    Query1 += ",birthday_month='" + month + "',birthday_day='" + day + "',school_name='" + school_name + "',hospital_name='" + hospital_name + "',sick_name='" + sick_name + "'";
                    Query1 += " where id='" + upid + "';";
                    resin = gc1.update_cmd(Query1);

                    result = "success";
                    //return result;
                }
                //result = "fail";
            }
            else
            {
                if (real_first_name == "")
                {
                    result += "1,";
                }
                if (real_second_name == "")
                {
                    result += "2,";
                }
                if (real_spell_first_name == "")
                {
                    result += "3,";
                }
                if (real_spell_second_name == "")
                {
                    result += "4,";
                }
            }

        }
        catch (Exception ex)
        {
            //result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
    [WebMethod]
    public static string Save( string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";

        //string result = param1 + "," + param2 + "," + param3 + "," + param4 + "," + param5 + "," + param6 + "," + param7 + "," + param8 + "," + param9 + "," + param10;
        string result = "";
        try
        {
            string id = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_first_name = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_second_name = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_first_name = param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string real_spell_second_name = param5.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            string date = param6.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            string school_name = param7.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string hospital_name = param8.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string sick_name = param9.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string sex = param10.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();

            //get birthday year month day
            int ind = date.IndexOf(@"/"), ind2 = date.LastIndexOf(@"/");
            string year = date.Substring(0, ind), month = date.Substring(ind + 1, ind2 - ind - 1), day = date.Substring(ind2 + 1, date.Length - ind2 - 1);
            //result += ",Y:"+year+",M:"+month+",D:"+day;



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


            if (real_first_name != "" && real_second_name != "" && real_spell_first_name != "" && real_spell_second_name != "")
            {
                Query1 = "select id from user_login";
                Query1 += " where id='" + iid + "';";
                DataView ict_f = gc1.select_cmd(Query1);


                if (ict_f.Count > 0)
                {
                    Query1 = "insert into user_information_school_children(uid,real_first_name,real_second_name,real_spell_first_name,real_spell_second_name,sex,birthday_year";
                    Query1 += ",birthday_month,birthday_day,school_name,hospital_name,sick_name)";
                    Query1 += " values('" + iid + "','" + real_first_name + "','" + real_second_name + "','" + real_spell_first_name + "','" + real_spell_second_name + "','" + sexx + "'";
                    Query1 += ",'" + year + "','" + month + "','" + day + "','" + school_name + "','" + hospital_name + "','" + sick_name + "')";
                    resin = gc1.insert_cmd(Query1);




                    result = "success";
                    //return result;
                }
                //result = "fail";
            }
            else
            {
                if (real_first_name == "")
                {
                    result += "1,";
                }
                if (real_second_name == "")
                {
                    result += "2,";
                }
                if (real_spell_first_name == "")
                {
                    result += "3,";
                }
                if (real_spell_second_name == "")
                {
                    result += "4,";
                }
            }

        }
        catch (Exception ex)
        {
            //result = "fail";

            //return result;
            throw ex;
        }
        return result;
    }
    [WebMethod]
    public static string friend_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='2';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','2',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        Query1 = "select a.id,a.first_uid,b.username,b.photo,a.first_date_year,a.first_date_month,a.first_date_day,a.first_date_hour,a.first_date_minute,a.first_date_second ";
        Query1 += "from user_friendship as a inner join user_login as b on a.first_uid=b.id where a.second_uid='" + param1 + "' and a.second_check_connect='0'";
        Query1 += " ORDER BY a.first_date_year desc,a.first_date_month desc,a.first_date_day desc,a.first_date_hour desc,a.first_date_minute desc,a.first_date_second desc;";

        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_second"].ToString());
                string howdate = "";
                if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                {
                    hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                    min = DateTime.Now.Minute - min;
                    sec = DateTime.Now.Second - sec;
                    if (min < 0)
                    {
                        min += 60;
                        hour -= 1;
                    }
                    if (sec < 0)
                    {
                        sec += 60;
                        min -= 1;
                    }
                    string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                    if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                    if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                    if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                    if (hour == 0)
                    {
                        fh = "";
                    }
                    if (min == 0 && hour == 0)
                    {
                        fmin = "";
                    }
                    howdate = fh + fmin + fsec + "前";
                }
                else
                {
                    string fm = month.ToString(), fd = day.ToString();
                    if (month < 10) { fm = "0" + month.ToString(); }
                    if (day < 10) { fd = "0" + day.ToString(); }
                    howdate = year + "年" + fm + "月" + fd + "日";

                }

                string cutstr2 = ict_h_fri_notice.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='40%'>
<a href='user_home_friend.aspx?=" + ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() + @"' style='text-decoration:none;'>" + ict_h_fri_notice.Table.Rows[i]["username"].ToString() + @"</a>
                                        <br/>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
<td>

<input id='friendcheck_" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' type='button' value='友達承認' onclick='dlgcheckfriend(this.id)' class='file-upload1'/>

</td>
</tr>
</table><hr/>";
            }
        }


        return result;
    }
    public class friend_user
    {
        public int id = 0;
        public string username = "";
        public string photo = "";
        public int howmany = 0;
    }
    [WebMethod]
    public static string search_friend_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        Query1 = "select id,username,photo ";
        Query1 += "from user_login";
        Query1 += " where id!='" + param1.Trim() + "';";
        DataView ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int i = 0; i < ict_h_find_user.Count; i++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_h_find_user.Table.Rows[i]["id"].ToString());
                fu.username = ict_h_find_user.Table.Rows[i]["username"].ToString();
                string cutstr2 = ict_h_find_user.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                check_same1.Add(fu);
            }
        }
        Query1 = "select donotfind_uid ";
        Query1 += "from user_friendship_donotfind";
        Query1 += " where uid='" + param1.Trim() + "';";
        ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                bool checksam = true;
                for (int i = 0; i < ict_h_find_user.Count; i++)
                {
                    if (ict_h_find_user.Table.Rows[i]["donotfind_uid"].ToString() == check_same1[ii].id.ToString())
                    {
                        checksam = false;
                    }
                }
                if (checksam)
                {
                    fu = new friend_user();
                    fu.id = check_same1[ii].id;
                    fu.username = check_same1[ii].username;
                    fu.photo = check_same1[ii].photo;
                    output_friend.Add(fu);
                }
            }
        }
        else
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = check_same1[ii].id;
                fu.username = check_same1[ii].username;
                fu.photo = check_same1[ii].photo;
                output_friend.Add(fu);
            }
        }

        //SqlDataSource sql_h_fri_notice = new SqlDataSource();
        //sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sql_h_fri_notice.SelectCommand = "select first_uid,second_uid ";
        //sql_h_fri_notice.SelectCommand += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = (DataView)sql_h_fri_notice.Select(DataSourceSelectArguments.Empty);
        //if (ict_h_fri_notice.Count > 0)
        //{
        //    for (int ii = 0; ii < check_same.Count; ii++)
        //    {
        //        bool checksam = true;
        //        for (int i = 0; i < ict_h_fri_notice.Count; i++)
        //        {
        //            if (ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //            if (ict_h_fri_notice.Table.Rows[i]["second_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //        }
        //        if (checksam)
        //        {
        //            fu = new friend_user();
        //            fu.id = check_same[ii].id;
        //            fu.username = check_same[ii].username;
        //            fu.photo = check_same[ii].photo;
        //            output_friend.Add(fu);
        //        }
        //    }
        //}
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }


        for (int i = 0; i < output_friend.Count; i++)
        {
            int howto = 0;
            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f1.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            output_friend[i].howmany = howto;

        }

        //set up count
        HttpContext.Current.Session["friend_for_count"] = 10;

        Random rnd = new Random();

        //  宣告用來儲存亂數的陣列
        int[] ValueString = new int[Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString())];

        //  亂數產生
        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()); i++)
        {
            ValueString[i] = rnd.Next(0, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));

            //  檢查是否存在重複
            while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
            {
                ValueString[i] = rnd.Next(0, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));
            }
        }
        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()); i++)
        {
            result += @"<div id='friendpanel_" + i + @"' width='100%'><table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + output_friend[ValueString[i]].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + output_friend[ValueString[i]].id + @"' style='text-decoration:none;'>" + output_friend[ValueString[i]].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>";
            if (output_friend[ValueString[i]].howmany > 0)
            {
                result += @"<a id='listtofri_" + output_friend[ValueString[i]].id + @"' onclick='check_tofriend_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;color:#90949c;'>共通の友達" + output_friend[ValueString[i]].howmany + @"人</a>";
            }

            result += @"</td>
        <td width='30%'>

        <input id='addfriend_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload1' style='width:98% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + i + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload1 addfrienddelete' style='width:100% !important;'/>

        </td>
        </tr>
        </table><hr/></div>";
        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string search_friend_notice_list_scroll(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        List<friend_user> output_friend = new List<friend_user>();
        List<friend_user> check_same = new List<friend_user>();
        List<friend_user> check_same1 = new List<friend_user>();
        friend_user fu = new friend_user();

        Query1 = "select id,username,photo ";
        Query1 += "from user_login";
        Query1 += " where id!='" + param1.Trim() + "';";
        DataView ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int i = 0; i < ict_h_find_user.Count; i++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_h_find_user.Table.Rows[i]["id"].ToString());
                fu.username = ict_h_find_user.Table.Rows[i]["username"].ToString();
                string cutstr2 = ict_h_find_user.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                check_same1.Add(fu);
            }
        }
        Query1 = "select donotfind_uid ";
        Query1 += "from user_friendship_donotfind";
        Query1 += " where uid='" + param1.Trim() + "';";
        ict_h_find_user = gc1.select_cmd(Query1);
        if (ict_h_find_user.Count > 0)
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                bool checksam = true;
                for (int i = 0; i < ict_h_find_user.Count; i++)
                {
                    if (ict_h_find_user.Table.Rows[i]["donotfind_uid"].ToString() == check_same1[ii].id.ToString())
                    {
                        checksam = false;
                    }
                }
                if (checksam)
                {
                    fu = new friend_user();
                    fu.id = check_same1[ii].id;
                    fu.username = check_same1[ii].username;
                    fu.photo = check_same1[ii].photo;
                    output_friend.Add(fu);
                }
            }
        }
        else
        {
            for (int ii = 0; ii < check_same1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = check_same1[ii].id;
                fu.username = check_same1[ii].username;
                fu.photo = check_same1[ii].photo;
                output_friend.Add(fu);
            }
        }

        //SqlDataSource sql_h_fri_notice = new SqlDataSource();
        //sql_h_fri_notice.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sql_h_fri_notice.SelectCommand = "select first_uid,second_uid ";
        //sql_h_fri_notice.SelectCommand += "from user_friendship;";
        //sql_h_fri_notice.DataBind();
        //DataView ict_h_fri_notice = (DataView)sql_h_fri_notice.Select(DataSourceSelectArguments.Empty);
        //if (ict_h_fri_notice.Count > 0)
        //{
        //    for (int ii = 0; ii < check_same.Count; ii++)
        //    {
        //        bool checksam = true;
        //        for (int i = 0; i < ict_h_fri_notice.Count; i++)
        //        {
        //            if (ict_h_fri_notice.Table.Rows[i]["first_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //            if (ict_h_fri_notice.Table.Rows[i]["second_uid"].ToString() == check_same[ii].id.ToString())
        //            {
        //                checksam = false;
        //            }
        //        }
        //        if (checksam)
        //        {
        //            fu = new friend_user();
        //            fu.id = check_same[ii].id;
        //            fu.username = check_same[ii].username;
        //            fu.photo = check_same[ii].photo;
        //            output_friend.Add(fu);
        //        }
        //    }
        //}
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                user_friend.Add(ict_f.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }


        for (int i = 0; i < output_friend.Count; i++)
        {
            int howto = 0;
            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + output_friend[i].id + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    for (int iii = 0; iii < user_friend.Count; iii++)
                    {
                        if (user_friend[iii] == ict_f1.Table.Rows[ii]["id"].ToString())
                        {
                            howto += 1;
                        }
                    }
                }
            }
            output_friend[i].howmany = howto;

        }


        //set up count
        if (HttpContext.Current.Session["friend_for_count"] != null)
        {
            if (HttpContext.Current.Session["friend_for_count"].ToString() != "")
            {
                int count_bf = Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString());
                int count_f = Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString());
                count_f += 10;
                if (count_f < output_friend.Count)
                {
                    HttpContext.Current.Session["friend_for_count"] = count_f;
                    Random rnd = new Random();

                    //  宣告用來儲存亂數的陣列
                    int[] ValueString = new int[count_f - count_bf];

                    //  亂數產生
                    for (int i = 0; i < count_f - count_bf; i++)
                    {
                        ValueString[i] = rnd.Next( count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));

                        //  檢查是否存在重複
                        while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
                        {
                            ValueString[i] = rnd.Next( count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));
                        }
                    }
                    for (int i = 0; i < count_f - count_bf; i++)
                    {
                        result += @"<div id='friendpanel_" + (i + count_bf) + @"' width='100%'><table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + output_friend[ValueString[i]].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + output_friend[ValueString[i]].id + @"' style='text-decoration:none;'>" + output_friend[ValueString[i]].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>";
                        if (output_friend[ValueString[i]].howmany > 0)
                        {
                            result += @"<a id='listtofri_" + output_friend[ValueString[i]].id + @"' onclick='check_tofriend_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;color:#90949c;'>共通の友達" + output_friend[ValueString[i]].howmany + @"人</a>";
                        }

                        result += @"</td>
        <td width='30%'>

        <input id='addfriend_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='友達になる' onclick='dlgcheckfriend_addfri(this.id)' class='file-upload1' style='width:98% !important;'/>

        </td>
        <td width='10%'>

        <input id='addfrienddelete_" + (i + count_bf) + @"_" + output_friend[ValueString[i]].id + @"' type='button' value='削除する' onclick='dlgcheckfriend_donotfind(this.id)' class='file-upload1 addfrienddelete' style='width:100% !important;'/>

        </td>
        </tr>
        </table><hr/></div>";
                    }
                }
            }
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_addfind(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;

        result = "";

        string year = DateTime.Now.Year.ToString();
        string month = DateTime.Now.Month.ToString();
        string day = DateTime.Now.Day.ToString();
        int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
        string min = DateTime.Now.Minute.ToString();
        string sec = DateTime.Now.Second.ToString();

        string upid = "";
        bool chec = true;
        Query1 = "select a.id";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        Query1 += " and c.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            upid = ict_f.Table.Rows[0]["id"].ToString();
            chec = true;
        }
        Query1 = "select a.id";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";
        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "'";
        Query1 += " and b.id='" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "';";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            upid = ict_f1.Table.Rows[0]["id"].ToString();
            chec = false;
        }

        if (upid != "")
        {
            if (chec)
            {
                Query1 = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                Query1 += ",first_date_year='" + year + "',first_date_month='" + month + "',first_date_day='" + day + "',first_date_hour='" + hour + "',first_date_minute='" + min + "',first_date_second='" + sec + "'";
                Query1 += " where id='" + upid + "';";
                resin = gc1.update_cmd(Query1);
            }
            else
            {
                Query1 = "update user_friendship set first_check_connect='1',second_check_connect='1'";
                Query1 += ",second_date_year='" + year + "',second_date_month='" + month + "',second_date_day='" + day + "',second_date_hour='" + hour + "',second_date_minute='" + min + "',second_date_second='" + sec + "'";
                Query1 += " where id='" + upid + "';";
                resin = gc1.update_cmd(Query1);
            }

        }
        else
        {
            Query1 = "insert into user_friendship(first_uid,first_check_connect,second_uid,second_check_connect";
            Query1 += ",first_date_year,first_date_month,first_date_day,first_date_hour,first_date_minute,first_date_second)";
            Query1 += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','1','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','0'";
            Query1 += ",'" + year + "','" + month + "','" + day + "','" + hour + "','" + min + "','" + sec + "');";
            resin = gc1.insert_cmd(Query1);
        }



        return result;
    }
    [WebMethod]
    public static string friend_notice_donotfind(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;

        result = "";
        Query1 = "insert into user_friendship_donotfind(uid,donotfind_uid)";
        Query1 += " values('" + param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "','" + param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() + "');";
        resin = gc1.insert_cmd(Query1);

        return result;
    }
    [WebMethod]
    public static string toget_friend_list(string param1, string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //fu = new friend_user();
        //fu.id = check_same[ii].id;
        //fu.username = check_same[ii].username;
        //fu.photo = check_same[ii].photo;
        //output_friend.Add(fu);
        List<friend_user> user_friend = new List<friend_user>();
        friend_user fu = new friend_user();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f = gc1.select_cmd(Query1);
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                fu.username = ict_f.Table.Rows[ii]["username"].ToString();
                string cutstr2 = ict_f.Table.Rows[ii]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                user_friend.Add(fu);
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                fu = new friend_user();
                fu.id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                fu.username = ict_f1.Table.Rows[ii]["username"].ToString();
                string cutstr2 = ict_f1.Table.Rows[ii]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                fu.photo = cutstr3;
                user_friend.Add(fu);
            }
        }

        List<friend_user> user_to_friend = new List<friend_user>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            int howto = 0;

            Query1 = "select c.id,c.username,c.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where b.id='" + param2.Trim() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (user_friend[i].id.ToString() == ict_f.Table.Rows[ii]["id"].ToString())
                    {
                        fu = new friend_user();
                        fu.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        fu.username = ict_f.Table.Rows[ii]["username"].ToString();
                        string cutstr2 = ict_f.Table.Rows[ii]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        fu.photo = cutstr3;
                        user_to_friend.Add(fu);
                    }
                }

            }
            Query1 = "select b.id,b.username,b.photo";
            Query1 += " from user_friendship as a";
            Query1 += " inner join user_login as b on b.id=a.first_uid";
            Query1 += " inner join user_login as c on c.id=a.second_uid";

            //check by type use type=0,1
            Query1 += " where c.id='" + param2.Trim() + "'";
            Query1 += " and first_check_connect=1 and second_check_connect=1;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    if (user_friend[i].id.ToString() == ict_f1.Table.Rows[ii]["id"].ToString())
                    {
                        fu = new friend_user();
                        fu.id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                        fu.username = ict_f1.Table.Rows[ii]["username"].ToString();
                        string cutstr2 = ict_f1.Table.Rows[ii]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        fu.photo = cutstr3;
                        user_to_friend.Add(fu);
                    }
                }
            }

        }

        for (int i = 0; i < user_to_friend.Count; i++)
        {
            result += @"<table width='100%'>
        <tr>

         <td width='20%'>
                                                <img alt='' src='" + user_to_friend[i].photo + @"' width='100px' height='100px' />
                                            </td>
                                            <td align='left' width='40%'>
        <a href='user_home_friend.aspx?=" + user_to_friend[i].id + @"' style='text-decoration:none;'>" + user_to_friend[i].username + @"</a>
                                                <br/>
        <br/>
                                                <br/>


                                            </td>
        <td width='30%'>



        </td>
        <td width='10%'>


        </td>
        </tr>
        </table><hr/>";
        }


        return result;
    }
    [WebMethod]
    public static string friend_notice_check(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        Query1 = "update user_friendship set second_check_connect='1',second_date_year='" + DateTime.Now.Year + "',second_date_month='" + DateTime.Now.Month + "',second_date_day='" + DateTime.Now.Day + "'";
        Query1 += ",second_date_hour='" + DateTime.Now.ToString("HH") + "',second_date_minute='" + DateTime.Now.Minute + "',second_date_second='" + DateTime.Now.Second + "' ";
        Query1 += "where id='" + param1 + "';";
        resin = gc1.update_cmd(Query1);

        return result;
    }
    [WebMethod]
    public static string friend_notice_check_del(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        if (param1.Trim() != "")
        {
            Query1 = "DELETE FROM user_friendship ";
            Query1 += "where id='" + param1 + "';";
            resin = gc1.delete_cmd(Query1);
        }

        return result;
    }
    [WebMethod]
    public static string chat_notice_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='1';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','1',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        Query1 = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1 + "'";
        Query1 += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);

        List<friend_list_chat> fri = new List<friend_list_chat>();
        friend_list_chat frii = new friend_list_chat();
        int tempid = 0;
        for (int i = 0; i < ict_f.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
        }

        Query1 = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where c.id=" + param1 + "";
        Query1 += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        tempid = 0;
        for (int i = 0; i < ict_f1.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f1.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f1.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f1.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
        }

        fri = fri.OrderBy(c => c.id).ToList();

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //        .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();

        List<friend_list_chat> tmp_fri = new List<friend_list_chat>();
        List<friend_list_chat> fri_total = new List<friend_list_chat>();
        frii = new friend_list_chat();
        List<int> fri_ind = new List<int>();
        tempid = 0;
        for (int i = 0; i < fri.Count; i++)
        {
            if (tempid != fri[i].id)
            {
                tempid = fri[i].id;
                fri_ind.Add(tempid);
            }
        }
        for (int i = 0; i < fri_ind.Count; i++)
        {
            tmp_fri = new List<friend_list_chat>();
            for (int ii = 0; ii < fri.Count; ii++)
            {
                if (fri_ind[i] == fri[ii].id)
                {
                    tmp_fri.Add(fri[ii]);
                }
            }
            tmp_fri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            fri_total.Add(tmp_fri[tmp_fri.Count - 1]);
        }
        fri_total.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //       .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();
        fri = fri_total;
        for (int i = 0; i < fri.Count; i++)
        {

            int year = fri[i].year;
            int month = fri[i].month;
            int day = fri[i].day;
            int hour = fri[i].hour;
            int min = fri[i].min;
            int sec = fri[i].sec;
            string howdate = "";
            if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                min = DateTime.Now.Minute - min;
                sec = DateTime.Now.Second - sec;
                if (min < 0)
                {
                    min += 60;
                    hour -= 1;
                }
                if (sec < 0)
                {
                    sec += 60;
                    min -= 1;
                }
                string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                if (hour == 0)
                {
                    fh = "";
                }
                if (min == 0 && hour == 0)
                {
                    fmin = "";
                }
                howdate = fh + fmin + fsec + "前";
            }
            else
            {
                string fm = month.ToString(), fd = day.ToString();
                if (month < 10) { fm = "0" + month.ToString(); }
                if (day < 10) { fd = "0" + day.ToString(); }
                howdate = year + "年" + fm + "月" + fd + "日";

            }

            string cutstr2 = fri[i].photo;
            int ind2 = cutstr2.IndexOf(@"/");
            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            string mess = "";
            if (fri[i].mesg.Length < 20)
            {
                mess = fri[i].mesg;
            }
            else
            {
                mess = fri[i].mesg.Substring(0, 19) + "‧‧‧";
            }
            result += @"<div id='chat_" + fri[i].id + @"' style='cursor: pointer;' onclick='chat_notice_click(this.id)'><table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + fri[i].id + @"' style='text-decoration:none;'>" + fri[i].username + @"</a>
                                        <br/>
<br/>
<span style='color:#000;'>" + mess + @"</span>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";


        }

        return result;
    }
    [WebMethod]
    public static string new_state_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','0',NOW());";
            gc1.insert_cmd(Query1);
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

        //status message
        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f1.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f1.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message


        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer
                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message
        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {
                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }
                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //count
        HttpContext.Current.Session["new_state_for_count"] = 10;
        if (status_mess_like.Count < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()))
        {
            HttpContext.Current.Session["new_state_for_count"] = status_mess_like.Count;
        }

        for (int i = 0; i < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()); i++)
        {

            int year = status_mess_like[i].comdate.Year;
            int month = status_mess_like[i].comdate.Month;
            int day = status_mess_like[i].comdate.Day;
            int hour = status_mess_like[i].comdate.Hour;
            int min = status_mess_like[i].comdate.Minute;
            int sec = status_mess_like[i].comdate.Second;
            string howdate = "";
            if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                min = DateTime.Now.Minute - min;
                sec = DateTime.Now.Second - sec;
                if (min < 0)
                {
                    min += 60;
                    hour -= 1;
                }
                if (sec < 0)
                {
                    sec += 60;
                    min -= 1;
                }
                string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                if (hour == 0)
                {
                    fh = "";
                }
                if (min == 0 && hour == 0)
                {
                    fmin = "";
                }
                howdate = fh + fmin + fsec + "前";
            }
            else
            {
                string fm = month.ToString(), fd = day.ToString();
                if (month < 10) { fm = "0" + month.ToString(); }
                if (day < 10) { fd = "0" + day.ToString(); }
                howdate = year + "年" + fm + "月" + fd + "日";

            }
            if (status_mess_like[i].type == 2)
            {
                //friend post
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].uid + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                    int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    string mess = "";
                    if (status_mess_like[i].like_message.Length < 20)
                    {
                        mess = status_mess_like[i].like_message;
                    }
                    else
                    {
                        mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                    }
                    //check
                    result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが近況を更新しました「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                }
            }
            else if (status_mess_like[i].type == 3)
            {
                //friend like
                //other person name
                string othfri = "";
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    othfri = ict_f.Table.Rows[0]["username"].ToString();
                }
                Query1 = "select username,photo";
                Query1 += " from user_login";
                Query1 += " where id='" + status_mess_like[i].uid + "';";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                    int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    string mess = "";
                    if (status_mess_like[i].like_message.Length < 20)
                    {
                        mess = status_mess_like[i].like_message;
                    }
                    else
                    {
                        mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                    }
                    //status_mess_like[i].like_idlist[0]
                    //check
                    result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが</span>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + othfri + @"</a>
<span>さんの投稿について「いいね！」と言っています: 「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                }
            }
            else
            {
                if (status_mess_like[i].uid == 0)
                {
                    if (status_mess_like[i].like_idlist.Count > 0)
                    {
                        Query1 = "select username,photo";
                        Query1 += " from user_login";
                        Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                        ict_f = gc1.select_cmd(Query1);
                        if (ict_f.Count > 0)
                        {
                            string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                            int ind2 = cutstr2.IndexOf(@"/");
                            string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            string mess = "";
                            if (status_mess_like[i].like_message.Length < 20)
                            {
                                mess = status_mess_like[i].like_message;
                            }
                            else
                            {
                                mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                            }
                            //check
                            if (status_mess_like[i].type > 0)
                            {
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                            }
                            else
                            {
                                result += @"<div id='newstatus_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                            }
                            result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さん他" + (status_mess_like[i].like_idlist.Count - 1) + @"人があなたの投稿に「いいね」と言っています:「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                        }
                    }
                }
                else
                {
                    Query1 = "select username,photo";
                    Query1 += " from user_login";
                    Query1 += " where id='" + status_mess_like[i].uid + "';";
                    ict_f = gc1.select_cmd(Query1);
                    if (ict_f.Count > 0)
                    {
                        string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                        int ind2 = cutstr2.IndexOf(@"/");
                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        string mess = "";
                        if (status_mess_like[i].message.Length < 20)
                        {
                            mess = status_mess_like[i].message;
                        }
                        else
                        {
                            mess = status_mess_like[i].message.Substring(0, 19) + "‧‧‧";
                        }

                        result += @"<div id='newstatus_" + status_mess_like[i].id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                        result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんがあなたの投稿に返信をしました:「" + mess + @"」。</span>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                    }
                }
            }

        }

        return result;
    }
    [WebMethod]
    public static string new_state_notice_list_scroll(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        //setup check time
        Query1 = "select id";
        Query1 += " from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";
        DataView ict_f_t = gc1.select_cmd(Query1);
        if (ict_f_t.Count > 0)
        {
            Query1 = "update user_notice_check set check_time=NOW()";
            Query1 += " where id='" + ict_f_t.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into user_notice_check(uid,type,check_time)";
            Query1 += " values('" + param1 + "','0',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        //setup check time
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }
        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            for (int ii = 0; ii < ict_f1.Count; ii++)
            {
                user_friend.Add(ict_f1.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message

        //status message
        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();

        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                for (int ii = 0; ii < ict_f1.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f1.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f1.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f1.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f1.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f1.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message


        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer
                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message
        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {
                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }

                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //count
        if (HttpContext.Current.Session["new_state_for_count"] != null)
        {
            if (HttpContext.Current.Session["new_state_for_count"].ToString() != "")
            {
                int count_bf = Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString());
                int count_f = Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString());
                count_f += 10;
                if (count_f < status_mess_like.Count)
                {
                    HttpContext.Current.Session["new_state_for_count"] = count_f;

                    for (int i = count_bf; i < Convert.ToInt32(HttpContext.Current.Session["new_state_for_count"].ToString()); i++)
                    {

                        int year = status_mess_like[i].comdate.Year;
                        int month = status_mess_like[i].comdate.Month;
                        int day = status_mess_like[i].comdate.Day;
                        int hour = status_mess_like[i].comdate.Hour;
                        int min = status_mess_like[i].comdate.Minute;
                        int sec = status_mess_like[i].comdate.Second;
                        string howdate = "";
                        if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                        {
                            hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                            min = DateTime.Now.Minute - min;
                            sec = DateTime.Now.Second - sec;
                            if (min < 0)
                            {
                                min += 60;
                                hour -= 1;
                            }
                            if (sec < 0)
                            {
                                sec += 60;
                                min -= 1;
                            }
                            string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                            if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                            if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                            if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                            if (hour == 0)
                            {
                                fh = "";
                            }
                            if (min == 0 && hour == 0)
                            {
                                fmin = "";
                            }
                            howdate = fh + fmin + fsec + "前";
                        }
                        else
                        {
                            string fm = month.ToString(), fd = day.ToString();
                            if (month < 10) { fm = "0" + month.ToString(); }
                            if (day < 10) { fd = "0" + day.ToString(); }
                            howdate = year + "年" + fm + "月" + fd + "日";

                        }
                        if (status_mess_like[i].type == 2)
                        {
                            //friend post
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].uid + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                int ind2 = cutstr2.IndexOf(@"/");
                                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                string mess = "";
                                if (status_mess_like[i].like_message.Length < 20)
                                {
                                    mess = status_mess_like[i].like_message;
                                }
                                else
                                {
                                    mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                }
                                //check
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが近況を更新しました「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                            }
                        }
                        else if (status_mess_like[i].type == 3)
                        {
                            //friend like
                            //other person name
                            string othfri = "";
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                othfri = ict_f.Table.Rows[0]["username"].ToString();
                            }
                            Query1 = "select username,photo";
                            Query1 += " from user_login";
                            Query1 += " where id='" + status_mess_like[i].uid + "';";
                            ict_f = gc1.select_cmd(Query1);
                            if (ict_f.Count > 0)
                            {
                                string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                int ind2 = cutstr2.IndexOf(@"/");
                                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                string mess = "";
                                if (status_mess_like[i].like_message.Length < 20)
                                {
                                    mess = status_mess_like[i].like_message;
                                }
                                else
                                {
                                    mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                }
                                //status_mess_like[i].like_idlist[0]
                                //check
                                result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんが</span>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + othfri + @"</a>
<span>さんの投稿について「いいね！」と言っています: 「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                            }
                        }
                        else
                        {
                            if (status_mess_like[i].uid == 0)
                            {
                                if (status_mess_like[i].like_idlist.Count > 0)
                                {
                                    Query1 = "select username,photo";
                                    Query1 += " from user_login";
                                    Query1 += " where id='" + status_mess_like[i].like_idlist[0] + "';";
                                    ict_f = gc1.select_cmd(Query1);
                                    if (ict_f.Count > 0)
                                    {
                                        string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                        int ind2 = cutstr2.IndexOf(@"/");
                                        string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                        string mess = "";
                                        if (status_mess_like[i].like_message.Length < 20)
                                        {
                                            mess = status_mess_like[i].like_message;
                                        }
                                        else
                                        {
                                            mess = status_mess_like[i].like_message.Substring(0, 19) + "‧‧‧";
                                        }
                                        //check
                                        if (status_mess_like[i].type > 0)
                                        {
                                            result += @"<div id='newstatusbig_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_big_notice_click(this.id)'>";
                                        }
                                        else
                                        {
                                            result += @"<div id='newstatus_" + status_mess_like[i].like_id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                                        }
                                        result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].like_idlist[0] + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さん他" + (status_mess_like[i].like_idlist.Count - 1) + @"人があなたの投稿に「いいね」と言っています:「" + mess + @"」。</span>
                                        <br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                                    }
                                }
                            }
                            else
                            {
                                Query1 = "select username,photo";
                                Query1 += " from user_login";
                                Query1 += " where id='" + status_mess_like[i].uid + "';";
                                ict_f = gc1.select_cmd(Query1);
                                if (ict_f.Count > 0)
                                {
                                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                                    int ind2 = cutstr2.IndexOf(@"/");
                                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                    string mess = "";
                                    if (status_mess_like[i].message.Length < 20)
                                    {
                                        mess = status_mess_like[i].message;
                                    }
                                    else
                                    {
                                        mess = status_mess_like[i].message.Substring(0, 19) + "‧‧‧";
                                    }

                                    result += @"<div id='newstatus_" + status_mess_like[i].id + @"' style='cursor: pointer;' onclick='new_state_notice_click(this.id)'>";
                                    result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
<a href='user_home_friend.aspx?=" + status_mess_like[i].uid + @"' style='text-decoration:none;'>" + ict_f.Table.Rows[0]["username"].ToString() + @"</a>
<span>さんがあなたの投稿に返信をしました:「" + mess + @"」。</span>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
</tr>
</table></div><hr/>";

                                }
                            }
                        }

                    }
                }
            }
        }



        return result;
    }
    public class status_mess_list
    {
        public int id = 0;
        public string message = "";
    }
    public class status_mess_list_like
    {
        public int type = 0;

        public int id = 0;
        public int uid = 0;
        public string message = "";


        public int like_id = 0;
        public string like_message = "";
        public List<int> like_idlist = new List<int>();
        public DateTime comdate = new DateTime();
    }
    [WebMethod(EnableSession = true)]
    public static string report_page(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string results = "";

        //HttpContext.Current.Session["deal_id"] = param2;
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;

        Query1 = "select uid,suppid from user_information_appointment_check_deal";
        Query1 += " where id='" + param1 + "';";
        DataView ict_f = gc1.select_cmd(Query1);
        string ActivationCode = "", res = "";
        if (ict_f.Count > 0)
        {
            Query1 = "select ActivationCode from user_information_store_week_appointment_check_check";
            Query1 += " where uiacdid='" + param1 + "';";
            DataView ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_home.aspx/report_page", "makescore_w.aspx?ActivationCode=" + ActivationCode);

            }
            else
            {
                Query1 = "select ActivationCode from user_appointment_check";
                Query1 += " where uiacdid='" + param1 + "';";
                ict_f1 = gc1.select_cmd(Query1);
                if (ict_f1.Count > 0)
                {
                    ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                    res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_home.aspx/report_page", "makescore.aspx?ActivationCode=" + ActivationCode);
                }
            }


            //SqlDataSource sql_insert = new SqlDataSource();
            //sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            //sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            //sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認用URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
            //sql_insert.Insert();
            ////<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week(param1, param2, dealid) + ">確認用URL</a></li>
            //results += "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            //results += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
        }


        return res;
    }
    [WebMethod(EnableSession = true)]
    public static string score_page(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string results = "";

        //HttpContext.Current.Session["deal_id"] = param2;
        //now time
        string startd = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string starth = Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        string startm = DateTime.Now.Minute.ToString();
        string starts = DateTime.Now.Second.ToString();
        string start = startd + " " + starth + ":" + startm + ":" + starts;

        Query1 = "select uid,suppid from user_information_appointment_check_deal";
        Query1 += " where id='" + param1 + "';";

        DataView ict_f = gc1.select_cmd(Query1);
        string ActivationCode = "", res = "";
        if (ict_f.Count > 0)
        {
            Query1 = "select ActivationCode from user_information_store_week_appointment_check_check";
            Query1 += " where uiacdid='" + param1 + "';";
            DataView ict_f1 = gc1.select_cmd(Query1);
            if (ict_f1.Count > 0)
            {
                ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_home.aspx/score_page", "usermakescore.aspx?ActivationCode=" + ActivationCode);

            }
            else
            {
                Query1 = "select ActivationCode from user_appointment_check";
                Query1 += " where uiacdid='" + param1 + "';";
                ict_f1 = gc1.select_cmd(Query1);
                if (ict_f1.Count > 0)
                {
                    ActivationCode = ict_f1.Table.Rows[0]["ActivationCode"].ToString();
                    res = HttpContext.Current.Request.Url.AbsoluteUri.Replace("user_home.aspx/score_page", "usermakescore_o.aspx?ActivationCode=" + ActivationCode);
                }
            }


            //SqlDataSource sql_insert = new SqlDataSource();
            //sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            //sql_insert.InsertCommand = "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            //sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認用URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
            //sql_insert.Insert();
            ////<li>確認用URL&nbsp;&nbsp;<a href=" + SendActivationURL_week(param1, param2, dealid) + ">確認用URL</a></li>
            //results += "insert into user_chat_room(uid,to_uid,talk_message,year,month,day,hour,minute,second)";
            //results += " values('" + ict_f.Table.Rows[0]["suppid"].ToString() + "','" + ict_f.Table.Rows[0]["uid"].ToString() + "','<a href=" + res + ">報告書の確認URL</a>','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Day.ToString() + "','" + Convert.ToInt32(DateTime.Now.ToString("HH")).ToString() + "','" + DateTime.Now.Minute.ToString() + "','" + DateTime.Now.Second.ToString() + "')";
        }


        return res;
    }
    [WebMethod]
    public static string like_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        result = "";
        Query1 = "select b.id,b.username,b.photo,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
        Query1 += " where a.smid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["second"].ToString());
                string howdate = "";
                if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
                {
                    hour = Convert.ToInt32(DateTime.Now.ToString("HH")) - hour;
                    min = DateTime.Now.Minute - min;
                    sec = DateTime.Now.Second - sec;
                    if (min < 0)
                    {
                        min += 60;
                        hour -= 1;
                    }
                    if (sec < 0)
                    {
                        sec += 60;
                        min -= 1;
                    }
                    string fh = hour.ToString() + "時", fmin = min.ToString() + "分", fsec = sec.ToString() + "秒";
                    if (hour < 10) { fh = "0" + hour.ToString() + "時"; }
                    if (min < 10) { fmin = "0" + min.ToString() + "分"; }
                    if (sec < 10) { fsec = "0" + sec.ToString() + "秒"; }
                    if (hour == 0)
                    {
                        fh = "";
                    }
                    if (min == 0 && hour == 0)
                    {
                        fmin = "";
                    }
                    howdate = fh + fmin + fsec + "前";
                }
                else
                {
                    string fm = month.ToString(), fd = day.ToString();
                    if (month < 10) { fm = "0" + month.ToString(); }
                    if (day < 10) { fd = "0" + day.ToString(); }
                    howdate = year + "年" + fm + "月" + fd + "日";

                }

                string cutstr2 = ict_h_fri_notice.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result += @"<table width='100%'>
<tr>

 <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='40%'>
<a href='user_home_friend.aspx?=" + ict_h_fri_notice.Table.Rows[i]["id"].ToString() + @"' style='text-decoration:none;'>" + ict_h_fri_notice.Table.Rows[i]["username"].ToString() + @"</a>
                                        <br/>
<br/>
                                        <br/>
<span style='color:#CCCCCC;'>" + howdate + @"</span>

                                    </td>
<td>

</td>
</tr>
</table><hr/>";
            }
        }


        return result;
    }
    [WebMethod]
    public static string upload_head(string param1,string param2)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2;
        //result = "";
        Query1 = "select id";
        Query1 += " from user_login";
        Query1 += " where id='" + param1 + "';";
        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            Query1 = "update user_login set home_image='~/" + param2 + "'";
            Query1 += " where id='" + param1 + "';";
            resin = gc1.update_cmd(Query1);
        }
        return result;
    }
    [WebMethod(EnableSession = true)]
    public static string search_new_post(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result_res = "";
        string javascr = "";
        int counn = Convert.ToInt32(param1);
        //counn += 10;
        HttpContext.Current.Session["top_count"] = counn;

        if (HttpContext.Current.Session["id"].ToString() != "")
        {
            Query1 = " select a.place_lat,a.place_lng,a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
            Query1 += "from status_messages as a use index (IX_status_messages_1)";
            Query1 += " inner join user_login as b on b.id=a.uid where b.id='" + HttpContext.Current.Session["id"].ToString() + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc LIMIT " + (counn - 10) + ",10;";
            DataView ict = gc1.select_cmd(Query1);


            Literal li = new Literal();

            li.Text = @"
<script>

$(function () {

";

            for (int i = 0; i < ict.Count; i++)
            {
                li.Text += @"

$('#btnFileUpload" + ((counn - 10) + i) + @"').fileupload({
    url: 'FileUploadHandler.ashx?upload=start',
    add: function(e, data) {
        console.log('add', data);
        $('#progressbar" + ((counn - 10) + i) + @"').show();
        $('#image_place" + ((counn - 10) + i) + @"').hide();
        $('#image_place" + ((counn - 10) + i) + @" div').css('width', '0%');
        data.submit();
    },
    progress: function(e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', progress + '%');
    },
    success: function(response, status) {
        $('#progressbar" + ((counn - 10) + i) + @"').hide();
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', '0%');
        $('#image_place" + ((counn - 10) + i) + @"').show();
        document.getElementById('make-image" + ((counn - 10) + i) + @"').src = response;
        console.log('success', response);
    },
    error: function(error) {
        $('#progressbar" + ((counn - 10) + i) + @"').hide();
        $('#progressbar" + ((counn - 10) + i) + @" div').css('width', '0%');
        $('#image_place" + ((counn - 10) + i) + @"').hide();
        $('#image_place" + ((counn - 10) + i) + @" div').css('width', '0%');
        console.log('error', error);
    }
});";
            }
            li.Text += @"
                        });";
            li.Text += @"</script>";

            li.Text += @"
<script type='text/javascript'>

$(function () {

";
            for (int i = 0; i < ict.Count; i++)
            {
                li.Text += @"

$('.hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.box" + ((counn - 10) + i) + @"').click(function () {
                $('.hidde" + ((counn - 10) + i) + @"').toggle();
                $('.box" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.likehidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.likebox" + ((counn - 10) + i) + @"').click(function () {
                $('.likehidde" + ((counn - 10) + i) + @"').toggle();
                $('.likebox" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.likehidde" + ((counn - 10) + i) + @"').click(function () {
                $('.likebox" + ((counn - 10) + i) + @"').toggle();
                $('.likehidde" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.mess_hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.mess_box" + ((counn - 10) + i) + @"').click(function () {
                $('.mess_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.mess_box" + ((counn - 10) + i) + @"').toggle(false);
            })


            $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle(false);

            $('.big_mess_box" + ((counn - 10) + i) + @"').click(function () {
                $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.big_mess_box" + ((counn - 10) + i) + @"').toggle(false);
                $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle();
                $('.status_message_box" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.big_mess_hidde" + ((counn - 10) + i) + @"').click(function () {
                $('.big_mess_box" + ((counn - 10) + i) + @"').toggle();
                $('.big_mess_hidde" + ((counn - 10) + i) + @"').toggle(false);
                $('.status_message_box" + ((counn - 10) + i) + @"').toggle();
                $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle(false);
            })

            $('.status_message_hidde" + ((counn - 10) + i) + @"').toggle(false);


";

//                SqlDataSource sql3 = new SqlDataSource();
//                sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b WITH (INDEX(IX_status_messages_image)) on a.id=b.smid";
//                sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//                if (ict2.Count > 3)
//                {
//                    li.Text += @"
//$('.imhidde" + ((counn - 10) + i) + @"').toggle(false);
//$('.imhiddee" + ((counn - 10) + i) + @"').toggle(false);
//
//            $('.imbox" + ((counn - 10) + i) + @"').click(function () {
//                $('.imhidde" + ((counn - 10) + i) + @"').toggle();
//                $('.imhiddee" + ((counn - 10) + i) + @"').toggle();
//                $('.imbox" + ((counn - 10) + i) + @"').toggle(false);
//            })
//
//            $('.imhiddee" + ((counn - 10) + i) + @"').click(function () {
//                $('.imbox" + ((counn - 10) + i) + @"').toggle();
//                $('.imhidde" + ((counn - 10) + i) + @"').toggle(false);
//                $('.imhiddee" + ((counn - 10) + i) + @"').toggle(false);
//            })
//
//
//";
//                }
            }

            li.Text += @"

$('.image-link').magnificPopup({
                type: 'image',
                mainClass: 'mfp-with-zoom', // this class is for CSS animation below

                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it

                    duration: 100, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function

                    opener: function (openerElement) {
                        return openerElement.is('img') ? openerElement : openerElement.find('img');
                    }
                }

            });

            $('.zoom-gallery').each(function () { // the containers for all your galleries
                $(this).magnificPopup({
                    delegate: 'a',
                    type: 'image',
                    closeOnContentClick: false,
                    closeBtnInside: false,
                    mainClass: 'mfp-with-zoom mfp-img-mobile',
                    image: {
                        verticalFit: true,
                        titleSrc: function (item) {
                            return item.el.attr('title') + ' &middot; <a class=" + '"' + @"image-source-link" + '"' + @" href=" + '"' + @"' + item.el.attr('data-source') + '" + '"' + @" target=" + '"' + @"_blank" + '"' + @">image source</a>';
                        }
                    },
                    gallery: {
                        enabled: true
                    },
                    zoom: {
                        enabled: true,
                        duration: 100, // don't foget to change the duration also in CSS
                        opener: function (element) {
                            return element.find('img');
                        }
                    }
                });
            });

            $('.lazy').Lazy({
                threshold: 200,
                effect: 'fadeIn',
                visibleOnly: true,
                effect_speed: 'fast',
                onError: function (element) {
                    console.log('error loading ' + element.data('src'));
                }
            });

                        });";
            li.Text += @"</script>";
            javascr += li.Text;

            //result_res += li.Text;


            //this.Page.Controls.Add(li);


            //this.Page.Header.Controls.Add(li);
            ////添加至指定位置
            //this.Page.Header.Controls.AddAt(0, li);

            //            Literal litCss = new Literal();
            //            litCss.Text = @"
            //                <style type='text/css'>
            //                    #post_message_panel{
            //                    background-color:#fff;
            //                    border: thick solid #E9EBEE;
            //                            }
            //                 </style>";
            //            result_res += litCss.Text;

            for (int i = 0; i < ict.Count; i++)
            {
                result_res += "<div id='state_mess_" + ((counn - 10) + i) + "' style='background-color: #FFF;'>";

                result_res += "<table width='100%' style='border: thick solid #E9EBEE;'>";
                result_res += "<tr>";
                result_res += "<td>";
                //big message place
                result_res += "<table width='100%' style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;'>";
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'><br/></td><td width='90%' height='5%'><br/></td><td width='5%' height='5%'><br/></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                result_res += "<td></td>";
                result_res += "<td>";
                //new message place
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                //Poster photo
                result_res += "<td width='10%' rowspan='2' valign='top'>";

                result_res += "<div class='zoom-gallery'>";

                string cutstr2 = ict.Table.Rows[i]["photo"].ToString();
                int ind2 = cutstr2.IndexOf(@"/");
                string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:40px;height:40px;'>";
                result_res += "<img src='" + cutstr3 + "' width='40' height='40' />";
                result_res += "</a>";



                //Image img = new Image();
                //img.Width = 50; img.Height = 50;
                //img.ImageUrl = ict.Table.Rows[i]["photo"].ToString();
                //pdn2.Controls.Add(img);


                result_res += "</div>";
                result_res += "</td>";
                //poster username
                result_res += "<td width='100%'>";

                result_res += "<a href='user_home_friend.aspx?=" + ict.Table.Rows[i]["uid"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict.Table.Rows[i]["username"].ToString() + "</a>";
                result_res += "</td>";
                result_res += "</tr>";
                //poster message type and time
                result_res += "<tr>";
                result_res += "<td width='100%'>";
                result_res += "<span style='color:#CCCCCC;'>";
                result_res += "";
                if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 0)
                {
                    result_res += "お食事、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 1)
                {
                    result_res += "人気スポット、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 2)
                {
                    result_res += "イベント、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 3)
                {
                    result_res += "病院、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 4)
                {
                    result_res += "公園／レジャー、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 5)
                {
                    result_res += "授乳室、";
                }
                else if (Convert.ToInt32(ict.Table.Rows[i]["message_type"].ToString()) == 6)
                {
                    result_res += "指定なし、";
                }
                result_res += ict.Table.Rows[i]["place"].ToString() + " ";
                result_res += ict.Table.Rows[i]["year"].ToString() + "." + ict.Table.Rows[i]["month"].ToString() + "." + ict.Table.Rows[i]["day"].ToString();
                result_res += "</span>";

                //edit
                li = new Literal();

                //edit div css
                li.Text = @"<style>
           #dlgbox_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                display: none;
                position: fixed;
                width: 100%;
                z-index: 9999;
                border-radius: 10px;
                background-color: #7c7d7e;
            }
            #dlg-header_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: #f48686;
                color: white;
                font-size: 20px;
                padding: 10px;
                margin: 10px 10px 0px 10px;
                text-align: left;
            }

            #dlg-body_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: white;
                color: black;
                font-size: 14px;
                padding: 10px;
                margin: 0px 10px 0px 10px;
            }

            #dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @"{
                background-color: #f2f2f2;
                text-align: center;
                padding: 10px;
                margin: 0px 10px 10px 10px;
            }

            #dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @" button{
                background-color: #f48686;
                color: white;
                padding: 5px;
                border: 0px;
            }
</style>";



                //edit div
                li.Text += @"
<div id='dlgbox_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>
            <div id='dlg-header_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>編集</div>
            <div id='dlg-body_edit_" + ict.Table.Rows[i]["id"].ToString() + @"' style='height: 400px; overflow: auto'>
                <table style=' width: 100%;'>
                    <tr>
                        <td>
                            <hr/>
                            <table style='width: 100%;'>
                                <tr>
                                    <td width='20%'>
                                        <img alt='' src='" + cutstr3 + @"' width='100px' height='100px' />
                                    </td>
                                    <td align='left' width='80%'>
                                        <textarea id='updateText_" + ict.Table.Rows[i]["id"].ToString() + @"' cols='40' rows='5' style='border-style:none;width:100%;'>" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>",System.Environment.NewLine)+ @"</textarea><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2' style='width: 100%;'>
<table style='width: 100%;'>
                                 <tr>
                                     <td class='auto-style1'>&nbsp;</td>
                                     <td align='left' width='30%'>

                                         <img alt='' src='images/tag.png' height='20px' width='20px' />
 <select name='selectedittagname_" + ict.Table.Rows[i]["id"].ToString() + @"' id='selectedittag_" + ict.Table.Rows[i]["id"].ToString() + @"'>
     <option value=''>カテゴリー</option>
      <option value='0'>お食事</option>
      <option value='2'>イベント</option>
      <option value='3'>病院</option>
      <option value='4'>公園／レジャー</option>
       <option value='5'>授乳室</option>
</select>


                                     </td>
 <td width='25%'>
<span>
  <div class='ui inline dropdown'>
    <div class='text'>
      <img class='ui avatar image' src='images/icon/public.png'>
      一般公開
    </div>
    <i class='dropdown icon'></i>
    <div class='menu'>
       <div class='item'>
        <img class='ui avatar image' src='images/icon/public.png'>
        一般公開
      </div>
      <div class='item'>
        <img class='ui avatar image' src='images/icon/neighborhood.png'>
        地域限定
      </div>
      <div class='item'>
        <img class='ui avatar image' src='images/icon/friend.png'>
        友達
      </div>
    </div>
  </div>
</span>
    </td>
    </tr>
    </table>
                                    </td>
                                </tr>
                            </table>
                            <hr/>
                        </td>
</tr><tr>
<td colspan='2' style='width: 100%;'>
<div id='' style='overflow-x: auto; overflow-y:auto; height:200px;text-align: center;'>
";
                Query1 = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict_edit = gc1.select_cmd(Query1);
                if (ict_edit.Count > 0)
                {
                    for (int ik = 0; ik < ict_edit.Count; ik++)
                    {
                        string cutstr = ict_edit.Table.Rows[ik]["filename"].ToString();
                        int ind = cutstr.IndexOf(@"/");
                        string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        li.Text += @"<img src='" + cutstr1 + "' width='200px' height='200px' />";
                    }
                }
                li.Text += @"
</div>
</td>
                    </tr>
                </table>
            </div>
            <div id='dlg-footer_edit_" + ict.Table.Rows[i]["id"].ToString() + @"'>
<table style=' width: 100%;'>
<tr>
<td width='50%' align='left'>
<input id='statebutcancel_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='取り消す' onclick='dlgupcanel(this.id)' class='file-upload1'/>
</td>
<td width='50%' align='right'>
                <input id='statebutedit_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='保存' onclick='dlgupdate(this.id)' class='file-upload1'/>
</td>
            </tr>
</table>
</div>
        </div>
";

                //edit div js
                li.Text += @"<script>
 $(function () {
 $('#selectedittag_" + ict.Table.Rows[i]["id"].ToString() + @"')
                 .dropdown()
             ;
});
</script>";


                result_res +=li.Text;
                //edit


                result_res += "</td>";
                result_res += "</tr>";
                //poster message
                result_res += "<tr>";
                result_res += "<td colspan='2' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">";
                result_res += "<br/><div class='box" + ((counn - 10) + i) + "'>";
                if (ict.Table.Rows[i]["message"].ToString().Length < 37)
                {
                    result_res += user_home.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString());
                }
                else
                {
                    result_res += ict.Table.Rows[i]["message"].ToString().Substring(0, 37) + "‧‧‧";
                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>もっと見る</a>";
                }


                result_res += "</div>";
                result_res += "<div class='hidde" + ((counn - 10) + i) + "'>";

                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + user_home.ConvertUrlsToLinks(ict.Table.Rows[i]["message"].ToString()) + "</span>";

                result_res += "<br/>";


                //if (ict.Table.Rows[i]["message"].ToString().Length > 36)
                //{
                //    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>たたむ</a>";

                //}


                result_res += "</div>";
                result_res += "<span style='word-break:break-all;over-flow:hidden;'>" + user_home.ConvertUrlsToLinks_DIV(ict.Table.Rows[i]["message"].ToString()) + "</span>";
                result_res += "</div>";
                result_res += "</td>";
                result_res += "</tr>";
                //poster images
                string shareimg = "";
                result_res += "<tr>";
                result_res += "<td colspan='2' width='90%' align='center'><br/><br/>";

                Query1 = "select filename from status_messages as a inner join status_messages_image as b use index (IX_status_messages_image) on a.id=b.smid";
                Query1 += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict1 = gc1.select_cmd(Query1);
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                int typ = Convert.ToInt32(rand.Next(0, ict1.Count));
                if (ict1.Count > 3)
                {
                    result_res += "<div class='imbox" + ((counn - 10) + i) + "'>";
                    //for (int ii = 0; ii < 3; ii++)
                    //{
                    //    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                    //    int ind = cutstr.IndexOf(@"/");
                    //    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    //    result_res += "<img src='" + cutstr1 + "' style='height:100px;width:100px;'>";
                    //    result_res += "&nbsp;";
                    //}
                    //result_res += "<br/>";

                    //result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>もっと見る</a>";
                    //result_res += "</div>";
                    //result_res += "<div class='imhidde" + ((counn - 10) + i) + "'>";
                    result_res += "<div id='freewall" + ((counn - 10) + i) + "'>";
                    result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                    result_res += "<div class='zoom-gallery'>";
                    string morefour = "";
                    int countimg = 0;
                    for (int ii = 0; ii < ict1.Count; ii++)
                    {
                        //if (ii > 0 && ii % 3 == 0)
                        //{
                        //    result_res += "<br/>";
                        //}

                        string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                        int ind = cutstr.IndexOf(@"/");
                        string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        if (shareimg == "")
                        {
                            shareimg = cutstr1;
                        }
                        //block grid
                        if (ii > 3)
                        {
                            countimg += 1;
                            result_res += "<div style='visibility:hidden;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:0px; height:0px;outline : none;'>";
                            result_res += "<img src='images/test.png' width='0px' height='0px'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        else
                        {
                            if (ii == 3)
                            {
                                morefour += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                                morefour += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            }
                            else
                            {
                                result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";

                                //pdn2.Controls.Add(new LiteralControl("<div class='cell' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px; background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                                result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";

                                result_res += "</a>";
                                result_res += "</div>";
                            }


                        }

                        //result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>";
                        //result_res += "<img src='" + cutstr1 + "' width='100' height='100' />";
                        //result_res += "</a>";
                    }
                    //countimg
                    if (countimg > 0)
                    {
                        morefour += "<img src='images/test.png' style='background-color: #000; opacity: 0.8; width: 100%; height: 100%; text-align: center;'/>";
                        morefour += "<span style='color: white;position: absolute;top:50%;left:40%;font-size:xx-large;'>+" + countimg + "</span>";
                    }
                    else
                    {
                        morefour += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                    }
                    morefour += "</a>";
                    morefour += "</div>";
                    //string blockimg="<div style='position: absolute; background-color: #000; z-index: 999997; opacity: 0.8; width: 100%; height: 100%; text-align: center;'>";
                    result_res += morefour;
                    result_res += "</div>";
                    result_res += "</div>";
                    result_res += "</div>";
                    //
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>
                                        var wall" + ((counn - 10) + i) + @" = new Freewall('#freewall" + ((counn - 10) + i) + @"');
                    			wall" + ((counn - 10) + i) + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + ((counn - 10) + i) + @".fitWidth();
                    				}
                    			});
                    			wall" + ((counn - 10) + i) + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res += litjs.Text;


                    result_res += "</div>";

                    //result_res += "<div class='imhiddee" + ((counn - 10) + i) + "'>";
                    //result_res += "<br/>";
                    //result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>たたむ</a>";
                    //result_res += "</div>";
                }
                else if (ict1.Count > 0)
                {
                    string cutstr = ict1.Table.Rows[0]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    if (shareimg == "")
                    {
                        shareimg = cutstr1;
                    }
                    if (ict1.Count == 1)
                    {
                        result_res += "<div class='zoom-gallery'>";
                        cutstr = ict1.Table.Rows[0]["filename"].ToString();
                        ind = cutstr.IndexOf(@"/");
                        cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                        result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr1 + "' src='images/loading.gif' style='width:100%;height:100%;'/>";
                        result_res += "</a>";
                        result_res += "</div>";
                    }
                    else if (ict1.Count == 2)
                    {
                        result_res += "<div id='freewall" + ((counn - 10) + i) + "'>";
                        result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res += "<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        result_res += "</div>";
                        result_res += "</div>";
                        result_res += "</div>";
                    }
                    else if (ict1.Count == 3)
                    {
                        result_res += "<div id='freewall" + ((counn - 10) + i) + "'>";
                        result_res += "<div class='size320' data-nested='.level-1' data-gutterX=10 data-gutterY=10 data-cellW=0.5 data-cellH=0.5 >";
                        result_res += "<div class='zoom-gallery'>";
                        if (typ == 0)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                        }
                        else if (typ == 1)
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size42 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        else
                        {
                            cutstr = ict1.Table.Rows[0]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[1]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size24 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";

                            cutstr = ict1.Table.Rows[2]["filename"].ToString();
                            ind = cutstr.IndexOf(@"/");
                            cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                            result_res += "<div class='brick level-1 size22 lazy' data-src='" + cutstr1 + "' style='background-image: url(images/loading.gif);background-repeat:no-repeat; background-size: 100% 100%; background-position: center center;'>";
                            result_res += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                            result_res += "<img src='images/test.png' style='width:100%;height:100%;'/>";
                            result_res += "</a>";
                            result_res += "</div>";
                        }
                        result_res += "</div>";
                        result_res += "</div>";
                        result_res += "</div>";
                    }
                    Literal litjs = new Literal();
                    litjs.Text = @"
                                    <script type='text/javascript'>

                                        var wall" + ((counn - 10) + i) + @" = new Freewall('#freewall" + ((counn - 10) + i) + @"');
                    			wall" + ((counn - 10) + i) + @".reset({
                    				 selector: '.size320',
                    cellW: 280,
                    cellH: 280,
                    fixSize: 0,
                    gutterY: 20,
                    gutterX: 20,
                    				onResize: function() {
                    					wall" + ((counn - 10) + i) + @".fitWidth();
                    				}
                    			});
                    			wall" + ((counn - 10) + i) + @".fitWidth();
                    $(window).trigger('resize');
                                     </script>";
                    result_res += litjs.Text;
                }

                string id = "";
                bool check_li = false;
                if (HttpContext.Current.Session["id"] != null)
                {
                    if (HttpContext.Current.Session["id"].ToString() != "")
                    {
                        id = HttpContext.Current.Session["id"].ToString();

                        Query1 = "select id from status_messages_user_like";
                        Query1 += " where uid='" + id + "' and smid='" + ict.Table.Rows[i]["id"].ToString() + "';";
                        DataView ict_f_like = gc1.select_cmd(Query1);
                        if (ict_f_like.Count > 0)
                        {
                            check_li = true;
                        }
                    }
                }


                result_res += "</td>";
                result_res += "</tr>";
                result_res += "<tr>";
                //poster message like and share
                result_res += "<td width='15%' style='white-space: nowrap;' align='right'><br/><br/>";
                result_res += "<div style='cursor: pointer' class='likebox" + ((counn - 10) + i) + "'>";

                Image img1 = new Image();
                if (check_li)
                {
                    string cutstr = "~/images/like.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lalike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' style='color:#F06767;font-size:10pt;'>いいね</span>";
                }
                else
                {
                    string cutstr = "~/images/like_b.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lablike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' style='color:#CCCCCC;font-size:10pt;'>いいね</span>";
                }


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='likehidde" + ((counn - 10) + i) + "'>";
                img1 = new Image();
                if (check_li)
                {
                    string cutstr = "~/images/like_b.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "blike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lablike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='blike(this.id)' style='color:#CCCCCC;font-size:10pt;'>いいね</span>";
                }
                else
                {
                    string cutstr = "~/images/like.png";
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    result_res += "<img id='" + "like_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' src='" + cutstr1 + "' style='height:15px;width:15px;'>";
                    result_res += "<span id='" + "lalike_but" + ict.Table.Rows[i]["id"].ToString() + "' onclick='like(this.id)' style='color:#F06767;font-size:10pt;'>いいね</span>";
                }
                result_res += "</div>";
                result_res += "</td>";




                result_res += "<td>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td align='center'><br/><br/>";
                result_res += "<div style='cursor: pointer' class='big_mess_box" + ((counn - 10) + i) + "'>";

                string cutstr_m = "~/images/mess_b.png";
                int ind_m = cutstr_m.IndexOf(@"/");
                string cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#CCCCCC;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "<div style='cursor: pointer' class='big_mess_hidde" + ((counn - 10) + i) + "'>";

                cutstr_m = "~/images/mess.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#676767;font-size:10pt;'>コメント</span>";


                result_res += "</div>";
                result_res += "</td>";
                result_res += "<td align='left'><br/><br/>";
                result_res += "<div id='sharebox" + ((counn - 10) + i) + "' style='cursor: pointer'>";
                //pdn2.Controls.Add(new LiteralControl("<div id='sharebox" + i + "' data-tooltip='#html-content" + i + "'>"));
                cutstr_m = "~/images/share_b.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";
                result_res += "<span style='color:#CCCCCC;font-size:10pt;'>シェア</span>";

                result_res += "</div>";
                int len = ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length;
                if (ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Length > 99)
                {
                    len = 99;
                }
                li = new Literal();
                li.Text = @"
                       <div id='share_div" + ((counn - 10) + i) + @"' title='シェア' style='display:none;'><table width='100%'><tr><td><div id='facebook_share" + ((counn - 10) + i) + @"' class='jssocials-share jssocials-share-facebook'><a href='#' class='jssocials-share-link'><i class='fa fa-facebook jssocials-share-logo'></i></a></div></div></td><td><div id='share_div_" + ((counn - 10) + i) + @"'></div></td></tr><tr><td colspan='2'><div id='share_div__" + ((counn - 10) + i) + @"'></div></td></tr></table>

                       <script type='text/javascript'>
  $(function() {
$('#share_div_" + ((counn - 10) + i) + @"').jsSocials({
            showLabel: false,
            showCount: false,
            shares: ['email', 'twitter', 'googleplus', 'line'],
            url: 'http://.jp/',
            text: '地域のいい情報をGETしました！" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "").Substring(0, len) + @"',
            shareIn: 'popup'
        });
$('#share_div" + ((counn - 10) + i) + @"').dialog({
                autoOpen: false,
                show: {
                    effect: 'fold',
                    duration: 100
                },
                hide: {
                    effect: 'fold',
                    duration: 100
                }
            });
   $('#sharebox" + ((counn - 10) + i) + @"').on('click', function () {
                $('#share_div" + i + @"').dialog('open');

           });
$('#facebook_share" + ((counn - 10) + i) + @"').on('click', function () {
               postToWallUsingFBUi('http://.jp/', '" + shareimg + @"','”" + ict.Table.Rows[i]["message"].ToString().Replace("<br/>", "").Replace(@"\t|\n|\r", "").Replace("\r", "").Replace("\n", "") + @"”');

           });
 });
</script>

    ";
                result_res += li.Text;

                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";

                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</td>";
                result_res += "<td style='vertical-align: top;'>";

                //update mess
                cutstr_m = "~/images/edit.png";
                ind_m = cutstr_m.IndexOf(@"/");
                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                result_res += "<img id='" + "editstate_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='update_mess(this.id)' src='" + cutstr_m1 + "' style='cursor: pointer;'>";
                //update mess

                result_res += "</td>";
                result_res += "</tr>";

                result_res += "<tr style='background-color:#F6F7F9;'>";
                result_res += "<td></td>";
                result_res += "<td>";



                result_res += "<div class='status_message_box" + ((counn - 10) + i) + "' style='background-color: #ffffff'>";
                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "<td width='90%' height='5%'></td>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</div >";
                result_res += "<div class='status_message_hidde" + ((counn - 10) + i) + "' style='background-color: #dddddd'>";


                result_res += "<table width='100%' align='left'>";
                //first space way
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "<td width='90%' height='5%'></td>";
                result_res += "<td width='5%' height='5%'></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                //second space way
                result_res += "<td></td>";
                //someone answer
                result_res += "<td>";


                result_res += "<table width='100%' align='left'>";
                //who like this message
                result_res += "<tr>";
                result_res += "<td width='5%' height='10px'></td>";
                result_res += "<td width='90%' height='10px'></td>";
                result_res += "<td width='5%' height='10px'></td>";
                result_res += "</tr>";
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='90%'>";

                Query1 = "select b.username,b.id,a.id as smulid from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
                Query1 += " where a.smid=" + ict.Table.Rows[i]["id"].ToString() + "";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";

                DataView ict3 = gc1.select_cmd(Query1);
                if (ict3.Count > 2)
                {
                    cutstr_m = "~/images/like_b_1.png";
                    ind_m = cutstr_m.IndexOf(@"/");
                    cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                    result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                    for (int iii = 0; iii < 2; iii++)
                    {
                        result_res += "<a href='user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

                        result_res += "、";
                    }
                    result_res += "<a id='listlike_" + ict.Table.Rows[i]["id"].ToString() + "' onclick='check_like_list(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration: none;'>他" + (ict3.Count - 2) + "人</a>";


                    //hyy = new HyperLink();
                    //hyy.NavigateUrl = "javascript:void(0);";
                    //hyy.Target = "_blank";
                    //hyy.Text = "他" + (ict3.Count - 2) + "人";
                    //hyy.Font.Underline = false;
                    //pdn2.Controls.Add(hyy);

                }
                else if (ict3.Count > 0)
                {
                    cutstr_m = "~/images/like_b_1.png";
                    ind_m = cutstr_m.IndexOf(@"/");
                    cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                    result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                    for (int iii = 0; iii < ict3.Count; iii++)
                    {
                        result_res += "<a href='user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString() + "' target='_blank' style='text-decoration:none;'>" + ict3.Table.Rows[iii]["username"].ToString() + "</a>";

                        if (iii != ict3.Count - 1)
                        {
                            result_res += "、";
                        }
                    }
                }

                result_res += "<hr/>";
                result_res += "</td>";
                result_res += "<td width='5%'></td>";
                result_res += "</tr>";
                //who talk about this status message before
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='95%'>";



                Query1 = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level,c.uid";
                Query1 += " from status_messages as a inner join status_messages_user as c";
                Query1 += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                Query1 += " inner join status_messages_user_talk as e on e.smuid=c.id";
                Query1 += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + "";
                Query1 += " ORDER BY e.year desc,e.month desc,e.day desc,e.hour desc,e.minute desc,e.second desc;";
                ict3 = gc1.select_cmd(Query1);
                List<sorttalk> talk_list = new List<sorttalk>();
                sorttalk so = new sorttalk();
                for (int iy = 0; iy < ict3.Count; iy++)
                {
                    so = new sorttalk();
                    so.id = Convert.ToInt32(ict3.Table.Rows[iy]["id"].ToString());
                    so.level = Convert.ToInt32(ict3.Table.Rows[iy]["structure_level"].ToString());
                    so.point_id = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_message_id"].ToString());
                    so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["pointer_user_id"].ToString());
                    so.filename = ict3.Table.Rows[iy]["filename"].ToString();
                    so.mess = ict3.Table.Rows[iy]["message"].ToString();

                    if (ict3.Table.Rows[iy]["pointer_user_id"].ToString() == "0")
                    {

                        so.uid = Convert.ToInt32(ict3.Table.Rows[iy]["uid"].ToString());
                        so.username = ict3.Table.Rows[iy]["username"].ToString();
                        so.photo = ict3.Table.Rows[iy]["photo"].ToString();
                    }
                    else
                    {
                        Query1 = "select username,photo from user_login";
                        Query1 += " where id=" + ict3.Table.Rows[iy]["pointer_user_id"].ToString() + ";";
                        DataView ict5 = gc1.select_cmd(Query1);
                        so.username = ict5.Table.Rows[0]["username"].ToString();
                        so.photo = ict5.Table.Rows[0]["photo"].ToString();
                    }
                    talk_list.Add(so);
                }

                Query1 = "select max(e.structure_level) as maxlevel";
                //sql5.SelectCommand = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
                Query1 += " from status_messages as a inner join status_messages_user as c";
                Query1 += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
                Query1 += " inner join status_messages_user_talk as e on e.smuid=c.id";
                Query1 += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + ";";
                DataView ict4 = gc1.select_cmd(Query1);

                int maxlevel = 0;
                if (ict4.Table.Rows[0]["maxlevel"].ToString() != "")
                {
                    maxlevel = Convert.ToInt32(ict4.Table.Rows[0]["maxlevel"].ToString());
                }

                List<sorttalk> talk_list_tmp = new List<sorttalk>();
                so = new sorttalk();
                for (int ik = 0; ik < talk_list.Count; ik++)
                {
                    if (talk_list[ik].level == 0)
                    {
                        so = new sorttalk();
                        so.id = talk_list[ik].id;
                        so.level = talk_list[ik].level;
                        so.filename = talk_list[ik].filename;
                        so.mess = talk_list[ik].mess;
                        so.photo = talk_list[ik].photo;
                        so.point_id = talk_list[ik].point_id;
                        so.uid = talk_list[ik].uid;
                        so.username = talk_list[ik].username;
                        talk_list_tmp.Add(so);
                    }
                }
                talk_list.Sort((a, b) => a.id.CompareTo(b.id));
                for (int ik = 0; ik < talk_list.Count; ik++)
                {
                    for (int le = 1; le < maxlevel + 1; le++)
                    {
                        if (talk_list[ik].level == le)
                        {
                            so = new sorttalk();
                            so.id = talk_list[ik].id;
                            so.level = talk_list[ik].level;
                            so.filename = talk_list[ik].filename;
                            so.mess = talk_list[ik].mess;
                            so.photo = talk_list[ik].photo;
                            so.point_id = talk_list[ik].point_id;
                            so.uid = talk_list[ik].uid;
                            so.username = talk_list[ik].username;
                            for (int ikk = 0; ikk < talk_list_tmp.Count; ikk++)
                            {
                                if (talk_list_tmp[ikk].id == talk_list[ik].point_id)
                                {
                                    talk_list_tmp.Insert(ikk + 1, so);
                                }
                            }
                        }
                    }
                }
                Image img2 = new Image();
                if (ict3.Count > 1)
                {
                    //show div
                    result_res += "<div class='mess_box" + ((counn - 10) + i) + "'>";
                    result_res += "<table width='100%'>";
                    result_res += "<tr>";
                    result_res += "<td width='100%' align='left' colspan='2'>";

                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>以前のコメントを見る</a>";

                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "<tr>";
                    result_res += "<td width='10%' rowspan='2' valign='top'>";


                    result_res += "<div class='zoom-gallery'>";

                    cutstr2 = talk_list_tmp[0].photo;
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:32px;height:32px;'>";
                    result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                    result_res += "</a>";

                    result_res += "</div>";


                    result_res += "</td>";
                    result_res += "<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">";
                    result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[0].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[0].username.ToString() + "</a>";

                    result_res += "<br/>";
                    result_res += ict3.Table.Rows[0]["message"].ToString();
                    result_res += "<br/>";

                    if (talk_list_tmp[0].filename != "")
                    {
                        result_res += "<div class='zoom-gallery'>";

                        cutstr2 = talk_list_tmp[0].filename;
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                        result_res += "</a>";

                        result_res += "</div>";

                        result_res += "<br/>";

                    }

                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "<tr>";
                    result_res += "<td>";
                    //who talk about status message and who like

                    Query1 = "select good_status from status_messages_user_talk_like";
                    Query1 += " where smutid='" + talk_list_tmp[0].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                    DataView ict_who_like = gc1.select_cmd(Query1);
                    if (ict_who_like.Count > 0)
                    {
                        if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='sblike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                        }
                        else
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='slike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";
                        }
                    }
                    else
                    {
                        result_res += "<a id='" + "wholike_" + talk_list_tmp[0].id + "_s" + "' onclick='sblike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                    }


                    result_res += "&nbsp;&nbsp;";

                    result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

                    //who like who answer post message
                    Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                    Query1 += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                    //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_who_like = gc1.select_cmd(Query1);
                    if (ict_who_like.Count > 0)
                    {
                        cutstr_m = "~/images/like_b_1.png";
                        ind_m = cutstr_m.IndexOf(@"/");
                        cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                        result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                        result_res += "<a href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                    }
                    //who like who answer post message



                    result_res += "</td>";
                    result_res += "</tr>";
                    result_res += "</table>";
                    result_res += "</div>";
                    //hidde message
                    result_res += "<div class='mess_hidde" + ((counn - 10) + i) + "'>";
                    result_res += "<table width='100%'>";
                    for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                    {

                        result_res += "<table width='100%'>";
                        result_res += "<tr>";
                        int wid = (10 + (10 * talk_list_tmp[iiii].level));
                        if (wid > 90) { wid = 90; }
                        result_res += "<td width='" + wid + "%' align='right' rowspan='2' valign='top'>";

                        result_res += "<div class='zoom-gallery'>";

                        cutstr2 = talk_list_tmp[iiii].photo;
                        ind2 = cutstr2.IndexOf(@"/");
                        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                        result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>";
                        result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                        result_res += "</a>";

                        result_res += "</div>";


                        result_res += "</td>";

                        result_res += "<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">";

                        result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";
                        result_res += "<br/>";
                        result_res += talk_list_tmp[iiii].mess.ToString();
                        result_res += "<br/>";

                        if (talk_list_tmp[iiii].filename.ToString() != "")
                        {
                            result_res += "<div class='zoom-gallery'>";

                            cutstr2 = talk_list_tmp[iiii].filename.ToString();
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>";
                            result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                            result_res += "</a>";

                            result_res += "</div>";
                            result_res += "<br/>";
                        }

                        result_res += "</td>";
                        result_res += "</tr>";
                        result_res += "<tr>";
                        result_res += "<td>";


                        //who talk about status message and who like
                        Query1 = "select good_status from status_messages_user_talk_like";
                        Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                        ict_who_like = gc1.select_cmd(Query1);
                        if (ict_who_like.Count > 0)
                        {
                            if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                            }
                            else
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='like_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";
                            }
                        }
                        else
                        {
                            result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                        }
                        result_res += "&nbsp;&nbsp;";
                        result_res += "<a id='" + "whowantans_" + talk_list_tmp[iiii].id + "' onclick='who_answer(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

                        //who like who answer post message
                        Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                        Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                        //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                        ict_who_like = gc1.select_cmd(Query1);
                        if (ict_who_like.Count > 0)
                        {
                            cutstr_m = "~/images/like_b_1.png";
                            ind_m = cutstr_m.IndexOf(@"/");
                            cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                            result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                            result_res += "<a id='" + "likecount" + talk_list_tmp[iiii].id + "' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                        }
                        //who like who answer post message

                        result_res += "</td>";
                        result_res += "</tr>";


                        result_res += "</table>";
                        result_res += "<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>";
                    }
                    result_res += "</div>";

                }
                else
                {
                    if (ict3.Count > 0)
                    {
                        for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                        {

                            result_res += "<table width='100%'>";
                            result_res += "<tr>";
                            int wid = (10 + (10 * talk_list_tmp[iiii].level));
                            if (wid > 90) { wid = 90; }
                            result_res += "<td width='" + wid + "%' align='right' rowspan='2' valign='top'>";


                            result_res += "<div class='zoom-gallery'>";

                            cutstr2 = talk_list_tmp[iiii].photo;
                            ind2 = cutstr2.IndexOf(@"/");
                            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                            result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:32px;height:32px;'>";
                            result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='32' height='32' />";
                            result_res += "</a>";

                            result_res += "</div>";

                            result_res += "</td>";

                            result_res += "<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">";
                            result_res += "<a href='user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString() + "' target='_blank' style='text-decoration:none;'>" + talk_list_tmp[iiii].username.ToString() + "</a>";

                            result_res += "<br/>";
                            result_res += talk_list_tmp[iiii].mess.ToString();
                            result_res += "<br/>";

                            if (talk_list_tmp[iiii].filename.ToString() != "")
                            {
                                result_res += "<div class='zoom-gallery'>";

                                cutstr2 = talk_list_tmp[iiii].filename.ToString();
                                ind2 = cutstr2.IndexOf(@"/");
                                cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>";
                                result_res += "<img class='lazy' data-src='" + cutstr3 + "' src='images/loading.gif' width='50' height='50' />";
                                result_res += "</a>";

                                result_res += "</div>";
                                result_res += "<br/>";
                            }

                            result_res += "</td>";
                            result_res += "</tr>";
                            result_res += "<tr>";
                            result_res += "<td>";


                            //who talk about status message and who like

                            Query1 = "select good_status from status_messages_user_talk_like";
                            Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + HttpContext.Current.Session["id"].ToString() + "';";
                            DataView ict_who_like = gc1.select_cmd(Query1);
                            if (ict_who_like.Count > 0)
                            {
                                if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                                {
                                    result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                                }
                                else
                                {
                                    result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='like_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#D84C4B;text-decoration:none;'>いいね!</a>";

                                }
                            }
                            else
                            {
                                result_res += "<a id='" + "wholike_" + talk_list_tmp[iiii].id + "' onclick='blike_who_answer(this.id)' href='javascript:void(0);' target='_blank' style='color:#4183C4;text-decoration:none;'>いいね!</a>";
                            }
                            result_res += "&nbsp;&nbsp;";
                            result_res += "<a id='" + "whowantans_" + talk_list_tmp[iiii].id + "' onclick='who_answer(this.id)' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>返信</a>";

                            //who like who answer post message
                            Query1 = "select count(*) as howmany from status_messages_user_talk_like";
                            Query1 += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                            //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                            ict_who_like = gc1.select_cmd(Query1);
                            if (ict_who_like.Count > 0)
                            {
                                cutstr_m = "~/images/like_b_1.png";
                                ind_m = cutstr_m.IndexOf(@"/");
                                cutstr_m1 = cutstr_m.Substring(ind_m + 1, cutstr_m.Length - ind_m - 1);
                                result_res += "<img src='" + cutstr_m1 + "' style='height:15px;width:15px;'>";

                                result_res += "<a id='" + "likecount" + talk_list_tmp[iiii].id + "' href='javascript:void(0);' target='_blank' style='text-decoration:none;'>" + ict_who_like.Table.Rows[0]["howmany"].ToString() + "</a>";
                            }
                            //who like who answer post message

                            result_res += "</td>";
                            result_res += "</tr>";


                            result_res += "</table>";

                            //user answer user answer
                            result_res += "<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>";



                        }
                    }
                }

                result_res += "</td>";
                result_res += "<td width='5%'></td>";
                result_res += "</tr>";


                result_res += "</table>";


                result_res += "</td>";
                //second space way
                result_res += "<td></td>";
                result_res += "</tr>";
                //third space way
                result_res += "<tr>";
                result_res += "<td></td>";
                result_res += "<td>";

                result_res += "<table width='100%'>";
                result_res += "<tr>";
                result_res += "<td width='5%'></td>";
                result_res += "<td width='10%' valign='top'>";
                //user photo

                result_res += "<div class='zoom-gallery'>";

                Query1 = "select photo,username from user_login ";
                Query1 += " where id='" + HttpContext.Current.Session["id"].ToString() + "';";
                DataView ict2 = gc1.select_cmd(Query1);
                string userr = "";
                if (ict2.Count > 0)
                {
                    cutstr2 = ict2.Table.Rows[0]["photo"].ToString();
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    userr = ict2.Table.Rows[0]["username"].ToString();
                }

                result_res += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + userr + "' style='width:32px;height:32px;'>";
                result_res += "<img src='" + cutstr3 + "' width='32' height='32' />";
                result_res += "</a>";

                result_res += "</div>";

                result_res += "</td>";
                result_res += "<td width='85%'>";

                //user answer
                result_res += "<input type='text' id='why" + ict.Table.Rows[i]["id"].ToString() + "_" + ((counn - 10) + i) + "' onkeypress='sendmessage(event,this.id)'  placeholder='コメントする' style='width: 80%;height:30px;' title='【Enter】キーを押してください'>";
                //TextBox tex = new TextBox();
                //tex.Width = Unit.Percentage(95);
                //tex.Height = 30;
                //tex.ID = "send" + ict.Table.Rows[i]["id"].ToString();
                //tex.Attributes["onKeydown"] = "Javascript: if (event.which == 13 || event.keyCode == 13) sendmessage(this.id);";
                //tex.Attributes.Add("placeholder", "コメントする");
                //pdn2.Controls.Add(tex);

                //pdn2.Controls.Add(new LiteralControl("<br/>"));

                result_res += @"
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
            <input type='file' name='file' id='btnFileUpload" + ((counn - 10) + i) + @"' />
</label>
<br />
            <div id='progressbar" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
                <div>
                    読み込み中
                </div>
            </div>
<br />
                <div id='image_place" + ((counn - 10) + i) + @"' style='width:100px;display:none;'>
                    <div>
                        <img id='make-image" + ((counn - 10) + i) + @"' alt='' src='' width='100px' height='100px'/>
                    </div>
                </div>
";




                //pdn2.Controls.Add(new LiteralControl(@"<label class='file-upload'><span><strong>アップロード</strong></span>"));

                //FileUpload fi=new FileUpload();
                //fi.ID="fuDocument_"+i;
                //fi.Attributes.Add("onchange", "UploadFile(this,this.id);");
                //pdn2.Controls.Add(fi);
                //pdn2.Controls.Add(new LiteralControl(@"</label><br />"));



                //Button but = new Button();
                //but.ID = "btnUploadDoc_" + i;
                //but.Text = "Upload";
                //but.Click += new System.EventHandler(this.UploadDocument);
                //but.OnClientClick = "ShowProgressBar();";
                //but.Style["display"] = "none";
                //pdn2.Controls.Add(but);

                //img1 = new Image();
                //img1.Width = 100; img1.Height = 150;
                //img1.ID = "Image_" + i;
                //img1.Visible = false;


                //pdn2.Controls.Add(img1);



                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";


                result_res += "</div>";


                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";
                //fourth space way
                result_res += "<tr>";
                result_res += "<td width='5%' height='5%'><br/></td>";
                result_res += "<td width='90%' height='5%'><br/></td>";
                result_res += "<td width='5%' height='5%'><br/></td>";
                result_res += "</tr>";
                result_res += "</table>";
                result_res += "</td>";
                result_res += "<td></td>";
                result_res += "</tr>";



                result_res += "</table>";
                //pdn2.Controls.Add(new LiteralControl("<br/><br/>"));
                result_res += "</td>";
                result_res += "</tr>";
                result_res += "</table>";



                result_res += "</div>";
            }
            result_res += javascr;
        }



        return result_res;
    }
    public static bool UrlExists(string url)
    {
        try
        {
            new System.Net.WebClient().DownloadData(url);
            return true;
        }
        catch (System.Net.WebException e)
        {
            return false;
            throw;
        }
    }
    public static string ConvertUrlsToDIV(string url)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string res = "";

        Query1 = "select * from status_messages_link_info where link like '" + url + "';";

        DataView ict1 = gc1.select_cmd(Query1);
        if (ict1.Count > 0)
        {
            string sharetxt = "";
            if (ict1.Table.Rows[0]["title"].ToString() != "")
            {
                sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;line-height:30px;'>【" + ict1.Table.Rows[0]["title"].ToString() + "】</span>";
            }
            if (ict1.Table.Rows[0]["des"].ToString() != "")
            {
                sharetxt += "<br/><span style='font-size:medium;color:black;line-height:27px;'>" + ict1.Table.Rows[0]["des"].ToString() + "</span>";
            }
            res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + ict1.Table.Rows[0]["link"].ToString() + "' style='text-decoration:none'>";
            if (ict1.Table.Rows[0]["image_url"].ToString() != "")
            {
                res += "<img class='lazy' data-src='" + ict1.Table.Rows[0]["image_url"].ToString() + "' src='images/loading.gif' alt='' width='100%' height='200px' border='0' />";
            }
            if (sharetxt != "")
            {
                res += sharetxt;
            }
            res += "</a></div>";

        }
        else
        {
            WebService.LinkDetails wss = new WebService.LinkDetails();
            WebService ws = new WebService();
            wss = ws.GetDetails(url);
            string imgurl = "";
            if (wss.Image != null)
            {
                imgurl = wss.Image.Url;
            }
            else if (wss.Images != null)
            {
                if (wss.Images.Count > 0)
                {
                    imgurl = wss.Images[0].Url;
                }
            }
            URL_data urld = new URL_data();
            urld.url = wss.Url;
            string sharetxt = "";
            if (wss.Title != null)
            {
                urld.title = wss.Title;
                sharetxt += "<br/><span style='font-size:x-large;color:black;font-weight:bold;line-height:30px;'>【" + wss.Title + "】</span>";
            }
            if (wss.Description != null)
            {
                urld.des = wss.Description;
                sharetxt += "<br/><span style='font-size:medium;color:black;line-height:27px;'>" + wss.Description + "</span>";
            }

            res = "<div style='border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;word-break:break-all;width:100%;'><a href='" + wss.Url + "' style='text-decoration:none'>";
            if (imgurl != "")
            {
                if (UrlExists(imgurl))
                {
                    urld.image_url = imgurl;
                    res += "<img class='lazy' data-src='" + imgurl + "' src='images/loading.gif' alt='' width='100%' height='200px' border='0' />";
                }
            }
            if (sharetxt != "")
            {
                res += sharetxt;
            }
            res += "</a></div>";

            Query1 = "insert into status_messages_link_info(link,image_url,title,des,update_time)";
            Query1 += " values('" + url + "','" + urld.image_url + "','" + urld.title + "','" + urld.des + "',NOW());";
            resin = gc1.insert_cmd(Query1);

        }

        return res;
    }
    public static string ConvertUrlsToLinks_DIV(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);
        string txt = "";
        MatchCollection mactches = r.Matches(msg);
        foreach (Match match in mactches)
        {
            txt += ConvertUrlsToDIV(match.Value);
        }
        return txt;
    }
    public static string ConvertUrlsToLinks(string msg)
    {
        string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        Regex r = new Regex(regex, RegexOptions.IgnoreCase);

        MatchCollection mactches = r.Matches(msg);
        string txt = "";
        foreach (Match match in mactches)
        {
            //txt += GetMetaTagValue(match.Value) + ",";
            msg = msg.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
        }
        return msg;
        //return txt;

        //        msg = Regex.Replace(
        //msg,
        //@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])",
        //delegate(Match match)
        //{
        //    return GetMetaTagValue(match.ToString());

        //    //return string.Format("{0}", match.ToString());
        //});

        //        return msg;

        //return r.Replace(msg, "$1");

        //return GetMetaTagValue(r.Replace(msg, "$1"));

        //return r.Replace(msg, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
    }
    public static string GetMetaTagValue(string url)
    {
        string res = "";
        //Get Title
        WebClient x = new WebClient();
        string source = x.DownloadString(url);
        res = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        return res;
    }
    [WebMethod(EnableSession = true)]
    public static string update_message(string param1, string param2,string param3,string param4)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";
        if (HttpContext.Current.Session["id"] != null)
        {
            string uid = HttpContext.Current.Session["id"].ToString();
            if (uid.Trim() != "")
            {
                string smid = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                string post_mess = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim().Replace(System.Environment.NewLine, "<br/>");
                string post_mess_typ = param3.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
                if (post_mess_typ == "")
                {
                    post_mess_typ = "6";
                }
                string type = "0";
                if (param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "一般公開")
                {
                    type = "0";
                }
                else if (param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "地域限定")
                {
                    type = "1";
                }
                else if (param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "友達")
                {
                    type = "2";
                }
                else if (param4.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "")
                {
                    type = "0";
                }

                Query1 = "select uid from status_messages";
                Query1 += " where id='" + smid + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    if (uid == ict_f.Table.Rows[0]["uid"].ToString())
                    {
                        //update
                        //ict_f.Table.Rows[0]["id"].ToString()
                        Query1 = "update status_messages set type='" + type + "',message_type='" + post_mess_typ + "',message='" + post_mess + "'";
                        Query1 += " where id='" + smid + "';";
                        resin = gc1.update_cmd(Query1);

                    }
                }
            }
        }

        return result;
    }
    [WebMethod]
    public static string[] count_list(string param1)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1;
        string[] result_res = new string[3];
        result = "";
        //friend post message
        List<string> user_friend = new List<string>();
        Query1 = "select c.id,c.username,c.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_ff = gc1.select_cmd(Query1);
        if (ict_ff.Count > 0)
        {
            for (int ii = 0; ii < ict_ff.Count; ii++)
            {
                user_friend.Add(ict_ff.Table.Rows[ii]["id"].ToString());
            }
        }

        Query1 = "select b.id,b.username,b.photo";
        Query1 += " from user_friendship as a";
        Query1 += " inner join user_login as b on b.id=a.first_uid";
        Query1 += " inner join user_login as c on c.id=a.second_uid";

        //check by type use type=0,1
        Query1 += " where c.id='" + param1.Trim() + "'";
        Query1 += " and first_check_connect=1 and second_check_connect=1;";
        DataView ict_f1_f = gc1.select_cmd(Query1);
        if (ict_f1_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f1_f.Count; ii++)
            {
                user_friend.Add(ict_f1_f.Table.Rows[ii]["id"].ToString());
            }
        }
        //friend post message
        //status message

        Query1 = "select a.id,a.message";
        Query1 += " from status_messages as a";
        Query1 += " where a.uid='" + param1 + "'";
        Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_ind = new List<status_mess_list>();
        status_mess_list sml = new status_mess_list();
        for (int i = 0; i < ict_f.Count; i++)
        {
            sml = new status_mess_list();
            sml.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
            sml.message = ict_f.Table.Rows[i]["message"].ToString();
            smlist_ind.Add(sml);
        }
        List<status_mess_list_like> status_mess_like = new List<status_mess_list_like>();
        status_mess_list_like smll = new status_mess_list_like();
        //friend post message
        List<status_mess_list> smlist_ind_f = new List<status_mess_list>();
        for (int i = 0; i < user_friend.Count; i++)
        {
            //friend post
            Query1 = "select a.id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
            Query1 += " from status_messages as a";
            Query1 += " where a.uid='" + user_friend[i] + "'";
            Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            DataView ict_f12 = gc1.select_cmd(Query1);
            if (ict_f12.Count > 0)
            {
                for (int ii = 0; ii < ict_f12.Count; ii++)
                {

                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 2;
                    smll.like_id = Convert.ToInt32(ict_f12.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f12.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(user_friend[i]);
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f12.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //friend like
            Query1 = "select a.id,a.message,a.uid as uuid,b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where b.uid='" + user_friend[i] + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f12 = gc1.select_cmd(Query1);
            if (ict_f12.Count > 0)
            {
                for (int ii = 0; ii < ict_f12.Count; ii++)
                {
                    smll = new status_mess_list_like();
                    //check big message
                    smll.type = 3;
                    smll.like_id = Convert.ToInt32(ict_f12.Table.Rows[ii]["id"].ToString());
                    smll.like_message = ict_f12.Table.Rows[ii]["message"].ToString();
                    smll.uid = Convert.ToInt32(ict_f12.Table.Rows[ii]["uid"].ToString());
                    List<int> idl = new List<int>();
                    idl.Add(Convert.ToInt32(ict_f12.Table.Rows[ii]["uuid"].ToString()));
                    smll.like_idlist = idl;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f12.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f12.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f12.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
        }
        //friend post message
        for (int i = 0; i < smlist_ind.Count; i++)
        {
            Query1 = "select b.uid,b.year,b.month,b.day,b.hour,b.minute,b.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user_like as b on a.id=b.smid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and b.uid!='" + param1 + "' and b.good_status=1";
            Query1 += " ORDER BY b.year desc,b.month desc,b.day desc,b.hour desc,b.minute desc,b.second desc;";
            ict_f = gc1.select_cmd(Query1);
            if (ict_f.Count > 0)
            {
                smll = new status_mess_list_like();
                //check big message
                smll.type = 1;
                smll.like_id = smlist_ind[i].id;
                smll.like_message = smlist_ind[i].message;
                smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                    , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                List<int> idl = new List<int>();
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    idl.Add(Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString()));
                }
                smll.like_idlist = idl;
                status_mess_like.Add(smll);
            }
            //user answer status message
            Query1 = "select c.id,b.uid,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
            Query1 += " from status_messages as a";
            Query1 += " inner join status_messages_user as b on a.id=b.smid";
            Query1 += " inner join status_messages_user_talk as c on b.id=c.smuid";
            Query1 += " where a.id='" + smlist_ind[i].id + "' and c.structure_level=0";
            Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
            ict_f = gc1.select_cmd(Query1);
            List<status_mess_list> smlist_small_ind = new List<status_mess_list>();
            sml = new status_mess_list();
            if (ict_f.Count > 0)
            {
                for (int ii = 0; ii < ict_f.Count; ii++)
                {
                    if (ict_f.Table.Rows[ii]["uid"].ToString() == param1)
                    {
                        sml = new status_mess_list();
                        sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                        sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                        smlist_small_ind.Add(sml);
                    }

                    smll = new status_mess_list_like();
                    smll.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                    smll.uid = Convert.ToInt32(ict_f.Table.Rows[ii]["uid"].ToString());
                    smll.message = smlist_ind[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[ii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[ii]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[ii]["second"].ToString()));
                    status_mess_like.Add(smll);
                }
            }
            //if user answer user self and who answer user
            if (smlist_small_ind.Count > 0)
            {
                for (int ii = 0; ii < smlist_small_ind.Count; ii++)
                {
                    Query1 = "select a.id,a.pointer_user_id,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk as a";
                    Query1 += " where a.pointer_message_id='" + smlist_small_ind[ii].id + "' and a.structure_level=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            smll = new status_mess_list_like();
                            smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                            smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                            smll.message = smlist_small_ind[ii].message;
                            smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                                , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                            status_mess_like.Add(smll);
                        }
                    }
                    //who like user answer
                    Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                    Query1 += " from status_messages_user_talk_like as a";
                    Query1 += " where a.smutid='" + smlist_small_ind[ii].id + "' and a.good_status=1";
                    Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    ict_f = gc1.select_cmd(Query1);

                    if (ict_f.Count > 0)
                    {
                        smll = new status_mess_list_like();
                        smll.like_id = smlist_small_ind[ii].id;
                        smll.like_message = smlist_small_ind[ii].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                        List<int> idl = new List<int>();
                        for (int iii = 0; iii < ict_f.Count; iii++)
                        {
                            idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                        }
                        smll.like_idlist = idl;
                        status_mess_like.Add(smll);
                    }


                }
            }


        }
        //user answer other user answer status message
        Query1 = "select c.id,c.message,c.year,c.month,c.day,c.hour,c.minute,c.second";
        Query1 += " from status_messages_user_talk as c";
        Query1 += " where c.pointer_user_id='" + param1 + "' and c.structure_level>0";
        Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
        ict_f = gc1.select_cmd(Query1);
        List<status_mess_list> smlist_small_ind1 = new List<status_mess_list>();
        sml = new status_mess_list();
        if (ict_f.Count > 0)
        {
            for (int ii = 0; ii < ict_f.Count; ii++)
            {
                sml = new status_mess_list();
                sml.id = Convert.ToInt32(ict_f.Table.Rows[ii]["id"].ToString());
                sml.message = ict_f.Table.Rows[ii]["message"].ToString();
                smlist_small_ind1.Add(sml);
            }
        }
        if (smlist_small_ind1.Count > 0)
        {
            for (int i = 0; i < smlist_small_ind1.Count; i++)
            {
                Query1 = "select a.uid,a.year,a.month,a.day,a.hour,a.minute,a.second";
                Query1 += " from status_messages_user_talk_like as a";
                Query1 += " where a.smutid='" + smlist_small_ind1[i].id + "' and a.good_status=1";
                Query1 += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                ict_f = gc1.select_cmd(Query1);

                if (ict_f.Count > 0)
                {
                    smll = new status_mess_list_like();
                    smll.like_id = smlist_small_ind1[i].id;
                    smll.like_message = smlist_small_ind1[i].message;
                    smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[0]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["month"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[0]["minute"].ToString())
                        , Convert.ToInt32(ict_f.Table.Rows[0]["second"].ToString()));
                    List<int> idl = new List<int>();
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        idl.Add(Convert.ToInt32(ict_f.Table.Rows[iii]["uid"].ToString()));
                    }
                    smll.like_idlist = idl;
                    status_mess_like.Add(smll);
                }

                Query1 = "select c.id,c.pointer_user_id,c.year,c.month,c.day,c.hour,c.minute,c.second";
                Query1 += " from status_messages_user_talk as c";
                Query1 += " where c.pointer_message_id='" + smlist_small_ind1[i].id + "'";
                Query1 += " ORDER BY c.year desc,c.month desc,c.day desc,c.hour desc,c.minute desc,c.second desc;";
                ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    for (int iii = 0; iii < ict_f.Count; iii++)
                    {
                        smll = new status_mess_list_like();
                        smll.id = Convert.ToInt32(ict_f.Table.Rows[iii]["id"].ToString());
                        smll.uid = Convert.ToInt32(ict_f.Table.Rows[iii]["pointer_user_id"].ToString());
                        smll.message = smlist_small_ind1[i].message;
                        smll.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[iii]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["month"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[iii]["minute"].ToString())
                            , Convert.ToInt32(ict_f.Table.Rows[iii]["second"].ToString()));
                        status_mess_like.Add(smll);
                    }
                }

            }
        }


        status_mess_like.Sort((x, y) => -x.comdate.CompareTo(y.comdate));
        DateTime nowtime = DateTime.Now;
        DateTime clicktime = new DateTime(2000, 1, 1);
        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='0';";
        DataView ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newmessage = 0;
        for (int i = 0; i < status_mess_like.Count; i++)
        {
            int year = status_mess_like[i].comdate.Year;
            int month = status_mess_like[i].comdate.Month;
            int day = status_mess_like[i].comdate.Day;
            int hour = status_mess_like[i].comdate.Hour;
            int min = status_mess_like[i].comdate.Minute;
            int sec = status_mess_like[i].comdate.Second;
            DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
            if (mesgdate > clicktime && mesgdate < nowtime)
            {
                newmessage += 1;
            }
        }
        result_res[0] = newmessage.ToString();


        //chat list count
        Query1 = "select DISTINCT a.to_uid,c.id,c.username,c.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where b.id='" + param1 + "'";
        Query1 += " ORDER BY a.to_uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f = gc1.select_cmd(Query1);

        List<friend_list_chat> fri = new List<friend_list_chat>();
        friend_list_chat frii = new friend_list_chat();
        int tempid = 0;
        for (int i = 0; i < ict_f.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f.Table.Rows[i]["id"].ToString());
        }

        Query1 = "select DISTINCT a.uid,b.id,b.username,b.photo,a.talk_message,a.year,a.month,a.day,a.hour,a.minute,a.second";
        Query1 += " from user_chat_room as a";
        Query1 += " inner join user_login as b on b.id=a.uid";
        Query1 += " inner join user_login as c on c.id=a.to_uid";

        //check by type use type=0,1
        Query1 += " where c.id=" + param1 + "";
        Query1 += " ORDER BY a.uid asc,a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        ict_f1 = gc1.select_cmd(Query1);
        tempid = 0;
        for (int i = 0; i < ict_f1.Count; i++)
        {
            if (tempid != Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString()))
            {
                frii = new friend_list_chat();
                frii.id = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
                frii.photo = ict_f1.Table.Rows[i]["photo"].ToString();
                frii.username = ict_f1.Table.Rows[i]["username"].ToString();
                frii.mesg = ict_f1.Table.Rows[i]["talk_message"].ToString();
                frii.year = Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString());
                frii.month = Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString());
                frii.day = Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString());
                frii.hour = Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString());
                frii.min = Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString());
                frii.sec = Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString());
                frii.comdate = new DateTime(Convert.ToInt32(ict_f1.Table.Rows[i]["year"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["month"].ToString()),
                    Convert.ToInt32(ict_f1.Table.Rows[i]["day"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["hour"].ToString()), Convert.ToInt32(ict_f1.Table.Rows[i]["minute"].ToString()),
                     Convert.ToInt32(ict_f1.Table.Rows[i]["second"].ToString()));
                fri.Add(frii);
            }

            tempid = Convert.ToInt32(ict_f1.Table.Rows[i]["id"].ToString());
        }

        fri = fri.OrderBy(c => c.id).ToList();

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //        .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();

        List<friend_list_chat> tmp_fri = new List<friend_list_chat>();
        List<friend_list_chat> fri_total = new List<friend_list_chat>();
        frii = new friend_list_chat();
        List<int> fri_ind = new List<int>();
        tempid = 0;
        for (int i = 0; i < fri.Count; i++)
        {
            if (tempid != fri[i].id)
            {
                tempid = fri[i].id;
                fri_ind.Add(tempid);
            }
        }
        for (int i = 0; i < fri_ind.Count; i++)
        {
            tmp_fri = new List<friend_list_chat>();
            for (int ii = 0; ii < fri.Count; ii++)
            {
                if (fri_ind[i] == fri[ii].id)
                {
                    tmp_fri.Add(fri[ii]);
                }
            }
            tmp_fri.Sort((x, y) => DateTime.Compare(x.comdate, y.comdate));
            fri_total.Add(tmp_fri[tmp_fri.Count - 1]);
        }
        fri_total.Sort((x, y) => -x.comdate.CompareTo(y.comdate));

        //fri = fri.OrderBy(c => c.id).ThenByDescending(c => c.year).ThenByDescending(c => c.month).ThenByDescending(c => c.day)
        //       .ThenByDescending(c => c.hour).ThenByDescending(c => c.min).ThenByDescending(c => c.sec).ToList();
        fri = fri_total;
        nowtime = DateTime.Now;
        clicktime = new DateTime(2000, 1, 1);
        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='1';";
        ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newchat = 0;
        for (int i = 0; i < fri.Count; i++)
        {

            int year = fri[i].year;
            int month = fri[i].month;
            int day = fri[i].day;
            int hour = fri[i].hour;
            int min = fri[i].min;
            int sec = fri[i].sec;
            DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
            if (mesgdate > clicktime && mesgdate < nowtime)
            {
                newchat += 1;
            }
        }
        result_res[1] = newchat.ToString();
        nowtime = DateTime.Now;
        clicktime = new DateTime(2000, 1, 1);
        Query1 = "select check_time from user_notice_check";
        Query1 += " where uid='" + param1 + "' and type='2';";
        ict_f1 = gc1.select_cmd(Query1);
        if (ict_f1.Count > 0)
        {
            clicktime = Convert.ToDateTime(ict_f1.Table.Rows[0]["check_time"].ToString());
        }
        int newfri = 0;
        Query1 = "select a.id,a.first_uid,b.username,b.photo,a.first_date_year,a.first_date_month,a.first_date_day,a.first_date_hour,a.first_date_minute,a.first_date_second ";
        Query1 += "from user_friendship as a inner join user_login as b on a.first_uid=b.id where a.second_uid='" + param1 + "' and a.second_check_connect='0'";
        Query1 += " ORDER BY a.first_date_year desc,a.first_date_month desc,a.first_date_day desc,a.first_date_hour desc,a.first_date_minute desc,a.first_date_second desc;";
        DataView ict_h_fri_notice = gc1.select_cmd(Query1);
        if (ict_h_fri_notice.Count > 0)
        {
            for (int i = 0; i < ict_h_fri_notice.Count; i++)
            {
                int year = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_year"].ToString());
                int month = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_month"].ToString());
                int day = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_day"].ToString());
                int hour = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_hour"].ToString());
                int min = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_minute"].ToString());
                int sec = Convert.ToInt32(ict_h_fri_notice.Table.Rows[i]["first_date_second"].ToString());
                DateTime mesgdate = new DateTime(year, month, day, hour, min, sec);
                if (mesgdate > clicktime && mesgdate < nowtime)
                {
                    newfri += 1;
                }
            }
        }
        result_res[2] = newfri.ToString();



        return result_res;
    }
}
