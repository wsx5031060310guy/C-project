<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_message.aspx.cs" Inherits="mamaro_message" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title> message</title>
     <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
        <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui.css">
     <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
    <script src="https://www.gstatic.com/firebasejs/4.8.0/firebase.js"></script>
         <script type="text/javascript">
             var firebase;
             $(document).ready(function () {
                 var config = {
                     databaseURL: ""
                 };

                 firebase.initializeApp(config);
                 var database = firebase.database().ref();

             $("#Button1").click(function () {
                 $.ajax({
                     type: "POST",
                     url: "mamaro_message.aspx/write_message",
                     data: "{param1: '" + $('#mamaro_TextArea').val() + "' }",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: true,
                     cache: false,
                     success: function (result) {
                         //Successfully gone to the server and returned with the string result of the server side function do what you want with the result
                         //console.log(result.d);
                         //alert(result.d);
                         //alert(result.d);
                         var str = result.d;

                         if (str != "" || str != null) {
                             var d = new Date();

                             var month = d.getMonth() + 1;
                             var day = d.getDate();
                             var hour = d.getHours();
                             var min = d.getMinutes();
                             var sec = d.getSeconds();
                             var mill = d.getMilliseconds();

                             var filename = '';

                             var output = d.getFullYear() + '-' +
                                (('' + month).length < 2 ? '0' : '') + month + '-' +
                                (('' + day).length < 2 ? '0' : '') + day + ' ' +
                                (('' + hour).length < 2 ? '0' : '') + hour + ':' +
                                (('' + min).length < 2 ? '0' : '') + min + ':' +
                                (('' + sec).length < 2 ? '0' : '') + sec + '.' +
                                (('' + mill).length < 3 ? '0' : '') + (('' + mill).length < 2 ? '0' : '') + mill;
                             firebase.database().ref('mamaro/' + str).push({
                                 message: $('#mamaro_TextArea').val(),
                                 time: output,
                                 type: '0',
                                 id: str
                             });

                         }

                         window.location = "";
                         //$('#showplace').empty();
                         //$('#showplace').append("thank you");
                     },
                     error: function (result) {
                         console.log(result.d);
                     }
                 });

             });


         });
     </script>
</head>
<body>
    <fieldset>
        <legend style="font-size: xx-large; font-weight: bold;text-align:  center;">mamaro message</legend>
        <p>
            <textarea style="width: 100%;height:500px;" id = "mamaro_TextArea"
                  rows = "3"
                  cols = "80">書き込もう</textarea>
        </p>
      </fieldset>
    <div class="container">
            <div class="row">
                <div class="col-md-12">
         <input id="Button1" type="button" value="次へ" class="file-upload" style="width: 100%;" />
                    </div>
                <br /><br />
                </div>
            </div>
    <div class="container">
            <div class="row">
                <div class="col-md-12">
        <div id="showplace" >
                    </div>
                    </div>
                </div>
            </div>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
