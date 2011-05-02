<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="PIManager._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        PI Manager
    </h2>
    <p>Bienvenue dans PI-Manager. Test: <%= DateTime.Now.ToString() %></p>
    <p>TODO: login</p>
</asp:Content>
