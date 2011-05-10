<%@ Page Title="Modification d'un projet" Language="C#" MasterPageFile="~/Professor/Professor.master"
    AutoEventWireup="true" CodeBehind="ModifyProject.aspx.cs" Inherits="PIManager.ModifyProject" ValidateRequest="false" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Modification du projet :</h2>
    <h3><asp:Label ID="lbName" runat="server" Text="Titre" /></h3>
    <asp:Label ID="lbTitle" runat="server" Text="Titre :" AssociatedControlID="tbTitle" />
    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="ValidateTitle" runat="server" 
        ErrorMessage="Le titre dépasse 80 caractères" 
        ControlToValidate="tbTitle" 
        ValidationExpression="^[\w\s]{1,80}$" 
        Display="Dynamic">
    </asp:RegularExpressionValidator>
    <br />
    <asp:Label ID="lbAbreviation" runat="server" Text="Abréviation :" AssociatedControlID="tbAbreviation" />
    <asp:TextBox ID="tbAbreviation" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="ValidateAbreviation" runat="server" 
        ErrorMessage="L'abréviation dépasse 50 caractères" 
        ControlToValidate="tbAbreviation" 
        ValidationExpression="^[\w\s]{1,50}$" 
        Display="Dynamic">
    </asp:RegularExpressionValidator>
    <br />
    <asp:Label ID="lbDescription" runat="server" Text="Description :" AssociatedControlID="tbDescription" />
    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
    <br /><br />
    <asp:Label ID="lbTechnologies" runat="server" Text="Technologies :" AssociatedControlID="listTechnologies" />
    <asp:GridView ID="gridTechnologies" runat="server" AutoGenerateColumns="False" OnRowDeleting="onRowDeleting">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Technologie" />
            <asp:CommandField DeleteText="Supprimer" ShowDeleteButton="true" HeaderText="Supprimer" />
        </Columns>
    </asp:GridView>
    <br />
    <label>&nbsp;</label>
    <asp:DropDownList ID="listTechnologies" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
    <asp:Button ID="btAddTechno" runat="server" Text="Ajouter" CssClass="but" 
        onclick="btAddTechno_Click" CausesValidation="false" />
    <br /><br />
    <asp:Label ID="lbNbStudents" runat="server" Text="Nombre d'étudiants :" AssociatedControlID="tbNbStudents" />
    <asp:TextBox ID="tbNbStudents" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="ValidateNbStudents" runat="server" 
        ErrorMessage="Le nombre d'étudiants n'est pas valide" 
        ControlToValidate="tbNbStudents" 
        ValidationExpression="^\d+$" 
        Display="Dynamic">
    </asp:RegularExpressionValidator>
    <br />
    <asp:Label ID="lbClients" runat="server" Text="Client :" AssociatedControlID="listClients" />
    <asp:DropDownList ID="listClients" runat="server" DataValueField="Id" DataTextField="CompleteName"></asp:DropDownList>
    <br />
    <div class="center">
        <asp:Button ID="btCancel" runat="server" Text="Annuler" CssClass="but" onclick="btCancel_Click" />
        <asp:Button ID="btSubmit" runat="server" Text="Valider" CssClass="but" onclick="btSubmit_Click" />
    </div>
</asp:Content>
