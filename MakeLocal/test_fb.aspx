<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_fb.aspx.cs" Inherits="test_fb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
        <script>
            window.fbAsyncInit = function () {
                FB.init({
                    appId: '',
                    xfbml: true,
                    version: 'v2.8',
                    scope: 'id,name,first_name,last_name,gender,locale,link,cover,picture,email',
                    status: true, // check login status
                    cookie: true // enable cookies to allow the server to access the session
                });
                // listen for and handle auth.statusChange events
                FB.Event.subscribe('auth.statusChange', OnLogin);
            };

            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) { return; }
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/ja_JP/sdk.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));

            function OnLogin(response) {
                if (response.authResponse) {
                    console.log(response.authResponse);
                    FB.api('/me?fields=id,name,first_name,last_name,gender,locale,link,cover,picture,email', LoadValues);
                }
            }

            //This method will load the values to the labels
            function LoadValues(me) {
                console.log(me);
                if (me.name) {
                    var myname = me.name;
                    if (myname)
                    {
                        document.getElementById('displayname').innerHTML = me.name;
                    }
                    myname = me.id;
                    if (myname) {
                        document.getElementById('FBId').innerHTML = me.id;
                    }
                    myname = me.email;
                    if (myname) {
                        document.getElementById('DisplayEmail').innerHTML = me.email;
                    }
                    myname = me.gender;
                    if (myname) {
                        document.getElementById('Gender').innerHTML = me.gender;
                    }
                    myname = me.picture;
                    if (myname) {
                        document.getElementById('pic').src = me.picture.data.url;
                    }
                    myname = me.cover;
                    if (myname) {
                        document.getElementById('cov').src = me.cover.source;
                    }
                    document.getElementById('auth-loggedin').style.display = 'block';
                }
            }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="fb-root"></div> <!-- This initializes the FB controls-->
<div class="fb-login-button" autologoutlink="false" scope="email" >
  Login with Facebook
 </div> <!-- FB Login Button -->
<!-- Details -->
<div id="auth-status">
<div id="auth-loggedin" style="display: none">
    Hi, <span id="displayname"></span><br/>
    Your Facebook ID : <span id="FBId"></span><br/>
    Your Email : <span id="DisplayEmail"></span><br/>
    Your Sex: <span id="Gender"></span><br/>
    Your Picture: <img alt="" src="" id="pic"/>
    <br/>
    Your cover:
    <img alt="" src="" id="cov"/>
    <br/>
</div>
</div>
    </form>
</body>
</html>
