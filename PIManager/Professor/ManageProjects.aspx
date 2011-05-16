<%@ Page Title="Modification / Suppression des projets" Language="C#" MasterPageFile="~/Professor/Professor.master" AutoEventWireup="true"
    CodeBehind="ManageProjects.aspx.cs" Inherits="PIManager.ManageProjects" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Modifier/Supprimer des projets</h2>
    <asp:PlaceHolder ID="phAdded" runat="server" Visible="false">
        <p class="confirm">Le projet a bien été ajouté.</p>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phModified" runat="server" Visible="false">
        <p class="confirm">Le projet a bien été modifié.</p>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phDeleted" runat="server" Visible="false">
        <p class="confirm">La projet a bien été supprimé.</p>
    </asp:PlaceHolder>
    <asp:GridView ID="ProjectsGrid" runat="server" AutoGenerateColumns="false" OnRowDeleting="onRowDeleting" OnRowDataBound="onRowDataBound">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Nom du projet" />
            <asp:BoundField DataField="Abreviation" HeaderText="Abréviation" />
            <asp:BoundField DataField="NbStudents" HeaderText="Nbre étud." />
            <asp:HyperLinkField Text="Modifier" HeaderText="Modification" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ModifyProject.aspx?id={0}" />
            <asp:CommandField HeaderText="Suppression" DeleteText="Supprimer" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>