<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" Async="true" %>
<% @ Import Namespace="EasyAPI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_player.User %> - Profile</title>
    <script type="text/javascript" src="/Scripts/jquery-3.2.1.js"></script>
</head>
<body>
    <div id="playerProfile"></div>
    <div id="floorList">
        <table>
            <thead>
                <tr>
                    <th>Theme</th>
                    <th>Time</th>
                    <th>P1</th>
                    <th>P2</th>
                    <th>P3</th>
                    <th>P4</th>
                    <th>P5</th>
                    <th>Image</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (Floor f in _floors) { %>
                    <tr class="floorRow <%=f.Theme %>">
                        <td class="themeCell"><%=f.Theme %></td>
                        <td class="timeCell"><%=f.FormattedTime %></td>
                            <%  int i;
                                var iter = f.Players.GetEnumerator();
                                Player player;
                                for (i = 0; iter.MoveNext(); i++) {
                                    player = iter.Current; %>
                            
                                <td class="playerCell"><a href="<%=player.User %>"><%=player.User %></a></td>
                            <%  }
                                Response.Write(new StringBuilder().Insert(0, "<td class=\"emptyPlayer\"></td>", 5 - i).ToString()); %>
                        <td class="dateCell"><%=f.Date.ToString("dd MMM yy") %></td>
                        <td class="imageCell"><a href="<%=f.Url%>">Img</a></td>
                    </tr>
                <% } %>
            </tbody>
        </table>
    </div>
</body>
</html>
