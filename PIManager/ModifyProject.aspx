<%@ Page Title="Modification d'un projet" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ModifyProject.aspx.cs" Inherits="PIManager.ModifyProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
.formLayout
{
    background-color: #f3f3f3;
    border: solid 1px #a1a1a1;
    padding: 10px;
    width: 600px;
}

.formLayout label, .formLayout input
{
    display: block;
    width: 120px;
    float: left;
    margin-bottom: 10px;
}

.formLayout label
{
    text-align: right;
    padding-right: 20px;
}
</style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Modification du projet :</h2>
    <h3>xxx</h3>

    <div class="formLayout">
        <label>Titre :</label> <input name="title" type="text" value="Projet 1" />
        <label>Description :</label> <textarea name="description" cols="25" rows="4">blablablablablablabla</textarea>
        <label>Technologies :</label>
        <select name="technoSelect">
            <option label="tech" value="1" selected="selected">Visual Studio 2010</option>
            <option label="tech" value="2">Java EE</option>
            <option label="tech" value="3">MySQL</option>
            <option label="tech" value="4">SQL Server</option>
        </select>
        <br />
        <input name="addTechno" type="button" value="Ajouter" />
        <select name="techsChoisies" multiple="multiple" size="3">
            <option label="tech" value="1" selected="selected">Visual Studio 2010</option>
            <option label="tech" value="2">Java EE</option>
        </select>
        <input name="supp" type="button" value="Supprimer" />
        <label>Nbr étudiants :</label> <input type="text" name="nbretu" value="3" />
        <label>Client :</label>
        <br />
        <select name="client">
            <option label="client" value="1" selected="selected">Jean XX</option>
            <option label="client" value="2">Blabla YY</option>
        </select>

        <input type="submit" value="Annuler" />
        <input type="submit" value="Valider" />
    </div>
</asp:Content>
