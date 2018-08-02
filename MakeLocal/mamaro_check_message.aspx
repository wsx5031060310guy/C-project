<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_check_message.aspx.cs" Inherits="mamaro_check_message" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>mamaro</title>
    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="js/ajaxfileupload.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Styles/style.css">
    <style>
        body{
            margin:0px;
        }
.center {
    margin: auto;
    margin-top: 60px;
    width: 60%;
    border: 3px solid #73AD21;
    padding: 10px;
    height:auto;
    cursor:pointer;
    text-align:center;
    font-size: xx-large;
}
</style>
    <script language="JavaScript">
	window.onload = function()
	{
		var lis = document.getElementById('cssdropdown').getElementsByTagName('li');
        var mcl=document.getElementById('menubutton');
		for(i = 0; i < lis.length; i++)
		{
			var li = lis[i];
			if (li.className == 'headlink')
			{
				li.onmouseover = function() { this.getElementsByTagName('ul').item(0).style.display = 'block';mcl.classList.toggle("change");}
				li.onmouseout = function() { this.getElementsByTagName('ul').item(0).style.display = 'none';mcl.classList.toggle("change");}
			}
		}

if ($(window).width() < 768){
$('#logo_pc').css( 'width', '180px' );
document.getElementById('headermenu').innerHTML='';
$('#newdiv').css( 'width', '100%' );
$('#newdiv').css( 'margin-left', '1%' );
$('#ff-stream-1').css( 'margin', '0px' );
}else{
$('#logo_pc').css( 'width', '150px' );
document.getElementById('headermenu').innerHTML='Menu';
// $('#newdiv').css( 'width', '74%' );
// $('#newdiv').css( 'margin-left', '35%' );
$('#ff-stream-1').css( 'margin', '0px 30%' );
}
window.onresize = function() {
if ($(window).width() < 768){
$('#logo_pc').css( 'width', '180px' );
document.getElementById('headermenu').innerHTML='';
$('#newdiv').css( 'width', '100%' );
$('#newdiv').css( 'margin-left', '1%' );
$('#ff-stream-1').css( 'margin', '0px' );

}else{
$('#logo_pc').css( 'width', '150px' );
document.getElementById('headermenu').innerHTML='Menu';
// $('#newdiv').css( 'width', '74%' );
// $('#newdiv').css( 'margin-left', '35%' );
$('#ff-stream-1').css( 'margin', '0px 30%' );

}
};


$('#cssdropdown').click(function(){

var lis = document.getElementById('cssdropdown').getElementsByTagName('li');
        var mcl=document.getElementById('menubutton');
		for(i = 0; i < lis.length; i++)
		{
			var li = lis[i];
			if (li.className == 'headlink')
			{
  if(this.getElementsByTagName('ul').item(0).style.display == 'block'){
this.getElementsByTagName('ul').item(0).style.display = 'none';mcl.classList.toggle("change");
if ($(window).width() < 768){
$('.header').css( 'height', '' );
$('.header').css( 'background-color', 'rgba(255,255,255,0.7)' );
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
}else{
 if($(window).scrollTop()>700){
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
$('#headermenu').css( 'color', 'rgb(110,110,110)' );
}else{

     $('.bar1').css('background-color', 'rgb(110,110,110)');
     $('.bar2').css('background-color', 'rgb(110,110,110)');
     $('.bar3').css('background-color', 'rgb(110,110,110)');
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
}

}
}
else{this.getElementsByTagName('ul').item(0).style.display = 'block';mcl.classList.toggle("change");
if ($(window).width() < 768){
$('.header').css( 'height', '450' );
$('.header').css( 'background-color', 'rgba(255,255,255,0.7)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );

}else{
	$('.header').css( 'background-color', 'rgba(0,0,0,0)' );
	$('#cssdropdown').find("li").find("a").css('color', 'rgb(110,110,110)');
}
}
			}
		}
});
$('#menubutton').click(function(){

var lis = document.getElementById('cssdropdown').getElementsByTagName('li');
        var mcl=document.getElementById('menubutton');
		for(i = 0; i < lis.length; i++)
		{
			var li = lis[i];
			if (li.className == 'headlink')
			{
  if(this.getElementsByTagName('ul').item(0).style.display == 'block'){
this.getElementsByTagName('ul').item(0).style.display = 'none';mcl.classList.toggle("change");
if ($(window).width() < 768){
$('.header').css( 'height', '' );
$('.header').css( 'background-color', 'rgba(255,255,255,0.7)' );
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
}else{
 if($(window).scrollTop()>700){
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
$('#headermenu').css( 'color', 'rgb(110,110,110)' );
}else{

     $('.bar1').css('background-color', 'rgb(110,110,110)');
     $('.bar2').css('background-color', 'rgb(110,110,110)');
     $('.bar3').css('background-color', 'rgb(110,110,110)');
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
}

}

}
else{this.getElementsByTagName('ul').item(0).style.display = 'block';mcl.classList.toggle("change");
if ($(window).width() < 768){
$('.header').css( 'height', '450' );
$('.header').css( 'background-color', 'rgba(255,255,255,0.7)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );

}else{
	$('.header').css( 'background-color', 'rgba(0,0,0,0)' );
 if($(window).scrollTop()>700){
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
}else{

     $('.bar1').css('background-color', 'rgb(110,110,110)');
     $('.bar2').css('background-color', 'rgb(110,110,110)');
     $('.bar3').css('background-color', 'rgb(110,110,110)');
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
}
}

}
			}
		}
});
	}

	/* or with jQuery:
	$(document).ready(function(){
		$('#cssdropdown li.headlink').hover(
			function() { $('ul', this).css('display', 'block'); },
			function() { $('ul', this).css('display', 'none'); });
	});
	*/
if ($(window).width() < 768){
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
$('#cssdropdown').find("li").find("a").css( 'color', 'rgb(110,110,110)' );
$('.header').css( 'background-color', 'rgba(255,255,255,0.7)' );
$('.bar1').css( 'background-color', 'rgb(110,110,110)' );
$('.bar2').css( 'background-color', 'rgb(110,110,110)' );
$('.bar3').css( 'background-color', 'rgb(110,110,110)' );

}else{
    $('.bar1').css('background-color', 'rgb(110,110,110)');
    $('.bar2').css('background-color', 'rgb(110,110,110)');
    $('.bar3').css('background-color', 'rgb(110,110,110)');
$('#logo_ph').css( 'color', 'rgb(110,110,110)' );
$('#cssdropdown').find("li").find("a").css( 'color', '#fff' );
}




</script>
</head>
<body>


    <header class="header" style="background-color: rgba(0, 0, 0, 0);">
 <h1><a id="logo_pc" href="" title="ホーム" style="text-decoration: none; width: 150px;">

<span id="logo_ph" style="font-family: futura; color: rgb(110, 110, 110); font-size: 22px; font-style: italic; letter-spacing: 2px;">mamaro</span>
</a>
</h1>
</header>
    <div class="nav">
  <nav>
  <div id="menubutton" class="container">
  <div class="bar1" style="background-color: rgb(110, 110, 110);"></div>
  <div class="bar2" style="background-color: rgb(110, 110, 110);"></div>
  <div class="bar3" style="background-color: rgb(110, 110, 110);"></div>
</div>
<ul id="cssdropdown">

		<li class="headlink">
			<a id="headermenu" style="font-family: futura; cursor: pointer; font-size: 26px; margin-top: -12px; color: rgb(110, 110, 110);">Menu</a>

			<ul style="display: none;">

				<li><a href="./mamaro_select.aspx" style="color: rgb(110, 110, 110);">TOP</a></li>
				<li><a href="./mamaro_check_message.aspx" style="color: rgb(110, 110, 110);">Report & message</a></li>
				<li><a href="" style="color: rgb(110, 110, 110);">お問い合わせ</a></li>
				<li><a href="" style="color: rgb(110, 110, 110);">Trim inc.</a></li>
			</ul>
		</li>

	</ul>

  </nav>
</div>
    <form id="form1" runat="server">
    <div>
           <asp:Panel ID="main_Panel" runat="server" HorizontalAlign="Center">
        </asp:Panel>
    </div>
    </form>
</body>
</html>
