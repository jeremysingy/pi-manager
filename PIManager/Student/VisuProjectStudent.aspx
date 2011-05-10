<%@ Page Title="Inscription to projects" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="VisuProjectStudent.aspx.cs" Inherits="PIManager.Visualization.VisuProjectStudent" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Liste des projets pour inscription/désinscription</h2>
    <asp:Label ID="errorLabel" runat="server" CssClass="error" />
    <asp:Label ID="msgLabel" runat="server" CssClass="message" />
    <asp:Table ID="ProjectTable" runat="server" CssClass="tableAlternate"></asp:Table>
    <asp:Label ID="projectDescription" runat="server" />
</asp:Content>