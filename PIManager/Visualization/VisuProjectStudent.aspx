<%@ Page Title="Inscription to projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisuProjectStudent.aspx.cs" Inherits="PIManager.Visualization.VisuProjectStudent" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Liste des projets pour inscription/désinscription</h2>
    <asp:Table ID="ProjectTable" runat="server" CssClass="tableAlternate"></asp:Table>
</asp:Content>