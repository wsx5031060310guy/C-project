<%@ Page Language="C#" AutoEventWireup="true" CodeFile="photo.aspx.cs" Inherits="photo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/linear-partition.js"></script>
     <!-- Magnific Popup core CSS file -->
<link rel="stylesheet" href="css/magnific-popup.css">
<!-- Magnific Popup core JS file -->
<script src="js/jquery.magnific-popup.js"></script>
    <script>
        $(function () {
            $('.image-link').magnificPopup({
                type: 'image',
                mainClass: 'mfp-with-zoom', // this class is for CSS animation below

                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it
                    
                    duration: 300, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function

                    // The "opener" function should return the element from which popup will be zoomed in
                    // and to which popup will be scaled down
                    // By defailt it looks for an image tag:
                    opener: function (openerElement) {
                        // openerElement is the element on which popup was initialized, in this case its <a> tag
                        // you don't need to add "opener" option if this code matches your needs, it's defailt one.
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
                            return item.el.attr('title') + ' &middot; <a class="image-source-link" href="' + item.el.attr('data-source') + '" target="_blank">image source</a>';
                        }
                    },
                    gallery: {
                        enabled: true
                    },
                    zoom: {
                        enabled: true,
                        duration: 300, // don't foget to change the duration also in CSS
                        opener: function (element) {
                            return element.find('img');
                        }
                    }
                });
            });
        });

        ///scroll new state list
        $(function () {
            var count = 10;
            $(window).scroll(function () {
                if ($(window).scrollTop() + $(window).height() == $(document).height()) {
                    //alert("bottom!");
                    count += 10;
                    $.ajax({
                        type: "POST",
                        url: "photo.aspx/search_new_photo",
                        data: "{param1: '" + count + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            //Successfully gone to the server and returned with the string result of the server side function do what you want with the result  

                            console.log(count);
                            //console.log(result.d);
                            //window.location.href = "main.aspx";
                            $('#javaplace_forphoto').empty();
                            $('#javaplace_forphoto').append(result.d);

                            //var whitebg = document.getElementById("white-background");
                            //var dlg = document.getElementById("dlgbox_report_" + up_str);
                            //whitebg.style.display = "none";
                            //dlg.style.display = "none";
                        },
                        error: function (result) {
                            //console.log(result.Message);
                            //alert(result.d);
                        }
                    });


                }
            });
        });

        </script>
    <style>
        #workspace{
	margin-left: 3%;
}
.photo{
	float: left;
	position: relative;
	background-position: center;
	background-size: 100% 100%;
	background-color: #77c4a5;
	/*margin: 2px;*/
}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="workspace">
    
    </div>
        <asp:Panel ID="javaplace_forphoto" runat="server"></asp:Panel>
    </form>
</body>
</html>
