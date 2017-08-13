<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" Async="true" MasterPageFile="~/Master.Master"%>
<% @ Import Namespace="EasyAPI" %>
<% @ Import Namespace="EasyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<!DOCTYPE html>

<link rel="stylesheet" type="text/css" href="/CSS/People.css"/>
<div id="playerProfile"></div>
<h1><%=_player.User %></h1>
<div id="categoryControls">
        <% for (int i = 1; i <= 5; i++) { %>
            <button class="categoryButton" onclick="NavigateToCategory(<%=i %>)"><%=i %>s</button>
        <% } %>
    <button class="categoryButton" onclick="NavigateToCategory('Summary')">Summary</button>
</div>
<% if (_count != null) { %>
    <div id="floors">
        <% foreach (string theme in Floor.Themes)
             { %>
        <h2><%=theme%></h2>
        <%=GenerateTable(_floors, theme, _count.Value) %>
        <br />
        <% } %>
    </div>
<% } else { %>
    <div id="aggregates">
    <%foreach (Aggregates a in _aggregates) { %>
        <table>
            <tr>
                <td class="summary
                    
                    
                    
                    "><%=Global.PlayerCountToString(a.PlayerCount) + " " + a.Theme %></td>
                <td><%=GenerateAggregates(a) %></td>
            </tr>
        </table>
        <h2></h2>
    <% } %>
    </div>
<% } %>
<script type="text/javascript">
    function NavigateToCategory(str_category) {
        window.location.href = "/People/<%=_player.User%>/" + str_category;
    }
</script>
</asp:Content>