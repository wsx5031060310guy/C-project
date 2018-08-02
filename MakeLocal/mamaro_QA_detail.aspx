<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_QA_detail.aspx.cs" Inherits="mamaro_QA_detail" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>mamaro page</title>
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
                     url: "mamaro_QA_detail.aspx/check_mamaro",
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

             function exportTableToCSV($table, filename) {

                 var $rows = $table.find('tr:has(td)'),

                     // Temporary delimiter characters unlikely to be typed by keyboard
                     // This is to avoid accidentally splitting the actual contents
                     tmpColDelim = String.fromCharCode(11), // vertical tab character
                     tmpRowDelim = String.fromCharCode(0), // null character

                     // actual delimiter characters for CSV format
                     colDelim = '","',
                     rowDelim = '"\r\n"',

                     // Grab text from table into CSV formatted string
                     csv = '"' + $rows.map(function (i, row) {
                         var $row = $(row),
                             $cols = $row.find('td');

                         return $cols.map(function (j, col) {
                             var $col = $(col),
                                 text = $col.text();

                             return text.replace(/"/g, '""'); // escape double quotes

                         }).get().join(tmpColDelim);

                     }).get().join(tmpRowDelim)
                         .split(tmpRowDelim).join(rowDelim)
                         .split(tmpColDelim).join(colDelim) + '"';

                 // Deliberate 'false', see comment below
                 if (false && window.navigator.msSaveBlob) {

                     var blob = new Blob([decodeURIComponent(csv)], {
                         type: 'text/csv;charset=utf8'
                     });

                     // Crashes in IE 10, IE 11 and Microsoft Edge
                     // See MS Edge Issue #10396033: https://goo.gl/AEiSjJ
                     // Hence, the deliberate 'false'
                     // This is here just for completeness
                     // Remove the 'false' at your own risk
                     window.navigator.msSaveBlob(blob, filename);

                 } else if (window.Blob && window.URL) {
                     // HTML5 Blob
                     var blob = new Blob([csv], { type: 'text/csv;charset=utf8' });
                     var csvUrl = URL.createObjectURL(blob);

                     $(this)
                             .attr({
                                 'download': filename,
                                 'href': csvUrl
                             });
                 } else {
                     // Data URI
                     var csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

                     $(this)
             .attr({
                 'download': filename,
                 'href': csvData,
                 'target': '_blank'
             });
                 }
             }

             // This must be a hyperlink
             $("#export").on('click', function (event) {
                 // CSV
                 var args = [$('#dvData>table'), 'export.csv'];

                 exportTableToCSV.apply(this, args);

                 // If CSV, don't do event.preventDefault() or return false
                 // We actually need this to be a typical hyperlink
             });

             // This must be a hyperlink
             $("#export1").on('click', function (event) {
                 // CSV
                 var args = [$('#dvData_new>table'), 'export.csv'];

                 exportTableToCSV.apply(this, args);

                 // If CSV, don't do event.preventDefault() or return false
                 // We actually need this to be a typical hyperlink
             });



         });
     </script>
    <style type="text/css">
        a.export, a.export:visited {
    text-decoration: none;
    color:#000;
    background-color:#ddd;
    border: 1px solid #ccc;
}
             a.export1, a.export1:visited {
    text-decoration: none;
    color:#000;
    background-color:#ddd;
    border: 1px solid #ccc;
}
             #Panel1{padding:5px;border:1px solid #ccc;margin-bottom:3rem;}
#Panel1 fieldset:hover{background-color:#eee;}

		</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <div class="container">
            <div class="row">
                <div class="col-md-12">
        <fieldset><legend style="font-size: xx-large; font-weight: bold">All  QA detail</legend>
    <div>
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
                <br /><br />
               <div class="col-md-12">
                <a href='#' id='export' class="file-upload" style="width: 100%;">Export QA Detail Table data(csv)</a>
                    </div>
                 <br /><br />
                <div class="col-md-12">
                <a href='#' id='export1' class="file-upload" style="width: 100%;">Export new QA Detail Table data(csv)</a>
                    </div>
                </div>
            </div>
        <br />
          <div class="container">
            <div class="row">
                <div class="col-md-12">
        <div id="showplace" >
                    </div>
                    </div>
                </div>
            </div>



    </div>
    </form>
</body>
</html>
