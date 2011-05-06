<%@ Page Title="Add document to project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="PIManager.AddDocument" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Ajouter un document</h2>
        <asp:Label runat="server" ID="errorLabel" />
        <asp:Label ID="Label1" runat="server" Text="Choisissez un document ZIP à ajouter. [max. 5MB]" />
        <asp:FileUpload ID="uploadFile" runat="server" /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Envoyer" OnClick="btnUpload_Click" />

        <asp:RegularExpressionValidator ID="extensionValidator" runat="server" 
                                        ErrorMessage="Seuls les fichiers .zip sont autorisés !"
                                        ValidationExpression="^.*\.[Zz][Ii][Pp]$"
                                        ControlToValidate="uploadFile"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ID="uploadFileRequired" runat="server"
                                    ErrorMessage="Ce champ est requis!"
                                    ControlToValidate="uploadFile"></asp:RequiredFieldValidator>
</asp:Content>

