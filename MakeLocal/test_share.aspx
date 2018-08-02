<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_share.aspx.cs" Inherits="test_share" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>


<%--<link type="text/css" rel="stylesheet" href="https://cdn.jsdelivr.net/jquery.jssocials/1.4.0/jssocials.css" />

<link type="text/css" rel="stylesheet" href="https://cdn.jsdelivr.net/jquery.jssocials/1.4.0/jssocials-theme-flat.css" />
<link type="text/css" rel="stylesheet" href="https://cdn.jsdelivr.net/jquery.jssocials/1.4.0/jssocials-theme-classic.css" />
<link type="text/css" rel="stylesheet" href="https://cdn.jsdelivr.net/jquery.jssocials/1.4.0/jssocials-theme-minima.css" />
<link type="text/css" rel="stylesheet" href="https://cdn.jsdelivr.net/jquery.jssocials/1.4.0/jssocials-theme-plain.css" />--%>
<%--    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css">--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="css/jssocials.css" />
    <link rel="stylesheet" type="text/css" href="css/jssocials-theme-flat.css" />
    <style>
        .jssocials-share-link { border-radius: 50%; }
    </style>
</head>
<body>


    <div id="share" height="100px"></div>
        <script src="Scripts/jssocials.js"></script>
        <script>
            $("#share").jsSocials({
                showLabel: false,
                showCount: false,
                shares: ["email", "twitter", "facebook", "googleplus", "linkedin", "pinterest", "stumbleupon", "whatsapp", "telegram", "line"],
                url: "",
                text: "",
                shareIn: "popup"
            });
    </script>
    <form id="form1" runat="server">

    <div>

    </div>
    </form>
</body>
</html>
