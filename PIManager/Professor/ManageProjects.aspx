<%@ Page Title="Modification / Suppression des projets" Language="C#" MasterPageFile="~/Professor/Professor.master" AutoEventWireup="true"
    CodeBehind="ManageProjects.aspx.cs" Inherits="PIManager.ManageProjects" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Modifier/Supprimer des projets</h2>
    <p class="error"><asp:Label ID="lbError" runat="server" Text="" /></p>
    <asp:GridView ID="ProjectsGrid" runat="server" AutoGenerateColumns="false" OnRowDeleting="onRowDeleting" OnRowDataBound="onRowDataBound">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Nom du projet" />
            <asp:BoundField DataField="NbStudents" HeaderText="Nombre d'étudiants" />
            <asp:HyperLinkField Text="Modifier" HeaderText="Modification" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ModifyProject.aspx?id={0}" />
            <asp:CommandField HeaderText="Suppression" DeleteText="Supprimer" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>