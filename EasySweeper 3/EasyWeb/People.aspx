<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script type="text/javascript" src="/Service/API.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            API.test("test", (obj_player) => {
                $("#pid").text($("#pid").text() + obj_player.ID);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>Generated Page for <%=RouteData.Values["Username"] %></p>
            <p id="pid">API.js returned Player ID: </p>
        </div>
    </form>
</body>
</html>
