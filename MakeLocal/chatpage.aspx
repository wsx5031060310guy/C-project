<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chatpage.aspx.cs" Inherits="chatpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SignalR 聊天室</title>
        <script src="Scripts/jquery-1.6.4.min.js" ></script>
    <!--Reference the SignalR library. -->
    <script src="Scripts/jquery.signalR-2.2.1.min.js"></script>
<%--很重要的參考，一定要加這一行，這之前一定要先參考jQuery.js與signalR.js--%>
<script src='<%= ResolveClientUrl("~/signalr/hubs") %>'></script>

<script type="text/javascript">
    $(function () {
        // Declare a proxy to reference the hub.          
        var notifications = $.connection.notificationHub;
        // Create a function that the hub can call to broadcast messages.
        notifications.client.recieveNotification = function (totalNewMessages) {
            // Add the message to the page.                    
            $('#spanNewMessages').text(totalNewMessages);
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
            notifications.server.sendNotifications();
        }).fail(function (e) {
            alert(e);
        });
        //$.connection.hub.start();
    });
    </script>
</head>
<body>
    <h1>New Notifications</h1>
<div>
   <b>You have <span id="spanNewMessages">0</span> New Message Notification.</b><br />
</div>
</body>
    </html>
