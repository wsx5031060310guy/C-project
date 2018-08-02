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
using MySql.Data.MySqlClient;

public partial class main_status : System.Web.UI.Page
{
    GCP_MYSQL gc = new GCP_MYSQL();
    string Query = "";
    string resin = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["id"] == null)
        {
            Response.Redirect("main.aspx");
        }
        if (this.Request.QueryString["_status"] != null)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["_status"]) ? Request.QueryString["_status"] : Guid.Empty.ToString();
            if (activationCode != "")
            {
                Session["status_id"] = activationCode;
                Session["big_status_id"] = null;
                Response.Redirect("main_status.aspx");
            }
        }else if (this.Request.QueryString["_status_big"] != null)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["_status_big"]) ? Request.QueryString["_status_big"] : Guid.Empty.ToString();
            if (activationCode != "")
            {
                Session["big_status_id"] = activationCode;
                Session["status_id"] = null;
                Response.Redirect("main_status.aspx");
            }
        }
        if (Session["status_id"] != null || Session["big_status_id"]!=null)
        {
            string cutstr2 = "", cutstr3 = "", messid = "";
            int ind2=0 ;
            if (Session["status_id"] != null)
            {
                if (Session["status_id"].ToString().Trim() != "")
                {
                    cutstr2 = Session["status_id"].ToString();
                    ind2 = cutstr2.IndexOf(@"_");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);

                    Query = "select a.id";
                    Query += " from status_messages as a";
                    Query += " inner join status_messages_user as b on a.id=b.smid";
                    Query += " inner join status_messages_user_talk as c on b.id=c.smuid";
                    Query += " where c.id='" + cutstr3 + "';";
                    DataView ict_h = gc.select_cmd(Query);
                    if (ict_h.Count > 0)
                    {
                        messid = ict_h.Table.Rows[0]["id"].ToString();
                    }
                }
            }
            else if (Session["big_status_id"] != null)
            {
                if (Session["big_status_id"].ToString().Trim() != "")
                {
                    cutstr2 = Session["big_status_id"].ToString();
                    ind2 = cutstr2.IndexOf(@"_");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    messid = cutstr3;
                }
            }








                ///////
                Panel pdn_j = (Panel)this.FindControl("javaplace");
                pdn_j.Controls.Clear();

                Query = "select a.id,a.type,a.message_type,a.place,a.message,a.year,a.month,a.day,a.hour,a.minute,a.second,b.username,b.photo,a.uid ";
                Query += "from status_messages as a";
                Query += " inner join user_login as b on b.id=a.uid where 1=1";
                if (messid != "")
                {
                    Query += " and a.id='" + messid + "'";
                }
                else
                {
                    Response.Redirect("main.aspx");
                }
                Query += " ORDER BY a.year desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                DataView ict = gc.select_cmd(Query);


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

            $('.likehidde" + i + @"').toggle(false);

            $('.likebox" + i + @"').click(function () {
                $('.likehidde" + i + @"').toggle();
                $('.likebox" + i + @"').toggle(false);
webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
            })

            $('.likehidde" + i + @"').click(function () {
                $('.likebox" + i + @"').toggle();
                $('.likehidde" + i + @"').toggle(false);
webaction(" + ict.Table.Rows[i]["id"].ToString() + @",0);
            })

            $('.mess_hidde" + i + @"').show();
            $('.mess_box" + i + @"').hide();

            $('.big_mess_hidde" + i + @"').show();
            $('.big_mess_box" + i + @"').hide();
            $('.status_message_hidde" + i + @"').show();
            $('.status_message_box" + i + @"').hide();



            $('.big_mess_hidde" + i + @"').show();
            $('.big_mess_box" + i + @"').hide();
            $('.status_message_hidde" + i + @"').show();
            $('.status_message_box" + i + @"').hide();



";

//                    SqlDataSource sql3 = new SqlDataSource();
//                    sql3.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//                    sql3.SelectCommand = "select filename from status_messages as a inner join status_messages_image as b on a.id=b.smid";
//                    sql3.SelectCommand += " where b.smid=" + ict.Table.Rows[i]["id"].ToString() + ";";
//                    DataView ict2 = (DataView)sql3.Select(DataSourceSelectArguments.Empty);
//                    if (ict2.Count > 3)
//                    {
//                        li.Text += @"
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
//                    }
                }

                li.Text += @"
                        });";
                li.Text += @"</script>";

                pdn_j = (Panel)this.FindControl("javaplace");
                //pdn_j.Controls.Clear();
                pdn_j.Controls.Add(li);

                //this.Page.Controls.Add(li);


                //this.Page.Header.Controls.Add(li);
                ////添加至指定位置
                //this.Page.Header.Controls.AddAt(0, li);



                Panel pdn2 = (Panel)this.FindControl("Panel2");
                pdn2.Controls.Clear();

                for (int i = 0; i < ict.Count; i++)
                {
                    pdn2.Controls.Add(new LiteralControl("<div id='state_mess_" + i + "'>"));


                    pdn2.Controls.Add(new LiteralControl("<table width='100%' style='border: thick solid #E9EBEE;'>"));
                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    pdn2.Controls.Add(new LiteralControl("<td>"));
                    pdn2.Controls.Add(new LiteralControl("<div id='newstatusbig_" + ict.Table.Rows[i]["id"].ToString() + "'>"));
                    //big message place
                    pdn2.Controls.Add(new LiteralControl("<table width='100%' style='background-color:#fff;border: 1px solid; border-color: #e5e6e9 #dfe0e4 #d0d1d5; border-radius: 3px;'>"));
                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    pdn2.Controls.Add(new LiteralControl("<td width='5%' height='5%'><br/></td><td width='90%' height='5%'><br/></td><td width='5%' height='5%'><br/></td>"));
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

                    cutstr2 = ict.Table.Rows[i]["photo"].ToString();
                    ind2 = cutstr2.IndexOf(@"/");
                    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
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


                    pdn2.Controls.Add(new LiteralControl("</td>"));
                    pdn2.Controls.Add(new LiteralControl("</tr>"));
                    //poster message
                    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    pdn2.Controls.Add(new LiteralControl("<td colspan='2' style=" + '"' + "word-break:break-all; width:90%;" + '"' + ">"));
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
                    //    hyy.Text = "閉じる";
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

                                    morefour += "<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>";
                                    morefour += "<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>";
                                }
                                else
                                {
                                    //Random rand = new Random(Guid.NewGuid().GetHashCode());

                                    //int w = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));
                                    //rand = new Random(Guid.NewGuid().GetHashCode());
                                    //int h = Convert.ToInt32(1.0 + 3.0 * rand.Next(0, 1));

                                    pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));

                                    //pdn2.Controls.Add(new LiteralControl("<div class='cell' style='width:" + (w * 100) + "px; height:" + (h * 100) + "px; background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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


                        pdn2.Controls.Add(new LiteralControl("</div>"));
                        //pdn2.Controls.Add(new LiteralControl("<div class='imhiddee" + i + "'>"));
                        //pdn2.Controls.Add(new LiteralControl("<br/>"));
                        //hyy = new HyperLink();
                        //hyy.NavigateUrl = "javascript:void(0);";
                        //hyy.Target = "_blank";
                        //hyy.Text = "たたむ";
                        //hyy.Font.Underline = false;
                        //pdn2.Controls.Add(hyy);
                        //pdn2.Controls.Add(new LiteralControl("</div>"));
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
                            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr1 + "' style='width:100%;height:100%;'/>"));
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

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size42' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[1]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size24' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
                                pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr1 + "' data-source='" + cutstr1 + "' title='" + ict.Table.Rows[i]["username"].ToString() + "' style='width:100%;height:100%;'>"));
                                pdn2.Controls.Add(new LiteralControl("<img src='images/test.png' style='width:100%;height:100%;'/>"));
                                pdn2.Controls.Add(new LiteralControl("</a>"));
                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                cutstr = ict1.Table.Rows[2]["filename"].ToString();
                                ind = cutstr.IndexOf(@"/");
                                cutstr1 = cutstr.Substring(ind + 1, cutstr.Length - ind - 1);

                                pdn2.Controls.Add(new LiteralControl("<div class='brick level-1 size22' style='background-image: url(" + cutstr1 + ");background-repeat:no-repeat; background-size: cover;'>"));
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
                    pdn2.Controls.Add(new LiteralControl("<td width='20%' align='right'><br/><br/>"));
                    pdn2.Controls.Add(new LiteralControl("<div style='cursor: pointer' class='likebox" + i + "'>"));

                    Label laa = new Label();
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
                    duration: 1000
                },
                hide: {
                    effect: 'explode',
                    duration: 1000
                }
            });
   $('#sharebox" + i + @"').on('click', function () {
                $('#share_div" + i + @"').dialog('open');
webaction(" + ict.Table.Rows[i]["id"].ToString() + @",2);

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
                    //big message end
                    pdn2.Controls.Add(new LiteralControl("</div>"));


                    pdn2.Controls.Add(new LiteralControl("</td>"));
                    pdn2.Controls.Add(new LiteralControl("<td style='vertical-align: top';>"));

                    //report
                    Image report = new Image();
                    report.ID = "reportstate_" + ict.Table.Rows[i]["id"].ToString();
                    report.ImageUrl = "~/images/report.png";
                    report.Style.Add("cursor", "pointer");
                    report.Attributes["onclick"] = "report_mess(this.id)";
                    li = new Literal();

                    //report div
                    li.Text = @"
<div id='dlgbox_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlg'>
            <div id='dlg-header_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgh'>問題の内容についてお聞かせください</div>
            <div id='dlg-body_report_" + ict.Table.Rows[i]["id"].ToString() + @"' style='height: 200px; overflow: auto' class='dlgb'>
                <table style=' width: 100%;'>
                    <tr>
                        <td>
                            <table class='report_dlg' style='width: 100%;'>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span style='font-weight: bold;font-size:medium;'>詳細を入力してください。</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='  不快または面白くない' style='margin-right: 15px;'> <span style='font-size:medium;'>不快または面白くない</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='  に載せるべきではないと思う' style='margin-right: 15px;'><span style='font-size:medium;'>に載せるべきではないと思う</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <input type='radio' name='report_" + ict.Table.Rows[i]["id"].ToString() + @"' value='  スパムである' style='margin-right: 15px;'><span style='font-size:medium;'>スパムである</span><br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='10%'>

                                    </td>
                                    <td align='left' width='90%'>
                                        <span id='reportla_" + ict.Table.Rows[i]["id"].ToString() + @"' style='color:red;font-size:medium;'></span><br/>
                                    </td>
                                </tr>
                            </table>
                            <hr/>
                        </td>
</tr>
                </table>
            </div>
            <div id='dlg-footer_report_" + ict.Table.Rows[i]["id"].ToString() + @"' class='dlgf'>
<table style=' width: 100%;'>
<tr>
<td width='50%' align='left'>
<input id='reportstatebutcancel_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='取り消す' onclick='dlgrecanel(this.id)' class='file-upload1'/>
</td>
<td width='50%' align='right'>
                <input id='reportstatebutedit_" + ict.Table.Rows[i]["id"].ToString() + @"' type='button' value='保存' onclick='dlgreport(this.id)' class='file-upload1'/>
</td>
            </tr>
</table>
</div>
        </div>
";

                    pdn2.Controls.Add(report);
                    pdn2.Controls.Add(li);

                    //report
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
                    Query = "select b.username,b.id,a.id as smulid from status_messages_user_like as a inner join user_login as b on a.uid=b.id";
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
                            hyy.NavigateUrl = "~/user_home_friend.aspx?=" + ict3.Table.Rows[iii]["id"].ToString();
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
                    //Query1 = "select e.id,e.message,e.filename,b.username,b.photo,e.pointer_message_id,e.pointer_user_id,e.structure_level";
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
                    //if (ict3.Count > 1)
                    //{
                    //    //show div
                    //    pdn2.Controls.Add(new LiteralControl("<div class='mess_box" + i + "'>"));
                    //    pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("<td width='100%' align='left' colspan='2'>"));

                    //    hyy = new HyperLink();
                    //    hyy.NavigateUrl = "javascript:void(0);";
                    //    hyy.Target = "_blank";
                    //    hyy.Text = "以前のコメントを見る";
                    //    hyy.Font.Underline = false;
                    //    pdn2.Controls.Add(hyy);

                    //    pdn2.Controls.Add(new LiteralControl("</td>"));
                    //    pdn2.Controls.Add(new LiteralControl("</tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("<td width='10%' rowspan='2' valign='top'>"));


                    //    pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    //    cutstr2 = talk_list_tmp[0].photo;
                    //    ind2 = cutstr2.IndexOf(@"/");
                    //    cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    //    pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                    //    pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                    //    pdn2.Controls.Add(new LiteralControl("</a>"));

                    //    pdn2.Controls.Add(new LiteralControl("</div>"));


                    //    pdn2.Controls.Add(new LiteralControl("</td>"));
                    //    pdn2.Controls.Add(new LiteralControl("<td width='90%' style=" + '"' + "word-break:break-all;" + '"' + ">"));

                    //    hyy = new HyperLink();
                    //    hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[0].uid.ToString();
                    //    hyy.Target = "_blank";
                    //    hyy.Text = talk_list_tmp[0].username.ToString();
                    //    hyy.Font.Underline = false;
                    //    pdn2.Controls.Add(hyy);
                    //    pdn2.Controls.Add(new LiteralControl("<br/>"));
                    //    pdn2.Controls.Add(new LiteralControl(ict3.Table.Rows[0]["message"].ToString()));
                    //    pdn2.Controls.Add(new LiteralControl("<br/>"));

                    //    if (talk_list_tmp[0].filename != "")
                    //    {
                    //        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    //        cutstr2 = talk_list_tmp[0].filename;
                    //        ind2 = cutstr2.IndexOf(@"/");
                    //        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    //        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + ict3.Table.Rows[0]["username"].ToString() + "' style='width:50px;height:50px;'>"));
                    //        pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                    //        pdn2.Controls.Add(new LiteralControl("</a>"));

                    //        pdn2.Controls.Add(new LiteralControl("</div>"));

                    //        pdn2.Controls.Add(new LiteralControl("<br/>"));

                    //    }

                    //    pdn2.Controls.Add(new LiteralControl("</td>"));
                    //    pdn2.Controls.Add(new LiteralControl("</tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("<tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("<td>"));
                    //    //who talk about status message and who like

                    //    hyy = new HyperLink();
                    //    hyy.ID = "wholike_" + talk_list_tmp[0].id + "_s";
                    //    hyy.NavigateUrl = "javascript:void(0);";
                    //    hyy.Target = "_blank";
                    //    hyy.Text = "いいね!";
                    //    SqlDataSource sql_who_like = new SqlDataSource();
                    //    sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //    sql_who_like.SelectCommand = "select good_status from status_messages_user_talk_like";
                    //    sql_who_like.SelectCommand += " where smutid='" + talk_list_tmp[0].id + "' and uid='" + Session["id"].ToString() + "';";
                    //    sql_who_like.DataBind();
                    //    DataView ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                    //    if (ict_who_like.Count > 0)
                    //    {
                    //        if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                    //        {
                    //            hyy.Style.Add("color", "#4183C4");
                    //            hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                    //        }
                    //        else
                    //        {
                    //            hyy.Style.Add("color", "#D84C4B");
                    //            hyy.Attributes["onclick"] = "slike_who_answer(this.id)";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        hyy.Style.Add("color", "#4183C4");
                    //        hyy.Attributes["onclick"] = "sblike_who_answer(this.id)";
                    //    }
                    //    pdn2.Controls.Add(hyy);


                    //    pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    //    hyy = new HyperLink();
                    //    hyy.NavigateUrl = "javascript:void(0);";
                    //    hyy.Target = "_blank";
                    //    hyy.Text = "返信";
                    //    hyy.Font.Underline = false;
                    //    pdn2.Controls.Add(hyy);

                    //    //who like who answer post message
                    //    sql_who_like = new SqlDataSource();
                    //    sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //    sql_who_like.SelectCommand = "select count(*) as howmany from status_messages_user_talk_like";
                    //    sql_who_like.SelectCommand += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                    //    //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    //    sql_who_like.DataBind();
                    //    ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                    //    if (ict_who_like.Count > 0)
                    //    {
                    //        img1 = new Image();
                    //        img1.Width = 15; img1.Height = 15;
                    //        img1.ImageUrl = "~/images/like_b_1.png";
                    //        pdn2.Controls.Add(img1);
                    //        hyy = new HyperLink();
                    //        hyy.NavigateUrl = "javascript:void(0);";
                    //        hyy.Target = "_blank";
                    //        hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                    //        hyy.Font.Underline = false;
                    //        pdn2.Controls.Add(hyy);
                    //    }
                    //    //who like who answer post message



                    //    pdn2.Controls.Add(new LiteralControl("</td>"));
                    //    pdn2.Controls.Add(new LiteralControl("</tr>"));
                    //    pdn2.Controls.Add(new LiteralControl("</table>"));
                    //    pdn2.Controls.Add(new LiteralControl("</div>"));
                    //    //hidde message
                    //    pdn2.Controls.Add(new LiteralControl("<div class='mess_hidde" + i + "'>"));
                    //    pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //    for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                    //    {
                    //        //check where
                    //        pdn2.Controls.Add(new LiteralControl("<div id='newstatus_" + talk_list_tmp[iiii].id + "' class='newstatus_class'>"));


                    //        pdn2.Controls.Add(new LiteralControl("<table width='100%'>"));
                    //        pdn2.Controls.Add(new LiteralControl("<tr>"));
                    //        int wid = (10 + (10 * talk_list_tmp[iiii].level));
                    //        if (wid > 90) { wid = 90; }
                    //        pdn2.Controls.Add(new LiteralControl("<td width='" + wid + "%' align='right' rowspan='2' valign='top'>"));

                    //        pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    //        cutstr2 = talk_list_tmp[iiii].photo;
                    //        ind2 = cutstr2.IndexOf(@"/");
                    //        cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    //        pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                    //        pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                    //        pdn2.Controls.Add(new LiteralControl("</a>"));

                    //        pdn2.Controls.Add(new LiteralControl("</div>"));


                    //        pdn2.Controls.Add(new LiteralControl("</td>"));

                    //        pdn2.Controls.Add(new LiteralControl("<td width='" + (100 - wid) + "%'  style=" + '"' + "word-break:break-all;" + '"' + ">"));
                    //        hyy = new HyperLink();
                    //        hyy.NavigateUrl = "~/user_home_friend.aspx?=" + talk_list_tmp[iiii].uid.ToString();
                    //        hyy.Target = "_blank";
                    //        hyy.Text = talk_list_tmp[iiii].username.ToString();
                    //        hyy.Font.Underline = false;
                    //        pdn2.Controls.Add(hyy);
                    //        pdn2.Controls.Add(new LiteralControl("<br/>"));
                    //        pdn2.Controls.Add(new LiteralControl(talk_list_tmp[iiii].mess.ToString()));
                    //        pdn2.Controls.Add(new LiteralControl("<br/>"));

                    //        if (talk_list_tmp[iiii].filename.ToString() != "")
                    //        {
                    //            pdn2.Controls.Add(new LiteralControl("<div class='zoom-gallery'>"));

                    //            cutstr2 = talk_list_tmp[iiii].filename.ToString();
                    //            ind2 = cutstr2.IndexOf(@"/");
                    //            cutstr3 = cutstr2.Substring(ind2 + 1, cutstr2.Length - ind2 - 1);
                    //            pdn2.Controls.Add(new LiteralControl("<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + talk_list_tmp[iiii].username.ToString() + "' style='width:50px;height:50px;'>"));
                    //            pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='50' height='50' />"));
                    //            pdn2.Controls.Add(new LiteralControl("</a>"));

                    //            pdn2.Controls.Add(new LiteralControl("</div>"));
                    //            pdn2.Controls.Add(new LiteralControl("<br/>"));
                    //        }

                    //        pdn2.Controls.Add(new LiteralControl("</td>"));
                    //        pdn2.Controls.Add(new LiteralControl("</tr>"));
                    //        pdn2.Controls.Add(new LiteralControl("<tr>"));
                    //        pdn2.Controls.Add(new LiteralControl("<td>"));


                    //        //who talk about status message and who like

                    //        hyy = new HyperLink();
                    //        hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
                    //        hyy.NavigateUrl = "javascript:void(0);";
                    //        hyy.Target = "_blank";
                    //        hyy.Text = "いいね!";
                    //        sql_who_like = new SqlDataSource();
                    //        sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //        sql_who_like.SelectCommand = "select good_status from status_messages_user_talk_like";
                    //        sql_who_like.SelectCommand += " where smutid='" + talk_list_tmp[iiii].id + "' and uid='" + Session["id"].ToString() + "';";
                    //        sql_who_like.DataBind();
                    //        ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                    //        if (ict_who_like.Count > 0)
                    //        {
                    //            if (ict_who_like.Table.Rows[0]["good_status"].ToString() == "0")
                    //            {
                    //                hyy.Style.Add("color", "#4183C4");
                    //                hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                    //            }
                    //            else
                    //            {
                    //                hyy.Style.Add("color", "#D84C4B");
                    //                hyy.Attributes["onclick"] = "like_who_answer(this.id)";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            hyy.Style.Add("color", "#4183C4");
                    //            hyy.Attributes["onclick"] = "blike_who_answer(this.id)";
                    //        }
                    //        pdn2.Controls.Add(hyy);
                    //        pdn2.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    //        hyy = new HyperLink();
                    //        hyy.ID = "whowantans_" + talk_list_tmp[iiii].id;
                    //        hyy.NavigateUrl = "javascript:void(0);";
                    //        hyy.Target = "_blank";
                    //        hyy.Text = "返信";
                    //        hyy.Attributes["onclick"] = "who_answer(this.id)";
                    //        hyy.Font.Underline = false;
                    //        pdn2.Controls.Add(hyy);

                    //        //who like who answer post message
                    //        sql_who_like = new SqlDataSource();
                    //        sql_who_like.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    //        sql_who_like.SelectCommand = "select count(*) as howmany from status_messages_user_talk_like";
                    //        sql_who_like.SelectCommand += " where smutid='" + talk_list_tmp[iiii].id + "' and good_status='1';";
                    //        //sql_who_like.SelectCommand += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";
                    //        sql_who_like.DataBind();
                    //        ict_who_like = (DataView)sql_who_like.Select(DataSourceSelectArguments.Empty);
                    //        if (ict_who_like.Count > 0)
                    //        {
                    //            img1 = new Image();
                    //            img1.Width = 15; img1.Height = 15;
                    //            img1.ImageUrl = "~/images/like_b_1.png";
                    //            pdn2.Controls.Add(img1);
                    //            hyy = new HyperLink();
                    //            hyy.ID = "likecount" + talk_list_tmp[iiii].id;
                    //            hyy.NavigateUrl = "javascript:void(0);";
                    //            hyy.Target = "_blank";
                    //            hyy.Text = ict_who_like.Table.Rows[0]["howmany"].ToString();
                    //            hyy.Font.Underline = false;
                    //            pdn2.Controls.Add(hyy);
                    //        }
                    //        //who like who answer post message

                    //        pdn2.Controls.Add(new LiteralControl("</td>"));
                    //        pdn2.Controls.Add(new LiteralControl("</tr>"));


                    //        pdn2.Controls.Add(new LiteralControl("</table>"));

                    //        pdn2.Controls.Add(new LiteralControl("</div>"));

                    //        pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));

                    //    }
                    //    pdn2.Controls.Add(new LiteralControl("</table>"));
                    //    pdn2.Controls.Add(new LiteralControl("</div>"));

                    //}
                    //else
                    //{
                        if (ict3.Count > 0)
                        {
                            for (int iiii = 0; iiii < talk_list_tmp.Count; iiii++)
                            {
                                //check where
                                pdn2.Controls.Add(new LiteralControl("<div id='newstatus_" + talk_list_tmp[iiii].id + "' class='newstatus_class'>"));

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
                                pdn2.Controls.Add(new LiteralControl("<img src='" + cutstr3 + "' width='32' height='32' />"));
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
                                hyy.ID = "wholike_" + talk_list_tmp[iiii].id;
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
                                Query += " where smutid='" + talk_list_tmp[0].id + "' and good_status='1';";
                                //Query1 += " ORDER BY ayear desc,a.month desc,a.day desc,a.hour desc,a.minute desc,a.second desc;";

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

                                pdn2.Controls.Add(new LiteralControl("</div>"));

                                //user answer user answer
                                pdn2.Controls.Add(new LiteralControl("<div id='whowanttoanswer_" + talk_list_tmp[iiii].id + "'></div>"));



                            }
                        }
                    //}

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
                    pdn2.Controls.Add(new LiteralControl("<td width='85%'>"));

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
<label class='file-upload2'><span><img src='images/photo.png' alt='' width='20px' height='20px'></span>
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

                    pdn2.Controls.Add(new LiteralControl("</div>"));
                }

        }




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
    [WebMethod]
    public static string small_sendtopost(string param1, string param2, string param3, string param4)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = param1 + "," + param2 + "," + param3 + "," + param4;

        string constr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
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
            result += "<a href='" + cutstr3 + "' data-source='" + cutstr3 + "' title='" + "Mike" + "' style='width:25px;height:25px;'>";
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
                        ValueString[i] = rnd.Next(count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));

                        //  檢查是否存在重複
                        while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
                        {
                            ValueString[i] = rnd.Next(count_bf, Convert.ToInt32(HttpContext.Current.Session["friend_for_count"].ToString()));
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
    public static string report_bad(string param1, string param2, string param3)
    {
        GCP_MYSQL gc1 = new GCP_MYSQL();
        string Query1 = "";
        string resin = "";
        string result = "";

        Query1 = "select id";
        Query1 += " from status_messages_report";
        Query1 += " where smid='" + param2 + "' and uid='" + param1 + "';";
        DataView ict_h_rep = gc1.select_cmd(Query1);
        if (ict_h_rep.Count > 0)
        {
            Query1 = "update status_messages_report";
            Query1 += " set report_mess='" + param3 + "',report_time=NOW()";
            Query1 += " where id='" + ict_h_rep.Table.Rows[0]["id"].ToString() + "';";
            resin = gc1.update_cmd(Query1);
        }
        else
        {
            Query1 = "insert into status_messages_report(smid,uid,report_mess,report_time)";
            Query1 += " values('" + param2 + "','" + param1 + "','" + param3 + "',NOW());";
            resin = gc1.insert_cmd(Query1);
        }
        result = "問題の内容が受けました。なるべく早くご返事いたします。";

        return result;
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
    public class URL_data
    {
        public string url = "";
        public string image_url = "";
        public string title = "";
        public string des = "";
        public string update_time = "";
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
    public static string user_action(string param1, string param2)
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
                string type = "";
                if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "0")
                {
                    type = "user_like";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "1")
                {
                    type = "user_message";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "2")
                {
                    type = "user_share";
                }
                else if (param2.Replace("\'", "").Replace("\"", "").Replace("`", "").Trim() == "3")
                {
                    type = "user_answer";
                }



                Query1 = "select id from user_action";
                Query1 += " where uid='" + uid + "' and smid='" + param1 + "';";
                DataView ict_f = gc1.select_cmd(Query1);
                if (ict_f.Count > 0)
                {
                    //update
                    //ict_f.Table.Rows[0]["id"].ToString()
                    Query1 = "update user_action set " + type + "=" + type + "+1";
                    Query1 += " where id='" + ict_f.Table.Rows[0]["id"].ToString() + "';";
                    resin = gc1.update_cmd(Query1);
                }
                else
                {
                    //insert
                    Query1 = "insert into user_action(smid,uid,user_like,user_message,user_share,user_answer)";
                    Query1 += " values('" + param1 + "','" + uid + "','0','0','0','0');";
                    resin = gc1.insert_cmd(Query1);

                    Query1 = "select id from user_action";
                    Query1 += " where uid='" + uid + "' and smid='" + param1 + "';";

                    DataView ict_f1 = gc1.select_cmd(Query1);
                    if (ict_f1.Count > 0)
                    {
                        Query1 = "update user_action set " + type + "=" + type + "+1";
                        Query1 += " where id='" + ict_f1.Table.Rows[0]["id"].ToString() + "';";
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
