<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" Async="true" %>
<% @ Import Namespace="EasyAPI" %>
<% @ Import Namespace="EasyWeb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_player.User %> - Profile</title>
    <script type="text/javascript" src="/Scripts/jquery-3.2.1.js"></script>
</head>
<body>
    <div id="playerProfile"></div>
    <div id="floorList">
        <%=HTMLGenerator.GenerateTable<Floor>(_floors, "Floor", "Theme", "P1", "Img").ToString() %>
    </div>
</body>
</html>
