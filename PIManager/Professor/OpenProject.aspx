<%@ Page Title="Ouvrir les inscriptions" Language="C#" MasterPageFile="~/Professor/Professor.master"
    AutoEventWireup="true" CodeBehind="OpenProject.aspx.cs" Inherits="PIManager.Professor.OpenProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Ouvrir les inscriptions</h2>
    <asp:GridView ID="ProjectsGrid" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Nom du projet" />
            <asp:BoundField DataField="Abreviation" HeaderText="Abréviation" />
            <asp:TemplateField HeaderText="Ouvrir">
                <ItemTemplate>
                    <asp:CheckBox ID="openProject" runat="server" 
                        AutoPostBack="true" OnCheckedChanged="onCheckboxChanged"
                        Checked="false"
                        Text="" />
                </ItemTemplate>                    
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="buttons">
        <asp:Button ID="btCancel" runat="server" Text="Annuler" CssClass="but" onclick="btCancel_Click" />
        <asp:Button ID="btSubmit" runat="server" Text="Valider" CssClass="but" onclick="btSubmit_Click" />
    </div>
</asp:Content>