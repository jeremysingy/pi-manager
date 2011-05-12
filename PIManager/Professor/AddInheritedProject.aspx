<%@ Page Title="Saisie d'un projet hérité" Language="C#" MasterPageFile="~/Professor/Professor.master"
    AutoEventWireup="true" CodeBehind="AddInheritedProject.aspx.cs" Inherits="PIManager.Professor.AddInheritedProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Choisir le projet duquel hériter</h2>
    <asp:GridView ID="ProjectsGrid" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Nom du projet" />
            <asp:BoundField DataField="Abreviation" HeaderText="Abréviation" />
            <asp:HyperLinkField Text="Choisir" HeaderText="Héritage" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="AddNewProject.aspx?parent={0}" />
        </Columns>
    </asp:GridView>
</asp:Content>