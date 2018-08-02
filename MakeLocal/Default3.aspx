<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
<script type="text/javascript">

     var items = [["Q458", "", "100", "85"], ["Q459", "TS", "90", "65"], ["Q460", "", "80", "15"]];
     sendToDb();
     function Create2DArray(rows) {
         var arr = [];
         for (var i = 0; i < rows; i++) {
             arr[i] = [];
         }
         return arr;
     }
     function useround(min, max) {
         return Math.round(Math.random() * (max - min) + min);
     }
     function usefloor(min, max) {
         return Math.floor(Math.random() * (max - min + 1) + min);
     }
     function sendToDb() {
         var pointgray = Create2DArray(500);
         for (i = 0; i < 500; i++) {
             for (j = 0; j < 500; j++) {
                 pointgray[i][j] = usefloor(0, 255);
             }
         }
         var inString = JSON.stringify(pointgray);

         $.ajax({
             url: 'some_generic_handler.ashx',
             dataType: 'json',
             type: 'post',
             async: true,
             data: { myVar: inString },
             success: function (data) {
                 if (data.success == true) {
                     var inStringx = data.xElement;
                     var inStringy = data.xElement;
                     alert("Here's the first element in the array: " + inStringx)
                     alert(data.message);
                     var res = [],res1=[];
                     res = inStringx.split(",");
                     res1 = inStringy.split(",");
                     document.getElementById("demo").innerHTML = res[0];
                 }

             },
             error: function (xhr, ajaxOptions, thrownError) {
                 alert(xhr.status);
                 alert(thrownError);
             }
         });
     }
 </script>
 <p id="demo"></p>
</body>
</html>
