<%@ Page Language="C#" AutoEventWireup="true" CodeFile="check_post.aspx.cs" Inherits="check_post" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
    <link href="vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
        <link rel="stylesheet" href="css/file-upload.css" type="text/css" />
    <script src="Scripts/jquery-1.12.4.js"></script>
     <script type="text/javascript">

         $(document).ready(function () {
             $("#Button1").click(function () {
                 window.location.href = "mamaro_main.aspx";

             });
         });
     </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <input id="Button1" type="button" value="mamaro" class="file-upload" style="width: 100%;" />
                       </div>
                </div>
            </div>
        <br/>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
        <fieldset><legend style="font-size: xx-large; font-weight: bold">How Many</legend>
    <div>
    
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="User Count:"></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Post Count:"></asp:Label>
        <br />
        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
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
         <fieldset><legend style="font-size: xx-large; font-weight: bold">Area How Many</legend>
    <div>
    
        <br />
        <br />
        <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Both">
        </asp:Panel>
        <br />
    
    </div>
            </fieldset>
                     </div>
                </div>
            </div>
         <div class="container">
            <div class="row">
                <div class="col-md-12">
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>
        <fieldset><legend style="font-size: xx-large; font-weight: bold">Post Detail</legend>
            <asp:Panel ID="Panel2" runat="server"></asp:Panel>
    <div style="width: 100%">
    
        <br />
        <br />
         <fieldset><legend style="font-size: large; font-weight: bold">お食事</legend>
             <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>

             </fieldset>
        <br />
        <br />
                 <fieldset><legend style="font-size: large; font-weight: bold">イベント</legend>
                     <br />
                     <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
       
             </fieldset>
    <br /><br />
         <fieldset><legend style="font-size: large; font-weight: bold">病院</legend>
             <br />
             <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        
             </fieldset>
    <br /><br />
         <fieldset><legend style="font-size: large; font-weight: bold">公園／レジャー</legend>
             <br />
             <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        
             </fieldset>
    <br /><br />
         <fieldset><legend style="font-size: large; font-weight: bold">授乳室</legend>
             <br />
             <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        
             </fieldset>
    <br /><br />
        <fieldset><legend style="font-size: large; font-weight: bold">指定なし</legend>
            <br />
            <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
       
             </fieldset>
    <br /><br />


    </div>
            </fieldset>
                             </ContentTemplate>
                 </asp:UpdatePanel>
                      </div>
                </div>
            </div>
    </form>
    <script src="vendor/jquery/jquery.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.js"></script>

    <!-- Plugin JavaScript -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>
</body>
</html>
