<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" Async="true" MasterPageFile="~/Master.Master"%>
<% @ Import Namespace="EasyAPI" %>
<% @ Import Namespace="EasyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<!DOCTYPE html>

<script type="text/javascript" src="/Scripts/jquery-3.2.1.js"></script>
<link rel="stylesheet" type="text/css" href="/CSS/People.css"/>
<div id="playerProfile"></div>
<h1><%=_player.User %></h1>
<div id="categoryControls">
        <% for (int i = 1; i <= 5; i++) { %>
            <button class="categoryButton" onclick="NavigateToCategory(<%=i %>)"><%=i %>s</button>
        <% } %>
</div>
<div id="floors">
    <% foreach (string theme in Floor.Themes) { %>
    <h2><%=theme%></h2>
    <%=GenerateTable(_floors, theme, _count.Value) %>
    <br />
    <% } %>
</div>
<script type="text/javascript">
    function NavigateToCategory(int_category) {
        window.location.href = "/People/<%=_player.User%>/" + int_category;
    }
</script>
</asp:Content>