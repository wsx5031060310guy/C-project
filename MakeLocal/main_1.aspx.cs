﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class main : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Label_logo.Attributes.Add("onclick", "javascript:self.location='home.aspx';");
        Label_logo.Style["cursor"] = "pointer";
        if (!Page.ClientScript.IsStartupScriptRegistered("alert"))
        {
            if (Session["id"] == null || Session["id"].ToString() == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "1", "showDialog_login();", true);
            }
        }
        if (!IsPostBack)
        {
            Session["message_type"] = null;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        SqlDataSource sql1 = new SqlDataSource();
        sql1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        sql1.SelectCommand = "select a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo ";
        sql1.SelectCommand += "from status_messages as a";
        sql1.SelectCommand += " inner join user_login as b on b.id=a.uid";

        //if want to class by type use type=0,1,2 ; message_type=0,1,2

        ////Before today's message
        //sql1.SelectCommand += " where a.year<=" + DateTime.Now.Year.ToString() + " and a.month<=" + DateTime.Now.Month.ToString();
        //sql1.SelectCommand += " and a.day<=" + DateTime.Now.Day.ToString() + " ";


        ////type message select
        if(Session["message_type"]!=null)
        {
            if (Session["message_type"].ToString() != "")
            {
                sql1.SelectCommand += " where a.message_type='" + Session["message_type"].ToString() + "'";
            }
        }

        //sql1.SelectCommand += " and a.day<=" + DateTime.Now.Day.ToString() + " and a.hour<="+Convert.ToInt32(DateTime.Now.ToString("HH")).ToString();
        //sql1.SelectCommand += " and a.minute<=" + DateTime.Now.Minute.ToString() + " and a.second<=" + DateTime.Now.Second.ToString();

        sql1.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
        DataView ict = (DataView)sql1.Select(DataSourceSelectArguments.Empty);


        Literal li = new Literal();

        li.Text = @"<script>

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

            $('.hidde" + i + @"').click(function () {
                $('.box" + i + @"').toggle();
                $('.hidde" + i + @"').toggle(false);
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

            $('.sharehidde" + i + @"').toggle(false);

            $('.sharebox" + i + @"').click(function () {
                $('.sharehidde" + i + @"').toggle();
                $('.sharebox" + i + @"').toggle(false);
            })

            $('.sharehidde" + i + @"').click(function () {
                $('.sharebox" + i + @"').toggle();
                $('.sharehidde" + i + @"').toggle(false);
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

            SqlDataSource sql3 = new SqlDataSource();
            sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b on a.id=b.smid";
            sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
            DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
            if (ict2.Count > 3)
            {
                li.Text += @"
$('.imhidde" + i + @"').toggle(false);
$('.imhiddee" + i + @"').toggle(false);

            $('.imbox" + i + @"').click(function () {
                $('.imhidde" + i + @"').toggle();
                $('.imhiddee" + i + @"').toggle();
                $('.imbox" + i + @"').toggle(false);
            })

            $('.imhiddee" + i + @"').click(function () {
                $('.imbox" + i + @"').toggle();
                $('.imhidde" + i + @"').toggle(false);
                $('.imhiddee" + i + @"').toggle(false);
            })


";
            }
        }

        li.Text += @"
                        })";
        li.Text += @"</script>";

        Panel pdn_j = (Panel)this.FindControl("javaplace");
        pdn_j.Controls.Add(li);

        //this.Page.Controls.Add(li);


        //this.Page.Header.Controls.Add(li);
        ////添加至指定位置
        //this.Page.Header.Controls.AddAt(0, li);



        Panel pdn2 = (Panel)this.FindControl("Panel2");


        for (int i = 0; i < ict.Count; i++)
        {
            //big message place
            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'></td><td width='90%' height='5%'></td><td width='5%' height='5%'></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
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
            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:50px;height:50px;'>"));
            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
            pdn2.Controls.Add(new LiteralControl("</a>"));



            //Image img = new Image();
            //img.Width = 50; img.Height = 50;
            //img.ImageUrl = ict.Table.Rows[i]["photo"].ToString();
            //pdn2.Controls.Add(img);


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            //poster username
            pdn2.Controls.Add(new LiteralControl("<td width='10%'>"));
            HyperLink hy = new HyperLink();
            hy.NavigateUrl = "javascript:void(0);";
            hy.Target = "_blank";
            hy.Text = ict.Table.Rows[i]["username"].ToString();
            hy.Font.Underline = false;
            pdn2.Controls.Add(hy);
            pdn2.Controls.Add(new LiteralControl("</td>"));
            //poster message type and time
            pdn2.Controls.Add(new LiteralControl("<td align='right' width='80%'>"));
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
            la.Text += ict.Table.Rows[i]["place"].ToString() + " ";
            la.Text += ict.Table.Rows[i]["year"].ToString() + "." + ict.Table.Rows[i]["month"].ToString() + "." + ict.Table.Rows[i]["day"].ToString();
            pdn2.Controls.Add(la);
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //poster message
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td colspan='3' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">"));
            pdn2.Controls.Add(new LiteralControl("<div class='box" + i + "'>"));
            HyperLink hyy;
            if (ict.Table.Rows[i]["message"].ToString().Length < 37)
            {
                pdn2.Controls.Add(new LiteralControl(ict.Table.Rows[i]["message"].ToString()));
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
            la1.Text = ict.Table.Rows[i]["message"].ToString();
            pdn2.Controls.Add(la1);
            pdn2.Controls.Add(new LiteralControl("<br/>"));


            if (ict.Table.Rows[i]["message"].ToString().Length > 36)
            {
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "たたむ";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
            }


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //poster images
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td colspan='3' width='90%' align='center'><br/><br/>"));
            SqlDataSource sql2 = new SqlDataSource();
            sql2.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql2.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b on a.id=b.smid";
            sql2.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
            DataView ict1 = (DataView)sql2.Select(DataSourceSelectArguments.Empty);
            if (ict1.Count > 3)
            {
                pdn2.Controls.Add(new LiteralControl("<div class='imbox" + i + "'>"));
                for (int ii = 0; ii < 3; ii++)
                {
                    Image imgg = new Image();
                    imgg.Width = 100; imgg.Height = 100;
                    imgg.ImageUrl = ict1.Table.Rows[ii]["filename"].ToString();
                    pdn2.Controls.Add(imgg);
                    pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                }
                pdn2.Controls.Add(new LiteralControl("<br/>"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "もっと見る";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("<div class='imhidde" + i + "'>"));
                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                for (int ii = 0; ii < ict1.Count; ii++)
                {
                    if (ii > 0 && ii % 3 == 0)
                    {
                        pdn2.Controls.Add(new LiteralControl("<br/>"));
                    }

                    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='100' height='100' />"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));

                    //Image imgg = new Image();
                    //imgg.Width = 100; imgg.Height = 100;
                    //imgg.ImageUrl = ict1.Table.Rows[ii]["filename"].ToString();
                    //pdn2.Controls.Add(imgg);
                    //pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                }
                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("</div>"));
                pdn2.Controls.Add(new LiteralControl("<div class='imhiddee" + i + "'>"));
                pdn2.Controls.Add(new LiteralControl("<br/>"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "たたむ";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
                pdn2.Controls.Add(new LiteralControl("</div>"));
            }
            else
            {
                pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));
                for (int ii = 0; ii < ict1.Count; ii++)
                {
                    string cutstr = ict1.Table.Rows[ii]["filename"].ToString();
                    int ind = cutstr.IndexOf(@"/");
                    string cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100px;height:100px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' width='100' height='100' />"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));


                    //Image imgg = new Image();
                    //imgg.Width = 100; imgg.Height = 100;
                    //imgg.ImageUrl = ict1.Table.Rows[ii]["filename"].ToString();
                    //pdn2.Controls.Add(imgg);
                    //pdn2.Controls.Add(new LiteralControl("&nbsp;"));
                }
                pdn2.Controls.Add(new LiteralControl("</div>"));
            }

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            //poster message like and share
            pdn2.Controls.Add(new LiteralControl("<td width='15%' align='right'><br/><br/>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likebox" + i + "'>"));
            Image img1 = new Image();
            img1.Width = 25; img1.Height = 25;
            img1.ImageUrl = "~/images/like_b.png";
            pdn2.Controls.Add(img1);
            Label laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
            laa.Text = "いいね";
            pdn2.Controls.Add(laa);


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likehidde" + i + "'>"));
            img1 = new Image();
            img1.Width = 25; img1.Height = 25;
            img1.ImageUrl = "~/images/like.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#F06767");
            laa.Text = "いいね";
            pdn2.Controls.Add(laa);
            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));


            pdn2.Controls.Add(new LiteralControl("<td>"));
            pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td align='center'><br/><br/>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='big_mess_box" + i + "'>"));
            img1 = new Image();
            img1.Width = 25; img1.Height = 25;
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
            img1.Width = 25; img1.Height = 25;
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
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='sharebox" + i + "'>"));
            img1 = new Image();
            img1.Width = 25; img1.Height = 25;
            img1.ImageUrl = "~/images/share_b.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
            laa.Text = "シェア";
            pdn2.Controls.Add(laa);


            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='sharehidde" + i + "'>"));
            img1 = new Image();
            img1.Width = 25; img1.Height = 25;
            img1.ImageUrl = "~/images/share.png";
            pdn2.Controls.Add(img1);
            laa = new Label();
            laa.Font.Size = FontUnit.Point(10);
            laa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#676767");
            laa.Text = "シェア";
            pdn2.Controls.Add(laa);
            pdn2.Controls.Add(new LiteralControl("</div>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            pdn2.Controls.Add(new LiteralControl("</table>"));
            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td></td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));

            pdn2.Controls.Add(new LiteralControl("<tr>"));
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
            pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));
            SqlDataSource sql4 = new SqlDataSource();
            sql4.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql4.SelectCommand = "select b.username from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
            sql4.SelectCommand += " where a.smid=" + ict.Table.Rows[i]["id"].ToString() + "";
            sql4.SelectCommand += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
            sql4.DataBind();
            DataView ict3 = (DataView)sql4.Select(DataSourceSelectArguments.Empty);
            if (ict3.Count > 2)
            {
                img1 = new Image();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b_1.png";
                pdn2.Controls.Add(img1);
                for (int iii = 0; iii < 2; iii++)
                {
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = ict3.Table.Rows[iii]["username"].ToString();
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);
                    pdn2.Controls.Add(new LiteralControl("、"));
                }
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "他" + (ict3.Count - 2) + "人";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);

            }
            else if(ict3.Count >0)
            {
                img1 = new Image();
                img1.Width = 15; img1.Height = 15;
                img1.ImageUrl = "~/images/like_b_1.png";
                pdn2.Controls.Add(img1);
                for (int iii = 0; iii < ict3.Count; iii++)
                {
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
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


            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("</tr>"));
            //who talk about this status message before
            pdn2.Controls.Add(new LiteralControl("<tr>"));
            pdn2.Controls.Add(new LiteralControl("<td width='100%'>"));



            sql4.SelectCommand = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
            sql4.SelectCommand += " from status_messages as a inner join status_messages_user as c";
            sql4.SelectCommand += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
            sql4.SelectCommand += " inner join status_messages_user_talk as e on e.smuid=c.id";
            sql4.SelectCommand += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + "";
            sql4.SelectCommand += " ORDER BY e.year desc,e.month desc,e.day desc,e.hour desc,e.minute desc,e.second desc;";
            sql4.DataBind();
            ict3 = (DataView)sql4.Select(DataSourceSelectArguments.Empty);
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
                    so.username = ict3.Table.Rows[iy]["username"].ToString();
                    so.photo = ict3.Table.Rows[iy]["photo"].ToString();
                }
                else
                {

                    SqlDataSource sql6 = new SqlDataSource();
                    sql6.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql6.SelectCommand = "select username,photo from user_login";
                    sql6.SelectCommand += " where id=" + ict3.Table.Rows[iy]["pointer_user_id"].ToString() + ";";
                    sql6.DataBind();
                    DataView ict5 = (DataView)sql6.Select(DataSourceSelectArguments.Empty);
                    so.username = ict5.Table.Rows[0]["username"].ToString();
                    so.photo = ict5.Table.Rows[0]["photo"].ToString();
                }
                talk_list.Add(so);
            }

            SqlDataSource sql5 = new SqlDataSource();
            sql5.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql5.SelectCommand = "select max(e.structure_level) as maxlevel";
            //sql5.SelectCommand = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
            sql5.SelectCommand += " from status_messages as a inner join status_messages_user as c";
            sql5.SelectCommand += " on a.id=c.smid inner join user_login as b on b.id=c.uid";
            sql5.SelectCommand += " inner join status_messages_user_talk as e on e.smuid=c.id";
            sql5.SelectCommand += " where a.id=" + ict.Table.Rows[i]["id"].ToString() + ";";
            sql5.DataBind();
            DataView ict4 = (DataView)sql5.Select(DataSourceSelectArguments.Empty);

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
                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                pdn2.Controls.Add(new LiteralControl("</a>"));

                pdn2.Controls.Add(new LiteralControl("</div>"));


                pdn2.Controls.Add(new LiteralControl("</td>"));
                pdn2.Controls.Add(new LiteralControl("<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">"));

                pdn2.Controls.Add(new LiteralControl(ict3.Table.Rows[0]["username"].ToString()));
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
                    pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
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
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "いいね!";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);
                pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                hyy = new HyperLink();
                hyy.NavigateUrl = "javascript:void(0);";
                hyy.Target = "_blank";
                hyy.Text = "返信";
                hyy.Font.Underline = false;
                pdn2.Controls.Add(hyy);

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
                    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                    pdn2.Controls.Add(new LiteralControl("</a>"));

                    pdn2.Controls.Add(new LiteralControl("</div>"));


                    pdn2.Controls.Add(new LiteralControl("</td>"));

                    pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));

                    pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].username.ToString()));
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
                        pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
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
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "いいね!";
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);
                    pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    hyy = new HyperLink();
                    hyy.NavigateUrl = "javascript:void(0);";
                    hyy.Target = "_blank";
                    hyy.Text = "返信";
                    hyy.Font.Underline = false;
                    pdn2.Controls.Add(hyy);

                    pdn2.Controls.Add(new LiteralControl("</td>"));
                    pdn2.Controls.Add(new LiteralControl("</tr>"));


                    pdn2.Controls.Add(new LiteralControl("</table>"));

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
                        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                        pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                        pdn2.Controls.Add(new LiteralControl("</a>"));

                        pdn2.Controls.Add(new LiteralControl("</div>"));

                        pdn2.Controls.Add(new LiteralControl("</td>"));

                        pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));

                        pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].username.ToString()));
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
                            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
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
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "いいね!";
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);
                        pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        hyy = new HyperLink();
                        hyy.NavigateUrl = "javascript:void(0);";
                        hyy.Target = "_blank";
                        hyy.Text = "返信";
                        hyy.Font.Underline = false;
                        pdn2.Controls.Add(hyy);

                        pdn2.Controls.Add(new LiteralControl("</td>"));
                        pdn2.Controls.Add(new LiteralControl("</tr>"));


                        pdn2.Controls.Add(new LiteralControl("</table>"));

                    }
                }
            }

            pdn2.Controls.Add(new LiteralControl("</td>"));
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
            pdn2.Controls.Add(new LiteralControl("<td width='10%' valign='top'>"));
            //user photo

            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

            cutstr2 = "~/images/mike.jpg";
            ind2 = cutstr2.IndexOf(@"/");
            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + "Mike" + "' style='width:50px;height:50px;'>"));
            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
            pdn2.Controls.Add(new LiteralControl("</a>"));

            pdn2.Controls.Add(new LiteralControl("</div>"));

            pdn2.Controls.Add(new LiteralControl("</td>"));
            pdn2.Controls.Add(new LiteralControl("<td width='90%'>"));

            //user answer

            TextBox tex = new TextBox();
            tex.Width = Unit.Percentage(100);
            tex.Height = 30;
            tex.Attributes.Add("placeholder", "コメントする");
            pdn2.Controls.Add(tex);
            pdn2.Controls.Add(new LiteralControl("<br/>"));

            pdn2.Controls.Add(new LiteralControl(@"
<label class='file-upload'><span><strong style='font-size: 20px;'>画像を登録</strong></span>
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
            pdn2.Controls.Add(new LiteralControl("<hr/>"));


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
    
    protected void UploadDocument(object sender, EventArgs e)
    {
        string input = "", DirRoot = "", filename = "";
        int stringindex = 0, cut = 0;
        Boolean check = false;
        if (fuDocument.HasFile)
        {
            foreach (HttpPostedFile postedFile in fuDocument.PostedFiles)
            {
                input = fuDocument.FileName;
                stringindex = input.LastIndexOf(@".");
                cut = input.Length - stringindex;
                DirRoot = input.Substring(stringindex + 1, cut - 1);

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

                        //sql_insert.InsertCommand = "insert into filename_detail(filename,name)";
                        //sql_insert.InsertCommand += " values('~/fileplace/" + filename + "','" + fuDocument.FileName.ToString() + "')";
                        //sql_insert.Insert();

                        //upload_finish_files.Text += postedFile.FileName.ToString() + "  finish<br>";

                        postedFile.SaveAs(Server.MapPath("fileplace") + "\\" + filename);

                        Image im = new Image();
                        im.Width = 100;
                        im.Height = 100;
                        im.ImageUrl = "~/fileplace/" + filename;
                        this.Panel1.Controls.Add(im);

                        image_HiddenField.Value+=",~/fileplace/" + filename;

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
    [WebMethod(EnableSession=true)]
    public static string check_login(string param1, string param2)
    {
        //string result = param1 + "," + param2;
        string result = "";
        try
        {
            string username = param1.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            string password = param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();
            if (username != "" && password != "" )
            {
                SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from user_login";
                sql_f.SelectCommand += " where login_name='" + username + "';";
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count > 0)
                {

                    SqlDataSource sql_f1 = new SqlDataSource();
                    sql_f1.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_f1.SelectCommand = "select id from user_login";
                    sql_f1.SelectCommand += " where login_name='" + username + "' and login_password='" + password + "';";
                    DataView ict_f1 = (DataView)sql_f1.Select(DataSourceSelectArguments.Empty);
                    if (ict_f1.Count > 0)
                    {
                        HttpContext.Current.Session["id"] = ict_f1.Table.Rows[0]["id"].ToString();
                        result = ict_f1.Table.Rows[0]["id"].ToString();
                    }
                    else
                    {
                        result = "Password is wrong.";
                    }

                }
                else
                {
                    result = "No this user.";
                }
            }



        }
        catch (Exception ex)
        {
            result = "fail";

            //return result;
            throw ex;
        }
        return result;

    }
    [WebMethod(EnableSession = true)]
    public static string check_photo(string param1)
    {
        //string result = param1 + "," + param2;
        string result = "";
        string id = param1;
         SqlDataSource sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select photo from user_login";
                sql_f.SelectCommand += " where id='" + id + "';";
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count > 0)
                {
                    string cutstr2 = ict_f.Table.Rows[0]["photo"].ToString();
                   int ind2 = cutstr2.IndexOf(@"/");
                    string cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    result = cutstr3;
                }

        return result;
    }

    protected void register_Button_Click(object sender, EventArgs e)
    {
        Response.Redirect("registered0.aspx");
    }
    protected void message_type0_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 0;
        Response.Redirect("main.aspx");
    }
    protected void message_type1_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 1;
        Response.Redirect("main.aspx");
    }
    protected void message_type2_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 2;
        Response.Redirect("main.aspx");
    }
    protected void message_type3_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 3;
        Response.Redirect("main.aspx");
    }
    protected void message_type4_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 4;
        Response.Redirect("main.aspx");
    }
    protected void message_type5_Button_Click(object sender, EventArgs e)
    {
        Session["message_type"] = 5;
        Response.Redirect("main.aspx");
    }
    protected void post_message_Button_Click(object sender, EventArgs e)
    {
        if (Session["id"] != null || Session["id"].ToString() != "")
        {
            string id = Session["id"].ToString();
            string message = post_message_TextBox.Text.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim();;
            string message_type = Request.Form["post_type"].ToString();
            if (message_type == "")
            {
                message_type = "0";
            }
            string type = "";
            if (type_div.Value == "一般公開")
            {
                type = "0";
            }
            else if (type_div.Value == "neighborhood")
            {
                type = "1";
            }
            else if (type_div.Value == "friend")
            {
                type = "2";
            }
            else if (type_div.Value == "")
            {
                type = "0";
            }

            string place = place_va.Value;

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
            string min = DateTime.Now.Minute.ToString();
            string sec = DateTime.Now.Second.ToString();


            //ScriptManager.RegisterStartupScript(post_message_Button, post_message_Button.GetType(), "alert", "alert('@" + place + "')", true);

            SqlDataSource sql_f = new SqlDataSource();
            sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            sql_f.InsertCommand = "insert into status_messages(uid,type,message_type,place,message,year,month,day,hour,minute,second)";
            sql_f.InsertCommand += " values('" + id + "','" + type + "','" + message_type +"','"+place+"','"+message+ "','" + year + "','" + month + "'";
            sql_f.InsertCommand += ",'" + day + "','" + hour + "','" + min + "','" + sec + "')";
            sql_f.Insert();


            sql_f = new SqlDataSource();
                sql_f.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                sql_f.SelectCommand = "select id from status_messages";
                sql_f.SelectCommand += " where uid='" + id + "' and type='"+type+"' and message_type='"+message_type+"' and place='"+place+"'";
                sql_f.SelectCommand += " and message='" + message + "' and year='"+year+"' and month='"+month+"' and day='"+day+"' and hour='"+hour+"'";
            sql_f.SelectCommand += " and minute='" + min + "' and second='"+sec+"';";    
            sql_f.DataBind();
                DataView ict_f = (DataView)sql_f.Select(DataSourceSelectArguments.Empty);
                if (ict_f.Count > 0)
                {
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
                    for (int i = 0; i < image_find.Count - 1; i++)
                    {
                        len = image_find[i + 1] - image_find[i];
                        naa = image_path.Substring(image_find[i] + 1, len - 1);
                        sql_insert = new SqlDataSource();
                        sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                        sql_insert.InsertCommand = "insert into status_messages_image(smid,filename)";
                        sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["id"].ToString() +"','" + naa + "');";
                        sql_insert.Insert();

                    }
                    len = image_path.Length - image_find[image_find.Count - 1];
                    naa = image_path.Substring(image_find[image_find.Count - 1] + 1, len - 1);
                    sql_insert = new SqlDataSource();
                    sql_insert.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    sql_insert.InsertCommand = "insert into status_messages_image(smid,filename)";
                    sql_insert.InsertCommand += " values('" + ict_f.Table.Rows[0]["id"].ToString() + "','" + naa + "');";
                    sql_insert.Insert();

                    post_message_TextBox.Text = "";
                    image_HiddenField.Value = "";
                    Response.Redirect("main.aspx");
                }
           
        }
    }
    protected void video_list_Button_Click(object sender, EventArgs e)
    {

    }
}