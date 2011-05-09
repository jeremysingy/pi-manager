<%@ Page Title="" Language="C#" MasterPageFile="~/Professor/Professor.Master" AutoEventWireup="true" CodeBehind="VisualizationInscription.aspx.cs" Inherits="PIManager.Professor.VisualizationInscription" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Liste des inscriptions
    </h2>
    <asp:Label ID="errorLabel" runat="server" CssClass="error" />
    <asp:Table ID="InscriptionsTable" runat="server" CssClass="tableAlternate"></asp:Table>
  
</asp:Content>
