<%@ Page Title="Ouvrir les inscriptions" Language="C#" MasterPageFile="~/Professor/Professor.master"
    AutoEventWireup="true" CodeBehind="OpenProject.aspx.cs" Inherits="PIManager.Professor.OpenProject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../Calendar/js/jscal2.js" type="text/javascript"></script>
    <script src="../Calendar/js/lang/fr.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../Calendar/css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../Calendar/css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="../Calendar/css/steel/steel.css" />
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Ouvrir les inscriptions</h2>
    <asp:PlaceHolder ID="phOpened" runat="server" Visible="false">
        <p class="confirm">Les projets sélectionnés ont bien été ouverts</p>
    </asp:PlaceHolder>
    <asp:GridView ID="ProjectsGrid" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Nom du projet" />
            <asp:BoundField DataField="Abreviation" HeaderText="Abréviation" />
            <asp:TemplateField HeaderText="Ouvrir">
                <ItemTemplate>
                    <asp:CheckBox ID="openProject" runat="server" 
                        AutoPostBack="true" OnCheckedChanged="onCheckboxChanged"
                        Checked="false"
                        Text=""
                        CssClass="checkboxopen" />
                </ItemTemplate>                    
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lbErrorProjects" runat="server" Text="Un projet au moins doit être sélectionné" Visible="false" CssClass="error" />
    <br /><br />

    <asp:Label ID="lbStart" runat="server" Text="Date de début :" AssociatedControlID="tbStart" />
    <asp:TextBox ID="tbStart" runat="server" />
    <input type="button" id="btStart" value="..." class="buttoncal" />
    <asp:RequiredFieldValidator ID="RequiredStart" runat="server"
        ErrorMessage="Date de début obligatoire" 
        ControlToValidate="tbStart" 
        Display="Dynamic"
        CssClass="error">
    </asp:RequiredFieldValidator>
    <asp:CompareValidator ID="ValidateStart" runat="server" 
        ControlToValidate="tbStart"
        ErrorMessage="Date non valide"
        Operator="DataTypeCheck" Type="Date" ValidationGroup="grpDate"
        CssClass="error">
    </asp:CompareValidator>
    <br />
    <asp:Label ID="lbEnd" runat="server" Text="Date de fin :" AssociatedControlID="tbEnd" />
    <asp:TextBox ID="tbEnd" runat="server" />
    <input type="button" id="btEnd" value="..." class="buttoncal" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
        ErrorMessage="Date de fin obligatoire" 
        ControlToValidate="tbEnd" 
        Display="Dynamic"
        CssClass="error">
    </asp:RequiredFieldValidator>
    <asp:CompareValidator ID="ValidateEnd" runat="server" 
        ControlToValidate="tbEnd" ControlToCompare="tbStart" Operator="GreaterThan"
        ErrorMessage="La date doit être plus grande"
        Type="Date" ValidationGroup="grpDate"
        CssClass="error">
    </asp:CompareValidator>
    <br />
    <asp:Label ID="lbErrorDates" runat="server" Text="La date de fin doit être supérieure à la date de début" Visible="false" CssClass="error" />

    <script type="text/javascript">//<![CDATA[
        var cal = Calendar.setup({
            //inputField: "MainContent_tbEnd",
            //trigger: "btEnd",
            onSelect: function () { this.hide() },
            showTime: true,
            dateFormat: "%Y-%m-%d %I:%M"
        });

        cal.manageFields("btStart", "MainContent_tbStart", "%d.%m.%Y %H:%M");
        cal.manageFields("btEnd", "MainContent_tbEnd", "%d.%m.%Y %H:%M");
    //]]></script>

    <div class="buttons">
        <asp:Button ID="btCancel" runat="server" Text="Annuler" CssClass="but" onclick="btCancel_Click" />
        <asp:Button ID="btSubmit" runat="server" Text="Ouvrir" CssClass="but" onclick="btSubmit_Click" />
    </div>
</asp:Content>