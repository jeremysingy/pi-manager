<%@ Page Title="Add document to project" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="PIManager.AddDocument" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Ajouter un document</h2>
        <asp:Label runat="server" ID="errorLabel" CssClass="error" />
        <asp:Label runat="server" ID="msgLabel" />
        <asp:RegularExpressionValidator ID="extensionValidator" runat="server" CssClass="error"
                                        ErrorMessage="Seuls les fichiers .zip sont autorisés !"
                                        ValidationExpression="^.*\.[Zz][Ii][Pp]$"
                                        ControlToValidate="uploadFile" />
        <asp:RequiredFieldValidator ID="uploadFileRequired" runat="server" CssClass="error"
                                    ErrorMessage="Ce champ est requis!"
                                    ControlToValidate="uploadFile" /><br />
        <asp:Label ID="Label1" runat="server" Text="Ajouter un document ZIP [max. 5MB]: " />
        <asp:FileUpload ID="uploadFile" runat="server" /><br /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Envoyer" OnClick="btnUpload_Click" />

        
</asp:Content>

