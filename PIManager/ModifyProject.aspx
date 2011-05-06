<%@ Page Title="Modification d'un projet" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ModifyProject.aspx.cs" Inherits="PIManager.ModifyProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<style type="text/css">
label {
	display: inline;
	float: left;
	width: 180px;
}

input {
	width: 300px;
	margin: 2px;
}

textarea {
	width: 300px;
	height: 150px;
}

.but 
{
    width: 80px;
}

.center 
{
    text-align: center;
}
</style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Modification du projet :</h2>
    <h3>xxx</h3>
    <label for="Title">Titre :</label>
    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
    <br />
    <label for="description">Description :</label>
    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
    <br />
    <label for="newtechno">Technologies :</label>
    <asp:DropDownList ID="listTechnologies" runat="server"></asp:DropDownList>
    <br />
    <label>&nbsp;</label>
    <asp:GridView ID="gridTechnologies" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataFormatString="Haha" HeaderText="header" />
            <asp:BoundField DataFormatString="Supprimer" HeaderText="header2" />
        </Columns>
    </asp:GridView>
    <br />
    <label for="nbstudents">Nombre d'étudiants :</label>
    <asp:TextBox ID="tbNbStudents" runat="server"></asp:TextBox>
    <br />
    <label for="client">Client :</label>
    <asp:DropDownList ID="listClients" runat="server"></asp:DropDownList>
    <br />
    <div class="center">
        <asp:Button ID="btCancel" runat="server" Text="Annuler" CssClass="but" />
        <asp:Button ID="btSubmit" runat="server" Text="Valider" CssClass="but" />
    </div>
</asp:Content>
