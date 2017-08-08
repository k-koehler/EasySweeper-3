<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="EasyWeb.People" Async="true" MasterPageFile="~/Master.Master"%>
<% @ Import Namespace="EasyAPI" %>
<% @ Import Namespace="EasyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<!DOCTYPE html>

<script type="text/javascript" src="/Scripts/jquery-3.2.1.js"></script>
<link rel="stylesheet" type="text/css" href="/CSS/People.css"/>

<div id="playerProfile"></div>
<h1>Floors</h1>
<div id="categoryControls"></div>
<div id="floors">
    <h1>Frozen</h1>
    <%=GenerateTable(_floors, "Frozen", _count.Value) %>
    <br />
</div>

</asp:Content>
