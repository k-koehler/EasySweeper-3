<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="EasyWeb.Main" %>
<%@ Import Namespace="Microsoft.ApplicationInsights.Extensibility.Implementation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<table>
    <thead>
        <tr>
            <th>Floor</th>
            <th>Time</th>
            <th>P1</th>
            <th>P2</th>
            <th>P3</th>
            <th>P4</th>
            <th>P5</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><%=Floors[0].FloorNum %></td>
            <td><%=Floors[0].Time %></td>
            <% foreach (var player in Floors[0].Players)
               {
                   Response.Write("<td>" + player.User + "</td>");
               }

            %>
            <%=Page.RouteData.Values["floor"]%>
        </tr>
    </tbody>
</table>
</body>
</html>
