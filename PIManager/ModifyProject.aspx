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
    <h3><asp:Label ID="lbName" runat="server" Text="Titre" /></h3>
    <asp:Label ID="lbTitle" runat="server" Text="Titre :" AssociatedControlID="tbTitle" />
    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lbDescription" runat="server" Text="Description :" AssociatedControlID="tbDescription" />
    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
    <br />
    <asp:Label ID="lbTechnologies" runat="server" Text="Technologies :" AssociatedControlID="listTechnologies" />
    <asp:DropDownList ID="listTechnologies" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
    <br />
    <label>&nbsp;</label>
    <asp:GridView ID="gridTechnologies" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataFormatString="Haha" HeaderText="header" />
            <asp:BoundField DataFormatString="Supprimer" HeaderText="header2" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Label ID="lbNbStudents" runat="server" Text="Nombre d'étudiants :" AssociatedControlID="tbNbStudents" />
    <asp:TextBox ID="tbNbStudents" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lbClients" runat="server" Text="Client :" AssociatedControlID="listClients" />
    <asp:DropDownList ID="listClients" runat="server" DataValueField="Id" DataTextField="CompleteName"></asp:DropDownList>
    <br />
    <div class="center">
        <asp:Button ID="btCancel" runat="server" Text="Annuler" CssClass="but" />
        <asp:Button ID="btSubmit" runat="server" Text="Valider" CssClass="but" onclick="btSubmit_Click" />
    </div>
</asp:Content>
