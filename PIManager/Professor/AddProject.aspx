<%@ Page Title="Ajout d'un projet" Language="C#" MasterPageFile="~/Professor/Professor.master"
    AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="PIManager.Professor.AddProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Saisir un projet</h2>
    <p>Veuillez choisir la manière de créer votre projet</p>
    <asp:BulletedList ID="AddList" runat="server" DisplayMode="HyperLink" >
        <asp:ListItem Text="Créer un nouveau projet" Value="AddNewProject.aspx" />
        <asp:ListItem Text="Hériter depuis un projet existant" Value="AddInheritedProject.aspx" />
    </asp:BulletedList>
</asp:Content>