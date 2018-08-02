<%@ Page Language="C#" AutoEventWireup="true" CodeFile="nursing_room_counter.aspx.cs" Inherits="nursing_room_counter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Door counter page</title>
    <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
        <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui.css">
     <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
    <script src="Scripts/datepicker-ja.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.6.0/Chart.js"></script>
    <script src="Scripts/utils.js"></script>
     <script type="text/javascript">
         function myDateFormatter(dateObject) {
             var d = new Date(dateObject);
             var day = d.getDate();
             var month = d.getMonth() + 1;
             var year = d.getFullYear();
             if (day < 10) {
                 day = "0" + day;
             }
             if (month < 10) {
                 month = "0" + month;
             }
             var date = year + "-" + month + "-" + day;

             return date;
         }; 
         $(document).ready(function () {
             $("#Button1").click(function () {
                 $.ajax({
                     type: "POST",
                     url: "nursing_room_counter.aspx/check_counter",
                     data: "{param1: '" + myDateFormatter($('#datepicker1').val()) + "' , param2 :'" + myDateFormatter($('#datepicker2').val()) + "' }",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: true,
                     cache: false,
                     success: function (result) {
                         //Successfully gone to the server and returned with the string result of the server side function do what you want with the result  
                         //console.log(result.d);
                         //alert(result.d);
                         //alert(result.d);
                         $('#showplace').empty();
                         $('#showplace').append(result.d);
                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });

             });
             $("#Button2").click(function () {
                 window.location.href = "check_post.aspx";

             });
         });
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <input id="Button2" type="button" value="User Post" class="file-upload" style="width: 100%;" />
                       </div>
                </div>
            </div>
        <br/>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
        <fieldset><legend style="font-size: xx-large; font-weight: bold">Door</legend>
    <div>
     <br />
        <br />
        
         <asp:Label ID="Label1" runat="server" Text="Start : " Font-Size="Larger"></asp:Label>
                                                        <input type='text' name='datepicker1' id='datepicker1' class='textbox' placeholder='2016/01/01' readonly style="font-size: larger">
                                                    <script>
                                                        $(function () {
                                                            $("#datepicker1").datepicker({
                                                                format: 'yyyy-mm-dd',
                                                                language: 'ja',
                                                                changeMonth: true,
                                                                changeYear: true,
                                                                autoclose: true, // これ
                                                                clearBtn: true,
                                                                clear: '閉じる'
                                                            });
                                                        });
                                                    </script>
        <br /><asp:Label ID="Label2" runat="server" Text="End : " Font-Size="Larger"></asp:Label>
        <input type='text' name='datepicker2' id='datepicker2' class='textbox' placeholder='2016/01/01' readonly style="font-size: larger">
                                                    <script>
                                                        $(function () {
                                                            $("#datepicker2").datepicker({
                                                                format: 'yyyy-mm-dd',
                                                                language: 'ja',
                                                                changeMonth: true,
                                                                changeYear: true,
                                                                autoclose: true, // これ
                                                                clearBtn: true,
                                                                clear: '閉じる'
                                                            });
                                                        });
                                                    </script>
        <hr>
        <br />
        <br />
    
    </div>
            </fieldset>
                     </div>
                </div>
            </div>
                  <div class="container">
            <div class="row">
                <div class="col-md-12">                                     
         <input id="Button1" type="button" value="次へ" class="file-upload" style="width: 100%;" />
                    </div>
                </div>
            </div>
        <br />
          <div class="container">
            <div class="row">
                <div class="col-md-12">          
        <div id="showplace" ></div>
                    </div>
                </div>
            </div>
    </div>
    </form>
</body>
</html>
