<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" oncontextmenu="return false" >
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>

    <title></title>

    <meta name="description" content="は、お住いのエリアで子育てを楽しむための情報や各自治体、お店の情報など住んでるからこそ必要な情報が探せます。" />
    <meta name="keywords" content="">




  <meta property="og:type" content="website" />
<meta property="og:image:type" content="image/jpeg" />
<meta property="og:image:width" content="200" />
<meta property="og:image:height" content="200" />
    <meta property="og:site_name" content=".jp" />



    <meta name="twitter:card" content="summary_large_image" />
    <meta name="twitter:site" content="@JP">
    <meta name="twitter:title" content="">




 <%--<meta name="twitter:card" content="website" />

    <meta name="twitter:image:width" content="600" />
<meta name="twitter:image:height" content="600" />--%>

    <!-- Bootstrap Core CSS -->
    <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="//fonts.googleapis.com/earlyaccess/hannari.css" rel="stylesheet" />
    <link href='//fonts.googleapis.com/css?family=Kaushan+Script' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Droid+Serif:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Roboto+Slab:400,100,300,700' rel='stylesheet' type='text/css'>

    <!-- Theme CSS -->
    <link href="css/agency.css" rel="stylesheet">
    <link rel="stylesheet" href="css/file-upload.css" type="text/css"/>

     <link href="css/home.css" rel="stylesheet" type="text/css">

    <script type="text/javascript" src="js/konami.js"></script>
<script type="text/javascript">



    // a key map of allowed keys
    var allowedKeys = {
        37: 'left',
        38: 'up',
        39: 'right',
        40: 'down',
        65: 'a',
        66: 'b'
    };

    // the 'official' Konami Code sequence
    var konamiCode = ['up', 'up', 'down', 'down', 'left', 'right', 'left', 'right', 'b', 'a'];

    // a variable to remember the 'position' the user has reached so far.
    var konamiCodePosition = 0;

    // add keydown event listener
    document.addEventListener('keydown', function (e) {
        // get the value of the key code from the key map
        var key = allowedKeys[e.keyCode];
        // get the value of the required key from the konami code
        var requiredKey = konamiCode[konamiCodePosition];

        // compare the key with the required key
        if (key == requiredKey) {

            // move to the next key in the konami code sequence
            konamiCodePosition++;

            // if the last key is reached, activate cheats
            if (konamiCodePosition == konamiCode.length) {
                activateCheats();
                konamiCodePosition = 0;
            }
        } else {
            konamiCodePosition = 0;
        }
    });

    function activateCheats() {

        $.ajax({
            type: "POST",
            url: "home.aspx/check_mamaro",
            data: "{param1: 'tt' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (result) {
                if (result.d == "out") {
                    window.location = "3Dbox.aspx";
                }
            },
            error: function (result) {
                console.log(result.d);
            }
        });
    }
</script>
</head>
<body id="page-top" class="index">
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <!-- Navigation -->
    <nav id="mainNav" class="navbar navbar-default navbar-custom navbar-fixed-top">
        <div class="container">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span> Menu <i class="fa fa-bars"></i>
                </button>
                <a class="navbar-brand page-scroll" href="#page-top"><img alt="" src="images/logo1.png" /></a>
                <%--<a class="navbar-brand page-scroll" href="#page-top">RiN</a>--%>
            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">
                    <li class="hidden">
                        <a href="#page-top"></a>
                    </li>
                    <%--<li>
                        <a class="page-scroll" href="#services">Services</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#portfolio">Portfolio</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#about">About</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#team">Team</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#contact">Contact</a>
                    </li>--%>
                    <li>
                        <a class="page-scroll" href="main.aspx">ログイン</a>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>

    <!-- Header -->
    <header class="topdiv">
        <div class="topdiv1">
        <div class="container">
            <div class="intro-text">
                 <div class="row spacediv1">
                </div>
                <div class="row spacediv1">
                </div>
                <div class="row spacediv1">
                </div>
                <div class="intro-heading fontdiv1"><img alt="" src="images/logo.png" /></div>
                <div class="intro-lead-in fontdiv1 sth"><b>地域のみんなとつながる子育てSNS</b></div>

                <div style="width: 100%; color: #fff; font-size: large;" align="center">
                    まずは郵便番号を入力して地元情報をチェック！<br />(※数字と半角ハイフンを入力してください)
                    <br /><br />
                </div>
                <div style="width: 100%" align="center">
                <div style="background-color: rgba(51, 51, 51, 0.49); width: 50%; opacity:1;">
                    <br />
                    <input id="postcode_Text" type="text" style="text-align: center ;width: 80%; height: 50px; color: #000000;" placeholder="〒　000-0000" />
                    <br />
                    <br />
                    <input id="Button1" type="button" onclick="postcode()" value="さっそく始める" style="width: 80%; height: 50px;" class="file-upload" />
                    <br />
                    <br />

                </div>
                </div>
                <%--<a href="#services" class="page-scroll btn btn-xl">Tell Me More</a>--%>
            </div>
        </div>
             </div>
    </header>

    <!-- Services Section -->
    <section id="services">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading"></h2>
                    <h3 class="section-subheading text-muted"></h3>
                </div>
            </div>
           <!-- <div class="row text-center">
                <div class="col-md-4">
                    <h4 class="service-heading"></h4>

                    <p class="text-muted"></p>
                </div>
                <div class="col-md-4">
                    <h4 class="service-heading">提携自治体</h4>

                    <img alt="" src="images/home_images/gov.jpg" />
                </div>
                <div class="col-md-4">
                    <h4 class="service-heading"></h4>
                    <p class="text-muted"></p>
                </div>
            </div>-->
            <br/><br/>
            <div class="row text-center mapdiv">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <h3 class="section-heading mapdivfont">B版テスト中の工リア</h3>
                    <h4 class="text-muted mapdivfont">横浜市神奈川区</h4>
                </div>
                <div class="col-md-4"></div>
                <br/><br/>
                 <div class="col-lg-12 text-center">
                     <table style="width: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">
                                <div align="center">
                                    <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d25990.240276330147!2d139.60093894490916!3d35.48498582400563!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x60185eaf47e5e64b%3A0x423570cc7389d08d!2sKanagawa+Ward%2C+Yokohama%2C+Kanagawa+Prefecture!5e0!3m2!1svi!2sjp!4v1478676574296" width="100%" height="250" frameborder="0" style="border:0" allowfullscreen></iframe>
                </div>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                     </div>
                <br/><br/>
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <h3 class="section-heading mapdivfont">順次エリア拡大中</h3>
                </div>
                <div class="col-md-4"></div>
                <br/><br/>
            </div>
        </div>
    </section>

    <!-- Portfolio Grid Section -->
     <!-- <section id="portfolio" class="bg-light-gray">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <table style="width: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">
                                <iframe width="100%" height="350" src="https://www.youtube.com/embed/Vr-0lrK9OFg" frameborder="0" allowfullscreen></iframe>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                    <br/><br/>
                    <h2 class="section-heading"></h2>
                    <h3 class="section-subheading text-muted">ここにテキストが入りますここにテキストが入りますここにテキストが入りますここにテキストが入りますここにテキストが入ります</h3>
                </div>
            </div>
        </div>
    </section>

    <!-- About Section -->
    <section id="about">
        <div class="container">
            <div class="row comdiv">
                <div class="col-md-6">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/>
                                <img src="images/home_images/121.png" alt="">
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-6 text-center">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/> <br/><br/>
                                <h3 class="section-heading comtext">地元の情報も素早くキャッチ</h3>
                    <h4 class="section-subheading text-muted">ご近所さんやお店や自治体から様々な情報が配信されるので地域限定のおトク情報もすばやくキャッチ。</h4>
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                    </div>
            </div>
            <div class="row">
                <div class="col-md-6 text-center">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/> <br/><br/>
                                <h3 class="section-heading comtext">［未実装］近所の人に子どもをあずけれらる</h3>
                    <h4 class="section-subheading text-muted">よく顔を合わせる近所の方だからこそ安心して子どもをあずけることができます。</h4>
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                    </div>
                 <div class="col-md-6">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/>
                                <img src="images/home_images/120.png" alt="">
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row comdiv">
                <div class="col-md-6">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/>
                                <img src="images/home_images/122.png" alt="">
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-6 text-center">
                    <table style="width: 100%;height: 100%;">
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%" valign="middle">
                                <br/><br/> <br/><br/>
                                <h3 class="section-heading comtext">[未実装] 預ける手続きも簡単に</h3>
                    <h4 class="section-subheading text-muted">自宅にいながら預け・預かりを登録でき面談や講習のために出かける必要はありません。</h4>
                                <br/><br/>
                            </td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="10%">&nbsp;</td>
                            <td width="80%">&nbsp;</td>
                            <td width="10%">&nbsp;</td>
                        </tr>
                    </table>
                    </div>
            </div>
        </div>
    </section>

    <!-- Team Section -->
    <section id="team" class="mapdiv">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading text1">安全・安心への取り組み</h2>
                    <br/><br/>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-center">
                        <img src="images/home_images/safty01.png" alt="">
                        <h4 class="text1">自治体連携</h4>
                        <br/><br/>
                        <p class="text-muted text1">自治体と連携して取り組むことで信頼性の高い情報を配信しています。</p>
                </div>
                <div class="col-sm-4 text-center">
                        <img src="images/home_images/safty03.png" alt="">
                        <h4 class="text1">クチコミ情報</h4>
                        <br/><br/>
                        <p class="text-muted text1">ナマの声から実際の預かり状況などを確認することができます。</p>
                </div>
                <div class="col-sm-4 text-center">
                        <img src="images/home_images/safty02.png" alt="">
                        <h4 class="text1">暗号化技術</h4>
                        <br/><br/>
                        <p class="text-muted text1">高い暗号化技術を用い個人情報の漏洩リスクを低減します。</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Clients Aside -->
    <!-- <aside class="clients bg-light-gray">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <br/><br/>
                    <h2 class="section-heading">ニュース</h2>
                    <br/><br/>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1"></div>
  <div class="col-md-1"></div>
                <div class="col-md-8 text-center newmessdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td width="5%" height="5%">&nbsp;</td>
                            <td width="90%" height="5%">&nbsp;</td>
                            <td width="5%" height="5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="5%">&nbsp;</td>
                            <td width="90%">
                                <asp:Panel ID="new_message_Panel" runat="server" Height="400px" ScrollBars="Vertical"></asp:Panel>
                            </td>
                            <td width="5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="5%" height="5%">&nbsp;</td>
                            <td width="90%" height="5%">&nbsp;</td>
                            <td width="5%" height="5%">&nbsp;</td>
                        </tr>
                    </table>
                </div>
  <div class="col-md-1"></div>
  <div class="col-md-1"></div>
            </div>
             <div class="row">
                <div class="col-lg-12 text-center">
                    <br/><br/>
                    <h2 class="section-heading">メディア掲載</h2>
                    <br/><br/>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3 col-sm-6">
                    <a href="#">

                    </a>
                </div>
                <div class="col-md-3 col-sm-6">
                    <a href="#">
                        <img src="images/home_images/logo_footer_sp.png" class="img-responsive img-centered" alt="">
                    </a>
                </div>
                <div class="col-md-3 col-sm-6">
                    <a href="#">
                        <img src="images/home_images/logo12244.png" class="img-responsive img-centered" alt="">
                    </a>
                </div>
                <div class="col-md-3 col-sm-6">
                    <a href="#">

                    </a>
                </div>
            </div>
        </div>
    </aside>

    <!-- Contact Section -->
    <section id="contact">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">お問い合わせ</h2>
                    <br/><br/>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <form name="sentMessage" id="contactForm" novalidate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="お名前" id="name" data-validation-required-message="お名前お入力してください。">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="email" class="form-control" placeholder="メール" id="email" data-validation-required-message="メールお入力してください。">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="tel" class="form-control" placeholder="電話番号" id="phone" data-validation-required-message="電話番号お入力してください。">
                                    <p class="help-block text-danger"></p>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <textarea class="form-control" placeholder="メッセージ" id="message"  data-validation-required-message="メッセージお入力してください。"></textarea>
                                    <p class="help-block text-danger"></p>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-lg-12 text-center">
                                <div id="success"></div>

                                <button type="submit" class="btn btn-xl">送信</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>

    <footer class="footdiv">
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <br/><br/>
                     <ul class="list-inline social-buttons">
                    <table style="width: 100%;">
                        <tr>
                            <td width="30%" align="center">
                                <li><a href="">
                            <img alt="" src="images/home_images/twitter.png" />
                                        </a>
                        </li>
                            </td>
                            <td width="40%" align="center">
                                <li><a href="">
                            <img alt="" src="images/home_images/facebook.png" />
                                        </a>
                        </li>
                            </td>
                            <td width="30%" align="center">
                                <li><a href="">
                            <img alt="" src="images/home_images/instagram.png" />
                                        </a>
                        </li>
                            </td>
                        </tr>
                    </table>
                    </ul>
                    <br/><br/>
                </div>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                     <ul class="list-inline quicklinks">
                        <li><a class="text1" href="">運営企業</a>
                        </li>
                        <li><a class="text1" href="Help.html">プライバシーポリシー</a>
                        </li>
                    </ul>
                </div>
                <div class="col-md-4">
                </div>
            </div><div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                     <span class="copyright text1">Copyright &copy; <a class="text1" href="phone_key.aspx"></a> </span>
                </div>
                <div class="col-md-4">
                </div>
            </div>
        </div>
    </footer>


    <!-- jQuery -->
    <script src="vendor/jquery/jquery.js"></script>
    <script>
        $(function () {
            $('#postcode_Text').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    $('#Button1').click();
                    return false;
                }
            });



        });
        function postcode() {
            var checkclick_str = document.getElementById("postcode_Text").value;
            window.location.href = "main_guest.aspx?=" + checkclick_str;
        }
    </script>
    <!-- Bootstrap Core JavaScript -->
    <script src="vendor/bootstrap/js/bootstrap.js"></script>

    <!-- Plugin JavaScript -->
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>

    <!-- Contact Form JavaScript -->
    <script src="js/jqBootstrapValidation.js"></script>
    <script src="js/contact_me.js"></script>

    <!-- Theme JavaScript -->
    <script src="js/agency.js"></script>


</body>
</html>
