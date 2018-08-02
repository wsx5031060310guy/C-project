<%@ Page Language="C#" AutoEventWireup="true" CodeFile="content_main.aspx.cs" Inherits="content_main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="css/normalize.css">
	<link rel="stylesheet" href="css/bamboo.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style>
        body{
            height:100%;
        }
    .p01{
        background-image:url(images/p01.jpg);
        background-size:cover;
        background-repeat: no-repeat;
        height:100vh;
    }
    .p02{
        background-image:url(images/p02.jpg);
        background-size:cover;
        background-repeat: no-repeat;
        height:100vh;
    }
    .p03{
        background-image:url(images/p03.jpg);
        background-size:cover;
        background-repeat: no-repeat;
        height:100vh;
    }
    .p04{
        background-image:url(images/p04.jpg);
        background-size:cover;
        background-repeat: no-repeat;
        height:100vh;
    }
    .p05{
        background-image:url(images/p05.jpg);
        background-size:cover;
        background-repeat: no-repeat;
        height:100vh;
    }
    h1{
        font-size:60px;
        color:ghostwhite;
        text-shadow:0px 0px 15px black;
    }

</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="p01"><h1>Page 1</h1></div>

                        <div class="p02"><h1>Page 2</h1></div>

                        <div class="p03"><h1>Page 3</h1></div>

                        <div class="p04"><h1>Page 4</h1></div>

                        <div class="p05"><h1>Page 5</h1></div>
    </div>
    </form>
</body>
</html>
