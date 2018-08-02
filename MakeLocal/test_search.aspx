<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_search.aspx.cs" Inherits="test_search" validateRequest="False"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.12.4.js"></script>
      <script src="Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui.css">
       <!-- Magnific Popup core CSS file -->
<link rel="stylesheet" href="css/magnific-popup.css">

<!-- Magnific Popup core JS file -->
<script src="js/jquery.magnific-popup.js"></script>
    <style>
          .ui-autocomplete {
  max-height: 200px;
  overflow-y: auto;
  overflow-x: hidden;
  padding-right: 10px;
        white-space: nowrap;
        font-size: 16px;
     }
          .ui-autocomplete li div{
    display:block;
    height:55px;
    vertical-align:text-top;
     }
    </style>
    <script>
        $(document).ready(function () {
            $('.image-link').magnificPopup({ type: 'image' });

            $("#<%=txtSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "test_search.aspx/Getsearch",
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(';')[0],
                                    val: item.split(';')[1],
                                    icon: item.split(';')[2]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    window.location.href = "user_home_friend.aspx?=" + i.item.val;
                },
                minLength: 1
            });



            $("#<%=txtSearch.ClientID %>").data("ui-autocomplete")._renderItem = function (ul, item) {

                var $li = $('<li>'),
                    $img = $('<img>');

                $img.attr({
                    src: item.icon,
                    alt: item.label,
                    width: "50px",
                    height: "50px"
                });

                $li.attr('data-value', item.label);


                //$li.append('<a href="#">');
                //$li.find('a').append($img).append(item.label);

                $li.append('<div>');
                //$li.find('div').append($img).append(item.label);
                $li.find('div').append($img).append(item.label);

                return $li.appendTo(ul);
            };

            //search friend
            $("#<%=txtSearch_f.ClientID %>").autocomplete({
                source: function (request, response) {

                    var param1 = "1";
                    $.ajax({
                        url: "test_search.aspx/Getsearch_friend",
                        data: "{ 'prefix': '" + request.term + "','who':'" + param1 + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(';')[0],
                                    val: item.split(';')[1],
                                    icon: item.split(';')[2]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    window.location.href = "user_home_friend.aspx?=" + i.item.val;
                },
                minLength: 1
            });



            $("#<%=txtSearch_f.ClientID %>").data("ui-autocomplete")._renderItem = function (ul, item) {

                var $li = $('<li>'),
                    $img = $('<img>');

                $img.attr({
                    src: item.icon,
                    alt: item.label,
                    width: "50px",
                    height: "50px"
                });
                $li.attr('data-value', item.label);
                $li.append('<div>');
                $li.find('div').append($img).append(item.label);

                return $li.appendTo(ul);
            };


        });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hfCustomerId" runat="server" />
        <asp:TextBox ID="txtSearch_f" runat="server"></asp:TextBox>
    </form>
</body>
</html>
