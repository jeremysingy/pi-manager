<%@ Page Title="" Language="C#" MasterPageFile="~/Professor/Professor.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PIManager.Professor.Default" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        PI Manager - Professor
    </h2>
    <p>Bienvenue dans PI-Manager. Test: <%= DateTime.Now.ToString() %></p>
  
</asp:Content>