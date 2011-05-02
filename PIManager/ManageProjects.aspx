<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProjects.aspx.cs" Inherits="PIManager.ManageProjects" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="ProjectsTable" runat="server">
        </asp:Table>
        <asp:GridView ID="Test" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="NbStudents" HeaderText="Number of students" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
