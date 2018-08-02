<%@ Page Language="C#" AutoEventWireup="true" CodeFile="phone_key.aspx.cs" Inherits="phone_key" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>phone key</title>
    <script src="vendor/jquery/jquery.js"></script>
      <script type="text/javascript">
          var checkstr = "";
          function topFunction() { checkstr += "top"; };
          function downFunction() { checkstr += "down"; };
          function leftFunction() { checkstr += "left"; };
          function rightFunction() { checkstr += "right"; };
          function AFunction() {
              checkstr += "A";
              console.log(checkstr);
              if (checkstr == "toptopdowndownleftrightleftrightBA") {
                  activateCheats();
              }
              checkstr = "";
          };
          function BFunction() { checkstr += "B"; };
          function activateCheats() {

              $.ajax({
                  type: "POST",
                  url: "phone_key.aspx/check_mamaro",
                  data: "{param1: 'tt' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: true,
                  cache: false,
                  success: function (result) {
                      if (result.d == "out") {
                          window.location = "3Dbox.aspx";
                      }
                  },
                  error: function (result) {
                      console.log(result.d);
                  }
              });
          }
      </script>
    <style>
        body,
        html {
            height: 100%;
            padding: 0;
            margin: 0;
            box-sizing: border-box;
            overflow: hidden;
            background-color: rgb(255, 255, 255);
            position: relative;
        }

        .wraper {
            position: absolute;
            width: 100%;
            height: 260px;
            top:0;
    right:0;
    bottom:0;
    left:0;
    margin:auto;
            perspective: 1000px;
        }
        </style>
</head>
<body>
    <div class="wraper">
    <input type="button" style="background-image:url('images/manager/left.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="leftFunction();"/>
    <input type="button" style="background-image:url('images/manager/top.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="topFunction();"/>
    <input type="button" style="background-image:url('images/manager/down.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="downFunction();"/>
    <input type="button" style="background-image:url('images/manager/right.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="rightFunction();"/>
    <input type="button" style="background-image:url('images/manager/B.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="BFunction();"/>
    <input type="button" style="background-image:url('images/manager/A.png');width:100px;height:100px;background-color:#fff;background-size: cover;background-repeat: no-repeat;border-style: none;cursor: pointer;" onclick="AFunction();"/>

    </div>    
    
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
