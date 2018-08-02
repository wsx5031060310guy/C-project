<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mamaro_select.aspx.cs" Inherits="mamaro_select" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>mamaro</title>
       <script src="Scripts/jquery-1.12.4.js"></script>
    <script src="js/ajaxfileupload.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/jquery-ui.css">
     <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
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
.col-xs-6 {
    margin-top: 60px;
    padding-right: 0px;
    padding-left: 0px;
    font-size: 16px;
}
#data_but {
    margin: 0px;
    margin-left: 40px;
    border: 5px solid #191970;
    cursor:pointer;
    text-align:center;
}
#babymap_but {
    margin: 0px;
    margin-right: 40px;
    border: 5px solid #191970;
    cursor:pointer;
    text-align:center;
}
@media screen and (min-width: 768px){

.col-xs-6{
  font-size: xx-large;

}
}
</style>
</head>
<body>
    <div class="row">
  <div class="col-xs-6" style="text-align:center;">
      <div id="data_but">
                Data
  </div>

  </div>
  <div class="col-xs-6" style="text-align:center;">
   <div id="babymap_but">
       Edit info of Babymap
  </div>
  </div>
</div>
    <form id="form1" runat="server">
    <div>
           <asp:Panel ID="main_Panel" runat="server" HorizontalAlign="Center">
        </asp:Panel>
    </div>
    </form>
</body>
</html>
