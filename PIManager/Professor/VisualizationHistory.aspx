<%@ Page Title="" Language="C#" MasterPageFile="~/Professor/Professor.Master" AutoEventWireup="true" CodeBehind="VisualizationHistory.aspx.cs" Inherits="PIManager.Professor.VisualizationHistory" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h2>
        Liste des projets
    </h2>
    <asp:Label ID="historyMsg" runat="server" CssClass="message" />
    <br />
    <br />
    <a href="/Professor/VisualizationGroup.aspx">Retour</a>

</asp:Content>
